#pragma once

#include "IMonoInterface.h"

struct WaterManagerParameters
{
	params_wavesim waveSim;
	Vec3 posViewer;
	int nExtraTiles;
	int nCells;
	float tileSize;

	void ToParams(pe_params_waterman &params) const
	{
		params.dampingCenter = this->waveSim.dampingCenter;
		params.dampingRim = this->waveSim.dampingRim;
		params.heightLimit = this->waveSim.heightLimit;
		params.minhSpread = this->waveSim.minhSpread;
		params.minVel = this->waveSim.minVel;
		params.resistance = this->waveSim.resistance;
		params.simDepth = this->waveSim.simDepth;
		params.timeStep = this->waveSim.timeStep;
		params.waveSpeed = this->waveSim.waveSpeed;
		params.posViewer = this->posViewer;
		params.nExtraTiles = this->nExtraTiles;
		params.nCells = this->nCells;
		params.tileSize = this->tileSize;
	}
	void FromParams(const pe_params_waterman &params)
	{
		this->waveSim.dampingCenter = params.dampingCenter;
		this->waveSim.dampingRim = params.dampingRim;
		this->waveSim.heightLimit = params.heightLimit;
		this->waveSim.minhSpread = params.minhSpread;
		this->waveSim.minVel = params.minVel;
		this->waveSim.resistance = params.resistance;
		this->waveSim.simDepth = params.simDepth;
		this->waveSim.timeStep = params.timeStep;
		this->waveSim.waveSpeed = params.waveSpeed;
		this->posViewer = params.posViewer;
		this->nExtraTiles = params.nExtraTiles;
		this->nCells = params.nCells;
		this->tileSize = params.tileSize;
	}
};