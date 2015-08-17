using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of moments at the impulse can be applied.
	/// </summary>
	public enum ImpulseApplicationTime
	{
		/// <summary>
		/// Specifies that the impulse must be applied immediately.
		/// </summary>
		Immediately,
		/// <summary>
		/// Specifies that the impulse must be applied before next time step.
		/// </summary>
		BeforeTimeStep,
		/// <summary>
		/// Specifies that the impulse must be applied after next time step.
		/// </summary>
		AfterTimeStep
	}
	/// <summary>
	/// Encapsulates description of the action that applies impulse to the physical entity.
	/// </summary>
	/// <remarks>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionImpulse
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to <see cref="PhysicalEntity.ActUpon"/> in order execute this
		/// action.
		/// </summary>
		[UsedImplicitly]
		public PhysicsAction Base;
		[UsedImplicitly]
		private Vector3 impulse;
		[UsedImplicitly]
		private Vector3 angImpulse;
		[UsedImplicitly]
		private Vector3 point;
		[UsedImplicitly]
		private int partid;
		[UsedImplicitly]
		private int ipart;
		[UsedImplicitly]
		private ImpulseApplicationTime iApplyTime;
		#endregion
		#region Properties
		/// <summary>
		/// Gets directional impulse that will be applied to the entity.
		/// </summary>
		/// <returns><c>null</c>, if directional impulse won't be applied.</returns>
		public Vector3? Impulse
		{
			get { return this.impulse.IsUsed() ? this.impulse : (Vector3?)null; }
		}
		/// <summary>
		/// Gets angular impulse that will be applied to the entity.
		/// </summary>
		/// <returns><c>null</c>, if angular impulse won't be applied.</returns>
		public Vector3? AngularImpulse
		{
			get { return this.angImpulse.IsUsed() ? this.angImpulse : (Vector3?)null; }
		}
		/// <summary>
		/// Gets the coordinates of the point in world space the impulse will be applied at to the entity.
		/// </summary>
		/// <returns><c>null</c>, if not specified.</returns>
		public Vector3? PointOfApplication
		{
			get { return this.point.IsUsed() ? this.point : (Vector3?)null; }
		}
		/// <summary>
		/// Gets the identifier of the part of this entity to which the impulse will be applied.
		/// </summary>
		/// <returns><c>null</c>, if not specified or the part is identified by index.</returns>
		public int? PartIdentifier
		{
			get { return this.partid.IsUsed() ? this.partid : (int?)null; }
		}
		/// <summary>
		/// Gets the index of the part of this entity to which the impulse will be applied.
		/// </summary>
		/// <returns><c>null</c>, if not specified or the part is identified by identifier.</returns>
		public int? PartIndex
		{
			get { return this.ipart.IsUsed() ? this.ipart : (int?)null; }
		}
		/// <summary>
		/// Gets the value that indicates when to apply the impulse.
		/// </summary>
		public ImpulseApplicationTime ApplicationTime
		{
			get { return this.iApplyTime; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new action that applies directional impulse.
		/// </summary>
		/// <param name="impulse">        A vector that defines the directional impulse.</param>
		/// <param name="partId">         
		/// Optional identifier of the part of the entity to apply impulse to. Don't specify, if
		/// <paramref name="partIndex"/> is specified.
		/// </param>
		/// <param name="partIndex">      
		/// Optional index of the part of the entity to apply impulse to. Don't specify, if
		/// <paramref name="partId"/> is specified.
		/// </param>
		/// <param name="applicationTime">Optional value that indicates when to apply the impulse.</param>
		/// <exception cref="ArgumentException">
		/// Don't specify identifier of the part of the entity and the index. You can only specify one of
		/// them.
		/// </exception>
		public PhysicsActionImpulse(Vector3 impulse, int partId = UnusedValue.Int32, int partIndex = UnusedValue.Int32,
									ImpulseApplicationTime applicationTime = ImpulseApplicationTime.AfterTimeStep)
			: this(impulse, UnusedValue.Vector, UnusedValue.Vector, partId, partIndex, applicationTime)
		{
		}
		/// <summary>
		/// Creates a new action that applies directional and angular impulse.
		/// </summary>
		/// <param name="impulse">        
		/// A vector that defines the directional impulse. Pass <see cref="UnusedValue.Vector"/>, if you
		/// don't need this parameter.
		/// </param>
		/// <param name="angularImpulse"> A vector that defines the angular impulse.</param>
		/// <param name="partId">         
		/// Optional identifier of the part of the entity to apply impulse to. Don't specify, if
		/// <paramref name="partIndex"/> is specified.
		/// </param>
		/// <param name="partIndex">      
		/// Optional index of the part of the entity to apply impulse to. Don't specify, if
		/// <paramref name="partId"/> is specified.
		/// </param>
		/// <param name="applicationTime">Optional value that indicates when to apply the impulse.</param>
		/// <exception cref="ArgumentException">
		/// Don't specify identifier of the part of the entity and the index. You can only specify one of
		/// them.
		/// </exception>
		public PhysicsActionImpulse(Vector3 impulse, Vector3 angularImpulse, int partId = UnusedValue.Int32, int partIndex = UnusedValue.Int32,
									ImpulseApplicationTime applicationTime = ImpulseApplicationTime.AfterTimeStep)
			: this(impulse, angularImpulse, UnusedValue.Vector, partId, partIndex, applicationTime)
		{
		}
		/// <summary>
		/// Creates a new action that applies directional and angular impulse.
		/// </summary>
		/// <param name="impulse">        
		/// A vector that defines the directional impulse. Pass <see cref="UnusedValue.Vector"/>, if you
		/// don't need this parameter.
		/// </param>
		/// <param name="angularImpulse"> 
		/// A vector that defines the angular impulse. Pass <see cref="UnusedValue.Vector"/>, if you don't
		/// need this parameter.
		/// </param>
		/// <param name="point">          
		/// A vector that defines the point of application of the impulse in world space.
		/// </param>
		/// <param name="partId">         
		/// Optional identifier of the part of the entity to apply impulse to. Don't specify, if
		/// <paramref name="partIndex"/> is specified.
		/// </param>
		/// <param name="partIndex">      
		/// Optional index of the part of the entity to apply impulse to. Don't specify, if
		/// <paramref name="partId"/> is specified.
		/// </param>
		/// <param name="applicationTime">Optional value that indicates when to apply the impulse.</param>
		/// <exception cref="ArgumentException">
		/// Don't specify identifier of the part of the entity and the index. You can only specify one of
		/// them.
		/// </exception>
		public PhysicsActionImpulse(Vector3 impulse, Vector3 angularImpulse, Vector3 point, int partId = UnusedValue.Int32,
									int partIndex = UnusedValue.Int32, ImpulseApplicationTime applicationTime =
																		   ImpulseApplicationTime.AfterTimeStep)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Impulse);
			this.impulse = impulse;
			this.angImpulse = angularImpulse;
			this.point = point;
			this.partid = partId;
			this.ipart = partIndex;
			this.iApplyTime = applicationTime;

			if (this.partid.IsUsed() && this.ipart.IsUsed())
			{
				throw new ArgumentException("Don't specify identifier of the part of the entity and the index. You can " +
											"only specify one of them.");
			}
		}
		#endregion
	}
}