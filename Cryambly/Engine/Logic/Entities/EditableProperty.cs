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
		public MemberInfo Member => this.member;
		/// <summary>
		/// Gets the type of the property.
		/// </summary>
		public EditablePropertyType Type => this.type;
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="member">Object that represents underlying field of property object.</param>
		/// <exception cref="NotSupportedException">Type of editable property is not supported.</exception>
		public EditableProperty(MemberInfo member) : this()
		{
			this.member = member;
			Type type = member.GetAssociatedType();
			int typeIndex = Array.IndexOf(supportedTypes, type);
			if (typeIndex == -1)
			{
				string message =
					$"Attempt was made to register an editable property with type {type.FullName} that is not supported. Supported types are: {supportedTypes.Select(supportedType => supportedType.FullName).ContentsToString(", ")}";
				throw new NotSupportedException(message);
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
		/// <exception cref="TargetException">
		/// The field is non-static and <paramref name="entity"/> is null.
		/// </exception>
		/// <exception cref="FieldAccessException">
		/// The caller does not have permission to access this field.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// A field is marked literal, but the field does not have one of the accepted literal types.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The method is neither declared nor inherited by the class of <paramref name="entity"/>.
		/// </exception>
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
			return propertyInfo?.GetValue(entity).ToString();
		}
		/// <summary>
		/// Sets the value of the property.
		/// </summary>
		/// <param name="entity">Entity which editable property value to set.</param>
		/// <param name="value"> Text representation of the new value.</param>
		/// <exception cref="FieldAccessException">
		/// The caller does not have permission to access this field.
		/// </exception>
		/// <exception cref="TargetException">
		/// The <paramref name="entity"/> parameter is null and the field is an instance field.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The field does not exist on the object.-or- The <paramref name="value"/> parameter cannot be
		/// converted and stored in the field.
		/// </exception>
		public void Set(object entity, string value)
		{
			if (entity == null)
			{
				return;
			}

			object v = this.Parse(value);

			FieldInfo field = this.Member as FieldInfo;
			field?.SetValue(entity, v);

			PropertyInfo propertyInfo = this.Member as PropertyInfo;
			propertyInfo?.SetValue(entity, v);
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
		/// <exception cref="OverflowException">
		/// <paramref name="value"/> represents a number that is less than
		/// <see cref="F:System.Int32.MinValue"/> or greater than <see cref="F:System.Int32.MaxValue"/>.
		/// </exception>
		private static object ParseInt32(string value)
		{
			return Convert.ToInt32(value);
		}
		/// <exception cref="FormatException">
		/// <paramref name="value"/> is not a number in a valid format.
		/// </exception>
		/// <exception cref="OverflowException">
		/// <paramref name="value"/> represents a number that is less than
		/// <see cref="F:System.Single.MinValue"/> or greater than <see cref="F:System.Single.MaxValue"/>.
		/// </exception>
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
		/// <exception cref="FormatException">
		/// <paramref name="value"/> does not consist of an optional sign followed by a sequence of digits
		/// (0 through 9).
		/// </exception>
		/// <exception cref="OverflowException">
		/// <paramref name="value"/> represents a number that is less than
		/// <see cref="F:System.UInt32.MinValue"/> or greater than <see cref="F:System.UInt32.MaxValue"/>.
		/// </exception>
		private static object ParseEntity(string value)
		{
			return Convert.ToUInt32(value);
		}
		#endregion
	}
}