#pragma once

#include <CryThread.h>
#include <Cry_Math.h>
#include <ISystem.h>
#include <I3DEngine.h>
#include <IInput.h>
#include <IConsole.h>
#include <ITimer.h>
#include <ILog.h>
#include <IGameplayRecorder.h>
#include <ISerialize.h>
#include <IGameFramework.h>

#include "List.h"
#include "SortedList.h"
#include "Text.h"

#include "Logs.h"

// Use MONOINTERFACE_LIBRARY constant to get OS-specific name of MonoInterface library.
#if defined(LINUX)
#define MONOINTERFACE_LIBRARY "MonoInterface.so"
#elif defined(APPLE)
#define MONOINTERFACE_LIBRARY "MonoInterface.dylib"
#else
#define MONOINTERFACE_LIBRARY "MonoInterface.dll"
#endif

#define MONO_INTERFACE_INIT "InitializeCryCilSubsystem"

// Forward declarations.
struct IDefaultBoxinator;
struct IMonoFunctionalityWrapper;
struct IMonoHandle;
struct IMonoAssembly;
struct IMonoArray;
struct IMonoClass;
struct IMonoMethod;
struct IMonoSystemListener;
struct IMonoInterface;
struct IMonoGCHandle;
struct IMonoGC;

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
	//!
	//!     2) Persistent : This is the best type of handle to use when there is a need
	//!                     to keep a reference to the object for prolonged periods of
	//!                     time. GC will not remove the object when there is at least
	//!                     one persistent IMonoHandle active. You must, however, call
	//!                     Release method when you don't need it, otherwise a memory
	//!                     leak will be created. Accessing the object held by a persistent
	//!                     wrapper requires some code to be executed, therefore it is
	//!                     only recommended to use this type of wrapper when you only
	//!                     access the object rarely.
	//!
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
	typedef class MonoObject *object;

	//! Typedefs marked by this define represent a reference to an object that is located
	//! within managed memory. Technically all of them are equivalents to mono::object
	//! but they can be used to designate what the object is supposed to be.
	//!
	//! In C++'s analog for mono::object is void *. Both can be casted to whatever type
	//! like bool *. The same can be done to mono::object.
	//!
	//! Examples:
	//!
	//! // The following two methods are technically the same, they are both are unmanaged
	//! thunks of static String.IsNullOrWhitespace method, but the last one uses OBJECT_NAME
	//! typedefs do describe the types of objects better.
	//!
	//! @code{.cpp}
	//! bool __stdcall IsNullOrWhitespace(mono::object text, mono::object    *exception);
	//!
	//! bool __stdcall IsNullOrWhitespace(mono::string text, mono::exception *exception);
	//! @endcode
#define OBJECT_NAME

	//! Represents a reference to a managed string.
	OBJECT_NAME typedef object string;
	//! Represents a reference to a boxed Boolean value.
	OBJECT_NAME typedef object boolean;
	//! Represents a reference to a boxed signed 1-byte long integer.
	OBJECT_NAME typedef object sbyte;
	//! Represents a reference to a boxed unsigned 1-byte long integer.
	OBJECT_NAME typedef object byte;
	//! Represents a reference to a boxed System.Char instance.
	OBJECT_NAME typedef object character;
	//! Represents a reference to a boxed signed 2-byte long integer.
	OBJECT_NAME typedef object int16;
	//! Represents a reference to a boxed unsigned 2-byte long integer.
	OBJECT_NAME typedef object uint16;
	//! Represents a reference to a boxed signed 4-byte long integer.
	OBJECT_NAME typedef object int32;
	//! Represents a reference to a boxed unsigned 4-byte long integer.
	OBJECT_NAME typedef object uint32;
	//! Represents a reference to a boxed signed 8-byte long integer.
	OBJECT_NAME typedef object int64;
	//! Represents a reference to a boxed unsigned 8-byte long integer.
	OBJECT_NAME typedef object uint64;
	//! Represents a reference to a boxed signed pointer represented by System.IntPtr.
	OBJECT_NAME typedef object intptr;
	//! Represents a reference to a boxed unsigned pointer represented by System.IntPtr.
	OBJECT_NAME typedef object uintptr;
	//! Represents a reference to a boxed 4-byte floating-point number.
	OBJECT_NAME typedef object float32;
	//! Represents a reference to a boxed 8-byte floating-point number.
	OBJECT_NAME typedef object float64;
	//! Represents a reference to a boxed precise 16-byte floating-point number.
	OBJECT_NAME typedef object decimal;
	//! Represents a reference to a managed object passed to internal call via ref keyword.
	OBJECT_NAME typedef object ref_param;
	//! Represents a reference to a managed object passed to internal call via out keyword.
	OBJECT_NAME typedef object out_param;
	//! Represents a reference to a managed thread interface.
	OBJECT_NAME typedef object Thread;
	//! Represents a reference to a managed exception object.
	OBJECT_NAME typedef object exception;
	//! Represents a reference to a managed System.Type object.
	OBJECT_NAME typedef object type;
	//! Represents a reference to a managed array object.
	OBJECT_NAME typedef object Array;
	//! Represents a reference to a managed System.Reflection.Assembly object.
	OBJECT_NAME typedef object assembly;
	//! Represents a reference to an object that is returned by the method that returns System.Void.
	OBJECT_NAME typedef object nothing;
	//! Represents a reference to a boxed vector with 2 components.
	OBJECT_NAME typedef object vector2;
	//! Represents a reference to a boxed vector with 3 components.
	OBJECT_NAME typedef object vector3;
	//! Represents a reference to a boxed vector with 4 components.
	OBJECT_NAME typedef object vector4;
	//! Represents a reference to a boxed quaternion.
	OBJECT_NAME typedef object quaternion;
	//! Represents a reference to a boxed quaternion coupled with translation vector.
	OBJECT_NAME typedef object quat_trans;
	//! Represents a reference to a boxed 2-byte floating-point number.
	OBJECT_NAME typedef object half;
	//! Represents a reference to a boxed 3x3 matrix.
	OBJECT_NAME typedef object matrix33;
	//! Represents a reference to a boxed 3x4 matrix.
	OBJECT_NAME typedef object matrix34;
	//! Represents a reference to a boxed 4x4 matrix.
	OBJECT_NAME typedef object matrix44;
	//! Represents a reference to a boxed plane.
	OBJECT_NAME typedef object plane;
	//! Represents a reference to a boxed ray.
	OBJECT_NAME typedef object ray;
	//! Represents a reference to a boxed RGBA color with 8-bit integer components.
	OBJECT_NAME typedef object byte_color;
	//! Represents a reference to a boxed RGBA color with 32-bit floating-point components.
	OBJECT_NAME typedef object float32_color;
	//! Represents a reference to a boxed axis-aligned bounding box.
	OBJECT_NAME typedef object aabb;
	//! Represents a reference to a boxed set of Euler angles.
	OBJECT_NAME typedef object angles3;
}

