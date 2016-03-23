using System;
using System.Linq;
using System.Linq.Expressions;

namespace CryCil.Utilities
{
	/// <summary>
	/// Defines a functions that convert values of one to <typeparamref name="ResultType"/>.
	/// </summary>
	/// <typeparam name="ResultType">Type to convert objects to.</typeparam>
	public static class Cast<ResultType>
	{
		/// <summary>
		/// Converts a value from <typeparamref name="InitialType"/> to <typeparamref name="ResultType"/>.
		/// This function avoids boxing for value types.
		/// </summary>
		/// <typeparam name="InitialType">Initial type of the value.</typeparam>
		/// <param name="value">Value to convert to <typeparamref name="ResultType"/>.</param>
		/// <returns>Converted object.</returns>
		/// <exception cref="InvalidCastException">
		/// Unable to convert object of type <typeparamref name="InitialType"/> to
		/// <typeparamref name="ResultType"/>.
		/// </exception>
		public static ResultType From<InitialType>(InitialType value)
		{
			try
			{
				return CasterCache<InitialType>.UncheckedCaster(value);
			}
			catch (Exception ex)
			{
				string initial = typeof(InitialType).FullName;
				string result = typeof(ResultType).FullName;
				throw new InvalidCastException($"Unable to convert objects of type {initial} to {result}.", ex);
			}
		}
		/// <summary>
		/// Converts a value from <typeparamref name="InitialType"/> to <typeparamref name="ResultType"/>
		/// and checks for overflow. This function avoids boxing for value types.
		/// </summary>
		/// <typeparam name="InitialType">Initial type of the value.</typeparam>
		/// <param name="value">Value to convert to <typeparamref name="ResultType"/>.</param>
		/// <returns>Converted object.</returns>
		/// <exception cref="InvalidCastException">
		/// Unable to convert object of type <typeparamref name="InitialType"/> to
		/// <typeparamref name="ResultType"/>.
		/// </exception>
		/// <exception cref="OverflowException">
		/// An object of type <typeparamref name="InitialType"/> cannot fit into object of type
		/// <typeparamref name="ResultType"/>.
		/// </exception>
		public static ResultType FromChecked<InitialType>(InitialType value)
		{
			try
			{
				return CasterCache<InitialType>.CheckedCaster(value);
			}
			catch (OverflowException)
			{
				// Don't catch overflow errors.
				throw;
			}
			catch (Exception ex)
			{
				string initial = typeof(InitialType).FullName;
				string result = typeof(ResultType).FullName;
				throw new InvalidCastException($"Unable to convert objects of type {initial} to {result}.", ex);
			}
		}
		/// <summary>
		/// Attempts to convert a value of type <typeparamref name="InitialType"/> to
		/// <typeparamref name="ResultType"/>. This function avoids boxing for value types.
		/// </summary>
		/// <typeparam name="InitialType">Initial type of the value.</typeparam>
		/// <param name="value"> Value to convert to <typeparamref name="ResultType"/>.</param>
		/// <param name="output">Converted value.</param>
		/// <returns>True, if conversion was successful, otherwise false.</returns>
		public static bool TryFrom<InitialType>(InitialType value, out ResultType output)
		{
			try
			{
				output = CasterCache<InitialType>.UncheckedCaster(value);
				return true;
			}
			catch (Exception)
			{
				output = default(ResultType);
				return false;
			}
		}
		/// <summary>
		/// Attempts to convert a value of type <typeparamref name="InitialType"/> to
		/// <typeparamref name="ResultType"/> and checks for overflow. This function avoids boxing for value
		/// types.
		/// </summary>
		/// <typeparam name="InitialType">Initial type of the value.</typeparam>
		/// <param name="value"> Value to convert to <typeparamref name="ResultType"/>.</param>
		/// <param name="output">Converted value.</param>
		/// <returns>True, if conversion was successful, otherwise false.</returns>
		public static bool TryFromChecked<InitialType>(InitialType value, out ResultType output)
		{
			try
			{
				output = CasterCache<InitialType>.CheckedCaster(value);
				return true;
			}
			catch (Exception)
			{
				output = default(ResultType);
				return false;
			}
		}
		private static class CasterCache<InitialType>
		{
			public static readonly Func<InitialType, ResultType> UncheckedCaster = GetUnchecked();
			public static readonly Func<InitialType, ResultType> CheckedCaster = GetChecked();

			private static Func<InitialType, ResultType> GetUnchecked()
			{
				var parameter = Expression.Parameter(typeof(InitialType));
				var convert = Expression.Convert(parameter, typeof(ResultType));
				return Expression.Lambda<Func<InitialType, ResultType>>(convert, parameter).Compile();
			}
			private static Func<InitialType, ResultType> GetChecked()
			{
				var parameter = Expression.Parameter(typeof(InitialType));
				var convert = Expression.ConvertChecked(parameter, typeof(ResultType));
				return Expression.Lambda<Func<InitialType, ResultType>>(convert, parameter).Compile();
			}
		}
	}
}