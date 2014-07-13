using System;

namespace CryEngine
{
	public class ActionMapEventArgs : EventArgs
	{
		public ActionMapEventArgs(KeyEvent keyEvent, string actionName, float value)
		{
			KeyEvent = keyEvent;
			ActionName = actionName;
			Value = value;
		}

		public string ActionName { get; private set; }

		public KeyEvent KeyEvent { get; private set; }

		public float Value { get; private set; }
	}
}