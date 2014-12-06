using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Utilities;
using CryEngine.Native;

namespace CryEngine.Physics
{
	public class PhysicalEntityParticle : PhysicalEntity
	{
		internal PhysicalEntityParticle(IntPtr physEntPtr)
		{
			Handle = physEntPtr;
		}

		public override PhysicalizationType Type
		{
			get { return PhysicalizationType.Particle; }
		}

		[CLSCompliant(false)]
		public bool SetParameters(ref ParticleParameters parameters)
		{
			return PhysicsInterop.SetParticleParams(Handle, ref parameters);
		}

		[CLSCompliant(false)]
		public bool GetParameters(ref ParticleParameters parameters)
		{
			return PhysicsInterop.GetParticleParams(Handle, ref parameters);
		}
	}
}