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
	Exponent,
	OpCount						//!< Total number of supported operations.
};

enum Math3NumberOperations
{
	Power = 0,
	Logarithm,
	SineCosine,
	Arctangent2,
	OpCount						//!< Total number of supported operations.
};

typedef float(*MathOpSimpleSingle)(float);
typedef double(*MathOpSimpleDouble)(double);

struct BatchOps : public IMonoInterop<true>
{
	BatchOps();

	virtual const char *GetName();
	virtual const char *GetNameSpace();

	virtual void OnRunTimeInitialized();

	static void MathSimpleOpSingle(float* numbers, __int64 count, MathSimpleOperations op);
	static void MathSimpleOpDouble(double* numbers, __int64 count, MathSimpleOperations op);
	static void Math3NumberOpSingle(Vec3* numbers, __int64 count, Math3NumberOperations op);
	static void Math3NumberOpDouble(Vec3d* numbers, __int64 count, Math3NumberOperations op);

	static MathOpSimpleSingle *opsSingle;
	static MathOpSimpleDouble *opsDouble;
};