using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CryEngine.Native
{
	internal static class ConsoleInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Execute(string command, bool silent);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RegisterCommand(string name, string description, CVarFlags flags);

		// CVars
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RegisterCVarFloat(string name, ref float val, float defaultVal, CVarFlags flags,
													   string description);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RegisterCVarInt(string name, ref int val, int defaultVal, CVarFlags flags,
													 string description);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RegisterCVarString(string name, [MarshalAs(UnmanagedType.LPStr)] string val,
														string defaultVal, CVarFlags flags, string description);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UnregisterCVar(string name, bool delete = false);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UnregisterCCommand(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetCVarFloat(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCVarInt(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetCVarString(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetCVarFloat(string name, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetCVarInt(string name, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetCVarString(string name, string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasCVar(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void HandleException(Exception ex);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetCmdArg(string name, int type);
	}
}