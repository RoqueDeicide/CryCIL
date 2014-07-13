namespace CryEngine
{
	public enum EntityMoveType
	{
		None = 0,
		Normal,
		Fly,
		Swim,
		ZeroG,

		Impulse,
		JumpInstant,
		JumpAccumulate
	}

	public struct PredictedCharacterStates
	{
		private object motionParameter;
		private object motionParameterId;
		private ushort numParams;
	}

	public struct EntityMovementRequest
	{
		public EntityMoveType type;

		public Vector3 velocity;
		public Quaternion rotation;

		public PredictedCharacterStates predictedCharacterStates;

		public bool allowStrafe;
		public float proceduralLeaning;
		public bool jumping;
	}
}