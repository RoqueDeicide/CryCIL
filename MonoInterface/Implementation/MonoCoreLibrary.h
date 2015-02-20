#pragma once

#include "IMonoInterface.h"

struct MonoCoreLibrary : public IMonoCoreLibrary
{
private:
	MonoAssembly *assembly;
	MonoImage *image;

	Text *name;
	Text *fullName;
	Text *fileName;

	IMonoClass *intPtr;
	IMonoClass *uintPtr;
	IMonoClass *boolean;
	IMonoClass *char16;
	IMonoClass *int8;
	IMonoClass *uint8;
	IMonoClass *int16;
	IMonoClass *uint16;
	IMonoClass *int32;
	IMonoClass *uint32;
	IMonoClass *int64;
	IMonoClass *uint64;
	IMonoClass *float32;
	IMonoClass *float64;
	IMonoClass *text;
	IMonoClass *fixedArray;
	IMonoClass *typeInfo;
	IMonoClass *enumeration;
	IMonoClass *exception;
	IMonoClass *objClass;
	IMonoClass *valueType;
public:
	MonoCoreLibrary();
	~MonoCoreLibrary();

	virtual IMonoClass *GetBoolean();
	virtual IMonoClass *GetIntPtr();
	virtual IMonoClass *GetUIntPtr();
	virtual IMonoClass *GetChar();
	virtual IMonoClass *GetSbyte();
	virtual IMonoClass *GetByte();
	virtual IMonoClass *GetInt16();
	virtual IMonoClass *GetUInt16();
	virtual IMonoClass *GetInt32();
	virtual IMonoClass *GetUInt32();
	virtual IMonoClass *GetInt64();
	virtual IMonoClass *GetUInt64();
	virtual IMonoClass *GetSingle();
	virtual IMonoClass *GetDouble();
	virtual IMonoClass *GetString();
	virtual IMonoClass *GetArray();
	virtual IMonoClass *GetType();
	virtual IMonoClass *GetEnum();
	virtual IMonoClass *GetException();
	virtual IMonoClass *GetObjectClass();
	virtual IMonoClass *GetValueType();

	virtual IMonoClass *GetClass(const char *nameSpace, const char *className);

	virtual Text *GetName();
	virtual Text *GetFullName();
	virtual Text *GetFileName();

	virtual mono::assembly GetReflectionObject();
	virtual void *GetWrappedPointer();
};