using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.RunTime;
using CryCil.Utilities;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Provides access to CryEngine ActionMaps API.
	/// </summary>
	[InitializationClass]
	public static unsafe partial class ActionMaps
	{
		#region Fields
		internal static readonly SortedList<uint, string> AllRegisteredActionMaps = new SortedList<uint, string>();
		internal static readonly SortedList<uint, string> AllRegisteredInstanceActionMaps = new SortedList<uint, string>();
		internal static readonly SortedList<uint, string> AllRegisteredStaticActionMaps = new SortedList<uint, string>();
		#endregion
		#region Interface
		/// <summary>
		/// Enables the action map that is associated with a static class.
		/// </summary>
		/// <typeparam name="StaticActionMapType">
		/// Type that represents the static class that is associated with the action map.
		/// </typeparam>
		/// <param name="enable">Indicates whether action map must be activated.</param>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="StaticActionMapType"/> is not a static class.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="StaticActionMapType"/> is not a class that is associated with an action
		/// map.
		/// </exception>
		public static void Enable<StaticActionMapType>(bool enable = true)
		{
			var type = typeof(StaticActionMapType);
			if (!(type.IsSealed && type.IsAbstract))
			{
				throw new NotSupportedException($"Type {type.FullName} is not a static class.");
			}
			ActionMapHandler handler = StaticActionMapDelegate<StaticActionMapType>.Handler;
			if (handler == null)
			{
				throw new ArgumentException($"Type {type.FullName} is not a class that is associated with an " +
											$"action map.");
			}

			handler.Active = enable;
		}
		/// <summary>
		/// Disables the action map that is associated with a static class.
		/// </summary>
		/// <typeparam name="StaticActionMapType">
		/// Type that represents the static class that is associated with the action map.
		/// </typeparam>
		/// <param name="disable">Indicates whether action map must be deactivated.</param>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="StaticActionMapType"/> is not a static class.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="StaticActionMapType"/> is not a class that is associated with an action
		/// map.
		/// </exception>
		public static void Disable<StaticActionMapType>(bool disable = true)
		{
			var type = typeof(StaticActionMapType);
			if (!(type.IsSealed && type.IsAbstract))
			{
				throw new NotSupportedException($"Type {type.FullName} is not a static class.");
			}
			ActionMapHandler handler = StaticActionMapDelegate<StaticActionMapType>.Handler;
			if (handler == null)
			{
				throw new ArgumentException($"Type {type.FullName} is not a class that is associated with an " +
											$"action map.");
			}

			handler.Active = !disable;
		}
		/// <summary>
		/// Indicates whether the action map that is associated with a static class is active.
		/// </summary>
		/// <typeparam name="StaticActionMapType">
		/// Type that represents the static class that is associated with the action map.
		/// </typeparam>
		/// <returns>True, if the action map is active.</returns>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="StaticActionMapType"/> is not a static class.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Type <typeparamref name="StaticActionMapType"/> is not a class that is associated with an action
		/// map.
		/// </exception>
		public static bool Enabled<StaticActionMapType>()
		{
			var type = typeof(StaticActionMapType);
			if (!(type.IsSealed && type.IsAbstract))
			{
				throw new NotSupportedException($"Type {type.FullName} is not a static class.");
			}
			ActionMapHandler handler = StaticActionMapDelegate<StaticActionMapType>.Handler;
			if (handler == null)
			{
				throw new ArgumentException($"Type {type.FullName} is not a class that is associated with an " +
											$"action map.");
			}

			return handler.Active;
		}

		[UnmanagedThunk]
		private static void ActivateAction(string actionName, InputActionHandler eventHandler,
										   ActionActivationMode activation, float value)
		{
			eventHandler?.Invoke(actionName, activation, value);
		}
		/// <summary>
		/// Saves the rebind data into the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool SaveRebindData(string file)
		{
			return SyncRebindDataWithFile(file, true);
		}
		/// <summary>
		/// Saves the rebind data into the Xml node.
		/// </summary>
		/// <param name="node">Object that represents the Xml node.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool SaveRebindData(CryXmlNode node)
		{
			return SyncRebindDataWithNode(node, true);
		}
		/// <summary>
		/// Loads the rebind data from the file.
		/// </summary>
		/// <param name="file">Path to the file.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool LoadRebindData(string file)
		{
			return SyncRebindDataWithFile(file, false);
		}
		/// <summary>
		/// Loads the rebind data from the Xml node.
		/// </summary>
		/// <param name="node">Object that represents the Xml node.</param>
		/// <returns>True, if operation was successful.</returns>
		public static bool LoadRebindData(CryXmlNode node)
		{
			return SyncRebindDataWithNode(node, false);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AddDeviceMapping(SupportedInputDevices device);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CryActionMap CreateActionMap(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern CryActionMap GetActionMap(string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CreateInternalActionMap(CryActionMap actionMap,
														   CryInputAction* actions, int actionCount,
														   IntPtr* fields, int fieldCount,
														   IntPtr* methods);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SyncRebindDataWithFile(string file, bool save);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SyncRebindDataWithNode(CryXmlNode node, bool save);
		#endregion
	}
}