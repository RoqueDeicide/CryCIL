using System;
using System.Collections.Generic;
using System.Linq;
using CryCil.Engine.Audio;
using CryCil.Engine.DebugServices;

namespace CryCil.RunTime.Registration
{
	[InitializationClass]
	internal static class AudioSystemImplementations
	{
		internal static SortedList<string, Type> RegisteredImplementations = new SortedList<string, Type>();
		
		[InitializationStage((int)DefaultInitializationStages.AudioSystemRegistrationStage)]
		internal static void RegisterAudioSystemImplementations(int stageIndex)
		{
#if DEBUG
			Log.Line("Beginning registration process for implementations of the audio system.");

			Log.Comment("Acquiring a list of all types that derive from {0} and have a parameterless constructor.",
						typeof(AudioSystemImplementation).FullName);
#endif

			var types = from assembly in MonoInterface.CryCilAssemblies
						from type in assembly.GetTypes()
						where type.Implements<AudioSystemImplementation>() &&
							  type.HasDefaultConstructor() &&
							  type.ContainsAttribute<AudioSystemImplementationAttribute>()
						select type;

			var typesArray = types.ToArray();

#if DEBUG
			Log.Comment("{0} types were found.", typesArray.Length);
#endif

			if (typesArray.IsNullOrEmpty())
			{
				return;
			}

#if DEBUG
			Log.Comment("Processing found types.");
#endif

			for (int i = 0; i < typesArray.Length; i++)
			{
				Type type = typesArray[i];

				Log.Line("Type {0} has been registered as an implementation of the audio system under name {1}.",
						 type.FullName, type.Name);
				RegisteredImplementations.Add(type.Name, type);
			}
		}
	}
}