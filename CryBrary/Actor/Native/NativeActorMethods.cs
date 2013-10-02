using System;
using System.Runtime.CompilerServices;

namespace CryEngine.Native
{
    internal static class NativeActorMethods
    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static float GetPlayerHealth(IntPtr actorPtr);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static void SetPlayerHealth(IntPtr actorPtr, float newHealth);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static float GetPlayerMaxHealth(IntPtr actorPtr);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static void SetPlayerMaxHealth(IntPtr actorPtr, float newMaxHealth);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static ActorInitializationParams GetActorInfoByChannelId(ushort channelId);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static ActorInitializationParams GetActorInfoById(uint entId);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static void RegisterActorClass(string name, bool isNative, bool isAI);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static ActorInitializationParams CreateActor(int channelId, string name, string className, Vec3 pos, Quat rot, Vec3 scale);
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static void RemoveActor(uint id);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        extern internal static uint GetClientActorId();

        [MethodImpl(MethodImplOptions.InternalCall)]
        extern internal static void RemoteInvocation(uint entityId, uint targetId, string methodName, object[] args, NetworkTarget target, int channelId);
    }
}