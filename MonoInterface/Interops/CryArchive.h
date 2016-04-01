#pragma once

#include "IMonoInterface.h"

struct CryArchiveInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryArchive"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Files"; }

	virtual void InitializeInterops() override;

	static uint get_Flags(mono::object obj);
	static void set_Flags(mono::object obj, uint _value);

	static ICryArchive *OpenArchive(const char *path, ICryArchive::EPakFlags flags);
	static void *FindFile(ICryArchive *archive, const char *szPath);
	static uint GetFileSize(ICryArchive *archive, void *handle);
	static int ReadFile(ICryArchive *archive, void *handle, void *pBuffer);
	static void RemoveFile(ICryArchive *archive, mono::string path);
	static void RemoveFilePtr(ICryArchive *archive, const char *path);
	static void RemoveDirectory(ICryArchive *archive, mono::string path);
	static void ClearAll(ICryArchive *archive);
	static void CloseArchive(ICryArchive *archive);
	static mono::string GetFullPath(ICryArchive *archive);
};