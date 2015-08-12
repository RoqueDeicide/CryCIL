#pragma once

#include "IMonoInterface.h"

#include "Geometry/BspNode.h"

struct MeshOpsInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FaceMesh"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Geometry"; }

	virtual void OnRunTimeInitialized() override;

	static List<Face> *CombineInternal  (List<Face> *faces1, List<Face> *faces2);
	static List<Face> *IntersectInternal(List<Face> *faces1, List<Face> *faces2);
	static List<Face> *SubtractInternal (List<Face> *faces1, List<Face> *faces2);
};