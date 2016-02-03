using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
using CryCil.Engine.Logic;

namespace CryCil.Engine.Rendering.Views
{
	/// <summary>
	/// Defines signature of methods that can handle <see cref="ViewController"/> update events.
	/// </summary>
	/// <param name="sender">    An object that raised the event.</param>
	/// <param name="parameters">
	/// Reference to the set of parameters that can be changed to change the view.
	/// </param>
	public delegate void GameViewUpdateHandler(ViewController sender, ref ViewParameters parameters);
	/// <summary>
	/// Represents an object that handle updates to the view that is used by the game.
	/// </summary>
	public class ViewController : IDisposable
	{
		#region Fields
		private IntPtr handle;
		private MonoEntity entity;
		#endregion
		#region Events
		/// <summary>
		/// Occurs when this view is being updated. This is the moment to change the camera position.
		/// </summary>
		public event GameViewUpdateHandler Updating;
		/// <summary>
		/// Occurs when this view has been updated.
		/// </summary>
		public event GameViewUpdateHandler Updated;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new view controller.
		/// </summary>
		/// <param name="entity">
		/// An optional entity this controller will be become automatically linked, if it's specified.
		/// </param>
		public ViewController(MonoEntity entity = null)
		{
			this.handle = Create(this);
			if (entity != null)
			{
				this.Link(entity);
			}
		}
		/// <summary>
		/// Destroys this object.
		/// </summary>
		~ViewController()
		{
			this.Dispose();
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases this controller.
		/// </summary>
		public void Dispose()
		{
			if (this.entity != null)
			{
				this.Unlink();
			}
			if (this.handle != IntPtr.Zero)
			{
				DeleteThis(this.handle);
				this.handle = IntPtr.Zero;
			}
		}
		/// <summary>
		/// Links this controller to the entity.
		/// </summary>
		/// <param name="entity">An entity to link this view to.</param>
		public void Link([CanBeNull] MonoEntity entity)
		{
			if (entity == null)
			{
				this.Unlink();
				return;
			}

			this.entity = entity;
			Link(this.handle, entity.Entity);
		}
		/// <summary>
		/// Unlinks this controller from its current entity.
		/// </summary>
		public void Unlink()
		{
			if (this.entity != null)
			{
				Unlink(this.handle, this.entity.Entity);
				this.entity = null;
			}
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Create(ViewController view);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteThis(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Link(IntPtr handle, CryEntity entity);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Unlink(IntPtr handle, CryEntity entity);

		[UnmanagedThunk("Raises event Updating")]
		private void OnUpdating(ref ViewParameters parameters)
		{
			var handler = this.Updating;
			if (handler != null)
			{
				handler(this, ref parameters);
			}
		}
		[UnmanagedThunk("Raises event Updated")]
		private void OnUpdated(ref ViewParameters parameters)
		{
			var handler = this.Updated;
			if (handler != null)
			{
				handler(this, ref parameters);
			}
		}
		#endregion
	}
}