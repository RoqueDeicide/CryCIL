using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using CryEngine.Mathematics.Geometry.Meshes;

namespace CryEngine.Native
{
	internal static class MeshInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetFaceCount(IntPtr cMeshHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetVertexCount(IntPtr cMeshHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetTexCoordCount(IntPtr cMeshHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetTangentCount(IntPtr cMeshHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetSubSetCount(IntPtr cMeshHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetIndexCount(IntPtr cMeshHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetFaceCount(IntPtr cMeshHandle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetVertexCount(IntPtr cMeshHandle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetTexCoordsCount(IntPtr cMeshHandle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetTexCoordsAndTangentsCount(IntPtr cMeshHandle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void SetIndexCount(IntPtr cMeshHandle, int nNewCount);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetStreamHandle
		(
			IntPtr cMeshHandle,
			[MarshalAs(UnmanagedType.I4)]
			NativeMeshMemoryRegion streamIdentifier
		);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void ReallocateStream
		(
			IntPtr cMeshHandle,
			[MarshalAs(UnmanagedType.I4)]
			NativeMeshMemoryRegion streamIdentifier,
			[MarshalAs(UnmanagedType.I4)]
			int newCount
		);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static void Export(IntPtr staticObjectHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static int GetNumberOfElements
		(
			IntPtr cMeshHandle,
			[MarshalAs(UnmanagedType.I4)]
			NativeMeshMemoryRegion streamIdentifier
		);
	}
}