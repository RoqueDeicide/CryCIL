using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CryCil.MemoryMapping;
using CryCil.Utilities;

namespace CryCil.Engine.Models.Characters.Faces
{
	/// <summary>
	/// Encapsulates details about the phoneme. Used in Lip-Syncing.
	/// </summary>
	public unsafe struct PhonemeInfo
	{
		#region Fields
		private readonly char ipa;
		private readonly Bytes4 asciiName;
		private readonly byte* description;
		#endregion
		#region Properties
		/// <summary>
		/// Gets the IPA code of this phoneme.
		/// </summary>
		public char IPACode
		{
			get { return this.ipa; }
		}
		/// <summary>
		/// Gets the sequence of ASCII characters that represent the name.
		/// </summary>
		public Bytes4 AsciiName
		{
			get { return this.asciiName; }
		}
		/// <summary>
		/// Gets the name of this phoneme.
		/// </summary>
		[SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional", Justification = "Reviewed")]
		public string Name
		{
			get
			{
				char* chars = stackalloc char[4];
				uint bytes = this.asciiName.UnsignedInt;
				Encoding.ASCII.GetChars((byte*)&bytes, 4, chars, 4);
				return new string(chars, 0, 4);
			}
		}
		/// <summary>
		/// Gets the description of this phoneme.
		/// </summary>
		public string Description
		{
			get { return CustomMarshaling.GetUtf8String(new IntPtr(this.description)); }
		}
		#endregion
		#region Construction
		internal PhonemeInfo(char ipa, Bytes4 asciiName, byte* description)
		{
			this.ipa = ipa;
			this.asciiName = asciiName;
			this.description = description;
		}
		#endregion
	}
}