#include "stdafx.h"
#include "MonoArrays.h"
#include "MonoArray.h"

IMonoArray *MonoArrays::Create(int capacity, IMonoClass *klass /*= nullptr*/)
{
	return new MonoArrayWrapper(klass, capacity);
}

IMonoArray *MonoArrays::Create(int dimCount, unsigned int *lengths, IMonoClass *klass /*= nullptr*/, int *lowerBounds /*= nullptr*/)
{
	return new MonoArrayWrapper(dimCount, lengths, klass, lowerBounds);
}

IMonoArray *MonoArrays::Wrap(mono::Array arrayHandle)
{
	return new MonoArrayWrapper((MonoArray *)arrayHandle);
}