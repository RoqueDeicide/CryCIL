using System;
using System.Runtime.InteropServices;

namespace CryEngine.Mathematics.Geometry.Meshes
{
	/// <summary>
	/// Defines common methods used for tangent space normals.
	/// </summary>
	public static class CommonTangentOperations
	{
		/// <summary>
		/// Gets size of objects that are currently being used for tangent space normals in bytes.
		/// </summary>
		public static int ByteCount;
		static CommonTangentOperations()
		{
			CommonTangentOperations.ByteCount =
			(
				Platform.MeshTangentsUseSingle
				? Marshal.SizeOf(typeof(TangentSingle))
				: Marshal.SizeOf(typeof(TangentInt16))
			);
		}
		/// <summary>
		/// Transfers tangent space normal from native memory.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to the beginning of the memory block that contains the normal.
		/// </param>
		/// <param name="offset">  Byte offset that is added to the <paramref name="pointer" />. </param>
		/// <returns>
		/// Instance of type that implements <see cref="ITangent" /> interface that either uses
		/// objects of type <see cref="Single" /> or <see cref="Int16" /> for coordinates.
		/// </returns>
		public static ITangent FromNativeMemory(IntPtr pointer, int offset)
		{
			if (Platform.MeshTangentsUseSingle)
			{
				return (TangentSingle)Marshal.PtrToStructure(pointer + offset, typeof(TangentSingle));
			}
			return (TangentInt16)Marshal.PtrToStructure(pointer + offset, typeof(TangentInt16));
		}
		/// <summary>
		/// Transfers instance of type that implements <see cref="ITangent" /> interface to native memory.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to the beginning of the memory block that will contain the normal.
		/// </param>
		/// <param name="offset">  Byte offset that is added to the <paramref name="pointer" />. </param>
		/// <param name="tangent"> Object to transfer. </param>
		public static void ToNativeMemory(IntPtr pointer, int offset, ITangent tangent)
		{
			if (tangent is TangentInt16)
			{
				ToNativeMemory(pointer, offset, (TangentInt16)tangent);
			}
			else if (tangent is TangentSingle)
			{
				ToNativeMemory(pointer, offset, (TangentSingle)tangent);
			}
		}
		/// <summary>
		/// Transfers instance of type that implements <see cref="ITangent" /> interface to native memory.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to the beginning of the memory block that will contain the normal.
		/// </param>
		/// <param name="offset">  Byte offset that is added to the <paramref name="pointer" />. </param>
		/// <param name="tangent"> Object to transfer. </param>
		public static void ToNativeMemory<TangentType>(IntPtr pointer, int offset, TangentType tangent)
			where TangentType : struct, ITangent
		{
			Marshal.StructureToPtr(tangent, pointer + offset, false);
		}
	}
	/// <summary>
	/// Defines common methods used for tangent space normals.
	/// </summary>
	public static class CommonQTangentOperations
	{
		/// <summary>
		/// Gets size of objects that are currently being used for tangent space normals in bytes.
		/// </summary>
		public static int ByteCount;
		static CommonQTangentOperations()
		{
			CommonTangentOperations.ByteCount =
			(
				Platform.MeshTangentsUseSingle
				? Marshal.SizeOf(typeof(QTangentSingle))
				: Marshal.SizeOf(typeof(QTangentInt16))
			);
		}
		/// <summary>
		/// Transfers tangent space normal from native memory.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to the beginning of the memory block that contains the normal.
		/// </param>
		/// <param name="offset">  Byte offset that is added to the <paramref name="pointer" />. </param>
		/// <returns>
		/// Instance of type that implements <see cref="IQTangent" /> interface that either uses
		/// objects of type <see cref="Single" /> or <see cref="Int16" /> for coordinates.
		/// </returns>
		public static IQTangent FromNativeMemory(IntPtr pointer, int offset)
		{
			if (Platform.MeshTangentsUseSingle)
			{
				return (QTangentSingle)Marshal.PtrToStructure(pointer + offset, typeof(QTangentSingle));
			}
			return (QTangentInt16)Marshal.PtrToStructure(pointer + offset, typeof(QTangentInt16));
		}
		/// <summary>
		/// Transfers instance of type that implements <see cref="IQTangent" /> interface to native memory.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to the beginning of the memory block that will contain the normal.
		/// </param>
		/// <param name="offset">  Byte offset that is added to the <paramref name="pointer" />. </param>
		/// <param name="tangent"> Object to transfer. </param>
		public static void ToNativeMemory(IntPtr pointer, int offset, IQTangent tangent)
		{
			if (tangent is QTangentInt16)
			{
				ToNativeMemory(pointer, offset, (QTangentInt16)tangent);
			}
			else if (tangent is QTangentSingle)
			{
				ToNativeMemory(pointer, offset, (QTangentSingle)tangent);
			}
		}
		/// <summary>
		/// Transfers instance of type that implements <see cref="IQTangent" /> interface to native memory.
		/// </summary>
		/// <param name="pointer">
		/// Pointer to the beginning of the memory block that will contain the normal.
		/// </param>
		/// <param name="offset">  Byte offset that is added to the <paramref name="pointer" />. </param>
		/// <param name="tangent"> Object to transfer. </param>
		public static void ToNativeMemory<QTangentType>(IntPtr pointer, int offset, QTangentType tangent)
			where QTangentType : struct, IQTangent
		{
			Marshal.StructureToPtr(tangent, pointer + offset, false);
		}
	}
}