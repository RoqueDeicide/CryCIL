#pragma once

struct IMonoMethod;
struct IMonoStaticMethod;
struct IMonoConstructor;

//! Base interface for wrappers of Mono functions (type members defined as functions, e.g. methods and
//! constructors).
struct IMonoFunction : public IMonoMember
{
protected:
	_MonoMethod *wrappedMethod;
	int paramCount;
	const char *name;

	const char *paramList;
	List<IMonoClass *> paramClasses;
	List<const char *> paramTypeNames;
	void *rawThunk;

	IMonoClass *klass;

	IMonoFunction(_MonoMethod *method, IMonoClass *klass = nullptr)
		: rawThunk(nullptr)
	{
		if (!method)
		{
			CryFatalError("Attempted to create a Mono function wrapper for a null pointer.");
		}
		
		this->klass = klass ? klass : MonoEnv->Functions->GetDeclaringClass(method);

		this->name       = MonoEnv->Functions->GetName(method);
		this->paramCount = MonoEnv->Functions->ParseSignature(method, this->paramTypeNames, this->paramList);
		
		this->wrappedMethod = method;
	}
public:
	~IMonoFunction()
	{
		SAFE_DELETE(this->paramList);

		this->paramClasses.Dispose();

		for (int i = 0; i < this->paramTypeNames.Length; i++)
		{
			delete this->paramTypeNames[i];
		}
		this->paramTypeNames.Dispose();
	}
	//! Returns a pointer to a C-style function that can be invoked like a standard function pointer.
	//!
	//! Signature of the function pointer is defined by the following rules:
	//!    1) Only built-in types can be used to pass values between managed and unmanaged code
	//!       directly. Every value-type that is not a built-in type must (un)boxed.
	//!
	//!    2) Calling convention is a standard one for the platform. (__stdcall for Windows.)
	//!
	//!    3) If the method is an instance method then the first parameter should be
	//!       mono::object that represents an instance this method is called for;
	//!       If the instance is a value-type object, then it must be boxed, even if it's a
	//!       built-in type.
	//!
	//!    4) The actual parameters that method takes are defined in the same order as in .Net
	//!       method signature.
	//!
	//!    5) If the parameter is of built-in type and it is passed by reference using re or out
	//!       keywords, then the pointer to that type must be used in C++ signature.
	//!
	//!    6) If the parameter is not of built-in type and it is passed by reference, then the
	//!       parameter in C++ must be defined in a usual manner.
	//!
	//!    7) The very last parameter is a pointer to mono::exception (mono::exception *), it
	//!       must not be null at the point of invocation and it will point at null after method
	//!       returns normally, if there was an unhandled exception however, the last parameter
	//!       will point at the exception object. You can handle it yourself, or pass it to
	//!       IMonoInterface::HandleException.
	//!
	//! Warning: Attempting to get unmanaged thunk of virtual method will result in
	//!          InvalidProgramException being raised.
	//!
	//! Examples of usage:
	//!
	//! Static method:
	//!
	//! Signatures:
	//! C#:  int CalculateSum(int a, int b);
	//! C++: typedef int (__stdcall *CalculateSum)(int a, int b, mono::exception *ex);
	//!
	//! Getting the thunk:
	//! @code{.cpp}
	//!    IMonoMethod *calculateSumMethod = ... (Get the method pointer).
	//!    CalculateSum sumFunc = (CalculateSum)calculateSumMethod->UnmanagedThunk;
	//! @endcode
	//!
	//! Invocation:
	//! @code{.cpp}
	//!    mono::exception exception;
	//!    int sum = sumFunc(10, 15, &exception);
	//!    if (exception)
	//!    {
	//!        MonoEnv->HandleException(exception);		// Or you can handle it yourself.
	//!    }
	//!    else
	//!    {
	//!        // These code region is the only place where the result of invocation is defined.
	//!        CryLogAlways("Sum of 10 and 15 is %d", sum);
	//!    }
	//! @endcode
	//!
	//! Instance method:
	//!
	//! Signatures:
	//! C#:  float[] ToArray();
	//! C++: typedef mono::Array (__stdcall *Vector2_ToArray)(mono::vector2 instance, mono::exception *ex);
	//!
	//! Getting the thunk:
	//! @code{.cpp}
	//!    IMonoMethod *toArrayMethod = ... (Get the method pointer).
	//!    Vector2_ToArray toArray = (Vector2_ToArray)toArrayMethod->UnmanagedThunk;
	//! @endcode
	//!
	//! Invocation:
	//! @code{.cpp}
	//!    mono::vector2 instance = Box(Vec2(10, 13));
	//!    mono::exception exception;
	//!    mono::Array componentsArray = toArray(instance, &exception);
	//!    if (exception)
	//!    {
	//!        MonoEnv->HandleException(exception);		// Or you can handle it yourself.
	//!    }
	//!    else
	//!    {
	//!        // This code region is the only place where the result of invocation is defined.
	//!
	//!        // Wrap the array.
	//!        IMonoArray *vectorComponents = MonoEnv->Objects->Arrays->Wrap(componentsArray, true);
	//!        // Print the components.
	//!        CryLogAlways("X component of the vector = %d", vectorComponents->At<float>(0));
	//!        CryLogAlways("Y component of the vector = %d", vectorComponents->At<float>(1));
	//!        // Release the array.
	//!        vectorComponents->Release();
	//!    }
	//! @endcode
	//!
	//! Static method with ref and out parameters:
	//!
	//! Signatures:
	//! C#:  void Clamp(ref Vector2 value, ref Vector2 min, ref Vector2 max, out Vector2 result);
	//! C++: typedef void (__stdcall *Clamp)(mono::vector2 value, mono::vector2 min, mono::vector2 max, mono::vector2 result, mono::exception *ex);
	//!
	//! Getting the thunk:
	//! @code{.cpp}
	//!    IMonoMethod *ClampMethod = ... (Get the method pointer).
	//!    Clamp clamp = (Clamp)ClampMethod->UnmanagedThunk;
	//! @endcode
	//!
	//! Invocation:
	//! @code{.cpp}
	//!    mono::vector2 value =  Box(Vec2(10, 13));
	//!    mono::vector2 min =    Box(Vec2(13, 8));
	//!    mono::vector2 max =    Box(Vec2(30, 12));
	//!    mono::vector2 result = Box(Vec2(0, 0));	// Box default values when using "out" parameters.
	//!                                           	// Otherwise expect the program to crash.
	//!    mono::exception exception;
	//!    clamp(value, min, max, result, &exception);
	//!    if (exception)
	//!    {
	//!        MonoEnv->HandleException(exception);		// Or you can handle it yourself.
	//!    }
	//!    else
	//!    {
	//!        // This code region is the only place where the result of invocation is defined.
	//!
	//!        // Unbox the "out" parameter.
	//!        Vec2 vector = Unbox<Vec2>(result);
	//!        // Print the components.
	//!        CryLogAlways("X component of the vector = %d", vector.x);
	//!        CryLogAlways("Y component of the vector = %d", vector.y);
	//!    }
	//! @endcode
	//!
	//! Instance method with custom value-type result:
	//!
	//! Signatures:
	//! C#:  Vector3 GetRotated(Vector3 axis, float angle);
	//! C++: typedef mono::vector3 (__stdcall *GetRotated)(mono::vector3 instance, mono::vector3 axis, float angle, mono::exception *ex);
	//!
	//! Getting the thunk:
	//! @code{.cpp}
	//!    IMonoMethod *GetRotatedMethod = ... (Get the method pointer).
	//!    GetRotated getRotated = (GetRotated)GetRotatedMethod->UnmanagedThunk;
	//! @endcode
	//!
	//! Invocation:
	//! @code{.cpp}
	//!    mono::vector3 instance = Box(Vec3(10, 13, 0));
	//!    mono::vector3 axis =     Box(Vec3(0, 0, 1));
	//!    mono::exception exception;
	//!    mono::vector3 result = getRotated(instance, axis, PI, &exception);
	//!    if (exception)
	//!    {
	//!        MonoEnv->HandleException(exception);		// Or you can handle it yourself.
	//!    }
	//!    else
	//!    {
	//!        // This code region is the only place where the result of invocation is defined.
	//!
	//!        // Unbox the result.
	//!        Vec3 vector = Unbox<Vec3>(result);
	//!        // Print the components.
	//!        CryLogAlways("X component of the vector = %d", vector.x);
	//!        CryLogAlways("Y component of the vector = %d", vector.y);
	//!        CryLogAlways("Z component of the vector = %d", vector.z);
	//!    }
	//! @endcode
	//! @example DoxygenExampleFiles\UnmanagedThunkExample.h
	__declspec(property(get = GetThunk)) void *UnmanagedThunk;
	//! Gets a raw thunk of this function.
	//!
	//! Raw thunks are probably one of the fastest ways of invoking a method, however they have quite a few
	//! problems.
	//!
	//! 1) If the method through an exception and it's not handled, then there is no way to catch it.
	//! 2) The method has to return mono::object unless it returns a built-in primitive type.
	//! 3) It is not known if raw thunks can be used to invoke instance or virtual methods.
	//!
	//! On brighter side, you don't need to box value-type objects that you pass as arguments, allowing you
	//! to completely avoid boxing values, which is very important for interop-calls with application loops
	//! (e.g the main game loop) because it helps avoiding creation of temporary managed objects that create
	//! garbage piles.
	//!
	//! You also don't need to specify __stdcall since __cdecl works as well.
	//!
	//! Other then all of the above, raw thunks are the same as unmanaged thunks.
	//!
	//! Perfect example of a function that can be invoked through a raw thunk:
	//!
	//! @code{.cs}
	//!
	//! public static void Interpolate(Vector3 start, Vector3 finish, float parameter, out Vector3 result)
	//! {
	//!     try
	//!     {
	//!         result = start + (finish - start) * parameter;
	//!     }
	//!     catch {}        // Use empty catch block to filter out unhandled exceptions, without it any one
	//!                     // of those will crash the program!
	//! }
	//!
	//! @endcode
	//!
	//! Here is the signature of the thunk in C++:
	//!
	//! @code{.cpp}
	//!
	//! typedef void(*InterpolateRawThunk)(Vec3, Vec3, float, Vec3 *);
	//!
	//! @endcode
	__declspec(property(get = GetFunctionPointer)) void *RawThunk;
	//! Gets number of arguments this function accepts.
	__declspec(property(get = GetParameterCount)) int ParameterCount;
	//! Gets a list of parameters this function accepts.
	__declspec(property(get = GetParameterTypeNames)) List<const char *> *ParameterTypeNames;
	//! Gets a list of classes of parameters this function accepts.
	__declspec(property(get = GetParameterClasses)) List<IMonoClass *> *ParameterClasses;
	//! Gets a list of parameters this function accepts.
	__declspec(property(get = GetParametersList)) const char *Parameters;
	//! Gets an object of type System.Reflection.MethodInfo that represents this method.
	__declspec(property(get = GetReflectionObject)) mono::object ReflectionObject;

