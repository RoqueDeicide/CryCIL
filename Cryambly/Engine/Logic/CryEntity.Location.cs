using System;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil.Engine.Logic
{
	public partial struct CryEntity
	{
		/// <summary>
		/// Gets or sets 3x4 matrix that represents a transformation of this entity in world space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Matrix34 WorldTransformation
		{
			get
			{
				this.AssertEntity();

				return GetWorldTM(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetWorldTM(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets local transformation matrix.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Matrix34 LocalTransformationMatrix
		{
			get
			{
				this.AssertEntity();

				return GetLocalTM(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetLocalTM(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets Axis-Aligned Bounding Box that represents bounds of this entity in world-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public BoundingBox WorldBounds
		{
			get
			{
				this.AssertEntity();

				BoundingBox box;
				GetWorldBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets Axis-Aligned Bounding Box that represents bounds of this entity in local-space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public BoundingBox LocalBounds
		{
			get
			{
				this.AssertEntity();

				BoundingBox box;
				GetLocalBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets or sets position of this entity in local space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Vector3 LocalPosition
		{
			get
			{
				this.AssertEntity();

				return GetPos(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetPos(this.handle, ref value, true);
			}
		}
		/// <summary>
		/// Gets or sets orientation of this entity in local space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Quaternion LocalOrientation
		{
			get
			{
				this.AssertEntity();

				return GetRotation(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetRotation(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets scale of this entity in local space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Vector3 LocalScale
		{
			get
			{
				this.AssertEntity();

				return GetScale(this.handle);
			}
			set
			{
				this.AssertEntity();

				SetScale(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets position of this entity in world space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Vector3 WorldPosition
		{
			get
			{
				this.AssertEntity();

				return GetWorldPos(this.handle);
			}
		}
		/// <summary>
		/// Gets orientation of this entity in world space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Quaternion WorldOrientation
		{
			get
			{
				this.AssertEntity();

				return GetWorldRotation(this.handle);
			}
		}
		/// <summary>
		/// Gets a set of Euler angles that represent orientation of this entity in world space.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public EulerAngles WorldAngles
		{
			get
			{
				this.AssertEntity();

				return GetWorldAngles(this.handle);
			}
		}
		/// <summary>
		/// Gets a vector that points from world origin in the direction this entity is facing.
		/// </summary>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public Vector3 ForwardDirection
		{
			get
			{
				this.AssertEntity();

				return GetForwardDir(this.handle);
			}
		}
		/// <summary>
		/// Gets location of this entity in local space.
		/// </summary>
		/// <param name="position">   Position of this entity.</param>
		/// <param name="orientation">Orientation of this entity.</param>
		/// <param name="scale">      Scale of this entity.</param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void GetLocation(out Vector3 position, out Quaternion orientation, out Vector3 scale)
		{
			this.AssertEntity();

			GetPosRotScale(this.handle, out position, out orientation, out scale);
		}
		/// <summary>
		/// Sets location of this entity in local space.
		/// </summary>
		/// <param name="position">   Position of this entity.</param>
		/// <param name="orientation">Orientation of this entity.</param>
		/// <param name="scale">      Scale of this entity.</param>
		/// <exception cref="NullReferenceException">This entity is not usable.</exception>
		public void SetLocation(ref Vector3 position, ref Quaternion orientation, ref Vector3 scale)
		{
			this.AssertEntity();

			SetPosRotScale(this.handle, ref position, ref orientation, ref scale);
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetWorldTM(IntPtr handle, ref Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLocalTM(IntPtr handle, ref Matrix34 tm);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix34 GetWorldTM(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix34 GetLocalTM(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetWorldBounds(IntPtr handle, out BoundingBox bbox);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetLocalBounds(IntPtr handle, out BoundingBox bbox);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPos(IntPtr handle, ref Vector3 vPos, bool bRecalcPhyBounds);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetPos(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRotation(IntPtr handle, ref Quaternion qRotation);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion GetRotation(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetScale(IntPtr handle, ref Vector3 vScale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetScale(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPosRotScale(IntPtr handle, out Vector3 pos, out Quaternion rotation, out Vector3 scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPosRotScale(IntPtr handle, ref Vector3 pos, ref Quaternion rotation, ref Vector3 scale);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetWorldPos(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern EulerAngles GetWorldAngles(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Quaternion GetWorldRotation(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 GetForwardDir(IntPtr handle);
	}
}