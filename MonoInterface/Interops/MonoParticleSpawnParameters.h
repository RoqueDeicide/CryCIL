#pragma once

#include <CryParticleSystem/IParticles.h>

struct MonoParticleSpawnParameters
{
	EGeomType eAttachType;             //!< What type of object particles emitted from.
	EGeomForm eAttachForm;             //!< What aspect of shape emitted from.
	bool      bCountPerUnit;           //!< Multiply particle count also by geometry extent (length/area/volume).
	bool      bPrime;                  //!< Advance emitter age to its equilibrium state.
	bool      bRegisterByBBox;         //!< Use the Bounding Box instead of Position to Register in VisArea.
	bool      bNowhere;                //!< Exists outside of level.
	float     fCountScale;             //!< Multiple for particle count (on top of bCountPerUnit if set).
	float     fSizeScale;              //!< Multiple for all effect sizes.
	float     fSpeedScale;             //!< Multiple for particle emission speed.
	float     fTimeScale;              //!< Multiple for emitter time evolution.
	float     fPulsePeriod;            //!< How often to restart emitter.
	float     fStrength;               //!< Controls parameter strength curves.

	bool                bEnableAudio;  //!< Used by particle effect instances to indicate whether audio should be updated or not.
	EAudioOcclusionType occlusionType; //!< Audio obstruction/occlusion calculation type.
	mono::string        audioRtpc;     //!< Indicates what audio RTPC this particle effect instance drives.

	MonoParticleSpawnParameters(const SpawnParams &params)
	{
		this->eAttachType = params.eAttachType;
		this->eAttachForm = params.eAttachForm;
		this->bCountPerUnit = params.bCountPerUnit;
		this->bPrime = params.bPrime;
		this->bRegisterByBBox = params.bRegisterByBBox;
		this->bNowhere = params.bNowhere;
		this->fCountScale = params.fCountScale;
		this->fSizeScale = params.fSizeScale;
		this->fSpeedScale = params.fSpeedScale;
		this->fTimeScale = params.fTimeScale;
		this->fPulsePeriod = params.fPulsePeriod;
		this->fStrength = params.fStrength;

		this->bEnableAudio = params.bEnableAudio;
		this->occlusionType = params.occlusionType;
		this->audioRtpc = ToMonoString(params.audioRtpc);
	}
	void ToNative(SpawnParams &params) const
	{
		params.eAttachType = this->eAttachType;
		params.eAttachForm = this->eAttachForm;
		params.bCountPerUnit = this->bCountPerUnit;
		params.bPrime = this->bPrime;
		params.bRegisterByBBox = this->bRegisterByBBox;
		params.bNowhere = this->bNowhere;
		params.fCountScale = this->fCountScale;
		params.fSizeScale = this->fSizeScale;
		params.fSpeedScale = this->fSpeedScale;
		params.fTimeScale = this->fTimeScale;
		params.fPulsePeriod = this->fPulsePeriod;
		params.fStrength = this->fStrength;

		params.bEnableAudio = this->bEnableAudio;
		params.occlusionType = this->occlusionType;
		params.audioRtpc = NtText(this->audioRtpc);
	}
};
