/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IActionListener mono extension
//////////////////////////////////////////////////////////////////////////
// 02/09/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#include <IMonoInterop.h>

#include "MonoCommon.h"

class ScriptTableInterop
	: public IMonoInterop
{
public:
	ScriptTableInterop();
	~ScriptTableInterop() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "ScriptTableInterop"; }
	// ~IMonoScriptBind

private:
	static IScriptTable *GetScriptTable(IEntity *pEntity);
	static IScriptTable *GetSubScriptTable(IScriptTable *pScriptTable, mono::string subTableName);

	static IMonoObject *CallMethod(IScriptTable *pScriptTable, mono::string methodName, IMonoObject *params);
	static IMonoObject *GetValue(IScriptTable *pScriptTable, mono::string keyName);

	static bool ExecuteBuffer(mono::string buffer);
};
