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
	//! This typedef is here to represent a reference to an object located within managed heap.
	//!
	//! Details:
	//!
	//! Pointers of this type are returned from a bunch of API calls, they are
	//! also used to pass arguments to managed methods.
	//!
	//! Always bear in mind that these are references to objects that are watched
	//! over by a .Net/Mono garbage collector (GC). This means that if GC has no
	//! information about references to a specific object, it will be removed during
	//! the next session of garbage collection, and even if there are live references
	//! to the object from managed code, it can be moved during heap compression.
	//!
	//! GC never tracks unmanaged references to objects (you can recognize these
	//! references by them being of type mono::object). This makes usage of mono::object
	//! references very dangerous, because the reference can become invalid without
	//! your consent at any point in time.
	//!
	//! The only time when mono::object is completely safe to use, is when it represents
	//! a reference to a variable allocated on the stack and passed to unmanaged code
	//! with help of either ref or out keyword. Make sure, however, that it is not used
	//! within unmanaged code after the method returns, as it will be removed from stack
	//! once it leaves its scope within managed method where it was declared.
	//!
	//! Also, you have no direct access to Mono API that works with objects,
	//! therefore there is only a handful of ways they can be used.
	//!
	//! Using mono::object instances:
	//!
	//! There is only a handful of API functions that accept mono::object instances, so the
	//! main use of these [instances] is:
	//!     1) Storage of references to the result of method invocation.
	//!     2) Storage of references to the arguments that need to be passed to the
	//!        method when it's invoked.
	//!     3) Storage of references to unhandled exceptions that were thrown during
	//!        invocation of the unmanaged thunk.
	//!
	//! In order to access Mono API for objects and/or make its usage more safe,
	//! mono::object instances require to be wrapped around by an object of type
	//! IMonoHandle.
	//!
	//! There are three types of objects that implement IMonoHandle:
	//!     1) Free       : This is the most simple wrapper and the least safe one:
	//!                     It only provides access to object's API and nothing more.
	//!     2) Persistent : This is the best type of handle to use when there is a need
	//!                     to keep a reference to the object for prolonged periods of
	//!                     time. GC will not remove the object when there is at least
	//!                     one persistent IMonoHandle active. You must, however, call
	//!                     Release method when you don't need it, otherwise a memory
	//!                     leak will be created.
	//!     3) Pinned     : This type is similar to Persistent IMonoHandle in the sense
	//!                     that the object won't be deleted by GC while there is an active
	//!                     handle. The difference however is that pinned handle will also
	//!                     instruct GC to not even Move the object during heap compression.
	//!                     This makes accessing object wrapped by the handle faster, but it
	//!                     creates performance issues with garbage collection.
	//!                     Only use this type of handle if you have a reference to the object
	//!                     that you keep for a somewhat long period of time and you need to
	//!                     access it frequently. Just like with persistent handles, you need
	//!                     to release them when you don't need them.
	//!
	//! You can use IMonoInterface::CreateObject() function to create object that
	//! already has wrapping.
	//!
	//! You can use IMonoInterface::WrapObject() function to create a wrapper for
	//! mono::object instance when you to work with it or keep it.
	typedef class _object *object;
	//! A simple typedef that relieves developers of games and modules
	//! from having to include original mono header files.
	typedef class _string *string;
}

#define BOX_VALUE(type) virtual mono::object Box(type value) = 0
//! Interface for the object that does default boxing operations.
struct IDefaultBoxinator
{
	//! Boxes a boolean value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(bool value) = 0;
	//! Boxes a signed byte value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(signed char value) = 0;
	//! Boxes an unsigned byte value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(unsigned char value) = 0;
	//! Boxes an Int16 value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(short value) = 0;
	//! Boxes a UInt16 value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(unsigned short value) = 0;
	//! Boxes an Int32 value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(int value) = 0;
	//! Boxes a UInt32 value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(unsigned int value) = 0;
	//! Boxes a float value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(float value) = 0;
	//! Boxes a double value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(double value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(Vec3 value) = 0;
	//! Boxes a EulerAngles value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(Ang3 value) = 0;
	//! Boxes a Quaternion value.
	//!
	//! @param value Value to box.
	virtual mono::object Box(Quat value) = 0;
};
#undef BOX_VALUE

//! Base interface for objects that wrap Mono functionality.
struct IMonoFunctionalityWrapper
{
	//! Returns pointer to Mono object this wrapper uses.
	virtual void *GetWrappedPointer() const = 0;
};

