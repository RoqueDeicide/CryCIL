#pragma once

// Use MONOINTERFACE_LIBRARY constant to get OS-specific name of MonoInterface library.
#if defined(LINUX)
#define MONOINTERFACE_LIBRARY "MonoInterface.so"
#elif defined(APPLE)
#define MONOINTERFACE_LIBRARY "MonoInterface.dylib"
#else
#define MONOINTERFACE_LIBRARY "MonoInterface.dll"
#endif

namespace mono
{
	//! A simple typedef that relieves developers of games and modules
	//! from having to include original mono header files.
	typedef class _object *object;			// Just remember that mono::object is a pointer type.
	//! A simple typedef that relieves developers of games and modules
	//! from having to include original mono header files.
	typedef class _string *string;			// Just remember that mono::string is a pointer type.
}

#define BOX_VALUE(type) virtual mono::object Box(type value) = 0
//! Interface for the object that does default boxing operations.
struct IDefaultBoxinator
{
	BOX_VALUE(bool);
	BOX_VALUE(signed char);
	BOX_VALUE(unsigned char);
	BOX_VALUE(short);
	BOX_VALUE(unsigned short);
	BOX_VALUE(int);
	BOX_VALUE(unsigned int);
	BOX_VALUE(float);
	BOX_VALUE(double);
	BOX_VALUE(Vec3);
	BOX_VALUE(Ang3);
	BOX_VALUE(Quat);
};
#undef BOX_VALUE

//! Base interface for objects that wrap Mono functionality.
struct IMonoFunctionalityWrapper
{
	//! Returns pointer to Mono object this wrapper uses.
	virtual void *GetWrappedPointer() const = 0;
};

