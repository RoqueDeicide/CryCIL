#pragma once

#include "IMonoAliases.h"
//! Defines interface of objects that wrap functionality of MonoMethod type.
struct IMonoMethod : public IMonoFunctionalityWrapper
{
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
	//!        IMonoArray *vectorComponents = MonoEnv->WrapArray(componentsArray, true);
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
	//!    mono::vector2 value = Box(Vec2(10, 13));
	//!    mono::vector2 min = Box(Vec2(13, 8));
	//!    mono::vector2 max = Box(Vec2(30, 12));
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
	//!    mono::vector3 axis = Box(Vec3(0, 0, 1));
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
	//! Gets the name of the method.
	__declspec(property(get = GetName)) const char *Name;
	//! Gets number of arguments this method accepts.
	__declspec(property(get = GetParameterCount)) int ParameterCount;

	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! Examples:
	//!
	//! @code{.cpp}
	//! // Get System.Int32 class.
	//! IMonoClass *int32Class = MonoEnv->CoreLibrary->GetClass("System", "Int32");
	//! // Get static method "Parse".
	//! IMonoMethod *parseMethod = int32Class->GetMethod("Parse", "System.String");
	//! // Invoke it using local array of parameters.
	//! void *stackedParams[1];
	//! stackedParams[0] = MonoEnv->ToManagedString("123");
	//! mono::int32 parsedBoxedNumber = parseMethod->Invoke(nullptr, stackedParams);
	//!
	//! // Get "ToString" method, note that, that is a virtual method.
	//! IMonoMethod *toStringMethod = int32Class->GetMethod("ToString", 0);
	//! // Unbox the result of previous invocation.
	//! int parsedNumber = Unbox<int>(parsedBoxedNumber);
	//! // Invoke it without polymorphing it. Note passing a pointer to a local variable as an instance.
	//! mono::string resultNoPolymorphism = toStringMethod->Invoke(&parsedNumber, nullptr, false);
	//! // Invoke it with polymorphing.
	//! mono::string resultPolymorphism = toStringMethod->Invoke(&parsedNumber, nullptr, true);
	//! // Convert results to Null-terminated strings.
	//! const char *resultNoPolymorphismNT = MonoEnv->ToNativeString(resultNoPolymorphism);
	//! const char *resultPolymorphismNT =   MonoEnv->ToNativeString(resultPolymorphism);
	//! // This will print "System.String".
	//! CryLogAlways(resultNoPolymorphismNT);
	//! // This will print "123".
	//! CryLogAlways(resultPolymorphismNT);
	//! // Delete string that are no use anymore.
	//! delete resultNoPolymorphismNT;
	//! delete resultPolymorphismNT;
	//!
	//! // Create an array that will be used to get the method and invoke it.
	//! void *setRotationParams[2];
	//! float angle = 1.0f;							// Angle of rotation.
	//! Vec3 axis(0, 0, 1);							// Axis of rotation.
	//! setRotationParams[0] = &angle;
	//! setRotationParams[1] = &axis;
	//! // Get Matrix33 class.
	//! IMonoClass *matrix33Class = MonoEnv->Cryambly->GetClass("CryCil.Mathematics", "Matrix33");
	//! // Get "SetRotationAroundAxis" method.
	//! IMonoMethod *setRotationMethod = matrix33Class->GetMethod("SetRotationAroundAxis", "System.Single,CryCil.Mathematics.Vector3");
	//! // Create a matrix to invoke this method on.
	//! Matrix33 mat;
	//! mat.SetZero();
	//! // Invoke it using the same array.
	//! setRotationMethod->Invoke(&mat, setRotationParams);
	//! @endcode
	//!
	//! @param object    Pointer to the instance to use, if this method is not
	//!                  static, it can be null otherwise. If you want to invoke
	//!                  this method on an instance of value type, you should either
	//!                  pass the pointer to it in unmanaged memory, or unbox it
	//!                  and pass the returned pointer.
	//! @param params    Pointer to the mono array of parameters to pass to the method.
	//!                  Pass null, if method can accept no arguments.
	//! @param polymorph Indicates whether we need to invoke a virtual method,
	//!                  that is specific to the instance.
	VIRTUAL_API virtual mono::object Invoke
		(
		void *object,
		IMonoArray *params = nullptr,
		mono::exception *exc = nullptr,
		bool polymorph = false
		) = 0;
	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature, you can pass null
	//! as object parameter, and that can work, if extension method is not using the
	//! instance. It's up to you to find uses for that minor detail.
	//!
	//! Examples:
	//!
	//! @code{.cpp}
	//! // Get System.Int32 class.
	//! IMonoClass *int32Class = MonoEnv->CoreLibrary->GetClass("System", "Int32");
	//! // Get static method "Parse".
	//! IMonoMethod *parseMethod = int32Class->GetMethod("Parse", "System.String");
	//! // Invoke it using object[] array of parameters.
	//! IMonoArray *parseParams[1];
	//! parseParams->At<mono::string>(0) = MonoEnv->ToManagedString("123");
	//! mono::int32 parsedBoxedNumber = parseMethod->Invoke(nullptr, parseParams);
	//!
	//! // Get "ToString" method, note that, that is a virtual method.
	//! IMonoMethod *toStringMethod = int32Class->GetMethod("ToString", 0);
	//! // Unbox the result of previous invocation.
	//! int parsedNumber = Unbox<int>(parsedBoxedNumber);
	//! // Invoke it without polymorphing it. Note passing a pointer to a local variable as an instance.
	//! mono::string resultNoPolymorphism = toStringMethod->Invoke(&parsedNumber, nullptr, false);
	//! // Invoke it with polymorphing.
	//! mono::string resultPolymorphism = toStringMethod->Invoke(&parsedNumber, nullptr, true);
	//! // Convert results to Null-terminated strings.
	//! const char *resultNoPolymorphismNT = MonoEnv->ToNativeString(resultNoPolymorphism);
	//! const char *resultPolymorphismNT =   MonoEnv->ToNativeString(resultPolymorphism);
	//! // This will print "System.String".
	//! CryLogAlways(resultNoPolymorphismNT);
	//! // This will print "123".
	//! CryLogAlways(resultPolymorphismNT);
	//! // Delete string that are no use anymore.
	//! delete resultNoPolymorphismNT;
	//! delete resultPolymorphismNT;
	//!
	//! // Create an array that will be used to get the method and invoke it.
	//! IMonoArray *setRotationParams = MonoEnv->CreateArray(2, true);
	//! setRotationParams->At<mono::float32>(0) = Box(1.0f);			// Angle of rotation.
	//! setRotationParams->At<mono::vector3>(1) = Box(Vec3(0, 0, 1));	// Axis of rotation.
	//! // Get Matrix33 class.
	//! IMonoClass *matrix33Class = MonoEnv->Cryambly->GetClass("CryCil.Mathematics", "Matrix33");
	//! // Get "SetRotationAroundAxis" method.
	//! IMonoMethod *setRotationMethod = matrix33Class->GetMethod("SetRotationAroundAxis", setRotationParams);
	//! // Create a matrix to invoke this method on.
	//! Matrix33 mat;
	//! mat.SetZero();
	//! // Invoke it using the same array.
	//! setRotationMethod->Invoke(&mat, setRotationParams);
	//! // Release parameters array so GC can collect it.
	//! setRotationParams->Release();
	//! @endcode
	//!
	//! @param object     Pointer to the instance to use, if this method is not
	//!                   static, it can be null otherwise. If you want to invoke
	//!                   this method on an instance of value type, you should either
	//!                   pass the pointer to it in unmanaged memory, or unbox it
	//!                   and pass the returned pointer.
	//! @param params     Pointer to the array of parameters to pass to the method.
	//!                   Pass null, if method can accept no arguments.
	//! @param polymorph  Indicates whether we need to invoke a virtual method,
	//!                   that is specific to the instance.
	VIRTUAL_API virtual mono::object Invoke
		(
		void *object,
		void **params = nullptr,
		mono::exception *exc = nullptr,
		bool polymorph = false
		) = 0;

	VIRTUAL_API virtual void *GetThunk() = 0;
	VIRTUAL_API virtual const char *GetName() = 0;
	VIRTUAL_API virtual int GetParameterCount() = 0;
};