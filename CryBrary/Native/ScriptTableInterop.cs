using System;
using System.Runtime.CompilerServices;
using CryEngine.Lua;

namespace CryEngine.Native
{
	internal static class ScriptTableInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetScriptTable(IntPtr entityPtr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetSubScriptTable(IntPtr scriptTablePtr, string tableName);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static object CallMethod(IntPtr scriptTablePtr, string methodName, object[] parameters);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static object GetValue(IntPtr scriptTablePtr, string keyName);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool ExecuteBuffer(string buffer);
	}
}