using System;

namespace CryEngine.Entities
{
	/// <summary>
	/// Entity ID's store references to game entities as unsigned integers. This class wraps that
	/// functionality for CLS compliance.
	/// </summary>
	public struct EntityId
	{
		internal uint Value;
		/// <summary></summary>
		/// <param name="id"></param>
		[CLSCompliant(false)]
		public EntityId(uint id)
		{
			this.Value = id;
		}
		#region Overrides
		/// <summary></summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is EntityId)
				return (EntityId)obj == this;
			if (obj is int)
				return (int)obj == this.Value;
			if (obj is uint)
				return (uint)obj == this.Value;

			return false;
		}
		/// <summary></summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			return this.Value.GetHashCode();
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}
		/// <summary></summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Value.ToString();
		}
		#endregion
		#region Operators
		/// <summary></summary>
		/// <param name="entId1"></param>
		/// <param name="entId2"></param>
		/// <returns></returns>
		public static bool operator ==(EntityId entId1, EntityId entId2)
		{
			return entId1.Value == entId2.Value;
		}
		/// <summary></summary>
		/// <param name="entId1"></param>
		/// <param name="entId2"></param>
		/// <returns></returns>
		public static bool operator !=(EntityId entId1, EntityId entId2)
		{
			return entId1.Value != entId2.Value;
		}
		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public static implicit operator EntityId(uint value)
		{
			return new EntityId(value);
		}
		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator EntityId(int value)
		{
			return new EntityId((uint)value);
		}
		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static implicit operator int(EntityId value)
		{
			return System.Convert.ToInt32(value.Value);
		}
		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public static implicit operator uint(EntityId value)
		{
			return System.Convert.ToUInt32(value.Value);
		}
		#endregion
	}
}