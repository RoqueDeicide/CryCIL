using System;
using CryCil.Engine.Rendering;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Represents a text message that is rendered using <see cref="DebugEngine"/> class.
	/// </summary>
	public class DebugText : Debug3DObject
	{
		#region Properties
		/// <summary>
		/// Gets or sets location of this text message in 3D world space or on screen (if
		/// <see cref="TextRenderOptions.OnScreen"/> is set on <see cref="RenderOptions"/> property of this
		/// object) .
		/// </summary>
		public Vector3 Location { get; set; }
		/// <summary>
		/// Gets or sets options that specify how this text message is rendered.
		/// </summary>
		public TextRenderOptions RenderOptions { get; set; }
		/// <summary>
		/// Gets or sets size of the font that is used to render this text message.
		/// </summary>
		public float FontSize { get; set; }
		/// <summary>
		/// Gets or sets contents of this text message.
		/// </summary>
		public string Text { get; set; }
		#endregion
		#region Interface
		/// <summary>
		/// Renders this text.
		/// </summary>
		public override void Render()
		{
			Renderer.DrawText(this.Location,
							  this.RenderOptions,
							  this.Color,
							  new Vector2(this.FontSize),
							  this.Text);
		}
		#endregion
	}
}