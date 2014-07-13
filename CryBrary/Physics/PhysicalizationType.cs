using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum PhysicalizationType
	{
		None = 0,
		Static,
		Rigid,
		WheeledVehicle,
		Living,
		Particle,
		Articulated,
		Rope,
		Soft,
		Area
	}
}