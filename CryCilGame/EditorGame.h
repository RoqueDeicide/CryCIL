#pragma once

#include "IMonoInterface.h"

#include <IEditorGame.h>

class CryCilEditorGame : public IEditorGame
{


	virtual bool Init(ISystem *pSystem, IGameToEditorInterface *pEditorInterface) override;

	virtual int Update(bool haveFocus, unsigned int updateFlags) override;

	virtual void Shutdown() override;

	virtual bool SetGameMode(bool bGameMode) override;

	virtual IEntity * GetPlayer() override;

	virtual void SetPlayerPosAng(Vec3 pos, Vec3 viewDir) override;

	virtual void HidePlayer(bool bHide) override;

	virtual void OnBeforeLevelLoad() override;

	virtual void OnAfterLevelInit(const char *levelName, const char *levelFolder) override;

	virtual void OnAfterLevelLoad(const char *levelName, const char *levelFolder) override;

	virtual void OnCloseLevel() override;

	virtual void OnSaveLevel() override;

	virtual bool BuildEntitySerializationList(XmlNodeRef output) override;

	virtual bool GetAdditionalMinimapData(XmlNodeRef output) override;

	virtual IFlowSystem * GetIFlowSystem() override;

	virtual IGameTokenSystem* GetIGameTokenSystem() override;

	virtual IEquipmentSystemInterface* GetIEquipmentSystemInterface() override;

	virtual void RegisterTelemetryTimelineRenderers(Telemetry::ITelemetryRepository* pRepository) override;

	virtual void OnDisplayRenderUpdated(bool displayHelpers) override;

};