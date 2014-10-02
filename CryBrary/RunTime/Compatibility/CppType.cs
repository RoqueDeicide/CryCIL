using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.RunTime.Compatibility
{
	/// <summary>
	/// Represents reflection data about a C++ type.
	/// </summary>
	public class CppType
	{
		/// <summary>
		/// Gets or sets the number of bytes object of this C++ type occupies in the
		/// stack.
		/// </summary>
		public int Size { get; set; }
		/// <summary>
		/// Gets or sets the list of field names and corresponding managed types that can
		/// be used to represent type of that field.
		/// </summary>
		public List<Tuple<string, Type>> Fields { get; set; }
	}
}