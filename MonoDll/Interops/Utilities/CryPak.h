/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// CryPak scriptbind; used for FS management
//////////////////////////////////////////////////////////////////////////
// 23/03/2013 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __CRYPAK_BINDING_H__
#define __CRYPAK_BINDING_H__

#include <MonoCommon.h>
#include <IMonoScriptBind.h>

class CScriptbind_CryPak : public IMonoScriptBind
{
public:
	CScriptbind_CryPak();
	virtual ~CScriptbind_CryPak() {}

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "CryPakInterop"; }
	// ~IMonoScriptBind

	static mono::string GetGameFolder();
	static void SetGameFolder(mono::string folder);

	static void SetAlias(mono::string name, mono::string alias, bool bAdd);
	static mono::string GetAlias(mono::string name, bool returnSame = true);

	static mono::string AdjustFileName(mono::string src, unsigned int flags);
};

#endif //__CRYPAK_BINDING_H__