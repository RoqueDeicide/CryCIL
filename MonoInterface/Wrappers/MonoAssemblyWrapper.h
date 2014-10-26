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
	MonoAssemblyWrapper(const char *assemblyFile, bool &failed)
	{
		if (Pdb2MdbThunks::Convert)
		{
			mono::exception ex;
			Pdb2MdbThunks::Convert(this->ToManagedString(assemblyFile), &ex);
		}
		this->assembly = mono_domain_assembly_open((MonoDomain *)MonoEnv->AppDomain, assemblyFile);
		failed = !this->assembly;
		this->image = (failed) ? nullptr : mono_assembly_get_image(assembly);
	}
	//! Gets the class.
	//!
	//! @param nameSpace Name space where the class is defined.
	//! @param className Name of the class to get.
	virtual IMonoClass *GetClass(const char *nameSpace, const char *className)
	{
		return MonoClassCache::Wrap(mono_class_from_name(this->image, nameSpace, className));
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
	virtual IMonoMethod *MethodFromDescription
	(
		const char *nameSpace, const char *className,
		const char *methodName, const char *params
	)
	{
		std::stringstream descriptionText;

		descriptionText << nameSpace << "." << className << ":" << methodName;
		if (params)
		{
			descriptionText << "(" << params << ")";
		}
		return new MonoMethodWrapper
		(
			mono_method_desc_search_in_image
			(
				mono_method_desc_new(descriptionText.str().c_str(), true),
				this->image
			)
		);
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