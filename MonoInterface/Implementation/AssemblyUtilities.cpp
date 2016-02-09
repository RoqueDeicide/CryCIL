#include "stdafx.h"

#if 1
#define UtilityMessage CryLogAlways
#else
#define UtilityMessage(...) void(0)
#endif

Pair<Text *, Text *> GetAssemblyNames(MonoImage *image)
{
	UtilityMessage("Getting names.");

	MonoAssemblyName *aname =
		mono_assembly_name_new("DummyName, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

	UtilityMessage("Created a dummmy name.");

	mono_assembly_fill_assembly_name(image, aname);

	UtilityMessage("Filled the assembly name from the image.");

	char *fullNameNt = mono_stringify_assembly_name(aname);
	char *nameNt = const_cast<char *>(mono_assembly_name_get_name(aname));

	UtilityMessage("Got null-terminated versions of name and a full name.");

	//mono_assembly_name_free(aname);

	UtilityMessage("Released internals of the name object.");

	mono_free(aname);

	UtilityMessage("Released the name object.");

	Text *fullNameText = new Text(fullNameNt);
	Text *nameText = new Text(nameNt);

	UtilityMessage("Created Text versions of names.");

	mono_free(fullNameNt);
	//mono_free(nameNt);

	UtilityMessage("Released null-terminated versions.");

	return Pair<Text *, Text *>(fullNameText, nameText);
}