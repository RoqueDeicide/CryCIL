using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine
{
	/// <summary>
	/// Represents an object that contains a parameter of type <see cref="ExtraDataType"/>
	/// that describes an event.
	/// </summary>
	/// <typeparam name="ExtraDataType">Type of the parameter.</typeparam>
	public class EventArgs<ExtraDataType> : EventArgs
	{
		/// <summary>
		/// Extra parameter that comes with the event.
		/// </summary>
		public ExtraDataType Parameter { get; private set; }
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
		public EventArgs(ExtraDataType parameter)
		{
			this.Parameter = parameter;
		}
	}
}