//! In various programming books about C++ authors usually tell us that virtual
//! dispatch is used for polymorphism.
//!
//! While that is true, there is another, less famous, usage for it: creation of
//! cross-Dll API.
//!
//! That is possible thanks to the fact that objects of types that have virtual
//! methods always carry a pointer to a VTable with them, allowing access to
//! their interface from anywhere within the process.
#define VIRTUAL_API

//! Interface for the object that does default boxing operations.
struct IDefaultBoxinator
{
	//! Boxes an unsigned pointer value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uintptr BoxUPtr(void *value) = 0;
	//! Boxes a pointer value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::intptr BoxPtr(void *value) = 0;
	//! Boxes a boolean value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::boolean Box(bool value) = 0;
	//! Boxes a signed byte value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::character Box(char value) = 0;
	//! Boxes a signed byte value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::sbyte Box(signed char value) = 0;
	//! Boxes an unsigned byte value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::byte Box(unsigned char value) = 0;
	//! Boxes an Int16 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::int16 Box(short value) = 0;
	//! Boxes a UInt16 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uint16 Box(unsigned short value) = 0;
	//! Boxes an Int32 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::int32 Box(int value) = 0;
	//! Boxes a UInt32 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uint32 Box(unsigned int value) = 0;
	//! Boxes an Int64 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::int64 Box(__int64 value) = 0;
	//! Boxes a UInt64 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::uint64 Box(unsigned __int64 value) = 0;
	//! Boxes a float value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::float32 Box(float value) = 0;
	//! Boxes a double value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::float64 Box(double value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::vector2 Box(Vec2 value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::vector3 Box(Vec3 value) = 0;
	//! Boxes a vector value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::vector4 Box(Vec4 value) = 0;
	//! Boxes a EulerAngles value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::angles3 Box(Ang3 value) = 0;
	//! Boxes a Quaternion value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::quaternion Box(Quat value) = 0;
	//! Boxes an QuaternionTranslation value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::quat_trans Box(QuatT value) = 0;
	//! Boxes a Matrix33 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::matrix33 Box(Matrix33 value) = 0;
	//! Boxes an Matrix34 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::matrix34 Box(Matrix34 value) = 0;
	//! Boxes a Matrix44 value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::matrix44 Box(Matrix44 value) = 0;
	//! Boxes a Plane value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::plane Box(Plane value) = 0;
	//! Boxes a Ray value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::ray Box(Ray value) = 0;
	//! Boxes a ColorB value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::byte_color Box(ColorB value) = 0;
	//! Boxes a ColorF value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::float32_color Box(ColorF value) = 0;
	//! Boxes a AABB value.
	//!
	//! @param value Value to box.
	VIRTUAL_API virtual mono::aabb Box(AABB value) = 0;
};