//! Base type of objects that wrap MonoObject instances with GC handles to allow
//! unmanaged code access the MonoObject, even if it was moved by garbage collector.
//!
//! @remarks The main purpose of this interface is to allow developers who are busy
//!          in other games and modules have access to Mono run-time environment
//!          with help of virtual dispatch.
struct IMonoHandle : public IMonoFunctionalityWrapper
{
	//! Tells the object to hold given MonoObject and prevent its collection.
	//!
	//! @param object MonoObject that is in danger of GC if not held by this object.
	virtual void Hold(mono::object object) = 0;
	//! Tells this object to release MonoObject it held previously.
	virtual void Release() = 0;
	//! Returns an instance of MonoObject this object is wrapped around.
	virtual mono::object Get() = 0;
	//! Calls a Mono method associated with this object.
	//!
	//! @remark Make sure that each overload of the method you are
	//!         calling has unique number of parameters.
	//!
	//! @param name Name of the method to invoke.
	//! @param args Array of arguments to pass, that also defines which method overload to use.
	virtual mono::object CallMethod(const char *name, IMonoArray *args) = 0;
	//! Gets the value of the object's field.
	//!
	//! @param name Name of the field which value to get.
	virtual mono::object GetField(const char *name) = 0;
	//! Sets the value of the object's field.
	//!
	//! @param name  Name of the field which value to set.
	//! @param value New value to assign to the field.
	virtual void SetField(const char *name, mono::object value) = 0;
	//! Gets the value of the object's property.
	//!
	//! @param name Name of the property which value to get.
	virtual mono::object GetProperty(const char *name) = 0;
	//! Sets the value of the object's property.
	//!
	//! @param name  Name of the property which value to set.
	//! @param value New value to assign to the property.
	virtual void SetProperty(const char *name, mono::object value) = 0;
	//! Unboxes value of this object.
	//!
	//! @tparam T Type of the value to unbox. bool, for instance if
	//!           managed object is of type System.Boolean.
	//!
	//! @returns Verbatim copy of managed memory block held by the object.
	template<class T> T Unbox()
	{
		return *(T *)this->UnboxObject();
	}
	//! Gets managed type that represents wrapped object.
	virtual struct IMonoClass *GetClass() = 0;

protected:
	virtual void *UnboxObject() = 0;
};
//! Defines interface of objects that wrap functionality of MonoAssembly type.
struct IMonoAssembly : public IMonoFunctionalityWrapper
{
	//! Gets the class.
	//!
	//! @param className
	virtual IMonoClass *GetClass(const char *className, const char *nameSpace = "CryCil") = 0;
};
//! Defines interface of objects that wrap functionality of MonoArray type.
struct IMonoArray : public IMonoFunctionalityWrapper
{
	//! Gets the length of the array.
	__declspec(property(get=GetSize)) int Length;
	//! Gets item located at the specified position.
	//!
	//! @param index Zero-based index of the item to get.
	virtual mono::object GetItem(int index);
	//! Sets item located at the specified position.
	//!
	//! @param index Zero-based index of the item to set.
	virtual void SetItem(int index, mono::object value);
protected:
	virtual int GetSize() const = 0;
};
//! Defines interface of objects that wrap functionality of MonoClass type.
struct IMonoClass : public IMonoFunctionalityWrapper
{
	//! Gets the name of this class.
	__declspec(property(get=GetName)) const char *Name;
	//! Gets the name space where this class is defined.
	__declspec(property(get=GetNameSpace)) const char *NameSpace;
	//! Gets assembly where this class is defined.
	__declspec(property(get=GetAssembly)) IMonoAssembly *Assembly;
	//! Gets assembly where this class is defined.
	__declspec(property(get=GetBase)) IMonoClass *Base;
	//! Gets method that can accept arguments of specified types.
	//!
	//! @param name  Name of the method to get.
	//! @param types An array of arguments which types specify method signature to use.
	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr);
	//! Gets the first that matches given description.
	//!
	//! @param name       Name of the method to find.
	//! @param paramCount Number of arguments the method should take.
	virtual IMonoMethod *GetMethod(const char *name, int paramCount);
	//! Gets an array of methods that matches given description.
	//!
	//! @param name       Name of the methods to find.
	//! @param paramCount Number of arguments the methods should take.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount);
	//! Gets an array of overload of the method.
	//!
	//! @param name       Name of the method which overloads to find.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	virtual IMonoMethod **GetMethods(const char *name, int &foundCount);
	//! Gets the value of the object's field.
	//!
	//! @param obj   Object which field to get.
	//! @param name Name of the field which value to get.
	virtual mono::object GetField(mono::object obj, const char *name) = 0;
	//! Sets the value of the object's field.
	//!
	//! @param obj   Object which field to set.
	//! @param name  Name of the field which value to set.
	//! @param value New value to assign to the field.
	virtual void SetField(mono::object obj, const char *name, mono::object value) = 0;
	//! Gets the value of the object's property.
	//!
	//! @param obj   Object which property to get.
	//! @param name Name of the property which value to get.
	virtual mono::object GetProperty(mono::object obj, const char *name) = 0;
	//! Sets the value of the object's property.
	//!
	//! @param obj   Object which property to set.
	//! @param name  Name of the property which value to set.
	//! @param value New value to assign to the property.
	virtual void SetProperty(mono::object obj, const char *name, mono::object value) = 0;
	//! Determines whether this class implements from specified class.
	//!
	//! @param nameSpace Full name of the name space where the class is located.
	//! @param className Name of the class.
	//! @returns True, if this class is a subclass of specified one.
	virtual bool Inherits(const char *nameSpace, const char *className) = 0;
	//! Determines whether this class implements a certain interface.
	//!
	//! @param nameSpace         Full name of the name space where the interface is located.
	//! @param interfaceName     Name of the interface.
	//! @param searchBaseClasses Indicates whether we should look if base classes implement
	//!                          this interface.
	//! @returns True, if this class does implement specified interface.
	virtual bool Implements(const char *nameSpace, const char *interfaceName, bool searchBaseClasses = true) = 0;

