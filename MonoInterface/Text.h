#pragma once

//// Stop compiler from complaining about strncpy.
//#pragma warning(disable:4996)

#ifdef USE_CRYCIL_API

#include "IMonoInterface.h"

#endif // USE_CRYCIL_API

#ifdef CRYCIL_MODULE

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

inline void *AllocateText(size_t count)
{
	return CryModuleMalloc(count);
}
inline void FreeText(void *ptr)
{
	CryModuleFree(ptr);
}

#endif // CRYCIL_MODULE

#ifdef CRYCIL_MODULE
#include <ISystem.h>

#else
#include <iostream>
#include <string>
#include <sstream>
#include <vector>
#include <stdexcept>
#include <algorithm>

inline void *AllocateText(size_t count)
{
	return malloc(count);
}

inline void FreeText(void *ptr)
{
	free(ptr);
}

#endif // CRYCIL_MODULE

#ifdef MONO_API

typedef MonoString *mono_string;

#elif defined(USE_CRYCIL_API)

typedef mono::string mono_string;

#else

typedef void *mono_string;

#endif // MONO_API

#include "MemoryTrackingUtilities.h"

//! Represents a reference counted null-terminated header-prefixed string.
//!
//! This type is, basically, a reimplementation of CryString and std::string.
//!
//! When a new object of this type is created, it allocates enough memory to hold: header object with
//! information about the text data, like reference count and length, and null-terminated string of characters
//! itself.
template<typename symbol>
struct TextTemplate
{
protected:
	typedef NullTerminatedArrayHeader<symbol> TextHeader;
	typedef MemoryTracker<TextTemplate> TextMemoryTracker;
#pragma region Fields
	symbol *str;
#pragma endregion
#pragma region Properties
	//! Gets the object that describes this text.
	__declspec(property(get = GetHeader)) TextHeader *Header;
	TextHeader *GetHeader() const
	{
		TextTemplate *_this = const_cast<TextTemplate *>(this);
		TextHeader *header = reinterpret_cast<TextHeader *>(_this->str) - 1;
		return header;
	}
	//! Indicates whether this object shares its text data with others.
	__declspec(property(get = IsShared)) bool Shared;
	bool IsShared() const
	{
		return this->Header->ReferenceCount > 1;
	}
public:
	//! Gets the number of characters in this text.
	__declspec(property(get = GetLength)) size_t Length;
	size_t GetLength() const
	{
		return this->Header->Length;
	}
	//! Gets the number of characters that can fit in the memory block that was allocated for this text.
	__declspec(property(get = GetCapacity)) size_t Capacity;
	size_t GetCapacity() const
	{
		return this->Header->Capacity;
	}
	//! Indicates whether this string is empty.
	__declspec(property(get = IsEmpty)) bool Empty;
	bool IsEmpty() const
	{
		return this->Length == 0;
	}
#pragma endregion

public:
#pragma region Construction
	//! Creates an empty text object.
	TextTemplate()
	{
		this->InitEmpty();
	}
	//! A copy constructor.
	TextTemplate(const TextTemplate &other)
	{
		// Make a shallow copy.
		if (other.Header->referenceCount >= 0)
		{
			this->str = other.str;
			this->Header->IncrementReferenceCount();
		}
		else
		{
			this->InitEmpty();
		}
	}
	//! Creates a new object of this type that is a substring of another one.
	//!
	//! @param other  Reference to the string to get the substring out of.
	//! @param offset Zero-based index of the first character in the sub-string. If this number is greater then
	//!               number of characters in the given characters, then this object remains the same.
	//! @param count  Number of characters in the sub-string. Will be trimmed, if necessary.
	TextTemplate(const TextTemplate &other, size_t offset, size_t count)
	{
		this->InitEmpty();
		this->Assign(other, offset, count);
	}
	//! Creates a new object that encapsulates a deep copy of specified number of characters.
	TextTemplate(const symbol *chars, size_t length)
	{
		this->InitEmpty();

		// Make a deep copy.
		if (length != 0)
		{
			this->AllocateMemory(length);
			CopyInternal(this->str, chars, length);
		}
	}
	//! Creates a new object of this type that encapsulates a deep copy of given null-terminated string.
	TextTemplate(const symbol *chars)
		: TextTemplate(chars, _strlen(chars))
	{
	}
	//! Creates a new object of this type that encapsulates a deep copy of a string delimited by 2 pointers.
	//!
	//! @param begin Pointer to the first character in the string.
	//! @param end   Pointer to the first character after last character in the string.
	TextTemplate(const symbol *begin, const symbol *end)
		: TextTemplate(begin, size_t(end - begin))
	{
	}
	//! Creates a new null-terminated string from given .Net/Mono string.
	//!
	//! @param managedString Instance of type System.String.
	TextTemplate(mono_string managedString)
	{
#if defined(MONO_API) || defined(USE_CRYCIL_API)
		this->InitEmpty();

		MonoError error;
		symbol *ntText = mono_string_native(managedString, &error);

		if (mono_error_ok(&error))
		{
			this->Assign(ntText);

			mono_free(ntText);
		}
		else
		{
			FatalError(mono_error_get_message(&error));
		}
#else
		this->str = nullptr;
#endif // MONO_API
	}
#ifdef CRYCIL_MODULE
	//! Creates a new null-terminated string from given .Net/Mono string.
	//!
	//! @param managedString Instance of type System.String.
	TextTemplate(mono::string managedString);
#endif // CRYCIL_MODULE
	//! Creates a new string for a list of parts.
	//!
	//! @param parts An initializer list that contains parts to build the new string out of.
	TextTemplate(std::initializer_list<const symbol *> parts)
	{
		int totalLength = 0;
		for (auto current = parts.begin(); current < parts.end(); current++)
		{
			totalLength += _strlen(*current);
		}

		if (totalLength == 0)
		{
			this->InitEmpty();
			return;
		}

		this->AllocateMemory(totalLength);
		for (auto current = parts.begin(); current < parts.end(); current++)
		{
			this->Append(*current);
		}
	}
#pragma endregion
private:
#pragma region Construction Utilities
	void MakeUnique()
	{
		if (this->Header->referenceCount <= 0)
		{
			return;
		}

		TextHeader *oldHeader = this->Header;
		this->Release();
		this->AllocateMemory(oldHeader->capacity);
		CopyInternal(this->str, oldHeader->Elements, oldHeader->length + 1);
	}
	static size_t CalculateMemoryToAllocate(size_t characterCapacity)
	{
		return sizeof(TextHeader) + (characterCapacity + 1) * sizeof(symbol);
	}
	//! Initializes an empty string object.
	void InitEmpty()
	{
		// Thanks to this code, no empty objects consume excessive amounts of memory.
		this->str = TextHeader::EmptyHeader()->Elements;
	}
	//! @param capacity Number of characters to fit into allocated memory not counting the terminator.
	void AllocateMemory(size_t capacity)
	{
		assert(capacity >= 0);
		assert(capacity <= INT_MAX - 1);

		if (capacity == 0)
		{
			this->InitEmpty();
		}
		else
		{
			auto byteCount = CalculateMemoryToAllocate(capacity);

			TextHeader *header = static_cast<TextHeader *>(AllocateText(byteCount));

			TextMemoryTracker::AddMemory(byteCount);
			header->ReferenceCount = 1;
			header->Length = capacity;
			header->Capacity = capacity;
			this->str = header->Elements;
			this->str[capacity] = '\0';
		}
	}
#pragma endregion
public:
#pragma region Destruction
	//! Releases this text object.
	~TextTemplate()
	{
		ReleaseData(this->Header);
	}
#pragma endregion
private:
#pragma region Destruction Utilities
	//! Makes this object into an empty one.
	void Release()
	{
		// Release, if not empty.
		if (this->Header->referenceCount >= 0)
		{
			ReleaseData(this->Header);
			this->InitEmpty();
		}
	}
	//! Releases this object's data.
	static void ReleaseData(TextHeader *header)
	{
		if (header->ReferenceCount >= 0)					// Check if empty.
		{
			if (!header->UnregisterReference())			// Check, if has any live references.
			{
				// Release.
				TextMemoryTracker::UnregisterArray(*header);	// For stats.

				FreeText(header);
			}
		}
	}
#pragma endregion
public:
#pragma region Interface
	void Clear()
	{
		if (this->Empty)
		{
			return;
		}
		if (this->Header->referenceCount >= 0)
		{
			this->Release();
		}
		else
		{
			this->Resize(0);
		}
	}
	//! Creates a deep copy of text data that is held by this object.
	//!
	//! @returns A pointer to data that has to be deleted when no longer in use.
	const symbol *Duplicate() const
	{
		if (this->Empty)
		{
			return nullptr;
		}

		size_t charCount = this->Length;
		symbol *dup = new char[charCount + 1];
		CopyInternal(dup, this->str, charCount);
		dup[charCount] = '\0';
		return dup;
	}
#pragma region Assignment
	//! Assigns a deep copy of a sub-string to this object.
	//!
	//! @param other  Reference to the object that contains a sub-string that will be assigned to this object.
	//! @param offset Zero-based index of the first character in the sub-string. If this number is greater then
	//!               number of characters in the given characters, then this object remains the same.
	//! @param count  Number of characters in the sub-string. Will be trimmed, if necessary.
	//!
	//! @returns A reference to this object.
	TextTemplate &Assign(const TextTemplate &other, size_t offset, size_t count)
	{
		int length = other.Length;
		if (offset > length)
		{
			// Assignment impossible.
			return *this;
		}
		if (offset + count > length)
		{
			// Trim the count.
			count = length - offset;
		}

		this->AssignInternal(other.str + offset, count);

		return *this;
	}
	//! Assigns a deep copy of a null-terminated string to this object.
	//!
	//! @param chars Pointer to the null-terminated string to assign to this object.
	//!
	//! @returns A reference to this object.
	TextTemplate &Assign(const symbol *chars)
	{
		if (chars)
		{
			this->AssignInternal(chars, _strlen(chars));
		}

		return *this;
	}
	//! Assigns a string built out of parts to this object.
	//!
	//! @param chars Pointer to the null-terminated string to assign to this object.
	//!
	//! @returns A reference to this object.
	TextTemplate &Assign(std::initializer_list<const symbol *> parts)
	{
		int totalLength = 0;
		for (auto current = parts.begin(); current < parts.end(); current++)
		{
			totalLength += _strlen(*current);
		}

		if (totalLength == 0)
		{
			return *this;
		}

		if (this->Shared || this->Capacity < totalLength)
		{
			this->Release();
			this->AllocateMemory(totalLength);
		}
		
		this->Header->Length = totalLength;
		this->str[totalLength] = '\0';
		size_t length = 0;
		for (auto current = parts.begin(); current < parts.end(); current++)
		{
			size_t currentLength = _strlen(*current);
			CopyInternal(this->str + length, *current, currentLength);
			length += currentLength;
		}

		return *this;
	}
#pragma endregion
private:
#pragma region Assignment Utilities
	void AssignInternal(const symbol *chars, size_t count)
	{
		// Allocate more data, if this string is shared, or doesn't have enough allocated memory.
		if (this->Shared || this->Capacity < count)
		{
			this->Release();
			this->AllocateMemory(count);
		}

		CopyInternal(this->str, chars, count);

		this->Header->Length = count;
		this->str[count] = '\0';
	}
#pragma endregion
public:
#pragma region Size
	//! Resizes this string and pads it with a filling character, if necessary.
	void Resize(size_t newCount, symbol filler = 0)
	{
		this->MakeUnique();
		auto length = this->Length;
		if (newCount > length)
		{
			this->Append(newCount - length, filler);
		}
		else if (newCount < length)
		{
			this->Header->Length = newCount;
			this->str[newCount] = '\0';
		}
	}
	//! Expands the memory block to fit more text, if given new capacity is greater then current one, otherwise
	//! trims the memory block to be exact size to fit current text.
	void Reserve(size_t newCapacity)
	{
		auto oldCapacity = this->Capacity;
		if (newCapacity > oldCapacity)
		{
			TextHeader *oldHeader = this->Header;

			this->AllocateMemory(newCapacity);
			CopyInternal(this->str, oldHeader->Elements, oldHeader->length);
			this->Header->Length = oldHeader->length;
			this->str[oldHeader->length] = '\0';
			ReleaseData(oldHeader);
		}
		else if (oldCapacity != this->Length)
		{
			TextHeader *oldHeader = this->Header;

			this->AllocateMemory(this->Length);
			CopyInternal(this->str, oldHeader->Elements, oldHeader->length);
			ReleaseData(oldHeader);
		}
	}
#pragma endregion
#pragma region Appending
	//! Appends a number of specified characters to the end of this string.
	TextTemplate &Append(size_t count, symbol character)
	{
		if (count <= 0)
		{
			return *this;
		}

		if (this->Shared || this->Length + count > this->Capacity)
		{
			TextHeader *oldHeader = this->Header;
			this->AllocateMemory(this->Length + count);
			CopyInternal(this->str, oldHeader->Elements, oldHeader->length);
			SetInternal(this->str + oldHeader->length, character, count);
			ReleaseData(oldHeader);
		}
		else
		{
			auto oldLength = this->Length;
			SetInternal(this->str + oldLength, character, count);
			this->Header->Length = oldLength + count;
			this->str[this->Length] = '\0';
		}
		
		return *this;
	}
	//! Appends a null-terminated string to the end of this string.
	TextTemplate &Append(const symbol *chars)
	{
		if (chars)
		{
			this->ConcatenateInPlace(chars, _strlen(chars));
		}

		return *this;
	}
#pragma endregion
#pragma endregion
public:
#pragma region Operators
	//! Assigns another string to this one.
	TextTemplate &operator=(const TextTemplate &other)
	{
		if (this->str == other.str)
		{
			return *this;
		}

		if (this->Header->referenceCount < 0)
		{
			if (other.Header->referenceCount >= 0)
			{
				this->str = other.str;
				this->Header->IncrementReferenceCount();
			}
		}
		else if (other.Header->referenceCount < 0)
		{
			this->Release();
			this->str = other.str;
		}
		else
		{
			this->Release();
			this->str = other.str;
			this->Header->IncrementReferenceCount();
		}
		return *this;
	}
	//! Assigns another string to this one.
	TextTemplate &operator=(const symbol *other)
	{
		this->Assign(other);

		return *this;
	}
	//! Assigns another string to this one.
	TextTemplate &operator=(std::initializer_list<const symbol *> parts)
	{
		this->Assign(parts);

		return *this;
	}

