using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Engine.Memory;
using CryCil.Engine.Physics;
using CryCil.Graphics;
using CryCil.Utilities;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents a material that can be assigned to entities.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Material
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Static Properties
		/// <summary>
		/// Gets default engine material.
		/// </summary>
		public static Material Default
		{
			get { return GetDefault(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefault();
		/// <summary>
		/// Gets default engine material for a terrain layer.
		/// </summary>
		public static Material DefaultTerrainLayer
		{
			get { return GetDefaultTerrainLayer(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefaultTerrainLayer();
		/// <summary>
		/// Gets default engine material with material layer presets.
		/// </summary>
		public static Material DefaultLayers
		{
			get { return GetDefaultLayers(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefaultLayers();
		/// <summary>
		/// Gets default engine material for drawing helpers.
		/// </summary>
		public static Material DefaultHelper
		{
			get { return GetDefaultHelper(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Material GetDefaultHelper();
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the name of the material.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public string Name
		{
			get { return this.GetName(); }
			set { this.SetName(value); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern string GetName();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetName(string name);
		/// <summary>
		/// Indicates whether this material is a default one.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool IsDefault
		{
			get { return this.GetIsDefault(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetIsDefault();
		/// <summary>
		/// Gets a collection of sub-materials held by this one.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public SubMaterials SubMaterials
		{
			get { return new SubMaterials(this.handle); }
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Sets the camera assigned to this material.
		/// </summary>
		/// <remarks>Used for materials that represent computer displays and such.</remarks>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public Camera Camera
		{
			set { this.SetCamera(value); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetCamera(Camera value);
		/// <summary>
		/// Gets the shader item that specifies the rendering process of this material.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public ShaderItem ShaderItem
		{
			get { return this.GetShaderItem(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ShaderItem GetShaderItem();
		/// <summary>
		/// Gets an object that describes some simple physical properties of the surface this material is
		/// applied to.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public SurfaceType SurfaceType
		{
			get { return this.GetSurfaceType(); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern SurfaceType GetSurfaceType();
		/// <summary>
		/// Gets the collection of layers this material consists of.
		/// </summary>
		public MaterialLayerCollection Layers
		{
			get { return new MaterialLayerCollection(this.handle); }
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
		/// <exception cref="ArgumentException">Name of the material cannot be an empty string.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Material Create(string name, MaterialFlags flags = 0);
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
		/// <param name="previewMode">     
		/// Indicates whether the material is loaded in preview mode.
		/// </param>
		/// <returns>A wrapper object for the material, or null if loading failed.</returns>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Material Load(string file, bool createIfNotFound = true,
										   bool nonRemovable = false, bool previewMode = false);
		/// <summary>
		/// Loads material from Xml data node.
		/// </summary>
		/// <param name="name">Name of the new material.</param>
		/// <param name="xml"> Object of type <see cref="CryXmlNode"/> that provides the data.</param>
		/// <returns>A wrapper object for the material, or null if loading failed.</returns>
		/// <exception cref="ArgumentNullException">Name of the material cannot be null.</exception>
		/// <exception cref="ArgumentException">Name of the material cannot be an empty string.</exception>
		/// <exception cref="ArgumentNullException">Xml data provider cannot be null.</exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Material LoadXml(string name, CryXmlNode xml);
		#endregion
		#region Interface
		/// <summary>
		/// Saves information about this material to the Xml data node.
		/// </summary>
		/// <param name="xml">Xml date node object.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		/// <exception cref="ArgumentNullException">Xml data provider cannot be null.</exception>
		/// <exception cref="ObjectDisposedException">The Xml data provider is not usable.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Save(CryXmlNode xml);
		/// <summary>
		/// Clones this material.
		/// </summary>
		/// <param name="slot">
		/// If less then 0, copies all sub-materials, otherwise copies only specified slot.
		/// </param>
		/// <returns>A wrapper object for a cloned material.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Material Clone(int slot = -1);
		/// <summary>
		/// Clones this material.
		/// </summary>
		/// <param name="slotName">
		/// Name of the sub-material to copy, if null all of them will be copied.
		/// </param>
		/// <returns>A wrapper object for a cloned material.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Material Clone(string slotName);
		/// <summary>
		/// Copies this material into another.
		/// </summary>
		/// <param name="material">Another material object to copy the data to.</param>
		/// <param name="flags">   A set of flags provide additional options for the process.</param>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyTo(Material material, MaterialCopyFlags flags);
		/// <summary>
		/// Gets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to get.</param>
		/// <param name="value">Returned value.</param>
		/// <returns>True, if the parameter was found and it is a floating point parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetFloatParameter(string name, out float value);
		/// <summary>
		/// Gets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to get.</param>
		/// <param name="value">Returned value.</param>
		/// <returns>True, if the parameter was found and it is a vector parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetVectorParameter(string name, out Vector3 value);
		/// <summary>
		/// Sets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to set.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <returns>True, if the parameter was found and it is a floating point parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetFloatParameter(string name, float value);
		/// <summary>
		/// Sets the value of the material parameter.
		/// </summary>
		/// <param name="name"> Name of the parameter which value to set.</param>
		/// <param name="value">The new value for the parameter.</param>
		/// <returns>True, if the parameter was found and it is a vector parameter.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetVectorParameter(string name, Vector3 value);
		/// <summary>
		/// Gets an array of identifiers of surface types that are used by this material.
		/// </summary>
		/// <returns>An array of identifiers of surface types that are used by this material.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetSurfaceTypesTable();
		/// <summary>
		/// Fills native array with identifiers of surface types that are used by this material.
		/// </summary>
		/// <param name="filledItems">Number of filled items.</param>
		/// <returns>A pointer that will need to be deleted with <see cref="CryMarshal.Free"/>.</returns>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern unsafe int* FillSurfaceTypesTable(out int filledItems);
		#endregion
		#region Utilities
		#endregion
	}
}