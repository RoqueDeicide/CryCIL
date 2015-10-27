namespace CryCil.Engine.Models.StaticObjects
{
	internal enum GeneralMeshStreamId
	{
		Positions = 0,
		PositionsF16,
		Normals,
		Faces,
		TopologyIds,
		TextureCoordinates,
		Colors0,
		Colors1,
		Indices,
		Tangents,
		BoneMapping,
		VertexMaterials,
		QTangents,
		P3Sc4Bt2S,

		ExtraBoneMapping, // Extra stream. Does not have a stream ID in the CGF. Its data is saved at the end of the BONEMAPPING stream.

		LastStream
	}
}