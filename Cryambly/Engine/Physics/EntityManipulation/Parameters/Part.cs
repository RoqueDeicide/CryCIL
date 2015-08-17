using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Logic;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to get and set parameters for the part of the physical
	/// entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct PhysicsParametersPart
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParams"/> to apply these onto
		/// physical entity or to <see cref="PhysicalEntity.GetParams"/> to get the currently applied ones.
		/// </summary>
		[UsedImplicitly]
		public PhysicsParameters Base;
		// Indicates whether this object is set up for assignment of parameters.
		private readonly bool forAssignment;
		private readonly int partid;
		private readonly int ipart;
		private bool bRecalcBBox;
		private Quatvecale location;
		private Matrix34 pMtx3x4;
		private uint flagsCond;
		private uint flagsOR, flagsAND;
		private uint flagsColliderOR, flagsColliderAND;
		private float mass;
		private float density;
		private float minContactDist;
		private RegisteredPhysicalGeometry pPhysGeom, pPhysGeomProxy;
		private int idmatBreakable;
		private TetraLattice pLattice;
		private int idSkeleton;
		private readonly int[] matMappings;
		private int* pMatMapping;
		private int nMats;
		private int idParent;
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of the part these parameters will be assigned to or were taken from.
		/// </summary>
		/// <returns>- 1, if this object works with multiple parts.</returns>
		public int PartIdentifier
		{
			get { return this.partid; }
		}
		/// <summary>
		/// Gets zero-based index of the part these parameters will be assigned to or were taken from.
		/// </summary>
		/// <returns>- 1, if this object works with multiple parts.</returns>
		public int PartIndex
		{
			get { return this.ipart; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether bounding box the part must be recalculated.
		/// </summary>
		public bool RecalculateBoundingBox
		{
			get { return this.bRecalcBBox; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.bRecalcBBox = value;
			}
		}
		/// <summary>
		/// Gets or sets position of the part.
		/// </summary>
		public Vector3 Position
		{
			get { return this.location.Translation; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.location.Translation = value;
			}
		}
		/// <summary>
		/// Gets or sets orientation of the part.
		/// </summary>
		public Quaternion Orientation
		{
			get { return this.location.Orientation; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.location.Orientation = value;
			}
		}
		/// <summary>
		/// Gets or sets scale of the part.
		/// </summary>
		public float Scale
		{
			get { return this.location.Scale; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.location.Scale = value;
			}
		}
		/// <summary>
		/// Gets or sets an object that represents position, orientation and scale of the part.
		/// </summary>
		public Quatvecale Location
		{
			get { return this.location; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.location = value;
			}
		}
		/// <summary>
		/// Gets or sets the matrix that represents position, orientation and scale of the part.
		/// </summary>
		public Matrix34 Transformation
		{
			get { return this.pMtx3x4; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.pMtx3x4 = value;
			}
		}
		/// <summary>
		/// Sets the object that can specify: <list type="number"><item>A set of flags that must be set on
		/// the part for the parameters to be assigned to it.</item><item>2 sets of flags that are used to
		/// modify the flags that are assigned to the part.</item><item>2 sets of flags that are used to
		/// modify the collision flags that are assigned to the part.</item></list>
		/// </summary>
		public PartFlags Flags
		{
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				if (value.HasFlagCond)
				{
					this.flagsCond = value.FlagsCond;
				}
				if (value.HasModFlags)
				{
					this.flagsOR = value.FlagsOr;
					this.flagsAND = value.FlagsAnd;
				}
				if (value.HasColliderModFlags)
				{
					this.flagsColliderOR = value.FlagsColliderOr;
					this.flagsColliderAND = value.FlagsColliderAnd;
				}
			}
		}
		/// <summary>
		/// Gets or sets the mass of the part in kilograms.
		/// </summary>
		/// <remarks>
		/// Assigning this property will clear the value of <see cref="Density"/> property.
		/// </remarks>
		public float Mass
		{
			get { return this.mass; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.mass = value;
				this.density = UnusedValue.Single;
			}
		}
		/// <summary>
		/// Gets or sets the density of the part in kilograms per cubic meter.
		/// </summary>
		/// <remarks>Assigning this property will clear the value of <see cref="Mass"/> property.</remarks>
		public float Density
		{
			get { return this.density; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.density = value;
				this.mass = UnusedValue.Single;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates how close another entity must be to this part, to make
		/// the part report the contact.
		/// </summary>
		public float MinimalContactDistance
		{
			get { return this.minContactDist; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.minContactDist = value;
			}
		}
		/// <summary>
		/// Gets or sets the geometry that is used by the part for ray-tracing.
		/// </summary>
		/// <remarks>
		/// Just assign this property without assigning <see cref="GeometryProxy"/> if you want the part to
		/// use the same geometry to be used for both ray-tracing and physical interactions.
		/// </remarks>
		public RegisteredPhysicalGeometry Geometry
		{
			get { return this.pPhysGeom; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.pPhysGeom = value;
			}
		}
		/// <summary>
		/// Gets or sets the geometry that is used by the part for physical interactions.
		/// </summary>
		/// <remarks>
		/// Just assign this property without assigning <see cref="Geometry"/> if you want the part to use
		/// the same geometry to be used for both ray-tracing and physical interactions.
		/// </remarks>
		public RegisteredPhysicalGeometry GeometryProxy
		{
			get { return this.pPhysGeomProxy; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.pPhysGeomProxy = value;
			}
		}
		/// <summary>
		/// Gets or sets the breakability index that can be acquired from
		/// <see cref="ExplosionShapes.Add"/>.
		/// </summary>
		public int BreakabilityIndex
		{
			get { return this.idmatBreakable; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.idmatBreakable = value;
			}
		}
		/// <summary>
		/// Gets or sets the object that specifies tetrahedral lattice that is used by soft bodies and
		/// procedurally breakable objects.
		/// </summary>
		public TetraLattice Lattice
		{
			get { return this.pLattice; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.pLattice = value;
			}
		}
		/// <summary>
		/// Gets or sets identifier of the part of this entity that is used by this part as a deformation
		/// skeleton.
		/// </summary>
		public int SkeletonId
		{
			get { return this.idSkeleton; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.idSkeleton = value;
			}
		}
		/// <summary>
		/// Gets an array of surface type mappings that are used the geometry of this part.
		/// </summary>
		public int[] SurfaceTypesMapping
		{
			get { return this.matMappings; }
		}
		/// <summary>
		/// Takes a table of surface type identifiers from the material that should be taken from the main
		/// <see cref="CryEntity"/> object and puts into this object.
		/// </summary>
		public Material MaterialSurfaceTypesMapping
		{
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.pMatMapping = value.FillSurfaceTypesTable(out this.nMats);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the part of the entity that must be broken in order for this
		/// part to show up.
		/// </summary>
		public int BreakageParentId
		{
			get { return this.idParent; }
			set
			{
				this.AssertAssignment();
				Contract.EndContractBlock();

				this.idParent = value;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type that can be used to get/set parameters of the part (or all
		/// parts) of the entity.
		/// </summary>
		/// <param name="part">
		/// An object that specify the part. <see cref="EntityPartSpec.AllParts"/> can be used to specify
		/// all parts.
		/// </param>
		public PhysicsParametersPart(EntityPartSpec part)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Part);
			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
			else
			{
				this.partid = UnusedValue.Int32;
				this.ipart = UnusedValue.Int32;
			}
			this.forAssignment = true;

			this.bRecalcBBox = true;
			this.pMtx3x4 = Matrix34.Identity;
			this.location = new Quatvecale(UnusedValue.Quaternion, UnusedValue.Vector, UnusedValue.Single);
			this.flagsCond = UnusedValue.UInt32;
			this.flagsOR = 0;
			this.flagsAND = uint.MaxValue;
			this.flagsColliderOR = 0;
			this.flagsColliderAND = uint.MaxValue;
			this.mass = UnusedValue.Single;
			this.density = UnusedValue.Single;
			this.minContactDist = UnusedValue.Single;
			this.idmatBreakable = UnusedValue.Int32;
			this.pLattice = new TetraLattice(UnusedValue.Pointer);
			this.idSkeleton = UnusedValue.Int32;
			this.matMappings = null;
			this.pMatMapping = (int*)UnusedValue.Pointer.ToPointer();
			this.nMats = UnusedValue.Int32;
			this.idParent = UnusedValue.Int32;
			this.pPhysGeom = new RegisteredPhysicalGeometry(UnusedValue.Pointer);
			this.pPhysGeomProxy = new RegisteredPhysicalGeometry(UnusedValue.Pointer);
		}
		/// <summary>
		/// Creates a new object of this type that can be used to get/set parameters of one (or more)
		/// part(s) of the entity.
		/// </summary>
		/// <param name="flags">
		/// A value that designates the flags that must be set for parts to have these parameters applied
		/// to them.
		/// </param>
		public PhysicsParametersPart(uint flags)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Part);
			this.flagsCond = flags;
			this.forAssignment = true;

			this.partid = UnusedValue.Int32;
			this.ipart = UnusedValue.Int32;
			this.bRecalcBBox = true;
			this.pMtx3x4 = Matrix34.Identity;
			this.location = new Quatvecale(UnusedValue.Quaternion, UnusedValue.Vector, UnusedValue.Single);
			this.flagsOR = 0;
			this.flagsAND = uint.MaxValue;
			this.flagsColliderOR = 0;
			this.flagsColliderAND = uint.MaxValue;
			this.mass = UnusedValue.Single;
			this.density = UnusedValue.Single;
			this.minContactDist = UnusedValue.Single;
			this.idmatBreakable = UnusedValue.Int32;
			this.pLattice = new TetraLattice(UnusedValue.Pointer);
			this.idSkeleton = UnusedValue.Int32;
			this.matMappings = null;
			this.pMatMapping = (int*)UnusedValue.Pointer.ToPointer();
			this.nMats = UnusedValue.Int32;
			this.idParent = UnusedValue.Int32;
			this.pPhysGeom = new RegisteredPhysicalGeometry(UnusedValue.Pointer);
			this.pPhysGeomProxy = new RegisteredPhysicalGeometry(UnusedValue.Pointer);
		}
		/// <summary>
		/// Creates a copy of parameters to be reassigned to the same part.
		/// </summary>
		/// <param name="parameters">A set of parameters to copy.</param>
		public PhysicsParametersPart(ref PhysicsParametersPart parameters)
		{
			this = parameters;
			this.forAssignment = true;
		}
		/// <summary>
		/// Creates a copy of parameters to be reassigned to another part.
		/// </summary>
		/// <param name="part">      
		/// An object that specify the new part to apply the parameters to.
		/// </param>
		/// <param name="parameters">A set of parameters to copy.</param>
		public PhysicsParametersPart(EntityPartSpec part, ref PhysicsParametersPart parameters)
		{
			this = parameters;
			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
			else
			{
				this.partid = UnusedValue.Int32;
				this.ipart = UnusedValue.Int32;
			}
			this.forAssignment = true;
		}
		/// <summary>
		/// Creates a copy of parameters to be reassigned to another part.
		/// </summary>
		/// <param name="flags">     
		/// A value that designates the flags that must be set for parts to have these parameters applied
		/// to them.
		/// </param>
		/// <param name="parameters">A set of parameters to copy.</param>
		public PhysicsParametersPart(uint flags, ref PhysicsParametersPart parameters)
		{
			this = parameters;
			this.flagsCond = flags;
			this.partid = UnusedValue.Int32;
			this.ipart = UnusedValue.Int32;
			this.forAssignment = true;
		}
		#endregion
		#region Utility
		private void AssertAssignment()
		{
			if (!this.forAssignment)
			{
				throw new InvalidOperationException(
					"Attempt was made to change an object that represents a set of parameters that represents parameters that were taken from the part of the physical entity. Pass this object to one of the constructors to create the copy that can be assigned.");
			}
		}
		#endregion
	}
}