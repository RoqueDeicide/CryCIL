#pragma once

// Stop compiler from complaining about strncpy.
#pragma warning(disable:4996)

#include "DocumentationMarkers.h"

#ifdef USE_CRYCIL_API

#include "IMonoInterface.h"

#endif // USE_CRYCIL_API

#ifdef MONO_API

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

#endif // MONO_API

#ifdef MONO_API

typedef MonoString *mono_string;

#elif defined(USE_CRYCIL_API)

typedef mono::string mono_string;

#else

typedef void *mono_string;

#endif // MONO_API


#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define FatalError(message) CryFatalError(message)

#else
#include <iostream>
#include <string>
#include <sstream>
#include <vector>
#include <stdexcept>
#define FatalError(message) throw std::logic_error(message)

#endif // CRYCIL_MODULE

#ifdef ENABLE_NTTEXT_DEBUG_REPORT

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#define DebugReport(message) CryLogAlways(message);

#else

#define DebugReport(message) std::cout << message << std::endl;

#endif // CRYCIL_MODULE

#else

#define DebugReport(message)

#endif // ENABLE_NTTEXT_DEBUG_REPORT

#include "List.h"

//! Wraps a null-terminated string.
//!
//! This wrapper will automatically delete memory associated with the wrapper pointer, so set it to null,
//! if you want to keep the memory from deletion.
//!
//! This wrapper can be converted to const char * pointer implicitly.
//!
//! Check documentation for this object's assignment operator overload.
//!
//! @typeparam SymbolType Type that represents symbols that comprise this string.
template<typename SymbolType>
struct NtTextTemplate
{
private:
	const SymbolType *chars;
public:
	//! Creates a default text.
	//!
	//! Only default text can be assigned to.
	NtTextTemplate()
	{
		DebugReport("Default constructor for NtText has been invoked.");

		this->chars = nullptr;
	}
	//! Assigns contents of the temporary object to the new one.
	NtTextTemplate(NtTextTemplate &&other)
		: chars(other.chars)
	{
		DebugReport("Move constructor for NtText has been invoked.");

		other.chars = nullptr;
	}
	//! Creates a new null-terminated string from given null-terminated one.
	//!
	//! @param t Text to initialize this object with.
	NtTextTemplate(const SymbolType *t)
	{
		DebugReport("Wrapping constructor for NtText has been invoked.");

		this->chars = t;
	}
	//! Creates a new null-terminated string from given null-terminated one.
	//!
	//! @param t         Text to initialize this object with.
	//! @param duplicate Indicates whether given string should be duplicated.
	NtTextTemplate(const SymbolType *t, bool duplicate)
	{
		DebugReport("Wrapping constructor with optional duplication for NtText has been invoked.");

		if (!duplicate)
		{
			this->chars = t;
		}
		else
		{
			int count = strlen(t);

			SymbolType *buffer = new SymbolType[count + 1];
			buffer[count] = '\0';

			for (int i = 0; i < count; i++)
			{
				buffer[i] = t[i];
			}

			this->chars = const_cast<const SymbolType *>(buffer);
		}
	}
	//! Creates a new null-terminated string from given .Net/Mono string.
	//!
	//! @param managedString Instance of type System.String.
	NtTextTemplate(mono_string managedString)
	{
#ifdef MONO_API
		MonoError error;
		char *ntText = mono_string_to_utf8_checked(managedString, &error);
		if (mono_error_ok(&error))
		{
			int length = _strlen(ntText);
			this->chars = new SymbolType[length + 1];
			for (int i = 0; i < length; i++)
			{
				((SymbolType *)this->chars)[i] = ntText[i];
			}
			mono_free(ntText);
			((SymbolType *)this->chars)[length] = '\0';
		}
		else
		{
			FatalError(mono_error_get_message(&error));
		}
#elif defined(USE_CRYCIL_API)
		this->chars = _str_mono(managedString);
#else
		this->chars = nullptr;
#endif // MONO_API
	}
#ifdef CRYCIL_MODULE
	//! Creates a new null-terminated string from given .Net/Mono string.
	//!
	//! @param managedString Instance of type System.String.
	NtTextTemplate(mono::string managedString)
	{
		this->chars = ToNativeString(managedString);
	}
#endif // CRYCIL_MODULE

