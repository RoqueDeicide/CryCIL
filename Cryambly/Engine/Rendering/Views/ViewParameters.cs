using System;
using System.Linq;
using CryCil.Engine.Logic;
using CryCil.Geometry;

namespace CryCil.Engine.Rendering.Views
{
	/// <summary>
	/// Encapsulates a set of parameters that describe the <see cref="CryView"/> object.
	/// </summary>
	public struct ViewParameters
	{
		#region Fields
		/// <summary>
		/// Coordinates of the view in world-space.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Orientation of the view in world-space.
		/// </summary>
		public Quaternion Rotation;
		/// <summary>
		/// Last rotation delta?
		/// </summary>
		public Quaternion LocalRotationLast;
		/// <summary>
		/// Custom near-clipping plane distance. 0 means use engine defaults.
		/// </summary>
		public float NearPlane;
		/// <summary>
		/// Vertical field of view in radians.
		/// </summary>
		public float Fov;
		/// <summary>
		/// Identifier of the view.
		/// </summary>
		public byte ViewId;

		/// <summary>
		/// Unknown.
		/// </summary>
		public bool GroundOnly;
		/// <summary>
		/// A number between 0 and 1 that specifies amount of shaking.
		/// </summary>
		public float ShakingRatio;
		/// <summary>
		/// Current deviation from normal orientation that is caused by shaking.
		/// </summary>
		public Quaternion CurrentShakeQuaternion;
		/// <summary>
		/// Current deviation from normal position that is caused by shaking.
		/// </summary>
		public Vector3 CurrentShakeShift;

		/// <summary>
		/// Identifier of the entity the camera is following.
		/// </summary>
		public EntityId IdTarget;
		/// <summary>
		/// Last location of the target.
		/// </summary>
		public Vector3 TargetPosition;
		/// <summary>
		/// Length of the last frame.
		/// </summary>
		public float FrameTime;
		/// <summary>
		/// Previous angular velocity.
		/// </summary>
		public float AngleVelocity;
		/// <summary>
		/// Previous velocity.
		/// </summary>
		public float Velocity;
		/// <summary>
		/// Previous distance from target.
		/// </summary>
		public float Distance;

		/// <summary>
		/// Indicates whether view movement is being blended.
		/// </summary>
		public bool Blend;
		/// <summary>
		/// Current position blending speed.
		/// </summary>
		public float BlendPositionSpeed;
		/// <summary>
		/// Current rotation blending speed.
		/// </summary>
		public float BlendRotationSpeed;
		/// <summary>
		/// Current FOV blending speed.
		/// </summary>
		public float BlendFovSpeed;
		/// <summary>
		/// Current position blending offset.
		/// </summary>
		public Vector3 BlendPositionOffset;
		/// <summary>
		/// Current rotation blending offset.
		/// </summary>
		public Quaternion BlendRotationOffset;
		/// <summary>
		/// Current FOV blending offset.
		/// </summary>
		public float BlendFovOffset;
		/// <summary>
		/// Indicates whether blending has just been activated.
		/// </summary>
		public bool JustActivated;

#pragma warning disable 414
		private readonly byte viewIdLast;
		// ReSharper disable NotAccessedField.Local
		private readonly Vector3 positionLast;
		private readonly Quaternion rotationLast;
		// ReSharper restore NotAccessedField.Local
		private readonly float fovLast;
#pragma warning restore 414
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewParameters"/> class.
		/// </summary>
		/// <param name="worldPosition">Position of the view in world-space.</param>
		public ViewParameters(Vector3 worldPosition) : this()
		{
			this.Position = worldPosition;
			this.Rotation = Quaternion.Identity;
			this.LocalRotationLast = Quaternion.Identity;
			this.CurrentShakeQuaternion = Quaternion.Identity;
			this.Blend = true;
			this.BlendPositionSpeed = 5;
			this.BlendRotationSpeed = 10;
			this.BlendFovSpeed = 5;
			this.BlendRotationOffset = Quaternion.Identity;

			this.viewIdLast = 0;
			this.positionLast = new Vector3();
			this.rotationLast = Quaternion.Identity;
			this.fovLast = 0;
		}
		#endregion
	}
}