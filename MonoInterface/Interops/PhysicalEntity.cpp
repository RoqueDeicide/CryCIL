#include "stdafx.h"

#include "PhysicalEntity.h"

typedef pe_params *(*ConvertToNativeParametersFunc)(PhysicsParameters *);
typedef void(*DisposeParametersFunc)(PhysicsParameters *);
typedef void(*ConvertToMonoParametersFunc)(pe_params *, PhysicsParameters *);

template<typename ParamsType>
pe_params *ParamsToCE(PhysicsParameters *parameters)
{
	return reinterpret_cast<ParamsType *>(parameters)->ToParams();
}
template<typename ParamsType>
void DisposeParams(PhysicsParameters *parameters)
{
	reinterpret_cast<ParamsType *>(parameters)->Dispose();
}
template<typename ParamsType>
void ParamsToMono(pe_params *pars, PhysicsParameters *parameters)
{
	reinterpret_cast<ParamsType *>(parameters)->FromParams(pars);
}

#define DECLARE_PARAMS_PROCESSING_FUNC(name, functionPtrType, functionPtr, typeCount) \
functionPtrType name(int type)\
{\
	static functionPtrType funcs[typeCount];\
	static bool initialized = false;\
	\
	if (!initialized)\
	{\
		memset(funcs, 0, sizeof(functionPtrType) * typeCount);\
		funcs[ePE_params_pos] = functionPtr<PhysicsParametersLocation>;\
		initialized = true;\
	}\
	\
	if (type < 0 || type >= typeCount)\
	{\
		return nullptr;\
	}\
	\
	return funcs[type];\
}

DECLARE_PARAMS_PROCESSING_FUNC(GetParamConverterToCE,   ConvertToNativeParametersFunc, ParamsToCE,    ePE_Params_Count)
DECLARE_PARAMS_PROCESSING_FUNC(GetParamDisposer,        DisposeParametersFunc,         DisposeParams, ePE_Params_Count)
DECLARE_PARAMS_PROCESSING_FUNC(GetParamConverterToMono, ConvertToMonoParametersFunc,   ParamsToMono,  ePE_Params_Count)

void PhysicalEntityInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetParams);
	REGISTER_METHOD(GetParams);
}

int PhysicalEntityInterop::SetParams(IPhysicalEntity *handle, PhysicsParameters *parameters, bool threadSafe)
{
	auto converter = GetParamConverterToCE(parameters->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto params = converter(parameters);
	int result = handle->SetParams(params, threadSafe);
	// Delete CryEngine object.
	delete params;
	// Dispose the CryCIL object.
	GetParamDisposer(parameters->type)(parameters);
	return result;
}

int PhysicalEntityInterop::GetParams(IPhysicalEntity *handle, PhysicsParameters *parameters)
{
	auto converter = GetParamConverterToCE(parameters->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto params = converter(parameters);
	int result = handle->GetParams(params);
	// Store result in the CryCIL object.
	GetParamConverterToMono(parameters->type)(params, parameters);
	// Delete CryEngine object.
	delete params;
	return result;
}
