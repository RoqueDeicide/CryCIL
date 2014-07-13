/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Contains various path utils used for Mono startup.
//////////////////////////////////////////////////////////////////////////
// 20/11/2011 : Created by Filip 'i59' Lundgren (based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_PATH_UTILS_H__
#define __MONO_PATH_UTILS_H__

#define MONO_CONFIG_PATH "etc"
#define MONO_LIB_PATH "lib"

class PathUtils
{
public:
	static string GetBinaryPath(bool force32 = false)
	{
		string binPath = "Bin32\\";

#ifdef WIN64
		if (!force32)
			binPath = "Bin64\\";
#endif

		return binPath.append("Plugins\\CryMono\\");
	}

	// Path to directory which contains CryMono config & utils, e.g. Mono/ & Plugins/.
	static string GetConfigPath()
	{
		return GetBinaryPath(true);
	}

	static string GetEnginePath()
	{
		return "Engine\\";
	}

	static string GetMonoPath()
	{
		return GetConfigPath().append("Mono").append("\\");
	}

	static string GetMonoConfigPath()
	{
		return GetMonoPath().append(MONO_CONFIG_PATH).append("\\");
	}

	static string GetMonoLibPath()
	{
		return GetMonoPath().append(MONO_LIB_PATH).append("\\");
	}
};

#endif //__MONO_PATH_UTILS_H__