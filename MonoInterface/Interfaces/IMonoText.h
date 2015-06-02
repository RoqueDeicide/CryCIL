#pragma once

#include "IMonoAliases.h"

//! Wraps access to Mono strings.
struct IMonoText : public IMonoObject
{
	//! Creates new wrapper for given string.
	IMonoText(mono::string str)
		: IMonoObject(str)
	{
#ifdef _DEBUG
		if (strcmp(MonoEnv->Objects->GetObjectClass(str)->Name, "String") != 0)
		{
			ReportError("Attempt was made to create a Mono text wrapper for an object that is not of type String.");
		}
#endif // _DEBUG

		this->klass = GetStringClass();
		this->obj = str;
	}
	//! Creates new wrapper for given string.
	IMonoText(MonoGCHandle &handle)
		: IMonoObject(handle)
	{
#ifdef _DEBUG
		if (strcmp(MonoEnv->Objects->GetObjectClass(handle.Object)->Name, "String") != 0)
		{
			ReportError("Attempt was made to create a Mono text wrapper for an object that is not of type String.");
		}
#endif // _DEBUG

		this->klass = GetStringClass();
		this->obj = handle.Object;
	}
	//! Creates a wrapper for the string that is created from given null-terminated one.
	IMonoText(const char *t)
	{
		this->klass = GetStringClass();
		this->obj = MonoEnv->Objects->Texts->ToManaged(t);
	}
	//! Creates a wrapper for the string that is created from given null-terminated one.
	IMonoText(const wchar_t *t)
	{
		this->klass = GetStringClass();
		this->obj = MonoEnv->Objects->Texts->ToManaged(t);
	}
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
	//! Gets the length of the string.
	__declspec(property(get = GetLength)) int Length;

	//! Determines whether this string is equal to another.
	bool Equals(IMonoText &other)
	{
		return MonoEnv->Objects->StringEquals(this->obj, other.obj);
	}
	//! Determines whether this string is equal to another.
	bool Equals(mono::string other)
	{
		return MonoEnv->Objects->StringEquals(this->obj, other);
	}
	//! Puts this string into intern pool.
	//!
	//! The memory this string was taking up before interning will be eventually GCed.
	//!
	//! All interned strings are pinned, so it is highly recommended to intern any string that is
	//! constantly being used.
	void Intern()
	{
		this->obj = MonoEnv->Objects->InternString(this->obj);
	}
	//! Gets reference to a character in UTF-16 encoding.
	//!
	//! @param index Zero-based index of the character to get.
	wchar_t &operator[](int index)
	{
		return MonoEnv->Objects->StringAt(this->obj, index);
	}

	int GetHashCode()
	{
		return MonoEnv->Objects->GetStringHashCode(this->obj);
	}
	bool IsInterned()
	{
		return MonoEnv->Objects->IsStringInterned(this->obj);
	}
	const char *ToNativeUTF8()
	{
		return MonoEnv->Objects->StringToNativeUTF8(this->obj);
	}
	const wchar_t *ToNativeUTF16()
	{
		return MonoEnv->Objects->StringToNativeUTF16(this->obj);
	}
	int GetLength()
	{
		return MonoEnv->Objects->StringLength(this->obj);
	}

	static IMonoClass *GetStringClass()
	{
		static IMonoClass *stringClass = MonoEnv->CoreLibrary->String;
		return stringClass;
	}
};