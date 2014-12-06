using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CryEngine.Entities;
using CryEngine.Extensions;
using CryEngine.Initialization;

namespace CryEngine.RunTime.Registration
{
	/// <summary>
	/// Defines means of registering entities in the CryEngine.
	/// </summary>
	public class EntityRegister
	{
		/// <summary>
		/// A list of registered entity types, allows to determine whether a particular
		/// entity is managed by CryMono.
		/// </summary>
		public static SortedList<string, Type> Types = new SortedList<string, Type>();
		/// <summary>
		/// Registers the entity class.
		/// </summary>
		/// <param name="type">Type of the entity.</param>
		/// <exception cref="RegistrationException">
		/// Unable to register type that is not marked with Entity attribute.
		/// </exception>
		/// <exception cref="EntityException">
		/// One of the properties had its type specified as a file, but it was not a
		/// string.
		/// </exception>
		/// <exception cref="EntityException">
		/// One of the properties had its type specified as a vector, but it was not a
		/// vector type.
		/// </exception>
		/// <exception cref="EntityException">
		/// One of the properties was registered with invalid type.
		/// </exception>
		public static void Register(Type type)
		{
			EntityAttribute attribute;
			if (!type.TryGetAttribute(out  attribute))
			{
#if DEBUG
				throw new RegistrationException("Unable to register type that is not marked with Entity attribute.");
#else
				return;
#endif
			}
			// Register class.
			EntityRegister.Types.Add(attribute.Name, type);
			Native.EntityInterop.RegisterEntityClass
			(
				new EntityRegistrationParams
				{
					name = attribute.Name,
					category = attribute.Category,
					flags = attribute.Flags,
					editorHelper = attribute.EditorHelper,
					editorIcon = attribute.Icon,
					properties = EntityRegister.GetProperties(type)
				}
			);
		}
		private static object[] GetProperties(IReflect type)
		{
			SortedList<string, List<EditorProperty>> folders = new SortedList<string, List<EditorProperty>>();
			// Get all public and not so public fields and properties with EditorProperty
			// attribute.
			var members =
				type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
					.Where(member => member.ContainsAttribute<EditorPropertyAttribute>());
			foreach (MemberInfo member in members)
			{
				EditorPropertyAttribute attribute = member.GetAttribute<EditorPropertyAttribute>();
				// Validate the type.
				EditorPropertyType propType = Entity.GetEditorType(member.GetAssociatedType(), attribute.Type);
				// Use default folder, if it is not specified.
				string folderName = attribute.Folder ?? "Default";
				// Register new folder, if there is a need for that.
				if (!folders.ContainsKey(folderName))
				{
					folders.Add(folderName, new List<EditorProperty>());
				}
				// Add the property to the folder.
				folders[folderName].Add
				(
					new EditorProperty
					(
						attribute.Name ?? member.Name,
						attribute.Description,
						attribute.DefaultValue ?? new Func<string>
						(
							delegate
							{
								switch (propType)
								{
									case EditorPropertyType.Bool:
										return "false";
									case EditorPropertyType.Int:
									case EditorPropertyType.Float:
										return "0";
									case EditorPropertyType.Vector3:
									case EditorPropertyType.Color:
										return "0,0,0";
									default:
										return "";
								}
							}
						).Invoke(),
						propType,
						new EditorPropertyLimits
						{
							Min = attribute.Min,
							Max = attribute.Max
						},
						attribute.Flags
					)
				);
			}
			// Now lets create the array of properties.
			List<object> properties = new List<object>();
			foreach (KeyValuePair<string, List<EditorProperty>> folder in folders)
			{
				if (folder.Key == "Default")
				{
					properties.AddRange(folder.Value.Cast<object>());
				}
				else
				{
					properties.Add(new EditorProperty(folder.Key, "", "", EditorPropertyType.FolderBegin));
					properties.AddRange(folder.Value.Cast<object>());
					properties.Add(new EditorProperty(folder.Key, "", "", EditorPropertyType.FolderEnd));
				}
			}
			return properties.ToArray();
		}
	}
}