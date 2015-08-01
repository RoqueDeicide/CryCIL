using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using CryCil.Geometry;

namespace CryCil.Engine.Logic
{
	public partial struct CryEntity
	{
		/// <summary>
		/// Gets or sets 3x4 matrix that represents a transformation of this entity in world space.
		/// </summary>
		public Matrix34 WorldTransformationMatrix
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetWorldTM(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetWorldTM(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets .
		/// </summary>
		public Matrix34 LocalTransformationMatrix
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetLocalTM(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetLocalTM(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets Axis-Aligned Bounding Box that represents bounds of this entity in world-space.
		/// </summary>
		public BoundingBox WorldBounds
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				BoundingBox box;
				GetWorldBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets Axis-Aligned Bounding Box that represents bounds of this entity in local-space.
		/// </summary>
		public BoundingBox LocalBounds
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				BoundingBox box;
				GetLocalBounds(this.handle, out box);
				return box;
			}
		}
		/// <summary>
		/// Gets or sets position of this entity in local space.
		/// </summary>
		public Vector3 LocalPosition
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetPos(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetPos(this.handle, ref value, true);
			}
		}
		/// <summary>
		/// Gets or sets orientation of this entity in local space.
		/// </summary>
		public Quaternion LocalOrientation
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetRotation(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetRotation(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets or sets scale of this entity in local space.
		/// </summary>
		public Vector3 LocalScale
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetScale(this.handle);
			}
			set
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				SetScale(this.handle, ref value);
			}
		}
		/// <summary>
		/// Gets position of this entity in world space.
		/// </summary>
		public Vector3 WorldPosition
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetWorldPos(this.handle);
			}
		}
		/// <summary>
		/// Gets orientation of this entity in world space.
		/// </summary>
		public Quaternion WorldOrientation
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetWorldRotation(this.handle);
			}
		}
		/// <summary>
		/// Gets a set of Euler angles that represent orientation of this entity in world space.
		/// </summary>
		public EulerAngles WorldAngles
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetWorldAngles(this.handle);
			}
		}
		/// <summary>
		/// Gets a vector that points from world origin in the direction this entity is facing.
		/// </summary>
		public Vector3 ForwardDirection
		{
			get
			{
				this.AssertEntity();

				Contract.EndContractBlock();

				return GetForwardDir(this.handle);
			}
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