#include "stdafx.h"

#include "CheckingBasics.h"

TYPE_MIRROR struct EntityUpdateContext
{
	// Current rendering frame id.
	int nFrameID;
	// Current camera.
	CCamera *pCamera;
	// Current system time.
	float fCurrTime;
	// Delta frame time (of last frame).
	float fFrameTime;
	// Indicates if a profile entity must update the log.
	bool bProfileToLog;
	// Number of updated entities.
	int numUpdatedEntities;
	// Number of visible and updated entities.
	int numVisibleEntities;
	// Maximal view distance.
	float fMaxViewDist;
	// Maximal view distance squared.
	float fMaxViewDistSquared;
	// Camera source position.
	Vec3 vCameraPos;
	EntityUpdateContext(SEntityUpdateContext other)
	{
		this->nFrameID = other.nFrameID;
		this->pCamera = other.pCamera;
		this->fCurrTime = other.fCurrTime;
		this->fFrameTime = other.fFrameTime;
		this->bProfileToLog = other.bProfileToLog;
		this->numUpdatedEntities = other.numUpdatedEntities;
		this->numVisibleEntities = other.numVisibleEntities;
		this->fMaxViewDist = other.fMaxViewDist;
		this->fMaxViewDistSquared = other.fMaxViewDistSquared;
		this->vCameraPos = other.vCameraPos;

		CHECK_TYPE(nFrameID);
		CHECK_TYPE(pCamera);
		CHECK_TYPE(fCurrTime);
		CHECK_TYPE(fFrameTime);
		CHECK_TYPE(bProfileToLog);
		CHECK_TYPE(numUpdatedEntities);
		CHECK_TYPE(numVisibleEntities);
		CHECK_TYPE(fMaxViewDist);
		CHECK_TYPE(fMaxViewDistSquared);
		CHECK_TYPE(vCameraPos);
	}
};