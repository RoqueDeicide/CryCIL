namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// A quaternion-based tangent-space normal in format that is used for storage in video memory.
	/// </summary>
	public struct PipelineQTangent
	{
		/// <summary>
		/// Tangent-Bitangent.
		/// </summary>
		public Vector4Int16 TangentBitangent;
	}
}