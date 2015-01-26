#pragma once

#include "IMonoInterface.h"

#include "Geometry/BspNode.h"

struct MeshOpsInterop : public IDefaultMonoInterop<true>
{

	virtual const char *GetName();

	virtual void OnRunTimeInitialized();

	static List<Face> *Combine(List<Face> *faces1, List<Face> *faces2);

	static List<Face> *Intersect(List<Face> *faces1, List<Face> *faces2);

	static List<Face> *Subtract(List<Face> *faces1, List<Face> *faces2);
};