//! Base interface for objects that wrap Mono functionality.
struct IMonoFunctionalityWrapper
{
	//! Returns pointer to Mono object this wrapper uses.
	VIRTUAL_API virtual void *GetWrappedPointer() = 0;
};

//! Base type of objects that wrap MonoObject instances granting access to Mono API.
struct IMonoHandle : public IMonoFunctionalityWrapper
{
	//! Returns an instance of MonoObject this object is wrapped around.
	VIRTUAL_API virtual mono::object Get() = 0;
	//! Calls a Mono method associated with this object.
	//!
	//! @param name Name of the method to invoke.
	//! @param args Array of arguments to pass, that also defines which method overload to use.
	VIRTUAL_API virtual mono::object CallMethod(const char *name, IMonoArray *args) = 0;
	//! Gets the value of the field. Only use it with objects of managed types.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! // Let's get the value of integer field. obj is our IMonoHandle instance.
	//!
	//! // We need to declare a variable that will store the value of the field.
	//! int fieldValue;		// No need to assign it.
	//!
	//! // Now we can get the value, just provide the address of the variable.
	//! obj->GetField("IntegerField", &fieldValue);
	//!
	//! // Now fieldValue variable contains the value of that field.
	//! //--------------------------------------------------------------------------
	//! // Let's do the same with a field of managed type.
	//!
	//! // Same deal, but we always use one of the mono::object aliases.
	//! mono::string text;
	//! obj->GetField("TextField", &text);
	//! @endcode
	//!
	//! @param name  Name of the field.
	//! @param value Pointer to the memory that will store the value of the field after this
	//!              function completes execution.
	VIRTUAL_API virtual void GetField(const char *name, void *value) = 0;
	//! Sets the value of the field. Only use it with objects of managed types.
	//!
	//! Example:
	//!
	//! @code{.cpp}
	//! // Let's set the value of integer field. obj is our IMonoHandle instance.
	//!
	//! // We need to declare a variable that stores the new value for the field.
	//! int fieldValue = 10;
	//!
	//! // Now we can set the value, just provide the address of the variable.
	//! obj->SetField("IntegerField", &fieldValue);
	//!
	//! // Now IntegerField has a value of 10.
	//! //--------------------------------------------------------------------------
	//! // Let's do the same with a field of managed type.
	//!
	//! // Same deal, but we always use one of the mono::object aliases.
	//! mono::string text = ToMonoString("Some new text");
	//! obj->SetField("TextField", &text);
	//! @endcode
	//!
	//! @param name  Name of the field.
	//! @param value Pointer to the memory that stores the value that should be assigned to the field.
	VIRTUAL_API virtual void SetField(const char *name, void *value) = 0;
	//! Gets the value of the object's property.
	//!
	//! @param name Name of the property which value to get.
	VIRTUAL_API virtual mono::object GetProperty(const char *name) = 0;
	//! Sets the value of the object's property.
	//!
	//! @param name  Name of the property which value to set.
	//! @param value New value to assign to the property.
	VIRTUAL_API virtual void SetProperty(const char *name, void *value) = 0;
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
	VIRTUAL_API virtual IMonoClass *GetClass() = 0;

protected:
	VIRTUAL_API virtual void *UnboxObject() = 0;
};

//! Represents an interface with Mono Garbage Collector (GC).
struct IMonoGC
{
	//! Gets index of the oldest generation that is used by GC.
	__declspec(property(get = GetMaxGeneration)) int MaxGeneration;
	//! Gets number of bytes that are currently allocated by this GC.
	__declspec(property(get = GetHeapSize)) __int64 HeapSize;

	//! Triggers garbage collection.
	//!
	//! @param generation Index of generation to clean up. Bigger number means older generation
	//!                   to collect garbage in. Clean up of older generations is always preceded
	//!                   by GCion of younger ones. Invocation with -1 or @see IMonoGC::MaxGeneration
	//!                   will trigger full scale garbage collection.
	VIRTUAL_API virtual void Collect(int generation = -1) = 0;
	//! Holds given managed object allowing to get the reference to it wherever it can be after heap
	//! compression. The object will still be eligible for collection if it becomes unreachable from
	//! the rest of the managed code.
	//!
	//! You have to use @see IMonoGCHandle::Object property to get the pointer to the object.
	//!
	//! @param obj Pointer to managed object to hold.
	VIRTUAL_API virtual IMonoGCHandle *Hold(mono::object obj) = 0;
	//! Keeps given managed object from being collected by GC.
	//!
	//! You will have to use @see IMonoGCHandle::Object property to get the valid pointer
	//! to the given object before using it.
	//!
	//! @param obj Pointer to managed object to keep.
	VIRTUAL_API virtual IMonoGCHandle *Keep(mono::object obj) = 0;
	//! Pins given managed object by prohibiting GC from collecting or moving it.
	//!
	//! As long as returned object is not released, it will be perfectly safe to access
	//! given object through its current pointer, as it will never be deleted or moved.
	//!
	//! @param obj Pointer to managed object to pin.
	VIRTUAL_API virtual IMonoGCHandle *Pin(mono::object obj) = 0;

