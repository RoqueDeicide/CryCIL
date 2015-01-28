#pragma once

#include "IMonoAliases.h"

//! Allows to look for specific
struct TypeSpec
{
	IMonoClass *Class;			//!< The class that is being specified.
	bool IsByRef;				//!< Indicates whether this is a type that is passed by reference (with "ref" and "out" keywords in C#).
	bool IsPointer;				//!< Indicates whether this is a pointer type.
	TypeSpec(IMonoClass *klass, bool isByRef = false, bool isPointer = false)
	{
		this->Class = klass;
		this->IsByRef = isByRef;
		this->IsPointer = isPointer;
	}
};

//! Defines interface of objects that wrap functionality of MonoClass type.
struct IMonoClass : public IMonoFunctionalityWrapper
{
	//! Gets the name of this class.
	__declspec(property(get = GetName)) const char *Name;
	//! Gets full name of this class.
	__declspec(property(get = GetName)) const char *FullName;
	//! Gets the name space where this class is defined.
	__declspec(property(get = GetNameSpace)) const char *NameSpace;
	//! Gets assembly where this class is defined.
	__declspec(property(get = GetAssembly)) IMonoAssembly *Assembly;
	//! Gets the class where this class is defined.
	__declspec(property(get = GetBase)) IMonoClass *Base;
	//! Creates an instance of this class.
	//!
	//! @param args Arguments to pass to the constructor, can be null if latter has no parameters.
	VIRTUAL_API virtual mono::object CreateInstance(IMonoArray *args = nullptr) = 0;
	//! Gets method that can accept arguments of specified types.
	//!
	//! @param name  Name of the method to get.
	//! @param types An array of types that specify method signature to use.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, List<TypeSpec> *types = nullptr) = 0;
	//! Gets the first method that matches given description.
	//!
	//! @param name       Name of the method to find.
	//! @param paramCount Number of arguments the method should take.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, int paramCount) = 0;
	//! Gets the method that matches given description.
	//!
	//! General rules for constructing the text of parameter types:
	//!     1) Don't put any parenthesis into the string.
	//!     2) Use full names for types.
	//!     3) Don't use parameter names.
	//!     4) Parameter declared with "params" keyword are simple arrays.
	//!
	//! Examples:
	//!
	//! C# method signature: CreateInstance(System.Type, params System.Object[]);
	//! C++ search: GetMethod("CreateInstance", "System.Type,System.Object[]");
	//!
	//! @param name   Name of the method to find.
	//! @param params Text that describes types arguments the method should take.
	//!
	//! @returns A pointer to the wrapper to the found method. Null is returned if
	//!          no method matching the description was found.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, const char *params) = 0;
	//! Gets an array of methods that matches given description.
	//!
	//! @param name       Name of the methods to find.
	//! @param paramCount Number of arguments the methods should take.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//!
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	VIRTUAL_API virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount) = 0;
	//! Gets an array of overload of the method.
	//!
	//! @param name       Name of the method which overloads to find.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	VIRTUAL_API virtual IMonoMethod **GetMethods(const char *name, int &foundCount) = 0;
	//! Gets the value of the object's field.
	//!
	//! @param obj   Object which field to get. Use nullptr when working with a static field.
	//! @param name Name of the field which value to get.
	//!
	//! @seealso IMonoHandle::SetField
	VIRTUAL_API virtual void GetField(mono::object obj, const char *name, void *value) = 0;
	//! Sets the value of the object's field.
	//!
	//! @param obj   Object which field to set. Use nullptr when working with a static field.
	//! @param name  Name of the field which value to set.
	//! @param value New value to assign to the field.
	//!
	//! @seealso IMonoHandle::SetField
	VIRTUAL_API virtual void SetField(mono::object obj, const char *name, void *value) = 0;
	//! Gets one of the properties defined in this class.
	//!
	//! Delete returned object once you don't need it anymore.
	//!
	//! @param name Name of the property to get.
	VIRTUAL_API virtual IMonoProperty *GetProperty(const char *name) = 0;
	//! Gets one of the events defined in this class.
	//!
	//! Delete returned object once you don't need it anymore.
	//!
	//! @param name Name of the event to get.
	VIRTUAL_API virtual IMonoEvent *GetEvent(const char *name) = 0;
	//! Gets the class or struct that is defined in this one.
	//!
	//! @param name Name of the class to get.
	VIRTUAL_API virtual IMonoClass *GetNestedType(const char *name) = 0;
	//! Determines whether this class implements from specified class.
	//!
	//! @param nameSpace Full name of the name space where the class is located.
	//! @param className Name of the class.
	//!
	//! @returns True, if this class is a subclass of specified one.
	VIRTUAL_API virtual bool Inherits(const char *nameSpace, const char *className) = 0;
	//! Determines whether this class implements a certain interface.
	//!
	//! @param nameSpace         Full name of the name space where the interface is located.
	//! @param interfaceName     Name of the interface.
	//! @param searchBaseClasses Indicates whether we should look if base classes implement
	//!                          this interface.
	//!
	//! @returns True, if this class implements specified interface.
	VIRTUAL_API virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses = true) = 0;
	//! Boxes given value.
	//!
	//! @returns Null if this class is not a value-type, or reference to the boxed object, if it is.
	VIRTUAL_API virtual mono::object Box(void *value) = 0;

	VIRTUAL_API virtual const char *GetName() = 0;
	VIRTUAL_API virtual const char *GetNameSpace() = 0;
	VIRTUAL_API virtual const char *GetFullName() = 0;
	VIRTUAL_API virtual IMonoAssembly *GetAssembly() = 0;
	VIRTUAL_API virtual IMonoClass *GetBase() = 0;
};