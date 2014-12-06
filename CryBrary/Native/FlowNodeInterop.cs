using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;

namespace CryEngine.Native
{
	internal static class FlowNodeInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void RegisterNode(string typeName);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetRegularlyUpdated(IntPtr nodePtr, bool updated);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsPortActive(IntPtr nodePtr, int port);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutput(IntPtr nodePtr, int port);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutputInt(IntPtr nodePtr, int port, int value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutputFloat(IntPtr nodePtr, int port, float value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutputEntityId(IntPtr nodePtr, int port, uint value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutputString(IntPtr nodePtr, int port, string value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutputBool(IntPtr nodePtr, int port, bool value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ActivateOutputVec3(IntPtr nodePtr, int port, Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPortValueInt(IntPtr nodePtr, int port);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetPortValueFloat(IntPtr nodePtr, int port);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern uint GetPortValueEntityId(IntPtr nodePtr, int port);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string GetPortValueString(IntPtr nodePtr, int port);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetPortValueBool(IntPtr nodePtr, int port);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Vector3 GetPortValueVec3(IntPtr nodePtr, int port);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsOutputConnected(IntPtr nodePtr, int port);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetTargetEntity(IntPtr nodePtr, out uint entId);
	}
}