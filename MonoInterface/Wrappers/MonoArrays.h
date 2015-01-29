#pragma once

#include "IMonoInterface.h"

struct MonoArrays : public IMonoArrays
{
	virtual IMonoArray *Create(int capacity, IMonoClass *klass = nullptr);

	virtual IMonoArray *Create(int dimCount, unsigned int *lengths, IMonoClass *klass = nullptr, int *lowerBounds = nullptr);

	virtual IMonoArray *Wrap(mono::Array arrayHandle);
};