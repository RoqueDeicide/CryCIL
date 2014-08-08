using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CryEngine.Entities;
using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// Represents a CryENGINE material applicable to any in-game object or entity.
	/// </summary>
	public class Material
	{
		private static readonly List<Material> Materials = new List<Material>();

		internal Material(IntPtr ptr)
		{
			Handle = ptr;
		}
		#region Properties
		/// <summary>
		/// Gets or sets the alphatest.
		/// </summary>
		public float AlphaTest
		{
			get { return GetParam("alpha"); }
			set { SetParam("alpha", value); }
		}

		/// <summary>
		/// Gets or sets the opacity.
		/// </summary>
		public float Opacity
		{
			get { return GetParam("opacity"); }
			set { SetParam("opacity", value); }
		}

		/// <summary>
		/// Gets or sets the glow.
		/// </summary>
		public float Glow
		{
			get { return GetParam("glow"); }
			set { SetParam("glow", value); }
		}

		/// <summary>
		/// Gets or sets the shininess.
		/// </summary>
		public float Shininess
		{
			get { return GetParam("shininess"); }
			set { SetParam("shininess", value); }
		}

		/// <summary>
		/// Gets or sets the diffuse color.
		/// </summary>
		public Color DiffuseColor
		{
			get { return GetParamColor("diffuse"); }
			set { SetParam("diffuse", value); }
		}

		/// <summary>
		/// Gets or sets the emissive color.
		/// </summary>
		public Color EmissiveColor
		{
			get { return GetParamColor("emissive"); }
			set { SetParam("emissive", value); }
		}

		/// <summary>
		/// Gets or sets the specular color.
		/// </summary>
		public Color SpecularColor
		{
			get { return GetParamColor("specular"); }
			set { SetParam("specular", value); }
		}

		/// <summary>
		/// Gets the surface type assigned to this material.
		/// </summary>
		public SurfaceType SurfaceType
		{
			get { return SurfaceType.TryGet(NativeMaterialMethods.GetSurfaceType(Handle)); }
		}

		/// <summary>
		/// Gets the amount of shader parameters in this material. See <see
		/// cref="GetShaderParamName(int)" />
		/// </summary>
		public int ShaderParamCount
		{
			get { return NativeMaterialMethods.GetShaderParamCount(Handle); }
		}

		/// <summary>
		/// Gets the amount of submaterials tied to this material.
		/// </summary>
		public int SubmaterialCount
		{
			get { return NativeMaterialMethods.GetSubmaterialCount(Handle); }
		}

		/// <summary>
		/// Gets or sets the native IMaterial pointer.
		/// </summary>
		internal IntPtr Handle { get; set; }
		#endregion
		#region Statics
		public static Material Find(string name)
		{
			var ptr = NativeMaterialMethods.FindMaterial(name);

			return TryGet(ptr);
		}

		public static Material Create(string name, bool makeIfNotFound = true, bool nonRemovable = false)
		{
			var ptr = NativeMaterialMethods.CreateMaterial(name);

			return TryGet(ptr);
		}

		public static Material Load(string name, bool makeIfNotFound = true, bool nonRemovable = false)
		{
			var ptr = NativeMaterialMethods.LoadMaterial(name, makeIfNotFound, nonRemovable);

			return TryGet(ptr);
		}

		public static Material Get(EntityBase entity, int slot = 0)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (entity == null)
				throw new ArgumentNullException("entity");
#endif

			var ptr = NativeMaterialMethods.GetMaterial(entity.GetIEntity(), slot);
			return TryGet(ptr);
		}

		public static void Set(EntityBase entity, Material mat, int slot = 0)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (entity == null)
				throw new ArgumentNullException("entity");
			if (mat == null)
				throw new ArgumentNullException("mat");
