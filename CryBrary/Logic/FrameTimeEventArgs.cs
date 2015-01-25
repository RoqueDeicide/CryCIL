using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Logic
{
	/// <summary>
	/// Encapsulates time that passed since last frame to the moment of the event.
	/// </summary>
	public class FrameTimeEventArgs : EventArgs
	{
		/// <summary>
		/// Time that passed since last frame to the moment of the event in seconds.
		/// </summary>
		public float DeltaTime { get; private set; }
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="deltaTime">
		/// Time that passed since last frame to the moment of the event in seconds.
		/// </param>
		public FrameTimeEventArgs(float deltaTime)
		{
			this.DeltaTime = deltaTime;
		}
	}
}