	//! Constructs a text out of given parts.
	//!
	//! @param t1 Number of arguments in the chain.
	NtTextTemplate(int count...)
	{
		// Gather the arguments into the list and calculate total length at the same time.
		va_list va;
		va_start(va, count);

		auto parts = List<const SymbolType *>(count);

		int length = 0;
		for (int i = 0; i < count; i++)
		{
			const char *next = va_arg(va, const char *);

			if (next)
			{
				length += _strlen(next);
				parts.Add(next);
			}
		}

		va_end(va);

		this->chars = new SymbolType[length + 1];
		// Copy the characters to this string.
		int j = 0;
		for (int i = 0; i < parts.Length; i++)
		{
			for (int k = 0; parts[i][k]; k++)
			{
				((SymbolType *)this->chars)[j++] = parts[i][k];
			}
		}
		((SymbolType *)this->chars)[length] = '\0';
	}
	virtual ~NtTextTemplate()
	{
		DebugReport("Destructor has been invoked.");

		if (this->chars)
		{
			DebugReport("Releasing characters.");

			delete this->chars;
			this->chars = nullptr;
		}
	}
	//! Swaps pointers to null-terminated strings of this and another object.
	//!
	//! @param another A reference to another NtTextTemplate object that will manage a pointer of this object.
	SWAP_ASSIGNMENT NtTextTemplate &operator=(NtTextTemplate &another)
	{
		if (this->chars != another.chars)
		{
			const SymbolType *ptr = this->chars;
			this->chars = another.chars;
			another.chars = ptr;
		}
		return *this;
	}
	//! Assigns another null-terminated string to this one.
	//!
	//! String that was wrapped by this object previously will be released, if it is not the same string as given one.
	NtTextTemplate &operator=(const SymbolType *chars)
	{
		if (this->chars != chars)
		{
			this->~NtTextTemplate();
		}
		this->chars = chars;
	}
	//! Creates a deep copy of this text.
	//!
	//! @returns A heap-allocated text that is a duplicate of this one. Wrapping it in NtText object is recommended
	//!          to guarantee automatic release of the memory.
	const SymbolType *Duplicate()
	{
		if (!this->chars)
		{
			return nullptr;
		}
		int length = _strlen(this->chars);
		SymbolType *ntText = new SymbolType[count + 1];
		ntText[count] = '\0';
		for (int i = 0; i < count; i++)
		{
			ntText[i] = this->chars[i];
		}
		return const_cast<const SymbolType *>(ntText);
	}
	//! Creates a null-terminated string from substring of this text.
	//!
	//! Created string is allocated within the heap and needs to be deleted after use.
	//!
	//! @param index Zero-based index of the first symbol of the substring to copy to
	//!              null-terminated result.
	//! @param count Number of characters to copy. Length of the resultant string will
	//!              be count + 1 to accommodate the null character.
	const SymbolType *Substring(int index, int count) const
	{
		if (!this->chars)
		{
			return nullptr;
		}
		int length = _strlen(this->chars);
		if (index + count > length)
		{
			FatalError("Attempt to copy too many characters from the string.");
		}
		SymbolType *ntText = new SymbolType[count + 1];
		ntText[count] = '\0';
		for
			(
			int i = index, j = 0, counter = 0;
		counter < count;					// Counter tells us where to stop.
		i++, j++, counter++					//
			)
		{
			ntText[j] = this->chars[i];
		}
		return const_cast<const SymbolType *>(ntText);
	}
	//! Copies characters from beginning of this text to the given character array.
	//!
	//! @param destination Pointer to the beginning of the destination array.
	//! @param index       Zero-based index of the first symbol within destination array
	//!                    to copy text to.
	//! @param symbolCount Number of characters to copy.
	void CopyTo(SymbolType *destination, int index, int symbolCount)
	{
		this->CopyTo(destination, 0, index, symbolCount);
	}
	//! Copies characters from substring of this text to the given character array.
	//!
	//! @param destination      Pointer to the beginning of the destination array.
	//! @param sourceIndex      Zero-based index of the first symbol of this text to copy.
	//! @param destinationIndex Zero-based index of the first symbol within destination array
	//!                         to copy text to.
	//! @param charCount        Number of characters to copy.
	void CopyTo(SymbolType *destination, int sourceIndex, int destinationIndex, int charCount)
	{
		int length = _strlen(this->chars);
		if (sourceIndex + charCount > length)
		{
			FatalError("Attempt to copy too many characters from the string.");
		}
		for
			(
			int i = sourceIndex, j = destinationIndex, counter = 0;
		counter < charCount;						// Counter tells us where to stop.
		i++, j++, counter++							//
			)
		{
			destination[j] = this->chars[i];
		}
	}
	//! Creates a managed string from this one.
	mono_string ToManagedString() const
	{
		return _mono_str(this->chars);
	}
	//! Splits this string into parts separated by the given symbol.
	//!
	//! @param symbol           Symbol that will separate the parts.
	//! @param removeEmptyParts Indicates whether empty parts should be present in the result.
	//!
	//! @return A list of parts. Must be deleted after use. Before that though, each part
	//!         must be deleted separately.
	List<const SymbolType *> *Split(SymbolType symbol, bool removeEmptyParts)
	{
		int length = _strlen(this->chars);
		List<const char *> *parts = new List<const char *>(6);

		int partStartIndex = 0;
		for (int i = 0; i <= length; i++)
		{
			if (this->chars[i] == symbol || i == length)
			{
				if (i != partStartIndex || !removeEmptyParts)
				{
					parts->Add(this->Substring(partStartIndex, i - partStartIndex));
				}
				partStartIndex = i + 1;
			}
		}

		return parts;
	}
	//! Finds first occurrence of the given symbol.
	//!
	//! @param symbol Character to find.
	//!
	//! @return Zero-based index of the first occurrence of the given symbol.
	//!         -1 is returned if symbol was not found.
	int IndexOf(SymbolType symbol)
	{
		for (int i = 0; this->chars[i]; i++)
		{
			if (this->chars[i] == symbol)
			{
				return i;
			}
		}
		return -1;
	}
	//! Finds last occurrence of the given symbol.
	//!
	//! @param symbol Character to find.
	//!
	//! @return Zero-based index of the last occurrence of the given symbol.
	//!         -1 is returned if symbol was not found.
	int LastIndexOf(SymbolType symbol)
	{
		int length = _strlen(this->chars);
		for (int i = length - 2; i >= 0; i--)
		{
			if (this->chars[i] == symbol)
			{
				return i;
			}
		}
		return -1;
	}
	//! Determines whether this string contains a substring.
	bool Contains(const SymbolType *subString, bool ignoreCase = false)
	{
		return _contains_substring(this->chars, subString, ignoreCase);
	}
	//! Gets the length of the string.
	__declspec(property(get = GetLength)) int Length;
	//! Gets the length of the string.
	int GetLength() const
	{
		return _strlen(this->chars);
	}
	//! Determines whether this text is equal to one in the null-terminated string.
	bool Equals(const SymbolType *ntString)
	{
		return _compare(this->chars, ntString) == 0;
	}
	//! Compares this object to another.
	int CompareTo(NtTextTemplate &other)
	{
		return _compare(this->chars, other.chars);
	}
	//! Compares this object to another.
	int CompareTo(const SymbolType *other)
	{
		return _compare(this->chars, other);
	}
	//! Detaches the pointer wrapped by this object from it.
	//!
	//! @returns A pointer to a null-terminated string of characters previously wrapped by this object.
	const SymbolType *Detach()
	{
		const SymbolType *ptr = this->chars;
		this->chars = nullptr;
		return ptr;
	}
	//! Implicit conversion. Returns wrapped pointer.
	operator const SymbolType*() const
	{
		return this->chars;
	}

