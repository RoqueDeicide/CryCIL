#include "stdafx.h"

#include "Spline.h"
#include <CryMath/ISplines.h>

void SplineInterop::InitializeInterops()
{
	REGISTER_METHOD(GetNumDimensionsInternal);

	REGISTER_METHOD(InsertKeyInternal);
	REGISTER_METHOD(RemoveKeyInternal);

	REGISTER_METHOD(FindKeysInternal);
	REGISTER_METHOD(RemoveKeysInternal);

	REGISTER_METHOD(GetKeyCountInternal);
	REGISTER_METHOD(SetKeyTimeInternal);
	REGISTER_METHOD(GetKeyTimeInternal);
	REGISTER_METHOD(SetKeyValueInternal);
	REGISTER_METHOD(GetKeyValueInternal);

	REGISTER_METHOD(SetKeyInTangentInternal);
	REGISTER_METHOD(SetKeyOutTangentInternal);
	REGISTER_METHOD(SetKeyTangentsInternal);
	REGISTER_METHOD(GetKeyTangentsInternal);
}

int SplineInterop::GetNumDimensionsInternal(ISplineInterpolator *handle)
{
	return handle->GetNumDimensions();
}

int SplineInterop::InsertKeyInternal(ISplineInterpolator *handle, float time, Vec4 value)
{
	return handle->InsertKey(time, reinterpret_cast<float *>(&value));
}

void SplineInterop::RemoveKeyInternal(ISplineInterpolator *handle, int key)
{
	handle->RemoveKey(key);
}

void SplineInterop::FindKeysInternal(ISplineInterpolator *handle, float startTime, float endTime, int *firstFoundKey, int *numFoundKeys)
{
	handle->FindKeysInRange(startTime, endTime, *firstFoundKey, *numFoundKeys);
}

void SplineInterop::RemoveKeysInternal(ISplineInterpolator *handle, float startTime, float endTime)
{
	handle->RemoveKeysInRange(startTime, endTime);
}

int SplineInterop::GetKeyCountInternal(ISplineInterpolator *handle)
{
	return handle->GetKeyCount();
}

void SplineInterop::SetKeyTimeInternal(ISplineInterpolator *handle, int key, float time)
{
	handle->SetKeyTime(key, time);
}

float SplineInterop::GetKeyTimeInternal(ISplineInterpolator *handle, int key)
{
	return handle->GetKeyTime(key);
}

void SplineInterop::SetKeyValueInternal(ISplineInterpolator *handle, int key, Vec4 value)
{
	handle->SetKeyValue(key, reinterpret_cast<float *>(&value));
}

bool SplineInterop::GetKeyValueInternal(ISplineInterpolator *handle, int key, Vec4 *value)
{
	ISplineInterpolator::ValueType v;
	bool result = handle->GetKeyValue(key, v);
	*value = *reinterpret_cast<Vec4 *>(v);
	return result;
}

void SplineInterop::SetKeyInTangentInternal(ISplineInterpolator *handle, int key, Vec4 tin)
{
	handle->SetKeyInTangent(key, reinterpret_cast<float *>(&tin));
}

void SplineInterop::SetKeyOutTangentInternal(ISplineInterpolator *handle, int key, Vec4 tout)
{
	handle->SetKeyInTangent(key, reinterpret_cast<float *>(&tout));
}

void SplineInterop::SetKeyTangentsInternal(ISplineInterpolator *handle, int key, Vec4 tin, Vec4 tout)
{
	handle->SetKeyTangents(key, reinterpret_cast<float *>(&tin), reinterpret_cast<float *>(&tout));
}

bool SplineInterop::GetKeyTangentsInternal(ISplineInterpolator *handle, int key, Vec4 *tin, Vec4 *tout)
{
	ISplineInterpolator::ValueType v0, v1;
	bool result = handle->GetKeyTangents(key, v0, v1);
	*tin = *reinterpret_cast<Vec4 *>(v0);
	*tout = *reinterpret_cast<Vec4 *>(v1);
	return result;
}

void SplineInterop::InterpolateInternal(ISplineInterpolator *handle, float time, Vec4 *value)
{
	ISplineInterpolator::ValueType v;
	handle->Interpolate(time, v);
	*value = *reinterpret_cast<Vec4 *>(v);
}