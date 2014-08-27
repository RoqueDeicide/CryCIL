using System;
using System.Linq;
using CryEngine.Annotations;
using CryEngine.Extensions;

namespace CryEngine
{
	public delegate void ActionMapEventDelegate(ActionMapEventArgs e);

	public delegate void KeyEventDelegate(KeyEventArgs e);

	public delegate void MouseEventDelegate(MouseEventArgs e);

	public static class Input
	{
		#region Events
		internal static void OnActionTriggered(string action, KeyEvent keyEvent, float value)
		{
			ActionmapEvents.Invoke(new ActionMapEventArgs(keyEvent, action, value));
		}

		internal static void OnKeyEvent(string keyName, float value)
		{
			if (KeyEvents != null)
				KeyEvents(new KeyEventArgs(keyName, value));
		}

		internal static void OnMouseEvent(int x, int y, MouseEvent mouseEvent, int wheelDelta)
		{
			MouseDeltaX = MouseX - x;
			MouseDeltaY = MouseY - y;

			MouseX = x;
			MouseY = y;

			if (MouseEvents != null)
				MouseEvents(new MouseEventArgs(x, y, wheelDelta, mouseEvent));
		}

		public static int MouseX { get; private set; }
		public static int MouseY { get; private set; }

		public static int MouseDeltaX { get; private set; }
		public static int MouseDeltaY { get; private set; }
		#endregion

		public static event KeyEventDelegate KeyEvents;
		internal static Delegate[] KeyEventsInvocationList
		{
			get
			{
				return KeyEvents != null ? KeyEvents.GetInvocationList() : null;
			}
		}

		public static event MouseEventDelegate MouseEvents;
		internal static Delegate[] MouseEventsInvocationList
		{
			get
			{
				return MouseEvents != null ? MouseEvents.GetInvocationList() : null;
			}
		}

		public static ActionmapHandler ActionmapEvents = new ActionmapHandler();

		[UsedImplicitly]
		private static void OnScriptInstanceDestroyed(CryScriptInstance instance)
		{
			// ReSharper disable PossibleUnintendedReferenceComparison
			Delegate[] invocationList = Input.KeyEvents.GetInvocationList();
			invocationList.Where(x=>x.Target == instance).ForEach(x=>KeyEvents -= (KeyEventDelegate)x);
			
			invocationList = Input.MouseEvents.GetInvocationList();
			invocationList.Where(x => x.Target == instance).ForEach(x => MouseEvents -= (MouseEventDelegate)x);

			ActionmapEvents.RemoveAll(instance);
			// ReSharper restore PossibleUnintendedReferenceComparison
		}
	}
}