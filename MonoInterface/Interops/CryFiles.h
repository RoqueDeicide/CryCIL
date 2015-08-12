#pragma once

#include "IMonoInterface.h"

struct CryFilesInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "CryFiles"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Files"; }

	virtual void OnRunTimeInitialized() override;

	static bool Exists(mono::string path, ICryPak::EFileSearchLocation location);
	static bool IsFolder(mono::string path);
	static FILE *Open(mono::string path, const char *modeSymbols, int flags);
	static void Close(FILE *file);
	static uint GetSize(FILE *file);
	static int GetCurrentPosition(FILE *file);
	static int Seek(FILE *file, int offset, int origin);
	static void Flush(FILE *file);
	static int ReadBytes(FILE *file, mono::Array bytes, int offset, int count);
	static int WriteBytes(FILE *file, mono::Array bytes, int offset, int count);
};