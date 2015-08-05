#include "stdafx.h"

#include "IMonoInterface.h"

// Define the signature.
typedef void (__stdcall *Clamp)(mono::vector2 value, mono::vector2 min, mono::vector2 max, mono::vector2 result, mono::exception *ex);

inline void example()
{
	// Get the method.
	auto clampMethod = MonoEnv->Cryambly->GetClass("CryCil", "Vector2")->GetFunction("Clamp", 4);
	// Get the pointer to the thunk.
	Clamp clamp = Clamp(clampMethod->UnmanagedThunk);
	// Prepare to invoke it.
	mono::vector2 value =  Box(Vec2(10, 13));
	mono::vector2 min =    Box(Vec2(13, 8));
	mono::vector2 max =    Box(Vec2(30, 12));
	mono::vector2 result = Box(Vec2(0, 0));         // Box default values when using "out" parameters.
	                                                // Otherwise expect the program to crash.
	mono::exception exc;
	// Call it like a standard function pointer.
	clamp(value, min, max, result, &exc);
	if (exc)
	{
		MonoEnv->HandleException(exc);
	}
	else
	{
		// This code region is the only place where the result of invocation is defined.
        
		// Unbox the "out" parameter.
		Vec2 vector = Unbox<Vec2>(result);
		// Print the components.
		CryLogAlways("X component of the vector = %d", vector.x);
		CryLogAlways("Y component of the vector = %d", vector.y);
	}
}