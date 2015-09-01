using System;
using System.Linq;
using System.Reflection;
using CryCil.RunTime;

namespace CryCil.Engine.Physics.Primitives
{
	/// <summary>
	/// Defines objects that represent primitive geometric objects.
	/// </summary>
	public static partial class Primitive
	{
		internal static readonly int[] RegisteredTypes;
		static Primitive()
		{
			var ids = from type in typeof(Primitive).GetNestedTypes()
					  where type.ContainsAttribute<PrimitiveTypeAttribute>()
					  let fieldInfo = type.GetField("Id", BindingFlags.Static | BindingFlags.Public)
					  where fieldInfo != null && fieldInfo.FieldType == typeof(int)
					  select (int)fieldInfo.GetValue(null);
			RegisteredTypes = ids.ToArray();
			Array.Sort(RegisteredTypes);
		}
		/// <summary>
		/// Base type for structures that represent primitives.
		/// </summary>
		/// <remarks>Currently it is not used for anything.</remarks>
		public struct BasePrimitive
		{
		}
	}
}