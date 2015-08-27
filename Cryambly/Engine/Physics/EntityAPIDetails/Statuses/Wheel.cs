using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query status of the wheel of the vehicle.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusWheel
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int iWheel;
		[UsedImplicitly] private int partid;
		[UsedImplicitly] private int bContact;
		[UsedImplicitly] private Vector3 ptContact;
		[UsedImplicitly] private Vector3 normContact;
		[UsedImplicitly] private float w;
		[UsedImplicitly] private int bSlip;
		[UsedImplicitly] private Vector3 velSlip;
		[UsedImplicitly] private int contactSurfaceIdx;
		[UsedImplicitly] private float friction;
		[UsedImplicitly] private float suspLen;
		[UsedImplicitly] private float suspLenFull;
		[UsedImplicitly] private float suspLen0;
		[UsedImplicitly] private float r;
		[UsedImplicitly] private float torque;
		[UsedImplicitly] private float steer;
		[UsedImplicitly] private PhysicalEntity pCollider;
		#endregion
		#region Properties
		/// <summary>
		/// Gets zero-based index of this wheel.
		/// </summary>
		/// <remarks>
		/// This property is useful if you are querying the wheel by the identifier of the part that
		/// represents it.
		/// </remarks>
		public int WheelIndex
		{
			get { return this.iWheel; }
		}
		/// <summary>
		/// Gets the identifier of the part that represents this wheel.
		/// </summary>
		/// <remarks>
		/// This property is useful if you are querying the wheel by its zero-based index.
		/// </remarks>
		public int PartIdentifier
		{
			get { return this.partid; }
		}
		/// <summary>
		/// Gets the value that indicates whether this wheel contacts the ground.
		/// </summary>
		public bool ContactsGround
		{
			get { return this.bContact != 0; }
		}
		/// <summary>
		/// Gets the coordinates of the point where this wheel touches the ground, if
		/// <see cref="ContactsGround"/> returns <c>true</c>.
		/// </summary>
		public Vector3 GroundContactPoint
		{
			get { return this.ptContact; }
		}
		/// <summary>
		/// Gets the direction of the normal to the ground surface this wheel is touching, if
		/// <see cref="ContactsGround"/> returns <c>true</c>.
		/// </summary>
		public Vector3 GroundContactNormal
		{
			get { return this.normContact; }
		}
		/// <summary>
		/// Gets the speed of this wheel's rotation around its axle.
		/// </summary>
		public float AngularSpeed
		{
			get { return this.w; }
		}
		/// <summary>
		/// Gets the value that indicates whether this wheel is slipping (has a velocity component that is
		/// not caused by the wheel's rotation).
		/// </summary>
		public bool Slipping
		{
			get { return this.bSlip != 0; }
		}
		/// <summary>
		/// Gets the velocity component of this wheel that is not caused by the rotation of the wheel.
		/// </summary>
		public Vector3 SlippingVelocity
		{
			get { return this.velSlip; }
		}
		/// <summary>
		/// Gets an object that represents the ground surface type.
		/// </summary>
		public SurfaceType GroundSurfaceType
		{
			get { return SurfaceType.Get(this.contactSurfaceIdx); }
		}
		/// <summary>
		/// Gets the current friction between the wheel and the surface.
		/// </summary>
		public float Friction
		{
			get { return this.friction; }
		}
		/// <summary>
		/// Gets the current length of the suspension spring that is attached to this wheel.
		/// </summary>
		public float CurrentSuspensionLength
		{
			get { return this.suspLen; }
		}
		/// <summary>
		/// Gets the normal length of the suspension spring that is attached to this wheel.
		/// </summary>
		public float NormalSuspensionLength
		{
			get { return this.suspLen0; }
		}
		/// <summary>
		/// Gets the length of the suspension spring that is attached to this wheel in relaxed state.
		/// </summary>
		public float RelaxedSuspensionLength
		{
			get { return this.suspLenFull; }
		}
		/// <summary>
		/// Gets the radius of this wheel in meters.
		/// </summary>
		public float Radius
		{
			get { return this.r; }
		}
		/// <summary>
		/// Gets the current driving axle torque.
		/// </summary>
		public float Torque
		{
			get { return this.torque; }
		}
		/// <summary>
		/// Gets the current steering angle if this wheel is a steering one.
		/// </summary>
		public float CurrentSteeringAngle
		{
			get { return this.steer; }
		}
		/// <summary>
		/// Gets the entity that represents this wheel.
		/// </summary>
		public PhysicalEntity Collider
		{
			get { return this.pCollider; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="number">     
		/// A number that can an identifier of the part that represents a wheel or it can be a zero-based
		/// index of the wheel. What it is is defined by <paramref name="getByPartId"/> argument.
		/// </param>
		/// <param name="getByPartId">
		/// Indicates whether <paramref name="number"/> is an identifier of the part that represents a
		/// wheel rather then a zero-based index of one.
		/// </param>
		public PhysicsStatusWheel(int number, bool getByPartId = true)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.Wheel);
			this.iWheel = 0;
			this.partid = UnusedValue.Int32;

			if (getByPartId)
			{
				this.partid = number;
			}
			else
			{
				this.iWheel = number;
			}
		}
		#endregion
	}
}