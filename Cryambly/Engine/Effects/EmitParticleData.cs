using System;
using System.Linq;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Physics;
using CryCil.Geometry;

namespace CryCil.Engine
{
	internal struct EmitParticleData
	{
		// The displayable geometry object for the entity. If NULL, uses emitter settings for sprite or
		// geometry.
		internal StaticObject StatObj;
		// A physical entity which controls the particle. If NULL, uses emitter settings to physicalize or
		// move particle.
		internal PhysicalEntity PhysEnt;
		internal Quatvecale Location; // Specified location for particle.
		internal Velocity3 Velocity; // Specified linear and rotational velocity for particle.
		internal bool HasLocation; // Location is specified.
		internal bool HasVel; // Velocities are specified.
	}
}