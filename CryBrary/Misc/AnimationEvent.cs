using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	[CLSCompliant(false)]
	public struct AnimationEvent
	{
		public float Time { get; set; }
		UInt32 AnimNumberInQueue { get; set; }
		float AnimPriority { get; set; }
		string AnimPathName { get; set; }
		int AnimID { get; set; }
		UInt32 EventNameLowercaseCRC32 { get; set; }
		string EventName { get; set; }
		/// <summary>
		/// Meaning depends on event - sound: sound path, effect: effect name
		/// </summary>
		string CustomParameter { get; set; }
		string BonePathName { get; set; }
		Vec3 vOffset { get; set; }
		Vec3 vDir { get; set; }
	}
}