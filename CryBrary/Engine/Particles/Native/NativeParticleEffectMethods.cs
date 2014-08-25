using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;

namespace CryEngine.Engine.Particles.Native
{
	internal static class NativeParticleEffectMethods
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr FindEffect(string effectName, bool loadResources);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr Spawn(IntPtr ptr, bool independent, Vector3 pos, Vector3 dir, float scale);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void Remove(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void LoadResoruces(IntPtr ptr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void ActivateEmitter(IntPtr emitter, bool activate);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static ParticleSpawnParameters GetParticleEmitterSpawnParams(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetParticleEmitterSpawnParams(IntPtr ptr, ref ParticleSpawnParameters spawnParams);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetParticleEmitterEffect(IntPtr ptr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetName(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetFullName(IntPtr ptr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void Enable(IntPtr ptr, bool enable);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool IsEnabled(IntPtr ptr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetChildCount(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetChild(IntPtr ptr, int i);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetParent(IntPtr ptr);
	}
}