/*************************************************************************
Crytek Source File.
Copyright (C), Crytek Studios, 2001-2004.
-------------------------------------------------------------------------
StringUtils.cpp
*************************************************************************/

#include "StdAfx.h"
#include "StringUtils.h"
#include "CryWatch.h"
//#include "HUD/HUDUtils.h"

static bool s_stringUtils_assertEnabled = true;



//--------------------------------------------------------------------------------
bool cry_strncpy(char * destination, const char * source, size_t bufferLength)
{
	bool reply = false;

	CRY_ASSERT(destination);
	CRY_ASSERT(source);

	if (bufferLength)
	{
		size_t i;
		for (i = 0; source[i] && (i + 1) < bufferLength; ++ i)
		{
			destination[i] = source[i];
		}
		destination[i] = '\0';
		reply = (source[i] == '\0');
	}

	CRY_ASSERT_MESSAGE(reply || !s_stringUtils_assertEnabled, string().Format("String '%s' is too big to fit into a buffer of length %u", source, (unsigned int) bufferLength));

	return reply;
}

//--------------------------------------------------------------------------------
bool cry_wstrncpy(wchar_t * destination, const wchar_t * source, size_t bufferLength)
{
	bool reply = false;

	CRY_ASSERT(destination);
	CRY_ASSERT(source);

	if (bufferLength)
	{
		size_t i;
		for (i = 0; source[i] && (i + 1) < bufferLength; ++ i)
		{
			destination[i] = source[i];
		}
		destination[i] = '\0';
		reply = (source[i] == '\0');
	}

	CRY_ASSERT_MESSAGE(reply || !s_stringUtils_assertEnabled, string().Format("String '%ls' is too big to fit into a buffer of length %u", source, (unsigned int) bufferLength));

	return reply;
}

//--------------------------------------------------------------------------------
void cry_sprintf(char * destination, size_t size, const char* format, ...)
{
	va_list args;
	va_start(args,format);
	int  ret = vsnprintf(destination, size, format, args);
	va_end(args);
	destination[size - 1] = 0;
	CRY_ASSERT_MESSAGE((ret < (int)size), string().Format("String '%s' was truncated to fit buffer of length %u", destination, (unsigned int) size));
}

//--------------------------------------------------------------------------------
size_t cry_copyStringUntilFindChar(char * destination, const char * source, size_t bufferLength, char until)
{
	size_t reply = 0;

	CRY_ASSERT(destination);
	CRY_ASSERT(source);

	if (bufferLength)
	{
		size_t i;
		for (i = 0; source[i] && source[i] != until && (i + 1) < bufferLength; ++ i)
		{
			destination[i] = source[i];
		}
		destination[i] = '\0';
		reply = (source[i] == until) ? (i + 1) : 0;
	}

	return reply;
}

//--------------------------------------------------------------------------------

#ifndef cry_displayMemInHexAndAscii
void cry_displayMemInHexAndAscii(const char * startEachLineWith, const void * data, int size, ITextOutputHandler & output, const int bytesPerLine)
{
	if (size > 0 && data)
	{
		const unsigned char * charData = (const unsigned char *) data;
		string hexLine, asciiLine;
		int bytesOnThisLineSoFar = 0;
		int padTo = 0;

		while (size)
		{
			if (++ bytesOnThisLineSoFar > bytesPerLine)
			{
				output.DoOutput(string().Format("%s%s  %s", startEachLineWith, hexLine.c_str(), asciiLine.c_str()));
				padTo = hexLine.length();
				bytesOnThisLineSoFar -= bytesPerLine;
				asciiLine = "";
				hexLine = "";
			}

			hexLine = hexLine + string().Format("%02x", (int) * charData);

			if ((bytesOnThisLineSoFar & 7) == 0 && (bytesOnThisLineSoFar != bytesPerLine))
			{
				hexLine += " ";
			}

			asciiLine = asciiLine + string().Format("%c", (*charData >= 32 && *charData != 127) ? *charData : '.');

			++ charData;
			-- size;
		}

		output.DoOutput(string().Format("%s%s%s  %s", startEachLineWith, hexLine.c_str(), padTo ? string(' ', padTo - hexLine.length()).c_str() : "", asciiLine.c_str()));
	}
	else
	{
		output.DoOutput(string().Format("%sPTR=%p SIZE=%d", startEachLineWith, size));
	}
}

//---------------------------------------------------------------------
void CCryWatchOutputHandler::DoOutput(const char * text)
{
	CryWatch ("%s", text);
}

//---------------------------------------------------------------------
void CCryLogOutputHandler::DoOutput(const char * text)
{
	CryLog ("%s", text);
}

//---------------------------------------------------------------------
void CCryLogAlwaysOutputHandler::DoOutput(const char * text)
{
	CryLogAlways ("%s", text);
}
#endif

//---------------------------------------------------------------------
void StrToWstr(const char* str, wstring& dstr)
{
	CryStackStringT<wchar_t, 64> tmp;
	tmp.resize(strlen(str));
	tmp.clear();

	while (const wchar_t c=(wchar_t)(*str++))
	{
		tmp.append(1, c);
	}

	dstr.assign(tmp.data(), tmp.length());
}