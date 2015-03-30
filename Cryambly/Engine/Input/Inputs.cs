using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryCil.Engine.Input
{
	internal static class InputEventPropagator
	{
		internal static bool Post<ArgsType>(List<EventHandler<ArgsType>> handlers, ArgsType args)
			where ArgsType : InputEventArgs
		{
			for (int i = 0; i < handlers.Count; i++)
			{
				handlers[i](null, args);
				if (args.BlockFurtherHandling)
				{
					return true;
				}
			}
			return false;
		}
	}
	/// <summary>
	/// Provides access to general CryEngine Input API.
	/// </summary>
	public static class Inputs
	{
		#region Fields
		
		#endregion
		#region Properties
		
		#endregion
		#region Events
		#endregion
		#region Construction

		#endregion
		#region Interface
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool DeviceAvailable(InputDeviceType type);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetModifiers();
		/// <summary>
		/// Clears states of all keys.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearKeys();
		/// <summary>
		/// Clears states of analog inputs.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearAnalogInputs();
		#endregion
		#region Utilities

		#endregion
	}
}
