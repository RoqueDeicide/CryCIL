#pragma once

#include "Interfaces/IMonoAliases.h"
#include "Interfaces/IMonoBox.h"
#include "Interfaces/IMonoFunctionalityWrapper.h"
#include "Interfaces/IMonoProperty.h"
#include "Interfaces/IMonoEvent.h"
#include "Interfaces/IMonoField.h"
#include "Interfaces/IMonoHandle.h"
#include "Interfaces/IMonoText.h"
#include "Interfaces/IMonoGC.h"
#include "Interfaces/IMonoGCHandle.h"
#include "Interfaces/IMonoAssembly.h"
#include "Interfaces/ICryambly.h"
#include "Interfaces/IMonoCoreLibrary.h"
#include "Interfaces/IMonoArray.h"
#include "Interfaces/IMonoClass.h"
#include "Interfaces/IMonoException.h"
#include "Interfaces/IMonoMethod.h"
#include "Interfaces/IMonoConstructor.h"
#include "Interfaces/IMonoDelegate.h"
#include "Interfaces/IMonoThread.h"
#include "Interfaces/IMonoObjects.h"
#include "Interfaces/IMonoSystemListener.h"

// Use MONOINTERFACE_LIBRARY constant to get OS-specific name of MonoInterface library.
#if defined(LINUX)
#define MONOINTERFACE_LIBRARY "MonoInterface.so"
#elif defined(APPLE)
#define MONOINTERFACE_LIBRARY "MonoInterface.dylib"
#else
#define MONOINTERFACE_LIBRARY "MonoInterface.dll"
#endif

#define MONO_INTERFACE_INIT "InitializeCryCilSubsystem"
//! Base class for MonoRunTime. Provides access to Mono interface.
struct IMonoInterface
{
	//! Triggers registration of FlowGraph nodes.
	//!
	//! Call this method from Game::RegisterGameFlowNodes function.
	VIRTUAL_API virtual void RegisterFlowGraphNodes() = 0;
	//! Shuts down Mono run-time environment.
	VIRTUAL_API virtual void Shutdown() = 0;
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
	__declspec(property(get = GetAssemblies)) IMonoAssemblies *Assemblies;
	//! Gets the pointer to IMonoAssembly that represents Cryambly.
	__declspec(property(get = GetCryambly)) ICryambly *Cryambly;
	//! Gets the pointer to IMonoAssembly that represents Pdb2mdb.
	__declspec(property(get = GetPdbMdbAssembly)) IMonoAssembly *Pdb2Mdb;
	//! Gets the pointer to IMonoAssembly that represents equivalent of mscorlib.
	__declspec(property(get = GetCoreLibrary)) IMonoCoreLibrary *CoreLibrary;
	//! Indicates whether Mono run-time environment is running.
	__declspec(property(get = GetInitializedIndication)) bool IsRunning;
	//! Gets the interface with Mono GC.
	__declspec(property(get = GetGC)) IMonoGC *GC;
	//! Gets the pointer to IGameFramework implementation that is available to CryCIL.
	__declspec(property(get = GetGameFramework)) IGameFramework *CryAction;
	//! Gets the interface that provides access to Mono object-related functionality.
	__declspec(property(get = GetObjects)) IMonoObjects *Objects;

	VIRTUAL_API virtual void *GetAppDomain() = 0;
	VIRTUAL_API virtual IMonoAssemblies *GetAssemblies() = 0;
	VIRTUAL_API virtual ICryambly *GetCryambly() = 0;
	VIRTUAL_API virtual IMonoAssembly *GetPdbMdbAssembly() = 0;
	VIRTUAL_API virtual IMonoCoreLibrary *GetCoreLibrary() = 0;
	VIRTUAL_API virtual bool GetInitializedIndication() = 0;
	VIRTUAL_API virtual IMonoGC *GetGC() = 0;
	VIRTUAL_API virtual IGameFramework *GetGameFramework() = 0;
	VIRTUAL_API virtual IMonoObjects *GetObjects() = 0;
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

// Include the file here, because we need MonoEnv to be declared.
#include "Interfaces/IMonoInterop.h"

#pragma region Conversions Interface

//! Creates managed string that contains given text.
//!
//! @param ntString Null-terminated string which text to copy to managed string.
inline mono::string ToMonoString(const char *ntString)
{
	return MonoEnv->Objects->Texts->ToManaged(ntString);
}
//! Creates native null-terminated string from managed one.
//!
//! @param monoString Reference to a managed string to convert.
inline const char *ToNativeString(mono::string monoString)
{
	return MonoEnv->Objects->Texts->ToNative(monoString);
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
	return *(T *)MonoEnv->Objects->Unbox(value);
}
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::intptr BoxUPtr(void *value) { return MonoEnv->Objects->Boxer->BoxUPtr(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::intptr BoxPtr(void *value) { return MonoEnv->Objects->Boxer->BoxPtr(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::boolean Box(bool value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::character Box(char value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::sbyte Box(signed char value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::byte Box(unsigned char value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::int16 Box(short value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::uint16 Box(unsigned short value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::int32 Box(int value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::uint32 Box(unsigned int value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::int64 Box(__int64 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::uint64 Box(unsigned __int64 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::float32 Box(float value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::float64 Box(double value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::vector2 Box(Vec2 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::vector3 Box(Vec3 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::vector4 Box(Vec4 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::angles3 Box(Ang3 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::quaternion Box(Quat value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::quat_trans Box(QuatT value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::matrix33 Box(Matrix33 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::matrix34 Box(Matrix34 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::matrix44 Box(Matrix44 value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::plane Box(Plane value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::ray Box(Ray value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::byte_color Box(ColorB value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::float32_color Box(ColorF value) { return MonoEnv->Objects->Boxer->Box(value); }
//! Boxes a value.
//!
//! @param value Value to box.
BOX_UNBOX inline mono::aabb Box(AABB value) { return MonoEnv->Objects->Boxer->Box(value); }
#pragma endregion