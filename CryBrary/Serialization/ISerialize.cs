using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Entities;
using CryEngine.Mathematics;

namespace CryEngine.Serialization
{
	[CLSCompliant(false)]
	public interface ICrySerialize
	{
		void BeginGroup(string name);
		void EndGroup();

		void Value(string name, ref string obj, string policy = null);
		void Value(string name, ref int obj, string policy = null);
		void Value(string name, ref uint obj, string policy = null);
		void Value(string name, ref bool obj, string policy = null);
		void Value(string name, ref EntityId obj, string policy = null);
		void Value(string name, ref float obj, string policy = null);
		void Value(string name, ref Vector3 obj, string policy = null);
		void Value(string name, ref Quaternion obj, string policy = null);
		void EnumValue(string name, ref int obj, int first, int last);
		void EnumValue(string name, ref uint obj, uint first, uint last);

		void FlagPartialRead();

		bool IsReading { get; }
		bool IsWriting { get; }

		SerializationTarget Target { get; }
	}
}