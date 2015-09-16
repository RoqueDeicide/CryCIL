using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify dimensions of the physical entity that is a
	/// living entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersDimensions
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private float heightPivot;
		[UsedImplicitly] private float heightEye;
		[UsedImplicitly] private Vector3 sizeCollider;
		[UsedImplicitly] private float heightCollider;
		[UsedImplicitly] private float headRadius;
		[UsedImplicitly] private float heightHead;
		[UsedImplicitly] private Vector3 dirUnproj;
		[UsedImplicitly] private float maxUnproj;
		[UsedImplicitly] private int bUseCapsule;
		[UsedImplicitly] private float groundContactEps;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the difference between the ground and living entity's 'feet'. Usually its zero.
		/// </summary>
		public float FeetLevel
		{
			get { return this.heightPivot; }
			set { this.heightPivot = value; }
		}
		/// <summary>
		/// Gets or sets the level the the camera that is used by the entity to view the world is located
		/// at.
		/// </summary>
		/// <remarks>
		/// This height is used as a reference when changing position of the camera when simulating certain
		/// situations, like lowering a head when passing through crawl-space as well as smoothing any
		/// height changes that happen when the entity, for instance, is going through the bumpy surface.
		/// </remarks>
		public float EyeLevel
		{
			get { return this.heightEye; }
			set { this.heightEye = value; }
		}
		/// <summary>
		/// Gets or sets the level the center of the cylinder that is used for collision detection by this
		/// entity is located on.
		/// </summary>
		public float ColliderLevel
		{
			get { return this.heightCollider; }
			set { this.heightCollider = value; }
		}
		/// <summary>
		/// Gets or sets the height of the cylinder that is used for collision detection by this entity.
		/// </summary>
		public float ColliderHeight
		{
			get { return this.sizeCollider.Z; }
			set { this.sizeCollider.Z = value; }
		}
		/// <summary>
		/// Gets or sets the radius of the cylinder that is used for collision detection by this entity.
		/// </summary>
		public float ColliderRadius
		{
			get { return this.sizeCollider.X; }
			set { this.sizeCollider.X = value; }
		}
		/// <summary>
		/// Gets or sets the level the center of the sphere that represents the head of this entity is
		/// located on.
		/// </summary>
		/// <remarks>
		/// Head of living entity doesn't affect the entity's movement when collides with anything, but it
		/// makes the 'head' along with 'eyes' to go down.
		/// </remarks>
		public float HeadLevel
		{
			get { return this.heightHead; }
			set { this.heightHead = value; }
		}
		/// <summary>
		/// Gets or sets the radius of the sphere that represents the head of this entity.
		/// </summary>
		/// <remarks>
		/// Head of living entity doesn't affect the entity's movement when collides with anything, but it
		/// makes the 'head' along with 'eyes' to go down.
		/// </remarks>
		public float HeadRadius
		{
			get { return this.headRadius; }
			set { this.headRadius = value; }
		}
		/// <summary>
		/// Gets or sets the normalized vector that is used as a direction that is tested when this entity
		/// ends up overlapping something.
		/// </summary>
		/// <remarks>
		/// If this vector is equal to <see cref="Vector3.Up"/> and this entity overlaps with something,
		/// then the entity will shot upwards in order to stop it from overlapping.
		/// </remarks>
		public Vector3 UnprojectionDirection
		{
			get { return this.dirUnproj; }
			set
			{
				this.dirUnproj = value;
				this.dirUnproj.Normalize();
			}
		}
		/// <summary>
		/// Gets or sets maximal distance that can be tested in <see cref="UnprojectionDirection"/> when
		/// this entity overlaps with environment.
		/// </summary>
		/// <remarks>By default it's equal to 0 which means the distance is chosen automatically.</remarks>
		public float MaximalUnprojectionDistance
		{
			get { return this.maxUnproj; }
			set { this.maxUnproj = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity uses a capsule instead of the
		/// cylinder for collision detection.
		/// </summary>
		public bool UseCapsule
		{
			get { return this.bUseCapsule != 0; }
			set { this.bUseCapsule = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets or sets the value that indicates how far the entity's feet must be from the ground to
		/// state loss of contact with the ground.
		/// </summary>
		/// <remarks>
		/// By default this value is chosen using the following code: <c>Max(0.04, 0.01 *
		/// <see cref="ColliderHeight"/>);</c>
		/// </remarks>
		public float GroundContactDetectionPrecision
		{
			get { return this.groundContactEps; }
			set { this.groundContactEps = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything to invoke this constructor.</param>
		public PhysicsParametersDimensions([UsedImplicitly] int notUsed)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.PlayerDimensions);
			this.heightPivot = UnusedValue.Single;
			this.heightEye = UnusedValue.Single;
			this.sizeCollider = UnusedValue.Vector;
			this.heightCollider = UnusedValue.Single;
			this.heightHead = UnusedValue.Single;
			this.dirUnproj = Vector3.Up;
			this.headRadius = UnusedValue.Single;
			this.maxUnproj = 0;
			this.groundContactEps = UnusedValue.Single;
			this.bUseCapsule = UnusedValue.Int32;
		}
		#endregion
	}
}