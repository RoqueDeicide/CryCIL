using System;
using System.Runtime.CompilerServices;
using CryEngine.Entities;
using CryEngine.Entities.Advanced;
using CryEngine.Logic.Entities;

namespace CryEngine.Native
{
	public static class GameObjectInterop
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetGameObject(EntityId id);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void EnablePostUpdates(IntPtr gameObjectPtr, IntPtr extensionPtr, bool enable);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void EnablePrePhysicsUpdates(IntPtr gameObjectPtr, PrePhysicsUpdateMode mode);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr QueryExtension(IntPtr gameObjectPtr, string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern IntPtr AcquireExtension(IntPtr gameObjectPtr, string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void ReleaseExtension(IntPtr gameObjectPtr, string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool ActivateExtension(IntPtr gameObjectPtr, string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void DeactivateExtension(IntPtr gameObjectPtr, string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void ChangedNetworkState(IntPtr gameObjectPtr, int aspect);

		[CLSCompliant(false)]
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool SetAspectProfile(IntPtr gameObjectPtr, EntityAspects aspect, ushort profile,
												   bool fromNetwork = false);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void EnablePhysicsEvent(IntPtr gameObjectPtr, bool enable, EntityPhysicsEvents physicsEvent);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool WantsPhysicsEvent(IntPtr gameObjectPtr, EntityPhysicsEvents physicsEvent);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern bool BindToNetwork(IntPtr gameObjectPtr, BindToNetworkMode mode);
	}
}