	bool operator ==(const TextTemplate &other) const
	{
		return _compare(this->str, other.str) == 0;
	}
	bool operator !=(const TextTemplate &other) const
	{
		return _compare(this->str, other.str) != 0;
	}
	bool operator ==(const symbol *other) const
	{
		return _compare(this->str, other) == 0;
	}
	bool operator !=(const symbol *other) const
	{
		return _compare(this->str, other) != 0;
	}

	//! Implicit conversion. Returns wrapped pointer.
	operator const symbol *() const
	{
		return this->str;
	}
	//! Used when this object has to be passed to the function with variadic parameter list.
	const symbol *c_str() const
	{
		return this->str;
	}
#pragma endregion
private:
#pragma region Utilities
	static void CopyInternal(symbol *destination, const symbol *source, size_t count)
	{
		if (destination != source)
		{
			memcpy(destination, source, count * sizeof(symbol));
		}
	}
	static void SetInternal(symbol *destination, symbol filler, size_t count)
	{
			memset(destination, filler, count * sizeof(symbol));
	}
	void ConcatenateInPlace(const char *chars, size_t count)
	{
		if (count <= 0)
		{
			return;
		}

		if (this->Shared || this->Length + count > this->Capacity)
		{
			TextHeader *oldHeader = this->Header;
			this->Concatenate(oldHeader->Elements, oldHeader->Length, chars, count);
			ReleaseData(oldHeader);
		}
		else
		{
			CopyInternal(this->str + this->Length, chars, count);
			this->Header->Length += count;
			this->str[this->Length] = '\0';
		}
	}
	void Concatenate(const char *chars1, size_t count1, const char *chars2, size_t count2)
	{
		auto sumLength = count1 + count2;

		auto doubleCount1 = count1 * 2;
		if (doubleCount1 > sumLength)
		{
			sumLength = doubleCount1;
		}

		if (sumLength != 0)
		{
			if (sumLength < 8)
			{
				sumLength = 8;
			}

			this->AllocateMemory(sumLength);
			CopyInternal(this->str, chars1, count1);
			CopyInternal(this->str + count1, chars2, count2);
			this->Header->Length = count1 + count2;
			this->str[count1 + count2] = 0;
		}
	}
#pragma endregion
#pragma region Specialization Utilities
	//! Counts number of characters in the string.
	static size_t _strlen(const symbol *str);
	//! Compares 2 strings.
	static int _compare(const symbol *str1, const symbol *str2);
	//! To managed string.
	static mono_string _mono_str(const symbol *str);
	//! From managed string.
	static symbol *mono_string_native(mono_string str, void *_error);
	//! Checks for the presence of the substring.
	static bool _contains_substring(const symbol *str0, const symbol *str1, bool ignoreCase);
#pragma endregion
};

