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

	Text *shortName;				//!< Short name of the assembly.
	Text *fullName;					//!< Full name of the assembly.
public:
	//! Wraps given assembly.
	MonoAssemblyWrapper(MonoAssembly *assembly);
	//! Attempts to load assembly located in the file.
	MonoAssemblyWrapper(const char *assemblyFile, bool &failed);
	//! Gets the class.
	virtual IMonoClass *GetClass(const char *nameSpace, const char *className);

	VIRTUAL_API virtual Text * GetName()
	{
		throw std::logic_error("The method or operation is not implemented.");
	}

	VIRTUAL_API virtual Text * GetFullName()
	{
		throw std::logic_error("The method or operation is not implemented.");
	}
	//! Returns a pointer to the MonoAssembly for Mono API calls.
	virtual void * GetWrappedPointer();
	virtual mono::assembly GetReflectionObject();

};