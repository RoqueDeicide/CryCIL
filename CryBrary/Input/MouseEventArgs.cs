using System;

namespace CryEngine
{
	public class MouseEventArgs : EventArgs
	{
		public MouseEventArgs(int x, int y, int wheelDelta, MouseEvent mouseEvent)
		{
			X = x;
			Y = y;
			WheelDelta = wheelDelta;
			MouseEvent = mouseEvent;
		}

		public int X { get; private set; }

		public int Y { get; private set; }

		public int WheelDelta { get; private set; }

		public MouseEvent MouseEvent { get; private set; }
	}
}