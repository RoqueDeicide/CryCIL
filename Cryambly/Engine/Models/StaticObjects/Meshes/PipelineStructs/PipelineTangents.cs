using System;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// A pair of tangent-space normals in format that is used for storage in video memory.
	/// </summary>
	public struct PipelineTangents
	{
		/// <summary>
		/// Tangent.
		/// </summary>
		public Vector4Int16 Tangent;
		/// <summary>
		/// Bitangent.
		/// </summary>
		public Vector4Int16 Bitangent;
	}
}