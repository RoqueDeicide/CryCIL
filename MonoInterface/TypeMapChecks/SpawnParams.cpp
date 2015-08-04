#include "stdafx.h"

#include "CheckingBasics.h"
#include <IParticles.h>

typedef SpawnParams SParticleSpawnParams;

TYPE_MIRROR struct ParticleSpawnParams
{
	EGeomType			eAttachType;			// What type of object particles emitted from.
	EGeomForm			eAttachForm;			// What aspect of shape emitted from.
	bool					bCountPerUnit;		// Multiply particle count also by geometry extent (length/area/volume).
	bool					bEnableAudio;     // Used by particle effect instances to indicate whether audio should be updated or not.
	bool					bRegisterByBBox;	// Use the Bounding Box instead of Position to Register in VisArea
	float					fCountScale;			// Multiple for particle count (on top of bCountPerUnit if set).
	float					fSizeScale;				// Multiple for all effect sizes.
	float					fSpeedScale;			// Multiple for particle emission speed.
	float					fTimeScale;				// Multiple for emitter time evolution.
	float					fPulsePeriod;			// How often to restart emitter.
	float					fStrength;				// Controls parameter strength curves.
	stack_string	sAudioRTPC;				// Indicates what audio RTPC this particle effect instance drives.

	ParticleSpawnParams(SpawnParams other)
	{
		CHECK_TYPE_SIZE(ParticleSpawnParams);

		ASSIGN_FIELD(eAttachType);
		ASSIGN_FIELD(eAttachForm);
		ASSIGN_FIELD(bCountPerUnit);
		ASSIGN_FIELD(bEnableAudio);
		ASSIGN_FIELD(bRegisterByBBox);
		ASSIGN_FIELD(fCountScale);
		ASSIGN_FIELD(fSizeScale);
		ASSIGN_FIELD(fSpeedScale);
		ASSIGN_FIELD(fTimeScale);
		ASSIGN_FIELD(fPulsePeriod);
		ASSIGN_FIELD(fStrength);
		ASSIGN_FIELD(sAudioRTPC);

		CHECK_TYPE(eAttachType);
		CHECK_TYPE(eAttachForm);
		CHECK_TYPE(bCountPerUnit);
		CHECK_TYPE(bEnableAudio);
		CHECK_TYPE(bRegisterByBBox);
		CHECK_TYPE(fCountScale);
		CHECK_TYPE(fSizeScale);
		CHECK_TYPE(fSpeedScale);
		CHECK_TYPE(fTimeScale);
		CHECK_TYPE(fPulsePeriod);
		CHECK_TYPE(fStrength);
		CHECK_TYPE(sAudioRTPC);
	}
};