protected:
	virtual const char *GetName() = 0;
	virtual const char *GetNameSpace() = 0;
	virtual IMonoAssembly *GetAssembly() = 0;
	virtual IMonoClass *GetBase() = 0;
};
//! Defines interface of objects that wrap functionality of MonoMethod type.
struct IMonoMethod : public IMonoFunctionalityWrapper
{
	//! Gets the pointer to unmanaged thunk of the method.
	//!
	//! Unmanaged thunk is a body of the method. Getting the thunk causes Mono to
	//! compile the method's code if needed, and using it allows coder to bypass
	//! quite a lot coding, therefore, if there is a specific method, that you
	//! need to invoke a lot from unmanaged code, use it's thunk, instead of normal
	//! invocation.
	//!
	//! @example DoxygenExampleFiles/UnmanagedThunkExample.cpp
	__declspec(property(get=GetThunk)) void *UnmanagedThunk;
	//! Gets the name of the method.
	__declspec(property(get=GetName)) const char *Name;
	//! Gets number of arguments this method accepts.
	__declspec(property(get=GetParameterCount)) int ParameterCount;

	//! Invokes this method.
	//!
	//! @remark Since extension methods are static by their internal nature,
	//!         you can pass null as object parameter, and that can work,
	//!         if extension method is not using the instance. It's up to
	//!         you to find uses for that minor detail.
	//!
	//! @param object    Pointer to the instance to use, if this method is not
	//!                  static, it can be null otherwise.
	//! @param params    Pointer to the mono array of parameters to pass to the method.
	//!                  Pass null, if method can accept no arguments.
	//! @param polymorph Indicates whether we need to invoke a virtual method,
	//!                  that is specific to the instance.
	virtual mono::object Invoke(mono::object object, IMonoArray *params = nullptr, bool polymorph = false) = 0;
	//! Invokes this method.
	//!
	//! @remark Since extension methods are static by their internal nature,
	//!         you can pass null as object parameter, and that can work,
	//!         if extension method is not using the instance. It's up to
	//!         you to find uses for that minor detail.
	//!
	//! @param object     Pointer to the instance to use, if this method is not
	//!                   static, it can be null otherwise.
	//! @param params     Pointer to the array of parameters to pass to the method.
	//!                   Pass null, if method can accept no arguments.
	//! @param paramCount Number of parameters that need to be passed to the method.
	//! @param polymorph  Indicates whether we need to invoke a virtual method,
	//!                   that is specific to the instance.
	virtual mono::object Invoke(mono::object object, void **params = nullptr, int paramCount = 0, bool polymorph = false) = 0;
protected:
	virtual void *GetThunk() = 0;
	virtual const char *GetName() = 0;
	virtual int GetParameterCount() = 0;
};

//! Base class for MonoRunTime. Provides access to Mono interface.
//!
//! @remarks In order to relieve us from having to export too many functions
//!          we have to use virtual dispatch. In order to do it, we need the
//!          objects that use virtual methods for any operations that happen
//!          in a different DLL. IMonoRunTime is one of base classes for
//!          such objects.
struct IMonoInterface
{
	//! Triggers registration of FlowGraph nodes.
	//!
	//! @remark Call this method from Game::RegisterGameFlowNodes function.
	virtual void RegisterFlowGraphNodes() = 0;
	//! Shuts down Mono run-time environment.
	//!
	//! @remark Call this method from GameStartup destructor.
	virtual void Shutdown() = 0;

