using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Logic;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Physics;
using CryCil.Geometry;
using CryCil.RunTime;

namespace CryCil.Engine
{
	/// <summary>
	/// Defines signature of methods that can handle <see cref="ParticleEmitter.Created"/> event.
	/// </summary>
	/// <param name="emitter"> An object that represents the created emitter.</param>
	/// <param name="location">Reference to location of the emitter.</param>
	/// <param name="effect">  
	/// An object that represents the particle effect that is used by the created emitter.
	/// </param>
	/// <param name="flags">   A set of flags the emitter was created with.</param>
	public delegate void EmitterCreatedEventHandler(
		ParticleEmitter emitter, ref Quatvecale location, ParticleEffect effect,
		ParticleEmitterFlags flags);
	/// <summary>
	/// Defines signature of methods that can handle <see cref="ParticleEmitter.Deleted"/> event.
	/// </summary>
	/// <param name="emitter">
	/// Particle emitter that is about to get deleted (or it's already deleted(?)).
	/// </param>
	public delegate void EmitterDeletedEventHandler(ParticleEmitter emitter);
	/// <summary>
	/// Represents an object that emits the particle effect.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ParticleEmitter
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Determines whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Indicates whether this emitter is alive in the engine.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Alive
		{
			get
			{
				this.AssertInstance();

				return IsAlive(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this emitter doesn't require further attachment.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Instant
		{
			get
			{
				this.AssertInstance();

				return IsInstant(this.handle);
			}
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
			get
			{
				this.AssertInstance();

				return GetEffect(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEffect(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets parameters that specify how the particles are spawned.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleSpawnParameters SpawnParameters
		{
			get
			{
				this.AssertInstance();

				ParticleSpawnParameters parameters;
				GetSpawnParamsInternal(this.handle, out parameters);
				return parameters;
			}
			set
			{
				this.AssertInstance();

				SetSpawnParamsInternal(this.handle, ref value, new GeometryReference());
			}
		}
		/// <summary>
		/// Sets position, orientation and scale of this emitter.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public Quatvecale Location
		{
			set
			{
				this.AssertInstance();

				SetLocation(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this emitter.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleEmitterFlags Flags
		{
			get
			{
				this.AssertInstance();

				return GetEmitterFlags(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetEmitterFlags(this.handle, value);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this emitter is active.
		/// </summary>
		/// <remarks>
		/// <para>Emitters are initially active.</para>
		/// <para>Emitter will eventually delete itself, if it has limited life-time.</para>
		/// <para>
		/// After deactivating the emitter any existing particles continue to update and render.
		/// </para>
		/// <para>Deactivation doesn't delete the emitter.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool Active
		{
			set
			{
				this.AssertInstance();

				ActivateInternal(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the entity and a slot position of this emitter is associated with.
		/// </summary>
		/// <remarks>
		/// Association that is created by utilizing this property is not synchronized automatically; you
		/// have to do it manually.
		/// </remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		/// <exception cref="ArgumentException">Given entity slot is not valid.</exception>
		public CryEntitySlot Entity
		{
			get
			{
				this.AssertInstance();

				IntPtr entityHandle;
				int slot;
				GetAttachedEntity(this.handle, out entityHandle, out slot);

				return new CryEntitySlot(entityHandle, slot);
			}
			set
			{
				this.AssertInstance();

				if (!value.IsValid)
				{
					throw new ArgumentException("Given entity slot is not valid.");
				}

				SetEntity(this.handle, new CryEntity(value.Handle), value.Slot);
			}
		}
		/// <summary>
		/// Sets the target for particles: a place that attracts the particles.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ParticleTarget Target
		{
			set
			{
				this.AssertInstance();

				SetTarget(this.handle, ref value);
			}
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when a particle emitter is created.
		/// </summary>
		public static event EmitterCreatedEventHandler Created;
		/// <summary>
		/// Occurs when a particle emitter is deleted.
		/// </summary>
		public static event EmitterDeletedEventHandler Deleted;
		#endregion
		#region Construction
		internal ParticleEmitter(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Removes this emitter and all particles it has spawned instantly.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void Kill()
		{
			this.AssertInstance();

			KillInternal(this.handle);
		}
		/// <summary>
		/// Advances the emitter to its equilibrium state.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void Prime()
		{
			this.AssertInstance();

			PrimeInternal(this.handle);
		}
		/// <summary>
		/// Restarts this emitter from scratch (if active).
		/// </summary>
		/// <remarks>Any existing particles are re-used oldest first.</remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void Restart()
		{
			this.AssertInstance();

			RestartInternal(this.handle);
		}
		/// <summary>
		/// Emits one particle as specified by the underlying particle effect.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public void Emit()
		{
			this.AssertInstance();

			EmitParticle(this.handle, null);
		}
		/// <summary>
		/// Emits one particle from this emitter.
		/// </summary>
		/// <param name="particleObject">Static object to use to render the particle.</param>
		/// <param name="physicalEntity">A physical entity which controls the particle.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Emit(StaticObject particleObject, PhysicalEntity physicalEntity)
		{
			this.AssertInstance();

			EmitParticleData data = new EmitParticleData
			{
				StatObj = particleObject,
				PhysEnt = physicalEntity
			};

			EmitParticle(this.handle, &data);
		}
		/// <summary>
		/// Emits one particle from this emitter.
		/// </summary>
		/// <param name="particleObject">Static object to use to render the particle.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Emit(StaticObject particleObject)
		{
			this.AssertInstance();

			EmitParticleData data = new EmitParticleData
			{
				StatObj = particleObject
			};

			EmitParticle(this.handle, &data);
		}
		/// <summary>
		/// Emits one particle from this emitter.
		/// </summary>
		/// <param name="physicalEntity">A physical entity which controls the particle.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Emit(PhysicalEntity physicalEntity)
		{
			this.AssertInstance();

			EmitParticleData data = new EmitParticleData
			{
				PhysEnt = physicalEntity
			};

			EmitParticle(this.handle, &data);
		}
		/// <summary>
		/// Emits one particle.
		/// </summary>
		/// <param name="location">      
		/// Reference to the object that defines position, orientation and scale of the particle.
		/// </param>
		/// <param name="velocity">      
		/// Reference to the object that defines linear and rotational velocities of the particle.
		/// </param>
		/// <param name="particleObject">
		/// Static object to use to render the particle. If not specified, emitter settings are used to
		/// display the particle.
		/// </param>
		/// <param name="physicalEntity">
		/// A physical entity which controls the particle. If not specified, emitter settings are used to
		/// physicalize or move the particle.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Emit(ref Quatvecale location, ref Velocity3 velocity, StaticObject particleObject = new StaticObject(),
						 PhysicalEntity physicalEntity = new PhysicalEntity())
		{
			this.AssertInstance();

			EmitParticleData data = new EmitParticleData
			{
				StatObj = particleObject,
				PhysEnt = physicalEntity,
				Location = location,
				HasLocation = true,
				Velocity = velocity,
				HasVel = true
			};

			EmitParticle(this.handle, &data);
		}
		/// <summary>
		/// Emits one particle.
		/// </summary>
		/// <param name="location">      
		/// Reference to the object that defines position, orientation and scale of the particle.
		/// </param>
		/// <param name="particleObject">
		/// Static object to use to render the particle. If not specified, emitter settings are used to
		/// display the particle.
		/// </param>
		/// <param name="physicalEntity">
		/// A physical entity which controls the particle. If not specified, emitter settings are used to
		/// physicalize or move the particle.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Emit(ref Quatvecale location, StaticObject particleObject = new StaticObject(),
						 PhysicalEntity physicalEntity = new PhysicalEntity())
		{
			this.AssertInstance();

			EmitParticleData data = new EmitParticleData
			{
				StatObj = particleObject,
				PhysEnt = physicalEntity,
				Location = location,
				HasLocation = true
			};

			EmitParticle(this.handle, &data);
		}
		/// <summary>
		/// Emits one particle.
		/// </summary>
		/// <param name="velocity">      
		/// Reference to the object that defines linear and rotational velocities of the particle.
		/// </param>
		/// <param name="particleObject">
		/// Static object to use to render the particle. If not specified, emitter settings are used to
		/// display the particle.
		/// </param>
		/// <param name="physicalEntity">
		/// A physical entity which controls the particle. If not specified, emitter settings are used to
		/// physicalize or move the particle.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Emit(ref Velocity3 velocity, StaticObject particleObject = new StaticObject(),
						 PhysicalEntity physicalEntity = new PhysicalEntity())
		{
			this.AssertInstance();

			EmitParticleData data = new EmitParticleData
			{
				StatObj = particleObject,
				PhysEnt = physicalEntity,
				Velocity = velocity,
				HasVel = true
			};

			EmitParticle(this.handle, &data);
		}
		/// <summary>
		/// Changes a set of parameters that specify how to spawn the particles.
		/// </summary>
		/// <remarks>
		/// <paramref name="geometry"/> can be used to engulf objects and characters in the particle effect,
		/// e.g. when setting the character or wooden box on fire.
		/// </remarks>
		/// <param name="parameters">Reference to the object that contains the parameters.</param>
		/// <param name="geometry">  
		/// An object that contains references to geometry to attach the particle effect to.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetSpawnParameters(ref ParticleSpawnParameters parameters, GeometryReference geometry)
		{
			this.AssertInstance();

			SetSpawnParamsInternal(this.handle, ref parameters, geometry);
		}
		#endregion
		#region Utilities
		[RawThunk("Invoked from the underlying framework to raise event Created.")]
		private static void OnCreated(ParticleEmitter emitter, ref Quatvecale location, ParticleEffect effect,
									  ParticleEmitterFlags flags)
		{
			try
			{
				var handler = Created;
				handler?.Invoke(emitter, ref location, effect, flags);
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
				var handler = Deleted;
				handler?.Invoke(emitter);
			}
			catch (Exception ex)
			{
				// We must do this because we are going to call this function via a raw thunk that doesn't
				// have proper exception handling.
				MonoInterface.DisplayException(ex);
			}
		}

		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsAlive(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsInstant(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ActivateInternal(IntPtr handle, bool bActive);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void KillInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PrimeInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RestartInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEffect(IntPtr handle, ParticleEffect pEffect);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEffect GetEffect(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSpawnParamsInternal(IntPtr handle, ref ParticleSpawnParameters spawnParams,
														  GeometryReference geom);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSpawnParamsInternal(IntPtr handle, out ParticleSpawnParameters spawnParams);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEntity(IntPtr handle, CryEntity pEntity, int nSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocation(IntPtr handle, ref Quatvecale loc);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTarget(IntPtr handle, ref ParticleTarget target);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EmitParticle(IntPtr handle, EmitParticleData* pData);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetAttachedEntity(IntPtr handle, out IntPtr entityHandle, out int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ParticleEmitterFlags GetEmitterFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEmitterFlags(IntPtr handle, ParticleEmitterFlags flags);
		#endregion
	}
}