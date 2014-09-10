#include "stdafx.h"
#include "ScriptTable.h"

#include "MonoScriptSystem.h"

#include <IEntitySystem.h>

#include <IMonoClass.h>

CScriptbind_ScriptTable::CScriptbind_ScriptTable()
{
	REGISTER_METHOD(GetScriptTable);
	REGISTER_METHOD(GetSubScriptTable);

	REGISTER_METHOD(CallMethod);
	REGISTER_METHOD(GetValue);

	REGISTER_METHOD(ExecuteBuffer);
}

IScriptTable *CScriptbind_ScriptTable::GetScriptTable(IEntity *pEntity)
{
	return pEntity->GetScriptTable();
}

ScriptAnyValue GetAnyValue(IMonoObject *pObject)
{
	switch (pObject->GetType())
	{
	case eMonoAnyType_String:
		return ScriptAnyValue(ToCryString((mono::string)pObject->GetManagedObject()));
	case eMonoAnyType_Float:
	case eMonoAnyType_Short:
	case eMonoAnyType_Integer:
		return ScriptAnyValue(pObject->Unbox<float>());
	case eMonoAnyType_Boolean:
		return ScriptAnyValue(pObject->Unbox<bool>());
	case eMonoAnyType_Vec3:
		return ScriptAnyValue(pObject->Unbox<Vec3>());
	}

	return ScriptAnyValue();
}

mono::object ToMonoObject(ScriptAnyValue anyValue)
{
	switch (anyValue.type)
	{
	case ANY_TSTRING:
		return (mono::object)ToMonoString(anyValue.str);
	case ANY_TNUMBER:
		return (mono::object)mono_value_box(mono_domain_get(), mono_get_single_class(), &anyValue.number);
	case ANY_TBOOLEAN:
		return (mono::object)mono_value_box(mono_domain_get(), mono_get_boolean_class(), &anyValue.b);
	case ANY_TVECTOR:
	{
						IMonoClass *pVec3Class = GetMonoScriptSystem()->GetCryBraryAssembly()->GetClass("Vec3");

						Vec3 vec(anyValue.vec3.x, anyValue.vec3.y, anyValue.vec3.z);
						return pVec3Class->BoxObject(&vec);
	}
	case ANY_TTABLE:
		return (mono::object)anyValue.table;
	default:
		break;
	}

	return nullptr;
}

mono::object CScriptbind_ScriptTable::CallMethod(IScriptTable *pScriptTable, mono::string methodName, mono::object params)
{
	HSCRIPTFUNCTION scriptFunction = 0;
	if (pScriptTable && pScriptTable->GetValue(ToCryString(methodName), scriptFunction))
	{
		if (!gEnv->pScriptSystem->BeginCall(scriptFunction))
			return nullptr;

		IMonoArray *pArgs = *params;

		gEnv->pScriptSystem->PushFuncParam(pScriptTable);

		for (int i = 0; i < pArgs->GetSize(); i++)
		{
			IMonoObject *pItem = *pArgs->GetItem(i);

			auto anyValue = GetAnyValue(pItem);
			gEnv->pScriptSystem->PushFuncParamAny(anyValue);

			SAFE_RELEASE(pItem);
		}

		pArgs->Release();

		ScriptAnyValue ret;
		gEnv->pScriptSystem->EndCallAny(ret);
		auto result = ToMonoObject(ret);

		gEnv->pScriptSystem->ReleaseFunc(scriptFunction);
		return result;
	}

	return nullptr;
}

mono::object CScriptbind_ScriptTable::GetValue(IScriptTable *pScriptTable, mono::string keyName)
{
	ScriptAnyValue anyValue;
	if (pScriptTable->GetValueAny(ToCryString(keyName), anyValue))
		return ToMonoObject(anyValue);

	return nullptr;
}

IScriptTable *CScriptbind_ScriptTable::GetSubScriptTable(IScriptTable *pScriptTable, mono::string subTableName)
{
	ScriptAnyValue anyValue;
	if (pScriptTable->GetValueAny(ToCryString(subTableName), anyValue))
		return anyValue.table;

	return nullptr;
}

bool CScriptbind_ScriptTable::ExecuteBuffer(mono::string mBuffer)
{
	if (IScriptSystem *pScriptSystem = gEnv->pSystem->GetIScriptSystem())
	{
		const char *buffer = ToCryString(mBuffer);
		return pScriptSystem->ExecuteBuffer(buffer + 1, strlen(buffer) - 1);
	}

	return false;
}