	//! Converts given null-terminated string to Mono managed object.
	virtual mono::string ToManagedString(const char *text) = 0;
	//! Converts given managed string to null-terminated one.
	virtual const char *ToNativeString(mono::string text) = 0;
	//! Creates a new wrapped MonoObject using constructor with specific parameters.
	//!
	//! @remark Object that is created by this method can made to be safely keepable by anything.
	//!         Pinning the object in place allows its pointer to be safe to use at any
	//!         time, however it makes GC sessions longer and decreases memory usage efficiency.
	//!
	//! @param assembly   Assembly where the type of the object is defined.
	//! @param name_space Name space that contains the type of the object.
	//! @param class_name Name of the type to use.
	//! @param persistent Indicates whether handle should keep the object away from GC.
	//! @param pinned     Indicates whether the object's location
	//!                   in the managed heap must be kept constant.
	//! @param params     An array of parameters to pass to the constructor.
	//!                   If null, default constructor will be used.
	virtual IMonoHandle *CreateObject
	(
		IMonoAssembly *assembly,
		const char *name_space,
		const char *class_name,
		bool persistent = false,
		bool pinned = false,
		IMonoArray *params = nullptr
	) = 0;
	//! Creates a new Mono handle wrapper for given MonoObject.
	//!
	//! @remark Making the object persistent allows the object to be accessible from
	//!         native code for prolonged periods of time.
	//!         Pinning the object in place allows its pointer to be safe to use at any
	//!         time, however it makes GC sessions longer and decreases memory usage efficiency.
	//!         Use this method to choose, what to do with results of invoking Mono methods.
	//!
	//! @param obj    An object to make persistent.
	//! @param pinned Indicates whether the object's location
	//!               in the managed heap must be kept constant.
	virtual IMonoHandle *WrapObject(mono::object obj, bool pinned) = 0;
	//! Creates object of type object[] with specified capacity.
	//!
	//! @param capacity   Number of elements that can be held by the array.
	//! @param persistent Indicates whether the array must be safe to
	//!                   keep a reference to for prolonged periods of time.
	virtual IMonoArray *CreateArray(int capacity, bool persistent) = 0;
	//! Handles exception that occurred during managed method invocation.
	virtual void HandleException(mono::object exception) = 0;
	//! Registers a new internal call.
	//!
	//! @remarks Internal calls allow .Net/Mono code to invoke unmanaged code.
	//!
	//! @param functionPointer Pointer to unmanaged thunk that needs to be exposed to Mono code.
	virtual void AddInternalCall(void *functionPointer) = 0;
	// Properties.

	//! Gets the pointer to AppDomain.
	__declspec(property(get=GetAppDomain)) void *AppDomain;
	//! Gets the pointer to IMonoAssembly that represents Cryambly.
	__declspec(property(get=GetCryambly)) IMonoAssembly *Cryambly;
	//! Gets the pointer to IMonoAssembly that represents Pdb2mdb.
	__declspec(property(get=GetPdbMdbAssembly)) IMonoAssembly *Pdb2Mdb;
	//! Gets the pointer to IMonoAssembly that represents equivalent of mscorlib.
	__declspec(property(get=GetCoreLibrary)) IMonoAssembly *CoreLibrary;
	//! Indicates whether Mono run-time environment is running.
	__declspec(property(get=GetInitializedIndication)) bool IsRunning;
	//! Returns the object that boxes some simple value types.
	__declspec(property(get=GetDefaultBoxer)) IDefaultBoxinator *DefaultBoxer;

protected:
	virtual void *GetAppDomain() = 0;
	virtual IMonoAssembly *GetCryambly() = 0;
	virtual IMonoAssembly *GetPdbMdbAssembly() = 0;
	virtual IMonoAssembly *GetCoreLibrary() = 0;
	virtual bool GetInitializedIndication() = 0;
	virtual IDefaultBoxinator *GetDefaultBoxer() = 0;
};

//! A simple global variable, provides access to MonoRunTime from anywhere.
//! @remark Within the game project, or any other module,
//!         this variable will have to be assigned manually.
IMonoInterface *MonoEnv = nullptr;
#ifdef CRYCIL_MODULE
// CRYCIL_MODULE is only supposed to be defined within MonoInterface project.

//! Store IGameFramework pointer here for use inside MonoInterface.dll;
IGameFramework *Framework = nullptr;
#endif

//! Creates managed string that contains given text.
//!
//! @param NTString Null-terminated string which text to copy to managed string.
mono::string ToMonoString(const char *NTString)
{
	return MonoEnv->ToManagedString(NTString);
}
//! Creates native null-terminated string from managed one.
const char *ToNativeString(mono::string monoString)
{
	return MonoEnv->ToNativeString(monoString);
}

#define BOX_FUNCTION(type) mono::object Box(type value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(bool value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(signed char value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(unsigned char value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(short value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(unsigned short value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(int value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(unsigned int value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(float value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(double value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(Vec3 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(Ang3 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
mono::object Box(Quat value) { return MonoEnv->DefaultBoxer->Box(value); }

#undef BOX_FUNCTION