#include "stdafx.h"
#include "DirectoryStructure.h"

const char *PathUtilities::Combine(std::vector<const char *> parts)
{
	std::stringstream resultantPath;
	int substringStart = 0;
	int substringEnd = 0;
	int separatorLength = strlen(PATH_SEPARATOR);
	for (int i = 0; i < parts.size(); i++)
	{
		int partLength = strlen(parts[i]);
		// Ignore directory name separators.
		// Separators at the first and last part are not checked.
		if (i != parts.size() && EndsWith(parts[i], PATH_SEPARATOR))
		{
			substringEnd = partLength - separatorLength;
		}
		if (i != 0 && StartsWith(parts[i], PATH_SEPARATOR))
		{
			substringStart = separatorLength;
		}

		resultantPath << std::string(parts[i]).substr(substringStart, substringEnd - substringStart);
		if (i != parts.size())
		{
			resultantPath << PATH_SEPARATOR;
		}
	}

	return resultantPath.str().c_str();
}

const char *DirectoryStructure::GetMonoConfigurationFolder()
{
	std::vector<const char *> names(5);
	names.push_back(BINARIES_FOLDER);
	names.push_back(MODULES_FOLDER);
	names.push_back(CRYCIL_FOLDER);
	names.push_back(MONO_FOLDER);
	names.push_back(MONO_CONFIG_FOLDER);
	return PathUtilities::Combine(names);
}
//! Returns a path to the folder that contains Mono libraries.
const char *DirectoryStructure::GetMonoLibraryFolder()
{
	std::vector<const char *> names(5);
	names.push_back(BINARIES_FOLDER);
	names.push_back(MODULES_FOLDER);
	names.push_back(CRYCIL_FOLDER);
	names.push_back(MONO_FOLDER);
	names.push_back(MONO_LIBS_FOLDER);
	return PathUtilities::Combine(names);
}
//! Returns a path to the folder that contains CryCIL libraries.
const char *DirectoryStructure::GetMonoBinariesFolder()
{
	std::vector<const char *> names(3);
	names.push_back(BINARIES_FOLDER);
	names.push_back(MODULES_FOLDER);
	names.push_back(CRYCIL_FOLDER);
	return PathUtilities::Combine(names);
}
//! Returns a path to the file that contains Cryambly.
const char *DirectoryStructure::GetCryamblyFile()
{
	std::vector<const char *> names(4);
	names.push_back(BINARIES_FOLDER);
	names.push_back(MODULES_FOLDER);
	names.push_back(CRYCIL_FOLDER);
	names.push_back("Cryambly.dll");
	return PathUtilities::Combine(names);
}
//! Returns a path to the file that contains Pdb to Mdb converter.
const char *DirectoryStructure::GetPdb2MdbFile()
{
	std::vector<const char *> names(4);
	names.push_back("Bin32");
	names.push_back(MODULES_FOLDER);
	names.push_back(CRYCIL_FOLDER);
	names.push_back(MONO_DEBUG_UTILITY_FILE);
	return PathUtilities::Combine(names);
}
//! Returns a path to the file that contains configuration data for 4.5 version AppDomains.
const char *DirectoryStructure::GetMonoAppDomainConfigurationFile()
{
	std::vector<const char *> names(8);
	names.push_back(BINARIES_FOLDER);
	names.push_back(MODULES_FOLDER);
	names.push_back(CRYCIL_FOLDER);
	names.push_back(MONO_FOLDER);
	names.push_back(MONO_CONFIG_FOLDER);
	names.push_back("mono");
	names.push_back("4.5");
	names.push_back("machine.config");
	return PathUtilities::Combine(names);
}