template<typename symbol>
inline mono_string TextTemplate<symbol>::_mono_str(const symbol *str)
{
#ifdef MONO_API
	return mono_string_new(mono_domain_get(), str);
#elif defined(USE_CRYCIL_API)
	return ToMonoString(str);
#else
	return nullptr;
#endif // MONO_API
}

template<typename symbol>
inline symbol *TextTemplate<symbol>::mono_string_native(mono_string str, void *error)
{
	if (!str)
	{
		return nullptr;
	}
#ifdef MONO_API
	return mono_string_to_utf8_checked(static_cast<MonoString *>(str), static_cast<MonoError *>(error));
#elif defined(USE_CRYCIL_API)
	return ToNativeString(str);
#else
	return nullptr;
#endif // MONO_API
}

template<typename symbol>
inline int TextTemplate<symbol>::_compare(const symbol *str1, const symbol *str2)
{
	return strcmp(str1, str2);
}

template<typename symbol>
inline size_t TextTemplate<symbol>::_strlen(const symbol *str)
{
	return strlen(str);
}

template<typename symbol>
inline bool TextTemplate<symbol>::_contains_substring(const symbol *str0, const symbol *str1, bool ignoreCase)
{
	if (!ignoreCase)
	{
		return strstr(str0, str1) != nullptr;
	}

	int nSuperstringLength = int(strlen(str0));
	int nSubstringLength = int(strlen(str1));

	for (int nSubstringPos = 0; nSubstringPos <= nSuperstringLength - nSubstringLength; ++nSubstringPos)
	{
		if (strnicmp(str0 + nSubstringPos, str1, nSubstringLength) == 0)
			return (str0 + nSubstringPos) != nullptr;
	}
	return false;
}

