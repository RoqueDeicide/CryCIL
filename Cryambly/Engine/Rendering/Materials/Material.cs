using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Physics;
using CryCil.Graphics;
using CryCil.Utilities;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents a material that can be assigned to entities.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct Material
	{
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// A collection of sub-materials.
		/// </summary>
		[FieldOffset(0)] public SubMaterials SubMaterials;
		/// <summary>
		/// A collection of material layers.
		/// </summary>
		[FieldOffset(0)] public MaterialLayerCollection Layers;
		#endregion
		#region Static Properties
		/// <summary>
		/// Gets default engine material.
		/// </summary>
		public static Material Default => GetDefault();
		/// <summary>
		/// Gets default engine material for a terrain layer.
		/// </summary>
		public static Material DefaultTerrainLayer => GetDefaultTerrainLayer();
		/// <summary>
		/// Gets default engine material with material layer presets.
		/// </summary>
		public static Material DefaultLayers => GetDefaultLayers();
		/// <summary>
		/// Gets default engine material for drawing helpers.
		/// </summary>
		public static Material DefaultHelper => GetDefaultHelper();
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool IsValid => this.handle != IntPtr.Zero;
		private IntPtr AssertedHandle
		{
			get
			{
				this.AssertInstance();

				return this.handle;
			}
		}
		/// <summary>
		/// Gets number of active references to this object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int ReferenceCount => GetNumRefs(this.AssertedHandle);
		/// <summary>
		/// Gets or sets the name of the material.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public string Name
		{
			get { return GetName(this.AssertedHandle); }
			set { SetName(this.AssertedHandle, value); }
		}
		/// <summary>
		/// Gets or sets a set of flags that specify this material.
		/// </summary>
		public MaterialFlags Flags
		{
			get { return GetFlags(this.AssertedHandle); }
			set { SetFlags(this.AssertedHandle, value); }
		}
		/// <summary>
		/// Indicates whether this material is a default one.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool IsDefault => GetIsDefault(this.AssertedHandle);
		/// <summary>
		/// Sets the camera assigned to this material.
		/// </summary>
		/// <remarks>Used for materials that represent computer displays and such.</remarks>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public Camera Camera
		{
			set { SetCamera(this.AssertedHandle, ref value); }
		}
		/// <summary>
		/// Gets or sets the shader item that specifies the rendering process of this material.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public ShaderItem ShaderItem
		{
			get
			{
				ShaderItem item;
				GetShaderItem(this.AssertedHandle, out item);
				return item;
			}
			set { SetShaderItem(this.AssertedHandle, ref value); }
		}
		/// <summary>
		/// Gets or sets identifier of the surface type this material uses.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public int SurfaceTypeId
		{
			get { return GetSurfaceTypeId(this.AssertedHandle); }
			set { SetSurfaceTypeId(this.AssertedHandle, value); }
		}
		/// <summary>
		/// Gets or sets an object that describes some simple physical properties of the surface this
		/// material is applied to.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public SurfaceType SurfaceType
		{
			get { return GetSurfaceType(this.AssertedHandle); }
			set { SetSurfaceType(this.AssertedHandle, value); }
		}
		/// <summary>
		/// Gets or sets name of the surface type this material uses.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public string SurfaceTypeName
		{
			get { return GetSurfaceTypeName(this.AssertedHandle); }
			set { SetSurfaceTypeName(this.AssertedHandle, value); }
		}
		/// <summary>
		/// Gets or sets the name of the material that is linked to this one.
		/// </summary>
		/// <remarks>
		/// This property doesn't have an effect internally, it's used on high level to allow easy switch to
		/// a new material (e.g. when breaking glass).
		/// </remarks>
		public string LinkedMaterialName
		{
			get { return GetMaterialLinkName(this.AssertedHandle); }
			set { SetMaterialLinkName(this.AssertedHandle, value); }
		}
		/// <summary>
		/// Gets the table that is filled with identifiers of surface types that this material and its
		/// sub-materials use.
		/// </summary>
		public SurfaceTypeTable SurfaceTypeIds
		{
			get
			{
				SurfaceTypeTable table;
				FillSurfaceTypeIds(this.AssertedHandle, out table);
				return table;
			}
		}
		#endregion
		#region Static Interface
		/// <summary>
		/// Creates new material.
		/// </summary>
		/// <param name="name"> Name of the material.</param>
		/// <param name="flags">Flags that specify the material.</param>
		/// <returns>A wrapper object for the material.</returns>
		/// <exception cref="ArgumentNullException">Name of the material cannot be null.</exception>
		public static Material Create(string name, MaterialFlags flags = 0)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "Name of the material cannot be null.");
			}

			return CreateInternal(name, flags);
		}
		/// <summary>
		/// Loads the material from the Xml .mtl file.
		/// </summary>
		/// <param name="file">            Path to the file in virtual file system.</param>
		/// <param name="createIfNotFound">
		/// Indicates whether new material should be created if the file could not be found.
		/// </param>
		/// <param name="nonRemovable">    
		/// Indicates whether should not be allowed to be removed after loading.
		/// </param>
		/// <param name="previewMode">     Indicates whether the material is loaded in preview mode.</param>
		/// <returns>A wrapper object for the material, or null if loading failed.</returns>
		public static Material Load(string file, bool createIfNotFound = true, bool nonRemovable = false,
									bool previewMode = false)
		{
			return LoadInternal(file, createIfNotFound, nonRemovable, previewMode);
		}
		/// <summary>
		/// Loads material from Xml data node.
		/// </summary>
		/// <param name="name">Name of the new material.</param>
		/// <param name="xml"> Object of type <see cref="CryXmlNode"/> that provides the data.</param>
		/// <returns>A wrapper object for the material, or null if loading failed.</returns>
		/// <exception cref="ArgumentNullException">
		/// Name of the material and Xml data provider cannot be null.
		/// </exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		public static Material LoadXml(string name, CryXmlNode xml)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException(nameof(name), "Name of the material cannot be null.");
			}
			if (xml == null)
			{
				throw new ArgumentNullException(nameof(xml), "Xml data provider cannot be null.");
			}
			if (!xml.IsValid)
			{
				throw new ObjectDisposedException(nameof(xml), "The Xml data provider is not usable.");
			}

			return LoadXmlInternal(name, xml.Handle);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Increases the reference count of this material object. Call this when you have multiple
		/// references to the same material object.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void IncrementReferenceCount()
		{
			AddRef(this.AssertedHandle);
		}
		/// <summary>
		/// Decreases the reference count of this material object. Call this when you destroy an object that
		/// held an extra reference to the this material object.
		/// </summary>
		/// <remarks>When reference count reaches zero, the object is deleted.</remarks>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void DecrementReferenceCount()
		{
			Release(this.AssertedHandle);
		}
		/// <summary>
		/// Gets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to get.</param>
		/// <param name="value">Returned value.</param>
		/// <returns>True, if the parameter was found and it is a floating point parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool GetFloatParameter(string name, out float value)
		{
			value = 0;
			return SetGetMaterialParamFloat(this.AssertedHandle, name, ref value, true);
		}
		/// <summary>
		/// Gets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to get.</param>
		/// <param name="value">Returned value.</param>
		/// <returns>True, if the parameter was found and it is a vector parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool GetVectorParameter(string name, out Vector3 value)
		{
			value = new Vector3();
			return SetGetMaterialParamVec3(this.AssertedHandle, name, ref value, true);
		}
		/// <summary>
		/// Sets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to set.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <returns>True, if the parameter was found and it is a floating point parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool SetFloatParameter(string name, float value)
		{
			return SetGetMaterialParamFloat(this.AssertedHandle, name, ref value, false);
		}
		/// <summary>
		/// Sets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to set.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <returns>True, if the parameter was found and it is a vector parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool SetVectorParameter(string name, ref Vector3 value)
		{
			return SetGetMaterialParamVec3(this.AssertedHandle, name, ref value, false);
		}
		/// <summary>
		/// Assigns a texture to this material.
		/// </summary>
		/// <param name="textureId">  Identifier of the texture to assign.</param>
		/// <param name="textureSlot">Type of the texture to set.</param>
		public void SetTexture(int textureId, ResourceTextureTypes textureSlot = ResourceTextureTypes.Diffuse)
		{
			SetTextureInternal(this.AssertedHandle, textureId, textureSlot);
		}
		/// <summary>
		/// Assigns a texture to this material.
		/// </summary>
		/// <param name="texture">    Texture to assign.</param>
		/// <param name="textureSlot">Type of the texture to set.</param>
		public void SetTexture(Texture texture, ResourceTextureTypes textureSlot = ResourceTextureTypes.Diffuse)
		{
			SetTextureInternal(this.AssertedHandle, texture.Identifier, textureSlot);
		}
		/// <summary>
		/// Clones this material.
		/// </summary>
		/// <param name="slot">
		/// If less then 0, copies all sub-materials, otherwise copies only specified slot.
		/// </param>
		/// <returns>A wrapper object for a cloned material.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public Material Clone(int slot = -1)
		{
			return CloneInternal(this.AssertedHandle, slot);
		}
		/// <summary>
		/// Clones this material.
		/// </summary>
		/// <param name="slotName">
		/// Name of the sub-material to copy, if null all of them will be copied.
		/// </param>
		/// <returns>A wrapper object for a cloned material.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public Material Clone(string slotName)
		{
			return CloneInternalName(this.AssertedHandle, slotName);
		}
		/// <summary>
		/// Copies this material into another.
		/// </summary>
		/// <param name="material">Another material object to copy the data to.</param>
		/// <param name="flags">   A set of flags provide additional options for the process.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public void CopyTo(Material material, MaterialCopyFlags flags)
		{
			CopyToInternal(this.AssertedHandle, material, flags);
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
		#region Internal Calls
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefault();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefaultTerrainLayer();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefaultLayers();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefaultHelper();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material CreateInternal(string name, MaterialFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material LoadInternal(string file, bool createIfNotFound, bool nonRemovable,
													bool previewMode);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material LoadXmlInternal(string name, IntPtr xml);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddRef(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Release(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetNumRefs(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetName(IntPtr handle, string pName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, MaterialFlags flags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MaterialFlags GetFlags(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetIsDefault(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetSurfaceTypeId(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSurfaceTypeId(IntPtr handle, int id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSurfaceType(IntPtr handle, SurfaceType sSurfaceTypeName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SurfaceType GetSurfaceType(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetSurfaceTypeName(IntPtr handle, string sSurfaceTypeName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetSurfaceTypeName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetShaderItem(IntPtr handle, ref ShaderItem item);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetShaderItem(IntPtr handle, out ShaderItem item);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int FillSurfaceTypeIds(IntPtr handle, out SurfaceTypeTable table);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetGetMaterialParamFloat(IntPtr handle, string sParamName, ref float v, bool bGet);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetGetMaterialParamVec3(IntPtr handle, string sParamName, ref Vector3 v, bool bGet);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetTextureInternal(IntPtr handle, int textureId, ResourceTextureTypes textureSlot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCamera(IntPtr handle, ref Camera cam);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMaterialLinkName(IntPtr handle, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetMaterialLinkName(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material CloneInternal(IntPtr handle, int slot);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material CloneInternalName(IntPtr handle, string slotName);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CopyToInternal(IntPtr handle, Material material, MaterialCopyFlags flags);
		#endregion
	}
}