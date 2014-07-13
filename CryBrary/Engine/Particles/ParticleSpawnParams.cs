namespace CryEngine
{
	public struct ParticleSpawnParameters
	{
		public GeometryType AttachType { get; set; }
		public GeometryForm AttachForm { get; set; }

		public bool CountPerUnit { get; set; }
		public bool EnableSound { get; set; }

		public float CountScale { get; set; }
		public float SizeScale { get; set; }
		public float SpeedScale { get; set; }

		public float PulsePeriod { get; set; }
		public float Strength { get; set; }
	}
}