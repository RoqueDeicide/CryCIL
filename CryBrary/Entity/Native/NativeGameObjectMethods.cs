using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
	public static class NativeGameObjectMethods
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static IntPtr GetGameObject(EntityId id);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void EnablePostUpdates(IntPtr gameObjectPtr, IntPtr extensionPtr, bool enable);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void EnablePrePhysicsUpdates(IntPtr gameObjectPtr, PrePhysicsUpdateMode mode);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static IntPtr QueryExtension(IntPtr gameObjectPtr, string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static IntPtr AcquireExtension(IntPtr gameObjectPtr, string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void ReleaseExtension(IntPtr gameObjectPtr, string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static bool ActivateExtension(IntPtr gameObjectPtr, string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void DeactivateExtension(IntPtr gameObjectPtr, string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void ChangedNetworkState(IntPtr gameObjectPtr, int aspect);

		[CLSCompliant(false)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static bool SetAspectProfile(IntPtr gameObjectPtr, EntityAspects aspect, ushort profile, bool fromNetwork = false);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static void EnablePhysicsEvent(IntPtr gameObjectPtr, bool enable, EntityPhysicsEvents physicsEvent);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static bool WantsPhysicsEvent(IntPtr gameObjectPtr, EntityPhysicsEvents physicsEvent);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern public static bool BindToNetwork(IntPtr gameObjectPtr, BindToNetworkMode mode);
	}
}