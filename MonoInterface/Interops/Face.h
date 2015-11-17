#pragma once

#include "IMonoInterface.h"

struct IFacialInstance;
class CFacialAnimForcedRotationEntry;
struct IFacialModel;
struct IFaceState;
struct IFacialEffector;
enum EFacialSequenceLayer;
struct IFacialAnimSequence;

struct FaceInterop : IMonoInterop<true, true>
{
	virtual const char *GetInteropClassName() override { return "Face"; }
	virtual const char *GetInteropNameSpace() override { return "CryCil.Engine.Models.Characters.Faces"; }

	virtual void OnRunTimeInitialized() override;

	static IFacialModel *GetFacialModel(IFacialInstance *handle);
	static IFaceState   *GetFaceState(IFacialInstance *handle);
	static uint          StartEffectorChannel(IFacialInstance *handle, IFacialEffector *pEffector, float fWeight, float fFadeTime, float fLifeTime, int nRepeatCount);
	static void          StopEffectorChannel(IFacialInstance *handle, uint nChannelID, float fFadeOutTime);
	static IFacialAnimSequence *LoadSequence(IFacialInstance *handle, mono::string sSequenceName, bool addToCache);
	static void PrecacheFacialExpression(IFacialInstance *handle, mono::string sSequenceName);
	static void PlaySequence(IFacialInstance *handle, IFacialAnimSequence *pSequence, EFacialSequenceLayer layer, bool bExclusive, bool bLooping);
	static void StopSequence(IFacialInstance *handle, EFacialSequenceLayer layer);
	static bool IsPlaySequence(IFacialInstance *handle, IFacialAnimSequence *pSequence, EFacialSequenceLayer layer);
	static void PauseSequence(IFacialInstance *handle, EFacialSequenceLayer layer, bool bPaused);
	static void SeekSequence(IFacialInstance *handle, EFacialSequenceLayer layer, float fTime);
	static void LipSyncWithSound(IFacialInstance *handle, uint nSoundId, bool bStop);
	static void EnableProceduralFacialAnimation(IFacialInstance *handle, bool bEnable);
	static bool IsProceduralFacialAnimationEnabled(IFacialInstance *handle);
	static void SetForcedRotations(IFacialInstance *handle, int numForcedRotations, CFacialAnimForcedRotationEntry* forcedRotations);
	static void SetMasterCharacter(IFacialInstance *handle, ICharacterInstance *pMasterInstance);
	static void StopAllSequencesAndChannels(IFacialInstance *handle);
};