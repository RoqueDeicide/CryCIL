using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about the joint that was broken.
	/// </summary>
	public struct JointBreakInfo
	{
		#region Fields
		private readonly int jointId;
		private readonly bool isJoint;
		private readonly int partIdEpicenter;
		private readonly Vector3 point;
		private readonly Vector3 zAxis;
		private readonly int sourcePartId;
		private readonly int targetPartId;
		private readonly int sourcePartMaterial;
		private readonly int targetPartMaterial;
		private readonly PhysicalEntity sourceEntity;
		private readonly PhysicalEntity targetEntity;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the identifier of the broken joint.
		/// </summary>
		public int JointId
		{
			get { return this.jointId; }
		}
		/// <summary>
		/// Gets the value that indicates whether broken joint is a structural joint rather then dynamics
		/// constraint.
		/// </summary>
		public bool IsStructuralJoint
		{
			get { return this.isJoint; }
		}
		/// <summary>
		/// Gets the identifier of the 'seed' part during the update.
		/// </summary>
		public int PartIdEpicenter
		{
			get { return this.partIdEpicenter; }
		}
		/// <summary>
		/// Gets the position of the joint in local frame.
		/// </summary>
		public Vector3 JointPosition
		{
			get { return this.point; }
		}
		/// <summary>
		/// Gets the direction of Z-axis of the joint's local coordinate system.
		/// </summary>
		public Vector3 ZAxis
		{
			get { return this.zAxis; }
		}
		/// <summary>
		/// Gets the identifier of the parent part of the joint.
		/// </summary>
		public int ParentPartId
		{
			get { return this.sourcePartId; }
		}
		/// <summary>
		/// Gets the identifier of the child part of the joint.
		/// </summary>
		public int ChildPartId
		{
			get { return this.targetPartId; }
		}
		/// <summary>
		/// Gets the identifier of the material of parent part of the joint.
		/// </summary>
		public int ParentMaterialId
		{
			get { return this.sourcePartMaterial; }
		}
		/// <summary>
		/// Gets the identifier of the material of child part of the joint.
		/// </summary>
		public int ChildMaterialId
		{
			get { return this.targetPartMaterial; }
		}
		/// <summary>
		/// Gets the parent entity of the joint, if the joint is dynamics constraint.
		/// </summary>
		public PhysicalEntity ParentEntity
		{
			get { return this.sourceEntity; }
		}
		/// <summary>
		/// Gets the child entity of the joint, if the joint is dynamics constraint.
		/// </summary>
		public PhysicalEntity ChildEntity
		{
			get { return this.targetEntity; }
		}
		#endregion
		#region Construction
		internal JointBreakInfo(int jointId, bool isJoint, int partIdEpicenter, Vector3 point, Vector3 zAxis,
								int sourcePartId, int targetPartId, int sourcePartMaterial, int targetPartMaterial,
								PhysicalEntity sourceEntity, PhysicalEntity targetEntity)
		{
			this.jointId = jointId;
			this.isJoint = isJoint;
			this.partIdEpicenter = partIdEpicenter;
			this.point = point;
			this.zAxis = zAxis;
			this.sourcePartId = sourcePartId;
			this.targetPartId = targetPartId;
			this.sourcePartMaterial = sourcePartMaterial;
			this.targetPartMaterial = targetPartMaterial;
			this.sourceEntity = sourceEntity;
			this.targetEntity = targetEntity;
		}
		#endregion
	}
}