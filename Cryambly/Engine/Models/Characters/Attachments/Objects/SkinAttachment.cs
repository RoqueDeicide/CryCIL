using System;
using System.Diagnostics.Contracts;
using System.Linq;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Models.Characters.Attachments
{
	/// <summary>
	/// Represents a skinning mesh that is attached to the character.
	/// </summary>
	public struct SkinAttachment
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
		/// Gets or sets the skinning mesh that is hosted by this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public AttachmentSkin SkinMesh
		{
			get
			{
				this.AssertInstance();

				return AttachedObjectsCommons.GetIAttachmentSkin(this.handle);
			}
			set
			{
				this.AssertInstance();

				AttachedObjectsCommons.SetIAttachmentSkin(this.handle, value);
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
		#endregion
		#region Construction
		internal SkinAttachment(IntPtr handle)
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
		/// Gets the base material that is used to render this model.
		/// </summary>
		/// <param name="lod">
		/// Zero-based index of the LOD model to get the custom material from. Cannot be greater then 5.
		/// </param>
		/// <returns>Base material that is used to render this model.</returns>
		/// <exception cref="IndexOutOfRangeException">
		/// Index must be between 0 and 5 inclusively.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material GetBaseMaterial(uint lod = 0)
		{
			if (lod > 5)
			{
				throw new IndexOutOfRangeException("Index must be between 0 and 5 inclusively.");
			}
			Contract.EndContractBlock();
			this.AssertInstance();

			return AttachedObjectsCommons.GetBaseMaterial(this.handle, lod);
		}
		/// <summary>
		/// Gets the custom material that is used to render this model.
		/// </summary>
		/// <param name="lod">
		/// Zero-based index of the LOD model to get the custom material from. Cannot be greater then 5.
		/// </param>
		/// <returns>Custom material that is used to render this model.</returns>
		/// <exception cref="IndexOutOfRangeException">
		/// Index must be between 0 and 5 inclusively.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material GetCustomMaterial(uint lod = 0)
		{
			if (lod > 5)
			{
				throw new IndexOutOfRangeException("Index must be between 0 and 5 inclusively.");
			}
			Contract.EndContractBlock();
			this.AssertInstance();

			return AttachedObjectsCommons.GetReplacementMaterial(this.handle, lod);
		}
		/// <summary>
		/// Changes the material that is used to render this model.
		/// </summary>
		/// <param name="material">An object that represents the new material to use.</param>
		/// <param name="lod">     
		/// Zero-based index of the LOD model to get the custom material from. Cannot be greater then 5.
		/// </param>
		/// <exception cref="IndexOutOfRangeException">
		/// Index must be between 0 and 5 inclusively.
		/// </exception>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void SetCustomMaterial(Material material, uint lod = 0)
		{
			if (lod > 5)
			{
				throw new IndexOutOfRangeException("Index must be between 0 and 5 inclusively.");
			}
			Contract.EndContractBlock();
			this.AssertInstance();

			AttachedObjectsCommons.SetReplacementMaterial(this.handle, material, lod);
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