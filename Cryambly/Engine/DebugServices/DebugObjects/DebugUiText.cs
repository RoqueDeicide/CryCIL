using System;
using CryCil.Engine.Localization;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a localizable text message that can be rendered using <see cref="DebugEngine"/>.
	/// </summary>
	public class DebugUiText : DebugObject
	{
		#region Fields
		private string value;
		private static readonly CryFont defaultFont = new CryFont("default");
		internal static float currentTextPosition;
		#endregion
		#region Properties
		/// <summary>
		/// Gets text message that will be rendered on the screen.
		/// </summary>
		public string DisplayText { get; private set; }
		/// <summary>
		/// Gets or sets unlocalized text value.
		/// </summary>
		public string Value
		{
			get { return this.value; }
			set
			{
				if (this.value != value)
				{
					this.value = value;
					this.DisplayText = Translator.Translate(value, false);
				}
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		static DebugUiText()
		{
			currentTextPosition = 400.0f;
			DebugEngine.FrameStart += // Reset position so UI messages start rendering from
				() => currentTextPosition = 400.0f; // the top and go down.
		}
		#endregion
		#region Interface
		/// <summary>
		/// Renders this text message on the screen.
		/// </summary>
		public override void Render()
		{
			TextDrawContext context = new TextDrawContext
			{
				DrawTextFlags = TextRenderOptions.AlignmentCentered,
				ColorOverride = this.Color
			};
			Vector2 textSize = defaultFont.GetSize(this.DisplayText, true, ref context);
			defaultFont.DrawText(new Vector2(0, 400 + currentTextPosition), this.DisplayText, true, ref context);
			// Change currentTextPosition so next UI text message will be rendered slightly below this one.
			currentTextPosition += textSize.Y + 2;
		}
		#endregion
		#region Utilities
		#endregion
	}
}