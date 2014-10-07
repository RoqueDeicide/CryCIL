using System;
using CryEngine.Mathematics;

namespace CryEngine.Animations
{
	public struct AnimationEvent
	{
		public float Time;
		public uint AnimNumberInQueue;
		public float AnimPriority;
		public string AnimPathName;
		public int AnimationIdentifier;
		public uint EventNameLowercaseCrc32;
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