	VIRTUAL_API virtual int GetMaxGeneration() = 0;
	VIRTUAL_API virtual __int64 GetHeapSize() = 0;
};

//! Represents a GC handle.
struct IMonoGCHandle
{
	//! Gets the wrapper for managed object that is being held by this GC handle.
	//!
	//! @returns A pointer that provides access to object's API, or null if this is a weak
	//!          handle and held object has been collected.
	__declspec(property(get = GetObjectHandle)) IMonoHandle *ObjectHandle;
	//! Gets the pointer to managed object that is being held by this GC handle.
	//!
	//! @returns A pointer that can be passed directly to methods and thunks, or null if
	//!          this is a weak handle and held object has been collected.
	__declspec(property(get = GetObjectPointer)) mono::object ObjectPointer;

	//! Releases this GC handle.
	VIRTUAL_API virtual void Release() = 0;

	VIRTUAL_API virtual IMonoHandle *GetObjectHandle() = 0;
	VIRTUAL_API virtual mono::object GetObjectPointer() = 0;
};

//! Defines interface of objects that wrap functionality of MonoAssembly type.
struct IMonoAssembly : public IMonoFunctionalityWrapper
{
	//! Gets the class.
	//!
	//! @param nameSpace Name space where the class is defined.
	//! @param className Name of the class to get.
	__declspec(property(get = GetClass)) IMonoClass *Classes[][];
	//! Gets the short name of the assembly.
	__declspec(property(get = GetName)) Text *Name;
	//! Gets the full name of the assembly.
	__declspec(property(get = GetFullName)) Text *FullName;
	//! Gets the full name of the assembly.
	__declspec(property(get = GetFileName)) Text *FileName;

	VIRTUAL_API virtual IMonoClass *GetClass(const char *nameSpace, const char *className) = 0;
	VIRTUAL_API virtual Text *GetName() = 0;
	VIRTUAL_API virtual Text *GetFullName() = 0;
	VIRTUAL_API virtual Text *GetFileName() = 0;
	//! Gets the reference to the instance of type System.Reflection.Assembly.
	__declspec(property(get = GetReflectionObject)) mono::assembly ReflectionObject;

	VIRTUAL_API virtual mono::assembly GetReflectionObject() = 0;
};

//! Represents an object that simplifies the process of getting an assembly that is not accessible
//! through MonoEnv variable.
struct IMonoAssemblyCollection
{
	//! Loads a Mono assembly into memory.
	//!
	//! If the assembly was already loaded from the same file, original wrapper will be created.
	//!
	//! @param path Path to the assembly file. Relative paths go from CryEngine installation
	//!             directory.
	//!
	//! @returns A wrapper for the loaded assembly.
	VIRTUAL_API virtual IMonoAssembly *Load(const char *path) = 0;
	//! Wraps an assembly pointer.
	//!
	//! Mostly for internal use since acquiring MonoAssembly pointer requires direct work with Mono.
	//!
	//! @param assemblyHandle Pointer to MonoAssembly to wrap.
	VIRTUAL_API virtual IMonoAssembly *Wrap(void *assemblyHandle) = 0;
	//! Gets the pointer to the assembly wrapper object.
	//!
	//! If there are multiple assemblies loaded with the same file name, it's not guaranteed
	//! that you will get the one you want.
	//!
	//! @param name Name of the assembly (equivalent of the file name but without the extension).
	VIRTUAL_API virtual IMonoAssembly *GetAssembly(const char *name) = 0;
	//! Gets the pointer to the assembly wrapper object.
	//!
	//! Requires fully-qualified name of the assembly. Getting such name is harder, but it's more
	//! reliable when there are multiple assemblies loaded with the same file name.
	//!
	//! @param name Full name of the assembly.
	VIRTUAL_API virtual IMonoAssembly *GetAssemblyFullName(const char *name) = 0;
};

//! Defines interface of objects that wrap functionality of MonoArray type.
struct IMonoArray : public IMonoFunctionalityWrapper
{
	//! Gets the length of the array.
	__declspec(property(get = GetSize)) int Length;
	//! Gets the type of the elements of the array.
	__declspec(property(get = GetElementClass)) IMonoClass *ElementClass;
	//! Provides access to the item.
	//!
	//! Don't hesitate on dereferencing returned pointer: Mono arrays have tendency of
	//! being moved around the memory.
	//!
	//! @param index Zero-based index of the item to access.
	//!
	//! @returns Pointer to the item. The pointer is either mono::object, if this is an
	//!          array of reference types, or a pointer to a struct that can be easily
	//!          dereferenced, if this is an array of value types.
	VIRTUAL_API virtual void *Item(int index) = 0;
	template<typename T> T& At(int index)
	{
		return *(T *)this->Item(index);
	}

