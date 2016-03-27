using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.Engine.DebugServices;
using CryCil.RunTime;

namespace CryCil.Engine.Input.ActionMapping
{
	/// <summary>
	/// Represents an object that facilitates execution of functions that represent input actions.
	/// </summary>
	[InitializationClass]
	public class ActionMapHandler : IDisposable
	{
		#region Fields
		private IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid => this.handle != IntPtr.Zero;
		/// <summary>
		/// Gets or sets the value that indicates whether this action map is active.
		/// </summary>
		public bool Active
		{
			get { return this.IsValid && IsActive(this.handle); }
			set
			{
				if (this.IsValid)
				{
					Activate(this.handle, value);
				}
			}
		}
		#endregion
		#region Construction
		internal ActionMapHandler(IntPtr handle)
		{
			this.handle = handle;
		}
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="actionMap">An action map this object will handle.</param>
		/// <exception cref="ArgumentNullException">Given action map object is not valid.</exception>
		/// <exception cref="ArgumentException">
		/// Action map <paramref name="actionMap"/> is not associated with a static class.
		/// </exception>
		internal ActionMapHandler(CryActionMap actionMap)
		{
			if (!actionMap.IsValid)
			{
				throw new ArgumentNullException(nameof(actionMap), "Given action map object is not valid.");
			}
			if (!ActionMaps.AllRegisteredStaticActionMaps.ContainsKey(actionMap.NameHash))
			{
				throw new ArgumentException($"Action map {actionMap.Name} is not associated with a static class.");
			}
			this.handle = CreateInternal(null, actionMap);
		}
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="obj">      An object to execute the actions on.</param>
		/// <param name="actionMap">An action map this object will handle.</param>
		/// <exception cref="ArgumentNullException">Given action map object is not valid.</exception>
		/// <exception cref="ArgumentException">
		/// Action map <paramref name="actionMap"/> is not a managed one.
		/// </exception>
		internal ActionMapHandler(object obj, CryActionMap actionMap)
		{
			if (!actionMap.IsValid)
			{
				throw new ArgumentNullException(nameof(actionMap), "Given action map object is not valid.");
			}
			if (obj == null)
			{
				Log.Warning($"Attempted to create an object of type {nameof(ActionMapHandler)} by calling a " +
							$"constructor that expects a non-null object.", false);
			}
			this.handle = CreateInternal(obj, actionMap);

			if (!this.IsValid)
			{
				throw new ArgumentException($"Action map {actionMap.Name} is not a managed one.");
			}
		}
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="type">Type that represents the class that is associated with an action map.</param>
		/// <param name="obj"> An object that is going to process the actions.</param>
		public ActionMapHandler(Type type, object obj)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type), "Given type cannot be null.");
			}
			if (obj == null)
			{
				throw new ArgumentNullException(nameof(obj), "Object that is going to handle the action activation cannot be null.");
			}
			ActionMapAttribute attribute = type.GetAttribute<ActionMapAttribute>();
			if (attribute == null)
			{
				throw new NotSupportedException($"Type {type.FullName} is not associated with an action map.");
			}
			string actionMapName = attribute.Name ?? type.Name;

			this.handle = CreateInternal(obj, ActionMaps.GetActionMap(actionMapName));
		}
		#endregion
		#region Interface
		/// <summary>
		/// Deactivates the action map and releases the underlying object.
		/// </summary>
		public void Dispose()
		{
			if (!this.IsValid)
			{
				return;
			}

			Activate(this.handle, false);

			DestroyInternal(this.handle);

			this.handle = IntPtr.Zero;
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr CreateInternal(object obj, CryActionMap actionMap);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DestroyInternal(IntPtr handler);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsActive(IntPtr handler);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Activate(IntPtr handler, bool activate);
		#endregion
	}
}