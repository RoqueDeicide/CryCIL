using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.RunTime;

namespace CryCil.Engine
{
	/// <summary>
	/// Represents an object that emits the particle effect.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ParticleEmitter
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this emitter is alive in the engine.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Alive
		{
			get { return IsAlive(this.handle); }
		}
		/// <summary>
		/// Indicates whether this emitter doesn't require further attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Instant
		{
			get { return IsInstant(this.handle); }
		}
		/// <summary>
		/// Gets or sets a particle effect that is emitted by this object.
		/// </summary>
		/// <remarks>
		/// It might be dangerous to try changing the particle effect. Expect crashes when setting.
		/// </remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEffect Effect
		{
			get { return GetEffect(this.handle); }
			set { SetEffect(this.handle, value); }
		}
		/// <summary>
		/// Gets or sets parameters that specify how the particles are spawned.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleSpawnParameters SpawnParameters
		{
			get { return GetSpawnParams(this.handle); }
			set { SetSpawnParams(this.handle, value); }
		}
		/// <summary>
		/// Sets position, orientation and scale of this emitter.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public Quatvecale Location
		{
			set { SetLocation(this.handle, value); }
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this emitter.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEmitterFlags Flags
		{
			get { return GetEmitterFlags(this.handle); }
			set { SetEmitterFlags(this.handle, value); }
		}
		/// <summary>
		/// Determines whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when a particle emitter is created.
		/// </summary>
		public static event EventHandler<EmitterEventArgs> Created;
		/// <summary>
		/// Occurs when a particle emitter is deleted.
		/// </summary>
		public static event EventHandler<EmitterEventArgs> Deleted;
		#endregion
		#region Construction
		internal ParticleEmitter(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Activates this emitter.
		/// </summary>
		/// <remarks>
		/// <para>Emitters are initially active.</para>
		/// <para>Emitter will eventually delete itself, if it has limited life-time.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Activate();
		/// <summary>
		/// Stops particle emission and updates of this emitter.
		/// </summary>
		/// <remarks>
		/// <para>Existing particles continue to update and render.</para>
		/// <para>Emitter is not deleted.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Deactivate();
		/// <summary>
		/// Removes this emitter and all particles it has spawned instantly.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Kill();
		/// <summary>
		/// Advances the emitter to its equilibrium state.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Prime();
		/// <summary>
		/// Restarts this emitter from scratch (if active).
		/// </summary>
		/// <remarks>Any existing particles are re-used oldest first.</remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Restart();
		/// <summary>
		/// Emits one particle as specified by the underlying particle effect.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Emit();
		#endregion
		#region Utilities
		[RawThunk("Invoked from the underlying framework to raise event Created.")]
		private static void OnCreated(ParticleEmitter emitter, Quatvecale location, ParticleEffect effect,
									  ParticleEmitterFlags flags)
		{
			try
			{
				EventHandler<EmitterEventArgs> handler = Created;
				if (handler != null) handler(null, new EmitterEventArgs(emitter, location, effect, flags));
			}
			catch (Exception ex)
			{
				// We must do this because we are going to call this function via a raw thunk that doesn't
				// have proper exception handling.
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from the underlying framework to raise event Deleted.")]
		private static void OnDeleted(ParticleEmitter emitter)
		{
			try
			{
				EventHandler<EmitterEventArgs> handler = Deleted;
				if (handler != null) handler(null, new EmitterEventArgs(emitter));
			}
			catch (Exception ex)
			{
				// We must do this because we are going to call this function via a raw thunk that doesn't
				// have proper exception handling.
				MonoInterface.DisplayException(ex);
			}
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsAlive(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsInstant(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffect(IntPtr handle, ParticleEffect pEffect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect GetEffect(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSpawnParams(IntPtr handle, ParticleSpawnParameters spawnParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleSpawnParameters GetSpawnParams(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocation(IntPtr handle, Quatvecale loc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitterFlags GetEmitterFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEmitterFlags(IntPtr handle, ParticleEmitterFlags flags);
		#endregion
	}
}