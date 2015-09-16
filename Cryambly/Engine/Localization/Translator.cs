using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using CryCil.Utilities;

namespace CryCil.Engine.Localization
{
	/// <summary>
	/// Provides access to CryEngine localization API.
	/// </summary>
	public static class Translator
	{
		#region Fields
		/// <summary>
		/// This is an array where values from <see cref="SupportedLanguages"/> enumeration are mapped to
		/// names of languages that are supported by CryEngine localization API.
		/// </summary>
		public static readonly string[] Languages;
		private static readonly IntPtr[] langPtrs;
		private static readonly string[] cultureMap;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets current language that is used by CryEngine localization API.
		/// </summary>
		public static SupportedLanguages CurrentLanguage
		{
			get { return (SupportedLanguages)Array.IndexOf(Languages, GetCurrentLanguage()); }
			set { SetCurrentLanguage(langPtrs[(int)value]); }
		}
		/// <summary>
		/// Gets or sets the language that is used by CryEngine localization API using .Net/Mono
		/// <see cref="CultureInfo"/> objects.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// Given culture is not supported by CryEngine localization API.
		/// </exception>
		public static CultureInfo CurrentCulture
		{
			get { return CultureInfo.GetCultureInfo(cultureMap[(int)CurrentLanguage]); }
			set
			{
				// Try specific name.
				int index = Array.IndexOf(cultureMap, value.Name);
				if (index < 0)
				{
					// Try a parent culture.
					index = Array.IndexOf(cultureMap, value.Parent.Name);
				}
				if (index < 0)
				{
					// Try more general name.
					index = Array.IndexOf(cultureMap, value.TwoLetterISOLanguageName);
				}
				if (index < 0)
				{
					throw new NotSupportedException
						(
						string.Format
							(
							 "Culture with name \"{0}\" is not supported by CryEngine localization API.",
							 value.Name
							)
						);
				}

				SetCurrentLanguage(langPtrs[index]);
			}
		}
		#endregion
		#region Events
		#endregion
		#region Construction
		static Translator()
		{
			// Cache the language names.
			const int langCount = (int)SupportedLanguages.SupportedLanguageCount;
			Languages = new string[langCount];
			langPtrs = new IntPtr[langCount];

			for (int i = 0; i < langCount; i++)
			{
				string name = GetLanguageNameInternal((SupportedLanguages)i);
				Languages[i] = name;
				langPtrs[i] = StringPool.Get(name); // Cache native representation of the name.
			}

			cultureMap = new string[langCount];
			cultureMap[(int)SupportedLanguages.Arabic] = "ar";
			cultureMap[(int)SupportedLanguages.ChineseSimplified] = "zh-CHS";
			cultureMap[(int)SupportedLanguages.ChineseTraditional] = "zh-CHT";
			cultureMap[(int)SupportedLanguages.Czech] = "cs-CZ";
			cultureMap[(int)SupportedLanguages.Danish] = "da-DK";
			cultureMap[(int)SupportedLanguages.Dutch] = "nl";
			cultureMap[(int)SupportedLanguages.English] = "en";
			cultureMap[(int)SupportedLanguages.Finnish] = "fi-FI";
			cultureMap[(int)SupportedLanguages.French] = "fr";
			cultureMap[(int)SupportedLanguages.German] = "de";
			cultureMap[(int)SupportedLanguages.Italian] = "it";
			cultureMap[(int)SupportedLanguages.Japanese] = "ja-JP";
			cultureMap[(int)SupportedLanguages.Korean] = "ko-KR";
			cultureMap[(int)SupportedLanguages.Norwegian] = "nb-NO";
			cultureMap[(int)SupportedLanguages.Polish] = "pl-PL";
			cultureMap[(int)SupportedLanguages.Portuguese] = "pt";
			cultureMap[(int)SupportedLanguages.Russian] = "ru-RU";
			cultureMap[(int)SupportedLanguages.Spanish] = "es";
			cultureMap[(int)SupportedLanguages.Swedish] = "sv";
			cultureMap[(int)SupportedLanguages.Turkish] = "tr-TR";
		}
		#endregion
		#region Interface
		/// <summary>
		/// Processes given string and replaces any labels with there translated versions.
		/// </summary>
		/// <param name="text">        Text to translate.</param>
		/// <param name="forceEnglish">If true, forces all translations to be English.</param>
		/// <returns>Translated text or null, if not successful.</returns>
		/// <exception cref="ArgumentNullException">Text for translation cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string Translate(string text, bool forceEnglish);
		/// <summary>
		/// Returns translated version of this label.
		/// </summary>
		/// <param name="labelName">   Label to translate.</param>
		/// <param name="forceEnglish">If true, forces translation to be English.</param>
		/// <returns>Translated text or null, if not successful.</returns>
		/// <exception cref="ArgumentNullException">Localization label cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string TranslateLabel(string labelName, bool forceEnglish);
		/// <summary>
		/// Loads localization data from Excel Xml spreadsheet.
		/// </summary>
		/// <param name="fileName">Name of the file that contains the spreadsheet.</param>
		/// <param name="reload">  True, if the data was loaded from that file before.</param>
		/// <returns>True, if successful otherwise false.</returns>
		/// <exception cref="ArgumentNullException">Name of the file to load cannot be null.</exception>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool LoadXml(string fileName, bool reload);
		/// <summary>
		/// Reloads localization data.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReloadData();
		/// <summary>
		/// Releases localization data.
		/// </summary>
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReleaseData();
		#endregion
		#region Utilities
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetLanguageNameInternal(SupportedLanguages language);
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string GetCurrentLanguage();
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetCurrentLanguage(IntPtr name);
		#endregion
	}
}