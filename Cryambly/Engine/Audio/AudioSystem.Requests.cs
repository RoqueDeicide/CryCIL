using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CryCil.Annotations;
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
		/// <summary>
		/// Reserves an identifier for an audio object.
		/// </summary>
		/// <remarks>
		/// <para>
		/// There is no documentation available about reservation of identifiers and there are no known use
		/// cases.
		/// </para>
		/// <para>Right now it's recommended to pass references to static fields.</para>
		/// </remarks>
		/// <param name="id">  Reference to the identifier to reserve.</param>
		/// <param name="name">Name to associated with the reserved identifier.</param>
		public static void ReserveAudioObjectId(ref uint id, [CanBeNull] string name)
		{
			RequestReserveAudioId(ref id, name);
		}
		/// <summary>
		/// Requests a set of audio to be preloaded.
		/// </summary>
		/// <param name="id">Identifier of the set. Can be acquired through <see cref="TryGetPreloadRequestId"/>.</param>
		public static void PreloadAudioRequest(uint id)
		{
			RequestPreloadAudioRequest(id);
		}
		/// <summary>
		/// Requests a set of audio to be preloaded.
		/// </summary>
		/// <param name="name">Name of the set.</param>
		public static void PreloadAudioRequest(string name)
		{
			uint id;
			if (TryGetPreloadRequestId(name, out id))
			{
				RequestPreloadAudioRequest(id);
			}
		}
		/// <summary>
		/// Requests a set of audio to be unloaded.
		/// </summary>
		/// <param name="id">Identifier of the set. Can be acquired through <see cref="TryGetPreloadRequestId"/>.</param>
		public static void UnloadAudioRequest(uint id)
		{
			RequestUnloadAudioRequest(id);
		}
		/// <summary>
		/// Requests a set of audio to be unloaded.
		/// </summary>
		/// <param name="name">Name of the set.</param>
		public static void UnloadAudioRequest(string name)
		{
			uint id;
			if (TryGetPreloadRequestId(name, out id))
			{
				RequestUnloadAudioRequest(id);
			}
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestSetImpl(IntPtr implHandle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestReserveAudioId(ref uint id, string name);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestPreloadAudioRequest(uint id);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RequestUnloadAudioRequest(uint id);
		#endregion
	}
}