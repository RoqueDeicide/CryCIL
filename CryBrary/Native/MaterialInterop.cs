using System;
using System.Runtime.CompilerServices;
using CryEngine.Mathematics;
using CryEngine.Mathematics.Graphics;

namespace CryEngine.Native
{
	internal static class MaterialInterop
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		extern internal static IntPtr FindMaterial(string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr CreateMaterial(string name);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr LoadMaterial(string name, bool makeIfNotFound = true, bool nonRemovable = false);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetSubMaterial(IntPtr materialPtr, int slot);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetSubmaterialCount(IntPtr materialPtr);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetMaterial(IntPtr entityPtr, int slot);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetMaterial(IntPtr entityPtr, IntPtr materialPtr, int slot);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr CloneMaterial(IntPtr materialPtr, int subMtl);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool SetGetMaterialParamFloat(IntPtr ptr, string paramName, ref float v, bool get);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static bool SetGetMaterialParamVec3(IntPtr ptr, string paramName, ref Vector3 v, bool get);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static int GetShaderParamCount(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetShaderParamName(IntPtr ptr, int index);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetShaderParam(IntPtr ptr, string paramName, float newVal);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetShaderParam(IntPtr ptr, string paramName, ColorSingle newVal);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static MaterialFlags GetFlags(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static void SetFlags(IntPtr ptr, MaterialFlags flags);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetSurfaceType(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetSurfaceTypeById(int id);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static IntPtr GetSurfaceTypeByName(string name);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static UInt16 GetSurfaceTypeId(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetSurfaceTypeName(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static string GetSurfaceTypeTypeName(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static SurfaceTypeFlags GetSurfaceTypeFlags(IntPtr ptr);
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern internal static SurfaceTypeParams GetSurfaceTypeParams(IntPtr ptr);
	}
}