	//! Attempts to statically cast this object to IMonoMethod.
	//!
	//! @returns A result of static_cast of this object to IMonoMethod.
	__forceinline IMonoMethod *ToInstance();			// Defined in IMonoMethod.h
	//! Attempts to statically cast this object to IMonoStaticMethod.
	//!
	//! @returns A result of static_cast of this object to IMonoStaticMethod.
	__forceinline IMonoStaticMethod *ToStatic();		// Defined in IMonoStaticMethod.h
	//! Attempts to statically cast this object to IMonoConstructor.
	//!
	//! @returns A result of static_cast of this object to IMonoConstructor.
	__forceinline IMonoConstructor *ToCtor();			// Defined in IMonoConstructor.h

	void *GetThunk()
	{
		return MonoEnv->Functions->GetThunk(this->wrappedMethod);
	}
	void *GetFunctionPointer()
	{
		if (!this->rawThunk)
		{
			this->rawThunk = MonoEnv->Functions->GetFunctionPointer(this->wrappedMethod);
		}
	}
	int GetParameterCount()
	{
		return this->paramCount;
	}
	List<const char *> *GetParameterTypeNames()
	{
		return &this->paramTypeNames;
	}
	List<IMonoClass *> *GetParameterClasses()
	{
		if (this->paramClasses.Length != this->paramCount)
		{
			MonoEnv->Functions->GetParameterClasses(this->wrappedMethod, this->paramClasses);
		}

		return &this->paramClasses;
	}
	const char *GetParametersList()
	{
		return this->paramList;
	}
	mono::object GetReflectionObject()
	{
		return MonoEnv->Functions->GetReflectionObject(this->wrappedMethod);
	}

	virtual const char *GetName()
	{
		return this->name;
	}

	virtual IMonoClass *GetDeclaringClass()
	{
		return this->klass;
	}

	virtual void *GetWrappedPointer()
	{
		return this->wrappedMethod;
	}

	// Internal method, just ignore it.
	__forceinline IMonoFunction *GetFunc()
	{
		return this;
	}
};