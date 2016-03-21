using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Provides access to CryEngine facial animation API.
	/// </summary>
	public static class FacialAnimation
	{
		#region Interface
		/// <summary>
		/// Clears all cached data: sequences and such.
		/// </summary>
		public static void ClearCaches()
		{
			ClearAllCachesInternal();
		}
		/// <summary>
		/// Creates a new library for facial effectors.
		/// </summary>
		/// <returns>A valid object that represents an empty library for facial effectors.</returns>
		public static FacialEffectorsLibrary CreateEffectorsLibrary()
		{
			return CreateEffectorsLibraryInternal();
		}
		/// <summary>
		/// Removes the facial effectors library from the cache.
		/// </summary>
		/// <param name="file">Name of the file the library was loaded from.</param>
		public static void ClearEffectorsLibraryFromCache(string file)
		{
			ClearEffectorsLibraryFromCacheInternal(file);
		}
		/// <summary>
		/// Loads a library of facial effectors.
		/// </summary>
		/// <param name="file">File where the library is stored.</param>
		/// <returns>A valid object that represents a library if successful.</returns>
		public static FacialEffectorsLibrary LoadEffectorsLibrary(string file)
		{
			return LoadEffectorsLibraryInternal(file);
		}
		/// <summary>
		/// Creates a facial animation sequence.
		/// </summary>
		/// <returns>A valid object that represents an empty animation sequence.</returns>
		public static FacialAnimationSequence CreateSequence()
		{
			return CreateSequenceInternal();
		}
		/// <summary>
		/// Removes the facial animation sequence from the cache.
		/// </summary>
		/// <param name="file">Name of the file the sequence was loaded from.</param>
		public static void ClearSequenceFromCache(string file)
		{
			ClearSequenceFromCacheInternal(file);
		}
		/// <summary>
		/// Loads a facial animation sequence synchronously.
		/// </summary>
		/// <param name="file">      Name of the file to load the sequence from.</param>
		/// <param name="noWarnings">Indicates whether any warnings must be supressed.</param>
		/// <param name="addToCache">Indicates whether loaded sequence must be cached.</param>
		/// <returns>An object that represents a loaded sequence if successful.</returns>
		public static FacialAnimationSequence LoadSequence(string file, bool noWarnings = false,
														   bool addToCache = true)
		{
			return LoadSequenceInternal(file, noWarnings, addToCache);
		}
		/// <summary>
		/// Loads a facial animation sequence asynchronously.
		/// </summary>
		/// <param name="file">Name of the file to load the sequence from.</param>
		/// <returns>An object that will represent a loaded sequence once it's loaded.</returns>
		public static FacialAnimationSequence LoadSequenceAsync(string file)
		{
			return StartStreamingSequenceInternal(file);
		}
		// Looks for a sequence amongst the currently loaded sequences.
		/// <summary>
		/// Looks up a loaded sequence.
		/// </summary>
		/// <param name="file">Name of the file the sequence was loaded from.</param>
		/// <returns>A valid object, if found.</returns>
		public static FacialAnimationSequence FindLoadedSequence(string file)
		{
			return FindLoadedSequenceInternal(file);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearAllCachesInternal();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffectorsLibrary CreateEffectorsLibraryInternal();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearEffectorsLibraryFromCacheInternal(string filename);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialEffectorsLibrary LoadEffectorsLibraryInternal(string filename);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationSequence CreateSequenceInternal();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ClearSequenceFromCacheInternal(string filename);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationSequence LoadSequenceInternal(string filename, bool bNoWarnings, bool addToCache);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationSequence StartStreamingSequenceInternal(string filename);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FacialAnimationSequence FindLoadedSequenceInternal(string filename);
		#endregion
	}
}