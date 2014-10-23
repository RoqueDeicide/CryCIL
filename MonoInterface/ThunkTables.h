#pragma once

#include "IMonoInterface.h"

typedef mono::object(*CreateInstanceThunk)(mono::type, mono::Array, mono::exception*);

typedef mono::boolean(*StaticEqualsThunk)(mono::object, mono::object, mono::exception*);

struct MonoClassThunks
{
	static CreateInstanceThunk CreateInstance;
	static StaticEqualsThunk StaticEquals;
};