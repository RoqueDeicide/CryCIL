using System;
using System.Diagnostics.Contracts;

namespace CryCil
{
	/// <summary>
	/// Represents an object that contains a parameter of type <typeparamref name="ExtraDataType"/> that
	/// describes an event.
	/// </summary>
	/// <typeparam name="ExtraDataType">Type of the parameter.</typeparam>
	public class EventArgs<ExtraDataType> : EventArgs, IEquatable<EventArgs<ExtraDataType>>
	{
		#region Properties
		/// <summary>
		/// Extra parameter that comes with the event.
		/// </summary>
		public ExtraDataType Parameter { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="parameter">Parameter.</param>
		public EventArgs(ExtraDataType parameter)
		{
			this.Parameter = parameter;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="other">Another object.</param>
		/// <returns>
		/// True, if this object references the same address as the other one or their
		/// <see cref="Parameter"/> properties are equal.
		/// </returns>
		public bool Equals(EventArgs<ExtraDataType> other)
		{
			return other != null &&
				   (ReferenceEquals(this, other) || this.Parameter.Equals(other.Parameter));
		}
		/// <summary>
		/// Determines whether this object is equal to another.
		/// </summary>
		/// <param name="obj">Another object.</param>
		/// <returns>
		/// True, if this object references the same address as the other one or their
		/// <see cref="Parameter"/> properties are equal in case of <paramref name="obj"/> being of type
		/// <see cref="EventArgs{ExtraDataType}"/>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return obj != null &&
				   (
					   ReferenceEquals(this, obj) ||
					   obj.GetType() == this.GetType() &&
					   this.Parameter.Equals(((EventArgs<ExtraDataType>)obj).Parameter)
					   );
		}
		/// <summary>
		/// Gets hash code of this object.
		/// </summary>
		/// <returns>Hash code of the <see cref="Parameter"/>.</returns>
		public override int GetHashCode()
		{
			return this.Parameter.GetHashCode();
		}
		/// <summary>
		/// Establishes fact of equality of two objects of type <see cref="EventArgs{ExtraDataType}"/>.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// True, if both object reference the same address or their parameters are equal in case both of
		/// them are not null, otherwise false.
		/// </returns>
		public static bool operator ==(EventArgs<ExtraDataType> left, EventArgs<ExtraDataType> right)
		{
			return
				ReferenceEquals(left, right) ||
				!(right == null) && !(left == null) &&
				left.Parameter.Equals(right.Parameter);
		}
		/// <summary>
		/// Establishes fact of inequality of two objects of type <see cref="EventArgs{ExtraDataType}"/>.
		/// </summary>
		/// <param name="left"> Left operand.</param>
		/// <param name="right">Right operand.</param>
		/// <returns>
		/// True, if both object do not reference the same address and one of the is null or their
		/// parameters differ, otherwise false.
		/// </returns>
		public static bool operator !=(EventArgs<ExtraDataType> left, EventArgs<ExtraDataType> right)
		{
			return
				!ReferenceEquals(left, right) &&
				right == null || left == null ||
				!left.Parameter.Equals(right.Parameter);
		}
		/// <summary>
		/// Creates string representation of this object.
		/// </summary>
		/// <returns>
		/// Text formatted as "Parameter: {0}", where {0} is a string representation of
		/// <see cref="Parameter"/>.
		/// </returns>
		public override string ToString()
		{
			Contract.Ensures(Contract.Result<string>() != null);
			return string.Format("Parameter: {0}", this.Parameter);
		}
		#endregion
	}
}