#include "stdafx.h"

#include "ArchiveStream.h"

void ArchiveStreamInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(UpdateFile);
}

void ArchiveStreamInterop::UpdateFile(mono::object obj, const char *path, byte* data, uint length)
{
	ICryArchive *arch = *GET_BOXED_OBJECT_DATA(ICryArchive *, obj);

	arch->UpdateFile(path, data, length);
}
