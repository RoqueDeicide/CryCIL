#include "stdafx.h"
#include "API_ImplementationHeaders.h"

MonoAssemblyWrapper::MonoAssemblyWrapper(MonoAssembly *assembly)
{
	this->assembly = assembly;
	this->image = mono_assembly_get_image(assembly);
}
//! Attempts to load assembly located in the file.
//!
//! @param assemblyFile Path to the assembly file to try loading.
//! @param failed       Indicates whether this constructor was successful.
MonoAssemblyWrapper::MonoAssemblyWrapper(const char *assemblyFile, bool &failed)
{
	if (Pdb2MdbThunks::Convert)
	{
		mono::exception ex;
		Pdb2MdbThunks::Convert(MonoEnv->ToManagedString(assemblyFile), &ex);
	}
	this->assembly = mono_domain_assembly_open((MonoDomain *)MonoEnv->AppDomain, assemblyFile);
	failed = !this->assembly;
	this->image = (failed) ? nullptr : mono_assembly_get_image(assembly);
}
//! Gets the class.
//!
//! @param nameSpace Name space where the class is defined.
//! @param className Name of the class to get.
IMonoClass *MonoAssemblyWrapper::GetClass(const char *nameSpace, const char *className)
{
	CryLogAlways("Searching for class.");
	CryLogAlways("Image: %d", this->image);
	CryLogAlways("Name space: %s.", nameSpace);
	CryLogAlways("Class name: %s.", className);

	if (this->image != mono_get_corlib())
	{
		CryLogAlways("This assembly is not corlib.");
	}

	MonoClass *klass = mono_class_from_name(this->image, nameSpace, className);
	CryLogAlways("Wrapping the class.");
	return MonoClassCache::Wrap(klass);
}
//! Returns a method that satisfies given description.
//!
//! @param nameSpace  Name space where the class where the method is declared is located.
//! @param className  Name of the class where the method is declared.
//! @param methodName Name of the method to look for.
//! @param params     A comma-separated list of names of types of arguments. Can be null
//!                   if method accepts no arguments.
//!
//! @returns A pointer to object that implements IMonoMethod that grants access to
//!          requested method if found, otherwise returns null.
IMonoMethod *MonoAssemblyWrapper::MethodFromDescription
(
const char *nameSpace, const char *className,
const char *methodName, const char *params
)
{
	CryLogAlways("Getting class.");
	// Get the class.
	IMonoClass *klass = this->GetClass(nameSpace, className);
	CryLogAlways("Getting method.");
	// Get the method.
	return klass->GetMethod(methodName, params);
}
//! Returns a pointer to the MonoAssembly for Mono API calls.
void *MonoAssemblyWrapper::GetWrappedPointer()
{
	return this->assembly;
}

mono::assembly MonoAssemblyWrapper::GetReflectionObject()
{
	return (mono::assembly)mono_assembly_get_object((MonoDomain *)MonoEnv->AppDomain, this->assembly);
}