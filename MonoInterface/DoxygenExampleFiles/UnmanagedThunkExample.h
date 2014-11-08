#include "stdafx.h"

#include "IMonoInterface.h"
// Lets pretend that this function will give some object, which hash code we need.
IMonoHandle *GetObj()
{
	return nullptr;
}
// Define the signature.
typedef mono::int32(__stdcall *GetHashCode) (mono::object thisObj, mono::exception *exception);

void example()
{
	IMonoHandle *obj = GetObj();
	// Get the method.
	IMonoMethod *hashCodeMethod = obj->GetClass()->GetMethod("GetHashCode");
	// Get the pointer to the thunk.
	GetHashCode hasCodeFunc = (GetHashCode)hashCodeMethod->UnmanagedThunk;
	// Prepare to invoke it.
	mono::exception exc;
	// Call it like a standard function pointer.
	int hasCode = Unbox<int>(hasCodeFunc(obj->Get(), &exc));
}