template<>
inline mono_string TextTemplate<wchar_t>::_mono_str(const wchar_t *str)
{
#ifdef MONO_API
	return mono_string_from_utf16(reinterpret_cast<mono_unichar2 *>(const_cast<wchar_t *>(str)));
#elif defined(USE_CRYCIL_API)
	return MonoEnv->Objects->Texts->ToManaged(str);
#else
	return nullptr;
#endif // MONO_API
}

template<>
inline wchar_t *TextTemplate<wchar_t>::mono_string_native(mono_string str, void *error)
{
#ifdef MONO_API
	// Initialize the error object with no error message in it.
	mono_error_init(static_cast<MonoError *>(error));
	return reinterpret_cast<wchar_t *>(mono_string_to_utf16(str));
#elif defined(USE_CRYCIL_API)
	return const_cast<wchar_t *>(MonoEnv->Objects->Texts->ToNative16(str));
#else
	return nullptr;
#endif // MONO_API
}

template<>
inline int TextTemplate<wchar_t>::_compare(const wchar_t *str1, const wchar_t *str2)
{
	return wcscmp(str1, str2);
}

template<>
inline size_t TextTemplate<wchar_t>::_strlen(const wchar_t *str)
{
	return wcslen(str);
}

template<>
inline bool TextTemplate<wchar_t>::_contains_substring(const wchar_t *str0, const wchar_t *str1, bool ignoreCase)
{
	if (!ignoreCase)
	{
		return wcsstr(str0, str1) != nullptr;
	}

	int superstringLength = int(wcslen(str0));
	int substringLength = int(wcslen(str1));

	for (int substringPos = 0; substringPos <= superstringLength - substringLength; ++substringPos)
	{
		if (wcsnicmp(str0 + substringPos, str1, substringLength) == 0)
			return (str0 + substringPos) != nullptr;
	}
	return false;
}

typedef TextTemplate<char> Text;
typedef TextTemplate<wchar_t> Text16;