namespace CryEngine.Entities
{
	/// <summary>
	/// Enumeration of entity movement flags.
	/// </summary>
	public enum EntityMoveFlags
	{
		/// <summary>
		/// Position of the entity is changed during movement.
		/// </summary>
		Position = 1 << 1,
		/// <summary>
		/// Orientation of the entity is changed during movement.
		/// </summary>
		Rotation = 1 << 2,
		/// <summary>
		/// Scale of the entity is changed during movement.
		/// </summary>
		Scale = 1 << 3,
		/// <summary>
		/// When set the movement is not propagated.
		/// </summary>
		NoPropagate = 1 << 4,
		/// <summary>
		/// When parent changes his transformation.
		/// </summary>
		FromParent = 1 << 5,

		PhysicsStep = 1 << 13,
		Editor = 1 << 14,
		TrackView = 1 << 15,
		TimeDemo = 1 << 16,
	}
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