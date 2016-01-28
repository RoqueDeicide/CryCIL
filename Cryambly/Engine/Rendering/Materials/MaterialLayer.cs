using System;
using System.Runtime.CompilerServices;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents an object that describes a layer of the material.
	/// </summary>
	public struct MaterialLayer
	{
		#region Fields
		[UsedImplicitly] private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this layer is enabled.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool Enabled
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				return IsEnabled(this.handle);
			}
		}
		/// <summary>
		/// Indicates whether this layer is fading out (?).
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public bool FadingOut
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				return DoesFadeOutInternal(this.handle);
			}
		}
		/// <summary>
		/// Gets the shader item that specifies the rendering of this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public ShaderItem ShaderItem
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				return GetShaderItem(this.handle);
			}
		}
		/// <summary>
		/// Gets or sets a set of flags that describe the usage of this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public MaterialLayerUsage Flags
		{
			get
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				return (MaterialLayerUsage)GetFlags(this.handle);
			}
			set
			{
				if (!this.IsValid)
				{
					throw new NullReferenceException("Instance object is not valid.");
				}
				SetFlags(this.handle, (byte)value);
			}
		}
		/// <summary>
		/// Indicates whether this object is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new layer for the material. Use at your own risk.
		/// </summary>
		/// <param name="mat">Material to create the layer for.</param>
		/// <exception cref="ArgumentNullException">Material handle cannot be null.</exception>
		public MaterialLayer(Material mat)
		{
			if (!mat.IsValid)
			{
				throw new ArgumentNullException("mat", "Material handle cannot be null.");
			}

			this.handle = Ctor(mat);
		}
		#endregion
		#region Interface
		/// <summary>
		/// Enables this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public void Enable()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			EnableInternal(this.handle, true);
		}
		/// <summary>
		/// Disables this layer.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public void Disable()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			EnableInternal(this.handle, false);
		}
		/// <summary>
		/// Fades this layer out.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public void FadeOut()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			FadeOutInternal(this.handle, true);
		}
		/// <summary>
		/// Fades this layer in.
		/// </summary>
		/// <exception cref="NullReferenceException">Instance object is not valid.</exception>
		public void FadeIn()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("Instance object is not valid.");
			}
			FadeOutInternal(this.handle, false);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Ctor(Material mat);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void EnableInternal(IntPtr handle, bool bEnable);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsEnabled(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FadeOutInternal(IntPtr handle, bool bFadeOut);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool DoesFadeOutInternal(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ShaderItem GetShaderItem(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetFlags(IntPtr handle, byte nFlags);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte GetFlags(IntPtr handle);
		#endregion
	}
}