/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// IMonoScriptBind interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 20/11/2011 : Created by Filip 'i59' Lundgren (Based on version by ins\)
////////////////////////////////////////////////////////////////////////*/
#ifndef __IMONOSCRIPTBIND_H__
#define __IMONOSCRIPTBIND_H__

#include <IMonoScriptSystem.h>
#include <IMonoAssembly.h>

/// <summary>
/// Simple pre-processor method used to quickly register methods within interops.
/// </summary>
#define REGISTER_METHOD(method) GetMonoScriptSystem()->RegisterInteropMethod(method, (GetNamespace() + (string)".").append(GetClassName()).append("::").append(#method))

#define REGISTER_METHOD_NAME(method, name) GetMonoScriptSystem()->RegisterInteropMethod(method, (GetNamespace() + (string)".").append(GetClassName()).append("::").append(name))

/// <summary>
/// </summary>
struct IMonoInterop
{
	virtual ~IMonoInterop() {}

	/// <summary>
	/// The namespace in which the Mono class this scriptbind is tied to resides in; returns "CryEngine.Native" by default if not overridden.
	/// </summary>
	virtual const char *GetNamespace() { return "CryEngine.Native"; }
	/// <summary>
	/// The Mono class which this scriptbind is tied to. Unlike GetNameSpace and GetNameSpaceExtension, this has no default value and MUST be set.
	/// </summary>
	virtual const char *GetClassName() = 0;
};

#endif //__IMONOSCRIPTBIND_H__