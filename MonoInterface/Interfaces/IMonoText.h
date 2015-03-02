#pragma once

#include "IMonoAliases.h"

//! Wraps access to Mono strings.
struct IMonoText : public IMonoHandle
{
	//! Indicates whether this string is located in an intern pool (shared memory for literals).
	//!
	//! All interned strings are pinned, so it is highly recommended to intern any string that is
	//! constantly being used.
	__declspec(property(get = IsInterned)) bool Interned;
	//! Gets hash code of this string.
	__declspec(property(get = GetHashCode)) int HashCode;
	//! Creates null-terminated version of this Mono string using UTF-8 encoding.
	//!
	//! Returned buffer needs to be deleted after use.
	__declspec(property(get = ToNativeUTF8)) const char *NativeUTF8;
	//! Creates null-terminated version of this Mono string using UTF-16 encoding.
	//!
	//! Returned buffer needs to be deleted after use.
	__declspec(property(get = ToNativeUTF16)) const wchar_t *NativeUTF16;

	//! Determines whether this string is equal to another.
	VIRTUAL_API virtual bool Equals(IMonoText *other) = 0;
	//! Determines whether this string is equal to another.
	VIRTUAL_API virtual bool Equals(mono::string other) = 0;
	//! Puts this string into intern pool.
	//!
	//! The memory this string was taking up before interning will be eventually GCed.
	//!
	//! All interned strings are pinned, so it is highly recommended to intern any string that is
	//! constantly being used.
	VIRTUAL_API virtual void Intern() = 0;
	//! Gets reference to a character in UTF-16 encoding.
	//!
	//! @param index Zero-based index of the character to get.
	VIRTUAL_API virtual wchar_t &At(int index) = 0;

	VIRTUAL_API virtual int GetHashCode() = 0;
	VIRTUAL_API virtual bool IsInterned() = 0;
	VIRTUAL_API virtual const char *ToNativeUTF8() = 0;
	VIRTUAL_API virtual const wchar_t *ToNativeUTF16() = 0;
};