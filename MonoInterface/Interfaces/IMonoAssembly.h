#pragma once

#include "IMonoAliases.h"

//! Defines interface of objects that wrap functionality of MonoAssembly type.
struct IMonoAssembly : public IMonoFunctionalityWrapper
{
	//! Gets the short name of the assembly.
	__declspec(property(get = GetName)) Text *Name;
	//! Gets the full name of the assembly.
	__declspec(property(get = GetFullName)) Text *FullName;
	//! Gets the full name of the assembly.
	__declspec(property(get = GetFileName)) Text *FileName;

	//! Gets the class.
	//!
	//! @param nameSpace Name space where the class is defined.
	//! @param className Name of the class to get.
	VIRTUAL_API virtual IMonoClass *GetClass(const char *nameSpace, const char *className) = 0;
	VIRTUAL_API virtual Text *GetName() = 0;
	VIRTUAL_API virtual Text *GetFullName() = 0;
	VIRTUAL_API virtual Text *GetFileName() = 0;
	//! Gets the reference to the instance of type System.Reflection.Assembly.
	__declspec(property(get = GetReflectionObject)) mono::assembly ReflectionObject;

	VIRTUAL_API virtual mono::assembly GetReflectionObject() = 0;
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
	VIRTUAL_API virtual IMonoAssembly *Load(const char *path) = 0;
	//! Wraps an assembly pointer.
	//!
	//! Mostly for internal use since acquiring MonoAssembly pointer requires direct work with Mono.
	//!
	//! @param assemblyHandle Pointer to MonoAssembly to wrap.
	VIRTUAL_API virtual IMonoAssembly *Wrap(void *assemblyHandle) = 0;
	//! Gets the pointer to the assembly wrapper object.
	//!
	//! If there are multiple assemblies loaded with the same file name, it's not guaranteed
	//! that you will get the one you want.
	//!
	//! @param name Name of the assembly (equivalent of the file name but without the extension).
	VIRTUAL_API virtual IMonoAssembly *GetAssembly(const char *name) = 0;
	//! Gets the pointer to the assembly wrapper object.
	//!
	//! Requires fully-qualified name of the assembly. Getting such name is harder, but it's more
	//! reliable when there are multiple assemblies loaded with the same file name.
	//!
	//! @param name Full name of the assembly.
	VIRTUAL_API virtual IMonoAssembly *GetAssemblyFullName(const char *name) = 0;
};