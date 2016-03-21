using System;
using System.Linq;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Encapsulates bone-mapping data using <see cref="ushort"/> values as indices.
	/// </summary>
	public unsafe struct CryMeshBoneMappingUint16
	{
		/// <summary>
		/// Indices of each of the 4 bones which movement affects position of the vertex.
		/// </summary>
		public fixed ushort BoneIds [4];
		/// <summary>
		/// Values that specify how much the movement of respective bone affects position of the vertex.
		/// </summary>
		public fixed byte Weights [4];
	}
	/// <summary>
	/// Encapsulates bone-mapping data using <see cref="byte"/> values as indices.
	/// </summary>
	public unsafe struct CryMeshBoneMappingByte
	{
		/// <summary>
		/// Indices of each of the 4 bones which movement affects position of the vertex.
		/// </summary>
		public fixed byte BoneIds [4];
		/// <summary>
		/// Values that specify how much the movement of respective bone affects position of the vertex.
		/// </summary>
		public fixed byte Weights [4];
	}
}