#pragma once

#include "IMonoAliases.h"

//! Represents a method parameter specification.
//!
//! Represents a pair where first value is a pointer to the class wrapper that represents a type of the
//! parameter, and second value is postfix that specifies the kind of parameter's type.
//!
//! Check IMonoClass::GetMethod() overloads for more information.
typedef Pair<IMonoClass *, const char *> ClassSpec;

//! Defines interface of objects that wrap functionality of MonoClass type.
//!
//! General advice: Avoid dealing with generics when using this API: the embedded Mono API is way too
//! limited for this.
struct IMonoClass : public IMonoFunctionalityWrapper
{
	//! Gets the name of this class.
	__declspec(property(get = GetName)) const char *Name;
	//! Gets full name of this class.
	__declspec(property(get = GetFullName)) const char *FullName;
	//! Gets full name of this class.
	//!
	//! If this class is nested its name will be separated from declaring type with "+" instead of ".".
	__declspec(property(get = GetFullNameIL)) const char *FullNameIL;
	//! Gets the name space where this class is defined.
	__declspec(property(get = GetNameSpace)) const char *NameSpace;
	//! Gets assembly where this class is defined.
	__declspec(property(get = GetAssembly)) IMonoAssembly *Assembly;
	//! Gets the class where this class is defined.
	__declspec(property(get = GetBase)) IMonoClass *Base;
	//! Indicates whether this class is a value-type.
	__declspec(property(get = GetIsValueType)) bool IsValueType;
	//! Indicates whether this class is an enumeration.
	__declspec(property(get = GetIsEnum)) bool IsEnum;
	//! Indicates whether this class is a delegate.
	__declspec(property(get = GetIsDelegate)) bool IsDelegate;
	//! Gets the list of fields available through this class.
	__declspec(property(get = GetFields)) ReadOnlyList<IMonoField *> *Fields;
	//! Gets the list of fields available through this class.
	__declspec(property(get = GetProperties)) ReadOnlyList<IMonoProperty *> *Properties;
	//! Gets the list of fields available through this class.
	__declspec(property(get = GetEvents)) ReadOnlyList<IMonoEvent *> *Events;
	//! Gets the list of fields available through this class.
	__declspec(property(get = GetMethods)) ReadOnlyList<IMonoMethod *> *Methods;
	
