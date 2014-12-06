using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CryEngine.Extensions;

namespace CryEngine.RunTime.Compatibility
{
	/// <summary>
	/// Represents an object that checks blittability of managed types and their unmanaged
	/// counter-parts.
	/// </summary>
	public class BlitChecker
	{
		#region Fields
		private static SortedList<string, CppType> unmanagedTypes;
		#endregion
		#region Construction
		static BlitChecker()
		{
			BlitChecker.unmanagedTypes = new SortedList<string, CppType>();
			if (!File.Exists("NativeMetaData.xml"))
			{
				return;
			}
			XmlDocument document = new XmlDocument();
			document.Load("NativeMetaData.xml");

			XmlElement[] typeElements =
				document.GetElementsByTagName("CppType").OfType<XmlElement>().ToArray();
			// Load the type information.
			for (int i = 0; i < typeElements.Length; i++)
			{
				// Load the name of the type.
				string typeName = typeElements[i].Name;
				XmlElement[] fieldElements =
					typeElements[i].GetElementsByTagName("Field").OfType<XmlElement>().ToArray();
				CppType nativeType = new CppType
				{
					// Load the size of objects of this type.
					Size = Convert.ToInt32(typeElements[i].GetAttribute("Size")),
					Fields = new List<Tuple<string, Type>>()
				};
				for (int j = 0; j < fieldElements.Length; j++)
				{
					// Load field metadata of the type.
					nativeType.Fields.Add
					(
						new Tuple<string, Type>
						(
							fieldElements[j].GetAttribute("Name"),
							Type.GetType(fieldElements[j].GetAttribute("TypeName"))
						)
					);
				}
				BlitChecker.unmanagedTypes.Add(typeName, nativeType);
			}
		}
		#endregion
		#region Interface
		/// <summary>
		/// Checks whether given type is blittable to its unmanaged counter-part.
		/// </summary>
		/// <param name="type">Type to check.</param>
		public static void Check(Type type)
		{
			BlittableAttribute attribute = type.GetAttribute<BlittableAttribute>();
			if (attribute == null)
			{
				return;
			}
			CppType cppType;
			// Do we have information about this type?
			if (!BlitChecker.unmanagedTypes.TryGetValue(attribute.BlitsTo, out cppType))
			{
				// Maybe it was removed entirely or renamed.
				Debug.LogError
				(
					"Unable to find unmanaged type {0} {1} is supposed to be blittable to.",
					attribute.BlitsTo, type.FullName
				);
				return;
			}
			// Check the size.
			if (attribute.CompatibilityFlags.HasFlag(CompatibilityFlags.MatchSize)
				&&
				Marshal.SizeOf(type) != cppType.Size)
			{
				Debug.LogError
				(
					"Size of objects of type {0} is not equal to the size of {1}.",
					type.FullName, attribute.BlitsTo
				);
			}
			// Do we need to check the fields?
			if (((int)attribute.CompatibilityFlags & 6) == 0)
			{
				// No, we don't.
				return;
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic);
			if (fields.Length != cppType.Fields.Count)
			{
				Debug.LogError
				(
					"Number of fields in {0} is not equal to number of fields in {1}.",
					type.FullName, attribute.BlitsTo
				);
			}
			for (int i = 0; i < fields.Length; i++)
			{
				NativeFieldNameAttribute fieldNameAttribute;
				string mappedName =
					fields[i].TryGetAttribute(out fieldNameAttribute)
					? fieldNameAttribute.UnmanagedName
					: fields[i].Name;
				// Are the field names matching?
				if (attribute.CompatibilityFlags.HasFlag(CompatibilityFlags.MatchFieldNameMapping) &&
					mappedName != cppType.Fields[i].Item1)
				{
					Debug.LogError
					(
						"The field {0} of type {1} is supposed to be mapped" +
						" to the field {2} of type {3}, however the name of" +
						" corresponding field in unmanaged type is {4}.",
						fields[i].Name, type.FullName, mappedName,
						attribute.BlitsTo, cppType.Fields[i].Item1
					);
				}
				// Are the field types matching?
				if (attribute.CompatibilityFlags.HasFlag(CompatibilityFlags.MatchFieldTypes) &&
					fields[i].FieldType != cppType.Fields[i].Item2)
				{
					Debug.LogError
					(
						"Type of the field {0} of type {1} doesn't match the" +
						" type of the field {2} of type {3}.",
						fields[i].Name, type.FullName, mappedName, attribute.BlitsTo
					);
				}
			}
		}
		#endregion
		#region Utilities
		#endregion
	}
}