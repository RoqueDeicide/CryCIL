#pragma once

#include "IMonoAliases.h"

//! Represents a constructor function in Mono.
//!
//! Constructors are always instance methods and when invoked using Invoke() with null passed as an
//! instance, they create a new object and initialize it. You cannot polymorph a constructor.
struct IMonoConstructor : public IMonoFunction
{
protected:
	IMonoConstructor(_MonoMethod *method, IMonoClass *klass = nullptr) : IMonoFunction(method, klass) {}
public:
	//! Creates a new object of the type where this class is defined.
	//!
	//! Use this method to create and initialize reference-type objects.
	//!
	//! @param ex A pointer to the object that will contain the reference to the caught unhandled exception
	//!           object, if there is one. If null is provided, then the exception will be handled by this
	//!           method in a default manner.
	//!
	//! @returns A reference to the created and initialized object.
	VIRTUAL_API virtual mono::object Create(mono::exception *ex = nullptr) = 0;
	//! Creates a new object of the type where this class is defined.
	//!
	//! Use this method to create and initialize reference-type objects.
	//!
	//! @param args A Mono array of arguments to pass to the constructor.
	//! @param ex   A pointer to the object that will contain the reference to the caught unhandled exception
	//!             object, if there is one. If null is provided, then the exception will be handled by this
	//!             method in a default manner.
	//!
	//! @returns A reference to the created and initialized object.
	VIRTUAL_API virtual mono::object Create(IMonoArray<> &args, mono::exception *ex = nullptr) = 0;
	//! Creates a new object of the type where this class is defined.
	//!
	//! Use this method to create and initialize reference-type objects.
	//!
	//! @param args A static array of arguments to pass to the constructor.
	//! @param ex   A pointer to the object that will contain the reference to the caught unhandled exception
	//!             object, if there is one. If null is provided, then the exception will be handled by this
	//!             method in a default manner.
	//!
	//! @returns A reference to the created and initialized object.
	VIRTUAL_API virtual mono::object Create(void **args, mono::exception *ex = nullptr) = 0;

	//! Initializes an object of the type where this class is defined.
	//!
	//! Use this method to initialize value-type objects.
	//!
	//! @param obj A pointer to the object to initialize.
	//! @param ex  A pointer to the object that will contain the reference to the caught unhandled exception
	//!            object, if there is one. If null is provided, then the exception will be handled by this
	//!            method in a default manner.
	//!
	//! @returns A reference to the created and initialized object.
	VIRTUAL_API virtual void Initialize(void *obj, mono::exception *ex = nullptr) = 0;
	//! Initializes an object of the type where this class is defined.
	//!
	//! Use this method to initialize value-type objects.
	//!
	//! @param obj  A pointer to the object to initialize.
	//! @param args A Mono array of arguments to pass to the constructor.
	//! @param ex   A pointer to the object that will contain the reference to the caught unhandled exception
	//!             object, if there is one. If null is provided, then the exception will be handled by this
	//!             method in a default manner.
	//!
	//! @returns A reference to the created and initialized object.
	VIRTUAL_API virtual void Initialize(void *obj, IMonoArray<> &args, mono::exception *ex = nullptr) = 0;
	//! Initializes an object of the type where this class is defined.
	//!
	//! Use this method to initialize value-type objects.
	//!
	//! @param obj  A pointer to the object to initialize.
	//! @param args A static array of arguments to pass to the constructor.
	//! @param ex   A pointer to the object that will contain the reference to the caught unhandled exception
	//!             object, if there is one. If null is provided, then the exception will be handled by this
	//!             method in a default manner.
	//!
	//! @returns A reference to the created and initialized object.
	VIRTUAL_API virtual void Initialize(void *obj, void **args, mono::exception *ex = nullptr) = 0;
};

__forceinline IMonoConstructor *IMonoFunction::ToCtor()
{
	return static_cast<IMonoConstructor *>(this);
}

__forceinline IMonoConstructor *IMonoClass::GetConstructor(int paramCount)
{
	return this->GetFunction(".ctor", paramCount)->ToCtor();
}

__forceinline IMonoConstructor *IMonoClass::GetConstructor(IMonoArray<> &types)
{
	return this->GetFunction(".ctor", types)->ToCtor();
}

__forceinline IMonoConstructor *IMonoClass::GetConstructor(List<IMonoClass *> &classes)
{
	return this->GetFunction(".ctor", classes)->ToCtor();
}

__forceinline IMonoConstructor *IMonoClass::GetConstructor(List<ClassSpec> &specifiedClasses)
{
	return this->GetFunction(".ctor", specifiedClasses)->ToCtor();
}

__forceinline IMonoConstructor *IMonoClass::GetConstructor(const char *params)
{
	return this->GetFunction(".ctor", params)->ToCtor();
}

__forceinline IMonoConstructor *IMonoClass::GetConstructor(List<const char *> &paramTypeNames)
{
	return this->GetFunction(".ctor", paramTypeNames)->ToCtor();
}