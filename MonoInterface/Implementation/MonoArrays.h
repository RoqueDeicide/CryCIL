#pragma once

#include "IMonoInterface.h"

//! Implementation for IMonoArrays.
struct MonoArrays : public IMonoArrays
{
	virtual mono::Array Create(int capacity, IMonoClass *klass = nullptr, intptr_t lowerBound = 0) override;
	virtual mono::Array Create(const List<uintptr_t> &lengths, IMonoClass *klass = nullptr) override;
};