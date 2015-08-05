#include "stdafx.h"

#include "MonoCoreLibrary.h"
#include "AssemblyUtilities.h"
#include "MonoClass.h"
#include "MonoAssemblies.h"

MonoCoreLibrary::MonoCoreLibrary()
{
	this->image = mono_get_corlib();
	this->assembly = mono_image_get_assembly(this->image);

	auto names = GetAssemblyNames(this->image);

	this->name = names.Value2;
	this->fullName = names.Value1;

	this->fileName = new Text("");

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

	List<IMonoAssembly *> *corlibList = new List<IMonoAssembly *>(1);
	corlibList->Add(this);
	static_cast<MonoAssemblies *>(MonoEnv->Assemblies)->AssemblyRegistry->Add(this->name, corlibList);
}

MonoCoreLibrary::~MonoCoreLibrary()
{
	SAFE_DELETE(this->name);
	SAFE_DELETE(this->fullName);
	SAFE_DELETE(this->fileName);
}

IMonoClass *MonoCoreLibrary::GetBoolean()
{
	return this->boolean;
}

IMonoClass *MonoCoreLibrary::GetIntPtr()
{
	return this->intPtr;
}

IMonoClass *MonoCoreLibrary::GetUIntPtr()
{
	return this->uintPtr;
}

IMonoClass *MonoCoreLibrary::GetChar()
{
	return this->char16;
}

IMonoClass *MonoCoreLibrary::GetSbyte()
{
	return this->int8;
}

IMonoClass *MonoCoreLibrary::GetByte()
{
	return this->uint8;
}

IMonoClass *MonoCoreLibrary::GetInt16()
{
	return this->int16;
}

IMonoClass *MonoCoreLibrary::GetUInt16()
{
	return this->uint16;
}

IMonoClass *MonoCoreLibrary::GetInt32()
{
	return this->int32;
}

IMonoClass *MonoCoreLibrary::GetUInt32()
{
	return this->uint32;
}

IMonoClass *MonoCoreLibrary::GetInt64()
{
	return this->int64;
}

IMonoClass *MonoCoreLibrary::GetUInt64()
{
	return this->uint64;
}

IMonoClass *MonoCoreLibrary::GetSingle()
{
	return this->float32;
}

IMonoClass *MonoCoreLibrary::GetDouble()
{
	return this->float64;
}

IMonoClass *MonoCoreLibrary::GetString()
{
	return this->text;
}

IMonoClass *MonoCoreLibrary::GetArray()
{
	return this->fixedArray;
}

IMonoClass *MonoCoreLibrary::GetType()
{
	return this->typeInfo;
}

IMonoClass *MonoCoreLibrary::GetEnum()
{
	return this->enumeration;
}

IMonoClass *MonoCoreLibrary::GetException()
{
	return this->exception;
}

IMonoClass *MonoCoreLibrary::GetObjectClass()
{
	return this->objClass;
}

IMonoClass *MonoCoreLibrary::GetValueType()
{
	return this->valueType;
}

IMonoClass *MonoCoreLibrary::GetThread()
{
	return this->_thread;
}

IMonoClass *MonoCoreLibrary::GetClass(const char *nameSpace, const char *className)
{
	return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
}

Text *MonoCoreLibrary::GetName()
{
	return this->name;
}

Text *MonoCoreLibrary::GetFullName()
{
	return this->fullName;
}

Text *MonoCoreLibrary::GetFileName()
{
	return this->fileName;
}

mono::assembly MonoCoreLibrary::GetReflectionObject()
{
	return mono::assembly(mono_assembly_get_object(mono_domain_get(), this->assembly));
}

void *MonoCoreLibrary::GetWrappedPointer()
{
	return this->assembly;
}