/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2011.
//////////////////////////////////////////////////////////////////////////
// Handles converting between C# and C++ types and objects.
//////////////////////////////////////////////////////////////////////////
// 11/01/2012 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#ifndef __I_MONO_CONVERTER_H__
#define __I_MONO_CONVERTER_H__

namespace mono
{
	class _string; typedef _string *string;
	class _object; typedef _object *object;
}

struct IMonoArray;
struct IMonoObject;

/// <summary>
/// Used to create and convert C++ / C# values.
/// </summary>
struct IMonoConverter
{
	/// <summary>
	/// Converts a mono string to a const char *.
	/// </summary>
	virtual const char *ToString(mono::string monoString) = 0;

	/// <summary>
	/// Converts a mono array to a IMonoArray. (To provide GetSize, GetItem etc functionality.)
	/// </summary>
	virtual IMonoArray *ToArray(mono::object arr) = 0;

	/// <summary>
	/// Converts an mono object to a IMonoObject.
	/// </summary>
	virtual IMonoObject *ToObject(mono::object obj, bool allowGC = true) = 0;
};

#endif //__I_MONO_CONVERTER_H__