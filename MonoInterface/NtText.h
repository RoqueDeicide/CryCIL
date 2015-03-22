#pragma once

// Stop compiler from complaining about strncpy.
#pragma warning(disable:4996)

#include "DocumentationMarkers.h"

#ifdef USE_CRYCIL_API

#include "IMonoInterface.h"

#endif // USE_CRYCIL_API

#ifdef CRYCIL_MODULE

#include <mono/metadata/object.h>
#include <mono/metadata/appdomain.h>

#endif // CRYCIL_MODULE

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

#include "List.h"

//! Wraps a null-terminated string.
//!
//! This wrapper will automatically delete memory associated with the wrapper pointer, so set it to null,
//! if you want to keep the memory from deletion.
//!
//! This wrapper can be converted to const char * pointer implicitly.
//!
//! Check documentation for this object's assignment operator overload.
struct NtText
{
private:
	const char *chars;
public:
	//! Creates a default text.
	//!
	//! Only default text can be assigned to.
	NtText()
	{
		this->chars = nullptr;
	}
	//! Creates a new null-terminated string from given null-terminated one.
	//!
	//! Text from given string is copied into new object without a terminating null character.
	//!
	//! @param t Text to initialize this object with.
	NtText(const char *t)
	{
		this->chars = t;
	}
#ifdef CRYCIL_MODULE
	//! Creates a new null-terminated string from given .Net/Mono string.
	//!
	//! @param managedString Instance of type System.String.
	NtText(MonoString *managedString)
	{
		MonoError error;
		char *ntText = mono_string_to_utf8_checked(managedString, &error);
		if (mono_error_ok(&error))
		{
			int length = strlen(ntText);
			this->chars = new char[length + 1];
			for (int i = 0; i < length; i++)
			{
				((char *)this->chars)[i] = ntText[i];
			}
			mono_free(ntText);
			((char *)this->chars)[length] = '\0';
		}
		else
		{
			FatalError(mono_error_get_message(&error));
		}
	}
#endif // CRYCIL_MODULE

#ifdef USE_CRYCIL_API

	//! Creates a new null-terminated string from given .Net/Mono string.
	//!
	//! @param managedString Instance of type System.String.
	NtText(mono::string managedString)
	{
		this->chars = ToNativeString(managedString);
	}

#endif // USE_CRYCIL_API
	//! Constructs a text out of given parts.
	//!
	//! @param t1 Number of arguments in the chain.
	NtText(int count...)
	{
		// Gather the arguments into the list and calculate total length at the same time.
		va_list va;
		va_start(va, count);

		auto parts = List<const char *>(count);

		int length = 0;
		for (int i = 0; i < count; i++)
		{
			const char *next = va_arg(va, const char *);

			if (next)
			{
				length += strlen(next);
				parts.Add(next);
			}
		}

		va_end(va);

		this->chars = new char[length + 1];
		// Copy the characters to this string.
		int j = 0;
		for (int i = 0; i < parts.Length; i++)
		{
			for (int k = 0; parts[i][k]; k++)
			{
				((char *)this->chars)[j++] = parts[i][k];
			}
		}
		((char *)this->chars)[length] = '\0';
	}
	virtual ~NtText()
	{
		if (this->chars)
		{
			delete this->chars;
			this->chars = nullptr;
		}
	}
	//! Swaps pointers to null-terminated strings of this and another object.
	//!
	//! @param another A reference to another NtText object that will manage a pointer of this object.
	SWAP_ASSIGNMENT NtText &operator=(NtText &another)
	{
		if (this->chars != another.chars)
		{
			const char *ptr = this->chars;
			this->chars = another.chars;
			another.chars = ptr;
		}
		return *this;
	}
	//! Creates a null-terminated string from substring of this text.
	//!
	//! Created string is allocated within the heap and needs to be deleted after use.
	//!
	//! @param index Zero-based index of the first symbol of the substring to copy to
	//!              null-terminated result.
	//! @param count Number of characters to copy. Length of the resultant string will
	//!              be count + 1 to accommodate the null character.
	const char *Substring(int index, int count) const
	{
		if (!this->chars)
		{
			return nullptr;
		}
		int length = strlen(this->chars);
		if (index + count > length)
		{
			FatalError("Attempt to copy too many characters from the string.");
		}
		char *ntText = new char[count + 1];
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
		return const_cast<const char *>(ntText);
	}
	//! Copies characters from beginning of this text to the given character array.
	//!
	//! @param destination Pointer to the beginning of the destination array.
	//! @param index       Zero-based index of the first symbol within destination array
	//!                    to copy text to.
	//! @param symbolCount Number of characters to copy.
	void CopyTo(char *destination, int index, int symbolCount)
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
	void CopyTo(char *destination, int sourceIndex, int destinationIndex, int charCount)
	{
		int length = strlen(this->chars);
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
#ifdef CRYCIL_MODULE
	//! Creates a managed string from this one.
	MonoString *ToManagedString() const
	{
		return mono_string_new_len(mono_domain_get(), this->chars, strlen(this->chars));
	}
#endif // CRYCIL_MODULE
	//! Splits this string into parts separated by the given symbol.
	//!
	//! @param symbol           Symbol that will separate the parts.
	//! @param removeEmptyParts Indicates whether empty parts should be present in the result.
	//!
	//! @return A list of parts. Must be deleted after use. Before that though, each part
	//!         must be deleted separately.
	List<const char *> *Split(char symbol, bool removeEmptyParts)
	{
		int length = strlen(this->chars);
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
	int IndexOf(char symbol)
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
	int LastIndexOf(char symbol)
	{
		int length = strlen(this->chars);
		for (int i = length - 2; i >= 0; i--)
		{
			if (this->chars[i] == symbol)
			{
				return i;
			}
		}
		return -1;
	}
	//! Gets the length of the string.
	__declspec(property(get = GetLength)) int Length;
	//! Gets the length of the string.
	int GetLength() const
	{
		return strlen(this->chars);
	}
	//! Determines whether this text is equal to one in the null-terminated string.
	bool Equals(const char *ntString)
	{
		return strcmp(this->chars, ntString) == 0;
	}
	int CompareTo(NtText &other)
	{
		return strcmp(this->chars, other.chars);
	}
	//! Detaches the pointer wrapped by this object from it.
	//!
	//! @returns A pointer to a null-terminated string of characters previously wrapped by this object.
	const char *Detach()
	{
		const char *ptr = this->chars;
		this->chars = nullptr;
		return ptr;
	}
	//! Implicit conversion. Returns wrapped pointer.
	operator const char*() const
	{
		return this->chars;
	}
};