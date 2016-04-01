#pragma once

#include "IMonoInterface.h"

class CFaceIdentifierHandle;
struct IFacialEffectorsLibrary;
enum EFacialEffectorType;
struct IFacialEffector;

struct FacialEffectorsLibraryInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "FacialEffectorsLibrary"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void InitializeInterops() override;

	static void             AddRef(IFacialEffectorsLibrary *handle);
	static void             Release(IFacialEffectorsLibrary *handle);
	static void             SetName(IFacialEffectorsLibrary *handle, mono::string name);
	static mono::string     GetName(IFacialEffectorsLibrary *handle);
	static IFacialEffector *FindInternal(IFacialEffectorsLibrary *handle, CFaceIdentifierHandle ident);
	static IFacialEffector *GetRoot(IFacialEffectorsLibrary *handle);
	static void             VisitEffectors(IFacialEffectorsLibrary *handle, mono::delegat pVisitor);
	static IFacialEffector *CreateEffector(IFacialEffectorsLibrary *handle, EFacialEffectorType nType, CFaceIdentifierHandle ident);
	static void             RemoveEffector(IFacialEffectorsLibrary *handle, IFacialEffector *pEffector);
	static void             MergeLibrary(IFacialEffectorsLibrary *handle, IFacialEffectorsLibrary *pMergeLibrary, bool overwrite);
	static void             Serialize(IFacialEffectorsLibrary *handle, IXmlNode *xmlNodeHandle, bool bLoading);
};