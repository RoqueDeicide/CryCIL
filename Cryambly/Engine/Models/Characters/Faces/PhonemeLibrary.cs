using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using CryCil.MemoryMapping;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a library of phonemes.
	/// </summary>
	public static unsafe class PhonemeLibrary
	{
		#region Properties
		/// <summary>
		/// Gets the number of phonemes in this library.
		/// </summary>
		public static int Count
		{
			get { return GetPhonemeCount(); }
		}
		#endregion
		#region Interface
		/// <summary>
		/// Gets the object that encapsulates information about a phoneme.
		/// </summary>
		/// <param name="index">Zero-based index of the phoneme.</param>
		/// <exception cref="IndexOutOfRangeException">Index of the phoneme was out of range.</exception>
		public static PhonemeInfo GetInfo(int index)
		{
			PhonemeInfo info;
			if (GetPhonemeInfo(index, out info))
			{
				return info;
			}
			throw new IndexOutOfRangeException("Index of the phoneme was out of range.");
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="phonemeName">
		/// A sequence of 4 (at most) ASCII symbols that represent the name of the phoneme to look for.
		/// </param>
		/// <returns>Zero-based index of the phoneme, if found. -1 otherwise(?).</returns>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme must be a sequence of 4 ASCII characters at most.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme can only contain the ASCII characters.
		/// </exception>
		[SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional", Justification = "Reviewed")]
		public static int IndexOf(string phonemeName)
		{
			if (phonemeName == null)
			{
				return -1;
			}
			try
			{
				if (Encoding.ASCII.GetByteCount(phonemeName) > 4)
				{
					throw new ArgumentException("The name of the phoneme must be a sequence of 4 ASCII characters at most.");
				}
			}
			catch (EncoderFallbackException encoderFallbackException)
			{
				throw new ArgumentException("The name of the phoneme can only contain the ASCII characters.",
											encoderFallbackException);
			}
			Contract.EndContractBlock();

			byte* bytes = stackalloc byte[5];
			fixed (char* chars = phonemeName)
			{
				Encoding.ASCII.GetBytes(chars, phonemeName.Length, bytes, 4);
			}

			bytes[phonemeName.Length] = 0;

			return FindPhonemeByName(bytes);
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="asciiName">
		/// Sequence of ASCII characters that represents the name of the phoneme to find.
		/// </param>
		/// <returns>Zero-based index of the phoneme, if found. -1 otherwise(?).</returns>
		public static int IndexOf(Bytes4 asciiName)
		{
			byte* bytes = stackalloc byte[5];
			*(uint*)bytes = asciiName.UnsignedInt;
			bytes[4] = 0;
			return FindPhonemeByName(bytes);
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="phonemeName">
		/// A sequence of 4 (at most) ASCII symbols that represent the name of the phoneme to look for.
		/// </param>
		/// <returns>A valid object if found.</returns>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme must be a sequence of 4 ASCII characters at most.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme can only contain the ASCII characters.
		/// </exception>
		public static PhonemeInfo Find(string phonemeName)
		{
			PhonemeInfo info;
			return GetPhonemeInfo(IndexOf(phonemeName), out info)
				? info
				: new PhonemeInfo();
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="asciiName">
		/// Sequence of ASCII characters that represents the name of the phoneme to find.
		/// </param>
		/// <returns>A valid object if found.</returns>
		public static PhonemeInfo Find(Bytes4 asciiName)
		{
			PhonemeInfo info;
			return GetPhonemeInfo(IndexOf(asciiName), out info)
				? info
				: new PhonemeInfo();
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="phonemeName">
		/// A sequence of 4 (at most) ASCII symbols that represent the name of the phoneme to look for.
		/// </param>
		/// <param name="phoneme">    Information about found phoneme, if successful.</param>
		/// <returns>Indication of success.</returns>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme must be a sequence of 4 ASCII characters at most.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme can only contain the ASCII characters.
		/// </exception>
		public static bool Find(string phonemeName, out PhonemeInfo phoneme)
		{
			return GetPhonemeInfo(IndexOf(phonemeName), out phoneme);
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="asciiName">
		/// Sequence of ASCII characters that represents the name of the phoneme to find.
		/// </param>
		/// <param name="phoneme">  Information about found phoneme, if successful.</param>
		/// <returns>Indication of success.</returns>
		public static bool Find(Bytes4 asciiName, out PhonemeInfo phoneme)
		{
			return GetPhonemeInfo(IndexOf(asciiName), out phoneme);
		}
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPhonemeCount();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPhonemeInfo(int nIndex, out PhonemeInfo phoneme);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int FindPhonemeByName(byte* sPhonemeName);
		#endregion
	}
}