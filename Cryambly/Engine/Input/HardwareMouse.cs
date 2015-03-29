using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;

namespace CryCil.Engine.Input
{
	/// <summary>
	/// Provides access to CryEngine IHardwareMouse API.
	/// </summary>
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
		public static event EventHandler<EventArgs<Vector2Int32>> RightMouseButtonDown;
		/// <summary>
		/// Occurs when right mouse button is released.
		/// </summary>
		public static event EventHandler<EventArgs<Vector2Int32>> RightMouseButtonUp;
		/// <summary>
		/// Occurs when right mouse button is double-clicked.
		/// </summary>
		/// <remarks>Series of events when double-clicking: Down - Up - DoubleClick - Up.</remarks>
		public static event EventHandler<EventArgs<Vector2Int32>> RightMouseButtonDoubleClick;
		/// <summary>
		/// Occurs when left mouse button is pressed.
		/// </summary>
		public static event EventHandler<EventArgs<Vector2Int32>> LeftMouseButtonDown;
		/// <summary>
		/// Occurs when left mouse button is released.
		/// </summary>
		public static event EventHandler<EventArgs<Vector2Int32>> LeftMouseButtonUp;
		/// <summary>
		/// Occurs when left mouse button is double-clicked.
		/// </summary>
		/// <remarks>Series of events when double-clicking: Down - Up - DoubleClick - Up.</remarks>
		public static event EventHandler<EventArgs<Vector2Int32>> LeftMouseButtonDoubleClick;
		/// <summary>
		/// Occurs when middle mouse button is pressed.
		/// </summary>
		public static event EventHandler<EventArgs<Vector2Int32>> MiddleMouseButtonDown;
		/// <summary>
		/// Occurs when middle mouse button is released.
		/// </summary>
		public static event EventHandler<EventArgs<Vector2Int32>> MiddleMouseButtonUp;
		/// <summary>
		/// Occurs when middle mouse button is double-clicked.
		/// </summary>
		/// <remarks>Series of events when double-clicking: Down - Up - DoubleClick - Up.</remarks>
		public static event EventHandler<EventArgs<Vector2Int32>> MiddleMouseButtonDoubleClick;
		/// <summary>
		/// Occurs when mouse movement is registered.
		/// </summary>
		public static event EventHandler<EventArgs<Vector2Int32>> Move;
		/// <summary>
		/// Occurs when mouse wheel rotation is registered.
		/// </summary>
		public static event EventHandler<EventArgs<int>> Wheel;
		#endregion
		#region Interface
		/// <summary>
		/// Signals underlying framework that mouse pointer needs to be on the screen.
		/// </summary>
		/// <returns>
		/// There is an internal counter that is in(de)cremented when <see cref="Show"/>(
		/// <see cref="Hide()"/>) is invoked. When the counter is positive, the mouse appears on the
		/// screen.
		/// </returns>
		public static void Show()
		{
			IncrementCounter();
		}
		/// <summary>
		/// Signals underlying framework that mouse pointer needs to disappear from the screen.
		/// </summary>
		/// <returns>
		/// There is an internal counter that is in(de)cremented when <see cref="Show"/>(
		/// <see cref="Hide()"/>) is invoked. When the counter is positive, the mouse appears on the
		/// screen.
		/// </returns>
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
		[PublicAPI("Invoked from underlying framework to raise RightMouseButtonDown event.")]
		private static void OnRightMouseButtonDown(int x, int y)
		{
			if (RightMouseButtonDown != null)
			{
				RightMouseButtonDown(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise RightMouseButtonUp event.")]
		private static void OnRightMouseButtonUp(int x, int y)
		{
			if (RightMouseButtonUp != null)
			{
				RightMouseButtonUp(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise RightMouseButtonDoubleClick event.")]
		private static void OnRightMouseButtonDoubleClick(int x, int y)
		{
			if (RightMouseButtonDoubleClick != null)
			{
				RightMouseButtonDoubleClick(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise LeftMouseButtonDown event.")]
		private static void OnLeftMouseButtonDown(int x, int y)
		{
			if (LeftMouseButtonDown != null)
			{
				LeftMouseButtonDown(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise LeftMouseButtonUp event.")]
		private static void OnLeftMouseButtonUp(int x, int y)
		{
			if (LeftMouseButtonUp != null)
			{
				LeftMouseButtonUp(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise LeftMouseButtonDoubleClick event.")]
		private static void OnLeftMouseButtonDoubleClick(int x, int y)
		{
			if (LeftMouseButtonDoubleClick != null)
			{
				LeftMouseButtonDoubleClick(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise MiddleMouseButtonDown event.")]
		private static void OnMiddleMouseButtonDown(int x, int y)
		{
			if (MiddleMouseButtonDown != null)
			{
				MiddleMouseButtonDown(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise MiddleMouseButtonUp event.")]
		private static void OnMiddleMouseButtonUp(int x, int y)
		{
			if (MiddleMouseButtonUp != null)
			{
				MiddleMouseButtonUp(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise MiddleMouseButtonDoubleClick event.")]
		private static void OnMiddleMouseButtonDoubleClick(int x, int y)
		{
			if (MiddleMouseButtonDoubleClick != null)
			{
				MiddleMouseButtonDoubleClick(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise Move event.")]
		private static void OnMove(int x, int y)
		{
			if (Move != null)
			{
				Move(null, new EventArgs<Vector2Int32>(new Vector2Int32(x, y)));
			}
		}
		[PublicAPI("Invoked from underlying framework to raise Wheel event.")]
		private static void OnWheel(int delta)
		{
			if (Wheel != null)
			{
				Wheel(null, new EventArgs<int>(delta));
			}
		}
		#endregion
	}
}