	//! Counts number of characters in the string.
	static int _strlen(const SymbolType *str);
	//! Compares 2 strings.
	static int _compare(const SymbolType *str1, const SymbolType *str2);
	//! To managed string.
	static mono_string _mono_str(const SymbolType *str);
	//! From managed string.
	static const SymbolType *_str_mono(mono_string str);
	//! Checks for the presence of the substring.
	static bool _contains_substring(const SymbolType *str0, const SymbolType *str1, bool ignoreCase);
};

template<typename SymbolType>
inline mono_string NtTextTemplate<SymbolType>::_mono_str(const SymbolType *str)
{
#ifdef MONO_API
	return mono_string_new(mono_domain_get(), str);
#elif defined(USE_CRYCIL_API)
	return ToMonoString(str);
#else
	return nullptr;
#endif // MONO_API
}

template<typename SymbolType>
inline const SymbolType *NtTextTemplate<SymbolType>::_str_mono(mono_string str)
{
	if (!str)
	{
		return nullptr;
	}
#ifdef MONO_API
	MonoError error;
	char *ntText = mono_string_to_utf8_checked((MonoString *)str, &error);
	if (mono_error_ok(&error))
	{
		int length = _strlen(ntText);
		char *chars = new char[length + 1];
		for (int i = 0; i < length; i++)
		{
			chars[i] = ntText[i];
		}
		mono_free(ntText);
		chars[length] = '\0';
		return chars;
	}
	else
	{
		FatalError(mono_error_get_message(&error));
		return nullptr;
	}
#elif defined(USE_CRYCIL_API)
	return ToNativeString(managedString);
#else
	return nullptr;
#endif // MONO_API
}

