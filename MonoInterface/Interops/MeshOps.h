#pragma once

#include "IMonoInterface.h"

#include "Geometry/BspNode.h"

struct CsgOpCode
{
	enum
	{
		Combine,
		Intersect,
		Subtract
	};
};

struct MeshOpsInterop : public IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FaceMesh"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Geometry"; }

	virtual void OnRunTimeInitialized() override;

	static void DeleteListItems(Face* facesPtr);
	static Face *CsgOpInternal(Face* facesPtr1, int faceCount1, Face* facesPtr2, int faceCount2, int op, int &faceCount);
};