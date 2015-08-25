#include "stdafx.h"

#include "PhysicalEntity.h"

typedef pe_params *(*ConvertToNativeParametersFunc)(PhysicsParameters *);
typedef void(*DisposeParametersFunc)(PhysicsParameters *);
typedef void(*ConvertToMonoParametersFunc)(pe_params *, PhysicsParameters *);

typedef pe_action *(*ConvertToNativeActionFunc)(PhysicsAction *);
typedef void(*DisposeActionFunc)(PhysicsAction *);

typedef pe_status *(*ConvertToNativeStatusFunc)(PhysicsStatus *);
typedef void(*ConvertToMonoStatusFunc)(pe_status *, PhysicsStatus *);

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

template<typename ActionType>
pe_action *ActionToCE(PhysicsAction *action)
{
	return reinterpret_cast<ActionType *>(action)->ToAction();
}
template<typename ActionType>
void DisposeAction(PhysicsAction *action)
{
	reinterpret_cast<ActionType *>(action)->Dispose();
}

template<typename StatusType>
pe_status *StatusToCE(PhysicsStatus *status)
{
	return reinterpret_cast<StatusType *>(status)->ToStatus();
}
template<typename StatusType>
void StatusToMono(pe_status *stat, PhysicsStatus *status)
{
	reinterpret_cast<StatusType *>(status)->FromStatus(stat);
}