template<typename SymbolType>
inline int NtTextTemplate<SymbolType>::_compare(const SymbolType *str1, const SymbolType *str2)
{
	return strcmp(str1, str2);
}

template<typename SymbolType>
inline int NtTextTemplate<SymbolType>::_strlen(const SymbolType *str)
{
	return strlen(str);
}

template<typename SymbolType>
inline bool NtTextTemplate<SymbolType>::_contains_substring(const SymbolType *str0, const SymbolType *str1, bool ignoreCase)
{
	if (!ignoreCase)
	{
		return strstr(str0, str1) != nullptr;
	}

	int nSuperstringLength = (int)strlen(str0);
	int nSubstringLength = (int)strlen(str1);

	for (int nSubstringPos = 0; nSubstringPos <= nSuperstringLength - nSubstringLength; ++nSubstringPos)
	{
		if (strnicmp(str0 + nSubstringPos, str1, nSubstringLength) == 0)
			return (str0 + nSubstringPos) != nullptr;
	}
	return false;
}

template<>
inline mono_string NtTextTemplate<wchar_t>::_mono_str(const wchar_t *str)
{
#ifdef MONO_API
	return mono_string_from_utf16((mono_unichar2 *)str);
#elif defined(USE_CRYCIL_API)
	return MonoEnv->Objects->Texts->ToManaged(str);
#else
	return nullptr;
#endif // MONO_API
}

template<>
inline const wchar_t *NtTextTemplate<wchar_t>::_str_mono(mono_string str)
{
#ifdef MONO_API
	wchar_t *ntText = (wchar_t *)mono_string_to_utf16(str);
	int length = wcslen(ntText);
	wchar_t *chars = new wchar_t[length + 1];
	for (int i = 0; i < length; i++)
	{
		chars[i] = ntText[i];
	}
	mono_free(ntText);
	chars[length] = '\0';
	return chars;
#elif defined(USE_CRYCIL_API)
	return MonoEnv->Objects->Texts->ToNative16(managedString);
#else
	return nullptr;
#endif // MONO_API
}

template<>
inline int NtTextTemplate<wchar_t>::_compare(const wchar_t *str1, const wchar_t *str2)
{
	return wcscmp(str1, str2);
}

template<>
inline int NtTextTemplate<wchar_t>::_strlen(const wchar_t *str)
{
	return wcslen(str);
}

template<>
inline bool NtTextTemplate<wchar_t>::_contains_substring(const wchar_t *str0, const wchar_t *str1, bool ignoreCase)
{
	if (!ignoreCase)
	{
		return wcsstr(str0, str1) != nullptr;
	}

	int superstringLength = (int)wcslen(str0);
	int substringLength = (int)wcslen(str1);

	for (int substringPos = 0; substringPos <= superstringLength - substringLength; ++substringPos)
	{
		if (wcsnicmp(str0 + substringPos, str1, substringLength) == 0)
			return (str0 + substringPos) != nullptr;
	}
	return false;
}

typedef NtTextTemplate<char> NtText;
typedef NtTextTemplate<wchar_t> NtText16;