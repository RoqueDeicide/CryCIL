using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Geometry;

namespace CryCil.Interops
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct NativeListHeader
	{
		internal IntPtr ElementsPtr;
		internal int Length;
		internal int Capacity;
	}

	internal static unsafe class MeshOps
	{
		internal static IntPtr ToNativeFaceList(List<FullFace> faces)
		{
			// Allocate list header object in native memory.
			NativeListHeader* listHeader = (NativeListHeader*)
				CryMarshal.AllocateMemory((ulong)Marshal.SizeOf(typeof(NativeListHeader)))
						  .ToPointer();
			// Assign length and capacity.
			listHeader->Length = faces.Count;
			listHeader->Capacity = faces.Count;
			// Allocate memory for faces.
			listHeader->ElementsPtr =
				CryMarshal.AllocateMemory((ulong)(Marshal.SizeOf(typeof(FullFace)) * faces.Count));
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
			CryMarshal.FreeMemory(facesPtr);
			return list;
		}
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Combine(IntPtr facesPtr1, IntPtr facesPtr2);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Intersect(IntPtr facesPtr1, IntPtr facesPtr2);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Subtract(IntPtr facesPtr1, IntPtr facesPtr2);
	}
}
