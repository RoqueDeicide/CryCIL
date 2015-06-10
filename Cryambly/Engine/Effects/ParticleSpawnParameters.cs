using System;
using System.Text;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Utilities;

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
		public GeometryForm AttachForm;
		/// <summary>
		/// Indicates whether particle count should be multiplied by geometry extent (length/area/volume).
		/// </summary>
		public bool CountPerUnit;
		/// <summary>
		/// Indicates whether audio should be enabled for this particle effect.
		/// </summary>
		public bool EnableAudio;
		/// <summary>
		/// Indicates whether bounding box should be used to register this effect in VisArea instead of
		/// position.
		/// </summary>
		public bool RegisterByBoundingBox;
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
		[UsedImplicitly]
		private StackString audioRtpc;
		/// <summary>
		/// Gets or sets the name of the audio RTPC this particle effect instance drives.
		/// </summary>
		public string AudioRtpc
		{
			get { return this.audioRtpc.Text; }
			set { this.audioRtpc.Text = value; }
		}
	}
}