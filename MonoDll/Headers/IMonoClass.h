/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMonoClass interface for external projects, i.e. CryGame.
// Represents a managed type.
//////////////////////////////////////////////////////////////////////////
// 18/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __I_MONO_CLASS_H__
#define __I_MONO_CLASS_H__

#include <IMonoObject.h>

struct IMonoAssembly;
struct IMonoArray;

struct IMonoMethod;

/// <summary>
/// Reference to a Mono class, used to call static methods and etc.
/// </summary>
/// <example>
/// IMonoScript *pCryNetwork = g_pScriptSystem->GetCustomClass("CryNetwork", "CryEngine");
/// </example>
struct IMonoClass
	: public IMonoObject
{
public:
	virtual void AddRef() = 0;

	/// <summary>
	/// Gets the class name.
	/// </summary>
	virtual const char *GetName() = 0;

	/// <summary>
	/// Gets the namespace the class resides in.
	/// </summary>
	virtual const char *GetNamespace() = 0;

	/// <summary>
	/// Gets the assembly that declares this type.
	/// </summary>
	virtual IMonoAssembly *GetAssembly() = 0;

	/// <summary>
	/// Instantiates the class, if not already instantiated.
	/// </summary>
	virtual mono::object CreateInstance(IMonoArray *pConstructorParams = nullptr) = 0;

	/// <summary>
	/// Boxes a type to be received as a managed type.
	/// </summary>
	virtual mono::object BoxObject(void *object, IMonoDomain *pDomain = nullptr) = 0;

	/// <summary>
	/// Gets a method based on a set of parameters
	/// Compares the types of the parameters to find the relevant function.
	/// </summary>
	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *pArgs = nullptr, bool throwOnFail = true) = 0;
	/// <summary>
	/// Gets methods matching the specified pattern
	/// </summary>
	virtual IMonoMethod *GetMethod(const char *name, int numParams, bool throwOnFail = true) = 0;
	/// <summary>
	/// Gets methods matching the specified pattern
	/// </summary>
	virtual int GetMethods(const char *name, int numParams, IMonoMethod ***pMethodsOut, int maxMethods, bool throwOnFail = true) = 0;
	
	/// <summary>
	/// Gets the value of a property in the specified instance.
	/// </summary>
	virtual mono::object GetPropertyValue(mono::object object, const char *propertyName, bool throwOnFail = true) = 0;
	/// <summary>
	/// Sets the value of a property in the specified instance.
	/// </summary>
	virtual void SetPropertyValue(mono::object object, const char *propertyName, mono::object newValue, bool throwOnFail = true) = 0;
	/// <summary>
	/// Gets the value of a field in the specified instance.
	/// </summary>
	virtual mono::object GetFieldValue(mono::object object, const char *fieldName, bool throwOnFail = true) = 0;
	/// <summary>
	/// Sets the value of a field in the specified instance.
	/// </summary>
	virtual void SetFieldValue(mono::object object, const char *fieldName, mono::object newValue, bool throwOnFail = true) = 0;

	/// <summary>
	/// Checks whether this class implements another class by name.
	/// </summary>
	virtual bool ImplementsClass(const char *className, const char *nameSpace = nullptr) = 0;
	/// <summary>
	/// Checks whether this class implements an interface by name.
	/// </summary>
	virtual bool ImplementsInterface(const char *interfaceName, const char *nameSpace = nullptr, bool bSearchDerivedClasses = true) = 0;

	/// <summary>
	/// Gets the parent class this class derives from.
	/// </summary>
	virtual IMonoClass *GetParent() = 0;
};

#endif //__I_MONO_CLASS_H__