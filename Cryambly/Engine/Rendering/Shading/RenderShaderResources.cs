using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Graphics;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents an object that provides resources for shaders.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RenderShaderResources
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets the value that is in range from 0 to 1 where 0 means that no alpha testing will be
		/// done, any value between 0 and 1 will make the object semi-transparent and the value of 1 means
		/// full invisibility.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public float Transparency
		{
			get { return this.GetTransparency(); }
			set { this.SetTransparency(value); }
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float GetTransparency();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTransparency(float value);
		/// <summary>
		/// Indicates whether this shader will make the rendered object glow in some way.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool IsGlowing
		{
			get { return this.GetStrength(ResourceTextureTypes.Glow) > 0.0f; }
		}
		/// <summary>
		/// Indicates whether this shader will make the rendered object opaque by having the strength value
		/// of the opacity map less then 1.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool IsTransparent
		{
			get { return this.GetStrength(ResourceTextureTypes.Opacity) < 1.0f; }
		}
		/// <summary>
		/// Indicates whether this shader will make the rendered object alpha tested.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool IsAlphaTested
		{
			get { return this.Transparency > 0.01f /*0.0f*/; }
		}
		/// <summary>
		/// Indicates whether this shader will make the rendered object completely invisible.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public bool IsInvisible
		{
			get
			{
				float o = this.GetStrength(ResourceTextureTypes.Opacity);
				float a = this.Transparency;

				return o == 0.0f || a == 1.0f || o <= a;
			}
		}
		/// <summary>
		/// Gets the collection of shader parameters.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		public ShaderParameters Parameters
		{
			get { return this.GetParameters(); }
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Updates constant variables defined in shader code.
		/// </summary>
		/// <param name="shader">Shader object for which to update the constants.</param>
		/// <remarks>Invoke this method after changing any of the shader parameters.</remarks>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdateConstants(Shader shader);
		/// <summary>
		/// Gets the color value that is used to modify appearance of a texture map.
		/// </summary>
		/// <param name="textureType">
		/// Type of the texture map the color is supposed to modify the appearance of.
		/// </param>
		/// <returns>A color object.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ColorSingle GetColor(ResourceTextureTypes textureType);
		/// <summary>
		/// Sets the color value that is used to modify appearance of a texture map.
		/// </summary>
		/// <param name="textureType">
		/// Type of the texture map the color is supposed to modify the appearance of.
		/// </param>
		/// <param name="color">      A new color for the texture map.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetColor(ResourceTextureTypes textureType, ColorSingle color);
		/// <summary>
		/// Gets the value that specifies expressiveness of a texture map.
		/// </summary>
		/// <param name="textureType">
		/// Type of the texture map which expressiveness is defined by the value.
		/// </param>
		/// <returns>A strength value between 0 and 1.</returns>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetStrength(ResourceTextureTypes textureType);
		/// <summary>
		/// Sets the color value that is used to modify appearance of a texture map.
		/// </summary>
		/// <param name="textureType">
		/// Type of the texture map which expressiveness is defined by the value.
		/// </param>
		/// <param name="value">      A new strength value between 0 and 1.</param>
		/// <exception cref="NullReferenceException">Instance object is invalid.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetStrength(ResourceTextureTypes textureType, float value);
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern ShaderParameters GetParameters();
		#endregion
	}
}