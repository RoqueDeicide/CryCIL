using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to specify the skeleton (hidden mesh that uses cloth
	/// simulation to skin the main physics geometry) of the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersSkeleton
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private float stiffness;
		[UsedImplicitly] private float thickness;
		[UsedImplicitly] private float maxStretch;
		[UsedImplicitly] private float maxImpulse;
		[UsedImplicitly] private float timeStep;
		[UsedImplicitly] private int nSteps;
		[UsedImplicitly] private float hardness;
		[UsedImplicitly] private float explosionScale;
		[UsedImplicitly] private int bReset;
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of this part.
		/// </summary>
		public int PartId
		{
			get { return this.partid; }
		}
		/// <summary>
		/// Gets zero-based index of this part.
		/// </summary>
		public int PartIndex
		{
			get { return this.ipart; }
		}
		/// <summary>
		/// Gets or sets value that defines skeleton's resistance to bending and shearing.
		/// </summary>
		public float Stiffness
		{
			get { return this.stiffness; }
			set { this.stiffness = value; }
		}
		/// <summary>
		/// Gets or sets thickness value of this skeleton that is used for collisions.
		/// </summary>
		public float Thickness
		{
			get { return this.thickness; }
			set { this.thickness = value; }
		}
		/// <summary>
		/// Gets or sets the value that defines how far the skeleton can stretch.
		/// </summary>
		public float MaximalStretch
		{
			get { return this.maxStretch; }
			set { this.maxStretch = value; }
		}
		/// <summary>
		/// Gets or sets the magnitude of maximal impulse the skeleton can react to.
		/// </summary>
		public float MaximalImpulse
		{
			get { return this.maxImpulse; }
			set { this.maxImpulse = value; }
		}
		/// <summary>
		/// Gets or sets length of the time step that is used when simulating the skeleton.
		/// </summary>
		public float TimeStep
		{
			get { return this.timeStep; }
			set { this.timeStep = value; }
		}
		/// <summary>
		/// Gets or sets number of sub steps that are taken to simulate this skeleton when updating the
		/// entire structure of the entity.
		/// </summary>
		public int SubStepCount
		{
			get { return this.nSteps; }
			set { this.nSteps = value; }
		}
		/// <summary>
		/// Gets or sets value that defines skeleton's resistance to stretching.
		/// </summary>
		public float Hardness
		{
			get { return this.hardness; }
			set { this.hardness = value; }
		}
		/// <summary>
		/// Gets or sets the scale of skeleton's reaction to the explosion impulse.
		/// </summary>
		public float ExplosionScale
		{
			get { return this.explosionScale; }
			set { this.explosionScale = value; }
		}
		/// <summary>
		/// Sets that value that can be used to reset the skeleton to its original pose.
		/// </summary>
		public int Reset
		{
			set { this.bReset = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="part">
		/// An object that specifies which part's skeleton parameters to get/set.
		/// </param>
		public PhysicsParametersSkeleton(EntityPartSpec part)
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.Skeleton);
			this.ipart = UnusedValue.Int32;
			this.partid = UnusedValue.Int32;
			this.bReset = UnusedValue.Int32;
			this.explosionScale = UnusedValue.Single;
			this.hardness = UnusedValue.Single;
			this.timeStep = UnusedValue.Single;
			this.nSteps = UnusedValue.Int32;
			this.maxImpulse = UnusedValue.Single;
			this.maxStretch = UnusedValue.Single;
			this.thickness = UnusedValue.Single;
			this.stiffness = UnusedValue.Single;

			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
		}
		#endregion
	}
}