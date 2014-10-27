using System;

namespace CryCil
{
	/// <summary>
	/// Represents an object that contains a parameter of type
	/// <typeparamref name="ExtraDataType"/> that describes an event.
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