//! Base type of objects that wrap MonoObject instances granting access to Mono API
//! and optionally making usage of managed objects safer.
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
	//! Unboxes value of this object. Don't use with non-value types.
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
	//! @param className Name of the class to get.
	//! @param nameSpace Name space where the class is defined.
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
	virtual mono::object GetItem(int index) = 0;
	//! Sets item located at the specified position.
	//!
	//! @param index Zero-based index of the item to set.
	virtual void SetItem(int index, mono::object value) = 0;
	//! Tells the object that it's no longer needed.
	virtual void Release() = 0;
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
	//! Creates an instance of this class.
	//!
	//! @param args Arguments to pass to the constructor, can be null if latter has no parameters.
	virtual mono::object CreateInstance(IMonoArray *args = nullptr) = 0;
	//! Gets method that can accept arguments of specified types.
	//!
	//! @param name  Name of the method to get.
	//! @param types An array of arguments which types specify method signature to use.
	virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr) = 0;
	//! Gets the first that matches given description.
	//!
	//! @param name       Name of the method to find.
	//! @param paramCount Number of arguments the method should take.
	virtual IMonoMethod *GetMethod(const char *name, int paramCount) = 0;
	//! Gets an array of methods that matches given description.
	//!
	//! @param name       Name of the methods to find.
	//! @param paramCount Number of arguments the methods should take.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	virtual IMonoMethod **GetMethods(const char *name, int paramCount, int &foundCount) = 0;
	//! Gets an array of overload of the method.
	//!
	//! @param name       Name of the method which overloads to find.
	//! @param foundCount Reference to the variable that will contain
	//!                   number of found methods.
	//! @returns A pointer to the first found method. You should release
	//!          resultant array once you don't need it anymore.
	virtual IMonoMethod **GetMethods(const char *name, int &foundCount) = 0;
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
	//! Boxes given value.
	//!
	//! @returns Null if this class is not a value-type, or reference to the boxed object, if it is.
	virtual mono::object Box(void *value) = 0;
protected:
	virtual const char *GetName() = 0;
	virtual const char *GetNameSpace() = 0;
	virtual IMonoAssembly *GetAssembly() = 0;
	virtual IMonoClass *GetBase() = 0;
};
//! Defines interface of objects that wrap functionality of MonoMethod type.
struct IMonoMethod : public IMonoFunctionalityWrapper
{
	//! Returns a pointer to a C-style function that can be invoked like a standard function pointer.
	//!
	//! Signature of the function pointer is defined by the following rules:
	//!    1) The end result is always boxed, therefore it should be mono::object at all times;
	//!    2) Calling convention is a standard one for the platform. (__stdcall for Windows.)
	//!    3) If the method is an instance method then the first parameter should be
	//!       mono::object that represents an instance this method is called for;
	//!    4) The list of methods parameters follows where every argument is represented by
	//!       mono::object that points at the appropriate object, if that object is a value,
	//!       then it must be boxed;
	//!    5) The very last parameter is a pointer to mono::object (mono::object *), it must
	//!       not be null at the point of invocation and it will be null after method returns
	//!       normally, if there was an unhandled exception however, the last parameter will
	//!       point at the exception object. You can handle it yourself, or pass it to
	//!       IMonoInterface::HandleException.
	//!
	//! Simple rules to remember:
	//!    1) Thunk returns mono::object and only takes mono::object parameters.
	//!    2) Box every parameter that is a value-type before invocation, unbox result after.
	//!    3) Result is undefined, if there was an exception that wasn't handled.
	//!
	//! Examples of usage:
	//!
	//! Static method:
	//!
	//! Signatures:
	//! C#:  int CalculateSum(int a, int b);
	//! C++: typedef mono::object (*CalculateSum)(mono::object a, mono::object b, mono::object *ex);
	//!
	//! Getting the thunk:
	//!    IMonoMethod *calculateSumMethod = ... (Get the method pointer).
	//!    CalculateSum sumFunc = (CalculateSum)calculateSumMethod->UnmanagedThunk;
	//!
	//! Invocation:
	//!    int a,b = 0;
	//!    mono::object exception;
	//!    mono::object boxedSum = sumFunc(Box(a), Box(b), &exception);
	//!    if (exception)
	//!    {
	//!        MonoEnv->HandleException(exception);		// Or you can handle it yourself.
	//!    }
	//!    else
	//!    {
	//!        // These code region is the only place where the result of invocation is defined.
	//!        int sum = Unbox<int>(boxedSum);
	//!    }
	//!
	//! Instance method:
	//!
	//! Signatures:
	//! C#:  int GetHashCode();
	//! C++: typedef mono::object (*GetHashCode)(mono::object *ex);
	//!
	//! Getting the thunk:
	//!    IMonoMethod *getHashCodeMethod = ... (Get the method pointer).
	//!    GetHashCode hashFunc = (GetHashCode)getHashCodeMethod->UnmanagedThunk;
	//!
	//! Invocation:
	//!    mono::object instance = ...(Get the instance);
	//!    mono::object exception;
	//!    mono::object boxedHash = hashFunc(&exception);
	//!    if (exception)
	//!    {
	//!        MonoEnv->HandleException(exception);		// Or you can handle it yourself.
	//!    }
	//!    else
	//!    {
	//!        // These code region is the only place where the result of invocation is defined.
	//!        int hash = Unbox<int>(boxedHash);
	//!    }
	__declspec(property(get=GetThunk)) void *UnmanagedThunk;
	//! Gets the name of the method.
	__declspec(property(get=GetName)) const char *Name;
	//! Gets number of arguments this method accepts.
	__declspec(property(get=GetParameterCount)) int ParameterCount;

