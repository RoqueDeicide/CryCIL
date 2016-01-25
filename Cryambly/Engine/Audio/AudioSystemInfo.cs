using System;
using System.Linq;

namespace CryCil.Engine.Audio
{
	/// <summary>
	/// Encapsulates information about the audio system.
	/// </summary>
	public struct AudioSystemInfo
	{
		#region Fields
		private IntPtr countUsedAudioTriggers;
		private IntPtr countUnusedAudioTriggers;
		private IntPtr countUsedAudioEvents;
		private IntPtr countUnusedAudioEvents;
		private readonly Vector3 listenerPos;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the number of audio trigger that are used by the audio system at this moment.
		/// </summary>
		public long UsedAudioTriggersCount
		{
			get { return this.countUsedAudioTriggers.ToInt64(); }
		}
		/// <summary>
		/// Gets the number of audio trigger that are used by the audio system at this moment.
		/// </summary>
		public long UnusedAudioTriggersCount
		{
			get { return this.countUnusedAudioTriggers.ToInt64(); }
		}
		/// <summary>
		/// Gets the number of audio trigger that are used by the audio system at this moment.
		/// </summary>
		public long UsedAudioEventsCount
		{
			get { return this.countUsedAudioEvents.ToInt64(); }
		}
		/// <summary>
		/// Gets the number of audio trigger that are used by the audio system at this moment.
		/// </summary>
		public long UnusedAudioEventsCount
		{
			get { return this.countUnusedAudioEvents.ToInt64(); }
		}
		/// <summary>
		/// Gets the coordinates of the audio system listener.
		/// </summary>
		public Vector3 ListenerPosition
		{
			get { return this.listenerPos; }
		}
		#endregion
		#region Construction
		internal AudioSystemInfo(IntPtr countUsedAudioTriggers, IntPtr countUnusedAudioTriggers,
								 IntPtr countUsedAudioEvents, IntPtr countUnusedAudioEvents,
								 Vector3 listenerPos)
		{
			this.countUsedAudioTriggers = countUsedAudioTriggers;
			this.countUnusedAudioTriggers = countUnusedAudioTriggers;
			this.countUsedAudioEvents = countUsedAudioEvents;
			this.countUnusedAudioEvents = countUnusedAudioEvents;
			this.listenerPos = listenerPos;
		}
		#endregion
	}
}