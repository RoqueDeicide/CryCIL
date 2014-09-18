#include "stdafx.h"
#include "CrySerialize.h"

CrySerializeInterop::CrySerializeInterop()
{
	REGISTER_METHOD(BeginGroup);
	REGISTER_METHOD(EndGroup);

	REGISTER_METHOD(ValueString);
	REGISTER_METHOD(ValueInt);
	REGISTER_METHOD(ValueUInt);
	REGISTER_METHOD(ValueBool);
	REGISTER_METHOD(ValueEntityId);
	REGISTER_METHOD(ValueFloat);
	REGISTER_METHOD(ValueVec3);
	REGISTER_METHOD(ValueQuat);

	REGISTER_METHOD(EnumValue);
	REGISTER_METHOD(UnsignedEnumValue);

	REGISTER_METHOD(_IsReading);

	REGISTER_METHOD(FlagPartialRead);
}

CrySerializeInterop::~CrySerializeInterop()
{}

int CrySerializeInterop::ConvertPolicy(mono::string policy)
{
	if (policy == nullptr)
		return 0;

	const char *policyStr = ToCryString(policy);

	int size = sizeof(policyStr);
	int result = 0;

	for (int i = 0; i < size; i++)
		result += policyStr[size - (i + 1)] << (i * 8);

	return result;
}

void CrySerializeInterop::BeginGroup(ISerialize *ser, mono::string name)
{
	ser->BeginGroup(ToCryString(name));
}

void CrySerializeInterop::EndGroup(ISerialize *ser)
{
	ser->EndGroup();
}

void CrySerializeInterop::ValueString(ISerialize *ser, mono::string name, mono::string &str, mono::string policy)
{
	int iPolicy = ConvertPolicy(policy);

	TSerialize serialize = TSerialize(ser);
	if (!ser->IsReading())
		serialize.Value(ToCryString(name), ToCryString(str), iPolicy);
	else
	{
		string cryStr;
		serialize.Value(ToCryString(name), cryStr, iPolicy);
		str = ToMonoString(cryStr.c_str());
	}
}

void CrySerializeInterop::ValueInt(ISerialize *ser, mono::string name, int &obj, mono::string policy)
{
	int iPolicy = ConvertPolicy(policy);

	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, iPolicy);
}

void CrySerializeInterop::ValueUInt(ISerialize *ser, mono::string name, unsigned int &obj, mono::string policy)
{
	int iPolicy = ConvertPolicy(policy);

	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, iPolicy);
}

void CrySerializeInterop::ValueBool(ISerialize *ser, mono::string name, bool &obj)
{
	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, 'bool');
}

void CrySerializeInterop::ValueEntityId(ISerialize *ser, mono::string name, EntityId &obj)
{
	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, 'eid');
}

void CrySerializeInterop::ValueFloat(ISerialize *ser, mono::string name, float &obj, mono::string policy)
{
	int iPolicy = ConvertPolicy(policy);

	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, iPolicy);
}

void CrySerializeInterop::ValueVec3(ISerialize *ser, mono::string name, Vec3 &obj, mono::string policy)
{
	int iPolicy = ConvertPolicy(policy);

	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, iPolicy);
}

void CrySerializeInterop::ValueQuat(ISerialize *ser, mono::string name, Quat &obj, mono::string policy)
{
	int iPolicy = ConvertPolicy(policy);

	TSerialize serialize = TSerialize(ser);
	serialize.Value(ToCryString(name), obj, iPolicy);
}

void CrySerializeInterop::EnumValue(ISerialize *ser, mono::string name, int &obj, int first, int last)
{
	TSerialize serialize = TSerialize(ser);
	serialize.EnumValue(ToCryString(name), obj, first, last);
}

void CrySerializeInterop::UnsignedEnumValue(ISerialize *ser, mono::string name, unsigned int &obj, unsigned int first, unsigned int last)
{
	TSerialize serialize = TSerialize(ser);
	serialize.EnumValue(ToCryString(name), obj, first, last);
}

bool CrySerializeInterop::_IsReading(ISerialize *ser)
{
	return ser->IsReading();
}

void CrySerializeInterop::FlagPartialRead(ISerialize *ser)
{
	ser->FlagPartialRead();
}