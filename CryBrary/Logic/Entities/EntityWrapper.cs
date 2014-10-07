using System;
using System.Runtime.CompilerServices;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Physics;
using CryEngine.RunTime.Registration;
using CryEngine.StaticObjects;

namespace CryEngine.Logic.Entities
{
	/// <summary>
	/// Represents a wrapper for a an IEntity object.
	/// </summary>
	public class EntityWrapper : IDisposable
	{
		#region Fields
		/// <summary>
		/// Gets the handle of this entity.
		/// </summary>
		public IntPtr Handle { get; protected set; }
		/// <summary>
		/// Gets identifier of this entity.
		/// </summary>
		public EntityId Identifier { get; private set; }
		/// <summary>
		/// Gets the object that represents this entity within physical world.
		/// </summary>
		public PhysicalEntity Physics { get; private set; }
		/// <summary>
		/// Indicates whether this entity has to validate its identifier, in case the game removes the entity without invalidating the <see cref="Handle"/>.
		/// </summary>
		public bool ExtraSafe { get; set; }
		private int managed;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this entity object is managed by CryMono.
		/// </summary>
		public bool Managed
		{
			get
			{
				if (this.managed == -1)
				{
					// If this object was create as instance of type that derives from
					// Entity, then it is definitely managed, otherwise, try finding out
					// the name of the entity class, and see if it is registered as one of
					// the CryMono ones.
					if (this is GameObjectExtension)
					{
						this.managed = 1;
					}
					else
					{
						this.managed =
							EntityRegister.Types.ContainsKey
							(
								Native.EntityInterop.GetEntityClassName(this.Handle)
							)
							? 1
							: 0;
					}
				}
				return this.managed == 1;
			}
		}
		/// <summary>
		/// Gets object that manages logic for this entity on CryMono side.
		/// </summary>
		public object ManagedExtension
		{
			get
			{
				if (this.IsDisposed || !this.Managed)
				{
					return null;
				}
				return Native.EntityInterop.GetManagedObject(this.Handle, this.Identifier);
			}
		}
		/// <summary>
		/// Indicates whether it is possible to use this object.
		/// </summary>
		public bool IsDisposed
		{
			get
			{
				return
					this.ExtraSafe
					? this.Handle == IntPtr.Zero
						// Check if identifier is still valid.
						|| this.Handle != Native.EntityInterop.GetEntity(this.Identifier)
					: this.Handle == IntPtr.Zero;
			}
		}
		/// <summary>
		/// Gets or sets view distance ratio for this entity.
		/// </summary>
		/// <remarks>
		/// This property allows to determine how small this entity's image on the screen
		/// must be for it to be considered invisible. Entities that are considered
		/// invisible are not rendered.
		/// </remarks>
		public int ViewDistanceRatio
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetViewDistRatio(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetViewDistRatio(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets level of detail switch ratio.
		/// </summary>
		/// <remarks>
		/// This property allows to determine how small this entity's image on the screen
		/// must be to make render node switch to lower level of detail model.
		/// </remarks>
		public int LevelOfDetailRatio
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetLodRatio(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetLodRatio(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets camera associated with this entity.
		/// </summary>
		/// <returns>Null, if no camera is associated with this entity.</returns>
		public Camera Camera
		{
			get
			{
				this.AssertObjectValidity();

				return Camera.TryGet(Native.EntityInterop.GetCameraProxy(this.Handle));
			}
		}
		/// <summary>
		/// Indicates whether this entity is hidden.
		/// </summary>
		public bool Hidden
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.IsHidden(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.Hide(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets position of this entity in world coordinates.
		/// </summary>
		public Vector3 WorldPosition
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetWorldPos(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetWorldPos(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets position of this entity in local coordinates (relative to parent
		/// entity) .
		/// </summary>
		public Vector3 LocalPosition
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetPos(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetPos(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets quaternion that describes orientation of this entity in world
		/// space.
		/// </summary>
		/// <remarks>
		/// Quaternion describes rotation from vector that points
		/// <see cref="Vector3.Forward"/>(?) to vector that points in the same direction
		/// this entity is facing.
		/// </remarks>
		public Quaternion WorldOrientation
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetWorldRotation(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetWorldRotation(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets quaternion that describes orientation of this entity in local
		/// space.
		/// </summary>
		/// <remarks>
		/// Quaternion describes rotation from vector that points
		/// <see cref="Vector3.Forward"/>(?) to vector that points in the same direction
		/// this entity is facing.
		/// </remarks>
		public Quaternion LocalOrientation
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetRotation(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetRotation(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets the vector that points in the same direction this entity is facing.
		/// </summary>
		public Vector3 ForwardDirection
		{
			get
			{
				this.AssertObjectValidity();

				return this.WorldOrientation.Column1;
			}
		}
		/// <summary>
		/// Gets or sets the matrix that represents translation (position), rotation
		/// (orientation) and scale transformations applied to this entity in relation to
		/// the origin of coordinates.
		/// </summary>
		public Matrix34 WorldTransformation
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetWorldTM(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetWorldTM(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the matrix that represents translation (position), rotation
		/// (orientation) and scale transformations applied to this entity in relation to
		/// the parent entity.
		/// </summary>
		public Matrix34 LocalTransformation
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetLocalTM(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetLocalTM(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets the entity axis aligned bounding box in the world space.
		/// </summary>
		public BoundingBox WorldBoundingBox
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetWorldBoundingBox(this.Handle);
			}
		}
		/// <summary>
		/// Gets the entity axis aligned bounding box in the local space.
		/// </summary>
		public BoundingBox LocalBoundingBox
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetBoundingBox(this.Handle);
			}
		}
		/// <summary>
		/// Gets or sets the name of the entity.
		/// </summary>
		public string Name
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetName(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetName(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets the name of the entity class.
		/// </summary>
		public string ClassName
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetEntityClassName(this.Handle);
			}
		}
		/// <summary>
		/// Gets or sets the entity flags.
		/// </summary>
		public EntityFlags Flags
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetFlags(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetFlags(this.Handle, value);
			}
		}
		/// <summary>
		/// Gets or sets the material currently assigned to this entity.
		/// </summary>
		public Material Material
		{
			get
			{
				this.AssertObjectValidity();

				return Material.Get(this);
			}
			set
			{
				this.AssertObjectValidity();

				Material.Set(this, value);
			}
		}
		/// <summary>
		/// Gets or sets value that indicates when this entity should receive updates from
		/// the system.
		/// </summary>
		public EntityUpdatePolicy UpdatePolicy
		{
			get
			{
				this.AssertObjectValidity();

				return Native.EntityInterop.GetUpdatePolicy(this.Handle);
			}
			set
			{
				this.AssertObjectValidity();

				Native.EntityInterop.SetUpdatePolicy(this.Handle, value);
			}
		}
		#endregion
		#region Events

		#endregion
		#region Construction
		/// <summary>
		/// Creates a wrapper object for an entity.
		/// </summary>
		/// <param name="handle">Pointer to the entity object.</param>
		public EntityWrapper(IntPtr handle)
		{
			this.Handle = handle;
			this.Identifier = Native.EntityInterop.GetEntityId(this.Handle);
			this.managed = -1;
		}
		/// <summary>
		/// Creates a wrapper object for an entity.
		/// </summary>
		/// <param name="identifier">
		/// Identifier of the entity to create the wrapper for.
		/// </param>
		public EntityWrapper(EntityId identifier)
		{
			this.Handle = Native.EntityInterop.GetEntity(identifier);
			this.Identifier = identifier;
			this.managed = -1;
		}
		/// <summary>
		/// Creates a wrapper object for an entity.
		/// </summary>
		/// <param name="handle">Pointer to the entity object.</param>
		/// <param name="identifier">Identifier of the entity to create the wrapper for.</param>
		public EntityWrapper(IntPtr handle, EntityId identifier)
		{
			this.Handle = handle;
			this.Identifier = identifier;
			this.managed = -1;
		}
		#endregion
		#region Interface
		#region Statics
		#endregion
		#region Physics
		/// <summary>
		/// Gives this entity physical properties.
		/// </summary>
		/// <param name="parameters">
		/// A set of parameters that describe physical properties of the entity.
		/// </param>
		public void Physicalize(PhysicalizationParams parameters)
		{
			this.AssertObjectValidity();

			Native.PhysicsInterop.Physicalize(this.Handle, parameters);

			this.Physics =
				parameters.type == PhysicalizationType.None
				? null
				: PhysicalEntity.TryGet(Native.PhysicsInterop.GetPhysicalEntity(this.Handle));
		}
		/// <summary>
		/// Removes physical properties from this entity.
		/// </summary>
		public void UnPhysicalize()
		{
			this.AssertObjectValidity();

			this.Physicalize(new PhysicalizationParams(PhysicalizationType.None));
		}
		#endregion
		#region Slots
		/// <summary>
		/// Retrieves the flags assigned to the specified slot.
		/// </summary>
		/// <param name="slot">Index of the slot</param>
		/// <returns>The slot flags, or 0 if specified slot is not valid.</returns>
		public EntitySlotFlags GetSlotFlags(int slot = 0)
		{
			this.AssertObjectValidity();

			return Native.EntityInterop.GetSlotFlags(this.Handle, slot);
		}
		/// <summary>
		/// Sets the flags assigned to the specified slot.
		/// </summary>
		/// <param name="flags">Flags to set.</param>
		/// <param name="slot"> 
		/// Index of the slot, if -1 apply to all existing slots.
		/// </param>
		public void SetSlotFlags(EntitySlotFlags flags, int slot = 0)
		{
			this.AssertObjectValidity();

			Native.EntityInterop.SetSlotFlags(this.Handle, slot, flags);
		}
		/// <summary>
		/// Frees the specified slot of all objects.
		/// </summary>
		/// <param name="slot">Zero-based index of the slot to free.</param>
		public void FreeSlot(int slot)
		{
			Native.EntityInterop.FreeSlot(this.Handle, slot);
		}
		#endregion
		#region Static Objects
		/// <summary>
		/// Gets a wrapper around static object located in specified slot.
		/// </summary>
		/// <param name="slot">
		/// Index of the slot where static object we need is located.
		/// </param>
		/// <returns>
		/// A wrapper around static object located in specified slot or a disposed
		/// <see cref="StaticObject"/> instance if slot was empty.
		/// </returns>
		public StaticObject GetStaticObject(int slot)
		{
			this.AssertObjectValidity();

			return new StaticObject(Native.EntityInterop.GetStaticObjectHandle(this.Handle, slot));
		}
		/// <summary>
		/// Assigns a static object to a specified slot.
		/// </summary>
		/// <param name="staticObject">Static object to assign to the entity.</param>
		/// <param name="slot">        
		/// Index of the slot where to put the static object.
		/// </param>
		public void AssignStaticObject(StaticObject staticObject, int slot)
		{
			if (staticObject.Disposed)
			{
				throw new ObjectDisposedException
					("staticObject", "Attempt to assign disposed or invalid static object to the entity.");
			}

			this.AssertObjectValidity();

			Native.EntityInterop.AssignStaticObject(this.Handle, staticObject.Handle, slot);
		}
		#endregion
		/// <summary>
		/// Marks this wrapper as unusable.
		/// </summary>
		public virtual void Dispose()
		{
			this.Handle = IntPtr.Zero;
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AssertObjectValidity()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("EntityWrapper");
			}
		}
		#endregion
	}
}