	VIRTUAL_API virtual int GetSize() = 0;
	VIRTUAL_API virtual IMonoClass *GetElementClass() = 0;
};
//! Defines interface of objects that wrap functionality of MonoClass type.
struct IMonoClass : public IMonoFunctionalityWrapper
{
	//! Gets the name of this class.
	__declspec(property(get = GetName)) const char *Name;
	//! Gets the name space where this class is defined.
	__declspec(property(get = GetNameSpace)) const char *NameSpace;
	//! Gets assembly where this class is defined.
	__declspec(property(get = GetAssembly)) IMonoAssembly *Assembly;
	//! Gets assembly where this class is defined.
	__declspec(property(get = GetBase)) IMonoClass *Base;
	//! Creates an instance of this class.
	//!
	//! @param args Arguments to pass to the constructor, can be null if latter has no parameters.
	VIRTUAL_API virtual mono::object CreateInstance(IMonoArray *args = nullptr) = 0;
	//! Gets method that can accept arguments of specified types.
	//!
	//! @param name  Name of the method to get.
	//! @param types An array of arguments which types specify method signature to use.
	VIRTUAL_API virtual IMonoMethod *GetMethod(const char *name, IMonoArray *types = nullptr) = 0;
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
	//! Gets the value of the object's property.
	//!
	//! @param obj   Object which property to get.
	//! @param name Name of the property which value to get.
	VIRTUAL_API virtual mono::object GetProperty(void *obj, const char *name) = 0;
	//! Sets the value of the object's property.
	//!
	//! @param obj   Object which property to set.
	//! @param name  Name of the property which value to set.
	//! @param value New value to assign to the property.
	VIRTUAL_API virtual void SetProperty(void *obj, const char *name, void *value) = 0;
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
	VIRTUAL_API virtual IMonoAssembly *GetAssembly() = 0;
	VIRTUAL_API virtual IMonoClass *GetBase() = 0;
};
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
	//! @example DoxygenExampleFiles\UnmanagedThunkExample.cpp
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

//! CryCIL uses initialization stages to allow various systems perform various initialization
//! tasks in a very specific order.
//!
//! This define marks indices used by CryCIL.
#define DEFAULT_INITIALIZATION_STAGE

//! Index of the initialization stage during which entities defined in CryCIL are registered.
#define ENTITY_REGISTRATION_STAGE DEFAULT_INITIALIZATION_STAGE 1000000
//! Index of the initialization stage during which actors defined in CryCIL are registered.
#define ACTORS_REGISTRATION_STAGE DEFAULT_INITIALIZATION_STAGE 2000000
//! Index of the initialization stage during which game modes defined in CryCIL are registered.
#define GAME_MODE_REGISTRATION_STAGE DEFAULT_INITIALIZATION_STAGE 3000000
//! Index of the initialization stage during which data required to register CryCIL flow nodes
//! is gathered.
#define FLOWNODE_RECOGNITION_STAGE DEFAULT_INITIALIZATION_STAGE 4000000

//! Base interface for objects that subscribe to the events raised by IMonoInterface.
//!
//! Listeners receive events in the order of registration. Internal listeners are always
//! registered first.
//!
//! @example DoxygenExampleFiles\ListenerExample.cpp
struct IMonoSystemListener
{
protected:
	IMonoInterface *monoInterface;
public:
	//! Allows IMonoInterface implementation let the listener access it before global
	//! variable MonoEnv is initialized.
	//!
	//! @param handle Pointer to IMonoInterface implementation that can be used.
	virtual void SetInterface(IMonoInterface *handle)
	{
		this->monoInterface = handle;
	}
	//! Invoked before Mono interface is initialized.
	//!
	//! IMonoInterface object is not usable at this stage.
	virtual void OnPreInitialization() = 0;
	//! Invoked before Mono run-time initialization begins.
	//!
	//! CryCIL Mono run-time API is not usable at this stage.
	virtual void OnRunTimeInitializing() = 0;
	//! Invoked after Mono run-time is initialized.
	//!
	//! Cryambly is loaded at this point and Mono is running: CryCIL API can be used now.
	virtual void OnRunTimeInitialized() = 0;
	//! Invoked before MonoInterface object defined in Cryambly is initialized.
	virtual void OnCryamblyInitilizing() = 0;
	//! Invoked before Cryambly attempts to compile game code.
	virtual void OnCompilationStarting() = 0;
	//! Invoked after Cryambly finishes compilation of game code.
	//!
	//! @param success Indicates whether compilation was successful.
	virtual void OnCompilationComplete(bool success) = 0;
	//! Invoked when this listener is registered to get indices of initialization stages
	//! this listener would like to subscribe to.
	//!
	//! Resultant list will be deleted after use, so return a deep copy, if you want it
	//! to be used somewhere else.
	//!
	//! @returns A pointer to a list of integer numbers that represent indices of
	//!          initialization stages this listener wants to subscribe to.
	//!          Null can be return if the listener doesn't want to subscribe to anything.
	virtual List<int> *GetSubscribedStages() = 0;
	//! Invoked when one of initialization stages this listener has subscribed to begins.
	//!
	//! @param stageIndex Zero-based index of the stage.
	virtual void OnInitializationStage(int stageIndex) = 0;
	//! Invoked after MonoInterface object defined in Cryambly is initialized.
	virtual void OnCryamblyInitilized() = 0;
	//! Invoked after all initialization of CryCIL is complete.
	virtual void OnPostInitialization() = 0;
	//! Invoked when logical frame of CryCIL subsystem starts.
	virtual void Update() = 0;
	//! Invoked when logical frame of CryCIL subsystem ends.
	virtual void PostUpdate() = 0;
	//! Invoked when CryCIL shuts down.
	virtual void Shutdown() = 0;
};
//! Base class for MonoRunTime. Provides access to Mono interface.
struct IMonoInterface
{
	//! Triggers registration of FlowGraph nodes.
	//!
	//! Call this method from Game::RegisterGameFlowNodes function.
	VIRTUAL_API virtual void RegisterFlowGraphNodes() = 0;
	//! Shuts down Mono run-time environment.
	VIRTUAL_API virtual void Shutdown() = 0;