	//! Gets one of the constructors that can accept specified number of arguments.
	//!
	//! It's not easy to predict which one of the constructors will be acquired when there are several
	//! of them that accept the same number of arguments.
	VIRTUAL_API virtual IMonoConstructor *GetConstructor(int paramCount) = 0;
	//! Gets constructor that can accept arguments of specified types.
	//!
	//! @param types An array of System.Type objects that specify constructor signature to use.
	VIRTUAL_API virtual IMonoConstructor *GetConstructor(IMonoArray *types = nullptr) = 0;
	//! Gets constructor that can accept arguments of specified types.
	//!
	//! Refer to documentation of corresponding GetMethod() overload for details.
	//!
	//! @param classes A list IMonoClass wrappers that specify constructor signature to use.
	VIRTUAL_API virtual IMonoConstructor *GetConstructor(List<IMonoClass *> &classes) = 0;
	//! Gets constructor that can accept arguments of specified types.
	//!
	//! Refer to documentation of corresponding GetMethod() overload for details.
	//!
	//! @param specifiedClasses A list of classes and postfixes that specify constructor signature
	//!                         to use.
	VIRTUAL_API virtual IMonoConstructor *GetConstructor(List<ClassSpec> &specifiedClasses) = 0;
	//! Gets the constructor that matches given description.
	//!
	//! Refer to documentation of corresponding GetMethod() overload for details.
	//!
	//! @param params Text that describes types arguments the method should take.
	//!
	//! @returns A pointer to the wrapper to the found constructor. Null is returned if
	//!          no constructor matching the description was found.
	VIRTUAL_API virtual IMonoConstructor *GetConstructor(const char *params) = 0;
	//! Gets a constructor defined in this class.
	//!
	//! Refer to documentation of corresponding GetMethod() overload for details.
	//!
	//! @param paramTypeNames A list of full type names that specify the parameters the constructor
	//!                       accepts.
	VIRTUAL_API virtual IMonoConstructor *GetConstructor(List<const char *> &paramTypeNames) = 0;
	//! Gets method that can accept arguments of specified types.
	//!
	//! @param name  Name of the method to get.
	//! @param types An array of System.Type objects that specify method signature to use.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr) = 0;
	//! Gets method that can accept arguments of specified types.
	//!
	//! This method does not bother with checking how arguments are passed to the method.
	//!
	//! Use it when you have a lot overloads that just accept simple values.
	//!
	//! Some types are remapped for this method: any arrays are mapped to System.Array and
	//! pointers are mapped to IntPtr.
	//!
	//! For instance:
	//! @code{.cs}
	//! void Add(sbyte);
	//! void Add(short);
	//! void Add(int);
	//! void Add(long);
	//! void Add(int[]);
	//! @endcode
	//!
	//! To get the last one in the above list, use the following code:
	//!
	//! @code{.cpp}
	//! List<IMonoClass *> type = List<IMonoClass *>(1);
	//! type.Add(MonoEnv->CoreLibrary->GetClass("System", "Array"));
	//! klass->GetMethod("Add", type);
	//! @endcode
	//!
	//! @param name    Name of the method to get.
	//! @param classes A list IMonoClass wrappers that specify method signature to use.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, List<IMonoClass *> &classes) = 0;
	//! Gets method that can accept arguments of specified types.
	//!
	//! Postfixes allow you to specify what kind of parameter to use.
	//!
	//! Available postfixes:
	//!     1) &  - specifies that parameter is passed by reference using either "ref" or "out"
	//!             keyword. When combining this postfix with others put it at the end, i.e
	//!             "[,]&" specifies an array with two dimensions that is passed by reference.
	//!     2) *  - specifies that parameter is a pointer. Bare in mind that some pointer types
	//!             may not be allowed.
	//!     1) [] - Specifies an array type. When working with multi-dimensional arrays, put
	//!             N - 1 number of commas between the brackets where N = number of dimensions.
	//!
	//! @param name             Name of the method to get.
	//! @param specifiedClasses A list of classes and postfixes that specify method signature to use.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, List<ClassSpec> &specifiedClasses) = 0;
	//! Gets the method that matches given description.
	//!
	//! The easiest way to learn the signature of the method is to use the following code:
	//!
	//! @code{.cs}
	//!
	//! MethodInfo method = typeof(Foo).GetMethod("Boo");
	//!
	//! StringBuilder builder = new StringBuilder(20);
	//!
	//! Type[] types = method.GetParameters().Select(x = > x.ParameterType).ToArray();
	//!
	//! builder.Append(types[0].FullName);
	//!
	//! for(int i = 1; i < types.Length; i++)
	//! {
	//!     builder.Append(',');
	//!     builder.Append(types[i].FullName);
	//! }
	//!
	//! string ourParameterList = builder.ToString();
	//!
	//! @endcode
	//!
	//! Examples:
	//!
	//! C# method signature: SetupNumber(out int result, ref double value, ref void *ptr, ref object[,] pars, Foo.Boo objectOfNestedType);
	//! C++ search: GetMethod("SetupNumber", "System.Int32&,System.Double&,System.Void*&,System.Object[,]&,Foo+Boo");
	//!
	//! @param name   Name of the method to find.
	//! @param params Text that describes types arguments the method should take.
	//!
	//! @returns A pointer to the wrapper to the found method. Null is returned if
	//!          no method matching the description was found.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, const char *params) = 0;
	//! Gets a method defined in this class.
	//!
	//! Examples:
	//!
	//! C# method signature: SetupNumber(out int result, ref double value, ref void *ptr, ref object[,] pars);
	//!
	//! @code{.cpp}
	//!
	//! List<const char *> typeNames = List(4);
	//!
	//! typeNames.Add("System.Int32&");
	//! typeNames.Add("System.Double&");
	//! typeNames.Add("System.Void*&");
	//! typeNames.Add("System.Object[,]&");
	//!
	//! IMonoMethod *method = ourClass->GetMethod("SetupNumber", typeNames);
	//!
	//! @endcode
	//!
	//! @param name           Name of the method to find.
	//! @param paramTypeNames A list of full type names that specify the parameters the method accepts.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, List<const char *> &paramTypeNames) = 0;
	//! Gets the first method that matches given description.
	//!
	//! @param name       Name of the method to find.
	//! @param paramCount Number of arguments the method should take.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, int paramCount) = 0;
	//! Gets an array of methods that matches given description.
	//!
	//! @param name       Name of the methods to find.
	//! @param paramCount Number of arguments the methods should take.
	//! @param foundCount Reference to the variable that will contain number of found methods.
	//!
	//! @returns A pointer to the first found method. You should release resultant array once
	//!          you don't need it anymore.
	VIRTUAL_API virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount) = 0;
	//! Gets an array of overload of the method.
	//!
	//! @param name       Name of the method which overloads to find.
	//! @param foundCount Reference to the variable that will contain number of found methods.
	//!
	//! @returns A pointer to the first found method. You should release resultant array once
	//!          you don't need it anymore.
	VIRTUAL_API virtual IMonoMethod **GetMethods(const char *name, int &foundCount) = 0;
	//! Gets a metadata wrapper for the field of this class.
	//!
	//! @param name Name of the field to get.
	//!
	//! @returns A pointer to the object that implements IMonoField that represents the field of interest,
	//!          if found, otherwise null.
	VIRTUAL_API virtual IMonoField *GetField(const char *name) = 0;
	//! Gets the value of the object's field.
	//!
	//! @param obj   Object which field to get. Use nullptr when working with a static field.
	//! @param name  Name of the field which value to get.
	//! @param value Pointer to the object that will contain the value of the field.
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
	//! Gets the value of the object's field.
	//!
	//! @param obj   Object which field to get. Use nullptr when working with a static field.
	//! @param field Wrapper that identifies the field which value to get.
	//! @param value Pointer to the object that will contain the value of the field.
	//!
	//! @seealso IMonoHandle::SetField
	VIRTUAL_API virtual void GetField(mono::object obj, IMonoField *field, void *value) = 0;
	//! Sets the value of the object's field.
	//!
	//! @param obj   Object which field to set. Use nullptr when working with a static field.
	//! @param field Wrapper that identifies the field which value to set.
	//! @param value New value to assign to the field.
	//!
	//! @seealso IMonoHandle::SetField
	VIRTUAL_API virtual void SetField(mono::object obj, IMonoField *field, void *value) = 0;
	//! Gets one of the properties defined in this class.
	//!
	//! @param name Name of the property to get.
	VIRTUAL_API virtual IMonoProperty *GetProperty(const char *name) = 0;
	//! Gets one of the events defined in this class.
	//!
	//! @param name Name of the event to get.
	VIRTUAL_API virtual IMonoEvent *GetEvent(const char *name) = 0;
	//! Gets the class or struct that is defined in this one.
	//!
	//! @param name Name of the class to get.
	VIRTUAL_API virtual IMonoClass *GetNestedType(const char *name) = 0;
	//! Determines whether this class inherits from specified class.
	//!
	//! Entire inheritance path will be searched for the specified class.
	//!
	//! @param nameSpace Full name of the name space where the class is located.
	//! @param className Name of the class.
	//!
	//! @returns True, if this class is a subclass of specified one.
	VIRTUAL_API virtual bool Inherits(const char *nameSpace, const char *className) = 0;
	//! Determines whether this class inherits from specified class.
	//!
	//! Entire inheritance path will be searched for the specified class.
	//!
	//! @param klass Pointer to the wrapper that represents the class the fact of inheritance from which
	//!              must be determined.
	//!
	//! @returns True, if this class is a subclass of specified one.
	VIRTUAL_API virtual bool Inherits(IMonoClass *klass) = 0;
	//! Determines whether this class inherits from specified class.
	//!
	//! @param nameSpace Full name of the name space where the class is located.
	//! @param className Name of the class.
	//! @param direct    Indicates whether only direct base class should be checked. If false, the entire
	//!                  inheritance path will be searched for specified class.
	//!
	//! @returns True, if this class is a subclass of specified one.
	VIRTUAL_API virtual bool Inherits(const char *nameSpace, const char *className, bool direct) = 0;
	//! Determines whether this class inherits from specified class.
	//!
	//! @param klass  Pointer to the wrapper that represents the class the fact of inheritance from which
	//!               must be determined.
	//! @param direct Indicates whether only direct base class should be checked. If false, the entire
	//!               inheritance path will be searched for specified class.
	//!
	//! @returns True, if this class is a subclass of specified one.
	VIRTUAL_API virtual bool Inherits(IMonoClass *klass, bool direct) = 0;
	//! Determines whether this class implements a certain interface.
	//!
	//! @param nameSpace         Full name of the name space where the interface is located.
	//! @param interfaceName     Name of the interface.
	//! @param searchBaseClasses Indicates whether we should look if base classes implement
	//!                          this interface.
	//!
	//! @returns True, if this class implements specified interface.
	VIRTUAL_API virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses = true) = 0;
	//! Determines whether this class implements a certain interface.
	//!
	//! @param interfacePtr      Pointer to the wrapper that represents the interface the fact of
	//!                          implementation of which must be determined.
	//! @param searchBaseClasses Indicates whether we should look if base classes implement
	//!                          this interface.
	//!
	//! @returns True, if this class implements specified interface.
	VIRTUAL_API virtual bool Implements(IMonoClass *interfacePtr, bool searchBaseClasses = true) = 0;
	//! Boxes given value.
	//!
	//! @returns Null if this class is not a value-type, or reference to the boxed object, if it is.
	VIRTUAL_API virtual mono::object Box(void *value) = 0;
	//! Gets an instance of type System.Type that represents this class.
	VIRTUAL_API virtual mono::type GetType() = 0;
	//! Gets an instance of type System.Type that represents an array of instances of this class.
	VIRTUAL_API virtual mono::type MakeArrayType() = 0;
	//! Gets an instance of type System.Type that represents an array of instances of this class.
	//!
	//! @param rank Number of dimensions in the array.
	VIRTUAL_API virtual mono::type MakeArrayType(int rank) = 0;
	//! Gets an instance of type System.Type that represents a reference to objects of this class.
	VIRTUAL_API virtual mono::type MakeByRefType() = 0;
	//! Gets an instance of type System.Type that represents a pointer to objects of this class.
	VIRTUAL_API virtual mono::type MakePointerType() = 0;
