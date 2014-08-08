using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Provides an incredibly quick access to columns in the object of type <see cref="Matrix44" />.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 52)]
	public struct Column44 : IConvertibleTo<Vector4>
	{
		/// <summary>
		/// First element in the column from the top.
		/// </summary>
		[FieldOffset(0)]
		public float ElementAtRow0;
		/// <summary>
		/// Second element in the column from the top.
		/// </summary>
		[FieldOffset(16)]
		public float ElementAtRow1;
		/// <summary>
		/// Third element in the column from the top.
		/// </summary>
		[FieldOffset(32)]
		public float ElementAtRow2;
		/// <summary>
		/// Fourth element in the column from the top.
		/// </summary>
		[FieldOffset(48)]
		public float ElementAtRow3;
		/// <summary>
		/// Converts this vector to a compact vector.
		/// </summary>
		/// <param name="output">Vector that contains all 4 elements of this column.</param>
		public void ConvertTo(out Vector4 output)
		{
			output = new Vector4(this.ElementAtRow0, this.ElementAtRow1, this.ElementAtRow2, this.ElementAtRow3);
		}
	}
}