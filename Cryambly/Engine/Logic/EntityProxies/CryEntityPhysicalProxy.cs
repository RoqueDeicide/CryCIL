using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Data;
using CryCil.Engine.Physics;
using CryCil.Geometry;

namespace CryCil.Engine.Logic.EntityProxies
{
	/// <summary>
	/// Represents a part of the entity that controls manifestation of it in physical world.
	/// </summary>
	public struct CryEntityPhysicalProxy
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets the Axis-Aligned Bounding Box that represents the bounding box of the physical entity in
		/// world-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox WorldBounds
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetWorldBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets the Axis-Aligned Bounding Box that represents the bounding box of the physical entity in
		/// entity-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox LocalBounds
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetLocalBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets the physical entity that represents this one in physical world.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity Physics
		{
			get
			{
				this.AssertInstance();

				return GetPhysicalEntity(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether physical representation of this entity is active.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool PhysicsEnabled
		{
			get
			{
				this.AssertInstance();

				return IsPhysicsEnabled(this.handle);
			}
			set
			{
				this.AssertInstance();

				EnablePhysics(this.handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the bounding box (coordinates are in entity-space) that defines the bounds of the
		/// proximity trigger that is bound to this entity.
		/// </summary>
		/// <remarks>
		/// <para>
		/// When entities enter/leave these bounds this entity receives
		/// <see cref="MonoEntity.AreaEntered"/>/ <see cref="MonoEntity.AreaLeft"/> events.
		/// </para>
		/// <para>You can disable the trigger by setting this property to the empty bounding box.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox TriggerBounds
		{
			get
			{
				this.AssertInstance();

				BoundingBox box;
				GetTriggerBounds(this.handle, out box);
				return box;
			}
			set
			{
				this.AssertInstance();

				SetTriggerBounds(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets the identifier of the first part of the physical entity that is hosted by this one.
		/// </summary>
		/// <remarks>
		/// When entities that have multiple slots are physicalized, their slots are physicalized as parts
		/// of the physical entity which identifiers are a sequence of consecutive numbers starting with
		/// <see cref="FirstPartId"/>.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int FirstPartId
		{
			get
			{
				this.AssertInstance();

				return GetPartId0(this.handle);
			}
		}
		/// <summary>
		/// Sets the value that indicates whether this proxy should ignore the event of the entity getting
		/// moved.
		/// </summary>
		/// <remarks>
		/// Use this when explicitly changing position of the entity when reacting to the logged physics
		/// event without having the proxy react to manual relocation.
		/// </remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool XFormEventIgnored
		{
			set
			{
				this.AssertInstance();

				IgnoreXFormEvent(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal CryEntityPhysicalProxy(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Assigns an already created and initialized physical entity to this proxy.
		/// </summary>
		/// <param name="entity">Entity to assign.</param>
		/// <param name="slot">  
		/// Zero-based index of the slot the physical entity should inherit world-space tranformation from.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetPhysics(PhysicalEntity entity, int slot = -1)
		{
			this.AssertInstance();

			AssignPhysicalEntity(this.handle, entity, slot);
		}
		/// <summary>
		/// Creates a physical entity that will represent this entity in physical world.
		/// </summary>
		/// <param name="parameters">
		/// Reference to the object that encapsulates a set of parameters that describe the physical
		/// entity.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Physicalize(ref EntityPhysicalizationParameters parameters)
		{
			this.AssertInstance();

			PhysicalizeInternal(this.handle, ref parameters);
		}
		/// <summary>
		/// Attaches this physical entity to another entity that is a soft body.
		/// </summary>
		/// <param name="entity">
		/// Physical entity that represents the soft body to attach this one to.
		/// </param>
		/// <param name="partId">
		/// Optional identifier of the part of the <paramref name="entity"/> to attach this entity to.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentNullException">
		/// Physical entity to attach this one to cannot be null.
		/// </exception>
		public void AttachToSoftBody(PhysicalEntity entity, int partId = -1)
		{
			this.AssertInstance();
			if (!entity.IsValid)
			{
				throw new ArgumentNullException("entity", "Physical entity to attach this one to cannot be null.");
			}

			ReattachSoftEntityVtx(this.handle, entity, partId);
		}
		/// <summary>
		/// Synchronizes physical manifestation of this entity over network.
		/// </summary>
		/// <param name="sync"> An object that handle synchronization.</param>
		/// <param name="type"> Type of the entity to synchronize as.</param>
		/// <param name="flags">A set of flags that specify how work with a data snapshot.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SynchronizeTyped(CrySync sync, PhysicalEntityType type, SnapshotFlags flags = 0)
		{
			this.AssertInstance();

			SerializeTyped(this.handle, sync, (int)type, (int)flags);
		}
		/// <summary>
		/// Adds an impulse to the physical representation of this entity.
		/// </summary>
		/// <param name="position"> 
		/// Reference to the point in world-space at which to add the impulse.
		/// </param>
		/// <param name="impulse">  
		/// Reference to the vector that represents the direction and magnitude of he impulse.
		/// </param>
		/// <param name="auxScale"> Scaling factor to use if this entity is a living entity.</param>
		/// <param name="pushScale">Scaling factor to use if this entity is not a living entity.</param>
		/// <param name="partId">   
		/// Optional identifier of the part of the physical entity to add the impulse to. If not specified,
		/// all parts will be affected.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AddImpulse(ref Vector3 position, ref Vector3 impulse, float auxScale = 1.0f,
							   float pushScale = 1.0f, int partId = -1)
		{
			this.AssertInstance();

			AddImpulseInternal(this.handle, partId, ref position, ref impulse, true, auxScale, pushScale);
		}
		/// <summary>
		/// Adds an impulse to the physical representation of this entity.
		/// </summary>
		/// <param name="impulse">  
		/// Reference to the vector that represents the direction and magnitude of he impulse.
		/// </param>
		/// <param name="auxScale"> Scaling factor to use if this entity is a living entity.</param>
		/// <param name="pushScale">Scaling factor to use if this entity is not a living entity.</param>
		/// <param name="partId">   
		/// Optional identifier of the part of the physical entity to add the impulse to. If not specified,
		/// all parts will be affected.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void AddImpulse(ref Vector3 impulse, float auxScale = 1.0f, float pushScale = 1.0f, int partId = -1)
		{
			this.AssertInstance();

			Vector3 z = new Vector3();
			AddImpulseInternal(this.handle, partId, ref z, ref impulse, false, auxScale, pushScale);
		}
		/// <summary>
		/// Enables/disables automatic synchronization of the hosted physical entity over network.
		/// </summary>
		/// <remarks>
		/// Only set this to <c>true</c> when working with objects that were created via Editor (e.g. when
		/// implementing environmental weapons). This information is a ppure speculation based on how this
		/// functionality is used in CEnvironmentWeapon implementation in GameSDK.
		/// </remarks>
		/// <param name="enable">Indicates whether synchronization must be enabled.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void EnableNetworkSynchronization(bool enable)
		{
			this.AssertInstance();

			EnableNetworkSerialization(this.handle, enable);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AssignPhysicalEntity(IntPtr handle, PhysicalEntity pPhysEntity, int nSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetWorldBounds(IntPtr handle, out BoundingBox bounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalBounds(IntPtr handle, out BoundingBox bounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void PhysicalizeInternal
			(IntPtr handle, ref EntityPhysicalizationParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReattachSoftEntityVtx(IntPtr handle, PhysicalEntity pAttachToEntity,
														 int nAttachToPart);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PhysicalEntity GetPhysicalEntity(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SerializeTyped(IntPtr handle, CrySync ser, int type, int flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnablePhysics(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsPhysicsEnabled(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddImpulseInternal(IntPtr handle, int ipart, ref Vector3 pos,
													  ref Vector3 impulse, bool bPos, float fAuxScale,
													  float fPushScale = 1.0f);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTriggerBounds(IntPtr handle, ref BoundingBox bbox);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetTriggerBounds(IntPtr handle, out BoundingBox bbox);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPartId0(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableNetworkSerialization(IntPtr handle, bool enable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void IgnoreXFormEvent(IntPtr handle, bool ignore);
		#endregion
	}
}