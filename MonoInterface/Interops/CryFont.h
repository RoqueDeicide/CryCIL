#pragma once

#include "IMonoInterface.h"

struct CryFontInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return ""; }
	virtual const char *GetNameSpace() { return ""; }

	virtual void OnRunTimeInitialized();

	static void Ctor(mono::object obj, mono::string name);
	static void Release(mono::object obj);

	static mono::string get_LoadedFonts();

	static bool         LoadXml(mono::object obj, mono::string xmlFile);
	static bool         LoadTtf(mono::object obj, mono::string ttfFile, int width, int height,
								uint smoothing);
	static void         Free(mono::object obj);
	static Vec2         GetSize(mono::object obj, mono::string text, bool multiLine,
								STextDrawContext *context);
	static int          GetLength(mono::object obj, mono::string text, bool multiLine);
	static mono::string WrapText(mono::object obj, float maxWidth, mono::string text,
								 STextDrawContext *context);
	static uint         GetEffectIndex(mono::object obj, mono::string name);
	static void         DrawTextInternal(mono::object obj, float x, float y, float z, mono::string text,
										 bool asciiMultiLine, STextDrawContext *context);
};