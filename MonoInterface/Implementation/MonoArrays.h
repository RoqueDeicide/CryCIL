#pragma once

#include "IMonoInterface.h"

//! Implementation for IMonoArrays.
struct MonoArrays : public IMonoArrays
{
	virtual mono::Array Create(int capacity, IMonoClass *klass = nullptr) override;
	virtual mono::Array Create(int dimCount, unsigned int *lengths, IMonoClass *klass = nullptr, int *lowerBounds = nullptr) override;
};