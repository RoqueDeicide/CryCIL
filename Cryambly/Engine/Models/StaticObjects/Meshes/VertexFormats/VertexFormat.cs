using System;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Enumeration of vertex formats.
	/// </summary>
	public enum VertexFormat
	{
		/// <summary>
		/// Unknown vertex format.
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// Vertex format where position of the vertex is expressed with 3 single-precision floating point
		/// numbers, color - 4 8-bit integer numbers, texture coordinates - 2 single-precision floating
		/// point numbers.
		/// </summary>
		/// <remarks>Used in base data streams.</remarks>
		P3F_C4B_T2F = 1,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 3 half-precision floating point
		/// numbers, color - 4 8-bit integer numbers, texture coordinates - 2 half-precision floating point
		/// numbers.
		/// </summary>
		/// <remarks>Used in base data streams.</remarks>
		P3S_C4B_T2S = 2,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 3 half-precision floating point
		/// numbers, normal - 4 8-bit integer numbers, color - 4 8-bit integer numbers, texture coordinates
		/// - 2 half-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in base data streams.</remarks>
		P3S_N4B_C4B_T2S = 3,

		/// <summary>
		/// Vertex format where position of the vertex is expressed with 3 single-precision floating point
		/// numbers, color - 4 8-bit integer numbers, texture coordinates - 4 8-bit integer numbers, 2
		/// normals - 3 single-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in particle effect data streams.</remarks>
		P3F_C4B_T4B_N3F2 = 4,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 4 single-precision floating point
		/// numbers, color - 4 8-bit integer numbers, texture coordinates - 2 single-precision floating
		/// point numbers.
		/// </summary>
		/// <remarks>Used in font data streams.</remarks>
		TP3F_C4B_T2F = 5,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 4 single-precision floating point
		/// numbers, first set of texture coordinates - 2 single-precision floating point numbers and
		/// second set of texture coordinates - 3 single-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in miscellaneous data streams.</remarks>
		TP3F_T2F_T3F = 6,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 4 single-precision floating point
		/// numbers, texture coordinates - 3 single-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in miscellaneous data streams.</remarks>
		P3F_T3F = 7,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 3 single-precision floating point
		/// numbers, first set of texture coordinates - 2 single-precision floating point numbers and
		/// second set of texture coordinates - 3 single-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in miscellaneous data streams.</remarks>
		P3F_T2F_T3F = 8,

		/// <summary>
		/// Vertex format where texture coordinates of the vertex are expressed with 2 single-precision
		/// floating point numbers.
		/// </summary>
		/// <remarks>Used in light map data streams.</remarks>
		T2F = 9,
		/// <summary>
		/// Vertex format where weights of bones for vertex are expressed with 4 8-bit integer numbers and
		/// bone indices - 4 8-bit integer numbers.
		/// </summary>
		/// <remarks>Used in skinned weights/indices data streams.</remarks>
		W4B_I4B = 10,
		/// <summary>
		/// Vertex format with 2 sets of shape coefficients expressed with sets of 4 8-bit integer numbers.
		/// </summary>
		C4B_C4B = 11,
		/// <summary>
		/// Vertex format where 2 positions of the vertex are expressed with sets of 3 single-precision
		/// floating point numbers and bone indices - 4 8-bit integer numbers.
		/// </summary>
		/// <remarks>Used in shape deformation data streams.</remarks>
		P3F_P3F_I4B = 12,
		/// <summary>
		/// Vertex format where velocity of the vertex is expressed with 3 single-precision floating point
		/// numbers.
		/// </summary>
		P3F = 13,

		/// <summary>
		/// Vertex format where color of the vertex is expressed with 4 8-bit integer numbers, texture
		/// coordinates - 2 half-precision floating point numbers.
		/// </summary>
		/// <remarks>Used when positions stream is merged with tangents one.</remarks>
		C4B_T2S = 14,

		/// <summary>
		/// Vertex format where position of the vertex is expressed with 2 single-precision floating point
		/// numbers, texture coordinates - 4 single-precision floating point numbers, color - 4
		/// single-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in primary data streams for lens flare simulation.</remarks>
		P2F_T4F_C4F = 15,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 2 single-precision floating point
		/// numbers, 2 sets of texture coordinates - 4 single-precision floating point numbers, color - 4
		/// single-precision floating point numbers.
		/// </summary>
		/// <remarks>Used in secondary data streams for lens flare simulation.</remarks>
		P2F_T4F_T4F_C4F = 16,

		/// <summary>
		/// Vertex format where position of the vertex is expressed with 2 half-precision floating point
		/// numbers, normal - 4 8-bit integer numbers, color - 4 8-bit integer numbers, texture coordinates
		/// - 1 single-precision floating point number.
		/// </summary>
		P2S_N4B_C4B_T1F = 17,
		/// <summary>
		/// Vertex format where position of the vertex is expressed with 3 single-precision floating point
		/// numbers, color - 4 8-bit integer numbers, texture coordinates - 2 half-precision floating point
		/// numbers.
		/// </summary>
		P3F_C4B_T2S = 18,

		/// <summary>
		/// Maximal number of vertex formats.
		/// </summary>
		Max
	}
}