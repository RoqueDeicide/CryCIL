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
		public object MotionParameter;
		public object MotionParameterId;
		public ushort NumParams;
	}

	public struct EntityMovementRequest
	{
		public EntityMoveType Type;

		public Vector3 Velocity;
		public Quaternion Rotation;

		public PredictedCharacterStates PredictedCharacterStates;

		public bool AllowStrafe;
		public float ProceduralLeaning;
		public bool Jumping;
	}
}