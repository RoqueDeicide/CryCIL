#include "stdafx.h"
#include "DirectoryStructure.h"

const char *DirectoryStructure::GetMonoConfigurationFolder()
{
	char s = PATH_SEPARATOR;
	return NtText
	(
		10,
		BIN32_FOLDER, s,
		MODULES_FOLDER, s,
		CRYCIL_FOLDER, s,
		MONO_FOLDER, s,
		MONO_CONFIG_FOLDER, s
	).Detach();
}
//! Returns a path to the folder that contains Mono libraries.
const char *DirectoryStructure::GetMonoLibraryFolder()
{
	char s = PATH_SEPARATOR;
	return NtText
	(
		10,
		BIN32_FOLDER, s,
		MODULES_FOLDER, s,
		CRYCIL_FOLDER, s,
		MONO_FOLDER, s,
		MONO_LIBS_FOLDER, s
	).Detach();
}
//! Returns a path to the folder that contains CryCIL libraries.
const char *DirectoryStructure::GetCryCilBinariesFolder()
{
	char s = PATH_SEPARATOR;
	return NtText
	(
		6,
		BIN32_FOLDER, s,
		MODULES_FOLDER, s,
		CRYCIL_FOLDER, s
	).Detach();
}
//! Returns a path to the file that contains Cryambly.
const char *DirectoryStructure::GetCryamblyFile()
{
	char s = PATH_SEPARATOR;
	return NtText
	(
		7,
		BIN32_FOLDER, s,
		MODULES_FOLDER, s,
		CRYCIL_FOLDER, s,
		CRYAMBLY_FILE
	).Detach();
}
//! Returns a path to the file that contains Pdb to Mdb converter.
const char *DirectoryStructure::GetPdb2MdbFile()
{
	char s = PATH_SEPARATOR;
	return NtText
	(
		7,
		BIN32_FOLDER, s,
		MODULES_FOLDER, s,
		CRYCIL_FOLDER, s,
		MONO_DEBUG_UTILITY_FILE
	).Detach();
}
//! Returns a path to the file that contains configuration data for 4.5 version AppDomains.
const char *DirectoryStructure::GetMonoAppDomainConfigurationFile()
{
	char s = PATH_SEPARATOR;
	return NtText
	(
		15,
		BIN32_FOLDER, s,
		MODULES_FOLDER, s,
		CRYCIL_FOLDER, s,
		MONO_FOLDER, s,
		MONO_CONFIG_FOLDER, s,
		"mono", s,
		"4.5", s,
		"machine.config"
	).Detach();
}