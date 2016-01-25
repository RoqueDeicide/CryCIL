using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.Rendering.Nodes;
using CryCil.Geometry;

namespace CryCil.Engine
{
	/// <summary>
	/// Provides ability to create and delete the decals.
	/// </summary>
	public static class Decal
	{
		#region Interface
		/// <summary>
		/// Creates a decal.
		/// </summary>
		/// <param name="info">Reference to the object that specifies how to create the decal.</param>
		public static void Create(ref DecalInfo info)
		{
			CreateDecal(ref info);
		}
		/// <summary>
		/// Deletes decals.
		/// </summary>
		/// <param name="box">  
		/// Reference to the Axis-Aligned Bounding Box within which to remove the decals.
		/// </param>
		/// <param name="owner">If specified, represents the render node to wipe the decals off of.</param>
		public static void Delete(ref BoundingBox box, CryRenderNode owner = new CryRenderNode())
		{
			DeleteDecalsInRange(ref box, owner);
		}
		/// <summary>
		/// Deletes decals.
		/// </summary>
		/// <param name="owner">Represents the render node to wipe the decals off of.</param>
		public static void Delete(CryRenderNode owner)
		{
			DeleteEntityDecals(owner);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateDecal(ref DecalInfo info);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteDecalsInRange(ref BoundingBox pAreaBox, CryRenderNode pEntity);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DeleteEntityDecals(CryRenderNode pEntity);
		#endregion
	}
}