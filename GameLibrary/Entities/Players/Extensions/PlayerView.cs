using System;
using System.Linq;
using CryCil.Engine;
using CryCil.Engine.Logic;
using CryCil.Engine.Rendering.Views;

namespace GameLibrary.Entities.Players.Extensions
{
	/// <summary>
	/// Represents an entity extension that handles the player view.
	/// </summary>
	public class PlayerView : SimpleEntityExtension
	{
		#region Fields
		private ViewController viewController;
		private CryView view;
		private static float fov;
		private const string fovCvarName = "cl_fov";
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Creates a view object.
		/// </summary>
		public override void PostInitialize()
		{
			this.view = CryView.Create(); // Create a view.
			this.view.LinkTo(this.Host); // Link it to the player.
			this.view.Active = true; // Activate the view.

			this.viewController = new ViewController(this.Host);
			this.viewController.Updating += this.UpdateView;

			CryConsole.RegisterVariable(fovCvarName, ref fov, 60.0f, ConsoleFlags.Null,
										"Controls vertical Field-Of-View in degrees.");
		}
		/// <summary>
		/// Releases the view.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether release was caused by the entity getting disposed of.
		/// </param>
		public override void Release(bool disposing)
		{
			this.viewController.Updating -= this.UpdateView;
			this.viewController.Dispose();

			this.view.Release();

			CryConsole.UnregisterVariable(fovCvarName);
		}
		#endregion
		#region Utilities
		private void UpdateView(ViewController sender, ref ViewParameters parameters)
		{
			CryEntity entity = this.Host.Entity;
			parameters.Position = entity.WorldPosition;
			parameters.Rotation = entity.WorldOrientation;
			parameters.Fov = Degree.ToRadian(fov);
		}
		#endregion
	}
}