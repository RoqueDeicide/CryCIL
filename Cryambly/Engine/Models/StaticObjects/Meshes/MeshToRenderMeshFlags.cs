using System;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// A set of flags that specifies how to convert <see cref="CryMesh"/> to <see cref="CryRenderMesh"/>.
	/// </summary>
	[Flags]
	public enum MeshToRenderMeshFlags : uint
	{
		/// <summary>
		/// If set, specifies that vertex velocity streams need to be created.
		/// </summary>
		VertexVelocity = 1,
		/// <summary>
		/// If set, specifies that tangent-space normals should not be calculated.
		/// </summary>
		NoTangents = 2,
		/// <summary>
		/// If set, specifies that mesh should be created in video memory.
		/// </summary>
		CreateDeviceMesh = 4,
		/// <summary>
		/// If set, specifies that mesh should be set asynchronously.
		/// </summary>
		SetMeshAsynchronously = 8,
		/// <summary>
		/// If set, specifies that data stream of normals should be enabled.
		/// </summary>
		EnableNormalStream = 16,
		/// <summary>
		/// If set, specifies that density of texels should be ignored.
		/// </summary>
		IgnoreTexelDensity = 32
	}
}