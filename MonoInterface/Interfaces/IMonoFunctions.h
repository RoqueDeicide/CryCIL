#pragma once

#include "IMonoAliases.h"

struct _MonoMethod;

//! API for all Mono functions.
struct IMonoFunctions
{
	virtual ~IMonoFunctions() {}

	//! Attempts to get the wrapper for a class where given method is declared.
	VIRTUAL_API virtual IMonoClass *GetDeclaringClass(_MonoMethod *method) = 0;
	//! Attempts to get the name of the method.
	VIRTUAL_API virtual const char *GetName(_MonoMethod *method) = 0;

	//! Registers a new internal call.
	//!
	//! There are 2 ways of invoking unmanaged code from managed one:
	//!     1) Platform Invoke: Allows invocation of functions exported by unmanaged DLLs
	//!                         and provides easy marshaling for all data. Its main drawback
	//!                         is a huge cost of invocation itself which is about 100 times
	//!                         slower then invocation of normal method.
	//!
	//!     2) Internal Call:   Does not allow invocation of exported functions, requires
	//!                         internal unmanaged code to register the call, before it can be
	//!                         done. Also all arguments are passed using BLT with no extra
	//!                         data processing and conversion. However that is the cost of
	//!                         much faster invocation (speed rivals invocation of normal
	//!                         methods).
	//!
	//! Rules of definition:
	//!     1) Both parameters and the result are passed using BLT, therefore you have to
	//!        make sure the types of arguments and the result in C# and C++ are blittable.
	//!
	//!     2) There are no requirements specified for the calling convention, however
	//!        using __cdecl is recommended.
	//!
	//!     3) Any pointers to managed objects should be either unboxed (if they are boxed value-type
	//!        objects), wrapped into IMonoObject (when using a general objects) or IMonoArray when using
	//!        arrays or any other built-in wrapper.
	//!
	//!     4) Passing an argument using ref or out keyword causes a pointer to be passed.
	//!
	//!     5) Returning a non-primitive struct object doesn't require unboxing.
	//!
	//!     6) When working with internal calls for instance methods, the first argument is a pointer to
	//!        the object (no boxing/unboxing required).
	//!
	//!     7) When constructor is an internal call, its native representation is a member function with
	//!        constructor's signature that doesn't return anything. Names of all constructors are ".ctor".
	//!
	//!     8) When a property is represented by internal calls, the property itself must be defined as
	//!        extern and each of the accessors must be marked with
	//!        [MethodImpl(MethodImplOptions.InternalCall)] attribute and their native names are
	//!        get_<name of the property> for getter and set_<name of the property> for setter.
	//!        Getter's signature is <type of the property> get_<name of the property>().
	//!        Setter's signature is void set_<name of the property>(<type of the property> value).
	//!        Add pointers to objects to above to signatures if the property is not static.
	//!        Structs cannot contain internal call member properties.
	//!
	//! Examples:
	//!
	//! With built-in types only:
	//!
	//! Signatures:
	//! C#:  [MethodImpl(MethodImplOptions.InternalCall)]
	//!      extern internal static float GetTerrainElevation(float positionX, float positionY);
	//! C++: float GetTerrainElevation(float x, float y);
	//!
	//! C++ implementation:
	//! @code{.cpp}
	//! {
	//!     return gEnv->p3DEngine->GetTerrainElevation(x, y);
	//! }
	//! @endcode
	//!
	//! C# invocation:
	//! @code{.cs}
	//! {
	//!     float elevationAt3And5 = (ClassName).GetTerrainElevation(3, 5);
	//! }
	//! @endcode
	//!
	//! With custom structures:
	//!
	//! Signatures:
	//! C#:  [MethodImpl(MethodImplOptions.InternalCall)]
	//!      extern internal static float CalculateArea(Vector2 leftBottom, Vector2 rightTop);
	//! C++: float CalculateArea(Vec2 leftBottom, Vec2 rightTop);
	//!
	//! C++ implementation:
	//! @code{.cpp}
	//! {
	//!     // Calculate area.
	//!     return abs((leftBottom.x - rightTop.x) * (leftBottom.y - rightTop.y));
	//! }
	//! @endcode
	//!
	//! C# invocation:
	//! @code{.cs}
	//! {
	//!     float area = (ClassName).CalculateArea(new Vector2(2), new Vector2(4, 5));
	//! }
	//! @endcode
	//!
	//! With custom structures and managed objects:
	//!
	//! Signatures:
	//! C#:  [MethodImpl(MethodImplOptions.InternalCall)]
	//!      extern internal static Material LoadMaterial(string name, ref MaterialParameters params);
	//! C++: mono::object LoadMaterial(mono::string name, MaterialParameters *params);
	//!
	//! C++ implementation:
	//! @code{.cpp}
	//! {
	//!     // Get the handle to the loaded material.
	//!     IMaterial *materialHandle = m_pMaterialManager->LoadMaterial
	//!     (
	//!         MonoEnv->ToNativeString(name),
	//!         params->makeIfNotFound,
	//!         params->nonRemovable
	//!     );
	//!     // Create an array for Material construction parameters.
	//!     IMonoArray *ctorParams = MonoEnv->CreateArray(1, false);
	//!     // Fill it.
	//!     ctorParams->SetItem(0, BoxPtr(materialHandle));
	//!     // Create the object.
	//!     return MonoEnv->CreateObject
	//!     (
	//!         MonoEnv->Cryambly,          // Assembly where the type is defined.
	//!         "CryCil.Engine.Materials",  // Name space where the type is defined.
	//!         "Material",                 // Name of the type to instantiate.
	//!         ctorParams                  // Arguments.
	//!     );
	//! }
	//! @endcode
	//!
	//! C# invocation:
	//! @code{.cs}
	//! {
	//!     // Declare variable.
	//!     MaterialParameters params = new MaterialParameters
	//!     {
	//!         MakeIfNotFound = true,
	//!         NonRemovable = false
	//!     };
	//!     // Pass the variable by reference.
	//!     Material deathMetal = (ClassName).LoadMaterial("DeathMetal", ref params);
	//! }
	//! @endcode
	//!
	//! With custom structures passed with out keyword reference:
	//!
	//! Type definitions:
	//! C#:
	//! @code{.cs}
	//!      [StructLayout(LayoutKind.Sequential)]
	//!      struct Data
	//!      {
	//!          int number;
	//!      }
	//! @endcode
	//! C++:
	//! @code{.cpp}
	//! struct Data { int number; };
	//! @endcode
	//!
	//! Signatures:
	//! C#:  [MethodImpl(MethodImplOptions.InternalCall)]
	//!      extern internal static bool TryGet(string name, out Data data);
	//! C++: bool TryGet(mono::string name, Data *data);
	//!
	//! C++ implementation:
	//! @code{.cpp}
	//! {
	//!     // Modify the object.
	//!     data->number = 5;
	//!     return true;
	//! }
	//! @endcode
	//!
	//! C# invocation:
	//! @code{.cs}
	//! {
	//!     // Declare variable.
	//!     Data data;
	//!     // Pass those variables by reference.
	//!     bool success = (ClassName).TryGet("Not used", out data);
	//! }
	//! @endcode
	//!
	//! @param nameSpace       Name space where the class is located.
	//! @param className       Name of the class where managed method is declared.
	//! @param name            Name of the managed method.
	//! @param functionPointer Pointer to unmanaged thunk that needs to be exposed to Mono code.
	VIRTUAL_API virtual void AddInternalCall(const char *nameSpace, const char *className, const char *name,
											 void *functionPointer) = 0;
	//! Tries to get the function pointer that was registered as internal call.
	//!
	//! @param func Mono function that has been assigned to the internal call.
	VIRTUAL_API virtual void *LookupInternalCall(const IMonoFunction *func) = 0;

