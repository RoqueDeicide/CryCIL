#pragma once

#include "IMonoInterface.h"

//! Handles general game logic.
class CryCilGame : public IGame
{
	IGameFramework *gameFramework;
	NtText          gameName;
	NtText          longGameName;

public:
	CryCilGame();
	~CryCilGame();

	//! Performs initialization that became possible after initialization of the game framework.
	//!
	//! @param pFramework A pointer to the object that represents the game framework that will be saved in this object.
	virtual bool Init(IGameFramework *pFramework) override;
	//! Does nothing.
	virtual void InitEditor(IGameToEditorInterface *) override {}
	//! Does nothing.
	virtual void GetMemoryStatistics(ICrySizer *) override {}
	//! Does nothing.
	virtual bool CompleteInit() override { return true; }
	//! Shuts down the game.
	virtual void Shutdown() override;
	//! Updates the state of this game.
	virtual int Update(bool haveFocus, unsigned int updateFlags) override;
	//! Does nothing.
	virtual void EditorResetGame(bool) override {}
	//! Does nothing.
	virtual void PlayerIdSet(EntityId) override {}
	//! Gets the long name of the game.
	virtual const char *GetLongName() override;
	//! Gets the short name of the game.
	virtual const char *GetName() override;
	//! Does nothing.
	virtual void LoadActionMaps(const char *) override {}
	//! Does nothing.
	virtual void OnClearPlayerIds() override {}
	//! Does nothing.
	virtual IGame::TSaveGameName CreateSaveGameName() override { return TSaveGameName(); }
	//! Gets the object that represents the game framework that was saved in this object.
	virtual IGameFramework *GetIGameFramework() override;
	//! Does nothing.
	virtual const char *GetMappedLevelName(const char *) const override { return ""; }

	// ReSharper disable once CppConstValueFunctionReturnType
	virtual const bool DoInitialSavegame() const override { return true; }
	//! Does nothing.
	virtual uint32 AddGameWarning(const char *, const char *, IGameWarningsListener *) override
	{
		return 0;
	}
	//! Does nothing.
	virtual void OnRenderScene(const SRenderingPassInfo &) override {}
	//! Does nothing.
	virtual void RenderGameWarnings() override {}
	//! Does nothing.
	virtual void RemoveGameWarning(const char *) override {}
	//! Does nothing.
	virtual bool GameEndLevel(const char *) override { return false; }
	//! Does nothing.
	virtual IGameStateRecorder *CreateGameStateRecorder(IGameplayListener *) override { return nullptr; }
	//! Does nothing.
	virtual void FullSerialize(TSerialize) override {}
	//! Does nothing.
	virtual void PostSerialize() override {}
	//! Does whatever.
	virtual IGame::ExportFilesInfo ExportLevelData(const char *levelName, const char *) const override { return IGame::ExportFilesInfo(levelName, 0); }
	//! Does nothing.
	virtual void LoadExportedLevelData(const char *, const char *) override {}
	//! Registers all flow nodes that are defined in CryCIL.
	virtual void RegisterGameFlowNodes() override;
	//! Does nothing.
	virtual IGamePhysicsSettings *GetIGamePhysicsSettings() override { return nullptr; }
	//! Does nothing.
	virtual void *GetGameInterface() override { return nullptr; }
};