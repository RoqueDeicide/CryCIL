using System;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Rendering.Lighting
{
	/// <summary>
	/// Represents a CryEngine light source.
	/// </summary>
	public struct LightSource
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
		/// Gets or sets a set of properties that define this light source.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public LightProperties Properties
		{
			get
			{
				this.AssertInstance();

				LightProperties properties;
				GetLightProperties(this.handle, out properties);
				return properties;
			}
			set
			{
				this.AssertInstance();

				SetLightProperties(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets world transformation matrix of this light source.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Matrix34 Transformation
		{
			get
			{
				this.AssertInstance();

				Matrix34 matrix;
				GetMatrix(this.handle, out matrix);
				return matrix;
			}
			set
			{
				this.AssertInstance();

				SetMatrix(this.handle, ref value);
			}
		}
		#endregion
		#region Construction
		internal LightSource(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Creates a new light source with default properties.
		/// </summary>
		/// <returns>A valid object, if successful, otherwise an invalid one.</returns>
		public static LightSource Create()
		{
			return CreateLightSource();
		}
		/// <summary>
		/// Deletes the light source.
		/// </summary>
		/// <param name="light">A light source to delete.</param>
		public static void Delete(LightSource light)
		{
			if (light.IsValid)
			{
				DeleteLightSource(light);
			}
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
		private static extern void SetLightProperties(IntPtr handle, ref LightProperties properties);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLightProperties(IntPtr handle, out LightProperties properties);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetMatrix(IntPtr handle, out Matrix34 matrix);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetMatrix(IntPtr handle, ref Matrix34 mat);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern LightSource CreateLightSource();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteLightSource(LightSource pLightSource);
		#endregion
	}
}