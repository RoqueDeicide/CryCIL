using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine Input API that deals with touch-based devices.
	/// </summary>
	/// <remarks>Currently touch events are not raised.</remarks>
	public static class Touch
	{
		#region Events
		/// <summary>
		/// Occurs when one of the touch devices registers the touch input.
		/// </summary>
		public static event EventHandler<TouchEventArgs> Event;
		#endregion
		#region Utilities
		[UnmanagedThunk("Invoked by underlying framework to raise Event event.")]
		private static void OnEvent(int device, byte deviceIndex, byte id, float x, float y)
		{
			EventHandler<TouchEventArgs> handler = Event;
			if (handler != null)
			{
				handler(null,
					new TouchEventArgs((InputDeviceType)device, deviceIndex, id, new Vector2(x, y)));
			}
		}
		#endregion
	}
}
