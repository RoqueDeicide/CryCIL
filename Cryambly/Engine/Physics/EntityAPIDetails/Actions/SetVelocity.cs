using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the action that sets the velocity for the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionSetVelocity
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action to
		/// the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private int ipart;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private Vector3 v, w;
		[UsedImplicitly] private int bRotationAroundPivot;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the vector that represents the velocity to set.
		/// </summary>
		public Vector3 Velocity
		{
			get { return this.v; }
			set { this.v = value; }
		}
		/// <summary>
		/// Gets or sets the angular velocity to set.
		/// </summary>
		public EulerAngles AngularVelocity
		{
			get { return new EulerAngles(this.w.X, this.w.Y, this.w.Z); }
			set { this.w = new Vector3(value.Pitch, value.Roll, value.Yaw); }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether <see cref="AngularVelocity"/> is applied around
		/// the entity's pivot point rather then its center of mass.
		/// </summary>
		public bool RotateAroundPivot
		{
			get { return this.bRotationAroundPivot != 0; }
			set { this.bRotationAroundPivot = value ? 1 : 0; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="part">
		/// An object that specifies which part of the entity to set velocity for.
		/// </param>
		public PhysicsActionSetVelocity(EntityPartSpec part)
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.SetVelocity);
			this.ipart = UnusedValue.Int32;
			this.partid = UnusedValue.Int32;
			this.v = UnusedValue.Vector;
			this.w = UnusedValue.Vector;
			this.bRotationAroundPivot = UnusedValue.Int32;

			if (part.partIsSpecified)
			{
				this.partid = part.PartId;
				this.ipart = part.PartIndex;
			}
		}
		#endregion
	}
}