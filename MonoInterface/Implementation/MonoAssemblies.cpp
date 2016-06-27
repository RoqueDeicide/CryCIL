#include "stdafx.h"

#include "MonoAssemblies.h"
#include "MonoAssembly.h"
#include "ThunkTables.h"

MonoAssemblies::~MonoAssemblies()
{
	for (auto current : this->Registry.wrappers)
	{
		delete current;
	}
}

IMonoAssembly *MonoAssemblies::Load(const char *path)
{
	if (!path)
	{
		return nullptr;
	}

	MonoImageOpenStatus status;
	MonoAssembly       *assembly = mono_assembly_open(path, &status);

	if (status != MONO_IMAGE_OK)
	{
		return nullptr;
	}

	auto wrapper = Wrap(assembly);
	wrapper->SetFileName(path);
	return wrapper;
}

IMonoAssembly *MonoAssemblies::Wrap(void *assemblyHandle)
{
	if (!assemblyHandle)
	{
		return nullptr;
	}

	auto assembly = static_cast<MonoAssembly *>(assemblyHandle);
	IMonoAssembly *wrapper = this->Registry.Find(assembly);

	if (!wrapper)
	{
		// Register this wrapper.
		wrapper     = new MonoAssemblyWrapper(assembly);
		this->Registry.Add(wrapper);
	}
	return wrapper;
}

IMonoAssembly *MonoAssemblies::GetAssembly(const char *name)
{
	return this->Registry.Find(name, true);
}

IMonoAssembly *MonoAssemblies::GetAssemblyFullName(const char *name)
{
	return this->Registry.Find(name, false);
}


void MonoAssemblies::RegisterAssemblyHooks()
{
	mono_install_assembly_load_hook(MonoAssemblies::MonoLoadHook, this);
	mono_install_assembly_search_hook(MonoAssemblies::MonoSearchHook, this);
	mono_install_assembly_refonly_search_hook(MonoAssemblies::MonoSearchHook, this);
	mono_install_assembly_preload_hook(MonoAssemblies::MonoPreLoadHook, this);
	mono_install_assembly_refonly_preload_hook(MonoAssemblies::MonoPreLoadHook, this);
}

void MonoAssemblies::MonoLoadHook(MonoAssembly *assembly, void *user_data)
{
	MonoAssemblies *assemblies = static_cast<MonoAssemblies *>(user_data);
	if (!assemblies)
	{
		return;
	}
	
	assemblies->Wrap(assembly);
}

template<typename DataType>
ptrdiff_t MonoAssemblies::ReadFileToMemory(string path, DataType **data)
{
	// Try finding the file.
	_finddata_t fd;
	intptr_t handle = gEnv->pCryPak->FindFirst(path.c_str(), &fd, ICryPak::FLAGS_PATH_REAL, true);
	if (handle < 0)
	{
		return 0;
	}

	// Open the file.
	FILE *f = gEnv->pCryPak->FOpen(path, "rb", ICryPak::FLAGS_PATH_REAL);
	int size = gEnv->pCryPak->FGetSize(f);

	// Read all of the data into memory.
	*data = new DataType[size];
	gEnv->pCryPak->FRead(*data, size, f);
	gEnv->pCryPak->FClose(f);
	return size;
}

MonoAssembly *MonoAssemblies::MonoSearchHook(MonoAssemblyName *aname, void *user_data)
{
	auto assemblies = static_cast<MonoAssemblies *>(user_data);
	if (!assemblies || assemblies->NativeLookUpOnly)
	{
		return nullptr;
	}

	// Try to find the wrapper for the assembly.
	auto wrapper = assemblies->Registry.Find(aname);

	if (wrapper)
	{
		return wrapper->GetHandle<MonoAssembly>();
	}

	// The assembly is not loaded: try looking in .pak files.

	Text shortName = mono_assembly_name_get_name(aname);
	Text fileName = Text(shortName).Append(".dll");
	if (strcmp(shortName, "mscorlib") == 0)
	{
		// Don't try loading the mscorlib.
		return nullptr;
	}

	// Prefer to look in the project directory on the disk rather then .pak files.
	string path = PathUtil::Make(MonoEnv->ProjectPath, fileName);
	if (!gEnv->pCryPak->IsFileExist(path, ICryPak::eFileLocation_OnDisk))
	{
		path = PathUtil::Make(MonoEnv->ExePath, fileName);
	}

	// Try loading the file into memory.
	char *imageData = nullptr;
	auto imageSize = assemblies->ReadFileToMemory(path, &imageData);
	if (imageSize == 0)
	{
		return nullptr;
	}

	// Prevent infinite recursion.
	assemblies->NativeLookUpOnly = true;

	// Try loading the image from memory.
	MonoImageOpenStatus status = MONO_IMAGE_ERROR_ERRNO;
	MonoImage *image = mono_image_open_from_data_full(imageData, imageSize, false, &status, false);

#ifndef _RELEASE
	// Try loading the .mdb file for debugger.
	string debugFile = PathUtil::ReplaceExtension(path, "mdb");

	mono_byte *debugData = nullptr;
	auto debugSize = assemblies->ReadFileToMemory(debugFile, &debugData);
	if (debugSize > 0)
	{
		mono_debug_open_image_from_memory(image, debugData, debugSize);
	}
#endif // !_RELEASE

	// Load the assembly.
	auto assembly = mono_assembly_load_from_full(image, shortName, &status, false);

	// Probably should do some clean up here, if assembly fails to load...

	// Tell the wrapper where the data is, so it can delete it when unloaded.
	wrapper = assemblies->Wrap(assembly);

	wrapper->AssignData(imageData);
#ifndef _RELEASE
	wrapper->AssignDebugData(debugData);
#endif // _RELEASE

	assemblies->NativeLookUpOnly = false;

	return assembly;
}

MonoAssembly *MonoAssemblies::MonoPreLoadHook(MonoAssemblyName *, char **, void *)
{
	return nullptr;
}