	//! Converts given null-terminated string to Mono managed object.
	VIRTUAL_API virtual mono::string ToManagedString(const char *text) = 0;
	//! Converts given managed string to null-terminated one.
	VIRTUAL_API virtual const char *ToNativeString(mono::string text) = 0;

	//! Creates a new MonoObject using constructor with specific parameters.
	//!
	//! @param assembly   Assembly where the type of the object is defined.
	//! @param name_space Name space that contains the type of the object.
	//! @param class_name Name of the type to use.
	//! @param params     An array of parameters to pass to the constructor.
	//!                   If null, default constructor will be used.
	VIRTUAL_API virtual mono::object CreateObject
		(IMonoAssembly *assembly,
		const char *name_space, const char *class_name,
		IMonoArray *params = nullptr) = 0;
	//! Creates a new wrapper for given MonoObject.
	//!
	//! @param obj An object to wrap.
	VIRTUAL_API virtual IMonoHandle *WrapObject(mono::object obj) = 0;
	//! Creates object of type object[] with specified capacity.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param capacity Number of elements that can be held by the array.
	VIRTUAL_API virtual IMonoArray *CreateArray(int capacity) = 0;
	//! Creates object of specified type with specified capacity.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param klass    Pointer to the class that will represent objects
	//!                 within the array.
	//! @param capacity Number of elements that can be held by the array.
	VIRTUAL_API virtual IMonoArray *CreateArray(IMonoClass *klass, int capacity) = 0;
	//! Wraps already existing Mono array.
	//!
	//! Mono array objects are standard managed objects and are prone to GC.
	//!
	//! @param arrayHandle Pointer to the array that needs to be wrapped.
	VIRTUAL_API virtual IMonoArray *WrapArray(mono::Array arrayHandle) = 0;
	//! Unboxes managed value-type object.
	//!
	//! @param value Value-type object to unbox.
	VIRTUAL_API virtual void *Unbox(mono::object value) = 0;
	//! Handles exception that occurred during managed method invocation.
	//!
	//! @param exception Exception object to handle.
	VIRTUAL_API virtual void HandleException(mono::exception exception) = 0;
	//! Registers new object that receives notifications about CryCIL events.
	//!
	//! @param listener Pointer to the object that implements IMonoSystemListener.
	VIRTUAL_API virtual void AddListener(IMonoSystemListener *listener) = 0;
	//! Unregisters an object that receives notifications about CryCIL events.
	//!
	//! Search is done using the pointer value.
	//!
	//! @param listener Pointer to the object that implements IMonoSystemListener.
	VIRTUAL_API virtual void RemoveListener(IMonoSystemListener *listener) = 0;
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
	//!     3) Any pointers to managed objects should be either unboxed or wrapped into
	//!        IMonoHandle *(when using a general objects) or IMonoArray * when using
	//!        arrays.
	//!
	//!     4) Passing an argument using ref or out keyword causes a pointer to be passed.
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
	VIRTUAL_API virtual void AddInternalCall(const char *nameSpace, const char *className, const char *name, void *functionPointer) = 0;
	// Properties.

	//! Gets the pointer to AppDomain.
	__declspec(property(get = GetAppDomain)) void *AppDomain;
	//! Gets the pointer to the assembly wrapper registry.
	__declspec(property(get = GetAssemblyCollection)) IMonoAssemblyCollection *Assemblies;
	//! Gets the pointer to IMonoAssembly that represents Cryambly.
	__declspec(property(get = GetCryambly)) IMonoAssembly *Cryambly;
	//! Gets the pointer to IMonoAssembly that represents Pdb2mdb.
	__declspec(property(get = GetPdbMdbAssembly)) IMonoAssembly *Pdb2Mdb;
	//! Gets the pointer to IMonoAssembly that represents equivalent of mscorlib.
	__declspec(property(get = GetCoreLibrary)) IMonoAssembly *CoreLibrary;
	//! Indicates whether Mono run-time environment is running.
	__declspec(property(get = GetInitializedIndication)) bool IsRunning;
	//! Returns the object that boxes some simple value types.
	__declspec(property(get = GetDefaultBoxer)) IDefaultBoxinator *DefaultBoxer;
	//! Gets the interface with Mono GC.
	__declspec(property(get = GetGC)) IMonoGC *GC;
	//! Gets the pointer to IGameFramework implementation that is available to CryCIL.
	__declspec(property(get = GetGameFramework)) IGameFramework *CryAction;

