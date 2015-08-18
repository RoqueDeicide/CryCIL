using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Physics.Primitives;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersBuoyancy
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly]
		public PhysicsParameters Base;
		[UsedImplicitly]
		private float waterDensity;
		[UsedImplicitly]
		private float kwaterDensity;
		[UsedImplicitly]
		private float waterDamping;
		[UsedImplicitly]
		private float waterResistance, kwaterResistance;
		[UsedImplicitly]
		private Vector3 waterFlow;
		[UsedImplicitly]
		private float flowVariance;
		[UsedImplicitly]
		private Primitive.Plane waterPlane;
		[UsedImplicitly]
		private float waterEmin;
		[UsedImplicitly]
		private int iMedium;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets density of liquid. When set, overrides the density of liquid that is defined by
		/// any the liquid volume this entity can be in.
		/// </summary>
		public float Density
		{
			get { return this.waterDensity; }
			set { this.waterDensity = value; }
		}
		/// <summary>
		/// Gets or sets the multiplier that is applied to the density of liquid (defined either by the
		/// liquid volume this entity is currently in or <see cref="Density"/> property if set). This
		/// property is only used by entities.
		/// </summary>
		public float DensityScale
		{
			get { return this.kwaterDensity; }
			set { this.kwaterDensity = value; }
		}
		/// <summary>
		/// Gets or sets maximal velocity damping value that is achieved when the entity is fully submerged
		/// in the liquid.
		/// </summary>
		/// <remarks>Damping is used to simulate liquid resistance to entity's movement.</remarks>
		public float Damping
		{
			get { return this.waterDamping; }
			set { this.waterDamping = value; }
		}
		/// <summary>
		/// Gets or sets liquid's medium resistance.
		/// </summary>
		public float Resistance
		{
			get { return this.waterResistance; }
			set { this.waterResistance = value; }
		}
		/// <summary>
		/// Gets or sets the multiplier that is applied to the liquid's medium resistance. This property is
		/// only used by entities.
		/// </summary>
		public float ResistanceScale
		{
			get { return this.kwaterResistance; }
			set { this.kwaterResistance = value; }
		}
		/// <summary>
		/// Vector of liquid flow.
		/// </summary>
		/// <remarks>
		/// This property along with <see cref="Resistance"/> defines how much liquid affects entity's
		/// movement.
		/// </remarks>
		public Vector3 Flow
		{
			get { return this.waterFlow; }
			set { this.waterFlow = value; }
		}
		/// <summary>
		/// Gets or sets the plane that describes the location and orientation of the surface of fluid
		/// volume.
		/// </summary>
		/// <remarks>
		/// The entity is considered to be inside the fluid volume, when it's behind the plane.
		/// </remarks>
		public Primitive.Plane Surface
		{
			get { return this.waterPlane; }
			set { this.waterPlane = value; }
		}
		/// <summary>
		/// Gets or sets the minimal energy the entity must have in the water volume in order to not be
		/// moved into sleeping state.
		/// </summary>
		public float MinimalEnergy
		{
			get { return this.waterEmin; }
			set { this.waterEmin = value; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this entity is in the gas, rather then in liquid.
		/// </summary>
		public bool IsGas
		{
			get { return this.iMedium != 0; }
			set { this.iMedium = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="ignored">
		/// When passed, causes invocation of this particular constructor. Does nothing otherwise.
		/// </param>
		public PhysicsParametersBuoyancy(bool ignored)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Buoyancy);
			this.waterDensity = UnusedValue.Single;
			this.kwaterDensity = UnusedValue.Single;
			this.waterDamping = UnusedValue.Single;
			this.waterResistance = UnusedValue.Single;
			this.kwaterResistance = UnusedValue.Single;
			this.waterFlow = UnusedValue.Vector;
			this.flowVariance = UnusedValue.Single;
			this.waterEmin = UnusedValue.Single;
			this.waterPlane = new Primitive.Plane
			{
				Normal = UnusedValue.Vector,
				Origin = UnusedValue.Vector
			};
			this.iMedium = UnusedValue.Int32;
		}
		#endregion
	}
}