using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a HW skinning data.
	/// </summary>
	public unsafe struct W4B_I4S
	{
		/// <summary>
		/// 4 weights for each bone.
		/// </summary>
		public ColorByte Weights;
		/// <summary>
		/// Indices of bones.
		/// </summary>
		public fixed ushort Indices [4];
	}
}