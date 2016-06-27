#pragma once

#include "IMonoInterface.h"
#include "Implementation/MonoClass.h"

#include "MonoHeaders.h"

#include <sstream>
//! Implementation for IMonoAssembly.
struct MonoAssemblyWrapper : public IMonoAssembly
{
private:
	MonoAssembly *assembly;         //!< Pointer to the assembly object.
	//! Pointer to the assembly image: region of the assembly that contains all of the code and metadata.
	MonoImage *image;

	Text shortName;                 //!< Short name of the assembly.
	Text fullName;                  //!< Full name of the assembly.
	Text fileName;                  //!< Name of the file this assembly was loaded from.

	char *fileData;		//!< Pointer to the data this assembly was loaded from.
	void *debugData;	//!< Pointer to the data debug information was loaded from.
public:
	//! Wraps given assembly.
	explicit MonoAssemblyWrapper(MonoAssembly *assembly);
	~MonoAssemblyWrapper()
	{
		this->assembly = nullptr;
		this->image    = nullptr;

		if (this->fileData) delete[] this->fileData;
		if (this->debugData) delete[] this->debugData;
	}
	//! Gets the class.
	IMonoClass *GetClass(const char *nameSpace, const char *className) const override;
	void AssignData(char *data) override;
	void AssignDebugData(void *data) override;
	void TransferData(IMonoAssembly *other) override;

	const Text &GetName() const override;
	const Text &GetFullName() const override;
	const Text &GetFileName() const override;
	void        SetFileName(const char *fileName) override;

	void *GetWrappedPointer() const override;

	mono::assembly GetReflectionObject() const override;

};