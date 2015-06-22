#include "stdafx.h"

#include "CryFont.h"

void CryFontInterop::OnRunTimeInitialized()
{
	REGISTER_CTOR(Ctor);

	REGISTER_METHOD(get_LoadedFonts);

	REGISTER_METHOD(LoadXml);
	REGISTER_METHOD(LoadTtf);
	REGISTER_METHOD(Free);
	REGISTER_METHOD(GetSize);
	REGISTER_METHOD(GetLength);
	REGISTER_METHOD(WrapText);
	REGISTER_METHOD(GetEffectIndex);
	REGISTER_METHOD(DrawTextInternal);
}

struct ManagedFontData
{
	IFFont *font;
	mono::string name;
};

void CryFontInterop::Ctor(mono::object obj, mono::string name)
{
	if (!gEnv || !gEnv->pCryFont)
	{
		CryEngineException("Cannot create a new font object: CryEngine is not loaded.").Throw();
	}
	if (!name)
	{
		ArgumentNullException("Name of the font cannot be null.").Throw();
	}

	NtText ntName(name);

	auto f = gEnv->pCryFont->GetFont(ntName);
	if (!f)
	{
		f = gEnv->pCryFont->NewFont(ntName);
	}
	if (!f)
	{
		CryEngineException("Unable to create a new font object.").Throw();
	}

	ManagedFontData data;
	data.font = f;
	data.name = name;

	*GET_BOXED_OBJECT_DATA(ManagedFontData, obj) = data;
}

void CryFontInterop::Release(mono::object obj)
{
	ManagedFontData *data = GET_BOXED_OBJECT_DATA(ManagedFontData, obj);

	if (data->font)
	{
		data->font->Release();
		data->font = nullptr;
	}
}

mono::string CryFontInterop::get_LoadedFonts()
{
	if (!gEnv || !gEnv->pCryFont)
	{
		CryEngineException("Cannot fetch the list of loaded fonts: CryEngine is not loaded.").Throw();
	}

	return ToMonoString(gEnv->pCryFont->GetLoadedFontNames().c_str());
}

bool CryFontInterop::LoadXml(mono::object obj, mono::string xmlFile)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!xmlFile)
	{
		return false;
	}
	
	return font->Load(NtText(xmlFile));
}

bool CryFontInterop::LoadTtf(mono::object obj, mono::string ttfFile, int width, int height, uint smoothing)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!ttfFile)
	{
		return false;
	}

	return font->Load(NtText(ttfFile), width, height, smoothing);
}

void CryFontInterop::Free(mono::object obj)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}

	font->Free();
}

Vec2 CryFontInterop::GetSize(mono::object obj, mono::string text, bool multiLine, STextDrawContext *context)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!text)
	{
		ArgumentNullException("Text which dimensions to calculate cannot be null").Throw();
	}

	return font->GetTextSize(NtText(text), multiLine, *context);
}

int CryFontInterop::GetLength(mono::object obj, mono::string text, bool multiLine)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!text)
	{
		ArgumentNullException("Text which length to calculate cannot be null").Throw();
	}

	return font->GetTextLength(NtText(text), multiLine);
}

mono::string CryFontInterop::WrapText(mono::object obj, float maxWidth, mono::string text, STextDrawContext *context)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!text)
	{
		return nullptr;
	}

	string res;
	font->WrapText(res, maxWidth, NtText(text), *context);
	return ToMonoString(res.c_str());
}

uint CryFontInterop::GetEffectIndex(mono::object obj, mono::string name)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!name)
	{
		ArgumentNullException("Name of the font effect cannot be null.").Throw();
	}

	return font->GetEffectId(NtText(name));
}

void CryFontInterop::DrawTextInternal(mono::object obj, float x, float y, float z, mono::string text, bool asciiMultiLine, STextDrawContext *context)
{
	auto font = *GET_BOXED_OBJECT_DATA(IFFont *, obj);

	if (!font)
	{
		ObjectDisposedException("Cannot use invalid font object.").Throw();
	}
	if (!text)
	{
		return;
	}

	font->DrawString(x, y, z, NtText(text), asciiMultiLine, *context);
}
