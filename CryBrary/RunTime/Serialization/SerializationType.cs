﻿namespace CryEngine.RunTime.Serialization
{
	public enum SerializationType
	{
		Null = 0,
		Reference,
		Enum,
		Any,
		String,
		IntPtr,
		UnusedMarker,

		/// <summary>
		/// Not actually used, but signifies that any SerializationType higher than this value is a
		/// reference type.
		/// </summary>
		ReferenceTypes,

		Array,
		Enumerable,
		GenericEnumerable,

		Object,
		MemberInfo,
		Delegate,
		Type,
	}
}