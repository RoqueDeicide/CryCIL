#pragma once

#include "IMonoInterface.h"

//! Implementation for IMonoArrays.
struct MonoArrays : public IMonoArrays
{
	mono::Array Create(int capacity, IMonoClass *klass, intptr_t lowerBound) override;
	mono::Array Create(const List<uintptr_t> &lengths, IMonoClass *klass) override;
};