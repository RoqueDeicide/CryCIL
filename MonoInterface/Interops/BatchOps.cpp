#include "stdafx.h"

#include "BatchOps.h"

const char *BatchOps::GetName()
{
	return "BatchOps";
}

const char *BatchOps::GetNameSpace()
{
	return "CryCil";
}

void BatchOps::OnRunTimeInitialized()
{
	REGISTER_METHOD(MathSimpleOpSingle);
	REGISTER_METHOD(MathSimpleOpDouble);
	REGISTER_METHOD(Math3NumberOpSingle);
	REGISTER_METHOD(Math3NumberOpDouble);
}

void BatchOps::MathSimpleOpSingle(float* numbers, __int64 count, MathSimpleOperations op)
{
	for (__int64 i = 0; i < count; i++)
	{
		numbers[i] = opsSingle[(int)op](numbers[i]);
	}
}

void BatchOps::MathSimpleOpDouble(double* numbers, __int64 count, MathSimpleOperations op)
{
	for (__int64 i = 0; i < count; i++)
	{
		numbers[i] = opsDouble[(int)op](numbers[i]);
	}
}

void BatchOps::Math3NumberOpSingle(Vec3* numbers, __int64 count, Math3NumberOperations op)
{
	for (__int64 i = 0; i < count; i++)
	{
		Vec3 v = numbers[i];
		switch (op)
		{
		case Power:
			v.z = powf(v.x, v.y);
			break;
		case Logarithm:
			v.z = logf(v.x) / logf(v.y);
			break;
		case SineCosine:
			v.y = sinf(v.x);
			v.z = sqrtf(v.y * v.y - 1);
			break;
		case Arctangent2:
			v.z = atan2f(v.x, v.y);
			break;
		case OpCount:
			break;
		default:
			break;
		}
		numbers[i] = v;
	}
}

void BatchOps::Math3NumberOpDouble(Vec3d* numbers, __int64 count, Math3NumberOperations op)
{
	for (__int64 i = 0; i < count; i++)
	{
		Vec3 v = numbers[i];
		switch (op)
		{
		case Power:
			v.z = pow(v.x, v.y);
			break;
		case Logarithm:
			v.z = log(v.x) / log(v.y);
			break;
		case SineCosine:
			v.y = sin(v.x);
			v.z = sqrt(v.y * v.y - 1);
			break;
		case Arctangent2:
			v.z = atan2(v.x, v.y);
			break;
		case OpCount:
			break;
		default:
			break;
		}
		numbers[i] = v;
	}
}

#pragma region Extra math functions

inline float  cotanf(float value)    { return 1 / tanf(value); }
inline double cotan (double value)   { return 1 / tan(value); }

inline float  cotanhf(float value)   { float doubleExp = expf(2 * value); return doubleExp + 1 / doubleExp - 1; }
inline double cotanh (double value)  { double doubleExp = exp(2 * value); return doubleExp + 1 / doubleExp - 1; }

inline float  acotanf(float value)   { return atanf(1 / value); }
inline double acotan (double value)  { return atan(1 / value); }

inline float  asinhf(float value)    { return logf(value + sqrtf(1 + value * value)); }
inline double asinh (double value)   { return log (value + sqrt(1 + value * value)); }

inline float  acoshf(float value)    { return logf(value + sqrtf(value + 1) * sqrtf(value - 1)); }
inline double acosh (double value)   { return log (value + sqrt(value + 1) * sqrt(value - 1)); }

inline float  atanhf(float value)    { return (logf(1 + value) - logf(1 - value)) / 2.0f; }
inline double atanh (double value)   { return (log (1 + value) - log(1 - value)) / 2.0; }

inline float  acotanhf(float value)  { float in = 1 / value; return (logf(1 + in) - logf(1 - in)) / 2.0f; }
inline double acotanh (double value) { double in = 1 / value; return (log(1 + in) - log(1 - in)) / 2.0; }

#pragma endregion


BatchOps::BatchOps()
{
	static MathOpSimpleSingle opsS[MathSimpleOperations::OpCount];
	static MathOpSimpleDouble opsD[MathSimpleOperations::OpCount];

	opsS[MathSimpleOperations::Sine] = sinf;
	opsD[MathSimpleOperations::Sine] = sin;

	opsS[MathSimpleOperations::Cosine] = cosf;
	opsD[MathSimpleOperations::Cosine] = cos;

	opsS[MathSimpleOperations::Tangent] = tanf;
	opsD[MathSimpleOperations::Tangent] = tan;

	opsS[MathSimpleOperations::Cotangent] = cotanf;
	opsD[MathSimpleOperations::Cotangent] = cotan;

	opsS[MathSimpleOperations::SineHyperbolic] = sinhf;
	opsD[MathSimpleOperations::SineHyperbolic] = sinh;

	opsS[MathSimpleOperations::CosineHyperbolic] = coshf;
	opsD[MathSimpleOperations::CosineHyperbolic] = cosh;

	opsS[MathSimpleOperations::TangentHyperbolic] = tanhf;
	opsD[MathSimpleOperations::TangentHyperbolic] = tanh;

	opsS[MathSimpleOperations::CotangentHyperbolic] = cotanhf;
	opsD[MathSimpleOperations::CotangentHyperbolic] = cotanh;

	opsS[MathSimpleOperations::Arcsine] = asinf;
	opsD[MathSimpleOperations::Arcsine] = asin;

	opsS[MathSimpleOperations::Arccosine] = acosf;
	opsD[MathSimpleOperations::Arccosine] = acos;

	opsS[MathSimpleOperations::Arctangent] = atanf;
	opsD[MathSimpleOperations::Arctangent] = atan;

	opsS[MathSimpleOperations::Arccotangent] = acotanf;
	opsD[MathSimpleOperations::Arccotangent] = acotan;

	opsS[MathSimpleOperations::ArcsineHyperbolic] = asinhf;
	opsD[MathSimpleOperations::ArcsineHyperbolic] = asinh;

	opsS[MathSimpleOperations::ArccosineHyperbolic] = acoshf;
	opsD[MathSimpleOperations::ArccosineHyperbolic] = acosh;

	opsS[MathSimpleOperations::ArctangentHyperbolic] = atanhf;
	opsD[MathSimpleOperations::ArctangentHyperbolic] = atanh;

	opsS[MathSimpleOperations::ArccotangentHyperbolic] = acotanhf;
	opsD[MathSimpleOperations::ArccotangentHyperbolic] = acotanh;

	opsS[MathSimpleOperations::LogarithmNatural] = logf;
	opsD[MathSimpleOperations::LogarithmNatural] = log;

	opsS[MathSimpleOperations::LogarithmDecimal] = log10f;
	opsD[MathSimpleOperations::LogarithmDecimal] = log10;

	opsS[MathSimpleOperations::Exponent] = expf;
	opsD[MathSimpleOperations::Exponent] = exp;

	opsSingle = &opsS[0];
	opsDouble = &opsD[0];
}

MathOpSimpleDouble *BatchOps::opsDouble;
MathOpSimpleSingle *BatchOps::opsSingle;