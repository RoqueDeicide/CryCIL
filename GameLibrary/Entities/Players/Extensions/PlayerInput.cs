using System;
using CryCil;
using CryCil.Engine;
using CryCil.Engine.Data;
using CryCil.Engine.Input.ActionMapping;
using CryCil.Engine.Logic;
using CryCil.Geometry;

namespace GameLibrary.Entities.Players.Extensions
{
	/// <summary>
	/// Represents an extension of the <see cref="Player"/> entity that handles input.
	/// </summary>
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
		#region Events

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
			PlayerActions.MoveLeft += this.PlayerActionsOnMoveLeft;
			PlayerActions.MoveRight += this.PlayerActionsOnMoveRight;
			PlayerActions.MoveForward += this.PlayerActionsOnMoveForward;
			PlayerActions.MoveBackward += this.PlayerActionsOnMoveBackward;
			PlayerActions.MoveUp += this.PlayerActionsOnMoveUp;
			PlayerActions.MoveDown += this.PlayerActionsOnMoveDown;
			PlayerActions.Boost += this.PlayerActionsOnBoost;
			PlayerActions.MouseRotateYaw += this.PlayerActionsOnMouseRotateYaw;
			PlayerActions.MouseRotatePitch += this.PlayerActionsOnMouseRotatePitch;
			PlayerActions.ControllerMoveY += this.PlayerActionsOnControllerMoveY;
			PlayerActions.ControllerMoveX += this.PlayerActionsOnControllerMoveX;
			PlayerActions.ControllerRotateYaw += this.PlayerActionsOnControllerRotateYaw;
			PlayerActions.ControllerRotatePitch += this.PlayerActionsOnControllerRotatePitch;

			ActionMaps.Enable("player");

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
			PlayerActions.MoveLeft -= this.PlayerActionsOnMoveLeft;
			PlayerActions.MoveRight -= this.PlayerActionsOnMoveRight;
			PlayerActions.MoveForward -= this.PlayerActionsOnMoveForward;
			PlayerActions.MoveBackward -= this.PlayerActionsOnMoveBackward;
			PlayerActions.MoveUp -= this.PlayerActionsOnMoveUp;
			PlayerActions.MoveDown -= this.PlayerActionsOnMoveDown;
			PlayerActions.Boost -= this.PlayerActionsOnBoost;
			PlayerActions.MouseRotateYaw -= this.PlayerActionsOnMouseRotateYaw;
			PlayerActions.MouseRotatePitch -= this.PlayerActionsOnMouseRotatePitch;
			PlayerActions.ControllerMoveY -= this.PlayerActionsOnControllerMoveY;
			PlayerActions.ControllerMoveX -= this.PlayerActionsOnControllerMoveX;
			PlayerActions.ControllerRotateYaw -= this.PlayerActionsOnControllerRotateYaw;
			PlayerActions.ControllerRotatePitch -= this.PlayerActionsOnControllerRotatePitch;

			ActionMaps.Disable("player");

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
				// Controller works differently from the mouse, because it has sticks which deviation and not movement defines the motion.
				this.rotationAngles = this.Host.Entity.WorldAngles;
				this.rotationAngles += this.rotationDelta;

				this.rotationAngles.Roll = 0;
				this.rotationAngles.Pitch = Radian.Clamp(this.rotationAngles.Pitch,
														 -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f);
			}
		}
		#endregion
		#region Utilities
		private void PlayerActionsOnControllerRotatePitch(string actionName, ActionActivationMode mode, float value)
		{
			this.rotationDelta.Pitch = (value * controllerSensitivity);
			this.useControllerRotation = true;
		}
		private void PlayerActionsOnControllerRotateYaw(string actionName, ActionActivationMode mode, float value)
		{
			this.rotationDelta.Yaw = (-value * controllerSensitivity);
			this.useControllerRotation = true;
		}
		private void PlayerActionsOnControllerMoveX(string actionName, ActionActivationMode mode, float value)
		{
			this.deltaMovement.X = value;
		}
		private void PlayerActionsOnControllerMoveY(string actionName, ActionActivationMode mode, float value)
		{
			this.deltaMovement.Y = value;
		}
		private void PlayerActionsOnMouseRotatePitch(string actionName, ActionActivationMode mode, float value)
		{
			if (this.useControllerRotation)
			{
				this.useControllerRotation = false;
			}

			float rotation = this.rotationAngles.Pitch - (value * mouseSensitivity);
			this.rotationAngles.Pitch += Radian.Clamp(rotation, -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f);
		}
		private void PlayerActionsOnMouseRotateYaw(string actionName, ActionActivationMode mode, float value)
		{
			if (this.useControllerRotation)
			{
				this.useControllerRotation = false;
			}

			float rotation = this.rotationAngles.Pitch - (value * mouseSensitivity);
			this.rotationAngles.Pitch += Radian.Clamp(rotation, -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f);
		}
		private void PlayerActionsOnBoost(string actionName, ActionActivationMode mode, float value)
		{
			this.boost = mode == ActionActivationMode.OnPress;
		}
		private void PlayerActionsOnMoveDown(string actionName, ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case ActionActivationMode.OnPress:
					this.AddZ(-value);
					break;
				case ActionActivationMode.OnRelease:
					this.AddZ(value);
					break;
			}
		}
		private void PlayerActionsOnMoveUp(string actionName, ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case ActionActivationMode.OnPress:
					this.AddZ(value);
					break;
				case ActionActivationMode.OnRelease:
					this.AddZ(-value);
					break;
			}
		}
		private void PlayerActionsOnMoveBackward(string actionName, ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case ActionActivationMode.OnPress:
					this.AddY(-value);
					break;
				case ActionActivationMode.OnRelease:
					this.AddY(value);
					break;
			}
		}
		private void PlayerActionsOnMoveForward(string actionName, ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case ActionActivationMode.OnPress:
					this.AddY(value);
					break;
				case ActionActivationMode.OnRelease:
					this.AddY(-value);
					break;
			}
		}
		private void PlayerActionsOnMoveRight(string actionName, ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case ActionActivationMode.OnPress:
					this.AddX(value);
					break;
				case ActionActivationMode.OnRelease:
					this.AddX(-value);
					break;
			}
		}
		private void PlayerActionsOnMoveLeft(string actionName, ActionActivationMode mode, float value)
		{
			switch (mode)
			{
				case ActionActivationMode.OnPress:
					this.AddX(-value);
					break;
				case ActionActivationMode.OnRelease:
					this.AddX(value);
					break;
			}
		}
		
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