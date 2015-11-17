#pragma once

#include "IMonoInterface.h"

class CFaceIdentifierHandle;
struct IFacialEffCtrl;
struct IFacialEffector;
enum EFacialEffectorType;
enum EFacialEffectorParam;

struct FacialEffectorInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialEffector"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static void             SetIdentifier(IFacialEffector *handle, CFaceIdentifierHandle ident);
	static CFaceIdentifierHandle GetIdentifier(IFacialEffector *handle);
	static EFacialEffectorType   GetEffectorType(IFacialEffector *handle);
	static void             SetFlags(IFacialEffector *handle, uint32 nFlags);
	static uint32           GetFlags(IFacialEffector *handle);
	static int              GetIndexInState(IFacialEffector *handle);
	static void             SetParamString(IFacialEffector *handle, EFacialEffectorParam param, mono::string str);
	static mono::string     GetParamString(IFacialEffector *handle, EFacialEffectorParam param);
	static void             SetParamVec3(IFacialEffector *handle, EFacialEffectorParam param, Vec3 vValue);
	static Vec3             GetParamVec3(IFacialEffector *handle, EFacialEffectorParam param);
	static void             SetParamInt(IFacialEffector *handle, EFacialEffectorParam param, int nValue);
	static int              GetParamInt(IFacialEffector *handle, EFacialEffectorParam param);
	static int              GetSubEffectorCount(IFacialEffector *handle);
	static IFacialEffector *GetSubEffector(IFacialEffector *handle, int nIndex);
	static IFacialEffCtrl  *GetSubEffCtrl(IFacialEffector *handle, int nIndex);
	static IFacialEffCtrl  *GetSubEffCtrlByName(IFacialEffector *handle, mono::string effectorName);
	static IFacialEffCtrl  *AddSubEffector(IFacialEffector *handle, IFacialEffector *pEffector);
	static void             RemoveSubEffector(IFacialEffector *handle, IFacialEffector *pEffector);
	static void             RemoveAllSubEffectors(IFacialEffector *handle);
};