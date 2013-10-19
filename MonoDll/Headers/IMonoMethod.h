/////////////////////////////////////////////////////////////////////////*
//Ink Studios Source File.
//Copyright (C), Ink Studios, 2013.
//////////////////////////////////////////////////////////////////////////
// IMonoMethod interface for external projects, i.e. CryGame.
//////////////////////////////////////////////////////////////////////////
// 02/10/2013 : Created by Filip 'i59' Lundgren
////////////////////////////////////////////////////////////////////////*/
#pragma once

struct IMonoArray;

namespace mono 
{
	class _object; typedef _object* object;
};

struct IMonoMethod
{
	/// <summary>
	/// Gets a managed function as a native function pointer. See example for use case.
	/// Much faster than standard invoke!
	/// </summary>
	/// <example>
	/// typedef int (__stdcall *GetHashCode) (mono::object thisObj);
	///
	/// GetHashCode func = pMethod->GetMethodThunk();
	/// int hashCode = func(myObject);
	/// </example>
	virtual void *GetMethodThunk() = 0;

	/// <summary>
	/// Invokes the managed  method with the specified managed array of parameters.
	/// 'object' points to the managed object we want to invoke on, if null we assume this is a static method.
	/// </summary>
	virtual mono::object InvokeArray(mono::object object, IMonoArray *pParams = nullptr) = 0;
	/// <summary>
	/// Invokes the managed  method with the specified native array of parameters.
	/// 'object' points to the managed object we want to invoke on, if null we assume this is a static method.
	/// </summary>
	virtual mono::object Invoke(mono::object object, void **pParams = nullptr, int numParams = 0) = 0;

	/// <summary>
	/// Gets the name of the method
	/// </summary>
	virtual const char *GetName() = 0;

	/// <summary>
	/// Gets the number of parameters in the method.
	/// </summary>
	virtual int GetParameterCount() = 0;

	//////////////////////////////////////////////////////
	// HELPERS
	//////////////////////////////////////////////////////
	template<typename P1> 
	inline mono::object Call(mono::object object, const P1 &p1);

	template<typename P1, typename P2> 
	inline mono::object Call(mono::object object, const P1 &p1, const P2 &p2);

	template<typename P1, typename P2, typename P3> 
	inline mono::object Call(mono::object object, const P1 &p1, const P2 &p2, const P3 &p3);

	template<typename P1, typename P2, typename P3, typename P4> 
	inline mono::object Call(mono::object object, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4);

	template<typename P1, typename P2, typename P3, typename P4, typename P5> 
	inline mono::object Call(mono::object object, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5);

	template<typename P1, typename P2, typename P3, typename P4, typename P5, typename P6>
	inline mono::object Call(mono::object object, const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5, const P6 &p6);
};

template<typename P1> 
inline mono::object IMonoMethod::Call(mono::object object,const P1 &p1)
{
	IMonoArray *pArgs = CreateMonoArray(1);
	pArgs->Insert(p1);

	mono::object result = InvokeArray(object, pArgs);
	SAFE_RELEASE(pArgs);

	return result;
};

template<typename P1, typename P2> 
inline mono::object IMonoMethod::Call(mono::object object,const P1 &p1, const P2 &p2)
{
	IMonoArray *pArgs = CreateMonoArray(2);
	pArgs->Insert(p1);
	pArgs->Insert(p2);

	mono::object result = InvokeArray(object, pArgs);
	SAFE_RELEASE(pArgs);

	return result;
};

template<typename P1, typename P2, typename P3> 
inline mono::object IMonoMethod::Call(mono::object object,const P1 &p1, const P2 &p2, const P3 &p3)
{
	IMonoArray *pArgs = CreateMonoArray(3);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);

	mono::object result = InvokeArray(object, pArgs);
	SAFE_RELEASE(pArgs);

	return result;
};

template<typename P1, typename P2, typename P3, typename P4> 
inline mono::object IMonoMethod::Call(mono::object object,const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4)
{
	IMonoArray *pArgs = CreateMonoArray(4);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);
	pArgs->Insert(p4);

	mono::object result = InvokeArray(object, pArgs);
	SAFE_RELEASE(pArgs);

	return result;
};

template<typename P1, typename P2, typename P3, typename P4, typename P5> 
inline mono::object IMonoMethod::Call(mono::object object,const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5)
{
	IMonoArray *pArgs = CreateMonoArray(5);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);
	pArgs->Insert(p4);
	pArgs->Insert(p5);

	mono::object result = InvokeArray(object, pArgs);
	SAFE_RELEASE(pArgs);

	return result;
};

template<typename P1, typename P2, typename P3, typename P4, typename P5, typename P6>
inline mono::object IMonoMethod::Call(mono::object object,const P1 &p1, const P2 &p2, const P3 &p3, const P4 &p4, const P5 &p5, const P6 &p6)
{
	IMonoArray *pArgs = CreateMonoArray(6);
	pArgs->Insert(p1);
	pArgs->Insert(p2);
	pArgs->Insert(p3);
	pArgs->Insert(p4);
	pArgs->Insert(p5);
	pArgs->Insert(p6);

	mono::object result = InvokeArray(object, pArgs);
	SAFE_RELEASE(pArgs);

	return result;
};