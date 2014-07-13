/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMonoAssembly interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 18/12/2011 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __I_MONO_ASSEMBLY__
#define __I_MONO_ASSEMBLY__

struct IMonoClass;
struct IMonoException;

namespace mono { class _object; typedef _object* object; }

#include <IMonoObject.h>

/// <summary>
/// Reference to a Mono assembly; used to for example instantiate classes contained within a C# dll.
/// </summary>
struct IMonoAssembly
	: public IMonoObject
{
public:
	// IMonoObject
	virtual IMonoClass *GetClass() = 0;
	// ~IMonoObject

	/// <summary>
	/// Gets a custom C# class from within the assembly.
	/// Note: This does not construct an new instance of the class, only returns an uninitialized IMonoScript. To instantiate a class, see IMonoAssembly::InstantiateClass
	/// </summary>
	/// <example>
	/// IMonoScript *pClass = GetMonoScriptSystem()->GetCryBraryAssembly()->GetCustomClass("Vec3");
	/// </example>
	virtual IMonoClass *GetClass(const char *className, const char *nameSpace = "CryEngine") = 0;

	/// <summary>
	/// Gets the name of this assembly.
	/// </summary>
	virtual const char *GetName() = 0;

	/// <summary>
	/// Gets the full path to this assembly on disk.
	/// </summary>
	virtual const char *GetPath() = 0;

	/// <summary>
	/// Determines if this assembly was loaded from C++.
	/// </summary>
	virtual bool IsNative() = 0;

	/// <summary>
	/// Gets the domain this assembly is loaded in.
	/// </summary>
	virtual IMonoDomain *GetDomain() = 0;

	/// <summmary>
	/// Retrieves a throwable exception from the assembly.
	/// </summary>
	virtual IMonoException *GetException(const char *nameSpace, const char *exceptionClass, const char *message = nullptr, ...)
	{
		va_list	args;
		char szBuffer[4096];
		va_start(args, message);
		int count = vsnprintf_s(szBuffer, sizeof(szBuffer), message, args);
		if (count == -1 || count >= sizeof(szBuffer))
			szBuffer[sizeof(szBuffer)-1] = '\0';
		va_end(args);

		return _GetException(nameSpace, exceptionClass, szBuffer);
	}

private:
	virtual IMonoException *_GetException(const char *nameSpace, const char *exceptionClass, const char *message) = 0;
};

#endif //__I_MONO_ASSEMBLY__`	