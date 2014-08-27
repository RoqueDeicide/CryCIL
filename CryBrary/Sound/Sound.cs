using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Annotations;
using CryEngine.Native;

namespace CryEngine
{
	public class Sound
	{
		#region Statics
		[UsedImplicitly]
		private static Sound TryGet(IntPtr soundPtr)
		{
			if (soundPtr == IntPtr.Zero)
				return null;

			var sound = sounds.FirstOrDefault(x => x.Handle == soundPtr);
			if (sound != null) return sound;

			sound = new Sound(soundPtr);
			sounds.Add(sound);

			return sound;
		}

		private static readonly List<Sound> sounds = new List<Sound>();
		#endregion

		internal Sound(IntPtr ptr)
		{
			Handle = ptr;
		}

		internal IntPtr Handle { get; set; }
	}
}