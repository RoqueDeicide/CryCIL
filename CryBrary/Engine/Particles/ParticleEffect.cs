﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine
{
	public class ParticleEffect
	{
		internal ParticleEffect(IntPtr ptr)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (ptr == IntPtr.Zero)
				throw new NullPointerException();
#endif

			Handle = ptr;
		}

		internal IntPtr Handle { get; set; }

		#region Statics
		/// <summary>
		/// </summary>
		/// <param name="effectName">   </param>
		/// <param name="loadResources">Load all required resources?</param>
		/// <returns>The specified particle effect, or null if failed.</returns>
		public static ParticleEffect Get(string effectName, bool loadResources = true)
		{
			var effectHandle = ParticleEffectInterop.FindEffect(effectName, loadResources);
			if (effectHandle == IntPtr.Zero)
				return null;

			return TryGet(effectHandle);
		}

		internal static ParticleEffect TryGet(IntPtr handle)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (handle == IntPtr.Zero)
				throw new NullPointerException();
#endif
			var particleEffect = particleEffects.FirstOrDefault(x => x.Handle == handle);
			if (particleEffect == null)
			{
				particleEffect = new ParticleEffect(handle);
				particleEffects.Add(particleEffect);
			}

			return particleEffect;
		}

		private static readonly List<ParticleEffect> particleEffects = new List<ParticleEffect>();
		#endregion

		/// <summary>
		/// Spawns this effect
		/// </summary>
		/// <param name="independent"></param>
		/// <param name="pos">        World location to place emitter at.</param>
		/// <param name="dir">        
		/// World rotation of emitter, set to Vec3.Up if null.
		/// </param>
		/// <param name="scale">      Scale of the emitter.</param>
		public ParticleEmitter Spawn(Vector3 pos, Vector3? dir = null, float scale = 1f, bool independent = true)
		{
			return ParticleEmitter.TryGet(ParticleEffectInterop.Spawn(Handle, independent, pos, dir ?? Vector3.Up, scale));
		}

		public void Remove()
		{
			ParticleEffectInterop.Remove(Handle);
		}

		public void LoadResources()
		{
			ParticleEffectInterop.LoadResoruces(Handle);
		}

		public ParticleEffect GetChild(int index)
		{
			var childHandle = ParticleEffectInterop.GetChild(Handle, index);

			return TryGet(childHandle);
		}

		/// <summary>
		/// Gets the number of sub-particles assigned to this effect.
		/// </summary>
		public int ChildCount { get { return ParticleEffectInterop.GetChildCount(Handle); } }

		public string Name { get { return ParticleEffectInterop.GetName(Handle); } }
		public string FullName { get { return ParticleEffectInterop.GetFullName(Handle); } }

		public bool Enabled { get { return ParticleEffectInterop.IsEnabled(Handle); } set { ParticleEffectInterop.Enable(Handle, value); } }

		public ParticleEffect Parent { get { return TryGet(ParticleEffectInterop.GetParent(Handle)); } }

		#region Operator overloads
		/// <summary>
		/// Gets sub-effect by index.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public ParticleEffect this[int i]
		{
			get { return GetChild(i); }
		}

		/// <summary>
		/// Gets sub-effect by name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ParticleEffect this[string name]
		{
			get
			{
				for (var i = 0; i < ChildCount; i++)
				{
					var child = GetChild(i);
					if (child.Name == name)
						return child;
				}

				return null;
			}
		}
		#endregion

		#region Overrides
		public override bool Equals(object obj)
		{
			return obj is ParticleEffect && this == obj;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 29 + Handle.GetHashCode();

				return hash;
			}
		}
		#endregion
	}
}