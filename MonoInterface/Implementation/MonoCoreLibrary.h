#pragma once

#include "IMonoInterface.h"

struct MonoCoreLibrary : public IMonoCoreLibrary
{
private:
	MonoAssembly *assembly;
	MonoImage *image;

	Text name;
	Text fullName;
	Text fileName;

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

	virtual IMonoClass *GetBoolean() const override;
	virtual IMonoClass *GetIntPtr() const override;
	virtual IMonoClass *GetUIntPtr() const override;
	virtual IMonoClass *GetChar() const override;
	virtual IMonoClass *GetSbyte() const override;
	virtual IMonoClass *GetByte() const override;
	virtual IMonoClass *GetInt16() const override;
	virtual IMonoClass *GetUInt16() const override;
	virtual IMonoClass *GetInt32() const override;
	virtual IMonoClass *GetUInt32() const override;
	virtual IMonoClass *GetInt64() const override;
	virtual IMonoClass *GetUInt64() const override;
	virtual IMonoClass *GetSingle() const override;
	virtual IMonoClass *GetDouble() const override;
	virtual IMonoClass *GetString() const override;
	virtual IMonoClass *GetArray() const override;
	virtual IMonoClass *GetType() const override;
	virtual IMonoClass *GetEnum() const override;
	virtual IMonoClass *GetException() const override;
	virtual IMonoClass *GetObjectClass() const override;
	virtual IMonoClass *GetValueType() const override;
	virtual IMonoClass *GetThread() const override;

	virtual IMonoClass *GetClass(const char *nameSpace, const char *className) const override;

	virtual const Text &GetName() const override;
	virtual const Text &GetFullName() const override;
	virtual const Text &GetFileName() const override;

	virtual mono::assembly GetReflectionObject() const override;
	virtual void *GetWrappedPointer() const override;
};