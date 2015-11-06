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
	public unsafe struct PhonemeLibrary
	{
		#region Fields
		private readonly IntPtr handle;
		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether this instance is usable.
		/// </summary>
		public bool IsValid
		{
			get { return this.handle != IntPtr.Zero; }
		}

		/// <summary>
		/// Gets the number of phonemes in this library.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				return GetPhonemeCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the object that encapsulates information about a phoneme.
		/// </summary>
		/// <param name="index">Zero-based index of the phoneme.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="IndexOutOfRangeException">Index of the phoneme was out of range.</exception>
		public PhonemeInfo this[int index]
		{
			get
			{
				this.AssertInstance();
				Contract.EndContractBlock();

				PhonemeInfo info;
				if (GetPhonemeInfo(this.handle, index, out info))
				{
					return info;
				}
				throw new IndexOutOfRangeException("Index of the phoneme was out of range.");
			}
		}
		#endregion
		#region Construction
		internal PhonemeLibrary(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="phonemeName">
		/// A sequence of 4 (at most) ASCII symbols that represent the name of the phoneme to look for.
		/// </param>
		/// <returns>Zero-based index of the phoneme, if found. -1 otherwise(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme must be a sequence of 4 ASCII characters at most.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme can only contain the ASCII characters.
		/// </exception>
		[SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional", Justification = "Reviewed")]
		public int IndexOf(string phonemeName)
		{
			this.AssertInstance();
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

			return FindPhonemeByName(this.handle, bytes);
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="asciiName">
		/// Sequence of ASCII characters that represents the name of the phoneme to find.
		/// </param>
		/// <returns>Zero-based index of the phoneme, if found. -1 otherwise(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int IndexOf(Bytes4 asciiName)
		{
			this.AssertInstance();
			Contract.EndContractBlock();

			byte* bytes = stackalloc byte[5];
			*(uint*)bytes = asciiName.UnsignedInt;
			bytes[4] = 0;
			return FindPhonemeByName(this.handle, bytes);
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="phonemeName">
		/// A sequence of 4 (at most) ASCII symbols that represent the name of the phoneme to look for.
		/// </param>
		/// <returns>A valid object if found.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme must be a sequence of 4 ASCII characters at most.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme can only contain the ASCII characters.
		/// </exception>
		public PhonemeInfo Find(string phonemeName)
		{
			PhonemeInfo info;
			return GetPhonemeInfo(this.handle, this.IndexOf(phonemeName), out info)
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
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public PhonemeInfo Find(Bytes4 asciiName)
		{
			PhonemeInfo info;
			return GetPhonemeInfo(this.handle, this.IndexOf(asciiName), out info)
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
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme must be a sequence of 4 ASCII characters at most.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The name of the phoneme can only contain the ASCII characters.
		/// </exception>
		public bool Find(string phonemeName, out PhonemeInfo phoneme)
		{
			return GetPhonemeInfo(this.handle, this.IndexOf(phonemeName), out phoneme);
		}
		/// <summary>
		/// Looks up a phoneme in this library.
		/// </summary>
		/// <param name="asciiName">
		/// Sequence of ASCII characters that represents the name of the phoneme to find.
		/// </param>
		/// <param name="phoneme">  Information about found phoneme, if successful.</param>
		/// <returns>Indication of success.</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public bool Find(Bytes4 asciiName, out PhonemeInfo phoneme)
		{
			return GetPhonemeInfo(this.handle, this.IndexOf(asciiName), out phoneme);
		}
		#endregion
		#region Utilities
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		private void AssertInstance()
		{
			if (!this.IsValid)
			{
				throw new NullReferenceException("This instance is not valid.");
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetPhonemeCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetPhonemeInfo(IntPtr handle, int nIndex, out PhonemeInfo phoneme);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int FindPhonemeByName(IntPtr handle, byte* sPhonemeName);
		#endregion
	}
}