#pragma once

#include "IMonoAliases.h"

#include "IMonoArrays.h"
#include "IMonoTexts.h"
#include "IMonoExceptions.h"
#include "IMonoDelegates.h"
#include "IMonoThreads.h"

//! Provides access to functions that create and wrap various Mono objects.
struct IMonoObjects
{
	virtual ~IMonoObjects() {}

	//! Gets the object that allows creation and wrapping of Mono arrays.
	__declspec(property(get = GetArrays))     IMonoArrays       *Arrays;
	//! Gets the object that allows creation of general Mono strings.
	__declspec(property(get = GetTexts))      IMonoTexts        *Texts;
	//! Gets the object that allows creation and wrapping of Mono exceptions.
	__declspec(property(get = GetExceptions)) IMonoExceptions   *Exceptions;
	//! Gets the object that allows creation and wrapping of Mono delegates.
	__declspec(property(get = GetDelegates))  IMonoDelegates    *Delegates;
	//! Gets the object that allows boxing of number of built-in types.
	__declspec(property(get = GetBoxinator))  IDefaultBoxinator *Boxer;
	//! Gets the object that provides access to Mono threads interface.
	__declspec(property(get = GetThreads))    IMonoThreads      *Threads;

	//! Unboxes managed value-type object.
	//!
	//! @param value Value-type object to unbox.
	VIRTUAL_API virtual void *Unbox(mono::object value) = 0;

	VIRTUAL_API virtual IMonoArrays       *GetArrays() = 0;
	VIRTUAL_API virtual IMonoTexts        *GetTexts() = 0;
	VIRTUAL_API virtual IMonoExceptions   *GetExceptions() = 0;
	VIRTUAL_API virtual IMonoDelegates    *GetDelegates() = 0;
	VIRTUAL_API virtual IDefaultBoxinator *GetBoxinator() = 0;
	VIRTUAL_API virtual IMonoThreads      *GetThreads() = 0;

	//! Gets the wrapper for a class that represents provided object.
	VIRTUAL_API virtual IMonoClass *GetObjectClass(mono::object obj) = 0;

#pragma region Array API
	//! Gets number of dimensions in the Mono array.
	//!
	//! @param ar Pointer to the Mono array.
	VIRTUAL_API virtual int GetArrayRank(mono::Array ar) = 0;
	//! Gets the size of each element in the Mono array.
	//!
	//! @param ar Pointer to the Mono array.
	VIRTUAL_API virtual int GetArrayElementSize(mono::Array ar) = 0;
	//! Gets the pointer to the wrapper for the class that represents elements in the Mono array.
	//!
	//! @param ar Pointer to the Mono array.
	VIRTUAL_API virtual IMonoClass *GetArrayElementClass(mono::Array ar) = 0;
#pragma endregion

#pragma region Exception API
	//! Throws Mono exception in the Mono run-time.
	//!
	//! @param ex Exception to throw.
	VIRTUAL_API virtual void ThrowException(mono::exception ex) = 0;
#pragma endregion

#pragma region Delegate API
	//! Gets a wrapper for a function that is represented by the delegate.
	//!
	//! Returned wrapper is allocated in the heap, so delete it, once you don't need it.
	//!
	//! @param delegat Delegate object.
	VIRTUAL_API virtual IMonoFunction *GetDelegateFunction(mono::delegat delegat) = 0;
	//! Gets an object that is used when invoking instance method that is represented by the delegate.
	//!
	//! @param delegat Delegate object.
	VIRTUAL_API virtual mono::object GetDelegateTarget(mono::delegat delegat) = 0;
	//! Gets a function pointer for a wrapper that invokes Mono function that is represented by the delegate.
	//!
	//! @param delegat Delegate object.
	//!
	//! @returns A pointer to the special function that is called 'delegate trampoline' which is a special
	//!          wrapper-function that invokes method that is called by the delegate. It will be deleted
	//!          when the delegate is collected by GC.
	VIRTUAL_API virtual void *GetDelegateTrampoline(mono::delegat delegat) = 0;
#pragma endregion

#pragma region String API
	//! Determines whether one string is equal to another.
	VIRTUAL_API virtual bool StringEquals(mono::string str, mono::string other) = 0;
	//! Puts the string into intern pool.
	//!
	//! The memory the string was taking up before interning will be eventually GCed.
	//!
	//! All interned strings are pinned, so it is highly recommended to intern any string that is
	//! constantly being used.
	VIRTUAL_API virtual mono::string InternString(mono::string str) = 0;
	//! Gets reference to a string character in UTF-16 encoding.
	//!
	//! @param index Zero-based index of the character to get.
	VIRTUAL_API virtual wchar_t &StringAt(mono::string str, int index) = 0;
	//! Gets hash code of the string.
	VIRTUAL_API virtual int GetStringHashCode(mono::string str) = 0;
	//! Determines whether this string is interned.
	VIRTUAL_API virtual bool IsStringInterned(mono::string str) = 0;
	//! Creates a null-terminated array of UTF-8 characters from Mono string.
	VIRTUAL_API virtual const char *StringToNativeUTF8(mono::string str) = 0;
	//! Creates a null-terminated array of UTF-16 characters from Mono string.
	VIRTUAL_API virtual const wchar_t *StringToNativeUTF16(mono::string str) = 0;
#pragma endregion

#pragma region Thread API
	//! Detaches the thread from Mono run-time.
	//!
	//! Should be done when thread is finishing its work and when run-time is shutting down.
	VIRTUAL_API virtual void ThreadDetach(mono::Thread thr) = 0;
#pragma endregion
};