#define START_PROCESSING_FUNC_DECLARATION(name, functionPtrType, functionPtr, typeCount) \
functionPtrType name(int type)\
{\
	static functionPtrType funcs[typeCount];\
	static bool initialized = false;\
	\
	if (!initialized)\
		{\
		memset(funcs, 0, sizeof(functionPtrType) * typeCount);

#define END_PROCESSING_FUNC_DECLARATION(typeCount) \
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

#define DECLARE_PARAMS_PROCESSING_FUNC(name, functionPtrType, functionPtr, typeCount) \
START_PROCESSING_FUNC_DECLARATION(name, functionPtrType, functionPtr, typeCount) \
		funcs[ePE_params_pos]                         = functionPtr<PhysicsParametersLocation>;\
		funcs[ePE_params_bbox]                        = functionPtr<PhysicsParametersBoundingBox>;\
		funcs[ePE_params_outer_entity]                = functionPtr<PhysicsParametersOuterEntity>;\
		funcs[ePE_params_sensors]                     = functionPtr<PhysicsParametersSensors>;\
		funcs[ePE_simulation_params]                  = functionPtr<PhysicsParametersSimulation>;\
		funcs[ePE_params_part]                        = functionPtr<PhysicsParametersPart>;\
		funcs[ePE_params_foreign_data]                = functionPtr<PhysicsParametersForeignData>;\
		funcs[ePE_params_buoyancy]                    = functionPtr<PhysicsParametersBuoyancy>;\
		funcs[ePE_params_flags]                       = functionPtr<PhysicsParametersFlags>;\
		funcs[ePE_params_collision_class]             = functionPtr<PhysicsParametersCollisionClass>;\
		funcs[ePE_params_structural_joint]            = functionPtr<PhysicsParametersStructuralJoint>;\
		funcs[ePE_params_structural_initial_velocity] = functionPtr<PhysicsParametersStructuralInitialVelocity>;\
		funcs[ePE_params_timeout]                     = functionPtr<PhysicsParametersTimeout>;\
		funcs[ePE_params_skeleton]                    = functionPtr<PhysicsParametersSkeleton>;\
		funcs[ePE_params_joint]                       = functionPtr<PhysicsParametersJoint>;\
		funcs[ePE_params_articulated_body]            = functionPtr<PhysicsParametersArticulatedBody>;\
		funcs[ePE_player_dimensions]                  = functionPtr<PhysicsParametersDimensions>;\
		funcs[ePE_player_dynamics]                    = functionPtr<PhysicsParametersDynamics>;\
		funcs[ePE_params_particle]                    = functionPtr<PhysicsParametersParticle>;\
		funcs[ePE_params_car]                         = functionPtr<PhysicsParametersVehicle>;\
		funcs[ePE_params_wheel]                       = functionPtr<PhysicsParametersWheel>;\
		funcs[ePE_params_rope]                        = functionPtr<PhysicsParametersRope>;\
		funcs[ePE_params_softbody]                    = functionPtr<PhysicsParametersSoftBody>;\
		funcs[ePE_params_area]                        = functionPtr<PhysicsParametersArea>;\
END_PROCESSING_FUNC_DECLARATION(typeCount)

#define DECLARE_ACTION_PROCESSING_FUNC(name, functionPtrType, functionPtr, typeCount) \
START_PROCESSING_FUNC_DECLARATION(name, functionPtrType, functionPtr, typeCount) \
		funcs[ePE_action_impulse]              = functionPtr<PhysicsActionImpulse>;\
		funcs[ePE_action_reset]                = functionPtr<PhysicsActionReset>;\
		funcs[ePE_action_add_constraint]       = functionPtr<PhysicsActionAddConstraint>;\
		funcs[ePE_action_update_constraint]    = functionPtr<PhysicsActionUpdateConstraint>;\
		funcs[ePE_action_register_coll_event]  = functionPtr<PhysicsActionRegisterCollisionEvent>;\
		funcs[ePE_action_awake]                = functionPtr<PhysicsActionAwake>;\
		funcs[ePE_action_remove_all_parts]     = functionPtr<PhysicsActionRemoveAllParts>;\
		funcs[ePE_action_reset_part_mtx]       = functionPtr<PhysicsActionResetPartMatrix>;\
		funcs[ePE_action_set_velocity]         = functionPtr<PhysicsActionSetVelocity>;\
		funcs[ePE_action_auto_part_detachment] = functionPtr<PhysicsActionAutoPartDetachment>;\
		funcs[ePE_action_move_parts]           = functionPtr<PhysicsActionTransferParts>;\
		funcs[ePE_action_batch_parts_update]   = functionPtr<PhysicsActionBatchPartsUpdate>;\
		funcs[ePE_action_slice]                = functionPtr<PhysicsActionSlice>;\
		funcs[ePE_action_move]                 = functionPtr<PhysicsActionMove>;\
END_PROCESSING_FUNC_DECLARATION(typeCount)

#define DECLARE_STATUS_PROCESSING_FUNC(name, functionPtrType, functionPtr, typeCount) \
START_PROCESSING_FUNC_DECLARATION(name, functionPtrType, functionPtr, typeCount) \
		funcs[ePE_status_pos]          = functionPtr<PhysicsStatusLocation>;\
END_PROCESSING_FUNC_DECLARATION(typeCount)

DECLARE_PARAMS_PROCESSING_FUNC(GetParamConverterToCE,    ConvertToNativeParametersFunc, ParamsToCE,    ePE_Params_Count)
DECLARE_PARAMS_PROCESSING_FUNC(GetParamDisposer,         DisposeParametersFunc,         DisposeParams, ePE_Params_Count)
DECLARE_PARAMS_PROCESSING_FUNC(GetParamConverterToMono,  ConvertToMonoParametersFunc,   ParamsToMono,  ePE_Params_Count)

DECLARE_ACTION_PROCESSING_FUNC(GetActionConverterToCE,   ConvertToNativeActionFunc,     ActionToCE,    ePE_Action_Count)
DECLARE_ACTION_PROCESSING_FUNC(GetActionDisposer,        DisposeActionFunc,             DisposeAction, ePE_Action_Count)

DECLARE_STATUS_PROCESSING_FUNC(GetStatusConverterToCE,   ConvertToNativeStatusFunc,     StatusToCE,    ePE_Status_Count)
DECLARE_STATUS_PROCESSING_FUNC(GetStatusConverterToMono, ConvertToMonoStatusFunc,       StatusToMono,  ePE_Status_Count)


void PhysicalEntityInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetParams);
	REGISTER_METHOD(GetParams);
	REGISTER_METHOD(GetStatusInternal);
	REGISTER_METHOD(Action);
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

int PhysicalEntityInterop::GetStatusInternal(IPhysicalEntity *handle, PhysicsStatus *status)
{
	auto converter = GetStatusConverterToCE(status->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto stat = converter(status);
	int result = handle->GetStatus(stat);
	// Store result in the CryCIL object.
	GetStatusConverterToMono(status->type)(stat, status);
	// Delete CryEngine object.
	delete stat;
	return result;
}

int PhysicalEntityInterop::Action(IPhysicalEntity *handle, PhysicsAction *action, bool threadSafe)
{
	auto converter = GetActionConverterToCE(action->type);
	if (!converter)
	{
		return 0;
	}

	// Convert CryCIL object to CryEngine one.
	auto act = converter(action);
	// Store result in the CryCIL object.
	int result = handle->Action(act, threadSafe);
	// Delete CryEngine object.
	delete act;
	return result;
}
