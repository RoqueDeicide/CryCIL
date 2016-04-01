#include "stdafx.h"

#include "AttachmentSimulationParameters.h"
#include <IAttachment.h>
#include "DynArrayToMonoArray.h"

void AttachmentSimulationParametersInterop::InitializeInterops()
{
	REGISTER_METHOD(GetProxyNames);
	REGISTER_METHOD(SetProxyNames);
}

mono::Array AttachmentSimulationParametersInterop::GetProxyNames(SimulationParams *handle)
{
	return ToMonoArray(handle->m_arrProxyNames);
}

void AttachmentSimulationParametersInterop::SetProxyNames(SimulationParams *handle, mono::Array names)
{
	OverwriteFromMonoArray(names, handle->m_arrProxyNames);
}
