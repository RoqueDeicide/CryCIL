using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public struct EntityGUID
	{
		internal UInt64 _value;

		[System.CLSCompliant(false)]
		public EntityGUID(UInt64 id)
		{
			_value = id;
		}

		#region Overrides
		public override bool Equals(object obj)
		{
			if (obj is EntityGUID)
				return (EntityGUID)obj == this;
			if (obj is UInt64)
				return (UInt64)obj == _value;
			if (obj is uint)
				return (uint)obj == _value;

			return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public override string ToString()
		{
			return _value.ToString();
		}
		#endregion

		#region Operators
		public static bool operator ==(EntityGUID entId1, EntityGUID entId2)
		{
			return entId1._value == entId2._value;
		}

		public static bool operator !=(EntityGUID entId1, EntityGUID entId2)
		{
			return entId1._value != entId2._value;
		}

		[System.CLSCompliant(false)]
		public static implicit operator EntityGUID(UInt64 value)
		{
			return new EntityGUID(value);
		}

		public static implicit operator EntityGUID(Int64 value)
		{
			return new EntityGUID((UInt64)value);
		}

		public static implicit operator Int64(EntityGUID value)
		{
			return System.Convert.ToInt64(value._value);
		}

		[System.CLSCompliant(false)]
		public static implicit operator UInt64(EntityGUID value)
		{
			return System.Convert.ToUInt64(value._value);
		}
		#endregion
	}
}