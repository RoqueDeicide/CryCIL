using System;
using System.Linq;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a character that is attached to another character.
	/// </summary>
	public struct CharacterAttachment
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
		/// Gets or sets the custom material to use to render this static object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material CustomMaterial
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetReplacementMaterial(this.handle, 0);
			}
			set
			{
				this.AssertInstance();

				AttachedObjectsCommons.SetReplacementMaterial(this.handle, value, 0);
			}
		}
		/// <summary>
		/// Gets the base material that is used to render the attached object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material BaseMaterial
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetBaseMaterial(this.handle, 0);
			}
		}
		/// <summary>
		/// Gets the path to the file this object was loaded from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string FilePath
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetObjectFilePath(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets the attached object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Character Character
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetICharacterInstance(this.handle);
			}
			set
			{
				this.AssertInstance();

				AttachedObjectsCommons.SetICharacterInstance(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal CharacterAttachment(IntPtr handle)
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