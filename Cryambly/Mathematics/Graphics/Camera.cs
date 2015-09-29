using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.MemoryMapping;

namespace CryCil.Graphics
{
	/// <summary>
	/// Enumeration of planes that comprise the camera frustum.
	/// </summary>
	public enum FrustumPlanes
	{
		/// <summary>
		/// Near plane.
		/// </summary>
		Near,
		/// <summary>
		/// Far plane.
		/// </summary>
		Far,
		/// <summary>
		/// Right plane.
		/// </summary>
		Right,
		/// <summary>
		/// Left plane.
		/// </summary>
		Left,
		/// <summary>
		/// Top plane.
		/// </summary>
		Top,
		/// <summary>
		/// Bottom plane.
		/// </summary>
		Bottom,
		/// <summary>
		/// Number of planes.
		/// </summary>
		Count
	}
	/// <summary>
	/// Encapsulates frustum plane data.
	/// </summary>
	public unsafe struct FrustumPlaneData : IEnumerable<Plane>
	{
		private Plane near;
		private Plane far;
		private Plane right;
		private Plane left;
		private Plane top;
		private Plane bottom;
		[UsedImplicitly] private fixed uint m_idx1 [(int)FrustumPlanes.Count];
		[UsedImplicitly] private fixed uint m_idy1 [(int)FrustumPlanes.Count];
		[UsedImplicitly] private fixed uint m_idz1 [(int)FrustumPlanes.Count];
		[UsedImplicitly] private fixed uint m_idx2 [(int)FrustumPlanes.Count];
		[UsedImplicitly] private fixed uint m_idy2 [(int)FrustumPlanes.Count];
		[UsedImplicitly] private fixed uint m_idz2 [(int)FrustumPlanes.Count];
		/// <summary>
		/// Gets the frustum plane.
		/// </summary>
		/// <param name="index">Zero-based index of the frustum plane to get.</param>
		/// <returns>The frustum plane.</returns>
		public Plane GetPlane(FrustumPlanes index)
		{
			switch (index)
			{
				case FrustumPlanes.Near:
					return this.near;
				case FrustumPlanes.Far:
					return this.far;
				case FrustumPlanes.Right:
					return this.right;
				case FrustumPlanes.Left:
					return this.left;
				case FrustumPlanes.Top:
					return this.top;
				case FrustumPlanes.Bottom:
					return this.bottom;
				default:
					throw new ArgumentOutOfRangeException("index");
			}
		}
		/// <summary>
		/// Sets the frustum plane.
		/// </summary>
		/// <param name="index">Zero-based index of the frustum plane to set.</param>
		/// <param name="value">A value to set.</param>
		public void SetPlane(FrustumPlanes index, Plane value)
		{
			switch (index)
			{
				case FrustumPlanes.Near:
					this.near = value;
					break;
				case FrustumPlanes.Far:
					this.far = value;
					break;
				case FrustumPlanes.Right:
					this.right = value;
					break;
				case FrustumPlanes.Left:
					this.left = value;
					break;
				case FrustumPlanes.Top:
					this.top = value;
					break;
				case FrustumPlanes.Bottom:
					this.bottom = value;
					break;
				default:
					throw new ArgumentOutOfRangeException("index");
			}
			// Update the indexes.
			Bytes4 x = new Bytes4(value.Normal.X);
			Bytes4 y = new Bytes4(value.Normal.Y);
			Bytes4 z = new Bytes4(value.Normal.Z);

			uint bitX = x.UnsignedInt >> 31;
			uint bitY = y.UnsignedInt >> 31;
			uint bitZ = z.UnsignedInt >> 31;

			int i = (int)index;

			fixed (uint* ix1 = this.m_idx1, iy1 = this.m_idy1, iz1 = this.m_idz1, ix2 = this.m_idx2,
				iy2 = this.m_idy2, iz2 = this.m_idz2)
			{
				ix1[i] = bitX * 3 + 0;
				ix2[i] = (1 - bitX) * 3 + 0;
				iy1[i] = bitY * 3 + 1;
				iy2[i] = (1 - bitY) * 3 + 1;
				iz1[i] = bitZ * 3 + 2;
				iz2[i] = (1 - bitZ) * 3 + 2;
			}
		}
		/// <summary>
		/// Gets the index of the coordinate of the frustum edge vertex.
		/// </summary>
		/// <param name="planeIndex">???</param>
		/// <param name="edgeVertex">???</param>
		/// <param name="coordIndex">???</param>
		/// <returns>An index that is used for quick intersection tests.</returns>
		public uint GetEdgeIndex(FrustumPlanes planeIndex, uint edgeVertex, uint coordIndex)
		{
			edgeVertex = MathHelpers.Clamp(edgeVertex, 0u, 1u);
			coordIndex = MathHelpers.Clamp(coordIndex, 0u, 2u);
			// Calculate index of the field to access using the edge vertex index and a coordinate index.
			uint fieldIndex = 3 * edgeVertex + coordIndex;
			// Get the pointer to the first index field.
			fixed (uint* ptr = this.m_idx1)
			{
				// Shift the pointer so it points to the correct field.
				uint* tmp = ptr + fieldIndex * 4 * 6;
				// Finally get the index.
				return tmp[(int)planeIndex];
			}
		}
		/// <summary>
		/// Enumerates the frustum planes.
		/// </summary>
		/// <returns>An object that handles enumeration.</returns>
		public IEnumerator<Plane> GetEnumerator()
		{
			yield return this.near;
			yield return this.far;
			yield return this.left;
			yield return this.right;
			yield return this.bottom;
			yield return this.top;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
	/// <summary>
	/// Encapsulates vertices that represent a plane.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PlaneVertices
	{
		/// <summary>
		/// Top-left corner of the plane.
		/// </summary>
		public Vector3 TopLeft;
		/// <summary>
		/// Top-right corner of the plane.
		/// </summary>
		public Vector3 TopRight;
		/// <summary>
		/// Bottom-left corner of the plane.
		/// </summary>
		public Vector3 BottomLeft;
		/// <summary>
		/// Bottom-right corner of the plane.
		/// </summary>
		public Vector3 BottomRight;
	}
	/// <summary>
	/// Encapsulates data that describes a camera.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Camera
	{
		#region Constants
		/// <summary>
		/// Default vertical field of view in radians.
		/// </summary>
		public const float DefaultFov = (float)(75 * Math.PI / 180);
		/// <summary>
		/// Default minimal z-value of the view frustum.
		/// </summary>
		public const float DefaultNearDistance = .25f;
		/// <summary>
		/// Default maximal z-value of the view frustum.
		/// </summary>
		public const float DefaultFarDistance = 1024.0f;
		#endregion
		#region Fields
		/// <summary>
		/// 3x4 matrix that represents orientation and position of this camera in world space.
		/// </summary>
		private Matrix34 matrix;

		/// <summary>
		/// Vertical field of view in radians.
		/// </summary>
		private float fov;
		/// <summary>
		/// Horizontal resolution of the camera surface.
		/// </summary>
		public int width;
		/// <summary>
		/// Vertical resolution of the camera surface.
		/// </summary>
		public int height;
		private float projectionRatio;
		private float pixelAspectRatio;

		private Vector3 topLeftNear;
		private Vector3 topLeftProj;
		private Vector3 topLeftFar;

		//public float		m_asymL, m_asymR, m_asymB, m_asymT;
		private Vector4 asymShifts;

		/// <summary>
		/// Vertices that represent projection plane in camera space.
		/// </summary>
		/// <remarks>These values are usually updated once per frame, they depend on the matrix.</remarks>
		public PlaneVertices ProjectionPlaneVertices;
		/// <summary>
		/// Vertices that represent near plane in camera space.
		/// </summary>
		/// <remarks>These values are usually updated once per frame, they depend on the matrix.</remarks>
		public PlaneVertices NearPlaneVertices;
		/// <summary>
		/// Vertices that represent far plane in camera space.
		/// </summary>
		/// <remarks>These values are usually updated once per frame, they depend on the matrix.</remarks>
		public PlaneVertices FarPlaneVertices;

		/// <summary>
		/// Encapsulates frustum planes.
		/// </summary>
		public FrustumPlaneData CameraFrustumPlanes;

		/// <summary>
		/// Minimal bound of the range of the z-buffer to use for this camera.
		/// </summary>
		private float zRangeMin;
		/// <summary>
		/// Maximal bound of the range of the z-buffer to use for this camera.
		/// </summary>
		private float zRangeMax;
		/// <summary>
		/// Rectangle that represent this camera's viewport on the screen.
		/// </summary>
		public Rectangle Viewport;

		[UsedImplicitly] private IntPtr portal;
		[UsedImplicitly] private ulong scissorInfo;
		[UsedImplicitly] private IntPtr multiCamera;

		private Vector3 OccPosition;
		private int justActivated;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets 3x4 matrix that represents orientation and position of this camera in world space.
		/// </summary>
		public Matrix34 Matrix
		{
			get { return this.matrix; }
			set
			{
				Contract.Assert(value.IsOrthonormal(), "Camera matrix must be orthonormal.");
				this.matrix = value;
				this.UpdateFrustum();
			}
		}
		/// <summary>
		/// Gets the view transformation matrix. It can be used for something.
		/// </summary>
		public Matrix34 ViewMatrix
		{
			get { return this.matrix.Inverted; }
		}
		/// <summary>
		/// Gets or sets vertical field of view in radians.
		/// </summary>
		public float VerticalFieldOfView
		{
			get { return this.fov; }
			set { this.fov = value; }
		}
		/// <summary>
		/// Gets or sets horizontal field of view in radians.
		/// </summary>
		public float HorizontalFieldOfView
		{
			get { return (float)(Math.Atan(Math.Tan(this.fov * 0.5) * this.projectionRatio) * 2); }
			set { this.fov = (float)(Math.Atan(Math.Tan(value * 0.5) / this.projectionRatio) * 2); }
		}
		/// <summary>
		/// Gets angular resolution of this camera.
		/// </summary>
		public float AngularResolution
		{
			get { return this.height / this.fov; }
		}
		/// <summary>
		/// Gets near plane clipping distance of this camera.
		/// </summary>
		public float NearDistance
		{
			get { return this.topLeftFar.Y; }
		}
		/// <summary>
		/// Gets far plane clipping distance of this camera.
		/// </summary>
		public float FarDistance
		{
			get { return this.topLeftNear.Y; }
		}
		/// <summary>
		/// Gets or sets position of this camera in world space.
		/// </summary>
		public Vector3 Position
		{
			get { return this.matrix.Translation; }
			set
			{
				Translation.Set(ref this.matrix, ref value);
				this.UpdateFrustum();
			}
		}
		/// <summary>
		/// Gets the direction this camera is facing.
		/// </summary>
		public Vector3 ViewDirection
		{
			get { return this.matrix.ColumnVector1; }
		}
		/// <summary>
		/// Gets Up direction of this camera.
		/// </summary>
		public Vector3 UpDirection
		{
			get { return this.matrix.ColumnVector2; }
		}
		/// <summary>
		/// Gets or sets a set of Euler angles that represent orientation of this camera in world space.
		/// </summary>
		public EulerAngles Angle
		{
			get { return this.matrix.Angles; }
			set { Rotation.AroundAxes.Set(ref this.matrix, ref value); }
		}
		/// <summary>
		/// Gets or sets a set of values that shift the frustum in 4 directions.
		/// </summary>
		/// <remarks>
		/// <para>X component shifts left plane, Y - right plane, Z - bottom plane, W - top plane.</para>
		/// <para>Not used in culling.</para>
		/// </remarks>
		public Vector4 Asymmetry
		{
			get { return this.asymShifts; }
			set
			{
				this.asymShifts = value;
				this.UpdateFrustum();
			}
		}
		/// <summary>
		/// Gets the ratio between width and height of the camera view surface.
		/// </summary>
		public float ProjectionRatio
		{
			get { return this.projectionRatio; }
		}
		/// <summary>
		/// Gets the ratio between width and height of individual pixels.
		/// </summary>
		public float PixelAspectRatio
		{
			get { return this.pixelAspectRatio; }
		}
		/// <summary>
		/// Gets the width of the camera view surface in pixels.
		/// </summary>
		public int Width
		{
			get { return this.width; }
		}
		/// <summary>
		/// Gets the height of the camera view surface in pixels.
		/// </summary>
		public int Height
		{
			get { return this.height; }
		}
		/// <summary>
		/// Gets or sets the value that indicates whether this camera has just been activated. Used for
		/// motion blur.
		/// </summary>
		public bool JustActivated
		{
			get { return this.justActivated != 0; }
			set { this.justActivated = value ? 1 : 0; }
		}
		/// <summary>
		/// Gets position of the top-left vertex of the near-plane.
		/// </summary>
		public Vector3 NearPlaneTopLeft
		{
			get { return this.topLeftNear; }
		}
		/// <summary>
		/// Gets position of the top-left vertex of the projection-plane.
		/// </summary>
		public Vector3 ProjectionPlaneTopLeft
		{
			get { return this.topLeftProj; }
		}
		/// <summary>
		/// Gets position of the top-left vertex of the far-clip-plane.
		/// </summary>
		public Vector3 FarPlaneTopLeft
		{
			get { return this.topLeftFar; }
		}
		/// <summary>
		/// Minimal bound of the range of the z-buffer to use for this camera.
		/// </summary>
		/// <remarks>
		/// This value is used to specify z-buffer range. Use it only when you want to override default
		/// z-buffer range. Valid values for are: 0 &lt;= zrange &lt;= 1
		/// </remarks>
		public float ZRangeMin
		{
			get { return this.zRangeMin; }
			set { this.zRangeMin = MathHelpers.Clamp(value, 0, 1); }
		}
		/// <summary>
		/// Maximal bound of the range of the z-buffer to use for this camera.
		/// </summary>
		/// <remarks>
		/// This value is used to specify z-buffer range. Use it only when you want to override default
		/// z-buffer range. Valid values for are: 0 &lt;= zrange &lt;= 1
		/// </remarks>
		public float ZRangeMax
		{
			get { return this.zRangeMax; }
			set { this.zRangeMax = MathHelpers.Clamp(value, 0, 1); }
		}
		/// <summary>
		/// Gets an array of vertices that comprise this camera's frustum in world space.
		/// </summary>
		public Vector3[] FrustumVerticesWorld
		{
			get
			{
				Vector3[] vertices = new Vector3[8];

				Matrix33 m33 = this.matrix.Matrix33;

				int i = 0;

				Vector3 pos = this.Position;

				vertices[i++] = m33 * new Vector3(+this.topLeftFar.X, +this.topLeftFar.Y, +this.topLeftFar.Z) + pos;
				vertices[i++] = m33 * new Vector3(+this.topLeftFar.X, +this.topLeftFar.Y, -this.topLeftFar.Z) + pos;
				vertices[i++] = m33 * new Vector3(-this.topLeftFar.X, +this.topLeftFar.Y, -this.topLeftFar.Z) + pos;
				vertices[i++] = m33 * new Vector3(-this.topLeftFar.X, +this.topLeftFar.Y, +this.topLeftFar.Z) + pos;

				vertices[i++] = m33 * new Vector3(+this.topLeftNear.X, +this.topLeftNear.Y, +this.topLeftNear.Z) + pos;
				vertices[i++] = m33 * new Vector3(+this.topLeftNear.X, +this.topLeftNear.Y, -this.topLeftNear.Z) + pos;
				vertices[i++] = m33 * new Vector3(-this.topLeftNear.X, +this.topLeftNear.Y, -this.topLeftNear.Z) + pos;
				vertices[i] = m33 * new Vector3(-this.topLeftNear.X, +this.topLeftNear.Y, +this.topLeftNear.Z) + pos;

				return vertices;
			}
		}
		/// <summary>
		/// Gets an array of vertices that comprise this camera's frustum in camera space.
		/// </summary>
		public Vector3[] FrustumVerticesCamera
		{
			get
			{
				Vector3[] vertices = new Vector3[8];

				int i = 0;

				vertices[i++] = new Vector3(+this.topLeftFar.X, +this.topLeftFar.Y, +this.topLeftFar.Z);
				vertices[i++] = new Vector3(+this.topLeftFar.X, +this.topLeftFar.Y, -this.topLeftFar.Z);
				vertices[i++] = new Vector3(-this.topLeftFar.X, +this.topLeftFar.Y, -this.topLeftFar.Z);
				vertices[i++] = new Vector3(-this.topLeftFar.X, +this.topLeftFar.Y, +this.topLeftFar.Z);

				vertices[i++] = new Vector3(+this.topLeftNear.X, +this.topLeftNear.Y, +this.topLeftNear.Z);
				vertices[i++] = new Vector3(+this.topLeftNear.X, +this.topLeftNear.Y, -this.topLeftNear.Z);
				vertices[i++] = new Vector3(-this.topLeftNear.X, +this.topLeftNear.Y, -this.topLeftNear.Z);
				vertices[i] = new Vector3(-this.topLeftNear.X, +this.topLeftNear.Y, +this.topLeftNear.Z);

				return vertices;
			}
		}
		#endregion
		#region Construction
		/// <summary>
		/// Since we can't define a default constructor for structures, we have to call this function to do
		/// it.
		/// </summary>
		public void Init()
		{
			this.matrix = Matrix34.Identity;
			this.SetFrustum(640, 480);
			this.zRangeMax = 1.0f;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Projects a vector onto this camera's surface.
		/// </summary>
		/// <param name="position">      Location of the vector to project.</param>
		/// <param name="result">        Result of projection.</param>
		/// <param name="customViewport">
		/// Optional <see cref="Rectangle"/> object that can define the on-screen viewport to place the
		/// result into.
		/// </param>
		/// <returns>Indication whether the resultant vector is visible to the camera.</returns>
		public bool Project(Vector3 position, out Vector3 result, Rectangle customViewport = new Rectangle())
		{
			// Set up the projection matrix.
			Matrix44 projection =
				Transformation.Projection.Create
					(
					 this.HorizontalFieldOfView, this.VerticalFieldOfView,
					 this.NearDistance, this.FarDistance
					);
			// Set up the look-at matrix.
			Vector3 cameraPosition = this.Position;
			Matrix44 view = Transformation.View.Create(cameraPosition, cameraPosition + this.ViewDirection);

			Vector4 vector = new Vector4(position, 1);
			result = new Vector3();
			// Define the view port that will be used.
			Rectangle viewport = new Rectangle(0, 0, this.width, this.height);
			if (customViewport.X != 0 || customViewport.Y != 0 ||
				customViewport.Width != 0 || customViewport.Height != 0)
			{
				// Use given viewport if specified.
				viewport.X = customViewport.X;
				viewport.Y = customViewport.Y;
				viewport.Width = customViewport.Width;
				viewport.Height = customViewport.Height;
			}
			// Transform the vector from world space to camera frustum space.
			Transformation.Apply(ref vector, ref view);
			// We are not going to be visible, if we are behind the camera.
			bool visible = vector.Z < 0.0f;
			// Project the vector onto the camera surface.
			Transformation.Apply(ref vector, ref projection);
			// Prevent division by 0 that could have happened later on.
			if (vector.W == 0.0f)
			{
				return false;
			}

			vector.X /= vector.W;
			vector.Y /= vector.W;
			vector.Z /= vector.W;
			// Check if we are within the screen borders.
			visible = visible && Math.Abs(vector.X) < 1 && Math.Abs(vector.Y) < 1;
			// Place the result within the view port.
			result.X = viewport.X + (1 + vector.X) * viewport.Width / 2;
			result.Y = viewport.Y + (1 - vector.Y) * viewport.Height / 2;
			result.Z = vector.Z;

			return visible;
		}
		/// <summary>
		/// Projects an array of vectors onto this camera's surface.
		/// </summary>
		/// <param name="position">      An array of vectors to project.</param>
		/// <param name="result">        Results of projection.</param>
		/// <param name="customViewport">
		/// Optional <see cref="Rectangle"/> object that can define the on-screen viewport to place the
		/// results into.
		/// </param>
		/// <returns>An array of boolean values that indicate visibility of respective vectors.</returns>
		public bool[] Project(Vector3[] position, out Vector3[] result, Rectangle customViewport = new Rectangle())
		{
			// Set up the projection matrix.
			Matrix44 projection =
				Transformation.Projection.Create
					(
					 this.HorizontalFieldOfView, this.VerticalFieldOfView,
					 this.NearDistance, this.FarDistance
					);
			// Set up the look-at matrix.
			Vector3 cameraPosition = this.Position;
			Matrix44 view = Transformation.View.Create(cameraPosition, cameraPosition + this.ViewDirection);

			result = new Vector3[position.Length];
			bool[] visibility = new bool[position.Length];
			// Define the view port that will be used.
			Rectangle viewport = new Rectangle(0, 0, this.width, this.height);
			if (customViewport.X != 0 || customViewport.Y != 0 ||
				customViewport.Width != 0 || customViewport.Height != 0)
			{
				// Use given viewport if specified.
				viewport.X = customViewport.X;
				viewport.Y = customViewport.Y;
				viewport.Width = customViewport.Width;
				viewport.Height = customViewport.Height;
			}

			for (int i = 0; i < position.Length; i++)
			{
				Vector4 vector = new Vector4(position[i], 1);
				// Transform the vector from world space to camera frustum space.
				Transformation.Apply(ref vector, ref view);
				// We are not going to be visible, if we are behind the camera.
				visibility[i] = vector.Z < 0.0f;
				// Project the vector onto the camera surface.
				Transformation.Apply(ref vector, ref projection);
				// Prevent division by 0 that could have happened later on.
				if (vector.W == 0.0f)
				{
					result[i] = new Vector3();
				}
				else
				{
					vector.X /= vector.W;
					vector.Y /= vector.W;
					vector.Z /= vector.W;
					// Check if we are within the screen borders.
					visibility[i] = visibility[i] && Math.Abs(vector.X) < 1 && Math.Abs(vector.Y) < 1;
					// Place the result within the view port.
					result[i].X = viewport.X + (1 + vector.X) * viewport.Width / 2;
					result[i].Y = viewport.Y + (1 - vector.Y) * viewport.Height / 2;
					result[i].Z = vector.Z;
				}
			}
			return visibility;
		}
		/// <summary>
		/// Unprojects the vector from viewport's surface into world space.
		/// </summary>
		/// <param name="position">      Position of the vector in the viewport.</param>
		/// <param name="result">        Unprojected vector.</param>
		/// <param name="customViewport">
		/// Optional <see cref="Rectangle"/> object that can define the on-screen viewport where given
		/// vector was in.
		/// </param>
		/// <returns>True, if operation was successful.</returns>
		public bool Unproject(Vector3 position, out Vector3 result, Rectangle customViewport = new Rectangle())
		{
			// Set up the projection matrix.
			Matrix44 projection =
				Transformation.Projection.Create
					(
					 this.HorizontalFieldOfView, this.VerticalFieldOfView,
					 this.NearDistance, this.FarDistance
					);
			// Set up the look-at matrix.
			Vector3 cameraPosition = this.Position;
			Matrix44 view = Transformation.View.Create(cameraPosition, cameraPosition + this.ViewDirection);

			result = new Vector3();
			// Prepare the unprojection matrix that is a look-at matrix multiplied by projection matrix and
			// then inverted.
			Matrix44 unprojection = view * projection;
			if (!unprojection.Invert())
			{
				return false;
			}
			// Define the view port that will be used.
			Rectangle viewport = new Rectangle(0, 0, this.width, this.height);
			if (customViewport.X != 0 || customViewport.Y != 0 ||
				customViewport.Width != 0 || customViewport.Height != 0)
			{
				// Use given viewport if specified.
				viewport.X = customViewport.X;
				viewport.Y = customViewport.Y;
				viewport.Width = customViewport.Width;
				viewport.Height = customViewport.Height;
			}
			// Move the point out of the viewport's bounds.
			Vector4 vector = new Vector4();
			vector.X = (position.X - viewport.X) * 2 / viewport.Width - 1.0f;
			vector.Y = (position.Y - viewport.Y) * 2 / viewport.Height - 1.0f;
			vector.Z = position.Z;
			vector.W = 1.0f;
			// Unproject the vector.
			Transformation.Apply(ref vector, ref unprojection);
			// Prevent division by 0.
			if (vector.W == 0)
			{
				return false;
			}

			vector.X /= vector.W;
			vector.Y /= vector.W;
			vector.Z /= vector.W;

			return true;
		}
		/// <summary>
		/// Changes view frustum of this camera.
		/// </summary>
		/// <param name="widthPixels">     Horizontal resolution of the camera in pixels.</param>
		/// <param name="heightPixels">    Vertical resolution of the camera in pixels</param>
		/// <param name="_fov">            Vertical field of view.</param>
		/// <param name="nearPlane">       Near clipping distance.</param>
		/// <param name="farPlane">        Far clipping distance.</param>
		/// <param name="pixelAspectRatio">Aspect ratio of the pixels (1 for square pixels.)</param>
		public void SetFrustum(int widthPixels, int heightPixels, float _fov = DefaultFov,
							   float nearPlane = DefaultNearDistance, float farPlane = DefaultFarDistance,
// ReSharper disable ParameterHidesMember
							   float pixelAspectRatio = 1)
// ReSharper restore ParameterHidesMember
		{
			Contract.Assert(nearPlane > 0.001, "Near clipping plane distance is to small.");
			Contract.Assert(farPlane > 0.1, "Far clipping plane distance is to small.");
			Contract.Assert(farPlane > nearPlane,
							"Far clipping plane distance must be greater then near clipping plane distance.");
			Contract.Assert(_fov >= 0.0000001f && _fov < Math.PI, "Vertical field of view must be in range [0, 180].");

			this.fov = _fov;

			this.width = widthPixels; // Surface X-resolution.
			this.height = heightPixels; // Surface Z-resolution.

// ReSharper disable LocalVariableHidesMember
			float width = widthPixels / pixelAspectRatio;
			float height = heightPixels;
			// ReSharper restore LocalVariableHidesMember
			this.projectionRatio = width / height;

			this.pixelAspectRatio = pixelAspectRatio;

			float projLeftTopX = -width * 0.5f;
			float projLeftTopY = (float)((1.0f / Math.Tan(this.fov * 0.5f)) * (height * 0.5f));
			float projLeftTopZ = height * 0.5f;

			this.topLeftProj.X = projLeftTopX;
			this.topLeftProj.Y = projLeftTopY;
			this.topLeftProj.Z = projLeftTopZ;

			Contract.Assert(
						    Math.Abs(Math.Acos(new Vector3(0, this.topLeftProj.Y, this.topLeftProj.Z).Normalized.Y) * 2 - this.fov) <
							0.001);

			float invProjLeftTopY = 1.0f / projLeftTopY;
			this.topLeftNear.X = nearPlane * projLeftTopX * invProjLeftTopY;
			this.topLeftNear.Y = nearPlane;
			this.topLeftNear.Z = nearPlane * projLeftTopZ * invProjLeftTopY;

			//calculate the left/upper edge of the far-plane (=not rotated)
			this.topLeftFar.X = projLeftTopX * (farPlane * invProjLeftTopY);
			this.topLeftFar.Y = farPlane;
			this.topLeftFar.Z = projLeftTopZ * (farPlane * invProjLeftTopY);

			this.UpdateFrustum();
		}
		/// <summary>
		/// Determines whether a point is visible to this camera.
		/// </summary>
		/// <param name="p"><see cref="Vector3"/> object that represents a point in world space.</param>
		/// <returns>
		/// True, if the point is within the camera frustum and therefore visible, otherwise false.
		/// </returns>
		public bool IsPointVisible(Vector3 p)
		{
			return !this.CameraFrustumPlanes.Any(x => x.SignedDistance(p) > 0);
		}
		/// <summary>
		/// Determines whether the sphere is visible to this camera.
		/// </summary>
		/// <param name="s">An object that describes a sphere.</param>
		/// <returns>True, if the sphere is visible to the camera, otherwise false.</returns>
		public bool IsSphereVisible_F(Sphere s)
		{
			return !this.CameraFrustumPlanes.Any(x => x.SignedDistance(s.Center) > s.Radius);
		}
		/// <summary>
		/// Determines whether the Axis-Aligned Bounding Box is visible to this camera.
		/// </summary>
		/// <param name="aabb">An object that represents a bounding box.</param>
		/// <returns>True, if the AABB is visible, otherwise false.</returns>
		public unsafe bool IsAABBVisible_F(BoundingBox aabb)
		{
			float* p = &aabb.Minimum.X;

			for (int i = 0; i < 6; i++)
			{
				uint x = this.CameraFrustumPlanes.GetEdgeIndex((FrustumPlanes)i, 0, 0);
				uint y = this.CameraFrustumPlanes.GetEdgeIndex((FrustumPlanes)i, 0, 1);
				uint z = this.CameraFrustumPlanes.GetEdgeIndex((FrustumPlanes)i, 0, 1);
				if (this.CameraFrustumPlanes.GetPlane((FrustumPlanes)i).SignedDistance(new Vector3(p[x], p[y], p[z])) > 0)
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Determines whether given OBB is visible on the camera.
		/// </summary>
		/// <param name="wpos">Unknown.</param>
		/// <param name="obb"> OBB itself.</param>
		/// <returns>True, if OBB is visible on the camera.</returns>
		public bool IsOBBVisible_F(Vector3 wpos, ref OBB obb)
		{
			Contract.Assert(obb.Matrix.IsOrthonormalRightHanded, "OBB's orientation matrix is not orthonormal.");

			//transform the obb-center into world-space
			Vector3 p = obb.Matrix * obb.Center + wpos;

			//extract the orientation-vectors from the columns of the 3x3 matrix
			//and scale them by the half-lengths
			Vector3 ax = obb.Matrix.Column0 * obb.HalfLength.X;
			Vector3 ay = obb.Matrix.Column1 * obb.HalfLength.Y;
			Vector3 az = obb.Matrix.Column2 * obb.HalfLength.Z;

			//we project the axes of the OBB onto the normal of each of the 6 planes.
			//If the absolute value of the distance from the center of the OBB to the plane
			//is larger then the "radius" of the OBB, then the OBB is outside the frustum.

			return !this.CameraFrustumPlanes.Any
				(
				 plane =>
				 {
					 float t = plane.SignedDistance(p);
					 return t > 0 && t > Math.Abs(plane.Normal * ax) + Math.Abs(plane.Normal * ay) + Math.Abs(plane.Normal * az);
				 }
				);
		}
		#endregion
		#region Utilities
		private void UpdateFrustum()
		{
			// Calculate frustum-edges of projection-plane in camera space.
			Matrix33 m33 = this.matrix.Matrix33;
			Vector3 cltp = m33 * new Vector3(+this.topLeftProj.X, +this.topLeftProj.Y, +this.topLeftProj.Z);
			Vector3 crtp = m33 * new Vector3(-this.topLeftProj.X, +this.topLeftProj.Y, +this.topLeftProj.Z);
			Vector3 clbp = m33 * new Vector3(+this.topLeftProj.X, +this.topLeftProj.Y, -this.topLeftProj.Z);
			Vector3 crbp = m33 * new Vector3(-this.topLeftProj.X, +this.topLeftProj.Y, -this.topLeftProj.Z);

			Vector3 cltn = m33 * new Vector3(+this.topLeftNear.X, +this.topLeftNear.Y, +this.topLeftNear.Z);
			Vector3 crtn = m33 * new Vector3(-this.topLeftNear.X, +this.topLeftNear.Y, +this.topLeftNear.Z);
			Vector3 clbn = m33 * new Vector3(+this.topLeftNear.X, +this.topLeftNear.Y, -this.topLeftNear.Z);
			Vector3 crbn = m33 * new Vector3(-this.topLeftNear.X, +this.topLeftNear.Y, -this.topLeftNear.Z);

			Vector3 cltf = m33 * new Vector3(+this.topLeftFar.X, +this.topLeftFar.Y, +this.topLeftFar.Z);
			Vector3 crtf = m33 * new Vector3(-this.topLeftFar.X, +this.topLeftFar.Y, +this.topLeftFar.Z);
			Vector3 clbf = m33 * new Vector3(+this.topLeftFar.X, +this.topLeftFar.Y, -this.topLeftFar.Z);
			Vector3 crbf = m33 * new Vector3(-this.topLeftFar.X, +this.topLeftFar.Y, -this.topLeftFar.Z);

			this.ProjectionPlaneVertices.TopLeft = cltp;
			this.ProjectionPlaneVertices.TopRight = crtp;
			this.ProjectionPlaneVertices.BottomLeft = clbp;
			this.ProjectionPlaneVertices.BottomRight = crbp;

			this.NearPlaneVertices.TopLeft = cltn;
			this.NearPlaneVertices.TopRight = crtn;
			this.NearPlaneVertices.BottomLeft = clbn;
			this.NearPlaneVertices.BottomRight = crbn;

			this.FarPlaneVertices.TopLeft = cltf;
			this.FarPlaneVertices.TopRight = crtf;
			this.FarPlaneVertices.BottomLeft = clbf;
			this.FarPlaneVertices.BottomRight = crbf;

			Vector3 pos = this.Position;
			// We'll need to negate each frustum plane in case we are not using orthonormal base in
			// right-handed coordinate space.
			int n = this.matrix.IsOrthonormalRightHanded() ? 1 : -1;

			FrustumPlanes plane = FrustumPlanes.Near; // For nicer formatting.
			this.CameraFrustumPlanes.SetPlane(plane, new Plane(crtn + pos, cltn + pos, crbn + pos) * n);
			plane = FrustumPlanes.Right;
			this.CameraFrustumPlanes.SetPlane(plane, new Plane(crbf + pos, crtf + pos, pos) * n);
			plane = FrustumPlanes.Left;
			this.CameraFrustumPlanes.SetPlane(plane, new Plane(cltf + pos, clbf + pos, pos) * n);
			plane = FrustumPlanes.Top;
			this.CameraFrustumPlanes.SetPlane(plane, new Plane(crtf + pos, cltf + pos, pos) * n);
			plane = FrustumPlanes.Bottom;
			this.CameraFrustumPlanes.SetPlane(plane, new Plane(clbf + pos, crbf + pos, pos) * n);
			plane = FrustumPlanes.Far;
			this.CameraFrustumPlanes.SetPlane(plane, new Plane(crtf + pos, crbf + pos, cltf + pos) * n);

			this.OccPosition = pos;
		}
		#endregion
	}
}