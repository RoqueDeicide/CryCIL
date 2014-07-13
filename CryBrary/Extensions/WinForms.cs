using System;
using System.Windows.Forms;

namespace CryEngine.Extensions
{
	public static class WinFormsExtensions
	{
		/// <summary>
		/// Appends text to the current text of a text box, with formatted arguments.
		/// </summary>
		/// <param name="textBox"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Append(this TextBoxBase textBox, string format, params object[] args)
		{
			textBox.AppendText(string.Format(format, args));
		}

		/// <summary>
		/// Appends a number of Environment.NewLine strings to the current text of a text box.
		/// </summary>
		/// <param name="textBox"></param>
		/// <param name="count"></param>
		public static void NewLine(this TextBoxBase textBox, int count = 1)
		{
			for (var i = 0; i < count; i++)
				textBox.Append(Environment.NewLine);
		}

		/// <summary>
		/// Scrolls the contents of the text box to the given position. The value is clamped to the
		/// length of the text.
		/// </summary>
		/// <param name="textBox"></param>
		/// <param name="position"></param>
		public static void ScrollTo(this TextBoxBase textBox, int position)
		{
			textBox.SelectionStart = MathHelpers.Clamp(position, 0, textBox.TextLength);
			textBox.ScrollToCaret();
		}
	}
}