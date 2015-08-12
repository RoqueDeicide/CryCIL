#pragma once

#include "IMonoInterface.h"

struct SplineInterop : public IMonoInterop < true, true >
{
	virtual const char *GetInteropClassName() override { return "CryEngineSpline"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Geometry.Splines"; }

	virtual void OnRunTimeInitialized() override;

	static int GetNumDimensionsInternal(ISplineInterpolator *handle);

	static int InsertKeyInternal(ISplineInterpolator *handle, float time, Vec4 value);
	static void RemoveKeyInternal(ISplineInterpolator *handle, int key);

	static void FindKeysInternal(ISplineInterpolator *handle, float startTime, float endTime, int *firstFoundKey, int *numFoundKeys);
	static void RemoveKeysInternal(ISplineInterpolator *handle, float startTime, float endTime);

	static int GetKeyCountInternal(ISplineInterpolator *handle);
	static void SetKeyTimeInternal(ISplineInterpolator *handle, int key, float time);
	static float GetKeyTimeInternal(ISplineInterpolator *handle, int key);
	static void SetKeyValueInternal(ISplineInterpolator *handle, int key, Vec4 value);
	static bool GetKeyValueInternal(ISplineInterpolator *handle, int key, Vec4 *value);

	static void SetKeyInTangentInternal(ISplineInterpolator *handle, int key, Vec4 tin);
	static void SetKeyOutTangentInternal(ISplineInterpolator *handle, int key, Vec4 tout);
	static void SetKeyTangentsInternal(ISplineInterpolator *handle, int key, Vec4 tin, Vec4 tout);
	static bool GetKeyTangentsInternal(ISplineInterpolator *handle, int key, Vec4 *tin, Vec4 *tout);

	static void InterpolateInternal(ISplineInterpolator *handle, float time, Vec4 *value);
};