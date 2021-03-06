#pragma once

#include "IMonoAliases.h"

//! Provides access to a number of classes defined in the core library.
struct IMonoCoreLibrary : public IMonoAssembly
{
	//! Gets a wrapper for the corresponding managed type.
	__declspec(property(get = GetBoolean))     IMonoClass *Boolean;
	__declspec(property(get = GetIntPtr))      IMonoClass *IntPtr;
	__declspec(property(get = GetUIntPtr))     IMonoClass *UIntPtr;
	__declspec(property(get = GetChar))        IMonoClass *Char;
	__declspec(property(get = GetSbyte))       IMonoClass *Sbyte;
	__declspec(property(get = GetByte))        IMonoClass *Byte;
	__declspec(property(get = GetInt16))       IMonoClass *Int16;
	__declspec(property(get = GetUInt16))      IMonoClass *UInt16;
	__declspec(property(get = GetInt32))       IMonoClass *Int32;
	__declspec(property(get = GetUInt32))      IMonoClass *UInt32;
	__declspec(property(get = GetInt64))       IMonoClass *Int64;
	__declspec(property(get = GetUInt64))      IMonoClass *UInt64;
	__declspec(property(get = GetSingle))      IMonoClass *Single;
	__declspec(property(get = GetDouble))      IMonoClass *Double;
	__declspec(property(get = GetString))      IMonoClass *String;
	__declspec(property(get = GetArray))       IMonoClass *Array;
	__declspec(property(get = GetType))        IMonoClass *Type;
	__declspec(property(get = GetEnum))        IMonoClass *Enum;
	__declspec(property(get = GetException))   IMonoClass *Exception;
	__declspec(property(get = GetObjectClass)) IMonoClass *Object;
	__declspec(property(get = GetValueType))   IMonoClass *ValueType;
	__declspec(property(get = GetThread))      IMonoClass *Thread;

	VIRTUAL_API virtual IMonoClass *GetBoolean() const = 0;
	VIRTUAL_API virtual IMonoClass *GetIntPtr() const = 0;
	VIRTUAL_API virtual IMonoClass *GetUIntPtr() const = 0;
	VIRTUAL_API virtual IMonoClass *GetChar() const = 0;
	VIRTUAL_API virtual IMonoClass *GetSbyte() const = 0;
	VIRTUAL_API virtual IMonoClass *GetByte() const = 0;
	VIRTUAL_API virtual IMonoClass *GetInt16() const = 0;
	VIRTUAL_API virtual IMonoClass *GetUInt16() const = 0;
	VIRTUAL_API virtual IMonoClass *GetInt32() const = 0;
	VIRTUAL_API virtual IMonoClass *GetUInt32() const = 0;
	VIRTUAL_API virtual IMonoClass *GetInt64() const = 0;
	VIRTUAL_API virtual IMonoClass *GetUInt64() const = 0;
	VIRTUAL_API virtual IMonoClass *GetSingle() const = 0;
	VIRTUAL_API virtual IMonoClass *GetDouble() const = 0;
	VIRTUAL_API virtual IMonoClass *GetString() const = 0;
	VIRTUAL_API virtual IMonoClass *GetArray() const = 0;
	VIRTUAL_API virtual IMonoClass *GetType() const = 0;
	VIRTUAL_API virtual IMonoClass *GetEnum() const = 0;
	VIRTUAL_API virtual IMonoClass *GetException() const = 0;
	VIRTUAL_API virtual IMonoClass *GetObjectClass() const = 0;
	VIRTUAL_API virtual IMonoClass *GetValueType() const = 0;
	VIRTUAL_API virtual IMonoClass *GetThread() const = 0;
};