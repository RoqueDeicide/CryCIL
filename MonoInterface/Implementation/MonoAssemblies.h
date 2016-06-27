#pragma once

#include "IMonoInterface.h"
#include "AssemblyUtilities.h"

struct MonoAssemblies : public IMonoAssemblies
{
	AssemblyRegistry Registry;         //!< A collection of wrappers for assemblies.
	bool             NativeLookUpOnly; //!< Indicates whether a search hook must avoid execution.

	MonoAssemblies() : NativeLookUpOnly(false)
	{
	}

	~MonoAssemblies();

	IMonoAssembly *Load(const char *path) override;
	IMonoAssembly *Wrap(void *assemblyHandle) override;
	IMonoAssembly *GetAssembly(const char *name) override;
	IMonoAssembly *GetAssemblyFullName(const char *name) override;

	void           RegisterAssemblyHooks();

	//! Invoked by Mono after loading a Mono assembly.
	//!
	//! @param assembly  Pointer to the object that represents the loaded assembly.
	//! @param user_data Pointer to this interface object.
	static void MonoLoadHook(MonoAssembly *assembly, void *user_data);
	//! Invoked by Mono to perform a search for an assembly with a given name.
	//!
	//! This function is require to allow assemblies to be loaded from .pak files.
	//!
	//! @param aname     Name of the assembly to look up.
	//! @param user_data Pointer to this interface object.
	//!
	//! @returns A pointer to the object that represents the assembly with provided name, that either was
	//!          loaded before or has just been loaded by this function from the .pak file.
	static MonoAssembly *MonoSearchHook(MonoAssemblyName *aname, void *user_data);
	//! Returns null.
	static MonoAssembly *MonoPreLoadHook(MonoAssemblyName *aname, char **assemblies_path, void *user_data);
private:
	template<typename DataType>
	ptrdiff_t ReadFileToMemory(string path, DataType **data);
};
