#pragma once

#include <stdarg.h>

#include <sstream>

#if defined(PS3) || defined(LINUX) || defined(APPLE) || defined(ORBIS)
#define PATH_SEPARATOR "/"
#else
#define PATH_SEPARATOR "\\"
#endif // _WINDOWS

#ifdef WIN32
#define BINARIES_FOLDER         "Bin32"
#else
#define BINARIES_FOLDER         "Bin64"
#endif // WIN32

#define MODULES_FOLDER          "Modules"
#define CRYCIL_FOLDER           "CryCIL"
#define MONO_FOLDER             "Mono"
#define MONO_LIBS_FOLDER        "lib"
#define MONO_CONFIG_FOLDER      "etc"
#define MONO_DEBUG_UTILITY_FILE "pdb2mdb.dll"

//! Determines whether given text starts with given prefix.
//!
//! @param text   Text to check for prefix.
//! @param prefix Prefix which presence to check at the start of the text.
inline bool StartsWith(const char *text, const char *prefix)
{
	int textLength = strlen(text);
	int prefixLength = strlen(prefix);

	return textLength >= prefixLength && strncmp(text, prefix, prefixLength) == 0;
}
//! Determines whether given text ends with given postfix.
//!
//! @param text    Text to check for postfix.
//! @param postfix postfix which presence to check at the end of the text.
inline bool EndsWith(const char *text, const char *postfix)
{
	int textLength = strlen(text);
	int postfixLength = strlen(postfix);

	return textLength >= postfixLength &&
		strncmp(text + textLength - postfixLength, postfix, postfixLength) == 0;
}

//! Defines some simple utilities for working with paths.
struct PathUtilities
{
	//! Constructs a path from a collection of names.
	//!
	//! @param parts An array of names to combine.
	static const char *Combine(std::vector<const char *> parts);
};
//! Defines functions that return paths to various folders within CryEngine installation.
struct DirectoryStructure
{
	//! Returns a path to the folder that contains Mono configuration files.
	static const char *GetMonoConfigurationFolder();
	//! Returns a path to the folder that contains Mono libraries.
	static const char *GetMonoLibraryFolder();
	//! Returns a path to the folder that contains CryCIL libraries.
	static const char *GetMonoBinariesFolder();
	//! Returns a path to the file that contains Cryambly.
	static const char *GetCryamblyFile();
	//! Returns a path to the file that contains Pdb to Mdb converter.
	static const char *GetPdb2MdbFile();
	//! Returns a path to the file that contains configuration data for 4.5 version AppDomains.
	static const char *GetMonoAppDomainConfigurationFile();
};