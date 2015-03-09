#pragma once

#include "IMonoInterface.h"

struct MonoArrays : public IMonoArrays
{
	virtual mono::Array Create(int capacity, IMonoClass *klass = nullptr);
	virtual mono::Array Create(int dimCount, unsigned int *lengths, IMonoClass *klass = nullptr, int *lowerBounds = nullptr);
};