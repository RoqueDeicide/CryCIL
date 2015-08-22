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
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private Vector3 impulse;
		[UsedImplicitly] private Vector3 angImpulse;
		[UsedImplicitly] private Vector3 point;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private ImpulseApplicationTime iApplyTime;
		#endregion
		#region Properties
		/// <summary>
		/// Gets directional impulse that will be applied to the entity.
		/// </summary>
		public Vector3 Impulse
		{
			get { return this.impulse; }
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
		/// Creates a new action that applies the impulse to the physical entity.
		/// </summary>
		/// <param name="impulse">An object that describes the impulse.</param>
		/// <param name="part">   Optionally specifies the part of the entity to apply impulse to.</param>
		/// <param name="apply">  Optional value that indicates when to apply the impulse.</param>
		public PhysicsActionImpulse(ImpulseSpec impulse, EntityPartSpec part = new EntityPartSpec(),
									ImpulseApplicationTime apply = ImpulseApplicationTime.AfterTimeStep)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.Impulse);
			this.impulse = impulse.HasDir ? impulse.Dir : new Vector3();
			this.angImpulse = impulse.HasAng ? impulse.Ang : UnusedValue.Vector;
			this.point = impulse.HasPoint ? impulse.Point : UnusedValue.Vector;
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
			this.iApplyTime = apply;
		}
		#endregion
	}
}