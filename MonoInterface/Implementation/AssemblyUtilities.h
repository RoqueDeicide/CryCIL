#pragma once

#include "IMonoInterface.h"
#include "MonoAssemblies.h"

//! Gets full and short names of the given assembly.
//!
//! @param assembly  Assembly to get the names from.
//! @param fullName  An object that will contain the full name of the assembly.
//! @param shortName An object that will contain the short name of the assembly.
extern void GetAssemblyNames(MonoAssembly *assembly, Text &fullName, Text &shortName);

class AssemblyRegistry
{
	friend struct MonoAssemblies;

	//!< A list of objects that wrap Mono assemblies sorted by their short names and pointers to Mono objects.
	List<IMonoAssembly *> wrappers;
public:
	AssemblyRegistry();
	~AssemblyRegistry(){}

	//! Adds a wrapper to the registry.
	//!
	//! @param wrapper  An object that wraps a Mono assembly to add to this registry.
	//! @param previous Pointer to the pointer to the previous wrapper. Can be used to release resources.
	void Add(IMonoAssembly *wrapper, IMonoAssembly **previous = nullptr);
	//! Looks for an object that wraps given Mono assembly handle.
	//!
	//! @param handle Handle of the Mono assembly that may or may not wrapped by one of the objects in this
	//!               registry.
	//!
	//! @returns A pointer to the object that wraps given handle, if found, otherwise returns a null pointer.
	IMonoAssembly *Find(MonoAssembly *handle);
	//! Looks up a wrapper for an assembly with given name.
	//!
	//! @param name   Name of the assembly to look up.
	//! @param _short Indicates whether name is a short name rather then full one.
	IMonoAssembly *Find(const char *name, bool _short);
	//! Looks up a wrapper for an assembly with given name.
	//!
	//! @param aname Pointer to the object that specifies the name of the assembly.
	IMonoAssembly *Find(MonoAssemblyName *aname);

private:
	List<IMonoAssembly *>::iterator_type FindFirstWrapperWithShortName(const char *shortName);
};