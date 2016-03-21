using System;
using System.Linq;
using CryCil.Engine.Rendering.Lighting;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a light source that is attached to the character.
	/// </summary>
	public struct LightAttachment
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
		/// Gets the attached light source.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LightSource Source
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetLightSource(this.handle);
			}
		}
		#endregion
		#region Construction
		internal LightAttachment(IntPtr handle)
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
		/// <summary>
		/// Updates attached light source.
		/// </summary>
		/// <param name="properties">Reference to the object that defines the new light properties.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Update(ref LightProperties properties)
		{
			this.AssertInstance();

			AttachedObjectsCommons.LoadLight(this.handle, ref properties);
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