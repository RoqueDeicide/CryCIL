using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CryCil.RunTime;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Defines signature of methods that handle mouse click events.
	/// </summary>
	/// <param name="x">
	/// X-component of the vector that designates position of the mouse cursor at the moment of click.
	/// </param>
	/// <param name="y">
	/// Y-component of the vector that designates position of the mouse cursor at the moment of click.
	/// </param>
	public delegate void MouseClickHandler(int x, int y);
	/// <summary>
	/// Defines signature of methods that handle mouse wheel rotation events.
	/// </summary>
	/// <param name="delta">
	/// A value that represents how much the orientation of the wheel has changed.
	/// </param>
	public delegate void MouseWheelHandler(int delta);
	/// <summary>
	/// Provides access to CryEngine IHardwareMouse API.
	/// </summary>
	/// <remarks>This class provides means of getting raw and unchanged mouse input.</remarks>
	public static class HardwareMouse
	{
		#region Internal Calls
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void IncrementCounter();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DecrementCounter();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Vector2 GetAbsolutePosition();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetAbsolutePosition(Vector2 value);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Vector2 GetClientPosition();
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetClientPosition(Vector2 value);
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets position of the mouse pointer relative to the left-upper corner of the screen.
		/// </summary>
		public static Vector2 Position
		{
			get { return GetAbsolutePosition(); }
			set { SetAbsolutePosition(value); }
		}
		/// <summary>
		/// Gets or sets position of the mouse pointer relative to the left-upper corner of the
		/// application's window.
		/// </summary>
		public static Vector2 ClientPosition
		{
			get { return GetClientPosition(); }
			set { SetClientPosition(value); }
		}
		#endregion
		#region Events
		/// <summary>
		/// Occurs when right mouse button is pressed.
		/// </summary>
		public static event MouseClickHandler RightMouseButtonDown;
		/// <summary>
		/// Occurs when right mouse button is released.
		/// </summary>
		public static event MouseClickHandler RightMouseButtonUp;
		/// <summary>
		/// Occurs when right mouse button is double-clicked.
		/// </summary>
		/// <remarks>Series of events when double-clicking: Down - Up - DoubleClick - Up.</remarks>
		public static event MouseClickHandler RightMouseButtonDoubleClick;
		/// <summary>
		/// Occurs when left mouse button is pressed.
		/// </summary>
		public static event MouseClickHandler LeftMouseButtonDown;
		/// <summary>
		/// Occurs when left mouse button is released.
		/// </summary>
		public static event MouseClickHandler LeftMouseButtonUp;
		/// <summary>
		/// Occurs when left mouse button is double-clicked.
		/// </summary>
		/// <remarks>Series of events when double-clicking: Down - Up - DoubleClick - Up.</remarks>
		public static event MouseClickHandler LeftMouseButtonDoubleClick;
		/// <summary>
		/// Occurs when middle mouse button is pressed.
		/// </summary>
		public static event MouseClickHandler MiddleMouseButtonDown;
		/// <summary>
		/// Occurs when middle mouse button is released.
		/// </summary>
		public static event MouseClickHandler MiddleMouseButtonUp;
		/// <summary>
		/// Occurs when middle mouse button is double-clicked.
		/// </summary>
		/// <remarks>Series of events when double-clicking: Down - Up - DoubleClick - Up.</remarks>
		public static event MouseClickHandler MiddleMouseButtonDoubleClick;
		/// <summary>
		/// Occurs when mouse movement is registered.
		/// </summary>
		public static event MouseClickHandler Move;
		/// <summary>
		/// Occurs when mouse wheel rotation is registered.
		/// </summary>
		public static event MouseWheelHandler Wheel;
		#endregion
		#region Interface
		/// <summary>
		/// Signals underlying framework that mouse pointer needs to be on the screen.
		/// </summary>
		/// <remarks>
		/// There is an internal counter that is in(de)cremented when <see cref="Show"/>(
		/// <see cref="Hide()"/>) is invoked. When the counter is positive, the mouse appears on the screen.
		/// </remarks>
		public static void Show()
		{
			IncrementCounter();
		}
		/// <summary>
		/// Signals underlying framework that mouse pointer needs to disappear from the screen.
		/// </summary>
		/// <remarks>
		/// There is an internal counter that is in(de)cremented when <see cref="Show"/>(
		/// <see cref="Hide()"/>) is invoked. When the counter is positive, the mouse appears on the screen.
		/// </remarks>
		public static void Hide()
		{
			IncrementCounter();
		}
		/// <summary>
		/// Resets status of the mouse cursor.
		/// </summary>
		/// <param name="visibleByDefault">
		/// Indicates whether mouse cursor has to show up on the screen.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Reset(bool visibleByDefault);
		/// <summary>
		/// Instructs application to either prevent mouse cursor from leaving window borders or not.
		/// </summary>
		/// <param name="confine">
		/// Indicates whether mouse cursor should be confined to the borders of this game's window.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ConfineCursor(bool confine);
		/// <summary>
		/// Instructs application to either hide or show mouse cursor on the screen.
		/// </summary>
		/// <param name="hide">Indicates whether mouse cursor should be hidden or shown.</param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Hide(bool hide);
		/// <summary>
		/// Instructs application to either use or not use mouse cursor image defined by the system.
		/// </summary>
		/// <param name="useSystemCursor">
		/// Indicates whether mouse cursor's graphic should be defined by the system.
		/// </param>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UseSystemCursor(bool useSystemCursor);
		#endregion
		#region Utilities
		[RawThunk("Invoked from underlying framework to raise RightMouseButtonDown event.")]
		private static void OnRightMouseButtonDown(int x, int y)
		{
			try
			{
				RightMouseButtonDown?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise RightMouseButtonUp event.")]
		private static void OnRightMouseButtonUp(int x, int y)
		{
			try
			{
				RightMouseButtonUp?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise RightMouseButtonDoubleClick event.")]
		private static void OnRightMouseButtonDoubleClick(int x, int y)
		{
			try
			{
				RightMouseButtonDoubleClick?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise LeftMouseButtonDown event.")]
		private static void OnLeftMouseButtonDown(int x, int y)
		{
			try
			{
				LeftMouseButtonDown?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise LeftMouseButtonUp event.")]
		private static void OnLeftMouseButtonUp(int x, int y)
		{
			try
			{
				LeftMouseButtonUp?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise LeftMouseButtonDoubleClick event.")]
		private static void OnLeftMouseButtonDoubleClick(int x, int y)
		{
			try
			{
				LeftMouseButtonDoubleClick?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise MiddleMouseButtonDown event.")]
		private static void OnMiddleMouseButtonDown(int x, int y)
		{
			try
			{
				MiddleMouseButtonDown?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise MiddleMouseButtonUp event.")]
		private static void OnMiddleMouseButtonUp(int x, int y)
		{
			try
			{
				MiddleMouseButtonUp?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise MiddleMouseButtonDoubleClick event.")]
		private static void OnMiddleMouseButtonDoubleClick(int x, int y)
		{
			try
			{
				MiddleMouseButtonDoubleClick?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise Move event.")]
		private static void OnMove(int x, int y)
		{
			try
			{
				Move?.Invoke(x, y);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		[RawThunk("Invoked from underlying framework to raise Wheel event.")]
		private static void OnWheel(int delta)
		{
			try
			{
				Wheel?.Invoke(delta);
			}
			catch (Exception ex)
			{
				MonoInterface.DisplayException(ex);
			}
		}
		#endregion
	}
}