	//! Invokes this method.
	//!
	//! Since extension methods are static by their internal nature,
	//! you can pass null as object parameter, and that can work,
	//! if extension method is not using the instance. It's up to
	//! you to find uses for that minor detail.
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
	//! Since extension methods are static by their internal nature,
	//! you can pass null as object parameter, and that can work,
	//! if extension method is not using the instance. It's up to
	//! you to find uses for that minor detail.
	//!
	//! @param object     Pointer to the instance to use, if this method is not
	//!                   static, it can be null otherwise.
	//! @param params     Pointer to the array of parameters to pass to the method.
	//!                   Pass null, if method can accept no arguments.
	//! @param polymorph  Indicates whether we need to invoke a virtual method,
	//!                   that is specific to the instance.
	virtual mono::object Invoke(mono::object object, void **params = nullptr, bool polymorph = false) = 0;
protected:
	virtual void *GetThunk() = 0;
	virtual const char *GetName() = 0;
	virtual int GetParameterCount() = 0;
};

//! Base class for MonoRunTime. Provides access to Mono interface.
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
	//! Specifying the object to be persistent will wrap it into a handle that allows
	//! safe access to the object for prolonged periods of time.
	//!
	//! Pinning the object in place allows its pointer to be safe to use at any
	//! time, however it makes GC sessions longer and decreases memory usage efficiency.
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
	//! Making the object persistent allows the object to be accessible from
	//! native code for prolonged periods of time.
	//! Pinning the object in place allows its pointer to be safe to use at any
	//! time, however it makes GC sessions longer and decreases memory usage efficiency.
	//! Use this method to choose, what to do with results of invoking Mono methods.
	//!
	//! @param obj        An object to make persistent.
	//! @param persistent Indicates whether handle should keep the object away from GC.
	//! @param pinned     Indicates whether the object's location
	//!                   in the managed heap must be kept constant.
	virtual IMonoHandle *WrapObject(mono::object obj, bool persistent = false, bool pinned = false) = 0;
	//! Creates object of type object[] with specified capacity.
	//!
	//! @param capacity   Number of elements that can be held by the array.
	//! @param persistent Indicates whether the array must be safe to
	//!                   keep a reference to for prolonged periods of time.
	virtual IMonoArray *CreateArray(int capacity, bool persistent) = 0;
	//! Wraps already existing Mono array.
	//!
	//! @remark Avoid wrapping arrays that are not of type object[].
	//!
	//! @param arrayHandle Pointer to the array that needs to be wrapped.
	//! @param persistent  Indicates whether the array wrapping must be safe to
	//!                    keep a reference to for prolonged periods of time.
	virtual IMonoArray *WrapArray(mono::object arrayHandle, bool persistent) = 0;
	//! Handles exception that occurred during managed method invocation.
	virtual void HandleException(mono::object exception) = 0;
	//! Registers a new internal call.
	//!
	//! Internal calls allow .Net/Mono code to invoke unmanaged code.
	//! Bear in mind that any structure object that is not built-in
	//! .Net/Mono type (built-in types have keywords attached to them)
	//! must be passed with either ref or out keyword. Such object
	//! must then be unboxed before being access from C++ code.
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
	//! {
	//!     return gEnv->p3DEngine->GetTerrainElevation(x, y);
	//! }
	//!
	//! C# invocation:
	//! {
	//!     float elevationAt3And5 = (ClassName).GetTerrainElevation(3, 5);
	//! }
	//!
	//! With custom structures:
	//!
	//! Signatures:
	//! C#:  [MethodImpl(MethodImplOptions.InternalCall)]
	//!      extern internal static float CalculateArea(ref Vector2 leftBottom, ref Vector2 rightTop);
	//! C++: float CalculateArea(mono::object leftBottom, mono::object rightTop);
	//!
	//! C++ implementation:
	//! {
	//!     // Unbox vectors.
	//!     Vec2 leftBottomPosition = Unbox<Vec2>(leftBottom);
	//!     Vec2 rightTopPosition   = Unbox<Vec2>(rightTop);
	//!     // Calculate area.
	//!     return abs((leftBottomPosition.x - rightTop.x) * (leftBottomPosition.y - rightTop.y));
	//! }
	//!
	//! C# invocation:
	//! {
	//!     // Declare variables.
	//!     Vector2 leftBottom = new Vector2(2);
	//!     Vector2 rightTop   = new Vector2(4, 5);
	//!     // Pass those variables by reference.
	//!     float area = (ClassName).CalculateArea(ref leftBottom, ref rightTop);
	//! }
	//!
	//! With custom structures and managed objects:
	//!
	//! Signatures:
	//! C#:  [MethodImpl(MethodImplOptions.InternalCall)]
	//!      extern internal static Material LoadMaterial(string name, ref MaterialParameters params);
	//! C++: mono::object LoadMaterial(mono::string name, mono::object params);
	//!
	//! C++ implementation:
	//! {
	//!     // Unbox parameters.
	//!     MaterialParameters pars = Unbox<MaterialParameters>(params);
	//!     // Get the handle to the loaded material.
	//!     IMaterial *materialHandle = m_pMaterialManager->LoadMaterial
	//!     (
	//!         MonoEnv->ToNativeString(name),
	//!         pars.makeIfNotFound,
	//!         pars.nonRemovable
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
	//!         false,                      // Persistent?
	//!         false,                      // Pinned?
	//!         ctorParams                  // Arguments.
	//!     )->Get();
	//! }
	//!
	//! C# invocation:
	//! {
	//!     // Declare variable.
	//!     MaterialParameters params = new MaterialParameters
	//!     {
	//!         MakeIfNotFound = true,
	//!         NonRemovable = false
	//!     };
	//!     // Pass those variables by reference.
	//!     Material deathMetal = (ClassName).LoadMaterial("DeathMetal", ref params);
	//! }
	//!
	//! @param name            Full name of the method that can be used to access it
	//!                        from managed code.
	//! @param functionPointer Pointer to unmanaged thunk that needs to be exposed to Mono code.
	virtual void AddInternalCall(const char *name, void *functionPointer) = 0;
	//! Loads a Mono assembly into memory.
	//!
	//! @param moduleFileName Name of the file inside Modules folder.
	virtual IMonoAssembly *LoadAssembly(const char *moduleFileName) = 0;
	//! Wraps assembly pointer.
	virtual IMonoAssembly *WrapAssembly(void *assemblyHandle) = 0;
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

