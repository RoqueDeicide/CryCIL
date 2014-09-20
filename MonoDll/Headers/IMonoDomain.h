/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2012.
//////////////////////////////////////////////////////////////////////////
// IMonoDomain interface to handle script / app domains.
//////////////////////////////////////////////////////////////////////////
// 03/05/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __I_MONO_DOMAIN_H__
#define __I_MONO_DOMAIN_H__

struct IMonoAssembly;
struct IMonoClass;
struct IMonoArray;

struct MonoAnyValue;

namespace mono { class _object; typedef _object* object; class _string; typedef _string* string; }

struct IMonoDomain
{
	/// <summary>
	/// Deletes and cleans used objects in the domain.
	/// </summary>
	virtual void Release() = 0;

	/// <summary>
	/// Sets this domain as the currently active one.
	/// </summary>
	virtual bool SetActive(bool force = false) = 0;
	/// <summary>
	/// Determines if this domain is currently active.
	/// </summary>
	virtual bool IsActive() = 0;

	/// <summary>
	/// If true, this is the primary domain.
	/// </summary>
	virtual bool IsRoot() = 0;

	/// <summary>
	/// Loads an assembly into the domain.
	/// </summary>
	virtual IMonoAssembly *LoadAssembly(const char *file, bool shadowCopy = false, bool convertPdbToMdb = true) = 0;

	/// <summary>
	/// Gets domain name chosen at creation.
	/// </summary>
	virtual const char *GetName() = 0;

	/// <summary>
	/// Creates an IMonoArray with the pre-determined size.
	/// If element is nullptr, uses object class.
	/// </summary>
	/// <example>
	/// [Native C++]
	/// IMonoClass *pElementClass = gEnv->pMonoScriptSystem->GetCryBraryAssembly()->GetClass("Vec3");
	/// CreateArray(3, pElementClass);
	///
	/// [Managed]
	/// Vec3[] myElements;
	/// </example>
	virtual IMonoArray *CreateArray(int size, IMonoClass *pElementClass = nullptr, bool allowGC = true) = 0;

	/// <summary>
	/// Creates a dynamically resizing mono array.
	/// if element is null, uses object class.
	/// </summary>
	virtual IMonoArray *CreateDynamicArray(IMonoClass *pElementClass = nullptr, int size = 0, bool allowGC = true) = 0;

	virtual IMonoObject *BoxAnyValue(MonoAnyValue &value) = 0;

	/// <summary>
	/// Converts a C++ style character array to a managed string.
	/// </summary>
	virtual mono::string CreateMonoString(const char *cStr) = 0;
};

/// <summary>
/// Runtime version enum used when creating root domains.
/// When updating Mono version, verify runtime versions in domain.c, and update CScriptDomain(ERuntimeVersion) constructor accordingly.
/// </summary>
enum ERuntimeVersion
{
	eRV_2_50215,
	/// <summary>
	/// The default / most stable runtime.
	/// </summary>
	eRV_2_50727,

	eRV_4_20506,
	eRV_4_30128,
	eRV_4_30319
};

#include <IMonoScriptSystem.h>

/// <summary>
/// Creates an IMonoArray with the specified size.
/// </summary>
static IMonoArray *CreateMonoArray(int numArgs, IMonoClass *pElementClass = nullptr)
{
	return GetMonoRunTime()->AppDomain->CreateArray(numArgs, pElementClass);
}

/// <summary>
/// Creates an IDynMonoArray with the specified size.
/// </summary>
static IMonoArray *CreateDynamicMonoArray(IMonoClass *pElementClass = nullptr, int size = 0)
{
	return GetMonoRunTime()->AppDomain->CreateDynamicArray(pElementClass, size);
}

/// <summary>
/// Converts a C++ string to the C# equivalent.
/// </summary>
inline mono::string ToMonoString(const char *cryString)
{
	return GetMonoRunTime()->AppDomain->CreateMonoString(cryString);
}

#endif //__I_MONO_DOMAIN_H__