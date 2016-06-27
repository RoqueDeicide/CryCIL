#pragma once

#include "IMonoAliases.h"

//! Defines interface of objects that wrap functionality of MonoAssembly type.
struct IMonoAssembly : public IMonoFunctionalityWrapper
{
	//! Gets the short name of the assembly.
	__declspec(property(get = GetName)) const Text &Name;
	//! Gets the full name of the assembly.
	__declspec(property(get = GetFullName)) const Text &FullName;
	//! Gets or sets the name of the file this assembly was loaded from.
	//!
	//! File name can only be assigned once per assembly.
	__declspec(property(get = GetFileName)) const Text &FileName;

	//! Gets the class.
	//!
	//! @param nameSpace Name space where the class is defined.
	//! @param className Name of the class to get.
	VIRTUAL_API virtual IMonoClass *GetClass(const char *nameSpace, const char *className) const = 0;
	//! Assigns the pointer to the data this assembly was loaded from.
	VIRTUAL_API virtual void AssignData(char *data) = 0;
	//! Assigns the pointer to the debug data of this assembly was loaded from.
	VIRTUAL_API virtual void AssignDebugData(void *data) = 0;
	//! Reassigns data from this object to another.
	VIRTUAL_API virtual void TransferData(IMonoAssembly *other) = 0;
	VIRTUAL_API virtual const Text &GetName() const = 0;
	VIRTUAL_API virtual const Text &GetFullName() const = 0;
	VIRTUAL_API virtual const Text &GetFileName() const = 0;
	//! Assigns the name of the file this assembly was loaded from, if it didn't have one before.
	VIRTUAL_API virtual void SetFileName(const char *fileName) = 0;
	//! Gets the reference to the instance of type System.Reflection.Assembly.
	__declspec(property(get = GetReflectionObject)) mono::assembly ReflectionObject;

	VIRTUAL_API virtual mono::assembly GetReflectionObject() const = 0;
};

//! Represents an object that simplifies the process of getting an assembly that is not accessible
//! through MonoEnv variable.
struct IMonoAssemblies
{
	virtual ~IMonoAssemblies()
	{
	}

	//! Loads a Mono assembly into memory.
	//!
	//! If the assembly was already loaded from the same file, original wrapper will be returned.
	//!
	//! @param path Path to the assembly file. Relative paths go from CryEngine installation
	//!             directory.
	//!
	//! @returns A wrapper for the loaded assembly.
	VIRTUAL_API virtual const IMonoAssembly *Load(const char *path) = 0;
	//! Wraps an assembly pointer.
	//!
	//! Mostly for internal use since acquiring MonoAssembly pointer requires direct work with Mono.
	//!
	//! @param assemblyHandle Pointer to MonoAssembly to wrap.
	VIRTUAL_API virtual const IMonoAssembly *Wrap(void *assemblyHandle) = 0;
	//! Gets the pointer to the assembly wrapper object.
	//!
	//! If there are multiple assemblies loaded with the same file name, it's not guaranteed
	//! that you will get the one you want.
	//!
	//! @param name Name of the assembly (equivalent of the file name but without the extension).
	VIRTUAL_API virtual const IMonoAssembly *GetAssembly(const char *name) = 0;
	//! Gets the pointer to the assembly wrapper object.
	//!
	//! Requires fully-qualified name of the assembly. Getting such name is harder, but it's more
	//! reliable when there are multiple assemblies loaded with the same file name.
	//!
	//! @param name Full name of the assembly.
	VIRTUAL_API virtual const IMonoAssembly *GetAssemblyFullName(const char *name) = 0;
};