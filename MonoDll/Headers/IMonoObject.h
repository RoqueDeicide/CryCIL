/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMonoObject interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 18/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#pragma once

#include <IMonoConverter.h>
#include <MonoCommon.h>

struct IMonoObject;
struct MonoAnyValue;
struct IMonoClass;

enum EMonoAnyType;

namespace mono { class _object; typedef _object *object; }

/// <summary>
/// The IMonoObject class is used to wrap native mono objects of any type, and to
/// convert C++ types to the Mono equivalent.
/// </summary>
struct IMonoObject
{
	inline mono::object CallMethod(const char *funcName);

	template<typename P1> 
	inline mono::object CallMethod(const char *funcName, const P1 &p1);

	template<typename P1, typename P2> 
	inline mono::object CallMethod(const char *funcName, const P1 &p1, const P2 &p2);

	template<typename P1, typename P2, typename P3> 
	inline mono::object CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3);

	template<typename P1, typename P2, typename P3, typename P4> 
	inline mono::object CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4);

	template<typename P1, typename P2, typename P3, typename P4, typename P5> 
	inline mono::object CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5);

	template<typename P1, typename P2, typename P3, typename P4, typename P5, typename P6>
	inline mono::object CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5, const P6 &p6);

	inline mono::object GetPropertyValue(const char *fieldName, bool throwOnFail = true);
	inline void SetPropertyValue(const char *fieldName, mono::object newValue, bool throwOnFail = true);
	inline mono::object GetFieldValue(const char *fieldName, bool throwOnFail = true);
	inline void SetFieldValue(const char *fieldName, mono::object newValue, bool throwOnFail = true);

	/// <summary>
	/// Releases the object. Warning: also destructed in managed code!
	/// </summary>
	virtual void Release(bool triggerGC = true) = 0;

	/// <summary>
	/// Gets the unboxed object and casts it to the requested type. (class T)
	/// </summary>
	template <class T>
	T Unbox() { return *(T *)UnboxObject(); }

	/// <summary>
	/// Gets the type of this Mono object.
	/// </summary>
	virtual EMonoAnyType GetType() = 0;

	virtual MonoAnyValue GetAnyValue() = 0;

	virtual const char *ToString() = 0;
	
	/// <summary>
	/// Returns the object as it is seen in managed code, can be passed directly across languages.
	/// </summary>
	virtual mono::object GetManagedObject() = 0;

	virtual IMonoClass *GetClass() = 0;

private:
	/// <summary>
	/// Unboxes the object and returns it as a void pointer. (Use Unbox() method to easily cast to the C++ type)
	/// </summary>
	virtual void *UnboxObject() = 0;
};

/// <summary>
/// Used within MonoAnyValue and IMonoObject to easily get the object type contained within.
/// </summary>
enum EMonoAnyType
{
	eMonoAnyType_Unknown = -1,

	eMonoAnyType_Boolean,

	eMonoAnyType_Integer,
	eMonoAnyType_UnsignedInteger,
	eMonoAnyType_EntityId,
	eMonoAnyType_Short,
	eMonoAnyType_UnsignedShort,
	eMonoAnyType_Float,
	eMonoAnyType_Vec3,
	eMonoAnyType_Quat,

	eMonoAnyType_String,
	eMonoAnyType_Array,

	eMonoAnyType_Last
};

#include <IMonoArray.h>
#include <IMonoClass.h>

#include <IMonoMethod.h>

/// <summary>
/// Simple class used to easily convert common C++ types to their C# equivalents.
/// </summary>
struct MonoAnyValue : public ISerializable
{
	MonoAnyValue() : type(eMonoAnyType_Unknown), monoObject(nullptr) { };
	MonoAnyValue(bool value) : type(eMonoAnyType_Boolean) { b = value; }
	MonoAnyValue(int value) : type(eMonoAnyType_Integer) { i = value; }
	MonoAnyValue(unsigned int value) : type(eMonoAnyType_UnsignedInteger) { u = value; }
	MonoAnyValue(short value) : type(eMonoAnyType_Short) { i = value; }
	MonoAnyValue(unsigned short value) : type(eMonoAnyType_UnsignedShort) { u = value; }
	MonoAnyValue(float value) : type(eMonoAnyType_Float) { f = value; }
	MonoAnyValue(const char *value) : type(eMonoAnyType_String) { str = value; }
	MonoAnyValue(string value) : type(eMonoAnyType_String) { str = value.c_str(); }
	MonoAnyValue(Vec3 value) : type(eMonoAnyType_Vec3) { vec4.x = value.x; vec4.y = value.y; vec4.z = value.z; }
	MonoAnyValue(Ang3 value) : type(eMonoAnyType_Vec3) { vec4.x = value.x; vec4.y = value.y; vec4.z = value.z; }
	MonoAnyValue(Quat value) : type(eMonoAnyType_Quat) { vec4.x = value.v.x; vec4.y = value.v.y; vec4.z = value.v.z; vec4.w = value.w; }
	MonoAnyValue(mono::object value) : type(eMonoAnyType_Unknown), monoObject(value) { };

