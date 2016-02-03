using System;
using System.Linq;
using CryCil.Geometry;

namespace CryCil.Engine.Rendering.Views
{
	/// <summary>
	/// Encapsulates a set of parameters that specifies the shaking that can be applied to the
	/// <see cref="CryView"/>.
	/// </summary>
	public struct ShakeParameters
	{
		#region Fields
		/// <summary>
		/// Maximal angle of view deviation that can be caused by shaking.
		/// </summary>
		public EulerAngles ShakeAngle;
		/// <summary>
		/// Maximal shift of view that can be caused by shaking.
		/// </summary>
		public Vector3 ShakeShift;
		/// <summary>
		/// Duration of non-fading part of shaking in seconds.
		/// </summary>
		public float SustainDuration;
		/// <summary>
		/// Duration of beginning fading part of shaking in seconds.
		/// </summary>
		public float FadeInDuration;
		/// <summary>
		/// Duration of ending fading part of shaking in seconds.
		/// </summary>
		public float FadeOutDuration;
		/// <summary>
		/// Frequency of shakes.
		/// </summary>
		public float Frequency;
		/// <summary>
		/// Randomness of shakes.
		/// </summary>
		public float Randomness;
		/// <summary>
		/// Identifier of the shake effect.
		/// </summary>
		public int ShakeId;
		/// <summary>
		/// Unknown.
		/// </summary>
		public bool FlipVector;
		/// <summary>
		/// Unknown.
		/// </summary>
		public bool UpdateOnly;
		/// <summary>
		/// Indicates whether shaking should only be applied when the player is on the ground.
		/// </summary>
		public bool GroundOnly;
		/// <summary>
		/// Indicates whether duration of the shake is permanent. If
		/// <c>true</c><see cref="SustainDuration"/> value is ignored.
		/// </summary>
		public bool Permanent;
		/// <summary>
		/// Indicates whether sudden changes in the direction should be avoided.
		/// </summary>
		public bool IsSmooth;
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		#endregion
		#region Utilities
		#endregion
	}
}