using System;
using System.Linq;
using CryCil.Engine.Logic;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents an entity that is attached to the character.
	/// </summary>
	public struct EntityAttachment
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		internal IntPtr Handle => this.handle;
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;

		/// <summary>
		/// Gets the axis-aligned box that encompasses this attached object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public BoundingBox BoundingBox
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetAabb(this.handle);
			}
		}
		/// <summary>
		/// Gets the squared radius of the sphere that encompasses this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public float SquaredRadius
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetRadiusSqr(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the identifier of the attached entity.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public EntityId EntityId
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetEntityId(this.handle);
			}
			set
			{
				this.AssertInstance();

				AttachedObjectsCommons.SetEntityId(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal EntityAttachment(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Releases this object immediately.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Release()
		{
			this.AssertInstance();

			AttachedObjectsCommons.Release(this.handle);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}
		#endregion
	}
}