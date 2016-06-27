#include "stdafx.h"
#include "DirectoryStructure.h"

Text DirectoryStructure::monoConfigFolder;
Text DirectoryStructure::monoLibFolder;
Text DirectoryStructure::cryCilBinFolder;
Text DirectoryStructure::cryamblyFile;
Text DirectoryStructure::pdb2mdbFile;
Text DirectoryStructure::appDomainConfigFile;

const char *DirectoryStructure::GetMonoConfigurationFolder()
{
	if (monoConfigFolder.Empty)
	{
		monoConfigFolder = BuildPath({ MonoEnv->ExePath, MODULES_FOLDER, CRYCIL_FOLDER, MONO_FOLDER,
									   MONO_CONFIG_FOLDER }, false);
	}
	return monoConfigFolder;
}
//! Returns a path to the folder that contains Mono libraries.
const char *DirectoryStructure::GetMonoLibraryFolder()
{
	if (monoLibFolder.Empty)
	{
		monoLibFolder = BuildPath({ MonoEnv->ExePath, MODULES_FOLDER, CRYCIL_FOLDER, MONO_FOLDER,
									MONO_LIBS_FOLDER }, false);
	}
	return monoLibFolder;
}
//! Returns a path to the folder that contains CryCIL libraries.
const char *DirectoryStructure::GetCryCilBinariesFolder()
{
	if (cryCilBinFolder.Empty)
	{
		cryCilBinFolder = BuildPath({ MonoEnv->ExePath, MODULES_FOLDER, CRYCIL_FOLDER}, false);
	}
	return cryCilBinFolder;
}
//! Returns a path to the file that contains Cryambly.
const char *DirectoryStructure::GetCryamblyFile()
{
	if (cryamblyFile.Empty)
	{
		cryamblyFile = BuildPath({ MonoEnv->ExePath, MODULES_FOLDER, CRYCIL_FOLDER, CRYAMBLY_FILE }, true);
	}
	return cryamblyFile;
}
//! Returns a path to the file that contains Pdb to Mdb converter.
const char *DirectoryStructure::GetPdb2MdbFile()
{
	if (pdb2mdbFile.Empty)
	{
		pdb2mdbFile = BuildPath({ MonoEnv->ExePath, MODULES_FOLDER, CRYCIL_FOLDER,
								  MONO_DEBUG_UTILITY_FILE }, true);
	}
	return pdb2mdbFile;
}
//! Returns a path to the file that contains configuration data for 4.5 version AppDomains.
const char *DirectoryStructure::GetMonoAppDomainConfigurationFile()
{
	if (appDomainConfigFile.Empty)
	{
		appDomainConfigFile = BuildPath({ MonoEnv->ExePath, MODULES_FOLDER, CRYCIL_FOLDER, MONO_FOLDER,
										  MONO_CONFIG_FOLDER, "mono", "4.5", "machine.config" }, true);
	}
	return appDomainConfigFile;
}