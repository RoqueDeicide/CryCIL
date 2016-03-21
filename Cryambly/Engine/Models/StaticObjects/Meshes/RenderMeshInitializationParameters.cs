using System;
using System.Linq;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates a set of parameters that is used to create new <see cref="CryRenderMesh"/>.
	/// </summary>
	public unsafe struct RenderMeshInitializationParameters
	{
		/// <summary>
		/// Default instance of this type.
		/// </summary>
		public static readonly RenderMeshInitializationParameters Default = new RenderMeshInitializationParameters
		{
			VertexFormat = VertexFormat.P3F_C4B_T2F,
			Type = RenderMeshType.Static,
			PrimetiveType = PublicRenderPrimitiveType.TriangleList,
			Precache = true
		};

#pragma warning disable 1584,1711,1572,1581,1580
		/// <summary>
		/// Identifier of the vertex data format.
		/// <see cref="CryCil.Engine.Models.StaticObjects.VertexFormat.P3F_C4B_T2F"/> is the default one.
		/// </summary>
		public VertexFormat VertexFormat;
#pragma warning restore 1584,1711,1572,1581,1580
		/// <summary>
		/// Type of render mesh. <see cref="RenderMeshType.Static"/> is the default one.
		/// </summary>
		public RenderMeshType Type;

		/// <summary>
		/// Pointer to the array that contains the vertex data.
		/// </summary>
		public void* VertexDataBuffer;
		/// <summary>
		/// Number of elements in <see cref="VertexDataBuffer"/>, <see cref="Tangents"/> and
		/// <see cref="Normals"/>.
		/// </summary>
		public int VertexCount;
		/// <summary>
		/// Pointer to the array that contains tangent-space normals.
		/// </summary>
		public PipelineTangents* Tangents;
		/// <summary>
		/// Pointer to the array that contains normals.
		/// </summary>
		public PipelineNormal* Normals;
		/// <summary>
		/// Pointer to the array of indices that form faces that comprise this mesh.
		/// </summary>
		public uint* Indices;
		/// <summary>
		/// Number of indices in <see cref="Indices"/>.
		/// </summary>
		public int IndexCount;
		/// <summary>
		/// Type of rendering primitive to use for this mesh.
		/// <see cref="PublicRenderPrimitiveType.TriangleList"/>.
		/// </summary>
		public PublicRenderPrimitiveType PrimetiveType;
		/// <summary>
		/// Number of render chunks to allocate.
		/// </summary>
		public int RenderChunkCount;
		/// <summary>
		/// Unknown.
		/// </summary>
		public int ClientTextureBindId;
		/// <summary>
		/// Indicates whether render mesh should only stored in video memory.
		/// </summary>
		public bool OnlyVideoBuffer;
		/// <summary>
		/// Unknown. <c>true</c> is the default value.
		/// </summary>
		public bool Precache;
		/// <summary>
		/// Indicates whether render mesh should be locked for thread access after creation.
		/// </summary>
		public bool LockForThreadAccess;
	}
}