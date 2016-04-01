#include "stdafx.h"

#include "CrySync.h"
#include "TimeUtilities.h"

void CrySyncInterop::InitializeInterops()
{
	REGISTER_METHOD(FlagPartialRead);
	REGISTER_METHOD(StartGroup);
	REGISTER_METHOD(StartOptionalGroup);
	REGISTER_METHOD(FinishGroup);
	REGISTER_METHOD(IsReading);
	REGISTER_METHOD(GetSerializationTarget);

	REGISTER_METHOD(SyncString);
	REGISTER_METHOD(SyncStringDefault);
	REGISTER_METHOD(SyncBool);
	REGISTER_METHOD(SyncBoolDefault);
	REGISTER_METHOD(SyncSingle);
	REGISTER_METHOD(SyncSingleDefault);
	REGISTER_METHOD(SyncVec2);
	REGISTER_METHOD(SyncVec2Default);
	REGISTER_METHOD(SyncVec3);
	REGISTER_METHOD(SyncVec3Default);
	REGISTER_METHOD(SyncQuat);
	REGISTER_METHOD(SyncQuatDefault);
	REGISTER_METHOD(SyncAngles);
	REGISTER_METHOD(SyncAnglesDefault);
	REGISTER_METHOD(SyncInt8);
	REGISTER_METHOD(SyncInt8Default);
	REGISTER_METHOD(SyncInt16);
	REGISTER_METHOD(SyncInt16Default);
	REGISTER_METHOD(SyncInt32);
	REGISTER_METHOD(SyncInt32Default);
	REGISTER_METHOD(SyncInt64);
	REGISTER_METHOD(SyncInt64Default);
	REGISTER_METHOD(SyncUInt8);
	REGISTER_METHOD(SyncUInt8Default);
	REGISTER_METHOD(SyncUInt16);
	REGISTER_METHOD(SyncUInt16Default);
	REGISTER_METHOD(SyncUInt32);
	REGISTER_METHOD(SyncUInt32Default);
	REGISTER_METHOD(SyncUInt64);
	REGISTER_METHOD(SyncUInt64Default);
	REGISTER_METHOD(SyncTime);
	REGISTER_METHOD(SyncTimeDefault);
	REGISTER_METHOD(SyncXml);
}

void CrySyncInterop::FlagPartialRead(ISerialize *handle)
{
	handle->FlagPartialRead();
}

void CrySyncInterop::StartGroup(ISerialize *handle, const char *name)
{
	handle->BeginGroup(name);
}

bool CrySyncInterop::StartOptionalGroup(ISerialize *handle, const char *name, bool condition)
{
	return handle->BeginOptionalGroup(name, condition);
}

void CrySyncInterop::FinishGroup(ISerialize *handle)
{
	handle->EndGroup();
}

bool CrySyncInterop::IsReading(ISerialize *handle)
{
	return handle->IsReading();
}

int CrySyncInterop::GetSerializationTarget(ISerialize *handle)
{
	return handle->GetSerializationTarget();
}

void CrySyncInterop::SyncString(ISerialize *handle, const char *name, mono::string *value)
{
	TSerialize serialize = TSerialize(handle);
	
	NtText ntValue(*value);

	string strValue(ntValue);

	serialize.Value(name, strValue);
	
	if (handle->IsReading())
	{
		*value = ToMonoString(strValue);
	}
}

void CrySyncInterop::SyncStringDefault(ISerialize *handle, const char *name, mono::string *value, mono::string def)
{
	TSerialize serialize = TSerialize(handle);

	NtText ntValue(*value);
	NtText ntDef(def);

	string strValue(ntValue);
	string strDef(ntDef);

	serialize.ValueWithDefault(name, strValue, strDef);
	if (handle->IsReading())
	{
		*value = ToMonoString(strValue);
	}
}

void CrySyncInterop::SyncBool(ISerialize *handle, const char *name, bool *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncBoolDefault(ISerialize *handle, const char *name, bool *value, bool def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncSingle(ISerialize *handle, const char *name, float *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncSingleDefault(ISerialize *handle, const char *name, float *value, float def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncVec2(ISerialize *handle, const char *name, Vec2 *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncVec2Default(ISerialize *handle, const char *name, Vec2 *value, Vec2 def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncVec3(ISerialize *handle, const char *name, Vec3 *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncVec3Default(ISerialize *handle, const char *name, Vec3 *value, Vec3 def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncQuat(ISerialize *handle, const char *name, Quat *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncQuatDefault(ISerialize *handle, const char *name, Quat *value, Quat def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncAngles(ISerialize *handle, const char *name, Ang3 *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncAnglesDefault(ISerialize *handle, const char *name, Ang3 *value, Ang3 def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncInt8(ISerialize *handle, const char *name, signed char *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncInt8Default(ISerialize *handle, const char *name, signed char *value, signed char def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncInt16(ISerialize *handle, const char *name, short *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncInt16Default(ISerialize *handle, const char *name, short *value, short def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncInt32(ISerialize *handle, const char *name, int *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncInt32Default(ISerialize *handle, const char *name, int *value, int def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncInt64(ISerialize *handle, const char *name, int64 *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncInt64Default(ISerialize *handle, const char *name, int64 *value, int64 def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncUInt8(ISerialize *handle, const char *name, byte *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncUInt8Default(ISerialize *handle, const char *name, byte *value, byte def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncUInt16(ISerialize *handle, const char *name, ushort *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncUInt16Default(ISerialize *handle, const char *name, ushort *value, ushort def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncUInt32(ISerialize *handle, const char *name, uint *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncUInt32Default(ISerialize *handle, const char *name, uint *value, uint def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncUInt64(ISerialize *handle, const char *name, uint64 *value)
{
	handle->Value(name, *value);
}

void CrySyncInterop::SyncUInt64Default(ISerialize *handle, const char *name, uint64 *value, uint64 def)
{
	handle->ValueWithDefault(name, *value, def);
}

void CrySyncInterop::SyncTime(ISerialize *handle, const char *name, int64 *value, bool *reading)
{
	*reading = handle->IsReading();

	CTimeValue v(TimeUtilities::MonoTicksToCryEngineTicks(*value));

	handle->Value(name, v);

	if (*reading)
	{
		*value = TimeUtilities::CryEngineTicksToMonoTicks(v.GetValue());
	}
}

void CrySyncInterop::SyncTimeDefault(ISerialize *handle, const char *name, int64 *value, int64 def, bool *reading)
{
	*reading = handle->IsReading();

	CTimeValue v(TimeUtilities::MonoTicksToCryEngineTicks(*value));
	CTimeValue d(TimeUtilities::MonoTicksToCryEngineTicks(def));

	handle->ValueWithDefault(name, v, d);

	if (*reading)
	{
		*value = TimeUtilities::CryEngineTicksToMonoTicks(v.GetValue());
	}
}

void CrySyncInterop::SyncXml(ISerialize *handle, const char *name, IXmlNode **value)
{
	XmlNodeRef r(*value);
	handle->Value(name, r);
	
	if (handle->IsReading())
	{
		*value = r;
	}
}
