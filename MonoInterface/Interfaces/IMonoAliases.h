#pragma once

#ifndef PI
#define PI 3.14159265358979323f
#endif

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

#include "List.hpp"
#include "SortedList.h"
#include "Tuples.h"

#include "DebugHelpers.h"

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
	//! Represents a reference a delegate.
	OBJECT_NAME typedef object delegat;
	//! Represents a reference to a managed thread interface.
	OBJECT_NAME typedef object Thread;
	//! Represents a reference to a managed exception object.
	OBJECT_NAME typedef object exception;
	//! Represents a reference to a managed System.Type object.
	OBJECT_NAME typedef object type;
	//! Represents a reference an object of type System.Reflection.MethodInfo.
	OBJECT_NAME typedef object method;
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

// Forward declarations.
struct IDefaultBoxinator;
struct IMonoFunctionalityWrapper;
struct IMonoObject;
struct IMonoAssembly;
template<typename ElementType = mono::object> struct IMonoArray;
struct IMonoClass;
struct IMonoMethod;
struct IMonoStaticMethod;
struct IMonoFunction;
struct IMonoConstructor;
struct IMonoProperty;
struct IMonoEvent;
struct IMonoField;
struct IMonoConstructor;
struct IMonoSystemListener;
struct IMonoInterface;
struct MonoGCHandle;
struct IMonoGC;

template<typename symbol> struct TextTemplate;
typedef TextTemplate<char> Text;
typedef TextTemplate<wchar_t> Text16;