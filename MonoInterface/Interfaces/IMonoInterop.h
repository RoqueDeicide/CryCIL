#pragma once

#include "IMonoAliases.h"
#include "IMonoSystemListener.h"

// A Regex pattern for detecting method signatures for internal calls: "static\s+\S+\s*\**\s*\&*\s*([a-zA-Z0-9_]+)\(.*\);"

// Text for replacing method signatures for internal calls: "REGISTER_METHOD($1);"

//! Base class for a template class that defines interface for Mono interops.
//!
//! This class is here to reduce amount of code that has to be duplicated when
//! specializing aforementioned template.
struct IMonoInteropBase : public IMonoSystemListener
{
	//! Registers an interop method.
	//!
	//! @param methodName      Name of the method to register.
	//! @param functionPointer Pointer to the method that will be called internally.
	virtual void RegisterInteropMethod(const char *methodName, void *functionPointer)
	{
		this->monoInterface->Functions->AddInternalCall
			(this->GetInteropNameSpace(), this->GetInteropClassName(), methodName, functionPointer);
	}
	//! Returns the name of the class that will declare managed counter-parts
	//! of the internal calls.
	virtual const char *GetInteropClassName() = 0;
	//! Returns the name space where the class that will declare managed counter-parts
	//! of the internal calls is declared.
	virtual const char *GetInteropNameSpace() = 0;
	//! Unnecessary for most interops.
	virtual void OnPreInitialization() override
	{}
	//! Unnecessary for most interops.
	virtual void OnRunTimeInitializing() override {}
	//! Unnecessary for most interops.
	virtual void OnCryamblyInitilizing() override {}
	//! Unnecessary for most interops.
	virtual void OnCompilationStarting() override
	{}
	//! Unnecessary for most interops.
	virtual void OnCompilationComplete(bool) override
	{}
	//! Unnecessary for most interops.
	virtual List<int> *GetSubscribedStages() override
	{
		return nullptr;
	}
	//! Unnecessary for most interops.
	virtual void OnInitializationStage(int) override
	{}
	//! Unnecessary for most interops.
	virtual void OnCryamblyInitilized() override
	{}
	//! Unnecessary for most interops.
	virtual void OnPostInitialization() override
	{}
	//! Unnecessary for most interops.
	virtual void Update() override
	{}
	//! Unnecessary for most interops.
	virtual void PostUpdate() override
	{}
	//! Unnecessary for most interops.
	virtual void Shutdown() override
	{}
};

//! Interface of objects that specialize on setting up interops between C++ and Mono.
//!
//! The earliest time for registration of internal calls is during invocation
//! of OnRunTimeInitialized, which is why it is kept abstract.
//!
//! @typeparam callRegistrationOnly Indicates whether this interop object will unregister
//!                                 and destroy itself after adding internal calls to Mono.
//! @typeparam useMonoEnv Indicates whether this interop object will override SetInterface
//!                       to not save IMonoInterface implementation to the internal field.
template<bool callRegistrationOnly, bool useMonoEnv = false>
struct IMonoInterop : public IMonoInteropBase
{};
//! Specialization of IMonoInterop<,> template that behaves in a default manner.
template<> struct IMonoInterop<false, false> : public IMonoInteropBase
{
};
//! Specialization of IMonoInterop<,> template that unregisters and destroys itself
//! after registration of internal calls.
template<> struct IMonoInterop<true, false> : public IMonoInteropBase
{
	//! Unregisters itself and commits suicide.
	virtual void OnCryamblyInitilizing() override
	{
		this->monoInterface->RemoveListener(this);
		delete this;
	}
};
//! Specialization of IMonoInterop<,> template that relies on using MonoEnv variable
//! instead of internal field.
template<> struct IMonoInterop < false, true > : public IMonoInteropBase
{
	//! No saving.
	virtual void SetInterface(IMonoInterface *) override {}
	//! Registers internal calls through MonoEnv, since internal field is a null pointer.
	virtual void RegisterInteropMethod(const char *methodName, void *functionPointer) override
	{
		MonoEnv->Functions->AddInternalCall
			(this->GetInteropNameSpace(), this->GetInteropClassName(), methodName, functionPointer);
	}
};

