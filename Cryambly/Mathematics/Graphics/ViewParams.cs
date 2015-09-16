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
		/// </summary>
		public byte ViewId;

		//view shake status

		/// <summary>
		/// </summary>
		public bool GroundOnly;
		/// <summary>
		/// </summary>
		public float ShakingRatio; //whats the amount of shake, from 0.0 to 1.0
		/// <summary>
		/// </summary>
		public Quaternion CurrentShakeQuaternion; //what the current angular shake
		/// <summary>
		/// </summary>
		public Vector3 CurrentShakeShift; //what is the current translational shake

		// For damping camera movement.

		/// <summary>
		/// </summary>
		public uint TargetId; // Who we're watching. 0 == nobody.
		/// <summary>
		/// </summary>
		public Vector3 TargetPos; // Where the target was.
		/// <summary>
		/// </summary>
		public float FrameTime; // current dt.
		/// <summary>
		/// </summary>
		public float AngleVel; // previous rate of change of angle.
		/// <summary>
		/// </summary>
		public float Vel; // previous rate of change of dist between target and camera.
		/// <summary>
		/// </summary>
		public float Dist; // previous dist of cam from target

		//blending

		/// <summary>
		/// </summary>
		public bool Blend;
		/// <summary>
		/// </summary>
		public float BlendPosSpeed;
		/// <summary>
		/// </summary>
		public float BlendRotSpeed;
		/// <summary>
		/// </summary>
		public float BlendFOVSpeed;
		/// <summary>
		/// </summary>
		public Vector3 BlendPosOffset;
		/// <summary>
		/// </summary>
		public Quaternion BlendRotOffset;
		/// <summary>
		/// </summary>
		public float BlendFOVOffset;
		/// <summary>
		/// </summary>
		public bool JustActivated;

		/// <summary>
		/// </summary>
		private ushort ViewIDLast;
		/// <summary>
		/// </summary>
		public Vector3 PositionLast; //last view position
		/// <summary>
		/// </summary>
		public Quaternion RotationLast; //last view orientation
		/// <summary>
		/// </summary>
		public float FOVLast;

		/// <summary>
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