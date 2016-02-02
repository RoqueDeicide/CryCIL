#pragma once

#include "IMonoInterface.h"

#include <IEditorGame.h>

class CryCilEditorGame : public IEditorGame
{
	IGameRef game;
	IGameStartup *gameStartup;
	bool enabled;
	bool gameMode;
	bool player;
	bool saturateScreen;
public:
	CryCilEditorGame();
	virtual ~CryCilEditorGame();

	virtual bool Init(ISystem *system, IGameToEditorInterface *editorInterface) override;
	virtual int Update(bool haveFocus, unsigned int updateFlags) override;
	virtual void Shutdown() override;
	virtual bool SetGameMode(bool gameMode) override;
	virtual IEntity *GetPlayer() override;
	virtual void SetPlayerPosAng(Vec3 pos, Vec3 viewDir) override;
	virtual void HidePlayer(bool hide) override;
	virtual void OnBeforeLevelLoad() override;

	virtual void OnAfterLevelInit(const char *, const char *) override {}

	virtual void OnAfterLevelLoad(const char *levelName, const char *levelFolder) override;

	virtual void OnCloseLevel() override {}
	virtual void OnSaveLevel() override {}
	virtual bool BuildEntitySerializationList(XmlNodeRef) override { return true; }
	virtual bool GetAdditionalMinimapData(XmlNodeRef) override { return false; }

	virtual IFlowSystem *GetIFlowSystem() override;
	virtual IGameTokenSystem *GetIGameTokenSystem() override;

	virtual IEquipmentSystemInterface *GetIEquipmentSystemInterface() override { return nullptr; }
	virtual void RegisterTelemetryTimelineRenderers(Telemetry::ITelemetryRepository *) override {}
	virtual void OnDisplayRenderUpdated(bool) override {}

private:
	bool ConfigureNetContext(bool on);
	void EnablePlayer(bool player);
	void SaturateScreen();
};