using System;
using CryCil.Annotations;

namespace CryCil.RunTime.Logging
{
	partial class ConsoleLogWriter
	{
		/// <summary>
		/// Writes a new line symbol to CryEngine log.
		/// </summary>
		public override void WriteLine()
		{
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Boolean"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(bool value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Char"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(char value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes a part of the array of symbols followed up by new line symbol to
		/// CryEngine log.
		/// </summary>
		/// <param name="symbols">Symbols to write.</param>
		public override void WriteLine(char[] symbols)
		{
			this.buffer.Append(symbols);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes a part of the array of symbols followed up by new line symbol to
		/// CryEngine log.
		/// </summary>
		/// <param name="symbols">Symbols to write.</param>
		/// <param name="index">  
		/// Zero-based index of the first symbol from the array to write.
		/// </param>
		/// <param name="count">  Number of symbols to write.</param>
		public override void WriteLine(char[] symbols, int index, int count)
		{
			this.buffer.Append(symbols, index, count);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Decimal"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(decimal value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Double"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(double value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Single"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(float value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Int32"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(int value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Int64"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(long value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="Object"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(object value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes formatted string with new line symbol at the end to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void WriteLine(string format, object arg0)
		{
			this.buffer.AppendFormat(format, arg0);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes formatted string with new line symbol at the end to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		/// <param name="arg1">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void WriteLine(string format, object arg0, object arg1)
		{
			this.buffer.AppendFormat(format, arg0, arg1);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes formatted string with new line symbol at the end to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		/// <param name="arg1">  Value to format and insert into final output.</param>
		/// <param name="arg2">  Value to format and insert into final output.</param>
		public override void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			this.buffer.AppendFormat(format, arg0, arg1, arg2);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes formatted string with new line symbol at the end to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg">   Values to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void WriteLine(string format, params object[] arg)
		{
			this.buffer.AppendFormat(format, arg);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="UInt32"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(uint value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="UInt64"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(ulong value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
		/// <summary>
		/// Writes <see cref="String"/> value followed up by new line symbol to CryEngine
		/// log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void WriteLine(string value)
		{
			this.buffer.Append(value);
			this.buffer.Append(Environment.NewLine);
			this.Flush();
		}
	}
}