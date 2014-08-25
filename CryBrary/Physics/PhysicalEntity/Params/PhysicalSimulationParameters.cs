using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;
using CryEngine.Utilities;

namespace CryEngine
{
	public struct PhysicalSimulationParameters
	{
		public static PhysicalSimulationParameters Create()
		{
			var simParams = new PhysicalSimulationParameters();

			simParams.type = 10;

			simParams.maxTimeStep = UnusedMarker.Float;
			simParams.gravity = UnusedMarker.Vector3;
			simParams.minEnergy = UnusedMarker.Float;
			simParams.damping = UnusedMarker.Float;
			simParams.iSimClass = UnusedMarker.Integer;

			simParams.dampingFreefall = UnusedMarker.Float;
			simParams.mass = UnusedMarker.Float;
			simParams.density = UnusedMarker.Float;
			simParams.maxLoggedCollisions = UnusedMarker.Integer;
			simParams.maxRotVel = UnusedMarker.Float;
			simParams.disablePreCG = UnusedMarker.Integer;
			simParams.maxFriction = UnusedMarker.Float;

			return simParams;
		}

		internal int type;

		public int iSimClass;
		public float maxTimeStep; // maximum time step that entity can accept (larger steps will be split)
		public float minEnergy; // minimun of kinetic energy below which entity falls asleep (divided by mass)
		public float damping; // damped velocity = oridinal velocity * (1 - damping*time interval)
		public Vector3 gravity;	// per-entity gravity (note that if there are any phys areas with gravity, they will override it unless pef_ignore_areas is set
		public float dampingFreefall; // damping and gravity used when there are no collisions,
		public Vector3 gravityFreefall; // NOTE: if left unused, gravity value will be substituted (if provided)
		public float maxRotVel; // rotational velocity is clamped to this value
		public float mass;	// either mass of density should be set; mass = density*volume
		public float density;
		public int maxLoggedCollisions; // maximum EventPhysCollisions reported per frame (only supported by rigid bodies/ragdolls/vehicles)
		public int disablePreCG; // disables Pre-CG solver for the group this body is in (recommended for balls)
		public float maxFriction; // sets upper friction limit for this object and all objects it's currently in contact with
	}
}