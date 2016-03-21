using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;
using CryCil.Utilities;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of value that specify whether to leave the soft body be, or engage the lock on it,
	/// making it unchangeable by the physics, or release previously engaged lock.
	/// </summary>
	public enum SoftBodyLockMode
	{
		/// <summary>
		/// Specifies to neither engage nor release the lock.
		/// </summary>
		Normal,
		/// <summary>
		/// Specifies to engage the lock.
		/// </summary>
		Engage,
		/// <summary>
		/// Specifies to release previously engaged lock.
		/// </summary>
		Release
	}
	/// <summary>
	/// Encapsulates description of the object that is used to query the status of the physical entity that
	/// is a soft body.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.GetStatus"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsStatusSoftBodyVertices
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.GetStatus"/> to query information
		/// about the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsStatus Base;
		[UsedImplicitly] private int nVtx;
		[UsedImplicitly] private StridedPointer pVtx;
		[UsedImplicitly] private StridedPointer pNormals;
		[UsedImplicitly] private GeometryShape pMesh;
		[UsedImplicitly] private SoftBodyLockMode flags;
		[UsedImplicitly] private Quaternion qHost;
		[UsedImplicitly] private Vector3 posHost;
		[UsedImplicitly] private Vector3 pos;
		[UsedImplicitly] private Quaternion q;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of vertices this soft body consists of.
		/// </summary>
		public int VertexCount => this.nVtx;
		/// <summary>
		/// Gets the pointer to the internal array of vertices.
		/// </summary>
		public StridedPointer Vertices => this.pVtx;
		/// <summary>
		/// Gets the pointer to the internal array of normals to each vertex.
		/// </summary>
		public StridedPointer Normals => this.pNormals;
		/// <summary>
		/// Gets the object that represents the current geometry. You need to engage the lock on the soft
		/// body to make sure this geometry doesn't get invalidated while you use it.
		/// </summary>
		public GeometryShape Mesh => this.pMesh;
		/// <summary>
		/// Gets the position of the point the soft body was attached to when this status was queried.
		/// </summary>
		public Vector3 AttachmentPointPosition => this.posHost;
		/// <summary>
		/// Gets the orientation of the point the soft body was attached to when this status was queried.
		/// </summary>
		public Quaternion AttachmentPointOrientation => this.qHost;
		/// <summary>
		/// Gets the position of the soft body at the moment of query.
		/// </summary>
		public Vector3 Position => this.pos;
		/// <summary>
		/// Gets the orientation of the soft body at the moment of query.
		/// </summary>
		public Quaternion Orientation => this.q;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a valid object of this type.
		/// </summary>
		/// <param name="lockMode">
		/// Specifies whether to engage or release the lock on the soft body. Pass
		/// <see cref="SoftBodyLockMode.Normal"/> to do neither.
		/// </param>
		public PhysicsStatusSoftBodyVertices(SoftBodyLockMode lockMode)
			: this()
		{
			this.Base = new PhysicsStatus(PhysicsStatusTypes.SoftBodyVertices);
			this.flags = lockMode;
		}
		#endregion
	}
}