//! Registers an interop method within a method of a class that is derived from IMonoInterop.
//!
//! Use this macro when you want to register an internal call for a method that has no overloads.
//!
//! Examples:
//!
//! Managed class where internal call is declared.
//!
//! @code{.cs}
//! internal static class ClassWithInternalCalls
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(bool smth);
//! }
//! @endcode
//!
//! C++ class where internal call is implemented and registered.
//!
//! @code{.cpp}
//! #include "IMonoInterface.h"
//!
//! struct ClassWithInternalCallsInterop : public IMonoInterop<true, false>
//! {
//!     virtual const char *GetInteropClassName() override { return "ClassWithInternalCalls"; }
//!     virtual const char *GetInteropNameSpace() override { return "SomeNameSpace"; }
//!
//!     virtual void OnRunTimeInitialized() override
//!     {
//!         REGISTER_METHOD(Method);
//!     }
//!
//!     static void Method(bool smth);
//! }
//! @endcode
//!
//! @param method Method that will be invoked via internal call and which name is used as a name of the
//!               managed method.
#define REGISTER_METHOD(method) this->RegisterInteropMethod(#method, method)
//! Registers an interop method within a method of a class that is derived from IMonoInterop.
//!
//! Use this macro when you want to register an internal call for a method that has multiple overloads
//! to specify which overload to use. You also need to use this macro when names of managed and native
//! method are different.
//!
//! Examples:
//!
//! Managed classes where internal calls are declared.
//!
//! @code{.cs}
//! internal static class ClassWithInternalCalls
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(bool smth);
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(int smth);
//! }
//! @endcode
//!
//! C++ class where internal call is implemented and registered.
//!
//! @code{.cpp}
//! #include "IMonoInterface.h"
//!
//! struct ClassWithInternalCallsInterop : public IMonoInterop<true, false>
//! {
//!     virtual const char *GetInteropClassName() override { return "ClassWithInternalCalls"; }
//!     virtual const char *GetInteropNameSpace() override { return "SomeNameSpace"; }
//!
//!     virtual void OnRunTimeInitialized() override
//!     {
//!         REGISTER_METHOD_N("Method(bool)", Method);
//!         REGISTER_METHOD_N("Method(int)",  Method2);
//!     }
//!
//!     static void Method(bool smth);
//!     static void Method2(int smth);
//! }
//! @endcode
//!
//! @param name   Name that is used as a name of the managed method.
//! @param method Method that will be invoked via internal call.
#define REGISTER_METHOD_N(name, method) this->RegisterInteropMethod(name, method)
//! Registers an interop method.
//!
//! Use this macro when you want to register an internal call for a method that is defined in a different
//! class to one specified in declaration of the interop class.
//!
//! Examples:
//!
//! Managed classes where internal calls are declared.
//!
//! @code{.cs}
//! internal static class ClassWithInternalCalls
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(bool smth);
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(int smth);
//! }
//! internal static class ClassWithInternalCalls2
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(bool smth);
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern void Method(uint smth);
//! }
//! @endcode
//!
//! C++ class where internal call is implemented and registered.
//!
//! @code{.cpp}
//! #include "IMonoInterface.h"
//!
//! struct ClassWithInternalCallsInterop : public IMonoInterop<true, false>
//! {
//!     // This class registers internal calls for multiple managed classes, therefore this method doesn't
//!     // really matter.
//!     virtual const char *GetInteropClassName() override { return ""; }
//!     virtual const char *GetInteropNameSpace() override { return "SomeNameSpace"; }
//!
//!     virtual void OnRunTimeInitialized() override
//!     {
//!         const char *name_space = this->GetInteropNameSpace();
//!         REGISTER_METHOD_NCN(name_space, "ClassWithInternalCalls",  "Method(bool)", Method);
//!         REGISTER_METHOD_NCN(name_space, "ClassWithInternalCalls",  "Method(int)",  Method2);
//!         REGISTER_METHOD_NCN(name_space, "ClassWithInternalCalls2", "Method(bool)", Method);
//!         REGISTER_METHOD_NCN(name_space, "ClassWithInternalCalls2", "Method(uint)", Method3);
//!     }
//!
//!     static void Method(bool smth);
//!     static void Method2(int smth);
//!     static void Method3(uint smth);
//! }
//! @endcode
//!
//! @param name_space Name space contains the class where managed method is defined.
//! @param class_name Name of the class where managed method is defined.
//! @param name       Name that is used as a name of the managed method.
//! @param method     Method that will be invoked via internal call.
#define REGISTER_METHOD_NCN(name_space, class_name, name, method) \
	(this->monoInterface ? this->monoInterface : MonoEnv)->Functions->AddInternalCall(name_space, class_name, \
																					  name, method);
