using System;
using System.Linq;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Enumeration of boolean operations that can be used when comparing analog input.
	/// </summary>
	public enum AnalogComparisonOperation
	{
		/// <summary>
		/// Default value.
		/// </summary>
		None = 0,
		/// <summary>
		/// Equals (==) operation.
		/// </summary>
		Equals,
		/// <summary>
		/// Not equals (!=) operation.
		/// </summary>
		NotEquals,
		/// <summary>
		/// Less then (&gt;) operation.
		/// </summary>
		GreaterThan,
		/// <summary>
		/// Less then (&lt;) operation.
		/// </summary>
		LessThan
	}
}