	VIRTUAL_API virtual void *GetAppDomain() = 0;
	VIRTUAL_API virtual IMonoAssemblyCollection *GetAssemblyCollection() = 0;
	VIRTUAL_API virtual IMonoAssembly *GetCryambly() = 0;
	VIRTUAL_API virtual IMonoAssembly *GetPdbMdbAssembly() = 0;
	VIRTUAL_API virtual IMonoAssembly *GetCoreLibrary() = 0;
	VIRTUAL_API virtual bool GetInitializedIndication() = 0;
	VIRTUAL_API virtual IDefaultBoxinator *GetDefaultBoxer() = 0;
	VIRTUAL_API virtual IMonoGC *GetGC() = 0;
	VIRTUAL_API virtual IGameFramework *GetGameFramework() = 0;
};
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
		this->monoInterface->AddInternalCall
			(this->GetNameSpace(), this->GetName(), methodName, functionPointer);
	}
	//! Returns the name of the class that will declare managed counter-parts
	//! of the internal calls.
	virtual const char *GetName() = 0;
	//! Returns the name space where the class that will declare managed counter-parts
	//! of the internal calls is declared.
	virtual const char *GetNameSpace() = 0;
	//! Unnecessary for most interops.
	virtual void OnPreInitialization()
	{}
	//! Unnecessary for most interops.
	virtual void OnRunTimeInitializing() {}
	//! Unnecessary for most interops.
	virtual void OnCryamblyInitilizing() {}
	//! Unnecessary for most interops.
	virtual void OnCompilationStarting()
	{}
	//! Unnecessary for most interops.
	virtual void OnCompilationComplete(bool success)
	{}
	//! Unnecessary for most interops.
	virtual List<int> *GetSubscribedStages()
	{
		return nullptr;
	}
	//! Unnecessary for most interops.
	virtual void OnInitializationStage(int stageIndex)
	{}
	//! Unnecessary for most interops.
	virtual void OnCryamblyInitilized()
	{}
	//! Unnecessary for most interops.
	virtual void OnPostInitialization()
	{}
	//! Unnecessary for most interops.
	virtual void Update()
	{}
	//! Unnecessary for most interops.
	virtual void PostUpdate()
	{}
	//! Unnecessary for most interops.
	virtual void Shutdown()
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
	virtual void OnCryamblyInitilizing()
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
	virtual void SetInterface(IMonoInterface *handle) {}
};

#define REGISTER_METHOD(method) this->RegisterInteropMethod(#method, method)

//! Interface of interops that use classes within CryCil.Interops name space.
template<bool callRegistrationOnly>
struct IDefaultMonoInterop : public IMonoInterop < callRegistrationOnly, true >
{
	virtual const char *GetNameSpace() { return "CryCil.Interops"; }
};

//! Signature of the only method that is exported by MonoInterface.dll
typedef IMonoInterface *(*InitializeMonoInterface)(IGameFramework *, List<IMonoSystemListener *> *);

//! Pointer to IMonoInterface implementation for internal use.
//!
//! In order to have access to IMonoInterface implementation from other projects, similar
//! variable can be declared. It must be initialized with value returned by
//! InitializeCryCilSubsystem function exported by this Dll.
//!
//! Example:
//!
//! @code{.cpp}
//! // Load library. Save handle in a field or variable of type HMODULE.
//! this->monoInterfaceDll = CryLoadLibrary(MONOINTERFACE_LIBRARY);
//! // Check if it was loaded properly.
//! if (!this->monoInterfaceDll)
//! {
//!     CryFatalError("Could not locate MonoInterface.dll");
//! }
//! // Get InitializeModule function.
//! InitializeMonoInterface cryCilInitializer =
//!     CryGetProcAddress(this->monoInterfaceDll, MONO_INTERFACE_INIT);
//! // Invoke it, save the result in MonoEnv that was declared as a global somewhere else.
//! MonoEnv = cryCilInitializer(gameFramework, (pointer to listeners));
//! // Now MonoEnv can be used to communicate with CryCIL API!
//! @endcode
extern IMonoInterface *MonoEnv;

