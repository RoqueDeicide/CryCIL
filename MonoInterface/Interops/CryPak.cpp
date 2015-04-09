#include "stdafx.h"

#include "CryPak.h"

void CryPakInterop::OnRunTimeInitialized()
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

bool CryPakInterop::Exists(mono::string path, ICryPak::EFileSearchLocation location)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->IsFileExist(NtText(path), location);
	}
	return false;
}

bool CryPakInterop::IsFolder(mono::string path)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->IsFolder(NtText(path));
	}
	return false;
}

FILE *CryPakInterop::Open(mono::string path, const char *modeSymbols, int flags)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->FOpen(NtText(path), modeSymbols, flags);
	}
	return nullptr;
}

void CryPakInterop::Close(FILE *file)
{
	if (gEnv && gEnv->pCryPak && file)
	{
		gEnv->pCryPak->FClose(file);
	}
}

uint CryPakInterop::GetSize(FILE *file)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->FGetSize(file);
	}
	return 0;
}

int CryPakInterop::GetCurrentPosition(FILE *file)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->FTell(file);
	}
	return -1;
}

int CryPakInterop::Seek(FILE *file, int offset, int origin)
{
	if (gEnv && gEnv->pCryPak)
	{
		return gEnv->pCryPak->FSeek(file, offset, origin);
	}
	return -1;
}

void CryPakInterop::Flush(FILE *file)
{
	if (gEnv && gEnv->pCryPak)
	{
		gEnv->pCryPak->FFlush(file);
	}
}

int CryPakInterop::ReadBytes(FILE *file, mono::Array bytes, int offset, int count)
{
	if (gEnv && gEnv->pCryPak)
	{
		MonoGCHandle arrayPin = MonoEnv->GC->Pin(bytes);
		return gEnv->pCryPak->FReadRaw(&IMonoArray<unsigned char>(bytes)[0] + offset, 1, count, file);
	}
	return -1;
}

int CryPakInterop::WriteBytes(FILE *file, mono::Array bytes, int offset, int count)
{
	if (gEnv && gEnv->pCryPak)
	{
		MonoGCHandle arrayPin = MonoEnv->GC->Pin(bytes);
		return gEnv->pCryPak->FWrite(&IMonoArray<unsigned char>(bytes)[0] + offset, 1, count, file);
	}
	return -1;
}
