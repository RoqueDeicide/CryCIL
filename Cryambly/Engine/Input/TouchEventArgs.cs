using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides information about the touch input event.
	/// </summary>
	public class TouchEventArgs : EventArgs
	{
		#region Properties
		/// <summary>
		/// Gets the type of the input device the touch event originated from.
		/// </summary>
		public InputDeviceType Device { get; private set; }
		/// <summary>
		/// Gets the index of the device the touch event originated from.
		/// </summary>
		public byte DeviceIndex { get; private set; }
		/// <summary>
		/// Unknown.
		/// </summary>
		public byte Id { get; private set; }
		/// <summary>
		/// Gets location of the touch on the touch screen.
		/// </summary>
		public Vector2 Position { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="device">     Type of the input device the touch event originated from.</param>
		/// <param name="deviceIndex">Index of the device the touch event originated from.</param>
		/// <param name="id">         Unknown.</param>
		/// <param name="position">   Location of the touch on the touch screen.</param>
		public TouchEventArgs(InputDeviceType device, byte deviceIndex, byte id, Vector2 position)
		{
			this.Device = device;
			this.DeviceIndex = deviceIndex;
			this.Id = id;
			this.Position = position;
		}
		#endregion
	}
}