using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Utilities;

namespace CryEngine.Physics.Status
{
	public struct DynamicsPhysicsStatus
	{
		public static DynamicsPhysicsStatus Create()
		{
			var status = new DynamicsPhysicsStatus();

			status.type = 8;

			status.partid = UnusedMarker.Integer;
			status.ipart = UnusedMarker.Integer;

			return status;
		}

		private int type;

		private int partid;
		private int ipart;

		private Vector3 v; // velocity
		public Vector3 Velocity { get { return v; } }

		private Vector3 w; // angular velocity
		public Vector3 AngularVelocity { get { return w; } }

		private Vector3 a; // linear acceleration
		public Vector3 Acceleration { get { return a; } }

		private Vector3 wa; // angular acceleration
		public Vector3 AngularAcceleration { get { return wa; } }

		private Vector3 centerOfMass;
		/// <summary>
		/// The center of mass / pivot point for this entity.
		/// </summary>
		public Vector3 CenterOfMass { get { return centerOfMass; } }

		private float submergedFraction; // percentage of the entity that is underwater; 0..1. not supported for individual parts
		/// <summary>
		/// Percentage of the entity that is underwater. (0 - 1)
		/// </summary>
		public float SubmergedFraction { get { return submergedFraction; } }

		private float mass;	// entity's or part's mass
		public float Mass { get { return mass; } }

		private float energy;	// kinetic energy; only supported by PE_ARTICULATED currently
		private int nContacts;
		private float time_interval; // not used
	}
}