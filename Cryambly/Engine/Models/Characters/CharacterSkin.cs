using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents the skin of the character.
	/// </summary>
	public struct CharacterSkin
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets the render mesh from the first LOD model that contains the skinning data.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderMesh MainRenderMesh
		{
			get
			{
				this.AssertInstance();

				return GetIRenderMesh(this.handle, 0);
			}
		}
		/// <summary>
		/// Gets the material from the first LOD model.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material MainMaterial
		{
			get
			{
				this.AssertInstance();

				return GetIMaterial(this.handle, 0);
			}
		}
		/// <summary>
		/// Gets the path to the file this skin was loaded from.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string ModelFile
		{
			get
			{
				this.AssertInstance();

				return GetModelFilePath(this.handle);
			}
		}
		#endregion
		#region Construction
		internal CharacterSkin(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new object of this type and loads skinned mesh into it.
		/// </summary>
		/// <param name="file"> Path to the file.</param>
		/// <param name="flags">A set of flags that specify how to load the model.</param>
		public CharacterSkin(string file, CharacterLoadingFlags flags)
		{
			this.handle = Character.LoadModelSKIN(file, flags);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the render mesh that contains the skinning data.
		/// </summary>
		/// <param name="lod">Zero-based index of the LOD model to get skinning data from.</param>
		/// <returns><see cref="CryRenderMesh"/> object that contains skinning data.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public CryRenderMesh GetRenderMesh(uint lod)
		{
			this.AssertInstance();

			return GetIRenderMesh(this.handle, lod);
		}
		/// <summary>
		/// Gets the material.
		/// </summary>
		/// <param name="lod">Zero-based index of the LOD model to get material from.</param>
		/// <returns><see cref="Material"/> that is used by the LOD model.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material GetMaterial(uint lod)
		{
			this.AssertInstance();

			return GetIMaterial(this.handle, 0);
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
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryRenderMesh GetIRenderMesh(IntPtr handle, uint nLOD);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetModelFilePath(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetIMaterial(IntPtr handle, uint nLOD);
		#endregion
	}
}