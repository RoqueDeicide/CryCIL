#pragma once

#include <stdarg.h>

#include <sstream>

#ifdef _WINDOWS
#define PATH_SEPARATOR "\\"
#else
#define PATH_SEPARATOR "/"
#endif // _WINDOWS

#ifdef WIN32
#define BINARIES_FOLDER          "Bin32"
#else
#define BINARIES_FOLDER          "Bin64"
#endif // WIN32

#define MODULES_FOLDER          "Modules"
#define CRYCIL_FOLDER           "CryCIL"
#define MONO_FOLDER             "Mono"
#define MONO_LIBS_FOLDER        "lib"
#define MONO_CONFIG_FOLDER      "etc"
#define MONO_DEBUG_UTILITY_FILE "pdb2mdb.dll"

//! Defines some simple utilities for working with paths.
struct PathUtilities
{
	//! Combines a bunch of folder and file names into a path.
	//! @remark Don't put any path separators into parts.
	//!
	//! @param parts Pointer to an array of parts to combine.
	//! @param count Number of parts in the array.
	static char *Combine(const char** parts, int count)
	{
		std::stringstream resultantPath;

		for (int i = 0; i < count; i++)
		{
			resultantPath << parts[i] << PATH_SEPARATOR;
		}

		return const_cast<char *>(resultantPath.str().c_str());
	}
};
//! Defines functions that return paths to various folders within CryEngine installation.
struct DirectoryStructure
{
	//! Returns a path to the folder that contains Mono configuration files.
	static char *GetMonoConfigurationFolder()
	{
		char* parts[5];
		parts[0] = BINARIES_FOLDER;
		parts[1] = MODULES_FOLDER;
		parts[2] = CRYCIL_FOLDER;
		parts[3] = MONO_FOLDER;
		parts[4] = MONO_CONFIG_FOLDER;
		return PathUtilities::Combine((const char **)&parts[0], 5);
	}
	//! Returns a path to the folder that contains Mono libraries.
	static char *GetMonoLibraryFolder()
	{
		char* parts[5];
		parts[0] = BINARIES_FOLDER;
		parts[1] = MODULES_FOLDER;
		parts[2] = CRYCIL_FOLDER;
		parts[3] = MONO_FOLDER;
		parts[4] = MONO_LIBS_FOLDER;
		return PathUtilities::Combine((const char **)&parts[0], 5);
	}
	//! Returns a path to the folder that contains CryCIL libraries.
	static char *GetMonoBinariesFolder()
	{
		char* parts[3];
		parts[0] = BINARIES_FOLDER;
		parts[1] = MODULES_FOLDER;
		parts[2] = CRYCIL_FOLDER;
		return PathUtilities::Combine((const char **)&parts[0], 3);
	}
	//! Returns a path to the file that contains Cryambly.
	static char *GetCryamblyFile()
	{
		char* parts[4];
		parts[0] = BINARIES_FOLDER;
		parts[1] = MODULES_FOLDER;
		parts[2] = CRYCIL_FOLDER;
		parts[3] = "Cryambly.dll";
		return PathUtilities::Combine((const char **)&parts[0], 4);
	}
	//! Returns a path to the file that contains Pdb to Mdb converter.
	static char *GetPdb2MdbFile()
	{
		char* parts[4];
		parts[0] = "Bin32";
		parts[1] = MODULES_FOLDER;
		parts[2] = CRYCIL_FOLDER;
		parts[3] = MONO_DEBUG_UTILITY_FILE;
		return PathUtilities::Combine((const char **)&parts[0], 4);
	}
	//! Returns a path to the file that contains configuration data for 4.5 version AppDomains.
	static char *GetMonoAppDomainConfigurationFile()
	{
		char* parts[8];
		parts[0] = BINARIES_FOLDER;
		parts[1] = MODULES_FOLDER;
		parts[2] = CRYCIL_FOLDER;
		parts[3] = MONO_FOLDER;
		parts[4] = MONO_CONFIG_FOLDER;
		parts[5] = "mono";
		parts[6] = "4.5";
		parts[7] = "machine.config";
		return PathUtilities::Combine((const char **)&parts[0], 8);
	}
};