using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine
{
	public class Sound
	{
		#region Statics
		static Sound TryGet(IntPtr soundPtr)
		{
			if (soundPtr == IntPtr.Zero)
				return null;

			var sound = Sounds.FirstOrDefault(x => x.Handle == soundPtr);
			if(sound == null)
			{
				sound = new Sound(soundPtr);
				Sounds.Add(sound);
			}

			return sound;
		}

		static List<Sound> Sounds = new List<Sound>();
		#endregion

		internal Sound(IntPtr ptr)
		{
			Handle = ptr;
		}

		internal IntPtr Handle { get; set; }
	}
}
