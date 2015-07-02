#pragma once

#include "IMonoInterface.h"

static const char* GAME_NAME = "GameZero";
static const char* GAME_LONGNAME = "CRYENGINE SDK Game Example";
static const char* GAME_GUID = "{00000000-1111-2222-3333-444444444444}";

//! Handles general game logic.
class CryCilGame : public IGame
{
	IGameFramework *gameFramework;
public:
	CryCilGame();
	~CryCilGame();
	//! Performs initialization that became possible after initialization of the game framework.
	//!
	//! @param pFramework A pointer to the object that represents the game framework that will be saved in this object.
	virtual bool Init(IGameFramework *pFramework) override;
	//! Does nothing.
	virtual void InitEditor(IGameToEditorInterface* pGameToEditor) override;
	//! Does nothing.
	virtual void GetMemoryStatistics(ICrySizer * s) override;
	//! Does nothing.
	virtual bool CompleteInit() override;
	//! Shuts down the game.
	virtual void Shutdown() override;
	//! Updates the state of this game.
	virtual int Update(bool haveFocus, unsigned int updateFlags) override;
	//! Does nothing.
	virtual void EditorResetGame(bool bStart) override;
	//! Does nothing.
	virtual void PlayerIdSet(EntityId playerId) override;
	//! Gets the long name of the game.
	virtual const char *GetLongName() override;
	//! Gets the short name of the game.
	virtual const char *GetName() override;
	//! Does nothing.
	virtual void LoadActionMaps(const char *filename) override;
	//! Does nothing.
	virtual void OnClearPlayerIds() override;
	//! Does nothing.
	virtual IGame::TSaveGameName CreateSaveGameName() override;
	//! Gets the object that represents the game framework that was saved in this object.
	virtual IGameFramework *GetIGameFramework() override;
	//! Does nothing.
	virtual const char *GetMappedLevelName(const char *levelName) const override;
	//! Does nothing.
	virtual IAntiCheatManager *GetAntiCheatManager() override;
	//! Does nothing.
	virtual const bool DoInitialSavegame() const override;
	//! Does nothing.
	virtual uint32 AddGameWarning(const char *stringId, const char *paramMessage, IGameWarningsListener *pListener = NULL) override;
	//! Does nothing.
	virtual void OnRenderScene(const SRenderingPassInfo &passInfo) override;
	//! Does nothing.
	virtual void RenderGameWarnings() override;
	//! Does nothing.
	virtual void RemoveGameWarning(const char *stringId) override;
	//! Does nothing.
	virtual bool GameEndLevel(const char *stringId) override;
	//! Does nothing.
	virtual IGameStateRecorder *CreateGameStateRecorder(IGameplayListener *pL) override;
	//! Does nothing.
	virtual void FullSerialize(TSerialize ser) override;
	//! Does nothing.
	virtual void PostSerialize() override;
	//! Does whatever.
	virtual IGame::ExportFilesInfo ExportLevelData(const char* levelName, const char* missionName) const override;
	//! Does nothing.
	virtual void LoadExportedLevelData(const char* levelName, const char* missionName) override;
	//! Registers all flow nodes that are defined in CryCIL.
	virtual void RegisterGameFlowNodes() override;
	//! Does nothing.
	virtual IGamePhysicsSettings *GetIGamePhysicsSettings() override;
	//! Does nothing.
	virtual void *GetGameInterface() override;

};