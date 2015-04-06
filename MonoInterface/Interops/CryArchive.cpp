#include "stdafx.h"

#include "CryArchive.h"

void CryArchiveInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(get_Flags);
	REGISTER_METHOD(set_Flags);
	REGISTER_METHOD(OpenArchive);
	REGISTER_METHOD(FindFile);
	REGISTER_METHOD(GetFileSize);
	REGISTER_METHOD(ReadFile);
	REGISTER_METHOD(RemoveFile);
	REGISTER_METHOD(RemoveFilePtr);
	REGISTER_METHOD(RemoveDirectory);
	REGISTER_METHOD(ClearAll);
	REGISTER_METHOD(CloseArchive);
	REGISTER_METHOD(GetFullPath);
}

uint CryArchiveInterop::get_Flags(mono::object obj)
{
	ICryArchive *archive = *GET_BOXED_OBJECT_DATA(ICryArchive *, obj);

	return archive->GetFlags();
}

void CryArchiveInterop::set_Flags(mono::object obj, uint _value)
{
	ICryArchive *archive = *GET_BOXED_OBJECT_DATA(ICryArchive *, obj);

	archive->SetFlags(_value);
}

ICryArchive *CryArchiveInterop::OpenArchive(const char *path, ICryArchive::EPakFlags flags)
{
	if (!gEnv || !gEnv->pCryPak)
	{
		return nullptr;
	}
	auto archive = gEnv->pCryPak->OpenArchive(path, flags);

	if (archive)
	{
		archive->AddRef();
	}

	return archive;
}

void *CryArchiveInterop::FindFile(ICryArchive *archive, const char *szPath)
{
	return archive->FindFile(szPath);
}

uint CryArchiveInterop::GetFileSize(ICryArchive *archive, void *handle)
{
	return archive->GetFileSize(handle);
}

int CryArchiveInterop::ReadFile(ICryArchive *archive, void *handle, void *pBuffer)
{
	return archive->ReadFile(handle, pBuffer);
}

void CryArchiveInterop::RemoveFile(ICryArchive *archive, mono::string path)
{
	archive->RemoveFile(NtText(path));
}

void CryArchiveInterop::RemoveFilePtr(ICryArchive *archive, const char *path)
{
	archive->RemoveFile(path);
}

void CryArchiveInterop::RemoveDirectory(ICryArchive *archive, mono::string path)
{
	archive->RemoveDir(NtText(path));
}

void CryArchiveInterop::ClearAll(ICryArchive *archive)
{
	archive->RemoveAll();
}

void CryArchiveInterop::CloseArchive(ICryArchive *archive)
{
	archive->Release();
}

mono::string CryArchiveInterop::GetFullPath(ICryArchive *archive)
{
	return ToMonoString(archive->GetFullPath());
}
