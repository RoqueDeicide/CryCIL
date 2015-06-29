#pragma once

#include "IMonoInterface.h"

struct CrySyncInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() override { return "CrySync"; }
	virtual const char *GetNameSpace() override { return "CryCil.Engine.Data"; }

	virtual void OnRunTimeInitialized() override;

	static void FlagPartialRead(ISerialize *handle);
	static void StartGroup(ISerialize *handle, const char *name);
	static bool StartOptionalGroup(ISerialize *handle, const char *name, bool condition);
	static void FinishGroup(ISerialize *handle);
	static bool IsReading(ISerialize *handle);
	static int GetSerializationTarget(ISerialize *handle);

	static void SyncString       (ISerialize *handle, const char *name, mono::string *value);
	static void SyncStringDefault(ISerialize *handle, const char *name, mono::string *value, mono::string def);
	static void SyncBool         (ISerialize *handle, const char *name, bool *value);
	static void SyncBoolDefault  (ISerialize *handle, const char *name, bool *value, bool def);
	static void SyncSingle       (ISerialize *handle, const char *name, float *value);
	static void SyncSingleDefault(ISerialize *handle, const char *name, float *value, float def);
	static void SyncVec2         (ISerialize *handle, const char *name, Vec2 *value);
	static void SyncVec2Default  (ISerialize *handle, const char *name, Vec2 *value, Vec2 def);
	static void SyncVec3         (ISerialize *handle, const char *name, Vec3 *value);
	static void SyncVec3Default  (ISerialize *handle, const char *name, Vec3 *value, Vec3 def);
	static void SyncQuat         (ISerialize *handle, const char *name, Quat *value);
	static void SyncQuatDefault  (ISerialize *handle, const char *name, Quat *value, Quat def);
	static void SyncAngles       (ISerialize *handle, const char *name, Ang3 *value);
	static void SyncAnglesDefault(ISerialize *handle, const char *name, Ang3 *value, Ang3 def);
	static void SyncInt8         (ISerialize *handle, const char *name, signed char *value);
	static void SyncInt8Default  (ISerialize *handle, const char *name, signed char *value, signed char def);
	static void SyncInt16        (ISerialize *handle, const char *name, short *value);
	static void SyncInt16Default (ISerialize *handle, const char *name, short *value, short def);
	static void SyncInt32        (ISerialize *handle, const char *name, int *value);
	static void SyncInt32Default (ISerialize *handle, const char *name, int *value, int def);
	static void SyncInt64        (ISerialize *handle, const char *name, int64 *value);
	static void SyncInt64Default (ISerialize *handle, const char *name, int64 *value, int64 def);
	static void SyncUInt8        (ISerialize *handle, const char *name, byte *value);
	static void SyncUInt8Default (ISerialize *handle, const char *name, byte *value, byte def);
	static void SyncUInt16       (ISerialize *handle, const char *name, ushort *value);
	static void SyncUInt16Default(ISerialize *handle, const char *name, ushort *value, ushort def);
	static void SyncUInt32       (ISerialize *handle, const char *name, uint *value);
	static void SyncUInt32Default(ISerialize *handle, const char *name, uint *value, uint def);
	static void SyncUInt64       (ISerialize *handle, const char *name, uint64 *value);
	static void SyncUInt64Default(ISerialize *handle, const char *name, uint64 *value, uint64 def);
	static void SyncTime         (ISerialize *handle, const char *name, int64 *value, bool *reading);
	static void SyncTimeDefault  (ISerialize *handle, const char *name, int64 *value, int64 def, bool *reading);
	static void SyncXml          (ISerialize *handle, const char *name, IXmlNode **value);
};