	virtual void SerializeWith(TSerialize ser) override
	{
		ser.EnumValue("type", type, eMonoAnyType_Unknown, eMonoAnyType_Last);

		switch(type)
		{
		case eMonoAnyType_Boolean:
			ser.Value("boolean", b, 'bool');
			break;
		case eMonoAnyType_UnsignedInteger:
		case eMonoAnyType_UnsignedShort:
			ser.Value("uint", u, 'ui32');
			break;
		case eMonoAnyType_EntityId:
			ser.Value("entityId", u, 'eid');
			break;
		case eMonoAnyType_Integer:
		case eMonoAnyType_Short:
			ser.Value("int", i, 'i32');
			break;
		case eMonoAnyType_Float:
			ser.Value("float", f);
			break;
		case eMonoAnyType_Vec3:
			{
				if(ser.IsWriting())
					ser.Value("vec", Vec3(vec4.x, vec4.y, vec4.z));
				else
				{
					Vec3 v;
					ser.Value("vec", v);

					vec4.x = v.x;
					vec4.y = v.y;
					vec4.z = v.z;
				}
			}
			break;
		case eMonoAnyType_Quat:
			{
				if(ser.IsWriting())
					ser.Value("quat", Quat(vec4.w, vec4.x, vec4.y, vec4.z));
				else
				{
					Quat q;
					ser.Value("quat", q);

					vec4.w = q.w;
					vec4.x = q.v.x;
					vec4.y = q.v.y;
					vec4.z = q.v.z;
				}
			}
			break;
		case eMonoAnyType_String:
			{
				if(ser.IsWriting())
				{
					auto serializedString = string(str);
					ser.Value("str", serializedString); 
				}
				else
				{
					auto serializedString = string();
					ser.Value("str", serializedString);
					str = serializedString.c_str();
				}
			}
			break;
		case eMonoAnyType_Array:
			{
				IMonoScriptSystem *pScriptSystem = GetMonoScriptSystem();

				IMonoArray *pArray = nullptr;
				int arrayLength;

				if(ser.IsWriting())
				{
					pArray = pScriptSystem->GetConverter()->ToArray(monoObject);
					arrayLength = pArray->GetSize();
				}

				ser.Value("arrayLength", arrayLength);

				if(ser.IsWriting())
				{
					for(int i = 0; i < arrayLength; i++)
					{
						IMonoObject *pItem = *pArray->GetItem(i);
						pItem->GetAnyValue().SerializeWith(ser);
						SAFE_RELEASE(pItem);
					}
				}
				else
				{
					pArray = pScriptSystem->GetScriptDomain()->CreateArray(arrayLength);

					for(int i = 0; i < arrayLength; i++)
					{
						MonoAnyValue value;
						value.SerializeWith(ser);
						pArray->InsertAny(value);
					}

					monoObject = pArray->GetManagedObject();
				}

				SAFE_RELEASE(pArray);
			}
			break;
		case eMonoAnyType_Unknown:
			{
				if(ser.IsWriting())
				{
					if(monoObject != nullptr)
					{
						IMonoObject *pObject = *monoObject;
						IMonoClass *pObjectClass = pObject->GetClass();

						CryWarning(VALIDATOR_MODULE_GAME, VALIDATOR_WARNING, "Attempted to serialize unknown managed type %s.%s", pObjectClass->GetNamespace(), pObjectClass->GetName());
					}
					else
						CRY_ASSERT_MESSAGE(false, "Attempted to serialize unknown managed type");
				}
			}
			break;
		}
	}

