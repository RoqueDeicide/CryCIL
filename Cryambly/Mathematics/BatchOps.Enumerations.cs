using System;
using System.Linq;

namespace CryCil
{
	/// <summary>
	/// Enumeration of mathematical operations that take only 1 argument.
	/// </summary>
	public enum MathSimpleOperations
	{
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Sine = 0,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Cosine,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Tangent,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Cotangent,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Arcsine,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Arccosine,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Arctangent,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Arccotangent,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		SineHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		CosineHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		TangentHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		CotangentHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		ArcsineHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		ArccosineHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		ArctangentHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		ArccotangentHyperbolic,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		LogarithmNatural,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		LogarithmDecimal,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Exponent
	}
	/// <summary>
	/// Enumeration of mathematical operations that involve 3 numbers.
	/// </summary>
	public enum Math3NumberOperations
	{
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Power = 0,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Logarithm,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		SineCosine,
		/// <summary>
		/// Designates one of the mathematical operations of the same name.
		/// </summary>
		Arctangent2
	}
}