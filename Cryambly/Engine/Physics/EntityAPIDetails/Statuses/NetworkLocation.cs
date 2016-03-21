using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the object that is used to query current location of the physical entity
	/// that is registered on the server as a current location of the entity.
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
	public struct PhysicsStatusNetworkLocation
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		/// <summary>
		/// Coordinates of the entity in world space.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Orientation of the entity in world space.
		/// </summary>
		public Quaternion Orientation;
		/// <summary>
		/// Velocity of the entity in world space.
		/// </summary>
		public Vector3 Velocity;
		/// <summary>
		/// Angular velocity of the entity.
		/// </summary>
		public EulerAngles AngularVelocity;
		/// <summary>
		/// Difference between the time stamp from when this location was received from the server and
		/// current time stamp.
		/// </summary>
		public float TimeOffset;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="notUsed">Pass anything.</param>
		public PhysicsStatusNetworkLocation([UsedImplicitly] int notUsed)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.NetworkLocation);
		}
		#endregion
	}
}