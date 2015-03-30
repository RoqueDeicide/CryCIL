using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides index of the gamepad that caused the key input event to be raised.
	/// </summary>
	public class GamepadKeyEventArgs : SimpleKeyEventArgs
	{
		#region Properties
		/// <summary>
		/// Gets the identifier of the device.
		/// </summary>
		public byte DeviceIndex { get; private set; }
		#endregion
		#region Construction
		/// <summary>
		/// Creates new object that provides information about virtual key input.
		/// </summary>
		/// <param name="key">        Identifier of the key.</param>
		/// <param name="deviceIndex">Identifier of the device.</param>
		/// <param name="pressed">    Indicates whether the key was pressed.</param>
		public GamepadKeyEventArgs(InputId key, byte deviceIndex, bool pressed)
			: base(key, ModifierMask.None, pressed)
		{
			this.DeviceIndex = deviceIndex;
		}
		#endregion
	}
}