//! Signature of the only method that is exported by MonoInterface.dll
typedef IMonoInterface *(*InitializeMonoInterface)(IGameFramework *);

//! A simple global variable, provides access to MonoRunTime from anywhere.
//!
//! Within the game project, or any other module,
//! this variable will have to be assigned manually.
//!
//! A simplest way to do so is to include this header file into GameStartup.h
//! and have MonoEnv assigned a value returned by InitializeModule function.
//!
//! Example:
//!
//! // Load library. Save handle in a field of type HMODULE.
//! this->monoInterfaceDll = CryLoadLibrary("MonoInterface.dll");
//! // Check if it was loaded properly.
//! if (!this->monoInterfaceDll)
//! {
//!     CryFatalError("Could not locate MonoInterface.dll");
//! }
//! // Get InitializeModule function.
//! InitializeMonoInterface cryCilInitializer =
//!     CryGetProcAddress(this->monoInterfaceDll, "InitializeModule");
//! // Invoke it, save the result in MonoEnv.
//! MonoEnv = cryCilInitializer(gameFramework);
//! // Now MonoEnv can be used to communicate with CryCIL API!
IMonoInterface *MonoEnv = nullptr;
#ifdef CRYCIL_MODULE
// CRYCIL_MODULE is only supposed to be defined within MonoInterface project.

//! Store IGameFramework pointer here for use inside MonoInterface.dll;
IGameFramework *Framework = nullptr;
#endif

#pragma region Conversions Interface

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

#pragma endregion