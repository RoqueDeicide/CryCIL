using System;

namespace CryEngine
{
	public class KeyEventArgs : EventArgs
	{
		public KeyEventArgs(string actionName, float value)
		{
			ActionName = actionName;
			Value = value;
		}

		public string ActionName { get; private set; }

		public float Value { get; private set; }
	}
}