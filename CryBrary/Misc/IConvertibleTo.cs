using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Defines a method that converts an instance of type that
	/// implements this interface to <typeparamref
	/// name="DestinationType" />.
	/// </summary>
	/// <typeparam name="DestinationType">
	/// Type to which an instance of type that implements this
	/// interface can be converted.
	/// </typeparam>
	public interface IConvertibleTo<DestinationType>
	{
		/// <summary>
		/// Converts object of type that implements this interface to
		/// an object of type <typeparamref name="DestinationType" />.
		/// </summary>
		/// <param name="output">
		/// Reference to object of type <typeparamref
		/// name="DestinationType" /> that will contain a
		/// representation of this object.
		/// </param>
		void ConvertTo(out DestinationType output);
	}
}