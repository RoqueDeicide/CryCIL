#pragma once

#include "IMonoInterface.h"
#include "Wrappers/MonoClassWrapper.h"

#include "MonoHeaders.h"

struct MonoAssemblyWrapper : IMonoAssembly
{
private:
	MonoAssembly *assembly;			//!< Pointer to the assembly object.
	MonoImage *image;				//!< Pointer to the assembly image: region of the
									//!  assembly that contains all of the code and metadata.
public:
	MonoAssemblyWrapper(MonoAssembly *assembly)
	{
		this->assembly = assembly;
		this->image = mono_assembly_get_image(assembly);
	}
	MonoAssemblyWrapper(const char *assemblyFile)
	{
		this->assembly = mono_domain_assembly_open((MonoDomain *)MonoEnv->AppDomain, assemblyFile);

		if (!this->assembly)
		{
			CryFatalError("Unable to load assembly %s.", assemblyFile);
		}
		this->image = mono_assembly_get_image(assembly);
	}
	//! Gets the class.
	//!
	//! @param className Name of the class to get.
	//! @param nameSpace Name space where the class is defined.
	virtual IMonoClass *GetClass(const char *className, const char *nameSpace = "CryCil")
	{
		return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
	}
	//! Returns a pointer to the MonoAssembly for Mono API calls.
	virtual void * GetWrappedPointer()
	{
		return this->assembly;
	}

	VIRTUAL_API virtual mono::assembly GetReflectionObject()
	{
		return (mono::assembly)mono_assembly_get_object((MonoDomain *)MonoEnv->AppDomain, this->assembly);
	}

};