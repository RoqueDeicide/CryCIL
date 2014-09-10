#ifndef __SCRIPTBIND_CRYSERIALIZE_H__
#define __SCRIPTBIND_CRYSERIALIZE_H__

#include <IMonoScriptbind.h>

class CScriptbind_CrySerialize
	: public IMonoScriptBind
{
public:
	CScriptbind_CrySerialize();
	~CScriptbind_CrySerialize();

protected:
	// IMonoScriptBind
	virtual const char *GetNamespace() override { return "CryEngine.Serialization"; }
	virtual const char *GetClassName() override { return "CrySerialize"; }
	// ~IMonoScriptBind

	static int ConvertPolicy(mono::string policy);

	static void BeginGroup(ISerialize *ser, mono::string name);
	static void EndGroup(ISerialize *ser);

	static void ValueString(ISerialize *ser, mono::string name, mono::string &str, mono::string policy);
	static void ValueInt(ISerialize *ser, mono::string name, int &obj, mono::string policy);
	static void ValueUInt(ISerialize *ser, mono::string name, unsigned int &obj, mono::string policy);
	static void ValueBool(ISerialize *ser, mono::string name, bool &obj);
	static void ValueEntityId(ISerialize *ser, mono::string name, EntityId &obj);
	static void ValueFloat(ISerialize *ser, mono::string name, float &obj, mono::string policy);
	static void ValueVec3(ISerialize *ser, mono::string name, Vec3 &obj, mono::string policy);
	static void ValueQuat(ISerialize *ser, mono::string name, Quat &obj, mono::string policy);

	static void EnumValue(ISerialize *ser, mono::string name, int &obj, int first, int last);
	static void UnsignedEnumValue(ISerialize *ser, mono::string name, unsigned int &obj, unsigned int first, unsigned int last);

	static bool _IsReading(ISerialize *ser);

	static void FlagPartialRead(ISerialize *ser);
};

#endif //__SCRIPTBIND_CRYSERIALIZE_H__