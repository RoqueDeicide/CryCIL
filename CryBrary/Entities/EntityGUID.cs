using System;

namespace CryEngine.Entities
{
	/// <summary>
	/// Encapsulates unique identifer of the entity.
	/// </summary>
	public struct EntityGuid
	{
		internal UInt64 _value;

		[System.CLSCompliant(false)]
		public EntityGuid(UInt64 id)
		{
			this._value = id;
		}
		#region Overrides
		/// <summary>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is EntityGuid)
				return (EntityGuid)obj == this;
			if (obj is UInt64)
				return (UInt64)obj == this._value;
			if (obj is uint)
				return (uint)obj == this._value;

			return false;
		}
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			return this._value.GetHashCode();
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this._value.ToString();
		}
		#endregion
		#region Operators
		/// <summary>
		/// </summary>
		/// <param name="entId1"></param>
		/// <param name="entId2"></param>
		/// <returns></returns>
		public static bool operator ==(EntityGuid entId1, EntityGuid entId2)
		{
			return entId1._value == entId2._value;
		}
		/// <summary>
		/// </summary>
		/// <param name="entId1"></param>
		/// <param name="entId2"></param>
		/// <returns></returns>
		public static bool operator !=(EntityGuid entId1, EntityGuid entId2)
		{
			return entId1._value != entId2._value;
		}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[System.CLSCompliant(false)]
		public static implicit operator EntityGuid(UInt64 value)
		{
			return new EntityGuid(value);
		}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator EntityGuid(Int64 value)
		{
			return new EntityGuid((UInt64)value);
		}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator Int64(EntityGuid value)
		{
			return System.Convert.ToInt64(value._value);
		}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[System.CLSCompliant(false)]
		public static implicit operator UInt64(EntityGuid value)
		{
			return System.Convert.ToUInt64(value._value);
		}
		#endregion
	}
}