#pragma once

#include "IMonoInterface.h"

class CFaceIdentifierHandle;
struct IFacialAnimChannel;
struct IFacialEffector;

struct FacialAnimationChannelInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialAnimationChannel"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static void                  SetIdentifier(IFacialAnimChannel *handle, CFaceIdentifierHandle ident);
	static CFaceIdentifierHandle GetIdentifier(IFacialAnimChannel *handle);
	static void                  SetEffectorIdentifier(IFacialAnimChannel *handle, CFaceIdentifierHandle ident);
	static CFaceIdentifierHandle GetEffectorIdentifier(IFacialAnimChannel *handle);
	static void                  SetParent(IFacialAnimChannel *handle, IFacialAnimChannel *pParent);
	static IFacialAnimChannel   *GetParent(IFacialAnimChannel *handle);
	static void                  SetFlags(IFacialAnimChannel *handle, uint32 nFlags);
	static uint32                GetFlags(IFacialAnimChannel *handle);
	static void                  SetEffector(IFacialAnimChannel *handle, IFacialEffector *pEffector);
	static IFacialEffector      *GetEffector(IFacialAnimChannel *handle);
	static ISplineInterpolator  *GetInterpolator(IFacialAnimChannel *handle, int i);
	static ISplineInterpolator  *GetLastInterpolator(IFacialAnimChannel *handle);
	static void                  AddInterpolator(IFacialAnimChannel *handle);
	static void                  DeleteInterpolator(IFacialAnimChannel *handle, int i);
	static int                   GetInterpolatorCount(IFacialAnimChannel *handle);
	static void                  CleanupKeysInternal(IFacialAnimChannel *handle, float fErrorMax);
	static void                  SmoothKeysInternal(IFacialAnimChannel *handle, float sigma);
	static void                  RemoveNoiseInternal(IFacialAnimChannel *handle, float sigma, float threshold);
};