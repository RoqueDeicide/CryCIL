using System;
using System.Runtime.InteropServices;
using CryCil.Annotations;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Encapsulates information about an area of geometry contact.
	/// </summary>
	public unsafe struct GeometryContactArea
	{
#pragma warning disable 169
		[UsedImplicitly] private int type;
		[UsedImplicitly] private int npt;
		[UsedImplicitly] private int nmaxpt;
		[UsedImplicitly] private float minedge;
		[UsedImplicitly] private int* piPrim0;
		[UsedImplicitly] private int* piPrim1;
		[UsedImplicitly] private int* piFeature0;
		[UsedImplicitly] private int* piFeature1;
		[UsedImplicitly] private Vector3* pt;
		[UsedImplicitly] private Vector3 n1;
#pragma warning restore 169
	}
	/// <summary>
	/// Encapsulates information about a border of an areal contact.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct GeometryContactBorder
	{
		#region Fields
#pragma warning disable 169
		[UsedImplicitly] private Vector3* ptborder; // intersection border
		[UsedImplicitly] private int* idxborder; // primitive index | primitive's feature's id << IFEAT_LOG2
		[UsedImplicitly] private int nborderpt;
		[UsedImplicitly] private int bClosed;
		[UsedImplicitly] private Vector3 center;
		[UsedImplicitly] private bool bBorderConsecutive;
#pragma warning restore 169
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of points that form the border.
		/// </summary>
		public int Count
		{
			get { return this.nborderpt; }
		}
		/// <summary>
		/// Indicates whether the border exists.
		/// </summary>
		public bool BorderExists
		{
			get { return this.ptborder != null; }
		}
		/// <summary>
		/// Indicates whether this border is closed.
		/// </summary>
		public bool Closed
		{
			get { return this.bClosed != 0; }
		}
		/// <summary>
		/// Indicates whether points in this border are laid out in consecutive order.
		/// </summary>
		public bool Consecutive
		{
			get { return this.bBorderConsecutive; }
		}
		/// <summary>
		/// Gets the geometric center of the border region.
		/// </summary>
		public Vector3 Center
		{
			get { return this.center; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the point of the border.
		/// </summary>
		/// <param name="index">Zero-based index of the point to get.</param>
		/// <returns>Coordinates of the point.</returns>
		/// <exception cref="NullReferenceException">The border doesn't exist.</exception>
		/// <exception cref="IndexOutOfRangeException">Index is out of range.</exception>
		public Vector3 GetPoint(int index)
		{
			if (this.ptborder == null)
			{
				throw new NullReferenceException("The border doesn't exist.");
			}
			if (index < 0 || index >= this.nborderpt)
			{
				throw new IndexOutOfRangeException();
			}

			return this.ptborder[index];
		}
		#endregion
	}
	/// <summary>
	/// Encapsulates information about a contact between to geometric shapes.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct GeometryContact
	{
		#region Fields
#pragma warning disable 169
		[UsedImplicitly] private double t;
		[UsedImplicitly] private Vector3 pt;
		[UsedImplicitly] private Vector3 n;
		[UsedImplicitly] private Vector3 dir;
		[UsedImplicitly] private int iUnprojMode;
		[UsedImplicitly] private float vel;
		[UsedImplicitly] private int id0;
		[UsedImplicitly] private int id1;
		[UsedImplicitly] private int iPrim0;
		[UsedImplicitly] private int iPrim1;
		[UsedImplicitly] private int iFeature0;
		[UsedImplicitly] private int iFeature1;
		[UsedImplicitly] private int iNode0;
		[UsedImplicitly] private int iNode1;
		[UsedImplicitly] private GeometryContactBorder border;
		[UsedImplicitly] private GeometryContactArea* parea;
#pragma warning restore 169
		#endregion
		#region Properties
		/// <summary>
		/// Gets the time parameter that designates position of the contact along unprojection direction.
		/// </summary>
		public double TimeParameter
		{
			get { return this.t; }
		}
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
		/// <summary>
		/// Gets the direction of unprojection.
		/// </summary>
		public Vector3 Direction
		{
			get { return this.dir; }
		}
		/// <summary>
		/// Gets the speed of the geometric object along the <see cref="Direction"/>. If its less then 0
		/// then <see cref="Normal"/> was used instead of <see cref="Direction"/>.
		/// </summary>
		public float Speed
		{
			get { return this.vel; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int Id0
		{
			get { return this.id0; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int Id1
		{
			get { return this.id1; }
		}
		/// <summary>
		/// Identifier of the object that contains the first geometry that made contact(?) (can be a part
		/// identifier or physical entity identifier).
		/// </summary>
		public int ContainerId0
		{
			get { return this.iPrim0; }
		}
		/// <summary>
		/// Identifier of the object that contains the second geometry that made contact(?) (can be a part
		/// identifier or physical entity identifier).
		/// </summary>
		public int ContainerId1
		{
			get { return this.iPrim1; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int Feature0
		{
			get { return this.iFeature0; }
		}
		/// <summary>
		/// Unknown.
		/// </summary>
		public int Feature1
		{
			get { return this.iFeature1; }
		}
		/// <summary>
		/// Zero-based index of the BV-tree node of the first geometry.
		/// </summary>
		public int NodeIndex0
		{
			get { return this.iNode0; }
		}
		/// <summary>
		/// Zero-based index of the BV-tree node of the second geometry.
		/// </summary>
		public int NodeIndex1
		{
			get { return this.iNode1; }
		}
		/// <summary>
		/// Gets the object that represents a border of the areal contact.
		/// </summary>
		public GeometryContactBorder Border
		{
			get { return this.border; }
		}
		/// <summary>
		/// Gets the value that indicates whether this contact is an areal contact with a border.
		/// </summary>
		public bool HasBorder
		{
			get { return this.border.BorderExists; }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the object that provides information about the areal contact.
		/// </summary>
		/// <param name="area">Resultant object.</param>
		/// <returns>True, if this contact is an areal contact.</returns>
		public bool GetAreaOfContact(out GeometryContactArea area)
		{
			area = new GeometryContactArea();

			if (this.parea == null)
			{
				return false;
			}

			area = *this.parea;
			return true;
		}
		#endregion
	}
}