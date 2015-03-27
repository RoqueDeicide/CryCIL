#pragma once

#include "DocumentationMarkers.h"

#include "Interfaces/IMonoAliases.h"
#include "Interfaces/IMonoBox.h"
#include "Interfaces/IMonoFunctionalityWrapper.h"
#include "Interfaces/IMonoMember.h"
#include "Interfaces/IMonoFunctions.h"
#include "Interfaces/IMonoProperty.h"
#include "Interfaces/IMonoEvent.h"
#include "Interfaces/IMonoField.h"
#include "Interfaces/IMonoGC.h"
#include "Interfaces/IMonoAssembly.h"
#include "Interfaces/ICryambly.h"
#include "Interfaces/IMonoCoreLibrary.h"
#include "Interfaces/IMonoClass.h"
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
protected:
	// You better not change these fields, it only gonna mess stuff up (If you are not working on CryCil).

	bool running;

	IMonoAssemblies *assemblies;
	ICryambly *cryambly;					//! Extra pointer for Cryambly.
	IMonoCoreLibrary *corlib;				//! Extra pointer for mscorlib.
	IMonoAssembly *pdb2mdb;

	IMonoGC *gc;
	IGameFramework *framework;
	IMonoObjects *objs;
	IMonoFunctions *funcs;
public:
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
	//! Gets the interface that provides access to Mono functions API.
	__declspec(property(get = GetFunctions)) IMonoFunctions *Functions;

	VIRTUAL_API virtual void *GetAppDomain() = 0;
	IMonoAssemblies *GetAssemblies()
	{
		return this->assemblies;
	}
	ICryambly *GetCryambly()
	{
		return this->cryambly;
	}
	IMonoAssembly *GetPdbMdbAssembly()
	{
		return this->pdb2mdb;
	}
	IMonoCoreLibrary *GetCoreLibrary()
	{
		return this->corlib;
	}
	bool GetInitializedIndication()
	{
		return this->running;
	}
	IMonoGC *GetGC()
	{
		return this->gc;
	}
	IGameFramework *GetGameFramework()
	{
		return this->framework;
	}
	IMonoObjects *GetObjects()
	{
		return this->objs;
	}
	IMonoFunctions *GetFunctions()
	{
		return this->funcs;
	}
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

inline bool IMonoObjects::MonitorTryEnter(mono::object obj, unsigned int timeout)
{
	static void(*try_enter_with_atomic_var)(mono::object, unsigned int, char *) =
		(void(*)(mono::object, unsigned int, char *))
		MonoEnv->Functions->LookupInternalCall
		(
			MonoEnv->CoreLibrary->GetClass("System.Threading", "Monitor")
								->GetFunction("try_enter_with_atomic_var", -1)
		);

	char lockTaken = 0;
	try_enter_with_atomic_var(obj, timeout, &lockTaken);
	return lockTaken != 0;
}

inline bool IMonoObjects::MonitorIsEntered(mono::object obj)
{
	static int(*Monitor_test_owner)(mono::object) =
		(int(*)(mono::object))
		MonoEnv->Functions->LookupInternalCall
		(
			MonoEnv->CoreLibrary->GetClass("System.Threading", "Monitor")
								->GetFunction("Monitor_test_owner", -1)
		);

	return Monitor_test_owner(obj) != 0;
}

// Include these files here, because we need MonoEnv to be declared for them.
#include "Interfaces/IMonoInterop.h"
#include "Interfaces/MonoGCHandle.h"
#include "Interfaces/IMonoFunction.h"
#include "Interfaces/IMonoStaticMethod.h"
#include "Interfaces/IMonoConstructor.h"
#include "Interfaces/IMonoMethod.h"

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

#include "Interfaces/IMonoObject.h"
#include "Interfaces/IMonoArray.h"
#include "Interfaces/IMonoDelegate.h"
#include "Interfaces/IMonoException.h"
#include "Interfaces/IMonoThread.h"
#include "Interfaces/IMonoText.h"