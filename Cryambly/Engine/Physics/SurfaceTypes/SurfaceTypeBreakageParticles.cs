using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates description of the particle effect that is spawned during one of the breakage events.
	/// </summary>
	public struct SurfaceTypeBreakageParticles
	{
		[UsedImplicitly] private IntPtr type;
		[UsedImplicitly] private IntPtr particleEffect;
		[UsedImplicitly] private int countPerUnit;
		[UsedImplicitly] private float countScale;
		[UsedImplicitly] private float scale;
		/// <summary>
		/// Gets the name of the breakage event during which this particle effect will spawn.
		/// </summary>
		/// <remarks>
		/// Currently known breakage events are: breakage, destroy, joint_shatter, joint_break,
		/// freeze_vapor, freeze_shatter.
		/// </remarks>
		public string Type
		{
			get { return Marshal.PtrToStringAnsi(this.type); }
		}
		/// <summary>
		/// Gets the name of the particle effect to use.
		/// </summary>
		public string ParticleEffect
		{
			get { return Marshal.PtrToStringAnsi(this.particleEffect); }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int CountPerUnit
		{
			get { return this.countPerUnit; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public float CountScale
		{
			get { return this.countScale; }
		}
		/// <summary>
		/// Gets the scale of the particle effect.
		/// </summary>
		public float Scale
		{
			get { return this.scale; }
		}
	}
}