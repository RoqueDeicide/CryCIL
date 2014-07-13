using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum EntityMoveFlags
	{
		Position = 1 << 1,
		Rotation = 1 << 2,
		Scale = 1 << 3,

		NoPropagate = 1 << 4,

		/// <summary>
		/// When parent changes his transformation.
		/// </summary>
		FromParent = 1 << 5,

		PhysicsStep = 1 << 13,
		Editor = 1 << 14,
		TrackView = 1 << 15,
		TimeDemo = 1 << 16,
	}
}