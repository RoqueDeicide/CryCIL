#pragma once

#include "IMonoInterface.h"

struct MonoCoreLibrary : public IMonoCoreLibrary
{
private:
	MonoAssembly *assembly;
	MonoImage    *image;

	char *fileData;		//!< Pointer to the data this assembly was loaded from.
	void *debugData;	//!< Pointer to the data debug information was loaded from.

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

	IMonoClass *GetBoolean() const override;
	IMonoClass *GetIntPtr() const override;
	IMonoClass *GetUIntPtr() const override;
	IMonoClass *GetChar() const override;
	IMonoClass *GetSbyte() const override;
	IMonoClass *GetByte() const override;
	IMonoClass *GetInt16() const override;
	IMonoClass *GetUInt16() const override;
	IMonoClass *GetInt32() const override;
	IMonoClass *GetUInt32() const override;
	IMonoClass *GetInt64() const override;
	IMonoClass *GetUInt64() const override;
	IMonoClass *GetSingle() const override;
	IMonoClass *GetDouble() const override;
	IMonoClass *GetString() const override;
	IMonoClass *GetArray() const override;
	IMonoClass *GetType() const override;
	IMonoClass *GetEnum() const override;
	IMonoClass *GetException() const override;
	IMonoClass *GetObjectClass() const override;
	IMonoClass *GetValueType() const override;
	IMonoClass *GetThread() const override;

	IMonoClass *GetClass(const char *nameSpace, const char *className) const override;
	void AssignData(char *data) override;
	void AssignDebugData(void *data) override;
	void TransferData(IMonoAssembly *other) override;

	const Text &GetName() const override;
	const Text &GetFullName() const override;
	const Text &GetFileName() const override;
	void        SetFileName(const char *) override;

	mono::assembly GetReflectionObject() const override;
	void          *GetWrappedPointer() const override;

};