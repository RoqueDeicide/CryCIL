﻿using System;
using CryCil.Annotations;

namespace CryCil.RunTime.Logging
{
	partial class ConsoleLogWriter
	{
		/// <summary>
		/// Writes <see cref="bool"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(bool value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="char"/> value to CryEngine log.
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
			this.buffer.Append(symbols);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes a part of the array of symbols to CryEngine log.
		/// </summary>
		/// <param name="symbols">Symbols to write.</param>
		/// <param name="index">  Zero-based index of the first symbol from the array to write.</param>
		/// <param name="count">  Number of symbols to write.</param>
		public override void Write(char[] symbols, int index, int count)
		{
			this.buffer.Append(symbols, index, count);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes <see cref="decimal"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(decimal value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="double"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(double value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="float"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(float value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="int"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(int value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="long"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(long value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="object"/> value to CryEngine log.
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
			this.buffer.Append(value);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">String that defines format and position of arguments.</param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, object arg0)
		{
			this.buffer.AppendFormat(format, arg0);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">String that defines format and position of arguments.</param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		/// <param name="arg1">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, object arg0, object arg1)
		{
			this.buffer.AppendFormat(format, arg0, arg1);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">String that defines format and position of arguments.</param>
		/// <param name="arg0">  Value to format and insert into final output.</param>
		/// <param name="arg1">  Value to format and insert into final output.</param>
		/// <param name="arg2">  Value to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, object arg0, object arg1, object arg2)
		{
			this.buffer.AppendFormat(format, arg0, arg1, arg2);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes formatted string to CryEngine log.
		/// </summary>
		/// <param name="format">String that defines format and position of arguments.</param>
		/// <param name="arg">   Values to format and insert into final output.</param>
		[StringFormatMethod("format")]
		public override void Write(string format, params object[] arg)
		{
			this.buffer.AppendFormat(format, arg);
			if (this.buffer.ToString().Contains(Environment.NewLine))
			{
				this.Flush();
			}
		}
		/// <summary>
		/// Writes <see cref="ulong"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(uint value)
		{
			this.buffer.Append(value);
		}
		/// <summary>
		/// Writes <see cref="ulong"/> value to CryEngine log.
		/// </summary>
		/// <param name="value">Value to write.</param>
		public override void Write(ulong value)
		{
			this.buffer.Append(value);
		}
	}
}