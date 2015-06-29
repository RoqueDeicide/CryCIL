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
	IMonoClass *_thread;
public:
	MonoCoreLibrary();
	~MonoCoreLibrary();

	virtual IMonoClass *GetBoolean() override;
	virtual IMonoClass *GetIntPtr() override;
	virtual IMonoClass *GetUIntPtr() override;
	virtual IMonoClass *GetChar() override;
	virtual IMonoClass *GetSbyte() override;
	virtual IMonoClass *GetByte() override;
	virtual IMonoClass *GetInt16() override;
	virtual IMonoClass *GetUInt16() override;
	virtual IMonoClass *GetInt32() override;
	virtual IMonoClass *GetUInt32() override;
	virtual IMonoClass *GetInt64() override;
	virtual IMonoClass *GetUInt64() override;
	virtual IMonoClass *GetSingle() override;
	virtual IMonoClass *GetDouble() override;
	virtual IMonoClass *GetString() override;
	virtual IMonoClass *GetArray() override;
	virtual IMonoClass *GetType() override;
	virtual IMonoClass *GetEnum() override;
	virtual IMonoClass *GetException() override;
	virtual IMonoClass *GetObjectClass() override;
	virtual IMonoClass *GetValueType() override;
	virtual IMonoClass *GetThread() override;

	virtual IMonoClass *GetClass(const char *nameSpace, const char *className) override;

	virtual Text *GetName() override;
	virtual Text *GetFullName() override;
	virtual Text *GetFileName() override;

	virtual mono::assembly GetReflectionObject() override;
	virtual void *GetWrappedPointer() override;
};