#pragma once

#include "IMonoInterface.h"
#include "Implementation/MonoClass.h"

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
	Text *fileName;					//!< Name of the file this assembly was loaded from.
public:
	//! Wraps given assembly.
	explicit MonoAssemblyWrapper(MonoAssembly *assembly);
	//! Attempts to load assembly located in the file.
	MonoAssemblyWrapper(const char *assemblyFile, bool &failed);
	~MonoAssemblyWrapper()
	{
		this->assembly = nullptr;
		this->image = nullptr;

		SAFE_DELETE(this->shortName);
		SAFE_DELETE(this->fullName);
		SAFE_DELETE(this->fileName);
	}
	//! Gets the class.
	virtual IMonoClass *GetClass(const char *nameSpace, const char *className) const override;

	virtual Text *GetName() const override;
	virtual Text *GetFullName() const override;
	virtual Text *GetFileName() const override;

	virtual void *GetWrappedPointer() const override;
	
	virtual mono::assembly GetReflectionObject() const override;
};