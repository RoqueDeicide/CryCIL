using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to customize the tetrahedral lattice.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Objects of this type cannot be used with <see cref="PhysicalEntity"/> functions, instead they work
	/// with <see cref="TetraLattice"/>.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersTetraLattice
	{
		#region Fields
		[UsedImplicitly]
		private bool initialized;
		[UsedImplicitly]
		private int nMaxCracks;
		[UsedImplicitly]
		private float maxForcePush, maxForcePull, maxForceShift;
		[UsedImplicitly]
		private float maxTorqueTwist, maxTorqueBend;
		[UsedImplicitly]
		private float crackWeaken;
		[UsedImplicitly]
		private float density;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this object was created via correct constructor.
		/// </summary>
		public bool Initialized
		{
			get { return this.initialized; }
		}
		/// <summary>
		/// Gets maximal number of cracks that can be created in the lattice per physics update.
		/// </summary>
		public int MaxCracksPerUpdate
		{
			get { return this.nMaxCracks; }
			set { this.nMaxCracks = value; }
		}
		/// <summary>
		/// Gets or sets maximal pushing force that can be handled by the part of lattice before it cracks.
		/// </summary>
		public float MaxForcePush
		{
			get { return this.maxForcePush; }
			set { this.maxForcePush = value; }
		}
		/// <summary>
		/// Gets or sets maximal pulling force that can be handled by the part of lattice before it cracks.
		/// </summary>
		public float MaxForcePull
		{
			get { return this.maxForcePull; }
			set { this.maxForcePull = value; }
		}
		/// <summary>
		/// Gets or sets maximal shifting force that can be handled by the part of lattice before it
		/// cracks.
		/// </summary>
		public float MaxForceShift
		{
			get { return this.maxForceShift; }
			set { this.maxForceShift = value; }
		}
		/// <summary>
		/// Gets or sets maximal twisting torque that can be handled by the part of lattice before it
		/// cracks.
		/// </summary>
		public float MaxTorqueTwist
		{
			get { return this.maxTorqueTwist; }
			set { this.maxTorqueTwist = value; }
		}
		/// <summary>
		/// Gets or sets maximal bending torque that can be handled by the part of lattice before it
		/// cracks.
		/// </summary>
		public float MaxTorqueBend
		{
			get { return this.maxTorqueBend; }
			set { this.maxTorqueBend = value; }
		}
		/// <summary>
		/// Gets or sets the value between 0 and 1 that indicates how much the neighboring tetrahedra will
		/// be weakened by the crack.
		/// </summary>
		public float CrackWeakening
		{
			get { return this.crackWeaken; }
			set { this.crackWeaken = MathHelpers.Clamp(value, 0.0f, 1.0f); }
		}
		/// <summary>
		/// Gets or sets density of the lattice (affects the durability of the lattice).
		/// </summary>
		public float Density
		{
			get { return this.density; }
			set { this.density = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid default instance of this type.
		/// </summary>
		/// <param name="notUsed">
		/// This parameter is not used, but it must be provided to make sure correct constructor is
		/// invoked.
		/// </param>
		public PhysicsParametersTetraLattice(bool notUsed)
		{
			this.initialized = true;
			this.nMaxCracks = UnusedValue.Int32;
			this.density = UnusedValue.Single;
			this.maxForcePull = UnusedValue.Single;
			this.maxForcePush = UnusedValue.Single;
			this.maxForceShift = UnusedValue.Single;
			this.maxTorqueTwist = UnusedValue.Single;
			this.maxTorqueBend = UnusedValue.Single;
			this.crackWeaken = UnusedValue.Single;
		}
		#endregion
	}
}