using System;
using CryCil.Annotations;

namespace CryCil.RunTime.Logging
{
	partial class ConsoleLogWriter
	{
		/// <summary>
		/// Writes <see cref="Boolean"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(bool value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="Char"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(char value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes an array of symbols to CryEngine log.
		/// </summary>
		/// <param name="symbols">Symbols to write.</param>
		public override void Write(char[] symbols)
		{
			buffer.Append(symbols);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes a part of the array of symbols to CryEngine log.
		/// </summary>
		/// <param name="symbols">Symbols to write.</param>
		/// <param name="index">  
		/// Zero-based index of the first symbol from the array to write.
		/// </param>
		/// <param name="count">  Number of symbols to write.</param>
		public override void Write(char[] symbols, int index, int count)
		{
			buffer.Append(symbols, index, count);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes <see cref="Decimal"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(decimal value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="Double"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(double value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="Single"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(float value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="Int32"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(int value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="Int64"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(long value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="Object"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(object value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes text to the console.
		/// </summary>
		/// <param name="value">Text to write.</param>
		public override void Write(string value)
		{
			buffer.Append(value);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, object arg0)
		{
			buffer.AppendFormat(format, arg0);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		/// <param name="arg1">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, object arg0, object arg1)
		{
			buffer.AppendFormat(format, arg0, arg1);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		/// <param name="arg1">  Value to format and insert into final output.</param>
		/// <param name="arg2">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, object arg0, object arg1, object arg2)
		{
			buffer.AppendFormat(format, arg0, arg1, arg2);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">
		/// String that defines format and position of arguments.
		/// </param>
		/// <param name="arg">   Values to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, params object[] arg)
		{
			buffer.AppendFormat(format, arg);
			if (buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes <see cref="UInt64"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(uint value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="UInt64"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(ulong value)
		{
			this.buffer.Append(value);
		}
	}
}