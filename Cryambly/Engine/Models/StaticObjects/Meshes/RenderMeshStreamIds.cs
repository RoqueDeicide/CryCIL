namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Enumeration of data streams that contain render mesh data.
	/// </summary>
	public enum RenderMeshStreamIds
	{
		/// <summary>
		/// General vertex buffer.
		/// </summary>
		General,
		/// <summary>
		/// Tangent space normals buffer.
		/// </summary>
		Tangents,
		/// <summary>
		/// Tangent space normals buffer.
		/// </summary>
		QTangents,
		/// <summary>
		/// HW skinning buffer.
		/// </summary>
		HwSkinInfo,
		/// <summary>
		/// Velocity buffer.
		/// </summary>
		VertexVelocity,
		/// <summary>
		/// Normals buffer.
		/// </summary>
		Normals,
		/// <summary>
		/// Number of vertex streams.
		/// </summary>
		VertexStreamCount,

		/// <summary>
		/// Morph buddy data buffer.
		/// </summary>
		MorphBuddy = 8,
		/// <summary>
		/// Instancing data stream.
		/// </summary>
		Instanced = 9,
		/// <summary>
		/// Morphing weights buffer.
		/// </summary>
		MorphBuddyWeights = 15
	}
}