	//! Invokes provided method.
	//!
	//! @param func      Pointer to Mono runtime representation of the method.
	//! @param object    A pointer to the object to use as a target when invoking an instance method or null
	//!                  when invoking a static method (all extension methods are static).
	//! @param args      Pointer to the array of pointers to object to pass as arguments.
	//! @param ex        Pointer to the pointer to the exception object that will become valid, if unhandled
	//!                  exception gets thrown.
	//! @param polymorph Indicates whether attempt should be made to use late binding when invoking this
	//!                  function.
	VIRTUAL_API virtual mono::object InternalInvoke(_MonoMethod *func, void *object, void **args,
													mono::exception *ex, bool polymorph) = 0;
	//! Invokes provided method.
	//!
	//! @param func      Pointer to Mono runtime representation of the method.
	//! @param object    A pointer to the object to use as a target when invoking an instance method or null
	//!                  when invoking a static method (all extension methods are static).
	//! @param args      A wrapper for a Mono array that contains arguments to pass to the function.
	//! @param ex        Pointer to the pointer to the exception object that will become valid, if unhandled
	//!                  exception gets thrown.
	//! @param polymorph Indicates whether attempt should be made to use late binding when invoking this
	//!                  function.
	VIRTUAL_API virtual mono::object InternalInvokeArray(_MonoMethod *func, void *object, IMonoArray<> &args,
														 mono::exception *ex, bool polymorph) = 0;

	//! @see IMonoFunction::UnmanagedThunk property.
	VIRTUAL_API virtual void        *GetUnmanagedThunk(_MonoMethod *func) = 0;
	//! @see IMonoFunction::RawThunk property.
	VIRTUAL_API virtual void        *GetRawThunk(_MonoMethod *func) = 0;
	//! Parses signature of given Mono method and fills a number of lists.
	VIRTUAL_API virtual int          ParseSignature(_MonoMethod *func, List<Text> &names, Text &params) = 0;
	//! @see IMonoFunction::ParameterClasses property.
	VIRTUAL_API virtual void         GetParameterClasses(_MonoMethod *func, List<IMonoClass *> &classes) = 0;
	//! @see IMonoFunction::ReflectionObject property.
	VIRTUAL_API virtual mono::object GetReflectionObject(_MonoMethod *func) = 0;
};