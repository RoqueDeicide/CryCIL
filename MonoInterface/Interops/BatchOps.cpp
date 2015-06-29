#include "stdafx.h"

#include "BatchOps.h"

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
		switch (op)
		{
		case Sine:
			numbers[i] = sinf(numbers[i]);
			break;
		case Cosine:
			numbers[i] = cosf(numbers[i]);
			break;
		case Tangent:
			numbers[i] = tanf(numbers[i]);
			break;
		case Cotangent:
			numbers[i] = 1 / tanf(numbers[i]);
			break;
		case Arcsine:
			numbers[i] = asinf(numbers[i]);
			break;
		case Arccosine:
			numbers[i] = acosf(numbers[i]);
			break;
		case Arctangent:
			numbers[i] = atanf(numbers[i]);
			break;
		case Arccotangent:
			numbers[i] = atanf(1 / numbers[i]);
			break;
		case SineHyperbolic:
			numbers[i] = sinhf(numbers[i]);
			break;
		case CosineHyperbolic:
			numbers[i] = coshf(numbers[i]);
			break;
		case TangentHyperbolic:
			numbers[i] = tanhf(numbers[i]);
			break;
		case CotangentHyperbolic:
		{
			float doubleExp = expf(2 * numbers[i]);
			numbers[i] = doubleExp + 1 / doubleExp - 1;
		}
			break;
		case ArcsineHyperbolic:
		{
			float value = numbers[i];
			numbers[i] = logf(value + sqrtf(1 + value * value));
		}
			break;
		case ArccosineHyperbolic:
		{
			float value = numbers[i];
			numbers[i] = logf(value + sqrtf(value + 1) * sqrtf(value - 1));
		}
			break;
		case ArctangentHyperbolic:
		{
			float value = numbers[i];
			numbers[i] = (logf(1 + value) - logf(1 - value)) / 2.0f;
		}
			break;
		case ArccotangentHyperbolic:
		{
			float in = 1 / numbers[i];
			numbers[i] = (logf(1 + in) - logf(1 - in)) / 2.0f;
		}
			break;
		case LogarithmNatural:
			numbers[i] = logf(numbers[i]);
			break;
		case LogarithmDecimal:
			numbers[i] = log10f(numbers[i]);
			break;
		case Exponent:
			numbers[i] = expf(numbers[i]);
			break;
		default:
			break;
		}
	}
}

void BatchOps::MathSimpleOpDouble(double* numbers, __int64 count, MathSimpleOperations op)
{
	for (__int64 i = 0; i < count; i++)
	{
		switch (op)
		{
		case Sine:
			numbers[i] = sin(numbers[i]);
			break;
		case Cosine:
			numbers[i] = cos(numbers[i]);
			break;
		case Tangent:
			numbers[i] = tan(numbers[i]);
			break;
		case Cotangent:
			numbers[i] = 1 / tan(numbers[i]);
			break;
		case Arcsine:
			numbers[i] = asin(numbers[i]);
			break;
		case Arccosine:
			numbers[i] = acos(numbers[i]);
			break;
		case Arctangent:
			numbers[i] = atan(numbers[i]);
			break;
		case Arccotangent:
			numbers[i] = atan(1 / numbers[i]);
			break;
		case SineHyperbolic:
			numbers[i] = sinh(numbers[i]);
			break;
		case CosineHyperbolic:
			numbers[i] = cosh(numbers[i]);
			break;
		case TangentHyperbolic:
			numbers[i] = tanh(numbers[i]);
			break;
		case CotangentHyperbolic:
		{
			double doubleExp = exp(2 * numbers[i]);
			numbers[i] = doubleExp + 1 / doubleExp - 1;
		}
			break;
		case ArcsineHyperbolic:
		{
			double value = numbers[i];
			numbers[i] = log(value + sqrt(1 + value * value));
		}
			break;
		case ArccosineHyperbolic:
		{
			double value = numbers[i];
			numbers[i] = log(value + sqrt(value + 1) * sqrt(value - 1));
		}
			break;
		case ArctangentHyperbolic:
		{
			double value = numbers[i];
			numbers[i] = (log(1 + value) - log(1 - value)) / 2.0f;
		}
			break;
		case ArccotangentHyperbolic:
		{
			double in = 1 / numbers[i];
			numbers[i] = (log(1 + in) - log(1 - in)) / 2.0f;
		}
			break;
		case LogarithmNatural:
			numbers[i] = log(numbers[i]);
			break;
		case LogarithmDecimal:
			numbers[i] = log10(numbers[i]);
			break;
		case Exponent:
			numbers[i] = exp(numbers[i]);
			break;
		default:
			break;
		}
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
		default:
			break;
		}
		numbers[i] = v;
	}
}