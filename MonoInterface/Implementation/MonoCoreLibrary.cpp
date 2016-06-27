#include "stdafx.h"

#include "MonoCoreLibrary.h"
#include "AssemblyUtilities.h"
#include "MonoClass.h"
#include "MonoAssemblies.h"

MonoCoreLibrary::MonoCoreLibrary()
{
	this->image = mono_get_corlib();
	this->assembly = mono_image_get_assembly(this->image);

	GetAssemblyNames(this->assembly, this->fullName, this->name);

	this->fileName = "";

	this->intPtr      = MonoClassCache::Wrap(mono_get_intptr_class());
	this->uintPtr     = MonoClassCache::Wrap(mono_get_uintptr_class());
	this->boolean     = MonoClassCache::Wrap(mono_get_boolean_class());
	this->char16      = MonoClassCache::Wrap(mono_get_char_class());
	this->int8        = MonoClassCache::Wrap(mono_get_sbyte_class());
	this->uint8       = MonoClassCache::Wrap(mono_get_byte_class());
	this->int16       = MonoClassCache::Wrap(mono_get_int16_class());
	this->uint16      = MonoClassCache::Wrap(mono_get_uint16_class());
	this->int32       = MonoClassCache::Wrap(mono_get_int32_class());
	this->uint32      = MonoClassCache::Wrap(mono_get_uint32_class());
	this->int64       = MonoClassCache::Wrap(mono_get_int64_class());
	this->uint64      = MonoClassCache::Wrap(mono_get_uint64_class());
	this->float32     = MonoClassCache::Wrap(mono_get_single_class());
	this->float64     = MonoClassCache::Wrap(mono_get_double_class());
	this->text        = MonoClassCache::Wrap(mono_get_string_class());
	this->fixedArray  = MonoClassCache::Wrap(mono_get_array_class());
	this->typeInfo    = MonoClassCache::Wrap(mono_class_from_name(this->image, "System", "Type"));
	this->enumeration = MonoClassCache::Wrap(mono_get_enum_class());
	this->exception   = MonoClassCache::Wrap(mono_get_exception_class());
	this->objClass    = MonoClassCache::Wrap(mono_get_object_class());
	this->valueType   = MonoClassCache::Wrap(mono_class_from_name(this->image, "System", "ValueType"));
	this->_thread     = MonoClassCache::Wrap(mono_get_thread_class());

	auto assemblies = static_cast<MonoAssemblies *>(MonoEnv->Assemblies);
	IMonoAssembly *ptr;
	assemblies->Registry.Add(this, &ptr);
	ptr->TransferData(this);
	delete ptr;
}

MonoCoreLibrary::~MonoCoreLibrary()
{
	if (this->fileData) delete[] this->fileData;
	if (this->debugData) delete[] this->debugData;
}

IMonoClass *MonoCoreLibrary::GetBoolean() const
{
	return this->boolean;
}

IMonoClass *MonoCoreLibrary::GetIntPtr() const
{
	return this->intPtr;
}

IMonoClass *MonoCoreLibrary::GetUIntPtr() const
{
	return this->uintPtr;
}

IMonoClass *MonoCoreLibrary::GetChar() const
{
	return this->char16;
}

IMonoClass *MonoCoreLibrary::GetSbyte() const
{
	return this->int8;
}

IMonoClass *MonoCoreLibrary::GetByte() const
{
	return this->uint8;
}

IMonoClass *MonoCoreLibrary::GetInt16() const
{
	return this->int16;
}

IMonoClass *MonoCoreLibrary::GetUInt16() const
{
	return this->uint16;
}

IMonoClass *MonoCoreLibrary::GetInt32() const
{
	return this->int32;
}

IMonoClass *MonoCoreLibrary::GetUInt32() const
{
	return this->uint32;
}

IMonoClass *MonoCoreLibrary::GetInt64() const
{
	return this->int64;
}

IMonoClass *MonoCoreLibrary::GetUInt64() const
{
	return this->uint64;
}

IMonoClass *MonoCoreLibrary::GetSingle() const
{
	return this->float32;
}

IMonoClass *MonoCoreLibrary::GetDouble() const
{
	return this->float64;
}

IMonoClass *MonoCoreLibrary::GetString() const
{
	return this->text;
}

IMonoClass *MonoCoreLibrary::GetArray() const
{
	return this->fixedArray;
}

IMonoClass *MonoCoreLibrary::GetType() const
{
	return this->typeInfo;
}

IMonoClass *MonoCoreLibrary::GetEnum() const
{
	return this->enumeration;
}

IMonoClass *MonoCoreLibrary::GetException() const
{
	return this->exception;
}

IMonoClass *MonoCoreLibrary::GetObjectClass() const
{
	return this->objClass;
}

IMonoClass *MonoCoreLibrary::GetValueType() const
{
	return this->valueType;
}

IMonoClass *MonoCoreLibrary::GetThread() const
{
	return this->_thread;
}

IMonoClass *MonoCoreLibrary::GetClass(const char *nameSpace, const char *className) const
{
	return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
}

void MonoCoreLibrary::AssignData(char *data)
{
	if (this->fileData || !data)
	{
		return;
	}

	this->fileData = data;
}

void MonoCoreLibrary::AssignDebugData(void *data)
{
	if (this->debugData || !data)
	{
		return;
	}

	this->debugData = data;
}

void MonoCoreLibrary::TransferData(IMonoAssembly *other)
{
	other->AssignData(this->fileData);
	other->AssignDebugData(this->debugData);

	this->fileData = nullptr;
	this->debugData = nullptr;
}

const Text &MonoCoreLibrary::GetName() const
{
	return this->name;
}

const Text &MonoCoreLibrary::GetFullName() const
{
	return this->fullName;
}

const Text &MonoCoreLibrary::GetFileName() const
{
	return this->fileName;
}

void MonoCoreLibrary::SetFileName(const char *)
{
}

mono::assembly MonoCoreLibrary::GetReflectionObject() const
{
	return mono::assembly(mono_assembly_get_object(mono_domain_get(), this->assembly));
}

void *MonoCoreLibrary::GetWrappedPointer() const
{
	return this->assembly;
}
