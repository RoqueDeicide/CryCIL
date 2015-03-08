#pragma once

//! Represents a GC handle.
//!
//! GC handles are used to inform Mono run-time environment about references to managed objects from
//! unmanaged memory.
//!
//! There are 4 types of GC handles, all to be used in various situations:
//!
//!     1) Weak - weak GC handles allow you to track position of the object in managed memory as it
//!        gets moved around by the garbage collector, however, it doesn't count as a reliable reference
//!        to the object, which means that, if there are no more reliable references (references in
//!        managed memory, strong GC handles), the object will become eligible for the garbage collection.
//!        Bear in mind that this type of GC handle loses access to the managed object that
//!        is set for collection, even it it gets resurrected by the Finalize() method of that object.
//!     2) Weak with resurrection tracking - very similar to the previous type of GC handle, except this
//!        one will not lose access to the object that was resurrected by the destructor (Finalize method).
//!     3) Strong - strong GC handles work as weak ones but they count as reliable references to the object
//!        preventing garbage collection of the object when only references to it are in unmanaged memory.
//!     4) Pin - pins are special strong GC handles that pin location of the managed object in place,
//!        preventing GC from collecting it or moving to a different location.
struct MonoGCHandle
{
protected:
	unsigned int handle;
public:
	MonoGCHandle(unsigned int handle)
	{
		this->handle = handle;
	}
	~MonoGCHandle()
	{
		if (this->handle == -1)
		{
			return;
		}
		MonoEnv->GC->ReleaseGCHandle(this->handle);
		this->handle = -1;
	}
	//! Swaps internal GC handle identifiers held by this wrapper and another one.
	SWAP_ASSIGNMENT MonoGCHandle &operator= (MonoGCHandle &other)
	{
		if (this->handle != other.handle)
		{
			unsigned int temp = this->handle;
			this->handle = other.handle;
			other.handle = this->handle;
		}
		return *this;
	}
	//! Assigns a new GC handle to this object.
	//!
	//! GC handle that is held by this object at the moment of invocation will be released.
	//!
	//! Use either Detach() or Separate() or Release() if you don't want the handle to be released.
	MonoGCHandle &operator= (unsigned int other)
	{
		if (this->handle != other)
		{
			this->Release();
			this->handle = other;
		}
		return *this;
	}
	//! Detaches this GC handle that is wrapped by this object, preventing its automatic release.
	//!
	//! @returns A GC handle identifier that will have to be released by other means.
	unsigned int Detach()
	{
		unsigned int t = this->handle;
		this->handle = -1;
		return t;
	}
	//! Detaches this GC handle that is wrapped by this object, preventing its automatic release.
	//!
	//! @returns A reference to the current object allowing the chaining.
	MonoGCHandle &Separate()
	{
		this->handle = -1;
		return *this;
	}
	//! Detaches this GC handle that is wrapped by this object, preventing its automatic release.
	//!
	//! @returns A GC handle identifier that will have to be released by other means.
	MonoGCHandle &Release()
	{
		this->~MonoGCHandle();
		return *this;
	}

	//! Gets the pointer to managed object that is being held by this GC handle.
	//!
	//! @returns A pointer that can be passed directly to methods and thunks, or null if
	//!          this is a weak handle and held object has been collected.
	__declspec(property(get = GetObjectPointer)) mono::object Object;

	mono::object GetObjectPointer()
	{
		return MonoEnv->GC->GetGCHandleTarget(this->handle);
	}
};