// 		//! Creates a generic type instantiation where type arguments are substituted with given types.
// 		//!
// 		//! Cache the resultant type, since inflation is quite costly.
// 		//!
// 		//! @param types A list of types to use when inflating this class.
// 		//!
// 		//! @returns A pointer to the cached wrapper that represents inflated generic type, if this type is
// 		//!          generic and no exceptions were raised during the execution, otherwise this pointer will
// 		//!          be returned.
// 		VIRTUAL_API virtual IMonoClass *Inflate(List<IMonoClass *> &types) = 0;

	VIRTUAL_API virtual const char *GetName() = 0;
	VIRTUAL_API virtual const char *GetNameSpace() = 0;
	VIRTUAL_API virtual const char *GetFullName() = 0;
	VIRTUAL_API virtual const char *GetFullNameIL() = 0;

	VIRTUAL_API virtual bool GetIsValueType() = 0;
	VIRTUAL_API virtual bool GetIsEnum() = 0;
	VIRTUAL_API virtual bool GetIsDelegate() = 0;

	VIRTUAL_API virtual IMonoAssembly *GetAssembly() = 0;

	VIRTUAL_API virtual IMonoClass *GetBase() = 0;

	VIRTUAL_API virtual ReadOnlyList<IMonoField *>    *GetFields() = 0;
	VIRTUAL_API virtual ReadOnlyList<IMonoProperty *> *GetProperties() = 0;
	VIRTUAL_API virtual ReadOnlyList<IMonoEvent *>    *GetEvents() = 0;
	VIRTUAL_API virtual ReadOnlyList<IMonoMethod *>   *GetMethods() = 0;
};