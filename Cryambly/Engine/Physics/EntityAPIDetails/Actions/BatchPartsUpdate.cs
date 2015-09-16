using System;
using System.Linq;
using System.Runtime.InteropServices;
using CryCil.Annotations;
using CryCil.Geometry;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information that can be used to update a part of the entity.
	/// </summary>
	public struct PartUpdateInfo
	{
		/// <summary>
		/// Identifier of the entity to update.
		/// </summary>
		public int Id;
		/// <summary>
		/// New position of the part. If set to <see cref="UnusedValue.Vector"/>, then the position is left
		/// unchanged.
		/// </summary>
		public Vector3 NewPosition;
		/// <summary>
		/// New orientation of the part. If set to <see cref="UnusedValue.Quaternion"/>, then the
		/// orientation is left unchanged.
		/// </summary>
		public Quaternion NewOrientation;
	}
	/// <summary>
	/// Encapsulates description of the action that updates a number parts of the physical entity.
	/// </summary>
	/// <remarks>
	/// <para>
	/// When passed to <see cref="PhysicalEntity.ActUpon"/> the return value is an indication of success.
	/// </para>
	/// <para>
	/// Never use objects of this type that were created using a default constructor (they are not
	/// configured properly!).
	/// </para>
	/// </remarks>
	[StructLayout(LayoutKind.Sequential)]
	public struct PhysicsActionBatchPartsUpdate
	{
		#region Fields
		/// <summary>
		/// Pass reference to this field to the <see cref="PhysicalEntity.ActUpon"/> to apply this action
		/// to the physical entity.
		/// </summary>
		[UsedImplicitly] public PhysicsAction Base;
		[UsedImplicitly] private PartUpdateInfo[] infos;
		[UsedImplicitly] private Quaternion qOffs;
		[UsedImplicitly] private Vector3 posOffs;
		[UsedImplicitly] private IntPtr internal0;
		[UsedImplicitly] private IntPtr internal1;
		[UsedImplicitly] private IntPtr internal2;
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="infos">            
		/// An array of object that specifies which parts to update and how.
		/// </param>
		/// <param name="orientationOffset">
		/// Extra orientation offset to apply to each part, mentioned in <paramref name="infos"/>.
		/// </param>
		/// <param name="positionOffset">   
		/// Extra position offset to apply to each part, mentioned in <paramref name="infos"/>.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Given array of part update object has duplicated identifiers. Doesn't get thrown in non-DEBUG
		/// builds.
		/// </exception>
		public PhysicsActionBatchPartsUpdate(PartUpdateInfo[] infos, Quaternion orientationOffset,
											 Vector3 positionOffset = new Vector3())
			: this()
		{
			this.Base = new PhysicsAction(PhysicsActionTypes.BatchPartsUpdate);
			this.infos = infos;
			if (this.infos != null && this.infos.Length == 0)
			{
				this.infos = null;
			}
			this.qOffs = orientationOffset;
			this.posOffs = positionOffset;
#if DEBUG
			var undupedIds = from info in infos
							 group info by info.Id into duplicates
							 where duplicates.Count() == 1
							 select duplicates.Key;
			if (undupedIds.Count() != infos.Length)
			{
				throw new ArgumentException("Given array of part update object has duplicated identifiers.");
			}
#endif
		}
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="infos">         
		/// An array of object that specifies which parts to update and how.
		/// </param>
		/// <param name="positionOffset">
		/// Extra position offset to apply to each part, mentioned in <paramref name="infos"/>.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Given array of part update object has duplicated identifiers. Doesn't get thrown in non-DEBUG
		/// builds.
		/// </exception>
		public PhysicsActionBatchPartsUpdate(PartUpdateInfo[] infos, Vector3 positionOffset = new Vector3())
			: this(infos, Quaternion.Identity, positionOffset)
		{
		}
		#endregion
	}
}