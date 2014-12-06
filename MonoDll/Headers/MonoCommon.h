/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Common CryMono types and methods.
//
// DO NOT INCLUDE IN CRYMONO INTERFACES!
//////////////////////////////////////////////////////////////////////////
// 11/01/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __MONO_COMMON_H__
#define __MONO_COMMON_H__

#include <IMonoRunTime.h>

/// <summary>
/// Converts a C++ string to the C# equivalent.
/// </summary>
inline const char *ToCryString(mono::string monoString)
{
	if(!monoString)
		return "";

	return GetMonoRunTime()->ToString(monoString);
}

/// <summary>
/// Wrapped 'helpers' used to easily convert native mono objects to IMonoArray's, strings etc.
/// </summary>
namespace mono
{
	/// <summary>
	/// Mono String; used in scriptbind parameters and when invoking Mono scripts.
	/// </summary>
	class _string
	{
	public:
		/// <summary>
		/// Allows direct casting from mono::string to const char *, no more manual ToCryString calls, woo!
		/// </summary>
		operator const char*() const
		{
			return ToCryString(const_cast<_string *>(this));
		}

		/// <summary>
		/// Allows direct casting from mono::string to CryStringT<char> (string).
		/// </summary>
		operator CryStringT<char>() const
		{
			return (CryStringT<char>)ToCryString(const_cast<_string *>(this));
		}
	};

	class _object
	{
	public:
		operator IMonoObject *() const
		{
			return GetMonoRunTime()->ToObject(const_cast<_object *>(this));
		}

		operator IMonoArray *() const
		{
			return GetMonoRunTime()->ToArray(const_cast<_object *>(this));
		}
		IMonoObject *ToWrapper() const
		{
			return GetMonoRunTime()->ToObject(const_cast<_object *>(this));
		}
	};

	typedef _string* string;
	typedef _object* object;
}; 

#include <IMonoDomain.h>
#include <IMonoObject.h>

#endif //__MONO_COMMON_H__