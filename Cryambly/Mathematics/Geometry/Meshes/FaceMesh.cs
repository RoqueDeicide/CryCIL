using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryCil.Engine.Memory;
using CryCil.Geometry.Csg;
using CryCil.Geometry.Csg.Base;

namespace CryCil.Geometry
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct NativeListHeader
	{
		internal IntPtr ElementsPtr;
		internal int Length;
		internal int Capacity;
	}
	/// <summary>
	/// Represents a triangular mesh where each face is a triangle with it's own set of vertices.
	/// </summary>
	public unsafe class FaceMesh
	{
		#region Fields
		/// <summary>
		/// Indicates whether CSG operations must be done natively.
		/// </summary>
		public static readonly bool NativeCsg = true;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the list of faces that comprise this mesh.
		/// </summary>
		public List<FullFace> Faces { get; private set; }
		/// <summary>
		/// Creates a BSP tree from polygons that form this mesh.
		/// </summary>
		public BspNode<FullFace> BspTree
		{
			get { return new BspNode<FullFace>(this.Faces, null); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates empty mesh.
		/// </summary>
		public FaceMesh()
		{
			this.Faces = new List<FullFace>();
		}
		/// <summary>
		/// Creates a mesh from BSP tree.
		/// </summary>
		/// <param name="bspTree">Root of the BSP tree.</param>
		public FaceMesh(BspNode<FullFace> bspTree)
		{
			this.Faces = bspTree.AllElements;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Combine this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Union"/>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public virtual void Combine(FaceMesh anotherMesh)
		{
			if (NativeCsg)
			{
				this.Faces = FromNativeFaceList(CombineInternal(ToNativeFaceList(this.Faces),
																ToNativeFaceList(anotherMesh.Faces)));
			}
			else
			{
				BspNode<FullFace> a = this.BspTree;
				BspNode<FullFace> b = anotherMesh.BspTree;
				a.Unite(b, null);
				this.Set(a);
			}
		}
		/// <summary>
		/// Intersects this mesh with another.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Intersection"/>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public virtual void Intersect(FaceMesh anotherMesh)
		{
			if (NativeCsg)
			{
				this.Faces = FromNativeFaceList(IntersectInternal(ToNativeFaceList(this.Faces),
																  ToNativeFaceList(anotherMesh.Faces)));
			}
			else
			{
				BspNode<FullFace> a = this.BspTree;
				BspNode<FullFace> b = anotherMesh.BspTree;
				// Cut geometry that is not common for the meshes.
				a.Invert();
				b.CutTreeOut(a, null);
				b.Invert();
				a.CutTreeOut(b, null);
				// Clean up remains.
				b.CutTreeOut(a, null);
				// Combine geometry.
				a.AddElements(b.AllElements, null);
				// Invert everything.
				a.Invert();
				this.Set(a);
			}
		}
		/// <summary>
		/// Subtracts another mesh from this one.
		/// </summary>
		/// <param name="anotherMesh">Another mesh.</param>
		/// <seealso cref="ConstructiveSolidGeometry.Subtract"/>
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		public virtual void Subtract(FaceMesh anotherMesh)
		{
			if (NativeCsg)
			{
				this.Faces = FromNativeFaceList(SubtractInternal(ToNativeFaceList(this.Faces),
																 ToNativeFaceList(anotherMesh.Faces)));
			}
			else
			{
				BspNode<FullFace> a = this.BspTree;
				BspNode<FullFace> b = anotherMesh.BspTree;
				a.Invert();
				a.Unite(b, null);
				a.Invert();
				this.Set(a);
			}
		}
		/// <summary>
		/// Sets this mesh to one represented by a BSP tree.
		/// </summary>
		/// <param name="bspTree">Root of the BSP tree.</param>
		public void Set(BspNode<FullFace> bspTree)
		{
			this.Faces.Clear();
			this.Faces.AddRange(bspTree.AllElements);
		}
		#endregion
		#region Utilities
		/// <exception cref="OutOfMemoryException">Unable to allocate native memory block.</exception>
		internal static IntPtr ToNativeFaceList(List<FullFace> faces)
		{
			// Allocate list header object in native memory.
			NativeListHeader* listHeader = (NativeListHeader*)
				CryMarshal.Allocate((ulong)Marshal.SizeOf(typeof(NativeListHeader))).ToPointer();
			// Assign length and capacity.
			listHeader->Length = faces.Count;
			listHeader->Capacity = faces.Count;
			// Allocate memory for faces.
			listHeader->ElementsPtr =
				CryMarshal.Allocate((ulong)(Marshal.SizeOf(typeof(FullFace)) * faces.Count));
			// Copy faces to the list.
			FullFace* facesPtr = (FullFace*)listHeader->ElementsPtr.ToPointer();
			for (int i = 0; i < faces.Count; i++)
			{
				facesPtr[i] = faces[i];
			}
			return new IntPtr(listHeader);
		}
		internal static List<FullFace> FromNativeFaceList(IntPtr facesPtr)
		{
			NativeListHeader* listHeader = (NativeListHeader*)facesPtr;
			List<FullFace> list = new List<FullFace>(listHeader->Length);
			FullFace* faces = (FullFace*)listHeader->ElementsPtr;
			for (int i = 0; i < listHeader->Length; i++)
			{
				list.Add(faces[i]);
			}
			CryMarshal.Free(facesPtr);
			return list;
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr CombineInternal(IntPtr facesPtr1, IntPtr facesPtr2);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr IntersectInternal(IntPtr facesPtr1, IntPtr facesPtr2);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr SubtractInternal(IntPtr facesPtr1, IntPtr facesPtr2);
		#endregion
	}
}