#include "stdafx.h"

#include "CryEntitySubstitutionProxy.h"

void CryEntitySubstitutionProxyInterop::OnRunTimeInitialized()
{
	REGISTER_METHOD(SetSubstitute);
	REGISTER_METHOD(GetSubstitute);
}

void CryEntitySubstitutionProxyInterop::SetSubstitute(IEntitySubstitutionProxy *handle, IRenderNode *pSubstitute)
{
	handle->SetSubstitute(pSubstitute);
}

IRenderNode *CryEntitySubstitutionProxyInterop::GetSubstitute(IEntitySubstitutionProxy *handle)
{
	return handle->GetSubstitute();
}
