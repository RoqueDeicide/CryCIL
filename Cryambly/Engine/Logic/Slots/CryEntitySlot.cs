using System;
using System.Diagnostics.Contracts;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering;
using CryCil.Engine.Rendering.Lighting;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Represents a CryEngine entity slot.
	/// </summary>
	/// <remarks>
	/// Entity slots are used to represent the entity in visual and physical world. They can store render
	/// meshes (represented by objects of type <see cref="StaticObject"/>), animated characters, geometry
	/// cache nodes and particle emitters.
	/// </remarks>
	public struct CryEntitySlot
	{
		#region Fields
		/// <summary>
		/// A value that is usually used to designate all slots at the same time.
		/// </summary>
		public const int All = -1;

		private readonly IntPtr entityHandle;
		private readonly int index;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this slot object is valid.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return this.entityHandle != IntPtr.Zero && this.index >= 0 &&
					   EntitySlotOps.IsSlotValid(this.entityHandle, this.index);
			}
		}
		/// <summary>
		/// Gets collective information about this slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public EntitySlotInfo Information
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				EntitySlotInfo info;
				EntitySlotOps.GetSlotInfo(this.entityHandle, this.index, out info);
				return info;
			}
		}
		/// <summary>
		/// Gets or sets world transformation matrix for this slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public Matrix34 WorldTransformationMatrix
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				Matrix34 matrix;
				EntitySlotOps.GetSlotWorldTM(this.entityHandle, this.index, out matrix);
				return matrix;
			}
		}
		/// <summary>
		/// Gets or sets local transformation matrix for this slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public Matrix34 LocalTransformationMatrix
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				Matrix34 matrix;
				EntitySlotOps.GetSlotLocalTM(this.entityHandle, this.index, false, out matrix);
				return matrix;
			}
			set
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				EntitySlotOps.SetSlotLocalTM(this.entityHandle, this.index, ref value);
			}
		}
		/// <summary>
		/// Gets or sets local transformation matrix for this slot relative to its parent slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public Matrix34 LocalTransformationMatrixRelativeToParentSlot
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				Matrix34 matrix;
				EntitySlotOps.GetSlotLocalTM(this.entityHandle, this.index, true, out matrix);
				return matrix;
			}
		}
		/// <summary>
		/// Gets or sets position of this slot in camera space.
		/// </summary>
		/// <remarks>
		/// Used when positioning the slot that has <see cref="EntitySlotFlags.RenderNearest"/> flag set.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public Vector3 CameraSpacePosition
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				Vector3 position;
				EntitySlotOps.GetSlotCameraSpacePos(this.entityHandle, this.index, out position);
				return position;
			}
			set
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				EntitySlotOps.SetSlotCameraSpacePos(this.entityHandle, this.index, ref value);
			}
		}
		/// <summary>
		/// Gets or sets a zero-based index of the slot that is a parent of this one.
		/// </summary>
		/// <remarks>
		/// Invocation of the getter of this property involves invocation of the getter for
		/// <see cref="Information"/> property, so use that when you need to acquire a lot of information
		/// about this slot at once.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot set invalid slot as a parent for another.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Cannot set a slot of another entity as a parent slot for this one.
		/// </exception>
		public CryEntitySlot ParentSlot
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				return new CryEntitySlot(this.entityHandle, this.Information.ParentSlot);
			}
			set
			{
				this.AssertSlotValidity();
				if (!value.IsValid)
				{
					throw new ArgumentNullException("value", "Cannot set invalid slot as a parent for another.");
				}
				if (value.entityHandle != this.entityHandle)
				{
					throw new ArgumentException("Cannot set a slot of another entity as a parent slot for this one.");
				}
				Contract.EndContractBlock();

				EntitySlotOps.SetParentSlot(this.entityHandle, value.index, this.index);
			}
		}
		/// <summary>
		/// Gets or sets a material this slot uses for rendering.
		/// </summary>
		/// <remarks>
		/// Invocation of the getter of this property involves invocation of the getter for
		/// <see cref="Information"/> property, so use that when you need to acquire a lot of information
		/// about this slot at once.
		/// </remarks>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot assign invalid material to the entity slot.
		/// </exception>
		public Material Material
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				return this.Information.Material;
			}
			set
			{
				this.AssertSlotValidity();
				if (!value.IsValid)
				{
					throw new ArgumentNullException("value", "Cannot assign invalid material to the entity slot.");
				}
				Contract.EndContractBlock();

				EntitySlotOps.SetSlotMaterial(this.entityHandle, this.index, value);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that describe how to render this slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public EntitySlotFlags Flags
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				return EntitySlotOps.GetSlotFlags(this.entityHandle, this.index);
			}
			set
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				EntitySlotOps.SetSlotFlags(this.entityHandle, this.index, value);
			}
		}
		/// <summary>
		/// Gets or sets the particle emitter that is bound to this slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot bind null emitter to the entity slot.
		/// </exception>
		public ParticleEmitter BoundEmitter
		{
			get
			{
				this.AssertSlotValidity();
				Contract.EndContractBlock();

				return EntitySlotOps.GetParticleEmitter(this.entityHandle, this.index);
			}
			set
			{
				this.AssertSlotValidity();
				if (!value.IsValid)
				{
					throw new ArgumentNullException("value", "Cannot bind null emitter to the entity slot.");
				}
				Contract.EndContractBlock();

				EntitySlotOps.SetParticleEmitter(this.entityHandle, this.index, value, true);
			}
		}
		#endregion
		#region Construction
		internal CryEntitySlot(IntPtr entityHandle, int index)
		{
			this.entityHandle = entityHandle;
			this.index = index;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases this entity slot.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public void Release()
		{
			if (this.IsValid)
			{
				EntitySlotOps.FreeSlot(this.entityHandle, this.index);
				// I'm not setting entityHandle and index to invalid values so AssertSlotValidity function
				// can throw ObjectDisposedException instead of NullReferenceException.
			}
		}
		/// <summary>
		/// Moves this slot from current entity to another.
		/// </summary>
		/// <remarks>
		/// The index of the slot doesn't change during the transfer, if <paramref name="target"/> already
		/// has that slot allocated, it will be overwritten.
		/// </remarks>
		/// <param name="target">Identifier of the entity to move this slot to.</param>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot transfer a slot to an invalid entity.
		/// </exception>
		public void Transfer(EntityId target)
		{
			this.AssertSlotValidity();
			if (!target.Entity.IsValid)
			{
				throw new ArgumentNullException("target", "Cannot transfer a slot to an invalid entity.");
			}
			Contract.EndContractBlock();

			EntitySlotOps.MoveSlot(this.entityHandle, target.Entity, this.index);
		}
		/// <summary>
		/// Moves this slot from current entity to another.
		/// </summary>
		/// <remarks>
		/// The index of the slot doesn't change during the transfer, if <paramref name="target"/> already
		/// has that slot allocated, it will be overwritten.
		/// </remarks>
		/// <param name="target">Entity to move this slot to.</param>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot transfer a slot to an invalid entity.
		/// </exception>
		public void Transfer(CryEntity target)
		{
			this.AssertSlotValidity();
			if (!target.IsValid)
			{
				throw new ArgumentNullException("target", "Cannot transfer a slot to an invalid entity.");
			}
			Contract.EndContractBlock();

			EntitySlotOps.MoveSlot(this.entityHandle, target, this.index);
		}
		/// <summary>
		/// Creates a particle emitter and binds it to this slot.
		/// </summary>
		/// <param name="effect">Particle effect to use as a base for the emitter.</param>
		/// <param name="prime"> Indicates whether emitter should be primed immediatelly.</param>
		/// <param name="sync">  Indicates whether state of the emitter must be synchronized.</param>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot use null particle effect to create an emitter that can be bound to the entity slot.
		/// </exception>
		public void LoadParticleEmitter(ParticleEffect effect, bool prime = false, bool sync = false)
		{
			this.AssertSlotValidity();
			if (!effect.IsValid)
			{
				throw new ArgumentNullException("effect", "Cannot use null particle effect to create an emitter that can " +
														  "be bound to the entity slot.");
			}
			Contract.EndContractBlock();

			EntitySlotOps.LoadParticleEmitterDefault(this.entityHandle, this.index, effect, prime, sync);
		}
		/// <summary>
		/// Creates a particle emitter and binds it to this slot.
		/// </summary>
		/// <param name="effect">    Particle effect to use as a base for the emitter.</param>
		/// <param name="parameters">
		/// A reference to the set of parameters that indicate how to spawn the particles.
		/// </param>
		/// <param name="prime">     Indicates whether emitter should be primed immediatelly.</param>
		/// <param name="sync">      Indicates whether state of the emitter must be synchronized.</param>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		/// <exception cref="ArgumentNullException">
		/// Cannot use null particle effect to create an emitter that can be bound to the entity slot.
		/// </exception>
		public void LoadParticleEmitter(ParticleEffect effect, ref ParticleSpawnParameters parameters, bool prime = false,
										bool sync = false)
		{
			this.AssertSlotValidity();
			if (!effect.IsValid)
			{
				throw new ArgumentNullException("effect", "Cannot use null particle effect to create an emitter that can " +
														  "be bound to the entity slot.");
			}
			Contract.EndContractBlock();

			EntitySlotOps.LoadParticleEmitter(this.entityHandle, this.index, effect, ref parameters, prime, sync);
		}
		/// <summary>
		/// Puts a light source with given parameters into this slot.
		/// </summary>
		/// <param name="properties">A set of properties that describe the light source.</param>
		/// <exception cref="NullReferenceException">This entity slot object is not valid.</exception>
		/// <exception cref="ObjectDisposedException">This entity slot doesn't exist.</exception>
		public void LoadLight(ref LightProperties properties)
		{
			this.AssertSlotValidity();
			Contract.EndContractBlock();

			EntitySlotOps.LoadLight(this.entityHandle, this.index, ref properties);
		}
		/// <summary>
		/// Physicalizes this slot. Can only be done, if the entity was physicalized before.
		/// </summary>
		/// <param name="parameters">
		/// Reference to object that describes how to physicalize the slot.
		/// </param>
		/// <returns>Some number, not sure what it means.</returns>
		public int Physicalize(ref EntityPhysicalizationParameters parameters)
		{
			this.AssertSlotValidity();
			Contract.EndContractBlock();

			return EntitySlotOps.PhysicalizeSlot(this.entityHandle, this.index, ref parameters);
		}
		/// <summary>
		/// Dephysicalizes this slot.
		/// </summary>
		public void Unphysicalize()
		{
			this.AssertSlotValidity();
			Contract.EndContractBlock();

			EntitySlotOps.UnphysicalizeSlot(this.entityHandle, this.index);
		}
		/// <summary>
		/// Updates physics of this slot(?).
		/// </summary>
		public void UpdatePhysics()
		{
			this.AssertSlotValidity();
			Contract.EndContractBlock();

			EntitySlotOps.UpdateSlotPhysics(this.entityHandle, this.index);
		}
		#endregion
		#region Utilities
		private void AssertSlotValidity()
		{
			if (this.entityHandle == IntPtr.Zero && this.index < 0)
			{
				throw new NullReferenceException("This entity slot object is not valid.");
			}
			if (!EntitySlotOps.IsSlotValid(this.entityHandle, this.index))
			{
				throw new ObjectDisposedException("This entity slot doesn't exist.");
			}
		}
		#endregion
	}
}