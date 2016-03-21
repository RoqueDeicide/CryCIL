using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.Engine.Physics;
using CryCil.Geometry;
using CryCil.MemoryMapping;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of types of areas to use when physicalizing entities as areas.
	/// </summary>
	public enum PhysicalAreaType
	{
		/// <summary>
		/// Specifies the area to be a sphere.
		/// </summary>
		Sphere,
		/// <summary>
		/// Specifies the area to be a
		/// </summary>
		Box,
		/// <summary>
		/// Specifies the area to be a geometric object specified by the slot.
		/// </summary>
		Geometry,
		/// <summary>
		/// Specifies the area to be a 2D shape.
		/// </summary>
		Shape,
		/// <summary>
		/// Specifies the area to be a cylinder.
		/// </summary>
		Cylinder,
		/// <summary>
		/// Specifies the area to be a spline-tube.
		/// </summary>
		Spline
	}
	/// <summary>
	/// Encapsulates a set of parameters that define the area.
	/// </summary>
	public unsafe struct AreaDefinition
	{
		#region Fields
		[UsedImplicitly] private PhysicalAreaType physicalAreaType;
		[UsedImplicitly] private float fRadius;
		[UsedImplicitly] private Vector3 boxmin, boxmax;
		[UsedImplicitly] private Vector3* points;
		[UsedImplicitly] private int pointsCount;
		[UsedImplicitly] private float zmin, zmax;
		[UsedImplicitly] private Vector3 center;
		[UsedImplicitly] private Vector3 axis;
		[UsedImplicitly] private PhysicsParametersArea* pGravityParams;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the radius value that is used if the area is a sphere, a cylinder or a spline.
		/// </summary>
		public float Radius
		{
			get { return this.fRadius; }
			set { this.fRadius = value; }
		}
		/// <summary>
		/// Gets or sets the bounding box that is used if this area is a box.
		/// </summary>
		public BoundingBox BoundingBox
		{
			get { return new BoundingBox(this.boxmin, this.boxmax); }
			set
			{
				this.boxmin = value.Minimum;
				this.boxmax = value.Maximum;
			}
		}
		/// <summary>
		/// Sets the array of points that are used if this area is shape or a spline.
		/// </summary>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		/// <exception cref="OverflowException">
		/// The array is multidimensional and contains more than <see cref="F:System.Int32.MaxValue"/>
		/// elements.
		/// </exception>
		public Vector3[] Points
		{
			set
			{
				if (value.IsNullOrEmpty())
				{
					return;
				}

				Vector3* ps = (Vector3*)CryMarshal.Allocate((ulong)(value.Length * sizeof(Vector3)), false).ToPointer();
				fixed (Vector3* valuePtr = value)
				{
					for (int i = 0; i < value.Length; i++)
					{
						ps[i] = valuePtr[i];
					}
				}

				this.points = ps;
				this.pointsCount = value.Length;
			}
		}
		/// <summary>
		/// Gets or sets the minimal Z-coordinate that is used if this area is a shape.
		/// </summary>
		public float MinimalZ
		{
			get { return this.zmin; }
			set { this.zmin = value; }
		}
		/// <summary>
		/// Gets or sets the maximal Z-coordinate that is used if this area is a shape.
		/// </summary>
		public float MaximalZ
		{
			get { return this.zmax; }
			set { this.zmax = value; }
		}
		/// <summary>
		/// Gets or sets the coordinates of the center of the sphere or a base circle of the cylinder.
		/// </summary>
		public Vector3 Center
		{
			get { return this.center; }
			set { this.center = value; }
		}
		/// <summary>
		/// Gets or sets the vector that represents the axis vector that represents the orientation and
		/// height of the cylinder.
		/// </summary>
		public Vector3 Axis
		{
			get { return this.axis; }
			set { this.axis = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="physicalAreaType">Type of area to use.</param>
		/// <param name="gravityParams">   
		/// A valid pointer to a structure that contains the gravity parameters.
		/// </param>
		/// <exception cref="ArgumentOutOfRangeException">Unknown area type.</exception>
		public AreaDefinition(PhysicalAreaType physicalAreaType, PhysicsParametersArea* gravityParams = null)
			: this()
		{
			if (physicalAreaType < PhysicalAreaType.Sphere || physicalAreaType > PhysicalAreaType.Spline)
			{
				throw new ArgumentOutOfRangeException(nameof(physicalAreaType), "Unknown area type.");
			}
			Contract.EndContractBlock();

			this.physicalAreaType = physicalAreaType;
			this.pGravityParams = gravityParams;
		}
		#endregion
		#region Interface
		/// <exception cref="PhysicalizationException">
		/// An array of points must be provided when creating an area definition for a spline or shape area.
		/// </exception>
		internal void Validate()
		{
			if (this.physicalAreaType == PhysicalAreaType.Shape || this.physicalAreaType == PhysicalAreaType.Spline)
			{
				if (this.points == null || this.pointsCount == 0)
				{
					throw new PhysicalizationException(
						"An array of points must be provided when creating an area definition for a spline or shape area.");
				}
			}
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates a set of parameters that specifies how to physicalize the entity or one of its slots.
	/// Don't create objects of this type using default constructor.
	/// </summary>
	public unsafe struct EntityPhysicalizationParameters
	{
		#region Fields
		[UsedImplicitly] private PhysicalEntityType type;
		[UsedImplicitly] private int nSlot;
		[UsedImplicitly] private float density;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private PhysicalEntityFlags nFlagsAnd;
		[UsedImplicitly] private PhysicalEntityFlags nFlagsOr;
		[UsedImplicitly] private int nLod;
		[UsedImplicitly] private PhysicalEntity pAttachToEntity;
		[UsedImplicitly] private int nAttachToPart;
		[UsedImplicitly] private float fStiffnessScale;
		[UsedImplicitly] private bool bCopyJointVelocities;
		[UsedImplicitly] private PhysicsParametersParticle* pParticle;
		[UsedImplicitly] private PhysicsParametersBuoyancy* pBuoyancy;
		[UsedImplicitly] private PhysicsParametersDimensions* pPlayerDimensions;
		[UsedImplicitly] private PhysicsParametersDynamics* pPlayerDynamics;
		[UsedImplicitly] private PhysicsParametersVehicle* pCar;

		[UsedImplicitly] private AreaDefinition* pAreaDef;
		[UsedImplicitly] private string szPropsOverride;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the zero-based index of the slot to physicalize. Can be
		/// <see cref="CryEntitySlot.All"/> to designate all slots.
		/// </summary>
		[DefaultValue(CryEntitySlot.All)]
		public int Slot
		{
			get { return this.nSlot; }
			set { this.nSlot = value; }
		}
		/// <summary>
		/// Gets the mass of this entity. Setting this property invalidates the value in
		/// <see cref="Density"/>.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
			set
			{
				this.mass = value;
				this.density = 0;
			}
		}
		/// <summary>
		/// Gets the density of this entity. Setting this property invalidates the value in
		/// <see cref="Mass"/>.
		/// </summary>
		public float Density
		{
			get { return this.density; }
			set
			{
				this.density = value;
				this.mass = 0;
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that will applied to the flags of the physical entity using bitwise
		/// Or operation.
		/// </summary>
		/// <remarks>
		/// Can be used to add flags to physical entities in case the entity or a slot is already
		/// physicalized.
		/// </remarks>
		public PhysicalEntityFlags FlagsOr
		{
			get { return this.nFlagsOr; }
			set { this.nFlagsOr = value; }
		}
		/// <summary>
		/// Gets or sets a set of flags that will applied to the flags of the physical entity using bitwise
		/// And operation.
		/// </summary>
		/// <remarks>
		/// Can be used to remove flags from physical entities in case the entity or a slot is already
		/// physicalized.
		/// </remarks>
		public PhysicalEntityFlags FlagsAnd
		{
			get { return this.nFlagsAnd; }
			set { this.nFlagsAnd = value; }
		}
		/// <summary>
		/// Gets or sets the zero-based index of the LOD model to use physics from. Used by rag-doll
		/// characters that store the physics info in Lod 1.
		/// </summary>
		public int Lod
		{
			get { return this.nLod; }
			set { this.nLod = value; }
		}
		/// <summary>
		/// Gets or sets the entity to attach this entity to, if this entity is a soft body.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign an entity to attach this one to, if this one is not a soft body.
		/// </exception>
		public PhysicalEntity AttachToEntity
		{
			get { return this.pAttachToEntity; }
			set
			{
				if (this.type != PhysicalEntityType.Soft)
				{
					throw new InvalidOperationException(
						"Cannot assign an entity to attach this one to, if this one is not a soft body.");
				}
				this.pAttachToEntity = value;
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the part of <see cref="AttachToEntity"/> to attach this one to.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign an entity to attach this one to, if this one is not a soft body.
		/// </exception>
		public int AttachToPart
		{
			get { return this.nAttachToPart; }
			set
			{
				if (this.type != PhysicalEntityType.Soft)
				{
					throw new InvalidOperationException(
						"Cannot assign an entity to attach this one to, if this one is not a soft body.");
				}
				this.nAttachToPart = value;
			}
		}
		/// <summary>
		/// Gets or sets the scale of forces in joint springs.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign a stiffness scale to entity that is not an articulated body.
		/// </exception>
		public float StiffnessScale
		{
			get { return this.fStiffnessScale; }
			set
			{
				if (this.type != PhysicalEntityType.Articulated)
				{
					throw new InvalidOperationException("Cannot assign a stiffness scale to entity that is not an articulated body.");
				}
				this.fStiffnessScale = value;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates whether velocity of joints must be copied when converting
		/// character to rag-doll.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign a stiffness scale to entity that is not an articulated body.
		/// </exception>
		public bool CopyJointVelocities
		{
			get { return this.bCopyJointVelocities; }
			set
			{
				if (this.type != PhysicalEntityType.Articulated)
				{
					throw new InvalidOperationException("Cannot assign a stiffness scale to entity that is not an articulated body.");
				}
				this.bCopyJointVelocities = value;
			}
		}
		/// <summary>
		/// Gets or sets the pointer to a set of parameters that specify dimensions of this entity, if it's
		/// a living entity.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign dimensions to the entity that is not a living entity.
		/// </exception>
		public PhysicsParametersDimensions* Dimensions
		{
			get { return this.pPlayerDimensions; }
			set
			{
				if (this.type != PhysicalEntityType.Living)
				{
					throw new InvalidOperationException("Cannot assign dimensions to the entity that is not a living entity.");
				}
				this.pPlayerDimensions = value;
			}
		}
		/// <summary>
		/// Gets or sets the pointer to a set of parameters that specify dynamics parameters of this entity,
		/// if it's a living entity.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign dynamics parameters to the entity that is not a living entity.
		/// </exception>
		public PhysicsParametersDynamics* Dynamics
		{
			get { return this.pPlayerDynamics; }
			set
			{
				if (this.type != PhysicalEntityType.Living)
				{
					throw new InvalidOperationException("Cannot assign dynamics parameters to the entity that is not a living entity.");
				}
				this.pPlayerDynamics = value;
			}
		}
		/// <summary>
		/// Gets or sets the pointer to a set of parameters that specify buoyancy of this entity.
		/// </summary>
		public PhysicsParametersBuoyancy* BuoyancyParameters
		{
			get { return this.pBuoyancy; }
			set { this.pBuoyancy = value; }
		}
		/// <summary>
		/// Gets or sets the pointer to a set of parameters that specify this entity, if it's a particle.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign a particle parameters to entity that is not a particle.
		/// </exception>
		public PhysicsParametersParticle* ParticleParameters
		{
			get { return this.pParticle; }
			set
			{
				if (this.type != PhysicalEntityType.Particle)
				{
					throw new InvalidOperationException("Cannot assign a particle parameters to entity that is not a particle.");
				}
				this.pParticle = value;
			}
		}
		/// <summary>
		/// Gets or sets the pointer to a set of parameters that specify this entity, if it's a vehicle.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign a vehicle parameters to entity that is not a vehicle.
		/// </exception>
		public PhysicsParametersVehicle* VehicleParameters
		{
			get { return this.pCar; }
			set
			{
				if (this.type != PhysicalEntityType.Particle)
				{
					throw new InvalidOperationException("Cannot assign a vehicle parameters to entity that is not a vehicle.");
				}
				this.pCar = value;
			}
		}
		/// <summary>
		/// Gets or sets a pointer to the object that defines an area, if this entity is an area. This
		/// property has to be assigned with a valid pointer when physicalizing as
		/// <see cref="PhysicalEntityType.Area"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// Cannot assign an area definition parameters to entity that is not an area.
		/// </exception>
		public AreaDefinition* AreaDefinition
		{
			get { return this.pAreaDef; }
			set
			{
				if (this.type != PhysicalEntityType.Area)
				{
					throw new InvalidOperationException("Cannot assign an area definition parameters to entity that is not an area.");
				}
				this.pAreaDef = value;
			}
		}
		/// <summary>
		/// Gets or sets and optional string that contains overrides for static object properties.
		/// </summary>
		public string StaticPropertiesOverride
		{
			get { return this.szPropsOverride; }
			set { this.szPropsOverride = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new valid object of this type.
		/// </summary>
		/// <param name="type">Type of physical entity to use.</param>
		public EntityPhysicalizationParameters(PhysicalEntityType type)
		{
			this.type = type;
			this.nSlot = CryEntitySlot.All;
			this.density = 0;
			this.mass = 0;
			this.nFlagsAnd = (PhysicalEntityFlags)new Bytes4(uint.MaxValue).SignedInt;
			this.nFlagsOr = 0;
			this.nLod = 0;
			this.pAttachToEntity = new PhysicalEntity();
			this.nAttachToPart = -1;
			this.fStiffnessScale = 0;
			this.bCopyJointVelocities = false;
			this.pParticle = null;
			this.pBuoyancy = null;
			this.pPlayerDimensions = null;
			this.pPlayerDynamics = null;
			this.pCar = null;
			this.pAreaDef = null;
			this.szPropsOverride = null;
		}
		#endregion
		#region Interface
		/// <exception cref="PhysicalizationException">
		/// Physicalization of entity as area requires a valid pointer to AreaDefinition structure.
		/// </exception>
		internal void Validate()
		{
			if (this.type == PhysicalEntityType.Area)
			{
				if (this.pAreaDef == null)
				{
					throw new PhysicalizationException(
						"Physicalization of entity as area requires a valid pointer to AreaDefinition structure.");
				}
				this.pAreaDef->Validate();
			}
		}
		#endregion
		#region Utilities
		#endregion
	}
}