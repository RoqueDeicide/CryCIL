#include "ExampleDefines.h"

#ifdef EXAMPLES

#include "IMonoInterface.h"
// Lets pretend that this function will give some object, which hash code we need.
IMonoHandle *GetObj()
{
	return nullptr;
}
// Define the signature. Use mono::object wherever method accepts managed objects.
// Don't worry about methods that might have seemingly identical signatures sometimes,
// just pass objects of correct types when finding the method and Mono will figure
// out the rest.
//
// Notice that, if you want to invoke the instance method you have to pass the pointer
// to that instance as the first argument. Static methods are not associated with instances,
// therefore static method that accepts no arguments 's thunk's signature has no arguments at all.
typedef int (__stdcall *GetHashCode) (mono::object thisObj);

void example()
{
	IMonoHandle *obj = GetObj();
	// Get the method.
	IMonoMethod *hashCodeMethod = obj->GetClass()->GetMethod("GetHashCode");
	// Get the pointer to the thunk.
	GetHashCode hasCodeFunc = (GetHashCode)hashCodeMethod->UnmanagedThunk;
	// Call it like a standard function pointer.
	int hasCode = hasCodeFunc(obj->Get());
}
#endif // EXAMPLES