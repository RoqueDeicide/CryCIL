using CryCil;
using CryCil.Engine;
using CryCil.Engine.Logic;
using CryCil.Geometry;

namespace GameLibrary.Entities.Players.Extensions
{
	/// <summary>
	/// Represents an extension that handles player's movement.
	/// </summary>
	public class PlayerMovement : SimpleEntityExtension
	{
		#region Fields
		private const string MovementSpeedCVarName = "playerMovementSpeed";
		private const string BoostMultiplierCVarName = "playerBoostFactor";

		private static float movementSpeed;
		private static float boostMultiplier;
		
		private PlayerInput input;
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, performs final initialization of this extension.
		/// </summary>
		public override void PostInitialize()
		{
			CryConsole.RegisterVariable(MovementSpeedCVarName, ref movementSpeed, 20, ConsoleFlags.Null,
										"Player's movement speed in meters per second.");
			CryConsole.RegisterVariable(BoostMultiplierCVarName, ref boostMultiplier, 2, ConsoleFlags.Null,
										"Player's movement speed boost factor.");

			this.Host.PostUpdatesEnabled = true;

			this.input = this.Host.Extensions.Get<PlayerInput>();
		}
		/// <summary>
		/// When implemented in derived class updates logical state of this extension after most other
		/// stuff is updated.
		/// </summary>
		public override void PostUpdate()
		{
			CryEntity entity = this.Host.Entity;

			EulerAngles angles = this.input.RotationAngles;
			Quaternion orientation = new Quaternion(angles);
			entity.LocalOrientation = orientation;

			Vector3 delta = this.input.DeltaMovement;
			float boostFactor = this.input.Boosting ? boostMultiplier : 1;
			float frameTimeInSeconds = Time.Frame.Milliseconds / 1000.0f;

			// Not sure why we are getting world position. Seems like it relies on the player entity not being linked to anything.
			Vector3 position = entity.WorldPosition;
			Vector3 movement = delta * frameTimeInSeconds * boostFactor * movementSpeed;

			Transformation.Apply(ref movement, ref orientation);
			Translation.Apply(ref position, ref movement);

			entity.LocalPosition = position;
		}
		/// <summary>
		/// Releases this extension.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether release was caused by the entity getting disposed of.
		/// </param>
		public override void Release(bool disposing)
		{
			CryConsole.UnregisterVariable(MovementSpeedCVarName);
			CryConsole.UnregisterVariable(BoostMultiplierCVarName);

			this.input = null;
		}
		#endregion
		#region Utilities

		#endregion
	}
}