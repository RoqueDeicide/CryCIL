using System;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Encapsulates parameters for animation processing.
	/// </summary>
	public struct AnimationProcessParameters
	{
		/// <summary>
		/// Default object of this type.
		/// </summary>
		public static readonly AnimationProcessParameters Default = new AnimationProcessParameters
		{
			Location = Quatvecale.Identity,
			OverrideDeltaTime = -1
		};
		/// <summary>
		/// Location where to play the animation. Default value is <see cref="Quatvecale.Identity"/>.
		/// </summary>
		public Quatvecale Location;
		/// <summary>
		/// Indicates whether processing needs to start during render pass.
		/// </summary>
		public bool OnRender;
		/// <summary>
		/// Distance to camera adjusted to zoom.
		/// </summary>
		public float ZoomAdjustedDistanceFromCamera;
		/// <summary>
		/// An override for frame time to use for animation. Default value is -1.
		/// </summary>
		public float OverrideDeltaTime;
	}
}