#pragma once

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