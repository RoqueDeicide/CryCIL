#pragma once

#include "IMonoInterface.h"

#include "Geometry/BspNode.h"

struct MeshOpsInterop : public IMonoInterop<true, true>
{
	virtual const char *GetName() { return "FaceMesh"; }
	virtual const char *GetNameSpace() { return "CryCil.Geometry"; }

	virtual void OnRunTimeInitialized();

	static List<Face> *CombineInternal  (List<Face> *faces1, List<Face> *faces2);
	static List<Face> *IntersectInternal(List<Face> *faces1, List<Face> *faces2);
	static List<Face> *SubtractInternal (List<Face> *faces1, List<Face> *faces2);
};