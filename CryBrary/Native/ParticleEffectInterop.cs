using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;

namespace CryEngine.Native
{
	internal static class ParticleEffectInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr FindEffect(string effectName, bool loadResources);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr Spawn(IntPtr ptr, bool independent, Vector3 pos, Vector3 dir, float scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void Remove(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void LoadResoruces(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void ActivateEmitter(IntPtr emitter, bool activate);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static ParticleSpawnParameters GetParticleEmitterSpawnParams(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetParticleEmitterSpawnParams(IntPtr ptr, ref ParticleSpawnParameters spawnParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetParticleEmitterEffect(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static string GetName(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static string GetFullName(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void Enable(IntPtr ptr, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static bool IsEnabled(IntPtr ptr);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetChildCount(IntPtr ptr);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetChild(IntPtr ptr, int i);

		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetParent(IntPtr ptr);
	}
}