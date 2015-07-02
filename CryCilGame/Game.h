#pragma once

#include "IMonoInterface.h"

static const char* GAME_NAME = "GameZero";
static const char* GAME_LONGNAME = "CRYENGINE SDK Game Example";
static const char* GAME_GUID = "{00000000-1111-2222-3333-444444444444}";

class CryCilGame : public IGame
{
public:
	

	virtual bool Init(IGameFramework *pFramework) override;

	virtual void InitEditor(IGameToEditorInterface* pGameToEditor) override;

	virtual void GetMemoryStatistics(ICrySizer * s) override;

	virtual bool CompleteInit() override;

	virtual void Shutdown() override;

	virtual void PrePhysicsUpdate() override;

	virtual int Update(bool haveFocus, unsigned int updateFlags) override;

	virtual void EditorResetGame(bool bStart) override;

	virtual void PlayerIdSet(EntityId playerId) override;

	virtual const char * GetLongName() override;

	virtual const char * GetName() override;

	virtual void LoadActionMaps(const char* filename) override;

	virtual void OnClearPlayerIds() override;

	virtual IGame::TSaveGameName CreateSaveGameName() override;

	virtual IGameFramework * GetIGameFramework() override;

	virtual const char* GetMappedLevelName(const char *levelName) const override;

	virtual IAntiCheatManager * GetAntiCheatManager() override;

	virtual const bool DoInitialSavegame() const override;

	virtual uint32 AddGameWarning(const char* stringId, const char* paramMessage, IGameWarningsListener* pListener = NULL) override;

	virtual void OnRenderScene(const SRenderingPassInfo &passInfo) override;

	virtual void RenderGameWarnings() override;

	virtual void RemoveGameWarning(const char* stringId) override;

	virtual bool GameEndLevel(const char* stringId) override;

	virtual void SetUserProfileChanged(bool yesNo) override;

	virtual IGameStateRecorder* CreateGameStateRecorder(IGameplayListener* pL) override;

	virtual void FullSerialize(TSerialize ser) override;

	virtual void PostSerialize() override;

	virtual IGame::ExportFilesInfo ExportLevelData(const char* levelName, const char* missionName) const override;

	virtual void LoadExportedLevelData(const char* levelName, const char* missionName) override;

	virtual void RegisterGameFlowNodes() override;

	virtual IGamePhysicsSettings* GetIGamePhysicsSettings() override;

	virtual void* GetGameInterface() override;

};