	void *GetValue()
	{
		switch(type)
		{
		case eMonoAnyType_Boolean:
			return &b;
		case eMonoAnyType_UnsignedInteger:
		case eMonoAnyType_UnsignedShort:
		case eMonoAnyType_EntityId:
			return &u;
		case eMonoAnyType_Integer:
		case eMonoAnyType_Short:
			return &i;
		case eMonoAnyType_Float:
			return &f;
		case eMonoAnyType_Vec3:
			return Vec3(vec4.x, vec4.y, vec4.z);
		case eMonoAnyType_Quat:
			return new Quat(vec4.x, vec4.y, vec4.z, vec4.w);
		case eMonoAnyType_String:
			return &str;
		case eMonoAnyType_Array:
		case eMonoAnyType_Unknown:
			return monoObject;
		}
	}

	EMonoAnyType type;
	union
	{
		bool			b;
		float			f;
		int				i;
		unsigned int	u;
		const char*		str;
		struct { float x,y,z,w; } vec4;
		mono::object monoObject;
	};
};

inline mono::object IMonoObject::CallMethod(const char *funcName)
{
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName))
		return pMethod->Invoke(this->GetManagedObject());

	return nullptr;
}

template<typename P1> 
inline mono::object IMonoObject::CallMethod(const char *funcName, const P1 &p1)
{
	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->Insert(p1);

	mono::object result = nullptr;
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName, pArgs))
		result = pMethod->InvokeArray(this->GetManagedObject(), pArgs);
	
	SAFE_RELEASE(pArgs);
	return result;
};

template<typename P1, typename P2> 
inline mono::object IMonoObject::CallMethod(const char *funcName, const P1 &p1, const P2 &p2)
{
	IMonoArray *pArgs = CreateMonoArray(2);
	pArgs->Insert(p1);
	pArgs->Insert(p2);

	mono::object result = nullptr;
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName, pArgs))
		result = pMethod->InvokeArray(this->GetManagedObject(), pArgs);
	
	SAFE_RELEASE(pArgs);
	return result;
};

template<typename P1, typename P2, typename P3> 
inline mono::object IMonoObject::CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3)
{
	IMonoArray *pArgs = CreateMonoArray(3);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);

	mono::object result = nullptr;
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName, pArgs))
		result = pMethod->InvokeArray(this->GetManagedObject(), pArgs);
	
	SAFE_RELEASE(pArgs);
	return result;
};

template<typename P1, typename P2, typename P3, typename P4> 
inline mono::object IMonoObject::CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4)
{
	IMonoArray *pArgs = CreateMonoArray(4);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);
	pArgs->Insert(p4);

	mono::object result = nullptr;
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName, pArgs))
		result = pMethod->InvokeArray(this->GetManagedObject(), pArgs);

	SAFE_RELEASE(pArgs);
	return result;
};

template<typename P1, typename P2, typename P3, typename P4, typename P5> 
inline mono::object IMonoObject::CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5)
{
	IMonoArray *pArgs = CreateMonoArray(5);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);
	pArgs->Insert(p4);
	pArgs->Insert(p5);

	mono::object result = nullptr;
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName, pArgs))
		result = pMethod->InvokeArray(this->GetManagedObject(), pArgs);

	SAFE_RELEASE(pArgs);
	return result;
};

template<typename P1, typename P2, typename P3, typename P4, typename P5, typename P6>
inline mono::object IMonoObject::CallMethod(const char *funcName, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5, const P6 &p6)
{
	IMonoArray *pArgs = CreateMonoArray(6);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);
	pArgs->Insert(p4);
	pArgs->Insert(p5);
	pArgs->Insert(p6);

	mono::object result = nullptr;
	if(IMonoMethod *pMethod = GetClass()->GetMethod(funcName, pArgs))
		result = pMethod->InvokeArray(this->GetManagedObject(), pArgs);

	SAFE_RELEASE(pArgs);
	return result;
};

inline mono::object IMonoObject::GetPropertyValue(const char *propertyName, bool throwOnFail)
{
	return GetClass()->GetPropertyValue(this->GetManagedObject(), propertyName, throwOnFail);
}

inline void IMonoObject::SetPropertyValue(const char *propertyName, mono::object newValue, bool throwOnFail)
{
	GetClass()->SetPropertyValue(this->GetManagedObject(), propertyName, newValue, throwOnFail);
}

inline mono::object IMonoObject::GetFieldValue(const char *fieldName, bool throwOnFail)
{
	return GetClass()->GetFieldValue(this->GetManagedObject(), fieldName, throwOnFail);
}

inline void IMonoObject::SetFieldValue(const char *fieldName, mono::object newValue, bool throwOnFail)
{
	GetClass()->SetFieldValue(this->GetManagedObject(), fieldName, newValue, throwOnFail);
}