//! Specialization of IMonoInterop<,> template that relies on using MonoEnv variable
//! instead of internal field and unregisters and destroys itself after registration
//! of internal calls.
template<> struct IMonoInterop < true, true > : public IMonoInteropBase
{
	//! No saving.
	virtual void SetInterface(IMonoInterface *handle) {}
	//! Unregisters itself and commits suicide.
	virtual void OnCryamblyInitilizing()
	{
		MonoEnv->RemoveListener(this);
		delete this;
	}
};

#pragma region Conversions Interface

//! Creates managed string that contains given text.
//!
//! @param ntString Null-terminated string which text to copy to managed string.
inline mono::string ToMonoString(const char *ntString)
{
	return MonoEnv->ToManagedString(ntString);
}
//! Creates native null-terminated string from managed one.
//!
//! @param monoString Reference to a managed string to convert.
inline const char *ToNativeString(mono::string monoString)
{
	return MonoEnv->ToNativeString(monoString);
}

//! Boxing and unboxing are names of ways to marshal data to and from managed memory.
//!
//! Boxing is quite tricky due to C++ lacking any built-in metadata tracking
//! functionality. This means that are two ways of transferring the object to managed
//! memory:
//!     1) Official boxing : You have to get the class that will represent unmanaged
//!                          object in managed memory, then calling its Box method.
//!
//!     2) Boxing a pointer: You can use BoxPtr function to box a pointer to unmanaged
//!                          object, pass it managed method and let it dereference
//!                          that pointer.
//!
//!                          This method has some specifics though:
//!                           1) Make sure that managed are unmanaged types are blittable:
//!                            - Their objects take up the same amount of memory.
//!                            - Object is treated in same way in both codes.
//!
//!                           2) If the object contains pointer type fields, you will have
//!                              to dereference them as well before using them.
//! Examples:
//!
//! First method with built-in value-type:
//! @code{.cpp}
//! {
//!     mono::boolean boxedBool = Box(true);
//! }
//! @endcode
//!
//! First method with custom value-type:
//! @code{.cpp}
//! {
//!     // Get the type that will represent our object.
//!     IMonoClass *managedPlaneType =
//!         MonoEnv->Cryambly->GetClass("Plane", "CryCil.Mathematics.Geometry");
//!     // Box the object.
//!     mono::plane boxedPlane = managedPlaneType->Box(&plane);
//! }
//! @endcode
//!
//! Second method with custom type:
//!
//! C++:
//! @code{.cpp}
//! {
//!     Quat quaternion(1, 1, 1, 1);
//!     mono::exception exception;
//!     // Invoke unmanaged thunk that takes a pointer.
//!     mono::nothing result =
//!         ExampleQuatFunc
//!         (
//!             BoxPtr(&quaternion),                    // Box a pointer to our quaternion.
//!             &exception
//!         );
//! }
//! @endcode
//!
//! C#:
//! @code{.cs}
//! internal void ExampleQuatFunc(IntPtr quatHandle)
//! {
//!     // Convert a pointer to Quaternion * type and dereference it.
//!     Quaternion quat = *((Quaternion *)quatHandle.ToPointer());
//!     // Do something about this quaternion.
//!     ...
//! }
//! @endcode
//!
//! Unboxing on the other hand is relatively easy since .Net/Mono does have built-in
//! metadata tracking. This is why their is only one global function for unboxing.
//!
//! Example:
//!
//! @code{.cpp}
//! mono::boolean really = (...);
//! bool oReally = Unbox<bool>(really);          // EASY
//! @endcode
#define BOX_UNBOX

//! Unboxes a value-type object into another unmanaged value.
//! @tparam T type of unmanaged object to create.
//! @param value Value-type object to unbox.
//! @returns A verbatim copy of the memory that is occupied by managed object.
template<typename T>
BOX_UNBOX T Unbox(mono::object value)
{
	return *(T *)MonoEnv->Unbox(value);
}
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::intptr BoxUPtr(void *value) { return MonoEnv->DefaultBoxer->BoxUPtr(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::intptr BoxPtr(void *value) { return MonoEnv->DefaultBoxer->BoxPtr(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::boolean Box(bool value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::character Box(char value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::sbyte Box(signed char value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::byte Box(unsigned char value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::int16 Box(short value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::uint16 Box(unsigned short value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::int32 Box(int value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::uint32 Box(unsigned int value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::int64 Box(__int64 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::uint64 Box(unsigned __int64 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::float32 Box(float value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::float64 Box(double value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::vector2 Box(Vec2 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::vector3 Box(Vec3 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::vector4 Box(Vec4 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::angles3 Box(Ang3 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::quaternion Box(Quat value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::quat_trans Box(QuatT value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::matrix33 Box(Matrix33 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::matrix34 Box(Matrix34 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::matrix44 Box(Matrix44 value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::plane Box(Plane value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::ray Box(Ray value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::byte_color Box(ColorB value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::float32_color Box(ColorF value) { return MonoEnv->DefaultBoxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::aabb Box(AABB value) { return MonoEnv->DefaultBoxer->Box(value); }
#pragma endregion