//! Registers an interop constructor within a method of a class that is derived from IMonoInterop.
//!
//! Use this macro when you want to register an internal call for a constructor that has no overloads.
//!
//! Examples:
//!
//! Managed class where internal call is declared.
//!
//! @code{.cs}
//! internal static class ClassWithInternalCalls
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls(bool smth);
//! }
//! @endcode
//!
//! C++ class where internal call is implemented and registered.
//!
//! @code{.cpp}
//! #include "IMonoInterface.h"
//!
//! struct ClassWithInternalCallsInterop : public IMonoInterop<true, false>
//! {
//!     virtual const char *GetInteropClassName() override { return "ClassWithInternalCalls"; }
//!     virtual const char *GetInteropNameSpace() override { return "SomeNameSpace"; }
//!
//!     virtual void OnRunTimeInitialized() override
//!     {
//!         REGISTER_CTOR(Method);
//!     }
//!
//!     static void Method(bool smth);
//! }
//! @endcode
//!
//! @param method Method that will be invoked via internal call.
#define REGISTER_CTOR(method) this->RegisterInteropMethod(".ctor", method)
//! Registers an interop constructor within a method of a class that is derived from IMonoInterop.
//!
//! Use this macro when you want to register an internal call for a constructor that has multiple overloads
//! to specify which overload to use.
//!
//! Examples:
//!
//! Managed classes where internal calls are declared.
//!
//! @code{.cs}
//! internal static class ClassWithInternalCalls
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls(bool smth);
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls(int smth);
//! }
//! @endcode
//!
//! C++ class where internal call is implemented and registered.
//!
//! @code{.cpp}
//! #include "IMonoInterface.h"
//!
//! struct ClassWithInternalCallsInterop : public IMonoInterop<true, false>
//! {
//!     virtual const char *GetInteropClassName() override { return "ClassWithInternalCalls"; }
//!     virtual const char *GetInteropNameSpace() override { return "SomeNameSpace"; }
//!
//!     virtual void OnRunTimeInitialized() override
//!     {
//!         REGISTER_CTOR_N("bool", Method);
//!         REGISTER_CTOR_N("int",  Method2);
//!     }
//!
//!     static void Method(bool smth);
//!     static void Method2(int smth);
//! }
//! @endcode
//!
//! @param argTypes Names of types of arguments that are accepted by the constructor.
//! @param method   Method that will be invoked via internal call.
#define REGISTER_CTOR_N(argTypes, method) this->RegisterInteropMethod(".ctor("##argTypes##")", method)
//! Registers an interop constructor.
//!
//! Use this macro when you want to register an internal call for a constructor that is defined in a different
//! class to one specified in declaration of the interop class.
//!
//! Examples:
//!
//! Managed classes where internal calls are declared.
//!
//! @code{.cs}
//! internal static class ClassWithInternalCalls
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls(bool smth);
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls(int smth);
//! }
//! internal static class ClassWithInternalCalls2
//! {
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls2(bool smth);
//!     [MethodImpl(MethodImplOptions.InternalCall)]
//!     internal static extern ClassWithInternalCalls2(uint smth);
//! }
//! @endcode
//!
//! C++ class where internal call is implemented and registered.
//!
//! @code{.cpp}
//! #include "IMonoInterface.h"
//!
//! struct ClassWithInternalCallsInterop : public IMonoInterop<true, false>
//! {
//!     // This class registers internal calls for multiple managed classes, therefore this method doesn't
//!     // really matter.
//!     virtual const char *GetInteropClassName() override { return ""; }
//!     virtual const char *GetInteropNameSpace() override { return "SomeNameSpace"; }
//!
//!     virtual void OnRunTimeInitialized() override
//!     {
//!         const char *name_space = this->GetInteropNameSpace();
//!         REGISTER_CTOR_NCN(name_space, "ClassWithInternalCalls",  "bool", Method);
//!         REGISTER_CTOR_NCN(name_space, "ClassWithInternalCalls",  "int",  Method2);
//!         REGISTER_CTOR_NCN(name_space, "ClassWithInternalCalls2", "bool", Method);
//!         REGISTER_CTOR_NCN(name_space, "ClassWithInternalCalls2", "uint", Method3);
//!     }
//!
//!     static void Method(bool smth);
//!     static void Method2(int smth);
//!     static void Method3(uint smth);
//! }
//! @endcode
//!
//! @param name_space Name space contains the class where constructor is defined.
//! @param class_name Name of the class where constructor is defined.
//! @param argTypes   Names of types of arguments that are accepted by the constructor.
//! @param method     Method that will be invoked via internal call.
#define REGISTER_CTOR_NCN(name_space, class_name, argTypes, method) \
	(this->monoInterface ? this->monoInterface : MonoEnv)->Functions->AddInternalCall(name_space, class_name, \
																					  ".ctor("##argTypes##")", \
																					  method);

//! Specialization of IMonoInterop<,> template that relies on using MonoEnv variable
//! instead of internal field and unregisters and destroys itself after registration
//! of internal calls.
template<> struct IMonoInterop < true, true > : public IMonoInteropBase
{
	//! No saving.
	virtual void SetInterface(IMonoInterface *) override {}
	//! Unregisters itself and commits suicide.
	virtual void OnCryamblyInitilizing() override
	{
		MonoEnv->RemoveListener(this);
		delete this;
	}
	//! Registers internal calls through MonoEnv, since internal field is a null pointer.
	virtual void RegisterInteropMethod(const char *methodName, void *functionPointer) override
	{
		MonoEnv->Functions->AddInternalCall
			(this->GetInteropNameSpace(), this->GetInteropClassName(), methodName, functionPointer);
	}
};