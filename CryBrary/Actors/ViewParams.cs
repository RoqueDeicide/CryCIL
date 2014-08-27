using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Actors;
using CryEngine.Entities;
using CryEngine.Mathematics;

namespace CryEngine
{
	/// <summary>
	/// View parameters, commonly used by <see cref="Actor" /> to update the active view.
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

		public byte ViewId;

		//view shake status
		public bool GroundOnly;
		public float ShakingRatio;//whats the amount of shake, from 0.0 to 1.0
		public Quaternion CurrentShakeQuaternion;//what the current angular shake
		public Vector3 CurrentShakeShift;//what is the current translational shake

		// For damping camera movement.
		public EntityId TargetId;  // Who we're watching. 0 == nobody.
		public Vector3 TargetPos;     // Where the target was.
		public float FrameTime;    // current dt.
		public float AngleVel;     // previous rate of change of angle.
		public float Vel;          // previous rate of change of dist between target and camera.
		public float Dist;         // previous dist of cam from target

		//blending
		public bool Blend;
		public float BlendPosSpeed;
		public float BlendRotSpeed;
		public float BlendFOVSpeed;
		public Vector3 BlendPosOffset;
		public Quaternion BlendRotOffset;
		public float BlendFOVOffset;
		public bool JustActivated;

		private ushort ViewIDLast;
		public Vector3 PositionLast;//last view position
		public Quaternion RotationLast;//last view orientation
		public float FOVLast;

		public void SaveLast()
		{
			if (ViewIDLast != 0xff)
			{
				PositionLast = Position;
				RotationLast = Rotation;
				FOVLast = FieldOfView;
			}
			else
				ViewIDLast = 0xfe;
		}
	}
}