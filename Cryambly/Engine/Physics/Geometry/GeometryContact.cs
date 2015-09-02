using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about an area of geometry contact.
	/// </summary>
	public unsafe struct GeometryContactArea
	{
		int type;
		int npt;
		int nmaxpt;
		float minedge;
		int *piPrim0;
		int *piPrim1;
		int *piFeature0;
		int *piFeature1;
		Vector3 *pt;
		Vector3 n1; // normal of other object surface (or edge)
	}
	/// <summary>
	/// Encapsulates information about a contact between to geometric shapes.
	/// </summary>
	public unsafe struct GeometryContact
	{
		#region Fields
		[UsedImplicitly] private double t;
		[UsedImplicitly] private Vector3 pt;
		[UsedImplicitly] private Vector3 n;
		[UsedImplicitly] private Vector3 dir;
		[UsedImplicitly] private int iUnprojMode;
		[UsedImplicitly] private float vel; // original velocity along this direction, <0 if least squares normal was used
		[UsedImplicitly] private int id0; // external ids for colliding geometry parts
		[UsedImplicitly] private int id1; // external ids for colliding geometry parts
		[UsedImplicitly] private int iPrim0;
		[UsedImplicitly] private int iPrim1;
		[UsedImplicitly] private int iFeature0;
		[UsedImplicitly] private int iFeature1;
		[UsedImplicitly] private int iNode0; // BV-tree nodes of contacting primitives
		[UsedImplicitly] private int iNode1; // BV-tree nodes of contacting primitives
		[UsedImplicitly] private Vector3 *ptborder; // intersection border
		[UsedImplicitly] private int *idxborder0; // primitive index | primitive's feature's id << IFEAT_LOG2
		[UsedImplicitly] private int *idxborder1; // primitive index | primitive's feature's id << IFEAT_LOG2
		[UsedImplicitly] private int nborderpt;
		[UsedImplicitly] private int bClosed;
		[UsedImplicitly] private Vector3 center;
		[UsedImplicitly] private bool bBorderConsecutive;
		[UsedImplicitly] private GeometryContactArea *parea;
		#endregion
		#region Properties
		///// <summary>
		///// Gets the time stamp when the contact was detected(?).
		///// </summary>
		//public double TimeStamp
		//{
		//	get { return this.t; }
		//}

		/// <summary>
		/// Gets coordinates of the point of contact.
		/// </summary>
		public Vector3 Point
		{
			get { return this.pt; }
		}
		/// <summary>
		/// Gets the direction of the normal to the point of contact.
		/// </summary>
		public Vector3 Normal
		{
			get { return this.n; }
		}

		///// <summary>
		///// Gets the direction of unprojection.
		///// </summary>
		//public Vector3 Direction
		//{
		//	get { return this.dir; }
		//}


		#endregion
		#region Events
		
		#endregion
		#region Construction
		
		#endregion
		#region Interface
		
		#endregion
		#region Utilities
		
		#endregion
	}
}