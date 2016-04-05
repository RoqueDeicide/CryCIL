using System;
using System.Linq;
using CryCil.Hashing;

namespace CryCil.Engine.Models.Characters
{
	/// <summary>
	/// Encapsulates details about the character animation event.
	/// </summary>
	public struct AnimationEvent
	{
		#region Fields
		private readonly float time;
		private readonly float endTime;
		private readonly LowerCaseCrc32 eventNameHash;
		private readonly string eventName;
		private readonly string parameter;
		private readonly string boneName;
		private readonly Vector3 offset;
		private readonly Vector3 direction;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the normalized time (a value between 0 and 1) that specifies the start of the time interval
		/// when the event is occurring.
		/// </summary>
		public float StartTime => this.time;
		/// <summary>
		/// Gets the normalized time (a value between 0 and 1) that specifies the end of the time interval
		/// when the event is occurring.
		/// </summary>
		public float EndTime => this.endTime;
		/// <summary>
		/// Gets CRC32 hash code of the name of the animation.
		/// </summary>
		public LowerCaseCrc32 EventNameHash => this.eventNameHash;
		/// <summary>
		/// Gets the name of the event.
		/// </summary>
		public string EventName => this.eventName;
		/// <summary>
		/// Gets the string that contains the custom text data that was provided by the event.
		/// </summary>
		public string Parameter => this.parameter;
		/// <summary>
		/// Gets the name of the bone that is associated with the event.
		/// </summary>
		public string BoneName => this.boneName;
		/// <summary>
		/// A set of coordinates that represents the event-specific offset. Can be used to spawn skeleton
		/// effects.
		/// </summary>
		public Vector3 Offset => this.offset;
		/// <summary>
		/// Gets a normalized vector that represents the event-specific direction. Can be used to spawn
		/// skeleton effects.
		/// </summary>
		public Vector3 Direction => this.direction;
		#endregion
		#region Construction
		internal AnimationEvent(float time, float endTime, LowerCaseCrc32 eventNameHash, string eventName,
								string parameter, string boneName, Vector3 offset, Vector3 direction)
		{
			this.time = time;
			this.endTime = endTime;
			this.eventNameHash = eventNameHash;
			this.eventName = eventName;
			this.parameter = parameter;
			this.boneName = boneName;
			this.offset = offset;
			this.direction = direction;
		}
		#endregion
	}
}