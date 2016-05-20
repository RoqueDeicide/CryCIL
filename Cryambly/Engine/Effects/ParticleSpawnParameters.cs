using System;
using System.Linq;
using CryCil.Engine.Audio;
using CryCil.Geometry;

namespace CryCil.Engine
{
	/// <summary>
	/// Encapsulates parameters that specify how to spawn a particle emitter.
	/// </summary>
	public struct ParticleSpawnParameters
	{
		/// <summary>
		/// Indicates what type of object particles emitted from.
		/// </summary>
		public GeometryType AttachType;
		/// <summary>
		/// Indicates what aspect of shape emitted from.
		/// </summary>
		public GeometryFormat AttachFormat;
		/// <summary>
		/// Indicates whether particle count should be multiplied by geometry extent (length/area/volume).
		/// </summary>
		public bool CountPerUnit;
		/// <summary>
		/// Indicates whether the emitter's age must be advanced to its equilibrium state.
		/// </summary>
		public bool Prime;
		/// <summary>
		/// Indicates whether bounding box should be used to register this effect in VisArea instead of
		/// position.
		/// </summary>
		public bool RegisterByBoundingBox;
		/// <summary>
		/// Indicates whether the emitter is outside of the level.
		/// </summary>
		public bool Nowhere;
		/// <summary>
		/// Factor that multiplies the particle count (on top of <see cref="CountPerUnit"/> if set).
		/// </summary>
		public float CountScale;
		/// <summary>
		/// Scale of all particle effect sizes.
		/// </summary>
		public float SizeScale;
		/// <summary>
		/// Scale of emission speed.
		/// </summary>
		public float SpeedScale;
		/// <summary>
		/// Scale of emitter time evolution.
		/// </summary>
		public float TimeScale;
		/// <summary>
		/// How often to restart emitter.
		/// </summary>
		public float PulsePeriod;
		/// <summary>
		/// Controls parameter strength curves.
		/// </summary>
		public float Strength;
		/// <summary>
		/// Indicates whether audio should be enabled for this particle effect.
		/// </summary>
		public bool EnableAudio;
		/// <summary>
		/// Audio obstruction/occlusion calculation type.
		/// </summary>
		public AudioOcclusionType OcclusionType;
		/// <summary>
		/// Indicates what audio RTPC this particle effect instance drives.
		/// </summary>
		public string AudioRtpc;
	}
}