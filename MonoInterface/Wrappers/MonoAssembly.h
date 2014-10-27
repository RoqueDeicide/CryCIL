#pragma once

#include "IMonoInterface.h"
#include "Wrappers/MonoClass.h"

#include "MonoHeaders.h"

#include <sstream>
//! Implementation for IMonoAssembly.
struct MonoAssemblyWrapper : public IMonoAssembly
{
private:
	MonoAssembly *assembly;			//!< Pointer to the assembly object.
	MonoImage *image;				//!< Pointer to the assembly image: region of the
									//!  assembly that contains all of the code and metadata.
public:
	//! Wraps given assembly.
	//!
	//! @param assembly Pointer to assembly to wrap.
	MonoAssemblyWrapper(MonoAssembly *assembly);
	//! Attempts to load assembly located in the file.
	//!
	//! @param assemblyFile Path to the assembly file to try loading.
	//! @param failed       Indicates whether this constructor was successful.
	MonoAssemblyWrapper(const char *assemblyFile, bool &failed);
	//! Gets the class.
	//!
	//! @param nameSpace Name space where the class is defined.
	//! @param className Name of the class to get.
	virtual IMonoClass *GetClass(const char *nameSpace, const char *className);
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
	);
	//! Returns a pointer to the MonoAssembly for Mono API calls.
	virtual void * GetWrappedPointer();
	virtual mono::assembly GetReflectionObject();
};