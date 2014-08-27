using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;

namespace CryEngine
{
	public struct AnimationEvent
	{
		public float Time;
		public UInt32 AnimNumberInQueue;
		public float AnimPriority;
		public string AnimPathName;
		public int AnimationIdentifier;
		public UInt32 EventNameLowercaseCrc32;
		public string EventName;
		/// <summary>
		/// Meaning depends on event - sound: sound path, effect: effect name
		/// </summary>
		public string CustomParameter;
		public string BonePathName;
		public Vector3 ViewOffset;
		public Vector3 ViewDirection;
	}
}