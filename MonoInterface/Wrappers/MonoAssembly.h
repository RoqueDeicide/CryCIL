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
	MonoAssemblyWrapper(MonoAssembly *assembly);
	//! Attempts to load assembly located in the file.
	MonoAssemblyWrapper(const char *assemblyFile, bool &failed);
	//! Gets the class.
	virtual IMonoClass *GetClass(const char *nameSpace, const char *className);
	//! Returns a method that satisfies given description.
	virtual IMonoMethod *MethodFromDescription
	(
		const char *nameSpace, const char *className,
		const char *methodName, const char *params
	);
	//! Returns a pointer to the MonoAssembly for Mono API calls.
	virtual void * GetWrappedPointer();
	virtual mono::assembly GetReflectionObject();
};