#endif

			NativeMaterialMethods.SetMaterial(entity.GetIEntity(), mat.Handle, slot);
		}

		internal static Material TryGet(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
				return null;

			var mat = Materials.FirstOrDefault(x => x.Handle == ptr);
			if (mat != null)
				return mat;

			mat = new Material(ptr);
			Materials.Add(mat);

			return mat;
		}
		#endregion
		/// <summary>
		/// Gets a submaterial by slot.
		/// </summary>
		/// <param name="slot"></param>
		/// <returns>The submaterial, or null if failed.</returns>
		public Material GetSubmaterial(int slot)
		{
			var ptr = NativeMaterialMethods.GetSubMaterial(Handle, slot);

			return TryGet(ptr);
		}

		/// <summary>
		/// Clones a material
		/// </summary>
		/// <param name="subMaterial">
		/// If negative, all sub materials are cloned, otherwise only the specified slot is
		/// </param>
		/// <returns>The new clone.</returns>
		public Material Clone(int subMaterial = -1)
		{
			var ptr = NativeMaterialMethods.CloneMaterial(Handle, subMaterial);

			return TryGet(ptr);
		}

		/// <summary>
		/// Sets a material parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="value"></param>
		/// <returns>true if successful, otherwise false.</returns>
		public bool SetParam(string paramName, float value)
		{
			return NativeMaterialMethods.SetGetMaterialParamFloat(Handle, paramName, ref value, false);
		}

		/// <summary>
		/// Gets a material's parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <returns>The param value</returns>
		public float GetParam(string paramName)
		{
			float value;
			TryGetParam(paramName, out value);

			return value;
		}

		/// <summary>
		/// Attempts to get parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="value"></param>
		/// <returns>true if successful, otherwise false.</returns>
		public bool TryGetParam(string paramName, out float value)
		{
			value = 0;

			return NativeMaterialMethods.SetGetMaterialParamFloat(Handle, paramName, ref value, true);
		}

		/// <summary>
		/// Sets a material parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="value"></param>
		/// <returns>true if successful, otherwise false.</returns>
		public bool SetParam(string paramName, Color value)
		{
			Vector3 vecValue = new Vector3(value.R, value.G, value.B);
			var result = NativeMaterialMethods.SetGetMaterialParamVec3(Handle, paramName, ref vecValue, false);

			Opacity = value.A;

			return result;
		}

		/// <summary>
		/// Gets a material's parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <returns>The color value</returns>
		public Color GetParamColor(string paramName)
		{
			Color value;
			TryGetParam(paramName, out value);

			return value;
		}

		/// <summary>
		/// Attempts to get parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="value"></param>
		/// <returns>true if successful, otherwise false.</returns>
		public bool TryGetParam(string paramName, out Color value)
		{
			Vector3 vecVal = Vector3.Zero;
			bool result = NativeMaterialMethods.SetGetMaterialParamVec3(Handle, paramName, ref vecVal, true);

			value = new Color {R = vecVal.X, G = vecVal.Y, B = vecVal.Z, A = this.Opacity};

			return result;
		}

		/// <summary>
		/// Sets a shader parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="newVal"></param>
		public void SetShaderParam(string paramName, float newVal)
		{
			NativeMaterialMethods.SetShaderParam(Handle, paramName, newVal);
		}

		/// <summary>
		/// Sets a shader parameter value by name.
		/// </summary>
		/// <param name="param"></param>
		/// <param name="value"></param>
		public void SetShaderParam(ShaderFloatParameter param, float value)
		{
			SetShaderParam(param.GetEngineName(), value);
		}

		/// <summary>
		/// Sets a shader parameter value by name.
		/// </summary>
		/// <param name="paramName"></param>
		/// <param name="newVal"></param>
		public void SetShaderParam(string paramName, Color newVal)
		{
			NativeMaterialMethods.SetShaderParam(Handle, paramName, newVal);
		}

		/// <summary>
		/// Sets a shader parameter value by name.
		/// </summary>
		/// <param name="param"></param>
		/// <param name="value"></param>
		public void SetShaderParam(ShaderColorParameter param, Color value)
		{
			SetShaderParam(param.GetEngineName(), value);
		}

		/// <summary>
		/// Sets a shader parameter value by name.
		/// </summary>
		/// <param name="param"></param>
		/// <param name="value"></param>
		public void SetShaderParam(ShaderColorParameter param, Vector3 value)
		{
			SetShaderParam(param.GetEngineName(), new Color(value.X, value.Y, value.Z));
		}

		/// <summary>
		/// Gets a shader parameter name by index. See <see cref="ShaderParamCount" />
		/// </summary>
		/// <param name="index"></param>
		/// <returns>The shader parameter name.</returns>
		public string GetShaderParamName(int index)
		{
			return NativeMaterialMethods.GetShaderParamName(Handle, index);
		}
		#region Overrides
		public override bool Equals(object obj)
		{
			if (obj is Material)
				return this == obj;

			return false;
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;

				hash = hash * 29 + Handle.GetHashCode();

				return hash;
			}
		}
		#endregion
	}
}