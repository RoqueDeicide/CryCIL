using System;
using System.Diagnostics.Contracts;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Engine.Physics;
using CryCil.Engine.Rendering;
using CryCil.Geometry;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Represents a joint in the skeleton pose.
	/// </summary>
	public struct SkeletonJoint
	{
		#region Fields
		private readonly IntPtr handle;
		private readonly int id;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets or sets the entity that hosts the physical representation of this joint.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhysicalEntity Physics
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonPose.GetPhysEntOnJoint(this.handle, this.id);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonPose.SetPhysEntOnJoint(this.handle, this.id, value);
			}
		}
		/// <summary>
		/// Gets identifier of the part of the physical entity that represents this joint.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int PartId
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonPose.GetPhysIdOnJoint(this.handle, this.id);
			}
		}
		/// <summary>
		/// Gets current location of this joint in character-space(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec AbsoluteLocation
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonPose.GetAbsJointById(this.handle, this.id);
			}
		}
		/// <summary>
		/// Gets current location of this joint relative to its parent joint(?).
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Quatvec RelativeLocation
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonPose.GetRelJointById(this.handle, this.id);
			}
		}
		/// <summary>
		/// Gets or sets the static object that hosts geometry of this joint.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public StaticObject StaticObject
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonPose.GetStatObjOnJoint(this.handle, this.id);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonPose.SetStatObjOnJoint(this.handle, this.id, value);
			}
		}
		/// <summary>
		/// Gets or sets the material of this joint.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public Material Material
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return SkeletonPose.GetMaterialOnJoint(this.handle, this.id);
			}
			set
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				SkeletonPose.SetMaterialOnJoint(this.handle, this.id, value);
			}
		}
		#endregion
		#region Construction
		internal SkeletonJoint(IntPtr handle, int id)
		{
			this.handle = handle;
			this.id = id;
		}
		#endregion
		#region Interface

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
		#endregion
	}
	/// <summary>
	/// Represents a collection of objects that represent joints in <see cref="SkeletonPose"/>.
	/// </summary>
	public struct SkeletonJoints
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}
		/// <summary>
		/// Gets the object that represents a joint in <see cref="SkeletonPose"/>.
		/// </summary>
		/// <param name="id">Identifier of the joint.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public SkeletonJoint this[int id]
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return new SkeletonJoint(this.handle, id);
			}
		}
		#endregion
		#region Construction
		internal SkeletonJoints(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface

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
		#endregion
	}
}