#include "stdafx.h"

Pair<Text *, Text *> GetAssemblyNames(MonoImage *image)
{
	MonoAssemblyName *aname =
		mono_assembly_name_new("DummyName, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

	mono_assembly_fill_assembly_name(image, aname);

	char *fullNameNt = mono_stringify_assembly_name(aname);
	char *nameNt = const_cast<char *>(mono_assembly_name_get_name(aname));

	mono_assembly_name_free(aname);
	mono_free(aname);

	Text *fullNameText = new Text(fullNameNt);
	Text *nameText = new Text(nameNt);

	mono_free(fullNameNt);
	mono_free(nameNt);

	return Pair<Text *, Text *>(fullNameText, nameText);
}