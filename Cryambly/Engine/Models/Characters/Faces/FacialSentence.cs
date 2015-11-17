using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using CryCil.Annotations;
using CryCil.MemoryMapping;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Represents a collection of phonemes in the <see cref="FacialSentence"/>.
	/// </summary>
	public struct FacialSentencePhonemes
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
		/// Gets the number of phonemes in the sentence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialSentence.GetPhonemeCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the phoneme in the sentence.
		/// </summary>
		/// <param name="index">Zero-based index of the phoneme.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialSentence.Phoneme this[int index]
		{
			get
			{
				this.AssertInstance();

				FacialSentence.Phoneme phoneme;
				FacialSentence.GetPhoneme(this.handle, index, out phoneme);
				return phoneme;
			}
		}
		#endregion
		#region Construction
		internal FacialSentencePhonemes(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds a new phoneme to this sentence.
		/// </summary>
		/// <param name="phoneme">A new phoneme to add.</param>
		/// <returns>Index of the added phoneme in the sentence(?).</returns>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Add(FacialSentence.Phoneme phoneme)
		{
			this.AssertInstance();

			return FacialSentence.AddPhoneme(this.handle, ref phoneme);
		}
		/// <summary>
		/// Clears this sentence of all phonemes.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Clear()
		{
			this.AssertInstance();

			FacialSentence.ClearAllPhonemes(this.handle);
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
		#endregion
	}
	/// <summary>
	/// Represents a collection of words in the <see cref="FacialSentence"/>.
	/// </summary>
	public struct FacialSentenceWords
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
		/// Gets the number of words in the sentence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public int Count
		{
			get
			{
				this.AssertInstance();

				return FacialSentence.GetWordCount(this.handle);
			}
		}
		/// <summary>
		/// Gets the word in the sentence.
		/// </summary>
		/// <param name="index">Zero-based index of the word.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public FacialSentence.Word this[int index]
		{
			get
			{
				this.AssertInstance();

				FacialSentence.Word word;
				FacialSentence.GetWord(this.handle, index, out word);
				return word;
			}
		}
		#endregion
		#region Construction
		internal FacialSentenceWords(IntPtr handle)
		{
			this.handle = handle;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds a new word to this sentence.
		/// </summary>
		/// <param name="word">A new word to add.</param>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Add(FacialSentence.Word word)
		{
			this.AssertInstance();

			FacialSentence.AddWord(this.handle, ref word);
		}
		/// <summary>
		/// Clears this sentence of all words.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public void Clear()
		{
			this.AssertInstance();

			FacialSentence.ClearAllWords(this.handle);
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
		#endregion
	}
	/// <summary>
	/// Represents a sequence of phonemes and other lip-syncing data.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public struct FacialSentence
	{
		#region Nested Types
		/// <summary>
		/// Encapsulates information about a phoneme within a sentence.
		/// </summary>
		public unsafe struct Phoneme
		{
			#region Fields
			private Bytes4 phoneme;
			private int time;
			private int endtime;
			private float intensity;
			#endregion
			#region Properties
			/// <summary>
			/// Gets or sets a sequence of ASCII characters that represent the name of this phoneme.
			/// </summary>
			public Bytes4 AsciiName
			{
				get { return this.phoneme; }
				set { this.phoneme = value; }
			}
			/// <summary>
			/// Gets or sets the name of the phoneme that is a sequence of 4 (at most) ASCII characters.
			/// </summary>
			/// <exception cref="ArgumentException">
			/// The name of the phoneme can only include ASCII characters.
			/// </exception>
			/// <exception cref="ArgumentException">
			/// The name of the phoneme can only be represented by a sequence of 4 ASCII characters at
			/// most.
			/// </exception>
			public string Name
			{
				get
				{
					char* chars = stackalloc char[4];
					uint phoneme = this.phoneme.UnsignedInt;
					byte* bytes = (byte*)&phoneme;
					int charCount = Encoding.ASCII.GetCharCount(bytes, 4);

					Encoding.ASCII.GetChars(bytes, charCount, chars, charCount);
					return new string(chars, 0, charCount);
				}
				set
				{
					int byteCount;
					try
					{
						byteCount = Encoding.ASCII.GetByteCount(value);
					}
					catch (EncoderFallbackException encoderFallbackException)
					{
						throw new ArgumentException("The name of the phoneme can only include ASCII characters.", encoderFallbackException);
					}
					if (byteCount > 4)
					{
						throw new ArgumentException(
							"The name of the phoneme can only be represented by a sequence of 4 ASCII characters at most.");
					}

					byte* bytes = stackalloc byte[4];
					fixed (char* chars = value)
					{
						Encoding.ASCII.GetBytes(chars, value.Length, bytes, byteCount);
					}
				}
			}
			/// <summary>
			/// Gets or sets the time in milliseconds when this phoneme starts in the sentence.
			/// </summary>
			public int StartTime
			{
				get { return this.time; }
				set { this.time = value; }
			}
			/// <summary>
			/// Gets or sets the time in milliseconds when this phoneme ends in the sentence.
			/// </summary>
			public int EndTime
			{
				get { return this.endtime; }
				set { this.endtime = value; }
			}
			/// <summary>
			/// Gets or sets the phoneme intensity: a value that specifies its expressive strength.
			/// </summary>
			public float Intensity
			{
				get { return this.intensity; }
				set { this.intensity = value; }
			}
			#endregion
		}
		/// <summary>
		/// Encapsulates information about a word in the sentence.
		/// </summary>
		public struct Word
		{
			#region Fields
			private IntPtr sWord; // Word text
			private int startTime; // Start time of the word in milliseconds.
			private int endTime; // End time of the word in milliseconds.
			#endregion
			#region Properties
			/// <summary>
			/// Gets or sets the text representation of the word. Don't use it too often with the same
			/// object, since it creates memory leaks.
			/// </summary>
			/// <exception cref="OutOfMemoryException">There is insufficient memory available.</exception>
			/// <exception cref="ArgumentOutOfRangeException">
			/// The <paramref name="value"/> parameter exceeds the maximum length allowed by the operating
			/// system.
			/// </exception>
			[CanBeNull]
			public string Text
			{
				get { return Marshal.PtrToStringAnsi(this.sWord); }
				set { this.sWord = Marshal.StringToHGlobalAnsi(value); }
			}
			/// <summary>
			/// Gets or sets the time in milliseconds when this word starts in the sentence.
			/// </summary>
			public int StartTime
			{
				get { return this.startTime; }
				set { this.startTime = value; }
			}
			/// <summary>
			/// Gets or sets the time in milliseconds when this word ends in the sentence.
			/// </summary>
			public int EndTime
			{
				get { return this.endTime; }
				set { this.endTime = value; }
			}
			#endregion
		}
		#endregion
		#region Fields
		[FieldOffset(0)] private readonly IntPtr handle;
		/// <summary>
		/// Provides access to the list of phonemes in this sentence.
		/// </summary>
		[FieldOffset(0)] public readonly FacialSentencePhonemes Phonemes;
		/// <summary>
		/// Provides access to the list of words in this sentence.
		/// </summary>
		[FieldOffset(0)] public readonly FacialSentenceWords Words;
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
		/// Gets or sets the text in this sentence.
		/// </summary>
		/// <exception cref="NullReferenceException">This instance is not valid.</exception>
		public string Text
		{
			get
			{
				this.AssertInstance();

				return GetText(this.handle);
			}
			set
			{
				this.AssertInstance();

				SetText(this.handle, value);
			}
		}
		#endregion
		#region Construction
		internal FacialSentence(IntPtr handle)
			: this()
		{
			this.handle = handle;
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
		private static extern void SetText(IntPtr handle, string text);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetText(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearAllPhonemes(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPhonemeCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetPhoneme(IntPtr handle, int index, out Phoneme ph);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int AddPhoneme(IntPtr handle, ref Phoneme ph);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void ClearAllWords(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetWordCount(IntPtr handle);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetWord(IntPtr handle, int index, out Word wrd);
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void AddWord(IntPtr handle, ref Word wrd);
		#endregion
	}
}