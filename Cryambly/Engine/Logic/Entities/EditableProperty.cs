using System;
using System.Linq;
using System.Reflection;

namespace CryCil.Engine.Logic
{
	internal delegate object Parser(string value);
	/// <summary>
	/// Encapsulates description of the editable entity property.
	/// </summary>
	public struct EditableProperty
	{
		#region Fields
		private static readonly Parser[] parsers =
		{
			ParseBool,
			ParseInt32,
			ParseSingle,
			ParseVector,
			ParseString,
			ParseEntity
		};
		private static readonly Type[] supportedTypes =
		{
			typeof(bool),
			typeof(int),
			typeof(float),
			typeof(Vector3),
			typeof(string),
			typeof(EntityId)
		};
		private readonly MemberInfo member;
		private readonly EditablePropertyType type;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the underlying object of this property.
		/// </summary>
		public MemberInfo Member
		{
			get { return this.member; }
		}
		/// <summary>
		/// Gets the type of the property.
		/// </summary>
		public EditablePropertyType Type
		{
			get { return this.type; }
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="member">Object that represents underlying field of property object.</param>
		public EditableProperty(MemberInfo member) : this()
		{
			this.member = member;
			Type type = member.GetAssociatedType();
			int typeIndex = Array.IndexOf(supportedTypes, type);
			if (typeIndex == -1)
			{
				throw new NotSupportedException
				(
					string.Format
					(
						"Attempt was made to register an editable property with type {0} that is not supported. Supported types are: {1}",
						type.FullName,
						supportedTypes.Select(supportedType => supportedType.FullName).ContentsToString(", ")
					)
				);
			}
			this.type = (EditablePropertyType)typeIndex;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the value of the editable property.
		/// </summary>
		/// <param name="entity">Entity which editable property value to get from.</param>
		/// <returns>Text representation of the value.</returns>
		public string Get(object entity)
		{
			if (entity == null)
			{
				return null;
			}

			FieldInfo field = this.Member as FieldInfo;
			if (field != null)
			{
				return field.GetValue(entity).ToString();
			}
			PropertyInfo propertyInfo = this.Member as PropertyInfo;
			return
				propertyInfo != null
				? propertyInfo.GetValue(entity).ToString()
				: null;
		}
		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="entity">Entity which editable property value to set.</param>
		/// <param name="value">Text representation of the new value.</param>
		public void Set(object entity, string value)
		{
			if (entity == null)
			{
				return;
			}

			object v = this.Parse(value);

			FieldInfo field = this.Member as FieldInfo;
			if (field != null)
			{
				field.SetValue(entity, v);
			}
			PropertyInfo propertyInfo = this.Member as PropertyInfo;
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(entity, v);
			}
		}
		#endregion
		#region Utilities
		private object Parse(string value)
		{
			return parsers[(int)this.Type](value);
		}
		private static object ParseBool(string value)
		{
			switch (value)
			{
				case "0":
					value = "false";
					break;
				case "1":
					value = "true";
					break;
			}
			return Convert.ToBoolean(value);
		}
		private static object ParseInt32(string value)
		{
			return Convert.ToInt32(value);
		}
		private static object ParseSingle(string value)
		{
			return Convert.ToSingle(value);
		}
		private static object ParseVector(string value)
		{
			return Vector3.Parse(value);
		}
		private static object ParseString(string value)
		{
			return value;
		}
		private static object ParseEntity(string value)
		{
			return Convert.ToUInt32(value);
		}
		#endregion
	}
}