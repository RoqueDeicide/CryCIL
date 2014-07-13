using System;
using System.ComponentModel;

namespace CryEngine.Testing
{
	/// <summary>
	/// Contains methods used to make assertions for testing purposes.
	/// </summary>
	public static class Assert
	{
		/// <summary>
		/// Asserts that a given statement is true.
		/// </summary>
		/// <param name="value">The value to be asserted as true.</param>
		/// <exception cref="CryEngine.Testing.AssertionFailedException">
		/// Thrown if the value is false.
		/// </exception>
		public static void IsTrue(bool value)
		{
			if (!value)
				throw new AssertionFailedException("Assertion failed.");
		}

		/// <summary>
		/// Asserts that a given expression throws an exception of type T.
		/// </summary>
		/// <typeparam name="T">
		/// The expression that should be thrown within the supplied method.
		/// </typeparam>
		/// <param name="method">The method in which the expression should be thrown.</param>
		/// <param name="includeChildren">
		/// Specifies whether subclassed exceptions should be counted as valid.
		/// </param>
		/// <returns>The exception that was thrown as expected.</returns>
		/// <exception cref="CryEngine.Testing.AssertionFailedException">
		/// Thrown if an exception of type T is not thrown.
		/// </exception>
		public static T Throws<T>(Action method, bool includeChildren = true) where T : Exception
		{
			try
			{
				method();
			}
			catch (Exception ex)
			{
				if ((includeChildren && ex is T) || typeof(T) == ex.GetType())
					return ex as T;
			}

			throw new AssertionFailedException("The expected {0} was not thrown.", typeof(T).Name);
		}

		#region Hiding Object Equals/RefEquals
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new static bool Equals(object obj)
		{
			throw new NotImplementedException();
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public new static bool ReferenceEquals(object lhs, object rhs)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}