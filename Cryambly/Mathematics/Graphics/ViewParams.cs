using CryCil.Engine.Logic;
using CryCil.Geometry;

namespace CryCil.Graphics
{
	/// <summary>
	/// View parameters, commonly used by actors to update the active view.
	/// </summary>
	public struct ViewParams
	{
		/// <summary>
		/// View position
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// View orientation
		/// </summary>
		public Quaternion Rotation;
		/// <summary>
		/// Previous local view orientation
		/// </summary>
		public Quaternion LocalRotationLast;

		/// <summary>
		/// custom near clipping plane, 0 means use engine defaults
		/// </summary>
		public float NearPlane;
		/// <summary>
		/// View field of view
		/// </summary>
		public float FieldOfView;
		/// <summary>
		/// Identifier of the view object.
		/// </summary>
		public byte ViewId;

		//view shake status

		/// <summary>
		/// Indicates whether camera shaking should only be applied when on the ground.
		/// </summary>
		public bool GroundOnly;
		/// <summary>
		/// The value between 0 and 1 that represents amount of shake.
		/// </summary>
		public float ShakingRatio;
		/// <summary>
		/// Current angular shake;
		/// </summary>
		public Quaternion CurrentShakeQuaternion;
		/// <summary>
		/// Current translational shake.
		/// </summary>
		public Vector3 CurrentShakeShift;

		// For damping camera movement.

		/// <summary>
		/// Entity that is a target of the view.
		/// </summary>
		public EntityId TargetId;
		/// <summary>
		/// Position of the target.
		/// </summary>
		public Vector3 TargetPosition;
		/// <summary>
		/// Length of the last frame.
		/// </summary>
		public float FrameTime;
		/// <summary>
		/// Previous angular velocity.
		/// </summary>
		public float AngularVelocity;
		/// <summary>
		/// Previous velocity.
		/// </summary>
		public float Velocity;
		/// <summary>
		/// Previous distance from the target.
		/// </summary>
		public float Distance;

		//blending

		/// <summary>
		/// Unknown.
		/// </summary>
		public bool Blend;
		/// <summary>
		/// Unknown.
		/// </summary>
		public float BlendPosSpeed;
		/// <summary>
		/// Unknown.
		/// </summary>
		public float BlendRotSpeed;
		/// <summary>
		/// Unknown.
		/// </summary>
		public float BlendFOVSpeed;
		/// <summary>
		/// Unknown.
		/// </summary>
		public Vector3 BlendPosOffset;
		/// <summary>
		/// Unknown.
		/// </summary>
		public Quaternion BlendRotOffset;
		/// <summary>
		/// Unknown.
		/// </summary>
		public float BlendFOVOffset;
		/// <summary>
		/// Unknown.
		/// </summary>
		public bool JustActivated;

		/// <summary>
		/// Unknown.
		/// </summary>
		private ushort ViewIDLast;
		/// <summary>
		/// Unknown.
		/// </summary>
		public Vector3 PositionLast; //last view position
		/// <summary>
		/// Unknown.
		/// </summary>
		public Quaternion RotationLast; //last view orientation
		/// <summary>
		/// Unknown.
		/// </summary>
		public float FOVLast;

		/// <summary>
		/// Unknown.
		/// </summary>
		public void SaveLast()
		{
			if (this.ViewIDLast != 0xff)
			{
				this.PositionLast = this.Position;
				this.RotationLast = this.Rotation;
				this.FOVLast = this.FieldOfView;
			}
			else
				this.ViewIDLast = 0xfe;
		}
	}
}