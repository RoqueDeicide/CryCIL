using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryCil.Engine.DebugServices;
using CryCil.RunTime.Registration;

namespace CryCil.Engine.Audio
{
	public static partial class AudioSystem
	{
		#region Fields
		private static readonly List<AudioSystemImplementation> loadedImplementations =
			new List<AudioSystemImplementation>();
		#endregion
		#region Properties
		#endregion
		#region Events
		#endregion
		#region Construction
		#endregion
		#region Interface
		/// <summary>
		/// Switches audio system to a different implementation.
		/// </summary>
		/// <param name="type">Type of implementation.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="type"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// Given type is not a registered implementation of the audio system.
		/// </exception>
		public static void SetAudioSystemImplementation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!AudioSystemImplementations.RegisteredImplementations.ContainsKey(type.Name))
			{
				throw new NotSupportedException(type.FullName + " not a registered implementation of the audio system.");
			}

			var loadedImplementation = loadedImplementations.Find(implementation => implementation.GetType() == type);

			if (loadedImplementation == null)
			{
				// ReSharper disable once PossibleNullReferenceException

				// ReSharper disable ExceptionNotDocumented
				loadedImplementation = type.GetConstructor(Type.EmptyTypes).Invoke(null) as AudioSystemImplementation;
				// ReSharper restore ExceptionNotDocumented

				if (loadedImplementation == null)
				{
					Log.Error(true, "Failed to create managed object of type {0}.", type.FullName);
					return;
				}

				loadedImplementation.Handle = CreateNativeImplementationObject(loadedImplementation);

				loadedImplementations.Add(loadedImplementation);
			}

			RequestSetImpl(loadedImplementation.Handle);
		}
		/// <summary>
		/// Switches audio system to a different implementation. Unlike
		/// <see cref="M:CryCil.Engine.Audio.AudioSystem.SetAudioSystemImplementation(System.Type)"/> this
		/// overload can be used to set native implementations.
		/// </summary>
		/// <param name="name">Name of the implementation to set.</param>
		/// <exception cref="ArgumentNullException">
		/// Name of the audio system implementation cannot be null.
		/// </exception>
		public static void SetAudioSystemImplementation(string name)
		{
			if (name.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException("name", "Name of the audio system implementation cannot be null.");
			}

			Type registeredType;
			if (AudioSystemImplementations.RegisteredImplementations.TryGetValue(name, out registeredType))
			{
				SetAudioSystemImplementation(registeredType);
			}

			// If given audio system is not a registered managed implementation, try setting the console
			// variable, in case we were given the name of native implementation.
			var consoleVariable = CryConsole.GetVariable("s_AudioSystemImplementationName");
			consoleVariable.ValueString = name;
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetImpl(IntPtr implHandle);
		#endregion
	}
}