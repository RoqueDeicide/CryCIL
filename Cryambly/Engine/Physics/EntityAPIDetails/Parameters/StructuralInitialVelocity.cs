using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates a set of parameters that allows to set initial velocity for the part that was
	/// connected to another using the structural joint before the joint is force-broken the physical
	/// entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsParametersStructuralInitialVelocity
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.SetParameters"/> to apply these
		/// onto physical entity or to <see cref="PhysicalEntity.GetParameters"/> to get the currently
		/// applied ones.
		/// </summary>
		[UsedImplicitly] public PhysicsParameters Base;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private Vector3 v;
		[UsedImplicitly] private Vector3 w;
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of the part for which the initial velocity is specified.
		/// </summary>
		public int PartId
		{
			get { return this.partid; }
		}
		/// <summary>
		/// Gets initial directional velocity.
		/// </summary>
		public Vector3 Velocity
		{
			get { return this.v; }
		}
		/// <summary>
		/// Gets initial angular velocity.
		/// </summary>
		public Vector3 AngularVelocity
		{
			get { return this.w; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type that can be used to get or set parameters that define initial
		/// velocity of a part.
		/// </summary>
		/// <param name="part">           Identifier of the part.</param>
		/// <param name="velocity">       
		/// An optional value that can be used to set initial directional velocity of a part.
		/// </param>
		/// <param name="angularVelocity">
		/// An optional value that can be used to set initial angular velocity of a part.
		/// </param>
		public PhysicsParametersStructuralInitialVelocity(int part, Vector3 velocity = new Vector3(),
														  Vector3 angularVelocity = new Vector3())
		{
			this.Base = new PhysicsParameters(PhysicsParametersTypes.StructuralInitialVelocity);
			this.partid = part;
			this.v = velocity;
			this.w = angularVelocity;
		}
		#endregion
	}
}