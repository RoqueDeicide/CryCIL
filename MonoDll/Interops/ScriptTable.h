/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IActionListener mono extension
//////////////////////////////////////////////////////////////////////////
// 02/09/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/

#include <IMonoScriptBind.h>

#include "MonoCommon.h"

class CScriptbind_ScriptTable
	: public IMonoScriptBind
{
public:
	CScriptbind_ScriptTable();
	~CScriptbind_ScriptTable() {}

	// IMonoScriptBind
	virtual const char *GetClassName() { return "ScriptTableInterop"; }
	// ~IMonoScriptBind

private:
	static IScriptTable *GetScriptTable(IEntity *pEntity);
	static IScriptTable *GetSubScriptTable(IScriptTable *pScriptTable, mono::string subTableName);

	static mono::object CallMethod(IScriptTable *pScriptTable, mono::string methodName, mono::object params);
	static mono::object GetValue(IScriptTable *pScriptTable, mono::string keyName);

	static bool ExecuteBuffer(mono::string buffer);
};
