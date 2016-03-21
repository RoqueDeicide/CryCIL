using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents a CryEngine physical entity.
	/// </summary>
	public struct PhysicalEntity
	{
		#region Fields
		private readonly IntPtr handle;
		/// <summary>
		/// An object that represents the world physical entity. Cannot be manipulated directly, but can be
		/// passed to certain objects.
		/// </summary>
		public static readonly PhysicalEntity World = new PhysicalEntity(new IntPtr(-10));
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero && this.handle != new IntPtr(-10);
		/// <summary>
		/// Gets or sets identifier of this physical entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Identifier
		{
			get
			{
				this.AssertInstance();

				return GetPhysicalEntityId(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetPhysicalEntityId(this.handle, value);
			}
		}
		/// <summary>
		/// Gets the type of this entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntityType Type
		{
			get
			{
				this.AssertInstance();

				return GetPhysicalType(this.handle);
			}
		}
		#endregion
		#region Construction
		internal PhysicalEntity(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Gets the physical entity.
		/// </summary>
		/// <param name="id">Identifier of the entity to get.</param>
		public PhysicalEntity(int id)
		{
			this.handle = GetPhysicalEntityById(id);
		}
		/// <summary>
		/// Creates a new physical entity or a place holder for it.
		/// </summary>
		/// <param name="type">         Type of physical entity to create.</param>
		/// <param name="parameters">   
		/// Reference to the base part of the object that provides initial parameters for the new entity's
		/// simulation.
		/// </param>
		/// <param name="foreignData">  
		/// An optional object that specifies the external data that is associated with a new entity.
		/// </param>
		/// <param name="id">           
		/// An optional identifier of the new entity. If not specified an id will be generated
		/// automatically. Avoid using big numbers, since there is a simple array-based map id to entity.
		/// </param>
		/// <param name="isPlaceHolder">
		/// Indicates whether we are creating a place-holder for temporary entities.
		/// </param>
		public PhysicalEntity(PhysicalEntityType type, ref PhysicsParameters parameters,
							  ForeignData foreignData = new ForeignData(), int id = -1, bool isPlaceHolder = false)
		{
			this.handle =
				isPlaceHolder
					? PhysicalWorld.CreatePlaceHolder(type, ref parameters, foreignData, id)
					: PhysicalWorld.CreatePhysicalEntity(type, ref parameters, foreignData, id);
		}
		/// <summary>
		/// Creates a new physical entity or a place holder for it.
		/// </summary>
		/// <param name="type">         Type of physical entity to create.</param>
		/// <param name="foreignData">  
		/// An optional object that specifies the external data that is associated with a new entity.
		/// </param>
		/// <param name="id">           
		/// An optional identifier of the new entity. If not specified an id will be generated
		/// automatically. Avoid using big numbers, since there is a simple array-based map id to entity.
		/// </param>
		/// <param name="isPlaceHolder">
		/// Indicates whether we are creating a place-holder for temporary entities.
		/// </param>
		public PhysicalEntity(PhysicalEntityType type, ForeignData foreignData = new ForeignData(), int id = -1,
							  bool isPlaceHolder = false)
		{
			this.handle =
				isPlaceHolder
					? PhysicalWorld.CreatePlaceHolderNoParams(type, foreignData, id)
					: PhysicalWorld.CreatePhysicalEntityNoParams(type, foreignData, id);
		}
		/// <summary>
		/// Creates a temporary entity.
		/// </summary>
		/// <param name="type">       Type of physical entity to create.</param>
		/// <param name="lifeTime">   
		/// If <paramref name="placeHolder"/> is not specified then this parameter defines the time interval
		/// before new entity will be destroyed, otherwise its the time that must pass since last
		/// interaction before entity is converted back into place-holder.
		/// </param>
		/// <param name="parameters"> 
		/// Reference to the base part of the object that provides initial parameters for the new entity's
		/// simulation.
		/// </param>
		/// <param name="foreignData">
		/// An optional object that specifies the external data that is associated with a new entity.
		/// </param>
		/// <param name="id">         
		/// An optional identifier of the new entity. If not specified an id will be generated
		/// automatically. Avoid using big numbers, since there is a simple array-based map id to entity.
		/// </param>
		/// <param name="placeHolder">
		/// An optional object that represents a place-holder to create the entity from.
		/// </param>
		public PhysicalEntity(PhysicalEntityType type, float lifeTime, ref PhysicsParameters parameters,
							  ForeignData foreignData = new ForeignData(), int id = -1,
							  PhysicalEntity placeHolder = new PhysicalEntity())
		{
			this.handle = PhysicalWorld.CreatePhysicalEntityFromHolder(type, lifeTime, ref parameters, foreignData, id,
																	   placeHolder);
		}
		/// <summary>
		/// Creates a temporary entity.
		/// </summary>
		/// <param name="type">       Type of physical entity to create.</param>
		/// <param name="lifeTime">   
		/// If <paramref name="placeHolder"/> is not specified then this parameter defines the time interval
		/// before new entity will be destroyed, otherwise its the time that must pass since last
		/// interaction before entity is converted back into place-holder.
		/// </param>
		/// <param name="foreignData">
		/// An optional object that specifies the external data that is associated with a new entity.
		/// </param>
		/// <param name="id">         
		/// An optional identifier of the new entity. If not specified an id will be generated
		/// automatically. Avoid using big numbers, since there is a simple array-based map id to entity.
		/// </param>
		/// <param name="placeHolder">
		/// An optional object that represents a place-holder to create the entity from.
		/// </param>
		public PhysicalEntity(PhysicalEntityType type, float lifeTime, ForeignData foreignData = new ForeignData(),
							  int id = -1, PhysicalEntity placeHolder = new PhysicalEntity())
		{
			this.handle = PhysicalWorld.CreatePhysicalEntityNoParamsFromHolder(type, lifeTime, foreignData, id,
																			   placeHolder);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Sets parameters for this entity.
		/// </summary>
		/// <param name="parameters">
		/// A reference to a base part of the structure that encapsulates parameters to set.
		/// </param>
		/// <param name="threadSafe">
		/// An optional value that indicates whether these parameters must be set immediately in a
		/// thread-safe manner, rather then after undefined amount of time when safe.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents a set of parameters to set was created via default constructor which
		/// is not allowed.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool SetParameters(ref PhysicsParameters parameters, bool threadSafe = false)
		{
			this.AssertInstance();
			if (!parameters.Initialized)
			{
				throw new ArgumentNullException(nameof(parameters),
												"An object that represents a set of parameters to set was created via default constructor which is not allowed.");
			}

			return SetParams(this.handle, ref parameters, threadSafe) != 0;
		}
		/// <summary>
		/// Gets parameters that were previously assigned for this entity.
		/// </summary>
		/// <param name="parameters">
		/// A reference to a base part of the structure that encapsulates parameters to set.
		/// </param>
		/// <returns>True, if successful.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents a set of parameters to get was created via default constructor which
		/// is not allowed.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Don't use <see cref="PhysicsParametersLocation"/> structure when getting parameters, call
		/// <see cref="GetStatus"/> with <see cref="PhysicsStatusLocation"/> instead.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Unknown type of physics parameters was used.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool GetParameters(ref PhysicsParameters parameters)
		{
			this.AssertInstance();
			if (!parameters.Initialized)
			{
				throw new ArgumentNullException(nameof(parameters),
												"An object that represents a set of parameters to get was created via default constructor which is not allowed.");
			}
			switch (parameters.Type)
			{
				case PhysicsParametersTypes.Position:
					throw new ArgumentException(
						"Don't use PhysicsParametersLocation structure when getting parameters, call GetStatus with PhysicsStatusLocation instead.");
				case PhysicsParametersTypes.PlayerDimensions:
					break;
				case PhysicsParametersTypes.Vehicle:
					break;
				case PhysicsParametersTypes.Particle:
					break;
				case PhysicsParametersTypes.PlayerDynamics:
					break;
				case PhysicsParametersTypes.Joint:
					break;
				case PhysicsParametersTypes.Part:
					break;
				case PhysicsParametersTypes.Sensors:
					break;
				case PhysicsParametersTypes.ArticulatedBody:
					break;
				case PhysicsParametersTypes.OuterEntity:
					break;
				case PhysicsParametersTypes.Simulation:
					break;
				case PhysicsParametersTypes.ForeignData:
					break;
				case PhysicsParametersTypes.Buoyancy:
					break;
				case PhysicsParametersTypes.Rope:
					break;
				case PhysicsParametersTypes.BoundingBox:
					break;
				case PhysicsParametersTypes.Flags:
					break;
				case PhysicsParametersTypes.Wheel:
					break;
				case PhysicsParametersTypes.SoftBody:
					break;
				case PhysicsParametersTypes.Area:
					break;
				case PhysicsParametersTypes.TetraLattice:
					break;
				case PhysicsParametersTypes.GroundPlane:
					break;
				case PhysicsParametersTypes.StructuralJoint:
					break;
				case PhysicsParametersTypes.WaterMananger:
					break;
				case PhysicsParametersTypes.Timeout:
					break;
				case PhysicsParametersTypes.Skeleton:
					break;
				case PhysicsParametersTypes.StructuralInitialVelocity:
					break;
				case PhysicsParametersTypes.CollisionClass:
					break;
				case PhysicsParametersTypes.Count:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(parameters), "Unknown type of physics parameters was used.");
			}

			return GetParams(this.handle, ref parameters) != 0;
		}
		/// <summary>
		/// Queries the status of this physical entity.
		/// </summary>
		/// <param name="status">
		/// A reference to the base part of the object that defines what query to do and will contain the
		/// results.
		/// </param>
		/// <returns>An integer number which meaning depends on the query.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents information to query was created via default constructor which is not
		/// allowed.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int GetStatus(ref PhysicsStatus status)
		{
			this.AssertInstance();
			if (!status.Initialized)
			{
				throw new ArgumentNullException(nameof(status),
												"An object that represents information to query was created via default constructor which is not allowed.");
			}

			return GetStatusInternal(this.handle, ref status);
		}
		/// <summary>
		/// Executes an action upon this physical entity.
		/// </summary>
		/// <param name="action">    
		/// A reference to the base part of the object that describes the action.
		/// </param>
		/// <param name="threadSafe">
		/// An optional value that indicates whether this action must be executed immediately in a
		/// thread-safe manner, rather then after undefined amount of time when safe.
		/// </param>
		/// <returns>True, if action was executed or queued successfully.</returns>
		/// <exception cref="ArgumentNullException">
		/// An object that represents an action to execute was created via default constructor which is not
		/// allowed.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool ActUpon(ref PhysicsAction action, bool threadSafe = false)
		{
			this.AssertInstance();
			if (!action.Initialized)
			{
				throw new ArgumentNullException(nameof(action),
												"An object that represents an action to execute was created via default constructor which is not allowed.");
			}

			return Action(this.handle, ref action, threadSafe) != 0;
		}
		/// <summary>
		/// Adds a part to this physical entity.
		/// </summary>
		/// <param name="body">      A physical body that represents the part.</param>
		/// <param name="parameters">
		/// A reference to the base part of the object that provides a set of parameters that specify the
		/// body.
		/// </param>
		/// <param name="id">        
		/// An identifier to assign to the part. Expected to be unique within the entity. If default value
		/// of -1 is passed, the identifier is assigned automatically.
		/// </param>
		/// <param name="threadSafe">
		/// Indicates whether part must be created at the end of simulation step, rather then immediately
		/// which makes it more thread-safe.
		/// </param>
		/// <returns>An identifier that was assigned to the part, or -1, if creation has failed.</returns>
		/// <exception cref="ArgumentNullException">
		/// The object that represents the physical body must be valid.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The object that represents parameters that specify the physical body must not be created through
		/// default constructor.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int AddBody(PhysicalBody body, ref GeometryParameters parameters, int id = -1, bool threadSafe = false)
		{
			this.AssertInstance();
			if (!body.IsValid)
			{
				throw new ArgumentNullException(nameof(body), "The object that represents the physical body must be valid.");
			}
			if (!parameters.Initialized)
			{
				throw new ArgumentException(
					"The object that represents parameters that specify the physical body must not be created through default constructor.",
					nameof(parameters));
			}

			return AddGeometry(this.handle, body, ref parameters, id, threadSafe);
		}
		/// <summary>
		/// Removes a part of the entity.
		/// </summary>
		/// <param name="id">        An identifier of the part to remove.</param>
		/// <param name="threadSafe">
		/// Indicates whether part must be created at the end of simulation step, rather then immediately
		/// which makes it more thread-safe.
		/// </param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void RemoveBody(int id, bool threadSafe = false)
		{
			this.AssertInstance();

			RemoveGeometry(this.handle, id, threadSafe);
		}
		/// <summary>
		/// "Collides" this entity with a sphere that moves in a certain direction.
		/// </summary>
		/// <param name="ray">   
		/// An object that describes the trajectory of sphere's movement. To detect collisions properly the
		/// <see cref="Ray.Position"/> must be further then <paramref name="radius"/> away from this entity.
		/// </param>
		/// <param name="radius">Radius of the sphere.</param>
		/// <param name="hit">   
		/// An object that describes the hit, if this method returns <c>true</c>.
		/// </param>
		/// <returns>A value that indicates whether a beam has collided with this entity.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Radius of the sphere cannot be less then 0.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		[Annotations.Pure]
		public bool CollideWithBeam(Ray ray, float radius, out RayHit hit)
		{
			this.AssertInstance();
			if (radius < MathHelpers.NZeroTolerance)
			{
				throw new ArgumentOutOfRangeException(nameof(radius), "Radius of the sphere cannot be less then 0.");
			}

			return CollideEntityWithBeam(this.handle, ref ray.Position, ref ray.Direction, radius, out hit);
		}
		/// <summary>
		/// Destroys this entity.
		/// </summary>
		/// <param name="mode">      An object that specifies how to destroy the entity.</param>
		/// <param name="threadSafe">
		/// Indicates whether destruction must be done in a thread-safe manner.
		/// </param>
		/// <returns>
		/// True, if operation was successful. False can only be returned if <paramref name="mode"/> is
		/// equal to <see cref="PhysicalEntityRemovalMode.AttemptDeletion"/> and this entity has non-zero
		/// reference count.
		/// </returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Destroy(PhysicalEntityRemovalMode mode = PhysicalEntityRemovalMode.Destroy, bool threadSafe = false)
		{
			this.AssertInstance();

			return PhysicalWorld.DestroyPhysicalEntity(this.handle, (int)mode, threadSafe ? 1 : 0) != 0;
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
		private static extern PhysicalEntityType GetPhysicalType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SetParams(IntPtr handle, ref PhysicsParameters parameters, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetParams(IntPtr handle, ref PhysicsParameters parameters);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetStatusInternal(IntPtr handle, ref PhysicsStatus status);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Action(IntPtr handle, ref PhysicsAction action, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int AddGeometry(IntPtr handle, PhysicalBody pgeom, ref GeometryParameters parameters,
											  int id, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RemoveGeometry(IntPtr handle, int id, bool threadSafe);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CollideEntityWithBeam(IntPtr handle, ref Vector3 org, ref Vector3 dir, float r,
														 out RayHit phit);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int SetPhysicalEntityId(IntPtr handle, int id, int bReplace = 1, int bThreadSafe = 0);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPhysicalEntityId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetPhysicalEntityById(int id);
		#endregion
	}
}