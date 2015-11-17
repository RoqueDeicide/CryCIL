using System;
using System.Collections;
using System.Collections.Generic;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering.Lighting;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a collection of slots the entity has.
	/// </summary>
	public struct EntitySlotCollection : IEnumerable<CryEntitySlot>
	{
		#region Fields
		private readonly IntPtr entityHandle;
		#endregion
		#region Properties
		/// <summary>
		/// Grants access to one of the slots.
		/// </summary>
		/// <param name="index">Identifier of the slot to get.</param>
		public CryEntitySlot this[int index]
		{
			get { return new CryEntitySlot(this.entityHandle, index); }
		}
		/// <summary>
		/// Gets number of slots that allocated for this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		public int Count
		{
			get
			{
				this.AssertEntity();

				return EntitySlotOps.GetSlotCount(this.entityHandle);
			}
		}
		#endregion
		#region Construction
		internal EntitySlotCollection(IntPtr handle)
		{
			this.entityHandle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enumerates the entity slots.
		/// </summary>
		/// <remarks>
		/// Only valid slots are enumerated, which means that total number of enumerated slots might not be
		/// equal to the value returned by <see cref="Count"/> property.
		/// </remarks>
		/// <returns>An object that performs enumeration.</returns>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		public IEnumerator<CryEntitySlot> GetEnumerator()
		{
#if DEBUG
			this.AssertEntity();
#else
			if (this.entityHandle == IntPtr.Zero)
			{
				yield break;
			}
#endif
			int slotCount = this.Count;
			for (int i = 0; i < slotCount; i++)
			{
				if (EntitySlotOps.IsSlotValid(this.entityHandle, i))
				{
					yield return new CryEntitySlot(this.entityHandle, i);
				}
			}
		}
		/// <summary>
		/// Creates a particle emitter and binds it to the next available slot.
		/// </summary>
		/// <param name="effect">Particle effect to use as a base for the emitter.</param>
		/// <param name="prime"> Indicates whether emitter should be primed immediatelly.</param>
		/// <param name="sync">  Indicates whether state of the emitter must be synchronized.</param>
		/// <returns>
		/// An object that represents a valid slot, if emitter was loaded successfully, otherwise the slot
		/// won't be valid.
		/// </returns>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot use null particle effect to create an emitter that can be bound to the entity slot.
		/// </exception>
		public CryEntitySlot Load(ParticleEffect effect, bool prime = false, bool sync = false)
		{
			this.AssertEntity();
			if (!effect.IsValid)
			{
				throw new ArgumentNullException("effect", "Cannot use null particle effect to create an emitter that can " +
														  "be bound to the entity slot.");
			}

			return new CryEntitySlot(this.entityHandle,
									 EntitySlotOps.LoadParticleEmitterDefault(this.entityHandle, -1, effect, prime, sync));
		}
		/// <summary>
		/// Creates a particle emitter and binds it to the next available slot.
		/// </summary>
		/// <param name="effect">    Particle effect to use as a base for the emitter.</param>
		/// <param name="parameters">
		/// A reference to the set of parameters that indicate how to spawn the particles.
		/// </param>
		/// <param name="prime">     Indicates whether emitter should be primed immediatelly.</param>
		/// <param name="sync">      Indicates whether state of the emitter must be synchronized.</param>
		/// <returns>
		/// An object that represents a valid slot, if emitter was loaded successfully, otherwise the slot
		/// won't be valid.
		/// </returns>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot use null particle effect to create an emitter that can be bound to the entity slot.
		/// </exception>
		public CryEntitySlot Load(ParticleEffect effect, ref ParticleSpawnParameters parameters,
								  bool prime = false, bool sync = false)
		{
			this.AssertEntity();
			if (!effect.IsValid)
			{
				throw new ArgumentNullException("effect", "Cannot use null particle effect to create an emitter that can " +
														  "be bound to the entity slot.");
			}

			return new CryEntitySlot(this.entityHandle,
									 EntitySlotOps.LoadParticleEmitter(this.entityHandle, -1, effect, ref parameters, prime, sync));
		}
		/// <summary>
		/// Puts a light source with given parameters into next available slot.
		/// </summary>
		/// <param name="properties">A set of properties that describe the light source.</param>
		/// <returns>A valid entity slot the light source has been assigned to.</returns>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		public CryEntitySlot Load(ref LightProperties properties)
		{
			this.AssertEntity();

			return new CryEntitySlot(this.entityHandle, EntitySlotOps.LoadLight(this.entityHandle, -1, ref properties));
		}
		/// <summary>
		/// Puts given particle emitter into the next available slot.
		/// </summary>
		/// <param name="emitter">Emitter to add.</param>
		/// <param name="sync">   
		/// Indicates whether the slot where emitter will be put should be synchronized.
		/// </param>
		/// <returns>An object that represents a valid slot, if adding was successful.</returns>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot bind null emitter to the entity slot.
		/// </exception>
		public CryEntitySlot Add(ParticleEmitter emitter, bool sync = false)
		{
			this.AssertEntity();
			if (!emitter.IsValid)
			{
				throw new ArgumentNullException("emitter", "Cannot bind null emitter to the entity slot.");
			}

			return new CryEntitySlot(this.entityHandle, EntitySlotOps.SetParticleEmitter(this.entityHandle, -1, emitter, sync));
		}
		/// <summary>
		/// Adds static object to one of the slots.
		/// </summary>
		/// <param name="staticObject"> Static object to assign.</param>
		/// <param name="updatePhysics">Indicates whether physics of the entity should be updated.</param>
		/// <param name="mass">         
		/// Optional value that can be used to designate the mass of added static object.
		/// </param>
		/// <returns>An object that represents the slot where added static object now resides.</returns>
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot bind null static object to the entity slot.
		/// </exception>
		public CryEntitySlot Add(StaticObject staticObject, bool updatePhysics, float mass = -1.0f)
		{
			this.AssertEntity();
			if (!staticObject.IsValid)
			{
				throw new ArgumentNullException("staticObject", "Cannot bind null static object to the entity slot.");
			}

			return new CryEntitySlot(this.entityHandle,
									 EntitySlotOps.SetStatObj(this.entityHandle, staticObject.Handle, -1,
															  updatePhysics, mass));
		}
		#endregion
		#region Utilities
		// Assertion method.
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		private void AssertEntity()
		{
			if (this.entityHandle == IntPtr.Zero)
			{
				throw new NullReferenceException("Cannot access slots of an invalid entity.");
			}
		}
		/// <exception cref="NullReferenceException">Cannot access slots of an invalid entity.</exception>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		#endregion
	}
}