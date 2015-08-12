#pragma once

#include "IMonoInterface.h"

enum MathSimpleOperations
{
	Sine = 0,
	Cosine,
	Tangent,
	Cotangent,
	Arcsine,
	Arccosine,
	Arctangent,
	Arccotangent,
	SineHyperbolic,
	CosineHyperbolic,
	TangentHyperbolic,
	CotangentHyperbolic,
	ArcsineHyperbolic,
	ArccosineHyperbolic,
	ArctangentHyperbolic,
	ArccotangentHyperbolic,
	LogarithmNatural,
	LogarithmDecimal,
	Exponent
};

enum Math3NumberOperations
{
	Power = 0,
	Logarithm,
	SineCosine,
	Arctangent2
};

struct BatchOps : public IMonoInterop<true>
{
	virtual const char *GetInteropClassName() override { return "BatchOps"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil"; }

	virtual void OnRunTimeInitialized() override;

	static void MathSimpleOpSingle(float* numbers, __int64 count, MathSimpleOperations op);
	static void MathSimpleOpDouble(double* numbers, __int64 count, MathSimpleOperations op);
	static void Math3NumberOpSingle(Vec3* numbers, __int64 count, Math3NumberOperations op);
	static void Math3NumberOpDouble(Vec3d* numbers, __int64 count, Math3NumberOperations op);
};