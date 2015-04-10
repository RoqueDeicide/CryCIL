using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Enumeration of rendering styles for bounding boxes.
	/// </summary>
	public enum BoundingBoxRenderStyle
	{
		/// <summary>
		/// Simple flat shaded rendering.
		/// </summary>
		Faceted,
		/// <summary>
		/// Rendering with color coded extremums of the bounding box.
		/// </summary>
		ExtremesColorEncoded
	}
}
