#include "stdafx.h"

#include "PhysicalEntity.h"

typedef pe_params *(*ConvertToNativeParametersFunc)(PhysicsParameters *);
typedef void(*DisposeParametersFunc)(PhysicsParameters *);
typedef void(*ConvertToMonoParametersFunc)(pe_params *, PhysicsParameters *);

template<typename ParamsType>
pe_params *ConvertToParams(PhysicsParameters *parameters)
{
	return reinterpret_cast<ParamsType *>(parameters)->ToParams();
}
template<typename ParamsType>
void DisposeParams(PhysicsParameters *parameters)
{
	reinterpret_cast<ParamsType *>(parameters)->Dispose();
}
template<typename ParamsType>
void ConvertParamsToMono(pe_params *pars, PhysicsParameters *parameters)
{
	reinterpret_cast<ParamsType *>(parameters)->FromParams(pars);
}

ConvertToNativeParametersFunc paramConverters[EPE_Params::ePE_Params_Count] =
{
};
DisposeParametersFunc paramDisposers[EPE_Params::ePE_Params_Count] =
{
};
ConvertToMonoParametersFunc paramConvertersToMono[EPE_Params::ePE_Params_Count] =
{
};

void PhysicalEntityInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetParams);
	REGISTER_METHOD(GetParams);
}

int PhysicalEntityInterop::SetParams(IPhysicalEntity *handle, PhysicsParameters *parameters, bool threadSafe)
{
	// Convert CryCIL object to CryEngine one.
	auto params = paramConverters[parameters->type](parameters);
	int result = handle->SetParams(params, threadSafe);
	// Delete CryEngine object.
	delete params;
	// Dispose the CryCIL object.
	paramDisposers[parameters->type](parameters);
	return result;
}

int PhysicalEntityInterop::GetParams(IPhysicalEntity *handle, PhysicsParameters *parameters)
{
	// Convert CryCIL object to CryEngine one.
	auto params = paramConverters[parameters->type](parameters);
	int result = handle->GetParams(params);
	// Store result in the CryCIL object.
	paramConvertersToMono[parameters->type](params, parameters);
	// Delete CryEngine object.
	delete params;
	return result;
}
