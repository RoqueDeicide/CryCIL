using System;
using System.Linq;
using CryCil.RunTime;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that handle touch events.
	/// </summary>
	/// <param name="device">     Type of the device.</param>
	/// <param name="deviceIndex">Index of the device.</param>
	/// <param name="id">         Unknown.</param>
	/// <param name="position">   Position of the touch event.</param>
	public delegate void TouchHandler(InputDeviceType device, byte deviceIndex, byte id, Vector2 position);
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
		public static event TouchHandler Event;
		#endregion
		#region Utilities
		[RawThunk("Invoked by underlying framework to raise Event event.")]
		private static void OnEvent(int device, byte deviceIndex, byte id, float x, float y)
		{
			try
			{
				if (Event != null)
				{
					Event((InputDeviceType)device, deviceIndex, id, new Vector2(x, y));
				}
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		#endregion
	}
}