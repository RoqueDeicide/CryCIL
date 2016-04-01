using System;
using System.Linq;
using CryCil;
using CryCil.Engine;
using CryCil.Engine.Input.ActionMapping;
using CryCil.Engine.Logic;
using CryCil.Geometry;
using static CryCil.Engine.Input.ActionMapping.ActionActivationMode;
using static CryCil.Engine.Input.InputId;

namespace GameLibrary.Entities.Players.Extensions
{
	/// <summary>
	/// Represents an extension of the <see cref="Player"/> entity that handles input.
	/// </summary>
	[ActionMap("player")]
	public class PlayerInput : SimpleEntityExtension
	{
		#region Static Fields
		private const string MouseSensCvarName = "mouseSensitivity";
		private const string ControllerSensCvarName = "controllerSensitivity";

		private static float mouseSensitivity;
		private static float controllerSensitivity;
		#endregion
		#region Fields
		private EulerAngles rotationDelta;
		private Vector3 deltaMovement;
		private bool useControllerRotation;
		private bool boost;
		private EulerAngles rotationAngles;
		#endregion
		#region Properties
		/// <summary>
		/// Gets a set of Euler angles that describe the orientation of the player.
		/// </summary>
		public EulerAngles RotationAngles => this.rotationAngles;
		/// <summary>
		/// Gets the value that indicates whether player's movement is being boosted.
		/// </summary>
		public bool Boosting => this.boost;
		/// <summary>
		/// Gets the vector that specifies directions of movement.
		/// </summary>
		public Vector3 DeltaMovement => this.deltaMovement;
		#endregion
		#region Construction
		/// <summary>
		/// Initializes a new object of this type.
		/// </summary>
		public PlayerInput()
		{
			this.rotationDelta = new EulerAngles();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Initializes this extension.
		/// </summary>
		public override void PostInitialize()
		{
			this.ListeningToActions = true;

			CryConsole.RegisterVariable(MouseSensCvarName, ref mouseSensitivity, 0.002f, ConsoleFlags.Null,
										"Sensitivity of the mouse.");
			CryConsole.RegisterVariable(ControllerSensCvarName, ref controllerSensitivity, 0.03f,
										ConsoleFlags.Null, "Sensitivity of the controller.");

			if (Game.IsEditor)
			{
				SystemEvents.EditorGameModeChanged += this.SystemEventsOnEditorGameModeChanged;
			}
		}
		/// <summary>
		/// Releases this extension.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether release was caused by the entity getting disposed of.
		/// </param>
		public override void Release(bool disposing)
		{
			this.ListeningToActions = false;

			CryConsole.UnregisterVariable(MouseSensCvarName);
			CryConsole.UnregisterVariable(ControllerSensCvarName);

			if (Game.IsEditor)
			{
				SystemEvents.EditorGameModeChanged -= this.SystemEventsOnEditorGameModeChanged;
			}
		}
		/// <summary>
		/// Updates rotation angles in case the controller was used.
		/// </summary>
		/// <param name="context">The most up-to-date information for this frame.</param>
		public override void Update(ref EntityUpdateContext context)
		{
			if (this.useControllerRotation)
			{
				// Controller works differently from the mouse, because it has sticks which deviation and
				// not movement defines the motion.
				this.rotationAngles = this.Host.Entity.WorldAngles;
				this.rotationAngles += this.rotationDelta;

				this.rotationAngles.Roll = 0;
				this.rotationAngles.Pitch = Radian.Clamp(this.rotationAngles.Pitch,
														 -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f);
			}
		}
		#endregion
		#region Utilities
		#region Keyboard and Controller Button Actions
		// An action that moves the player to the left.
		[Action("move_left", OnPress | OnRelease | Retriggerable, KeyboardMouseInput = A)]
		private void MoveLeft(ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case OnPress:
					this.AddX(-value);
					break;
				case OnRelease:
					this.AddX(value);
					break;
			}
		}
		// An action that moves the player to the right.
		[Action("move_right", OnPress | OnRelease | Retriggerable, KeyboardMouseInput = D)]
		private void MoveRight(ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case OnPress:
					this.AddX(value);
					break;
				case OnRelease:
					this.AddX(-value);
					break;
			}
		}
		// An action that moves the player forwards.
		[Action("move_forward", OnPress | OnRelease | Retriggerable, KeyboardMouseInput = W)]
		private void MoveForward(ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case OnPress:
					this.AddY(value);
					break;
				case OnRelease:
					this.AddY(-value);
					break;
			}
		}
		// An action that moves the player backwards.
		[Action("move_backward", OnPress | OnRelease | Retriggerable, KeyboardMouseInput = S)]
		private void MoveBackward(ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case OnPress:
					this.AddY(-value);
					break;
				case OnRelease:
					this.AddY(value);
					break;
			}
		}
		// An action that moves the player up.
		[Action("move_up", OnPress | OnRelease,
			KeyboardMouseInput = Space, XboxInput = XboxA, OrbisInput = OrbisCross)]
		private void MoveUp(ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case OnPress:
					this.AddZ(value);
					break;
				case OnRelease:
					this.AddZ(-value);
					break;
			}
		}
		// An action that moves the player down.
		[Action("move_down", OnPress | OnRelease,
			KeyboardMouseInput = LeftCtrl, XboxInput = XboxX, OrbisInput = OrbisSquare)]
		private void MoveDown(ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case OnPress:
					this.AddZ(-value);
					break;
				case OnRelease:
					this.AddZ(value);
					break;
			}
		}
		// An action that boosts player's speed.
		[Action("boost", OnPress | OnRelease | Retriggerable,
			KeyboardMouseInput = LeftShift, XboxInput = XboxThumbLeft, OrbisInput = OrbisL3)]
		private void Boost(ActionActivationMode mode, float value)
		{
			this.boost = mode == OnPress;
		}
		#endregion
		#region Mouse Actions
		// An action that change Yaw orientation of player's camera.
		[Action("mouse_rotate_yaw", KeyboardMouseInput = MouseX)]
		private void MouseRotateYaw(ActionActivationMode mode, float value)
		{
			if (this.useControllerRotation)
			{
				this.useControllerRotation = false;
			}

			float rotation = this.rotationAngles.Pitch - (value * mouseSensitivity);
			this.rotationAngles.Pitch += Radian.Clamp(rotation, -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f);
		}
		// An action that change Pitch orientation of player's camera.
		[Action("mouse_rotate_pitch", KeyboardMouseInput = MouseY)]
		private void MouseRotatePitch(ActionActivationMode mode, float value)
		{
			if (this.useControllerRotation)
			{
				this.useControllerRotation = false;
			}

			float rotation = this.rotationAngles.Pitch - (value * mouseSensitivity);
			this.rotationAngles.Pitch += Radian.Clamp(rotation, -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f);
		}
		#endregion
		#region Controller Stick Actions
		// An action that moves the player forwards and backwards when using the controller.
		[Action("controller_move_y", XboxInput = XboxThumbLeftY, OrbisInput = OrbisStickLeftY)]
		private void ControllerMoveY(ActionActivationMode mode, float value)
		{
			this.deltaMovement.Y = value;
		}
		// An action that moves the player left and right when using the controller.
		[Action("controller_move_x", XboxInput = XboxThumbLeftX, OrbisInput = OrbisStickLeftX)]
		private void ControllerMoveX(ActionActivationMode mode, float value)
		{
			this.deltaMovement.X = value;
		}
		// An action that changes Yaw orientation of player's camera when using the controller.
		[Action("controller_rotate_yaw", XboxInput = XboxThumbRightX, OrbisInput = OrbisStickRightX)]
		private void ControllerRotateYaw(ActionActivationMode mode, float value)
		{
			this.rotationDelta.Yaw = (-value * controllerSensitivity);
			this.useControllerRotation = true;
		}
		// An action that changes Pitch orientation of player's camera when using the controller.
		[Action("controller_rotate_pitch", XboxInput = XboxThumbRightY, OrbisInput = OrbisStickRightY)]
		private void ControllerRotatePitch(ActionActivationMode mode, float value)
		{
			this.rotationDelta.Pitch = (value * controllerSensitivity);
			this.useControllerRotation = true;
		}
		#endregion
		private void SystemEventsOnEditorGameModeChanged(bool isTrue)
		{
			// Stop rotation with the controller.
			this.useControllerRotation = false;
			this.rotationAngles = this.Host.Entity.WorldAngles;
		}

		private void AddX(float value)
		{
			AddMovement(ref this.deltaMovement.X, value);
		}
		private void AddY(float value)
		{
			AddMovement(ref this.deltaMovement.Y, value);
		}
		private void AddZ(float value)
		{
			AddMovement(ref this.deltaMovement.Z, value);
		}
		private static void AddMovement(ref float parameter, float value)
		{
			// We use the sign to make sure the value can be used to create a correct range for clamping.
			int sign = Math.Sign(value);
			parameter = MathHelpers.Clamp(parameter + value, -value * sign, value * sign);
		}
		#endregion
	}
}