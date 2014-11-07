#include "stdafx.h"
#include "DirectoryStructure.h"

const char *DirectoryStructure::GetMonoConfigurationFolder()
{
	ConstructiveText *path = new ConstructiveText();
	*path
		<< BIN32_FOLDER       << PATH_SEPARATOR
		<< MODULES_FOLDER     << PATH_SEPARATOR
		<< CRYCIL_FOLDER      << PATH_SEPARATOR
		<< MONO_FOLDER        << PATH_SEPARATOR
		<< MONO_CONFIG_FOLDER << PATH_SEPARATOR;
	const char *result = path->ToNTString();
	delete path;
	return result;
}
//! Returns a path to the folder that contains Mono libraries.
const char *DirectoryStructure::GetMonoLibraryFolder()
{
	ConstructiveText *path = new ConstructiveText();
	*path
		<< BIN32_FOLDER     << PATH_SEPARATOR
		<< MODULES_FOLDER   << PATH_SEPARATOR
		<< CRYCIL_FOLDER    << PATH_SEPARATOR
		<< MONO_FOLDER      << PATH_SEPARATOR
		<< MONO_LIBS_FOLDER << PATH_SEPARATOR;
	const char *result = path->ToNTString();
	delete path;
	return result;
}
//! Returns a path to the folder that contains CryCIL libraries.
const char *DirectoryStructure::GetMonoBinariesFolder()
{
	ConstructiveText *path = new ConstructiveText();
	*path
		<< BIN32_FOLDER    << PATH_SEPARATOR
		<< MODULES_FOLDER  << PATH_SEPARATOR
		<< CRYCIL_FOLDER   << PATH_SEPARATOR;
	const char *result = path->ToNTString();
	delete path;
	return result;
}
//! Returns a path to the file that contains Cryambly.
const char *DirectoryStructure::GetCryamblyFile()
{
	ConstructiveText *path = new ConstructiveText();
	*path
		<< BIN32_FOLDER   << PATH_SEPARATOR
		<< MODULES_FOLDER << PATH_SEPARATOR
		<< CRYCIL_FOLDER  << PATH_SEPARATOR
		<< CRYAMBLY_FILE;
	const char *result = path->ToNTString();
	delete path;
	return result;
}
//! Returns a path to the file that contains Pdb to Mdb converter.
const char *DirectoryStructure::GetPdb2MdbFile()
{
	ConstructiveText *path = new ConstructiveText();
	*path
		<< BIN32_FOLDER            << PATH_SEPARATOR
		<< MODULES_FOLDER          << PATH_SEPARATOR
		<< CRYCIL_FOLDER           << PATH_SEPARATOR
		<< MONO_DEBUG_UTILITY_FILE;
	const char *result = path->ToNTString();
	delete path;
	return result;
}
//! Returns a path to the file that contains configuration data for 4.5 version AppDomains.
const char *DirectoryStructure::GetMonoAppDomainConfigurationFile()
{
	ConstructiveText *path = new ConstructiveText();
	*path
		<< BIN32_FOLDER       << PATH_SEPARATOR
		<< MODULES_FOLDER     << PATH_SEPARATOR
		<< CRYCIL_FOLDER      << PATH_SEPARATOR
		<< MONO_FOLDER        << PATH_SEPARATOR
		<< MONO_CONFIG_FOLDER << PATH_SEPARATOR
		<< "mono"             << PATH_SEPARATOR
		<< "4.5"              << PATH_SEPARATOR
		<< "machine.config";
	const char *result = path->ToNTString();
	delete path;
	return result;
}