#include "stdafx.h"

#include "AttachmentRowSimulationParameters.h"
#include <IAttachment.h>
#include "DynArrayToMonoArray.h"

void AttachmentRowSimulationParametersInterop::InitializeInterops()
{
	REGISTER_METHOD(GetProxyNames);
	REGISTER_METHOD(SetProxyNames);
}

mono::Array AttachmentRowSimulationParametersInterop::GetProxyNames(SimulationParams *handle)
{
	return ToMonoArray(handle->m_arrProxyNames);
}

void AttachmentRowSimulationParametersInterop::SetProxyNames(SimulationParams *handle, mono::Array names)
{
	OverwriteFromMonoArray(names, handle->m_arrProxyNames);
}
