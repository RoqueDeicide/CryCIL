using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CryCil.Engine.Logic;

namespace CryCil.RunTime.Registration
{
	/// <summary>
	/// Encapsulates description of one of the types of editable properties.
	/// </summary>
	public struct EditablePropertyTypeDesc
	{
		/// <summary>
		/// Text prefix that is used by the editor to determine the type of the property.
		/// </summary>
		public string Prefix;
		/// <summary>
		/// Object of class <see cref="Type"/> that has to be used by the editable property that is of this
		/// type.
		/// </summary>
		public Type ManagedType;
		/// <summary>
		/// Description of this type.
		/// </summary>
		public string Description;
		/// <summary>
		/// Initializes new instance of this type.
		/// </summary>
		/// <param name="prefix"><see cref="Prefix"/></param>
		/// <param name="type">  <see cref="ManagedType"/></param>
		/// <param name="desc">  <see cref="Description"/></param>
		public EditablePropertyTypeDesc(string prefix, Type type, string desc)
		{
			this.Prefix = prefix;
			this.ManagedType = type;
			this.Description = desc;
		}
	}
	/// <summary>
	/// Defines functions that register new types of entities.
	/// </summary>
	[InitializationClass]
	public static class EntityRegistry
	{
		#region Fields
		/// <summary>
		/// An array which defines string prefixes and type objects for editable properties.
		/// </summary>
		public static readonly EditablePropertyTypeDesc[] EditablePropertyUiMap =
		{
			new EditablePropertyTypeDesc("n", typeof(int), "Simple integer number."),
			new EditablePropertyTypeDesc("b", typeof(bool), "Simple Boolean value."),
			new EditablePropertyTypeDesc("f", typeof(float), "Simple single-precision floating point number."),
			new EditablePropertyTypeDesc("s", typeof(string), "Plain text."),
			new EditablePropertyTypeDesc("shader", typeof(string), "Name of the shader."),
			new EditablePropertyTypeDesc("clr", typeof(Vector3), "Simple color without alpha component."),
			new EditablePropertyTypeDesc("vector", typeof(Vector3), "Simple 3D vector."),
			new EditablePropertyTypeDesc("snd", typeof(string), "Path to the sound file."),
			new EditablePropertyTypeDesc("dialog", typeof(string), "Identifier of the dialog."),
			new EditablePropertyTypeDesc("tex", typeof(string), "Path to the texture file."),
			new EditablePropertyTypeDesc("obj", typeof(string), "Path to the object(?)."),
			new EditablePropertyTypeDesc("file", typeof(string), "Path to the file."),
			new EditablePropertyTypeDesc("text", typeof(string), "Simple text."),
			new EditablePropertyTypeDesc("equip", typeof(string), "Name of the equipment pack."),
			new EditablePropertyTypeDesc("reverbpreset", typeof(string), "Name of the reverberation preset."),
			new EditablePropertyTypeDesc("eaxpreset", typeof(string), "Name of the EAX preset."),
			new EditablePropertyTypeDesc("gametoken", typeof(string), "Name of the game token."),
			new EditablePropertyTypeDesc("seq_", typeof(string), "Name of the TrackView sequence."),
			new EditablePropertyTypeDesc("mission_", typeof(string), "Name of the mission.")
		};
		/// <summary>
		/// Maps names of entity types to arrays of editable properties that are defined for them.
		/// </summary>
		public static readonly SortedList<string, EditableProperty[]> DefinedProperties =
			new SortedList<string, EditableProperty[]>();
		/// <summary>
		/// Maps names of entity classes to their managed reflection counter-parts.
		/// </summary>
		public static readonly SortedList<string, Type> DefinedEntityClasses =
			new SortedList<string, Type>();
		#endregion
		#region Interface
		[InitializationStage((int)DefaultInitializationStages.EntityRegistrationStage)]
		private static void RegisterEntities(int index)
		{
			var entityTypes =
				from assembly in MonoInterface.CryCilAssemblies
				from type in assembly.GetTypes()
				where type.Implements<MonoEntity>() && type.ContainsAttribute<EntityAttribute>()
				select type;

			foreach (Type entityType in entityTypes)
			{
				EntityAttribute attribute = entityType.GetAttribute<EntityAttribute>();
				if (attribute.Inherit)
				{
					// Inherit information.
					attribute = attribute.InheritFrom(entityType);
				}

				string entityClassName = attribute.Name ?? entityType.Name;

				if (DefinedEntityClasses.ContainsKey(entityClassName))
				{
					if (attribute.Flags.HasFlag(EntityClassFlags.ModifyExisting))
					{
						DefinedEntityClasses[entityClassName] = entityType;
					}
					else
					{
						throw new Exception($"A class named {entityClassName} has already been registered.");
					}
				}
				else
				{
					DefinedEntityClasses.Add(entityClassName, entityType);
				}

				EntitySystem.RegisterEntityClass(entityClassName,
												 attribute.Category,
												 attribute.EditorHelper,
												 attribute.EditorIcon,
												 attribute.Flags,
												 GetEditableProperties(entityType),
												 entityType.Implements<MonoNetEntity>(),
												 attribute.DontSyncEditableProperties);
			}
		}
		#endregion
		#region Utilities
		private static EditablePropertyInfo[] GetEditableProperties(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type), "Entity type cannot be null.");
			}
			if (DefinedProperties.ContainsKey(type.Name))
			{
				throw new Exception($"A type with a name {type.Name} had its editable properties already defined.");
			}

			var folders = new SortedList<string, List<EditableProperty>>();

			// Get an array of fields and properties that are supposed to be editable.
			var editableMembers =
				from info in type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				where info.ContainsAttribute<EditablePropertyAttribute>()
				select info;

			foreach (MemberInfo editableMember in editableMembers)
			{
				var attr = editableMember.GetAttribute<EditablePropertyAttribute>();
				// Group into folders.
				if (!folders.ContainsKey(attr.Folder))
				{
					folders.Add(attr.Folder, new List<EditableProperty>());
				}
				folders[attr.Folder].Add(new EditableProperty(editableMember));
			}
			// Sort contents of all folders.
			Comparison<EditableProperty> sorter = (property0, property1) =>
			{
				var attribute0 = property0.Member.GetAttribute<EditablePropertyAttribute>();
				var attribute1 = property1.Member.GetAttribute<EditablePropertyAttribute>();

				int res = attribute0.SortHelper.CompareTo(attribute1.SortHelper);
				return res != 0
					? res
					: string.Compare(property0.Member.Name, property1.Member.Name, StringComparison.Ordinal);
			};
			foreach (string folderName in folders.Keys)
			{
				folders[folderName].Sort(sorter);
			}

			// Organize all properties into an array, so that all props can be accessed via index.
			List<EditableProperty> propList = new List<EditableProperty>(); // These are used in managed code.
			List<EditablePropertyInfo> propInfoList = new List<EditablePropertyInfo>(); // These are passed to native code.

			foreach (var folder in folders)
			{
				if (folder.Key == "")
				{
					propList.AddRange(folder.Value);
					propInfoList.AddRange(folder.Value.Select(prop => new EditablePropertyInfo(prop)));
				}
				else
				{
					propList.Add(new EditableProperty());
					// Not gonna be used, but has to take up the slot to insure consistency of indices.
					propList.AddRange(folder.Value);
					propList.Add(new EditableProperty()); // Same as one above.

					propInfoList.Add(new EditablePropertyInfo(folder.Key, true));
					propInfoList.AddRange(folder.Value.Select(prop => new EditablePropertyInfo(prop)));
					propInfoList.Add(new EditablePropertyInfo(folder.Key, false));
				}
			}

			DefinedProperties.Add(type.Name, propList.ToArray());

			return propInfoList.ToArray();
		}
		#endregion
	}
}