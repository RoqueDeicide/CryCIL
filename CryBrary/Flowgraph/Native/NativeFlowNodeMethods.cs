using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CryEngine.Flowgraph.Native
{
	internal static class NativeFlowNodeMethods
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void RegisterNode(string typeName);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetRegularlyUpdated(IntPtr nodePtr, bool updated);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool IsPortActive(IntPtr nodePtr, int port);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutput(IntPtr nodePtr, int port);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutputInt(IntPtr nodePtr, int port, int value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutputFloat(IntPtr nodePtr, int port, float value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutputEntityId(IntPtr nodePtr, int port, uint value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutputString(IntPtr nodePtr, int port, string value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutputBool(IntPtr nodePtr, int port, bool value);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateOutputVec3(IntPtr nodePtr, int port, Vector3 value);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetPortValueInt(IntPtr nodePtr, int port);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static float GetPortValueFloat(IntPtr nodePtr, int port);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static uint GetPortValueEntityId(IntPtr nodePtr, int port);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetPortValueString(IntPtr nodePtr, int port);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool GetPortValueBool(IntPtr nodePtr, int port);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static Vector3 GetPortValueVec3(IntPtr nodePtr, int port);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool IsOutputConnected(IntPtr nodePtr, int port);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetTargetEntity(IntPtr nodePtr, out uint entId);
	}
}