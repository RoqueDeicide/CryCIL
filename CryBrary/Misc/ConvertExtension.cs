using System;
using CryEngine.Entities;

namespace CryEngine
{
	/// <summary>
	/// Defines method that converts value of editor property to a respective type.
	/// </summary>
	public static class ConvertExtension
	{
		/// <summary>
		/// Converts value of editor property to a respective type.
		/// </summary>
		/// <param name="type">Type of the property.</param>
		/// <param name="value">Value of the property.</param>
		/// <returns>Value parsed as object of appropriate type.</returns>
		public static object FromString(EditorPropertyType type, string value)
		{
			if (type == EditorPropertyType.String)
				return value;
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (value == null)
				throw new ArgumentNullException("value");
			if (value.Length < 1)
				throw new ArgumentException("value string was empty");
#endif

			switch (type)
			{
				case EditorPropertyType.Bool:
					return value == "1";
				case EditorPropertyType.Int:
					return int.Parse(value);
				case EditorPropertyType.Float:
					return float.Parse(value);
				case EditorPropertyType.Vector3:
					return Vector3.Parse(value);
			}

			return null;
		}
	}
}