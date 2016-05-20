using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Logic;
using CryCil.Engine.Memory;
using CryCil.Engine.Rendering;
using CryCil.Geometry;
using CryCil.RunTime;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that specify the geometry that is used by the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct GeometryParameters
	{
		#region Fields
		private readonly bool initialized;
		[UsedImplicitly] internal PhysicsGeometryParametersTypes type;
		[UsedImplicitly] private float density;
		[UsedImplicitly] private float mass;
		[UsedImplicitly] private Vector3 pos;
		[UsedImplicitly] private Quaternion q;
		[UsedImplicitly] private float scale;
		[UsedImplicitly] private Matrix34 pMtx3x4;
		[UsedImplicitly] private int surface_idx;
		[UsedImplicitly] private PhysicsGeometryFlags flags;
		[UsedImplicitly] private ColliderTypes flagsCollider;
		[UsedImplicitly] private float minContactDist;
		[UsedImplicitly] private int idmatBreakable;
		[UsedImplicitly] private TetraLattice pLattice;
		[UsedImplicitly] private int[] matMappings;
		[UsedImplicitly] private int* pMatMapping;
		[UsedImplicitly] private int nMats;
		[UsedImplicitly] private int bRecalcBBox;
		#endregion
		#region Properties
		internal bool Initialized => this.initialized;
		/// <summary>
		/// Gets the value that indicates whether a bounding box should be recalculated for this entity.
		/// </summary>
		public bool RacalculateBounds
		{
			get { return this.bRecalcBBox != 0; }
			set { this.bRecalcBBox = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets of sets the density of the body. If you set this property, <see cref="Mass"/> will be
		/// invalidated.
		/// </summary>
		public float Density
		{
			get { return this.density; }
			set { this.density = value; }
		}
		/// <summary>
		/// Gets of sets the mass of the body. If you set this property, <see cref="Density"/> will be
		/// invalidated.
		/// </summary>
		public float Mass
		{
			get { return this.mass; }
			set { this.mass = value; }
		}
		/// <summary>
		/// Gets or sets position of this geometry relative to the physical entity or parent articulated
		/// body part.
		/// </summary>
		public Vector3 Position
		{
			get { return this.pos; }
			set { this.pos = value; }
		}
		/// <summary>
		/// Gets or sets orientation of this geometry relative to the physical entity or parent articulated
		/// body part.
		/// </summary>
		public Quaternion Orientation
		{
			get { return this.q; }
			set { this.q = value; }
		}
		/// <summary>
		/// Gets or sets the scale of the geometry.
		/// </summary>
		public float Scale
		{
			get { return this.scale; }
			set { this.scale = value; }
		}
		/// <summary>
		/// Gets or sets the matrix that represents transformation of this geometry relative to containing
		/// object.
		/// </summary>
		public Matrix34 Transformation
		{
			get { return this.pMtx3x4; }
			set { this.pMtx3x4 = value; }
		}
		/// <summary>
		/// Gets or sets the object that represents the surface that overrides one in corresponding geometry
		/// object.
		/// </summary>
		public PhysicalSurface Surface
		{
			get { return new PhysicalSurface(this.surface_idx); }
			set { this.surface_idx = value.Index; }
		}
		/// <summary>
		/// Gets or sets a set of flags that specifies the geometry.
		/// </summary>
		public PhysicsGeometryFlags Flags
		{
			get { return this.flags; }
			set { this.flags = value; }
		}
		/// <summary>
		/// Gets or sets a set of flags that specify what kind of geometry this one is.
		/// </summary>
		/// <remarks>
		/// Collision is detected when a collider's flags has at least one common flag set with collidee's
		/// flags.
		/// </remarks>
		public ColliderTypes ColliderFlags
		{
			get { return this.flagsCollider; }
			set { this.flagsCollider = value; }
		}
		/// <summary>
		/// Gets or sets the breakability index that can be acquired from <see cref="ExplosionShapes.Add"/>.
		/// </summary>
		public int BreakabilityIndex
		{
			get { return this.idmatBreakable; }
			set { this.idmatBreakable = value; }
		}
		/// <summary>
		/// Gets or sets the object that specifies tetrahedral lattice that is used by soft bodies and
		/// procedurally breakable objects.
		/// </summary>
		public TetraLattice Lattice
		{
			get { return this.pLattice; }
			set { this.pLattice = value; }
		}
		/// <summary>
		/// Gets or sets an array of surface type mappings that are used the geometry of this part.
		/// </summary>
		[CanBeNull]
		public int[] SurfaceTypesMapping
		{
			get { return this.matMappings; }
			set
			{
				this.matMappings = value;
				if (this.pMatMapping != null)
				{
					CryMarshal.Free(new IntPtr(this.pMatMapping), true);
				}

				if (value.IsNullOrEmpty())
				{
					this.pMatMapping = null;
					this.nMats = 0;
					return;
				}

				int* mappings;

				try
				{
					this.nMats = value.Length;
					mappings = (int*)CryMarshal.Allocate((ulong)(sizeof(int) * this.nMats), false).ToPointer();
				}
				catch (Exception ex)
				{
					MonoInterface.DisplayException(ex);
					return;
				}

				fixed (int* arrayPtr = value)
				{
					for (int i = 0; i < this.nMats; i++)
					{
						mappings[i] = arrayPtr[i];
					}
				}

				this.pMatMapping = mappings;
			}
		}
		/// <summary>
		/// Takes a table of surface type identifiers from the material that should be taken from the main
		/// <see cref="CryEntity"/> object and puts into this object.
		/// </summary>
		/// <exception cref="ArgumentNullException">Given material is null.</exception>
		public Material MaterialSurfaceTypesMapping
		{
			set
			{
				if (!value.IsValid)
				{
					throw new ArgumentNullException(nameof(value), "Given material is null");
				}
				var table = value.SurfaceTypeIds;
				this.pMatMapping = (int*)CryMarshal.Allocate((ulong)(sizeof(SurfaceTypeTable) - 4), false);
				*this.pMatMapping = *(int*)&table;
				this.nMats = table.Count;
			}
		}
		/// <summary>
		/// Gets or sets the value that indicates how close another entity must be to this part, to make the
		/// part report the contact.
		/// </summary>
		public float MinimalContactDistance
		{
			get { return this.minContactDist; }
			set { this.minContactDist = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass this parameter to invoked this constructor.</param>
		public GeometryParameters([UsedImplicitly] int notUsed)
		{
			this.initialized = true;

			this.type = PhysicsGeometryParametersTypes.General;
			this.pMtx3x4 = new Matrix34 {M00 = UnusedValue.Single};
			this.matMappings = null;
			this.density = this.mass = 0;
			this.pos = new Vector3();
			this.q = Quaternion.Identity;
			this.bRecalcBBox = 1;
			this.flags = PhysicsGeometryFlags.CollisionTypeSolid | PhysicsGeometryFlags.CollisionTypeRay |
						 PhysicsGeometryFlags.Floats | PhysicsGeometryFlags.CollisionTypeExplosion;
			this.flagsCollider = (ColliderTypes)GeometryCollisionTypeCodes.collision_type0;
			this.pMtx3x4.M00 = UnusedValue.Single;
			this.scale = 1.0f;
			this.pLattice = new TetraLattice();
			this.pMatMapping = null;
			this.nMats = 0;
			this.surface_idx = UnusedValue.Int32;
			this.minContactDist = UnusedValue.Single;
			this.idmatBreakable = UnusedValue.Int32;
		}
		#endregion
	}
}