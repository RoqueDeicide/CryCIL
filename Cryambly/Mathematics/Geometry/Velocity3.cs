using System;
using System.Linq;

namespace CryCil.Geometry
{
	/// <summary>
	/// Encapsulates information about linear and rotational velocities.
	/// </summary>
	public struct Velocity3
	{
		#region Fields
		private Vector3 linearVelocity;
		private Vector3 rotationalVelocity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets linear velocity stored within this object.
		/// </summary>
		public Vector3 LinearVelocity
		{
			get { return this.linearVelocity; }
			set { this.linearVelocity = value; }
		}
		/// <summary>
		/// Gets or sets rotational velocity stored within this object.
		/// </summary>
		public Vector3 RotationalVelocity
		{
			get { return this.rotationalVelocity; }
			set { this.rotationalVelocity = value; }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="linearVelocity">    Linear velocity.</param>
		/// <param name="rotationalVelocity">Rotational velocity.</param>
		public Velocity3(Vector3 linearVelocity, Vector3 rotationalVelocity)
		{
			this.linearVelocity = linearVelocity;
			this.rotationalVelocity = rotationalVelocity;
		}
		/// <summary>
		/// Calculates velocity of an oriented object that moves from one location to another.
		/// </summary>
		/// <param name="location0">Initial position and orientation.</param>
		/// <param name="location1">Final position and orientation.</param>
		/// <param name="time">     
		/// Time in seconds it takes for the object to move from <paramref name="location0"/> to
		/// <paramref name="location1"/>.
		/// </param>
		public Velocity3(Quatvec location0, Quatvec location1, float time)
		{
			this.linearVelocity = (location1.Vector - location0.Vector) / time;
			this.rotationalVelocity = (location1.Quaternion * location0.Quaternion.Inverted / time).Logarithm;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Calculates linear velocity from specified point of view(?).
		/// </summary>
		/// <param name="relativePosition">Position to observe the velocity from(?).</param>
		/// <returns>Projected linear velocity(?).</returns>
		public Vector3 VelocityAt(ref Vector3 relativePosition)
		{
			return this.linearVelocity + this.rotationalVelocity % relativePosition;
		}
		#endregion
	}
}