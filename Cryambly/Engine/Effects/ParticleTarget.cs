using System;
using System.Runtime.InteropServices;

namespace CryCil.Engine
{
	/// <summary>
	/// Encapsulates description of a target for particle effects.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ParticleTarget
	{
		/// <summary>
		/// Position of the target.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Velocity of the target.
		/// </summary>
		public Vector3 Velocity;
		/// <summary>
		/// Approximate radius of the target, for orbiting.
		/// </summary>
		public float Radius;
		/// <summary>
		/// Indicates whether this target is enabled.
		/// </summary>
		public bool Enabled;
		/// <summary>
		/// Indicates whether this target should take priority over others.
		/// </summary>
		public bool Priority;
	}
}