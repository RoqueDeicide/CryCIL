using System;
using CryCil.Graphics;

namespace CryCil.Engine.Models.StaticObjects
{
	/// <summary>
	/// Represents a vertex data format that is defined by <see cref="VertexFormat.C4B_C4B"/>.
	/// </summary>
	public struct C4B_C4B
	{
		/// <summary>
		/// First set of coefficients.
		/// </summary>
		public ColorByte Coefficients0;
		/// <summary>
		/// Second set of coefficients.
		/// </summary>
		public ColorByte Coefficients1;
	}
}