#include "stdafx.h"

#include "CryFiles.h"

void CryFilesInterop::InitializeInterops()
{
	REGISTER_METHOD(Exists);
	REGISTER_METHOD(IsFolder);
	REGISTER_METHOD(Open);
	REGISTER_METHOD(Close);
	REGISTER_METHOD(GetSize);
	REGISTER_METHOD(GetCurrentPosition);
	REGISTER_METHOD(Seek);
	REGISTER_METHOD(Flush);
	REGISTER_METHOD(ReadBytes);
	REGISTER_METHOD(WriteBytes);
}

bool CryFilesInterop::Exists(mono::string path, ICryPak::EFileSearchLocation location)
{
	if (gEnv && gEnv->pCryPak && path && location >= 0 && location < 3)
	{
		return gEnv->pCryPak->IsFileExist(NtText(path), location);
	}
	return false;
}

bool CryFilesInterop::IsFolder(mono::string path)
{
	if (gEnv && gEnv->pCryPak && path)
	{
		return gEnv->pCryPak->IsFolder(NtText(path));
	}
	return false;
}

FILE *CryFilesInterop::Open(mono::string path, const char *modeSymbols, int flags)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->FOpen(NtText(path), modeSymbols, flags);
	}
	return nullptr;
}

void CryFilesInterop::Close(FILE *file)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		gEnv->pCryPak->FClose(file);
	}
}

uint CryFilesInterop::GetSize(FILE *file)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		return gEnv->pCryPak->FGetSize(file);
	}
	return 0;
}

int CryFilesInterop::GetCurrentPosition(FILE *file)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		return gEnv->pCryPak->FTell(file);
	}
	return -1;
}

int CryFilesInterop::Seek(FILE *file, int offset, int origin)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		return gEnv->pCryPak->FSeek(file, offset, origin);
	}
	return -1;
}

void CryFilesInterop::Flush(FILE *file)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		gEnv->pCryPak->FFlush(file);
	}
}

int CryFilesInterop::ReadBytes(FILE *file, mono::Array bytes, int offset, int count)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		MonoGCHandle arrayPin = MonoEnv->GC->Pin(bytes);
		return gEnv->pCryPak->FReadRaw(&IMonoArray<unsigned char>(bytes)[0] + offset, 1, count, file);
	}
	return -1;
}

int CryFilesInterop::WriteBytes(FILE *file, mono::Array bytes, int offset, int count)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		MonoGCHandle arrayPin = MonoEnv->GC->Pin(bytes);
		return gEnv->pCryPak->FWrite(&IMonoArray<unsigned char>(bytes)[0] + offset, 1, count, file);
	}
	return -1;
}
