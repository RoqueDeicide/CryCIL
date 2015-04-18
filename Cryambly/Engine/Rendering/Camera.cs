using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Rendering
{
	/// <summary>
	/// Represents CryEngine camera.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public class Camera
	{
		#region Fields
		[UsedImplicitly]
		private IntPtr handle;

		/// <summary>
		/// Default vertical field of view in radians.
		/// </summary>
		public const float DefaultFieldOfView = (float)(75.0f * Math.PI / 180.0f);
		/// <summary>
		/// Default camera near clipping distance.
		/// </summary>
		public const float DefaultNear = 0.25f;
		/// <summary>
		/// Default camera far clipping distance.
		/// </summary>
		public const float DefaultFar = 1024.0f;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets matrix that represents orientation and position of this camera in 3D world space.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern Matrix34 Matrix
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Gets or sets position of this camera in 3D world space.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern Vector3 Position
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
		/// <summary>
		/// Gets vertical field of view of this camera in radians.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern float FieldOfView
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets horizontal resolution of this camera in pixels.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern int Width
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets vertical resolution of this camera in pixels.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern int Height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets near plane clipping distance of this camera.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern float NearDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets far plane clipping distance of this camera.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern float FarDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		/// <summary>
		/// Gets aspect ratio for pixels. 1.0 means square pixels.
		/// </summary>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		public extern float PixelAspectRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
		#endregion
		#region Events

		#endregion
		#region Construction

		#endregion
		#region Interface
		/// <summary>
		/// Sets the frustum of this camera.
		/// </summary>
		/// <param name="width">           Horizontal resolution in pixels.</param>
		/// <param name="height">          Vertical resolution in pixels.</param>
		/// <param name="fov">             Vertical field of view in radians.</param>
		/// <param name="nearplane">       Near plane clipping distance.</param>
		/// <param name="farPlane">        Far plane clipping distance.</param>
		/// <param name="pixelAspectRatio">Aspect ratio for pixels. 1.0 - square pixels.</param>
		/// <exception cref="NullReferenceException">Camera handle is not valid.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Near clipping plane distance cannot be less then 0.001.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Far clipping plane distance cannot be less then 0.1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Far clipping plane distance must be greater then near distance.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Field of view must be within range [0.0000001;PI].
		/// </exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFrustum(int width, int height, float fov = DefaultFieldOfView,
									  float nearplane = DefaultNear, float farPlane = DefaultFar,
									  float pixelAspectRatio = 1.0f);
		#endregion
		#region Utilities

		#endregion
	}
}