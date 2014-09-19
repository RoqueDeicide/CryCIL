using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;

namespace CryEngine.Native
{
	internal static class ActorInterop
	{
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static ActorInitializationParams GetActorInfoByChannelId(ushort channelId);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static ActorInitializationParams GetActorInfoById(uint entId);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void RegisterActorClass(string name, bool isNative);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static ActorInitializationParams CreateActor(int channelId, string name, string className, Vector3 pos, Quaternion rot, Vector3 scale);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void RemoveActor(uint id);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static uint GetClientActorId();

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void RemoteInvocation(uint entityId, uint targetId, string methodName, object[] args, NetworkTarget target, int channelId);
	}
}