using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Provides an incredibly quick access to columns in the object of type <see cref="Matrix33" />.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 28)]
	public struct Column33 : IConvertibleTo<Vector3>
	{
		/// <summary>
		/// Element of this column at the top row.
		/// </summary>
		[FieldOffset(0)]
		public float ElementAtRow0;
		/// <summary>
		/// Element of this column at the middle row.
		/// </summary>
		[FieldOffset(12)]
		public float ElementAtRow1;
		/// <summary>
		/// Element of this column at the bottom row.
		/// </summary>
		[FieldOffset(24)]
		public float ElementAtRow2;
		/// <summary>
		/// Converts this vector to a compact vector.
		/// </summary>
		/// <param name="output">Vector that contains all 3 elements of this column.</param>
		public void ConvertTo(out Vector3 output)
		{
			output = new Vector3(this.ElementAtRow0, this.ElementAtRow1, this.ElementAtRow2);
		}
	}
}