#include "stdafx.h"
#include "API_ImplementationHeaders.h"

#include "AssemblyUtilities.h"

MonoAssemblyWrapper::MonoAssemblyWrapper(MonoAssembly *assembly)
	: fileName()
	, fileData(nullptr)
	, debugData(nullptr)
{
	if (!assembly)
	{
		this->assembly  = nullptr;
		this->image     = nullptr;
	}

	this->assembly = assembly;
	this->image    = mono_assembly_get_image(assembly);

	GetAssemblyNames(assembly, this->fullName, this->shortName);
}

IMonoClass *MonoAssemblyWrapper::GetClass(const char *nameSpace, const char *className) const
{
	MonoClass *klass = mono_class_from_name(this->image, nameSpace, className);

	return MonoClassCache::Wrap(klass);
}

void MonoAssemblyWrapper::AssignData(char *data)
{
	if (this->fileData || !data)
	{
		return;
	}

	this->fileData = data;
}

void MonoAssemblyWrapper::AssignDebugData(void *data)
{
	if (this->debugData || !data)
	{
		return;
	}

	this->debugData = data;
}

void MonoAssemblyWrapper::TransferData(IMonoAssembly *other)
{
	other->AssignData(this->fileData);
	other->AssignDebugData(this->debugData);

	this->fileData = nullptr;
	this->debugData = nullptr;
}

const Text &MonoAssemblyWrapper::GetName() const
{
	return this->shortName;
}

const Text &MonoAssemblyWrapper::GetFullName() const
{
	return this->fullName;
}

const Text &MonoAssemblyWrapper::GetFileName() const
{
	return this->fileName;
}

void MonoAssemblyWrapper::SetFileName(const char *fileName)
{
	if (!this->fileName.Empty || !fileName)
	{
		return;
	}

	this->fileName = fileName;
}


//! Returns a pointer to the MonoAssembly for Mono API calls.
void *MonoAssemblyWrapper::GetWrappedPointer() const
{
	return this->assembly;
}

mono::assembly MonoAssemblyWrapper::GetReflectionObject() const
{
	return mono::assembly(mono_assembly_get_object(static_cast<MonoDomain *>(MonoEnv->AppDomain),
												   this->assembly));
}
