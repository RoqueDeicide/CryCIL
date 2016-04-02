#include "stdafx.h"
#include "API_ImplementationHeaders.h"

#include "AssemblyUtilities.h"

MonoAssemblyWrapper::MonoAssemblyWrapper(MonoAssembly *assembly)
{
	if (!assembly)
	{
		this->assembly = nullptr;
		this->image = nullptr;
		this->shortName = nullptr;
		this->fullName = nullptr;
		this->fileName = nullptr;
	}
	this->assembly = assembly;
	this->image = mono_assembly_get_image(assembly);

	auto names = GetAssemblyNames(this->image);

	this->fullName = names.Value1;
	this->shortName = names.Value2;

	this->fileName = "Unknown file name.";
}
//! Attempts to load assembly located in the file.
//!
//! @param assemblyFile Path to the assembly file to try loading.
//! @param failed       Indicates whether this constructor was successful.
MonoAssemblyWrapper::MonoAssemblyWrapper(const char *assemblyFile, bool &failed)
{
	MonoImageOpenStatus status;
	this->assembly = mono_assembly_open(assemblyFile, &status);
	failed = status != MONO_IMAGE_OK;

	if (status == MONO_IMAGE_OK)
	{
		this->image = mono_assembly_get_image(assembly);
		
		auto names = GetAssemblyNames(this->image);

		this->fullName = names.Value1;
		this->shortName = names.Value2;

		this->fileName = assemblyFile;
	}
	else
	{
		this->image = nullptr;
		this->shortName = nullptr;
		this->fullName = nullptr;
		this->fileName = nullptr;
	}
}

IMonoClass *MonoAssemblyWrapper::GetClass(const char *nameSpace, const char *className) const
{
	MonoClass *klass = mono_class_from_name(this->image, nameSpace, className);

	return MonoClassCache::Wrap(klass);
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