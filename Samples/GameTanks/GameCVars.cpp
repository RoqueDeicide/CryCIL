/*************************************************************************
  Crytek Source File.
  Copyright (C), Crytek Studios, 2001-2004.
 -------------------------------------------------------------------------
  $Id$
  $DateTime$
  
 -------------------------------------------------------------------------
  History:
  - 11:8:2004   10:50 : Created by Marcio Martins

*************************************************************************/
#include "StdAfx.h"
#include "GameCVars.h"
#include "GameRules.h"

#include <INetwork.h>
#include <IGameObject.h>
#include <IActorSystem.h>
#include <IItemSystem.h>
#include "NetInputChainDebug.h"
#include "INetworkService.h"

#include <IPathfinder.h>

#include <IVehicleSystem.h>

#define DEFAULT_CATALOG_TOKEN									"5A0D4D94-A35A-422A-9CA4-B14D9E7C47CA"

static void BroadcastChangeSafeMode( ICVar * )
{
	SGameObjectEvent event(eCGE_ResetMovementController, eGOEF_ToExtensions);
	IEntitySystem * pES = gEnv->pEntitySystem;
	IEntityItPtr pIt = pES->GetEntityIterator();
	while (!pIt->IsEnd())
	{
		if (IEntity * pEnt = pIt->Next())
			if (IActor * pActor = g_pGame->GetIGameFramework()->GetIActorSystem()->GetActor(pEnt->GetId()))
				pActor->HandleEvent( event );
	}
}

void CmdLogFPS( IConsoleCmdArgs* cmdArgs )
{
	CryLogAlways("FPS: %f", gEnv->pTimer->GetFrameRate());
}

void CmdLogFrameTime( IConsoleCmdArgs* cmdArgs )
{
	CryLogAlways("Game frametime: %f", gEnv->pTimer->GetFrameTime(ITimer::ETIMER_GAME));
	CryLogAlways("UI frametime: %f", gEnv->pTimer->GetFrameTime(ITimer::ETIMER_UI));
	CryLogAlways("Real frametime: %f", gEnv->pTimer->GetRealFrameTime());
}

void CmdLogDrawCalls( IConsoleCmdArgs* cmdArgs )
{
	CryLogAlways("Drawcalls: %i", gEnv->pRenderer->GetCurrentNumberOfDrawCalls());
}

void CmdLogMemory( IConsoleCmdArgs* cmdArgs )
{
	CryLogAlways("Used memory: %i", gEnv->pSystem->GetUsedMemory());
}

void CmdLogVisRegPos( IConsoleCmdArgs* cmdArgs )
{
	if(!cmdArgs)
		return;

	const int argCount = cmdArgs->GetArgCount();
	int logData = 0;
	if(argCount > 1 )
	{		
		for(int i = 1; i < argCount; ++i)
		{
			if( strcmp(cmdArgs->GetArg(i), "player") == 0 )
				logData |= 1;

			if( strcmp(cmdArgs->GetArg(i), "camera") == 0 )
				logData |= 2;
		}
	}
	else
	{
		logData = 3;
	}

	IGame*			pGame			= gEnv ? gEnv->pGame : NULL;
	IGameFramework*	pGameFramework	= pGame ? pGame->GetIGameFramework() : NULL;
	string levelName( pGameFramework ? pGameFramework->GetLevelName() : "");

	if(logData & 1)
	{		
		IViewSystem*	pViewSystem		= pGameFramework ? pGameFramework->GetIViewSystem() : NULL;
		IView*			pView			= pViewSystem ? pViewSystem->GetActiveView() : NULL;
		IEntity*		pEntity			= pView && gEnv->pEntitySystem ? gEnv->pEntitySystem->GetEntity(pView->GetLinkedId()) : NULL;
		if(pEntity)
		{
			const Vec3&	pos = pEntity->GetPos();
			const Ang3	rot = RAD2DEG(Ang3::GetAnglesXYZ( pEntity->GetRotation() ));
			CryLogAlways("VisRegInfo: %s [player] <Position location=\"%f %f %f %f %f %f\" />", 
				levelName.c_str(),
				pos.x, pos.y, pos.z, rot.x, rot.y, rot.z);
		}		
	}
	if(logData & 2)
	{
		const CCamera&	rCam	= gEnv->pRenderer->GetCamera();
		const Vec3&		pos		= rCam.GetPosition();
		const Ang3		rot		= RAD2DEG(Ang3::GetAnglesXYZ( rCam.GetMatrix() ));				
		CryLogAlways("VisRegInfo: %s [camera] <Position location=\"%f %f %f %f %f %f\" />", 
			levelName.c_str(),
			pos.x, pos.y, pos.z, rot.x, rot.y, rot.z);
	}
}

void SCVars::InitAIPerceptionCVars(IConsole *pConsole)
{
	REGISTER_CVAR(ai_perception.movement_useSurfaceType, 0, VF_CHEAT, "Toggle if surface type should be used to get the base radius instead of cvars");
	REGISTER_CVAR(ai_perception.movement_movingSurfaceDefault, 1.0f, VF_CHEAT, "Default value for movement speed effect on footstep radius (overridden by surface type)");
	REGISTER_CVAR(ai_perception.movement_standingRadiusDefault, 4.0f, VF_CHEAT, "Default value for standing footstep sound radius (overridden by surface type)");
	REGISTER_CVAR(ai_perception.movement_crouchRadiusDefault, 2.0f, VF_CHEAT, "Default value for crouching footstep sound radius multiplier (overridden by surface type)");
	REGISTER_CVAR(ai_perception.movement_standingMovingMultiplier, 2.5f, VF_CHEAT, "Multiplier for standing movement speed effect on footstep sound radius");
	REGISTER_CVAR(ai_perception.movement_crouchMovingMultiplier, 2.0f, VF_CHEAT, "Multiplier for crouched movement speed effect on footstep sound radius");
}

// game related cvars must start with an g_
// game server related cvars must start with sv_
// game client related cvars must start with cl_
// no other types of cvars are allowed to be defined here!
void SCVars::InitCVars(IConsole *pConsole)
{
	g_messageOfTheDay=REGISTER_STRING("g_messageOfTheDay","",VF_INVISIBLE, " message of the day");
	g_serverImageUrl=REGISTER_STRING("g_serverImageUrl","",VF_INVISIBLE, "server image");

	REGISTER_CVAR(g_useOnlineLobbyService, 0, 0, "Use Online Lobby Service (rather than LAN)");

	REGISTER_CVAR(g_ProcessOnlineCallbacks,1,VF_CHEAT|VF_READONLY,"Process online callbacks in the gamelobbymanager.");
	REGISTER_CVAR(gl_maxSessionNameTimeWithoutConnection, 5.f, 0, "Time a session name is allowed to persist before being removed");
	REGISTER_CVAR(g_maxGameBrowserResults, 32767, VF_NULL, "Maximum number of servers returned in game browser");

	REGISTER_CVAR(g_MatchmakingVersion, 0, VF_REQUIRE_APP_RESTART, "Defines your matchmaking version (Only join games over the same version)");
	REGISTER_CVAR(g_MatchmakingBlock, 0, VF_REQUIRE_APP_RESTART, "Used to shift matchmaking version for QA and EA builds - please leave as zero");

#if !defined(_RELEASE)
	net_onlyListGameServersContainingText = REGISTER_STRING("net_onlyListGameServersContainingText", "", 0, "Server list will only display host names containing this text");
	net_nat_type = REGISTER_STRING("net_nat_type", "Uninitialized", VF_READONLY, "The current NAT Type set.");
	REGISTER_CVAR(net_initLobbyServiceToLan, 0, VF_CHEAT, "Always initialize lobby in LAN mode (NB: mode can still be changed afterwards!)");
#endif

	REGISTER_CVAR(gl_skip, 0, 0, "Skips the game lobby");
	REGISTER_CVAR(gl_votingCloseTimeBeforeStart, 5.0f, 0, "Playlist voting will close this many secs before game start countdown finishes");
	REGISTER_CVAR(gl_experimentalPlaylistRotationAdvance, 1, 0, "Testing code that gets the first level from the playlist and advances it when the server enters the lobby. Here so can be easily disabled! Currently needed for gl_enablePlaylistVoting");
	REGISTER_CVAR(gl_enablePlaylistVoting, 1, 0, "Enabled voting on which level to play next within playlists that support it. Currently also needs gl_experimentalPlaylistRotationAdvance");
	REGISTER_CVAR(gl_minPlaylistSizeToEnableVoting, 2, 0, "A playlist must contain at least this many levels for voting to be enabled, even when gl_enablePlaylistVoting is set. Note that setting it to 1 and having a playlist with 1 level in it will still work, but it'll present a vote with the same level as both voting candidates");
	REGISTER_CVAR(gl_enableOfflinePlaylistVoting, 0, VF_CHEAT, "Enabled voting on offline games, just to make testing easier. Requires gl_enablePlaylistVoting to also be set");
	REGISTER_CVAR(gl_enableOfflineCountdown, 0, VF_CHEAT, "Enabled the 'Game starts in X secs...' countdown in offline games, just to make testing of voting easier");
	REGISTER_CVAR(gl_time, 45, 0, "Time in lobby between games");
	REGISTER_CVAR(gl_checkDLCBeforeStartTime, 20, 0, "When the server game start countdown gets to this time between games it will do DLC checks to make sure it has the DLC needed for both the levels being voted on");
	REGISTER_CVAR(gl_maxSessionNameTimeWithoutConnection, 5.f, 0, "Time a session name is allowed to persist before being removed");

	REGISTER_CVAR(g_autoAssignTeams, 1, 0, "1 = auto assign teams, 0 = players choose teams");

	REGISTER_CVAR(g_hostMigrationResumeTime, 3.0f, VF_CHEAT, "Time after players have rejoined before the game resumes");
	REGISTER_CVAR(g_hostMigrationUseAutoLobbyMigrateInPrivateGames, 0, VF_CHEAT, "1=Make calls to EnsureBestHost when in private games");

	//client cvars
	REGISTER_CVAR(cl_fov, 60.0f, VF_NULL, "field of view.");
	REGISTER_CVAR(cl_bob, 1.0f, VF_NULL, "view/weapon bobbing multiplier");
	REGISTER_CVAR(cl_headBob, 1.0f, VF_NULL, "head bobbing multiplier");
	REGISTER_CVAR(cl_headBobLimit, 0.06f, VF_NULL, "head bobbing distance limit");
	REGISTER_CVAR(cl_tpvDist, 3.5f, VF_NULL, "camera distance in 3rd person view");
	REGISTER_CVAR(cl_tpvYaw, 0, VF_NULL, "camera angle offset in 3rd person view");
	REGISTER_CVAR(cl_nearPlane, 0, VF_NULL, "overrides the near clipping plane if != 0, just for testing.");
	REGISTER_CVAR(cl_sprintShake, 0.0f, VF_NULL, "sprint shake");
	REGISTER_CVAR(cl_sensitivityZeroG, 70.0f, VF_DUMPTODISK, "Set mouse sensitivity in ZeroG!");
	REGISTER_CVAR(cl_sensitivity, 45.0f, VF_DUMPTODISK, "Set mouse sensitivity!");
	REGISTER_CVAR(cl_invertMouse, 0, VF_DUMPTODISK, "mouse invert?");
	REGISTER_CVAR(cl_invertController, 0, VF_DUMPTODISK, "Controller Look Up-Down invert");
	REGISTER_CVAR(cl_crouchToggle, 1, VF_DUMPTODISK, "To make the crouch key work as a toggle");
	REGISTER_CVAR(cl_fpBody, 2, VF_NULL, "first person body");
	REGISTER_CVAR(cl_player_landing_forcefeedback, 0, VF_NULL, "Play forcefeedback effect during player landing");
	

	//FIXME:just for testing
	REGISTER_CVAR(cl_strengthscale, 1.0f, VF_NULL, "nanosuit strength scale");

	REGISTER_CVAR(cl_sprintToggle, 0, VF_DUMPTODISK, "To make the sprint key work as a toggle");
	REGISTER_CVAR(cl_sprintTime, 0, VF_DUMPTODISK, "How long can the player sprint");
	REGISTER_CVAR(cl_sprintRestingTime, 0, VF_DUMPTODISK, "How long needs the player to rest until he can sprint again");

	REGISTER_CVAR(cl_camModify, 0, VF_NULL, "Enable camera modifiers.");
	REGISTER_CVAR(cl_camKeepX, 1.0f, VF_NULL, "Fraction of camera x-axis rotation to keep.");
	REGISTER_CVAR(cl_camKeepY, 1.0f, VF_NULL, "Fraction of camera y-axis rotation to keep.");
	REGISTER_CVAR(cl_camKeepZ, 1.0f, VF_NULL, "Fraction of camera z-axis rotation to keep.");
	REGISTER_CVAR(cl_camOffsetX, 0.0f, VF_NULL, "Degrees to offset camera around x-axis.");
	REGISTER_CVAR(cl_camOffsetY, 0.0f, VF_NULL, "Degrees to offset camera around y-axis.");
	REGISTER_CVAR(cl_camOffsetZ, 0.0f, VF_NULL, "Degrees to offset camera around z-axis.");
	REGISTER_CVAR(cl_camRotateX, 0.0f, VF_NULL, "Degrees to rotate camera per second around x-axis.");
	REGISTER_CVAR(cl_camRotateY, 0.0f, VF_NULL, "Degrees to rotate camera per second around y-axis.");
	REGISTER_CVAR(cl_camRotateZ, 0.0f, VF_NULL, "Degrees to rotate camera per second around z-axis.");
	REGISTER_CVAR(cl_camTranslateX, 0.0f, VF_NULL, "Translate along camera local x-axis.");
	REGISTER_CVAR(cl_camTranslateY, 0.0f, VF_NULL, "Translate along camera local y-axis.");
	REGISTER_CVAR(cl_camTranslateZ, 0.0f, VF_NULL, "Translate along camera local z-axis.");

	//bitmap ui
	REGISTER_CVAR(g_showBitmapUi, 1, VF_NULL, "Show or hide the bitmap user interface.");
	REGISTER_CVAR(g_show_fullscreen_info, 0, VF_NULL, "show fullscreen info (and which one to show)");
	REGISTER_CVAR(g_show_crosshair, 1, VF_NULL, "show crosshair");
	REGISTER_CVAR(g_show_crosshair_tp, 1, VF_NULL, "keep crosshair in third person");

	// 3d hud
	REGISTER_CVAR(g_hud3d_cameraDistance, 1.f, VF_NULL, "3D Hud distance to camera");
	REGISTER_CVAR(g_hud3d_cameraOffsetZ, 0.f, VF_NULL, "3D Hud z pos offset");
	REGISTER_CVAR(g_hud3D_cameraOverride, 0, VF_NULL, "if true g_hud3d_cameraDistance and g_hud3d_cameraOffsetZ can be used to change offsets, otherwise auto offsets");

	REGISTER_CVAR(g_showHud, 1, VF_NULL, "show/hide hud");

	//watch
	REGISTER_CVAR(watch_enabled, 1, 0, "On-screen watch text is enabled/disabled");
	REGISTER_CVAR(watch_text_render_start_pos_x, 35.0f, 0, "On-screen watch text render start x position");
	REGISTER_CVAR(watch_text_render_start_pos_y, 180.0f, 0, "On-screen watch text render start y position");
	REGISTER_CVAR(watch_text_render_size, 1.75f, 0, "On-screen watch text render size");
	REGISTER_CVAR(watch_text_render_lineSpacing, 9.3f, 0, "On-screen watch text line spacing (to cram more text on screen without shrinking the font)");
	REGISTER_CVAR(watch_text_render_fxscale, 13.0f, 0, "Draw2d label to IFFont x scale value (for calcing sizes)." );

	//autotest
	REGISTER_CVAR(autotest_enabled, 0, 0, "1 = enabled autotesting, 2 = enabled autotesting with no output results written.");

	autotest_state_setup = REGISTER_STRING("autotest_state_setup", "", 0, "setup string for autotesting");
	REGISTER_CVAR(autotest_quit_when_done, 0, 0, "quit the game when tests are done");

	//designer warning
	REGISTER_CVAR(designer_warning_enabled, 1, 0, "designer warnings are enabled");
	REGISTER_CVAR(designer_warning_level_resources, 0, 0, "Designer warnings about resource load during run-time");

	REGISTER_CVAR(g_enableSlimCheckpoints, 0, 0, "Enable the use of console style checkpoints instead of full save.");

	// the following CVars are for the free Third Person Cam
	//////////////////////////////////////////////////////////////////////////
	REGISTER_CVAR(cl_cam_orbit, 0, VF_DUMPTODISK,"In Third Person mode this will enable orbit camera\n"
		"see cl_cam_orbit_offset how to setup offset for this camera\n"
		"see cl_cam_orbit_slide and cl_cam_orbit_slidespeed how to setup slide for object clipping");
	REGISTER_CVAR(cl_cam_orbit_slide, 1, VF_DUMPTODISK,"Enable camera slide (Orbit mode).");
	REGISTER_CVAR(cl_cam_orbit_slidespeed, 1.f, VF_DUMPTODISK,"Camera slide speed (Orbit mode).");
	REGISTER_CVAR(cl_cam_orbit_offsetX, 0, VF_DUMPTODISK,"X Offset of orbit camera.");
	REGISTER_CVAR(cl_cam_orbit_offsetZ, 1.f, VF_DUMPTODISK,"Z Offset of orbit camera.");
	REGISTER_CVAR(cl_cam_orbit_distance, 5.f, VF_DUMPTODISK,"Distance of orbit camera.");
	REGISTER_CVAR(cl_cam_debug, 0, VF_DUMPTODISK,"Camera system debug output.");

	pConsole->Register("cl_enable_tree_transparency", &cl_enable_tree_transparency, 1, VF_DUMPTODISK, "Switches tree transparency on/off.");
	pConsole->Register("cl_fake_first_person",&cl_fake_first_person,0,VF_DUMPTODISK,"Enable fake first person view in new new camera control system (cl_cam_orbit 1).");
	pConsole->Register("cl_cam_clipping_offset_distance", &cl_cam_clipping_offset_distance, 1.5f, VF_DUMPTODISK, "Distance at which the camera starts offsetting horizontally to prevent clipping through the character/target.");
	pConsole->Register("cl_cam_min_distance", &cl_cam_min_distance, 0.01f, VF_DUMPTODISK, "Minimum distance for debugging.");
	pConsole->Register("cl_cam_yaw_input_inertia", &cl_cam_yaw_input_inertia, 0.2f, VF_DUMPTODISK, "Time delay until the camera fully stops after moving right stick in nav mode.");
	pConsole->Register("cl_cam_pitch_input_inertia", &cl_cam_pitch_input_inertia, 0.1f, VF_DUMPTODISK, "Time delay until the camera fully stops after moving right stick in nav mode.");

	// camera rotation
	pConsole->Register("cl_cam_rotation_speed", &cl_cam_rotation_speed, 0.054f, VF_DUMPTODISK, "Camera rotation speed.");
	pConsole->Register("cl_cam_rotation_enable_acceleration", &cl_cam_rotation_enable_acceleration, 1, VF_DUMPTODISK, "Camera rotation is accelerating, not jumping on / off.");
	pConsole->Register("cl_cam_rotation_acceleration_time_yaw", &cl_cam_rotation_acceleration_time_yaw, 0.5f, VF_DUMPTODISK, "Camera yaw acceleration time.");
	pConsole->Register("cl_cam_rotation_acceleration_time_pitch", &cl_cam_rotation_acceleration_time_pitch, 0.4f, VF_DUMPTODISK, "Camera pitch acceleration time.");
	pConsole->Register("cl_cam_mouseYawScale", &cl_cam_mouseYawScale, 0.35f, VF_DUMPTODISK, "Camera yaw dampening.");
	pConsole->Register("cl_cam_mousePitchScale", &cl_cam_mousePitchScale, 0.75f, VF_DUMPTODISK, "Camera pitch dampening.");
	
	// camera tracking
	pConsole->Register("cl_cam_tracking",&cl_cam_tracking,1,VF_DUMPTODISK,"Camera tracking enable/disable.");
	pConsole->Register("cl_cam_tracking_allow_pitch",&cl_cam_tracking_allow_pitch,0,VF_DUMPTODISK,"Camera tracking changes also vertical movement.");
	pConsole->Register("cl_cam_tracking_rotation_speed", &cl_cam_tracking_rotation_speed, 0.60f, 0, "Speed multiplicator, adjusting rotation speed.");
	pConsole->Register("cl_cam_tracking_volume", &cl_cam_tracking_volume, 5000.0f, 0, "By default cam tracking ignores trees, but when they are bigger than this volume, they get tracked around.");

	// lazy cam
	pConsole->Register("cl_cam_enable_lazy_cam", &cl_cam_enable_lazy_cam, 1, VF_DUMPTODISK, "Follow camera is 'lazy' and doesn't keep player in screen center.");
	pConsole->Register("cl_cam_lazy_cam_distance_XY", &cl_cam_lazy_cam_distance_XY, 0.5f, VF_DUMPTODISK, "Camera movement distance threshold in lazy cam mode. Bigger value => more lazy.");
	pConsole->Register("cl_cam_lazy_cam_distance_Z", &cl_cam_lazy_cam_distance_Z, 0.25f, VF_DUMPTODISK, "Camera movement distance threshold in lazy cam mode. Bigger value => more lazy.");
	pConsole->Register("cl_cam_lazy_cam_time_XY", &cl_cam_lazy_cam_time_XY, 0.2f, VF_DUMPTODISK, "Camera movement time in lazy cam mode. Bigger value => more lazy.");
	pConsole->Register("cl_cam_lazy_cam_time_Z", &cl_cam_lazy_cam_time_Z, 0.5f, VF_DUMPTODISK, "Camera movement time in lazy cam mode. Bigger value => more lazy.");

	// auto-follow
	pConsole->Register("cl_cam_auto_follow_rate", &cl_cam_auto_follow_rate, 0.2f, 0, "Scale the speed at which the camera auto-rotates in player direction (0.0f is off).");
	pConsole->Register("cl_cam_auto_follow_threshold", &cl_cam_auto_follow_threshold, gf_PI*0.3f, 0, "Camera auto-rotate threshold angle (how close it rotates to hero direction).");
	pConsole->Register("cl_cam_auto_follow_movement_speed", &cl_cam_auto_follow_movement_speed, 0.3f, 0, "Camera auto-rotate reacts on player movement on/off.");

	// View damping and lookahead
	pConsole->Register("cl_tpvDeltaDist",&cl_tpvDeltaDist,0,VF_DUMPTODISK,"Affects how far the cam can lag the player");
	pConsole->Register("cl_tpvDeltaVel",&cl_tpvDeltaVel,12,VF_DUMPTODISK,"Affects how far the cam can lag the player");
	pConsole->Register("cl_tpvDeltaDistNav",&cl_tpvDeltaDistNav,2.55f,VF_DUMPTODISK,"Nav mode: Affects how far the cam can lag the player");
	pConsole->Register("cl_tpvDeltaVelNav",&cl_tpvDeltaVelNav,12,VF_DUMPTODISK,"Nav mode: Affects how far the cam can lag the player");
	pConsole->Register("cl_tpvPosDamping",&cl_tpvPosDamping,15,VF_DUMPTODISK,"Camera position damping: 0..N 0 = camera responds immediately.");
	pConsole->Register("cl_tpvAngDamping",&cl_tpvAngDamping,0.001f,VF_DUMPTODISK,"Camera angle damping: 0..N 0 = camera responds immediately.");
	pConsole->Register("cl_tpvPosDampingNav",&cl_tpvPosDampingNav,15,VF_DUMPTODISK,"Nav mode: Camera position damping: 0..N 0 = camera responds immediately.");
	pConsole->Register("cl_tpvAngDampingNav",&cl_tpvAngDampingNav,0.01f,VF_DUMPTODISK,"Camera angle damping: 0..N 0 = camera responds immediately.");
	pConsole->Register("cl_tpvFOVDamping",&cl_tpvFOVDamping,0.0f,VF_DUMPTODISK,"Camera FOV damping");

	// nav
	pConsole->Register("cl_joy_nav_useHforV",					&cl_joy_nav_useHforV,						 0,	VF_DUMPTODISK,	"non zero => use the H values for V as well");
	pConsole->Register("cl_joy_nav_speedV",						&cl_joy_nav_speedV,						1.4f, VF_DUMPTODISK,	"scale the Vert turn speed in nav mode");
	pConsole->Register("cl_joy_nav_sensitivityV",			&cl_joy_nav_sensitivityV,			1.4f, VF_DUMPTODISK,	"the stick centre to edge Vert turn sensitivity in nav mode");
	pConsole->Register("cl_joy_nav_maxV",							&cl_joy_nav_maxV,							2.0f, VF_DUMPTODISK,	"max Vert turn speed in nav mode");
	pConsole->Register("cl_joy_nav_speedH",						&cl_joy_nav_speedH,					 2.05f,	VF_DUMPTODISK,	"scale the Horz turn speed in nav mode");
	pConsole->Register("cl_joy_nav_sensitivityH",			&cl_joy_nav_sensitivityH,		 1.4f,	VF_DUMPTODISK,	"the stick centre to edge Horz turn sensitivity in nav mode");
	pConsole->Register("cl_joy_nav_maxH",							&cl_joy_nav_maxH,						 2.05f,	VF_DUMPTODISK,	"max Horz turn speed in nav mode");
	pConsole->Register("cl_cam_keyLeftRightMove_SmoothingTime",	&cl_cam_keyLeftRightMove_SmoothingTime,	2.0f,	VF_DUMPTODISK,	"Time to interpolate the keyboard turning to full");

	//old cam cvars
	pConsole->Register("cl_cam_PitchMax", &cl_cam_PitchMax, 2.18f,VF_DUMPTODISK,"Max camera pitch");
	pConsole->Register("cl_cam_PitchMin", &cl_cam_PitchMin, 0.35f,VF_DUMPTODISK,"Min camera pitch");

	pConsole->UnregisterVariable("cl_tpvHOffCombatBias", true);
	pConsole->UnregisterVariable("cl_tpvVOffCombatBias", true);
	pConsole->UnregisterVariable("cl_tpvNavSwitchVCentreEnabled", true);
	pConsole->UnregisterVariable("cl_tpvNavSwitchVCentreValue", true);

	// end Third Person Cam
	//////////////////////////////////////////////////////////////////////////

	REGISTER_CVAR(dt_enable, 0, VF_NULL, "suit actions activated by double-tapping");
	REGISTER_CVAR(dt_time, 0.25f, VF_NULL, "time in seconds between double taps");
	REGISTER_CVAR(dt_meleeTime, 0.3f, VF_NULL, "time in seconds between double taps for melee");

	REGISTER_CVAR(i_staticfiresounds, 1, VF_DUMPTODISK, "Enable/Disable static fire sounds. Static sounds are not unloaded when idle.");
	REGISTER_CVAR(i_soundeffects,	1, VF_DUMPTODISK, "Enable/Disable playing item sound effects.");
	REGISTER_CVAR(i_lighteffects, 1, VF_DUMPTODISK, "Enable/Disable lights spawned during item effects.");
	REGISTER_CVAR(i_particleeffects,	1, VF_DUMPTODISK, "Enable/Disable particles spawned during item effects.");
	REGISTER_CVAR(i_rejecteffects, 1, VF_DUMPTODISK, "Enable/Disable ammo reject effects during weapon firing.");
	REGISTER_CVAR(i_offset_front, 0.0f, VF_NULL, "Item position front offset");
	REGISTER_CVAR(i_offset_up, 0.0f, VF_NULL, "Item position up offset");
	REGISTER_CVAR(i_offset_right, 0.0f, VF_NULL, "Item position right offset");
	REGISTER_CVAR(i_unlimitedammo, 0, VF_CHEAT, "unlimited ammo");
	REGISTER_CVAR(i_iceeffects, 0, VF_CHEAT, "Enable/Disable specific weapon effects for ice environments");

	// marcok TODO: seem to be only used on script side ... 
	REGISTER_FLOAT("cl_motionBlur", 0, VF_NULL, "motion blur type (0=off, 1=accumulation-based, 2=velocity-based)");
	REGISTER_FLOAT("cl_sprintBlur", 0.6f, VF_NULL, "sprint blur");
	REGISTER_FLOAT("cl_hitShake", 1.25f, VF_NULL, "hit shake");
	REGISTER_FLOAT("cl_hitBlur", 0.25f, VF_NULL, "blur on hit");

	REGISTER_INT("cl_righthand", 1, VF_NULL, "Select right-handed weapon!");
	REGISTER_INT("cl_screeneffects", 1, VF_NULL, "Enable player screen effects (depth-of-field, motion blur, ...).");
	
	REGISTER_CVAR(cl_debugSwimming, 0, VF_CHEAT, "enable swimming debugging");

	ca_GameControlledStrafingPtr = pConsole->GetCVar("ca_GameControlledStrafing");
	REGISTER_CVAR(pl_curvingSlowdownSpeedScale, 0.5f, VF_CHEAT, "Player only slowdown speedscale when curving/leaning extremely.");
	REGISTER_CVAR(ac_enableProceduralLeaning, 1.0f, VF_CHEAT, "Enable procedural leaning (disabled asset leaning and curving slowdown).");

	REGISTER_CVAR(cl_shallowWaterSpeedMulPlayer, 0.6f, VF_CHEAT, "shallow water speed multiplier (Players only)");
	REGISTER_CVAR(cl_shallowWaterSpeedMulAI, 0.8f, VF_CHEAT, "Shallow water speed multiplier (AI only)");
	REGISTER_CVAR(cl_shallowWaterDepthLo, 0.3f, VF_CHEAT, "Shallow water depth low (below has zero slowdown)");
	REGISTER_CVAR(cl_shallowWaterDepthHi, 1.0f, VF_CHEAT, "Shallow water depth high (above has full slowdown)");

	REGISTER_INT("g_grabLog", 0, VF_NULL, "verbosity for grab logging (0-2)");

	REGISTER_CVAR(pl_inputAccel, 30.0f, VF_NULL, "Movement input acceleration");

	REGISTER_INT("cl_actorsafemode", 0, VF_CHEAT, "Enable/disable actor safe mode")->SetOnChangeCallback(BroadcastChangeSafeMode);
	REGISTER_CVAR(g_enableSpeedLean, 0, VF_NULL, "Enables player-controlled curve leaning in speed mode.");
	//
	REGISTER_CVAR(int_zoomAmount, 0.75f, VF_CHEAT, "Maximum zoom, between 0.0 and 1.0. Default = .75");
	REGISTER_CVAR(int_zoomInTime, 5.0f, VF_CHEAT, "Number of seconds it takes to zoom in. Default = 5.0");
	REGISTER_CVAR(int_moveZoomTime, 0.1f, VF_CHEAT, "Number of seconds it takes to zoom out when moving. Default = 0.2");
	REGISTER_CVAR(int_zoomOutTime, 0.1f, VF_CHEAT, "Number of seconds it takes to zoom out when you stop firing. Default = 0.5");

	REGISTER_FLOAT("aa_maxDist", 10.0f, VF_CHEAT, "max lock distance");

	REGISTER_CVAR(hr_rotateFactor, -.1f, VF_CHEAT, "rotate factor");
	REGISTER_CVAR(hr_rotateTime, .07f, VF_CHEAT, "rotate time");
	REGISTER_CVAR(hr_dotAngle, .75f, VF_CHEAT, "max angle for FOV change");
	REGISTER_CVAR(hr_fovAmt, .03f, VF_CHEAT, "goal FOV when hit");
	REGISTER_CVAR(hr_fovTime, .05f, VF_CHEAT, "fov time");

	// animation triggered footsteps
	REGISTER_CVAR(g_footstepSoundsFollowEntity, 1, VF_CHEAT, "Toggles moving of footsteps sounds with it's entity.");
	REGISTER_CVAR(g_footstepSoundsDebug, 0, VF_CHEAT, "Toggles debug messages of footstep sounds.");
	REGISTER_CVAR(g_footstepSoundMaxDistanceSq, 900.0f, 0, "Maximum squared distance for footstep sounds / fx spawned by AI Actors.");

	// frozen shake vars (for tweaking only)
	REGISTER_CVAR(cl_debugFreezeShake, 0, VF_CHEAT|VF_DUMPTODISK, "Toggle freeze shake debug draw");  
	REGISTER_CVAR(cl_frozenSteps, 3, VF_CHEAT, "Number of steps for unfreeze shaking");  
	REGISTER_CVAR(cl_frozenSensMin, 1.0f, VF_CHEAT, "Frozen sensitivity min"); // was 0.2
	REGISTER_CVAR(cl_frozenSensMax, 1.0f, VF_CHEAT, "Frozen sensitivity max"); // was 0.4
	REGISTER_CVAR(cl_frozenAngleMin, 1.f, VF_CHEAT, "Frozen clamp angle min");
	REGISTER_CVAR(cl_frozenAngleMax, 10.f, VF_CHEAT, "Frozen clamp angle max");
	REGISTER_CVAR(cl_frozenMouseMult, 0.00015f, VF_CHEAT, "Frozen mouseshake multiplier");
  REGISTER_CVAR(cl_frozenKeyMult, 0.02f, VF_CHEAT, "Frozen movement keys multiplier");
	REGISTER_CVAR(cl_frozenSoundDelta, 0.004f, VF_CHEAT, "Threshold for unfreeze shake to trigger a crack sound");
	
	REGISTER_CVAR(g_frostDecay, 0.25f, VF_CHEAT, "Frost decay speed when freezing actors");

	REGISTER_CVAR(g_stanceTransitionSpeed, 15.0f, VF_CHEAT, "Set speed of camera transition from stance to stance");

	REGISTER_CVAR(g_playerHealthValue, 100, VF_CHEAT, "Maximum player health.");
	REGISTER_CVAR(g_walkMultiplier, 1, VF_CHEAT, "Modify movement speed");
	
	REGISTER_CVAR(g_difficultyLevel, 0, VF_CHEAT|VF_READONLY, "Difficulty level");
	REGISTER_CVAR(g_difficultyHintSystem, 2, VF_CHEAT|VF_READONLY, "Lower difficulty hint system (0 is off, 1 is radius based, 2 is save-game based)");
	REGISTER_CVAR(g_difficultyRadius, 300, VF_CHEAT|VF_READONLY, "Radius in which player needs to die to display lower difficulty level hint.");
	REGISTER_CVAR(g_difficultyRadiusThreshold, 5, VF_CHEAT|VF_READONLY, "Number of times player has to die within radius to trigger difficulty hint.");
	REGISTER_CVAR(g_difficultySaveThreshold, 5, VF_CHEAT|VF_READONLY, "Number of times player has to die with same savegame active to trigger difficulty hint.");
	REGISTER_CVAR(g_pp_scale_income, 1, VF_CHEAT, "Scales incoming PP.");
	REGISTER_CVAR(g_pp_scale_price, 1, VF_CHEAT, "Scales PP prices.");
	REGISTER_CVAR(g_energy_scale_price, 0, VF_CHEAT, "Scales energy prices.");
	REGISTER_CVAR(g_energy_scale_income, 1, VF_CHEAT, "Scales incoming energy.");
	REGISTER_CVAR(g_enableFriendlyFallAndPlay, 0, VF_NULL, "Enables fall&play feedback for friendly actors.");
	
	REGISTER_CVAR(g_playerRespawns, 0, VF_CHEAT, "Sets the player lives.");
	REGISTER_CVAR(g_playerLowHealthThreshold, 40.0f, VF_CHEAT, "The player health threshold when the low health effect kicks in.");
	REGISTER_CVAR(g_punishFriendlyDeaths, 1, VF_CHEAT, "The player gets punished by death when killing a friendly unit.");
	REGISTER_CVAR(g_enableMPStealthOMeter, 0, VF_CHEAT, "Enables the stealth-o-meter to detect enemies in MP matches.");
	REGISTER_CVAR(g_meleeWhileSprinting, 0, VF_NULL, "Enables option to melee while sprinting, using left mouse button.");
	REGISTER_CVAR(g_fallAndPlayThreshold, 50, VF_CHEAT, "Minimum damage for fall and play.");
	REGISTER_CVAR(g_inventoryNoLimits, 0, VF_CHEAT, "Disables inventory limiations, player can carry as many weapons as he wants.");

	// Depth of Field control
	REGISTER_CVAR(g_dofset_minScale, 1.0f, VF_CHEAT, "Scale Dof_FocusMin param when it gets set Default = 1");
	REGISTER_CVAR(g_dofset_maxScale, 3.0f, VF_CHEAT, "Scale Dof_FocusMax param when it gets set Default = 3");
	REGISTER_CVAR(g_dofset_limitScale, 9.0f, VF_CHEAT, "Scale Dof_FocusLimit param when it gets set Default = 9");
	REGISTER_CVAR(g_dof_minHitScale, 0.25f, VF_CHEAT, "Scale of ray hit distance which Min tries to approach. Default = 0.25");
	REGISTER_CVAR(g_dof_maxHitScale, 2.0f, VF_CHEAT, "Scale of ray hit distance which Max tries to approach. Default = 2.0f");
	REGISTER_CVAR(g_dof_sampleAngle, 5.0f, VF_CHEAT, "Sample angle in degrees. Default = 5");
	REGISTER_CVAR(g_dof_minAdjustSpeed, 100.0f, VF_CHEAT, "Speed that DoF can adjust the min value with. Default = 100");
	REGISTER_CVAR(g_dof_maxAdjustSpeed, 200.0f, VF_CHEAT, "Speed that DoF can adjust the max value with. Default = 200");
	REGISTER_CVAR(g_dof_averageAdjustSpeed, 20.0f, VF_CHEAT, "Speed that the average between min and max can be approached. Default = 20");
	REGISTER_CVAR(g_dof_distAppart, 10.0f, VF_CHEAT, "Minimum distance that max and min can be apart. Default = 10");
	REGISTER_CVAR(g_dof_ironsight, 1, VF_CHEAT, "Enable ironsight dof. Default = 1");
	REGISTER_CVAR(g_ColorGradingBlendTime, 3.0f, VF_NULL, "Time to blend from the last color grading chart to the next.");
	
#if USE_CRYLOBBY_GAMESPY
	// GameSpy credentials
	REGISTER_CVAR(g_gamespy_loginUI, 1, 0, "Whether to use the gamespy login UI, if set to 0 cfg file values are used.");
	g_gamespy_email = REGISTER_STRING( "g_gamespy_email", "", VF_INVISIBLE, "GameSpy email address" );
	g_gamespy_unique_nick = REGISTER_STRING( "g_gamespy_unique_nick", "", VF_INVISIBLE, "GameSpy unique nick" );
	g_gamespy_password = REGISTER_STRING( "g_gamespy_password", "", VF_INVISIBLE, "GameSpy password" );
	g_gamespy_cdkey = REGISTER_STRING( "g_gamespy_cdkey", "", VF_INVISIBLE, "GameSpy CD key" );
	g_gamespy_accountnumber = REGISTER_STRING( "g_gamespy_accountnumber", "", VF_INVISIBLE, "Account number for testing" );

	g_gamespy_catalog_version = REGISTER_INT( "g_gamespy_catalog_version", 1, VF_CHEAT, "GameSpy catalog version" );
#if !defined (_RELEASE)
	g_gamespy_catalog_tokenCVar = REGISTER_STRING( "g_gamespy_catalog_token", DEFAULT_CATALOG_TOKEN, VF_CHEAT, "GameSpy catalog token" );
	g_gamespy_catalog_regionCVar = REGISTER_STRING( "g_gamespy_catalog_region", "en", VF_CHEAT, "GameSpy catalog region" );
#else
	g_gamespy_catalog_tokenStr = DEFAULT_CATALOG_TOKEN;
	g_gamespy_catalog_regionStr = "en";
#endif
	g_gamespy_server_region = REGISTER_INT( "g_gamespy_server_region", 1, VF_NULL, "GameSpy server region" );

	REGISTER_CVAR(g_officialServer, 0, VF_NULL, "If a server is shown as official or not");
	REGISTER_CVAR(g_maxGameBrowserResults, 32767, VF_NULL, "Maximum number of servers returned in game browser");
#else
	REGISTER_CVAR(g_maxGameBrowserResults, 50, VF_NULL, "Maximum number of servers returned in game browser");
#endif

	// explosion culling
	REGISTER_CVAR(g_ec_enable, 1, VF_CHEAT, "Enable/Disable explosion culling of small objects. Default = 1");
	REGISTER_CVAR(g_ec_radiusScale, 2.0f, VF_CHEAT, "Explosion culling scale to apply to explosion radius for object query.");
	REGISTER_CVAR(g_ec_volume, 0.75f, VF_CHEAT, "Explosion culling volume which needs to be exceed for objects to not be culled.");
	REGISTER_CVAR(g_ec_extent, 2.0f, VF_CHEAT, "Explosion culling length of an AABB side which needs to be exceed for objects to not be culled.");
	REGISTER_CVAR(g_ec_removeThreshold, 20, VF_CHEAT, "At how many items in exploding area will it start removing items.");

	REGISTER_CVAR(g_radialBlur, 1.0f, VF_CHEAT, "Radial blur on explosions. Default = 1, 0 to disable");
	REGISTER_CVAR(g_playerFallAndPlay, 0, VF_NULL, "When enabled, the player doesn't die from direct damage, but goes to fall and play.");
	
	REGISTER_CVAR(g_enableTracers, 1, VF_NULL, "Enable/Disable tracers.");
	REGISTER_CVAR(g_enableAlternateIronSight, 0, VF_NULL, "Enable/Disable alternate ironsight mode");
	REGISTER_CVAR(g_ragdollMinTime, 10.0f, VF_NULL, "minimum time in seconds that a ragdoll will be visible");
	REGISTER_CVAR(g_ragdollUnseenTime, 2.0f, VF_NULL, "time in seconds that the player has to look away from the ragdoll before it disappears");
	REGISTER_CVAR(g_ragdollPollTime, 0.5f, VF_NULL, "time in seconds where 'unseen' polling is done");
	REGISTER_CVAR(g_ragdollDistance, 10.0f, VF_NULL, "distance in meters that the player has to be away from the ragdoll before it can disappear");
	REGISTER_CVAR(g_debugaimlook, 0, VF_CHEAT, "Debug aim/look direction");
	REGISTER_CVAR(g_enableIdleCheck, 1, VF_NULL,"");

	// Crysis supported gamemode CVars
	REGISTER_CVAR(g_timelimit, 60.0f, VF_NULL, "Duration of a time-limited game (in minutes). Default is 0, 0 means no time-limit.");
	REGISTER_CVAR(g_roundtime, 2.0f, VF_NULL, "Duration of a round (in minutes). Default is 0, 0 means no time-limit.");
	REGISTER_CVAR(g_preroundtime, 8, VF_NULL, "Frozen time before round starts. Default is 8, 0 Disables freeze time.");
	REGISTER_CVAR(g_suddendeathtime, 30, VF_NULL, "Number of seconds before round end to start sudden death. Default if 30. 0 Disables sudden death.");
	REGISTER_CVAR(g_roundlimit, 30, VF_NULL, "Maximum numbers of rounds to be played. Default is 0, 0 means no limit.");
	REGISTER_CVAR(g_fraglimit, 0, VF_NULL, "Number of frags before a round restarts. Default is 0, 0 means no frag-limit.");
  REGISTER_CVAR(g_fraglead, 1, VF_NULL, "Number of frags a player has to be ahead of other players once g_fraglimit is reached. Default is 1.");
  REGISTER_CVAR(g_friendlyfireratio, 1.0f, VF_NULL, "Sets friendly damage ratio.");
  REGISTER_CVAR(g_revivetime, 20, VF_NULL, "Revive wave timer.");
  REGISTER_CVAR(g_autoteambalance, 0, VF_NULL, "Enables auto team balance.");
	REGISTER_CVAR(g_minplayerlimit, 2, VF_NULL, "Minimum number of players to start a match.");
	REGISTER_CVAR(g_minteamlimit, 1, VF_NULL, "Minimum number of players in each team to start a match.");
	REGISTER_CVAR(g_tk_punish, 1, VF_NULL, "Turns on punishment for team kills");
	REGISTER_CVAR(g_tk_punish_limit, 5, VF_NULL, "Number of team kills user will be banned for");
	REGISTER_CVAR(g_teamlock, 2, VF_NULL, "Number of players one team needs to have over the other, for the game to deny joining it. 0 disables.");

	REGISTER_CVAR(g_debugNetPlayerInput, VF_NULL, VF_CHEAT, "Show some debug for player input");
	REGISTER_CVAR(g_debug_fscommand, 0, VF_NULL, "Print incoming fscommands to console");
	REGISTER_CVAR(g_debugDirectMPMenu, 0, VF_NULL, "Jump directly to MP menu on application start.");
	REGISTER_CVAR(g_skipIntro, 0, VF_CHEAT, "Skip all the intro videos.");
	REGISTER_CVAR(g_resetActionmapOnStart, 0, VF_NULL, "Resets Keyboard mapping on application start.");
	REGISTER_CVAR(g_useProfile, 1, VF_NULL, "Don't save anything to or load anything from profile.");
	REGISTER_CVAR(g_startFirstTime, 1, VF_DUMPTODISK, "1 before the game was started first time ever.");
	REGISTER_CVAR(g_cutsceneSkipDelay, 0.0f, VF_NULL, "Skip Delay for Cutscenes.");

	//
  REGISTER_CVAR(g_godMode, 0, VF_CHEAT, "God Mode");
  REGISTER_CVAR(g_detachCamera, 0, VF_NULL, "Detach camera");

  REGISTER_CVAR(g_debugCollisionDamage, 0, VF_DUMPTODISK, "Log collision damage");
	REGISTER_CVAR(g_debugHits, 0, VF_DUMPTODISK, "Log hits");
  
	REGISTER_CVAR(pl_debug_ladders, 0, VF_CHEAT,"");
	REGISTER_CVAR(pl_testGroundAlignOverride, 0, VF_CHEAT, "");
	REGISTER_CVAR(pl_ladder_animOffset, -0.54f, VF_CHEAT, "Additional animation/position offset");

	REGISTER_CVAR(pl_debug_movement, 0, VF_CHEAT,"");
	REGISTER_CVAR(pl_debug_jumping, 0, VF_CHEAT,"");
	pl_debug_filter = REGISTER_STRING("pl_debug_filter","",VF_CHEAT,"");
	
	// PLAYERPREDICTION
	REGISTER_CVAR(pl_velocityInterpAirControlScale, 1.0f, 0, "Use velocity based interpolation method with gravity adjustment");	
	REGISTER_CVAR(pl_velocityInterpSynchJump, 2, 0, "Velocity interp jump velocity synching");
	REGISTER_CVAR(pl_velocityInterpAirDeltaFactor, 0.75f, 0, "Interpolation air motion damping factor (0-1)");
	REGISTER_CVAR(pl_velocityInterpPathCorrection, 1.0f, 0, "Percentage of velocity to apply tangentally to the current velocity, used to reduce oscillation");
	REGISTER_CVAR(pl_velocityInterpAlwaysSnap, 0, 0, "Set to true to continually snap the remote player to the desired position, for debug usage only");
	REGISTER_CVAR(pl_debugInterpolation, 0, 0, "Debug interpolation");
	REGISTER_CVAR(pl_serialisePhysVel, 1, 0, "Serialise the physics vel rathe rthan the stick");
	REGISTER_CVAR(pl_clientInertia, 25.0f, 0, "Override the interia of clients");

	REGISTER_CVAR(pl_netAimLerpFactor, 0.5f, 0, "Factor to lerp the remote aim directions by");
	REGISTER_CVAR(pl_netSerialiseMaxSpeed, 4.0f, 0, "Maximum char speed, used by interpolation");
	REGISTER_CVAR(pl_playerErrorSnapDistSquare, 5.0f, 0, "Maximum distance between local and remote player pos to perform error snapping");
	
	// ~PLAYERPREDICTION

	REGISTER_CVAR(aln_debug_movement, 0, VF_CHEAT,"");
	aln_debug_filter = REGISTER_STRING("aln_debug_filter","",VF_CHEAT,"");

	// emp grenade
	REGISTER_CVAR(g_empStyle, 0, VF_CHEAT, "");	

	REGISTER_CVAR(pl_nightvisionModeBinocular, 4, 0, "Sets the default nightvision mode for binocular item");

	// Controller aim helper cvars
	REGISTER_CVAR(aim_assistSearchBox, 0.0f, VF_NULL, "The area autoaim looks for enemies within");
	REGISTER_CVAR(aim_assistMaxDistance, 0.0f, VF_NULL, "The maximum range at which autoaim operates");
	REGISTER_CVAR(aim_assistSnapDistance, 0.0f, VF_NULL, "The maximum deviation autoaim is willing to compensate for");
	REGISTER_CVAR(aim_assistVerticalScale, 0.0f, VF_NULL, "The amount of emphasis on vertical correction (the less the number is the more vertical component is compensated)");
	REGISTER_CVAR(aim_assistSingleCoeff, 0.0f, VF_NULL, "The scale of single-shot weapons' aim assistance");
	REGISTER_CVAR(aim_assistAutoCoeff, 0.0f, VF_NULL, "The scale of auto weapons' aim assistance at continuous fire");
	REGISTER_CVAR(aim_assistRestrictionTimeout, 0.0f, VF_NULL, "The restriction timeout on aim assistance after user uses a mouse");


	
	// Controller control
	REGISTER_CVAR(ctrl_aspectCorrection, 2, VF_NULL, "Aspect ratio corrections for controller rotation: 0-off, 1-direct, 2-inverse");
	REGISTER_CVAR(ctrl_Curve_X, 3.0f, VF_NULL, "Analog controller X rotation curve");
	REGISTER_CVAR(ctrl_Curve_Z, 3.0f, VF_NULL, "Analog controller Z rotation curve");
	REGISTER_CVAR(ctrl_Coeff_X, 3.5f*3.5f, VF_NULL, "Analog controller X rotation scale"); // was 3.5*3.5 but aspect ratio correction does the scaling now! adjust only if that gives no satisfactory results
	REGISTER_CVAR(ctrl_Coeff_Z, 5.0f*5.0f, VF_NULL, "Analog controller Z rotation scale");
	REGISTER_CVAR(ctrl_ZoomMode, 1, VF_NULL, "Weapon aiming mode with controller. 0 is same as mouse zoom, 1 cancels at release");

	REGISTER_CVAR(g_combatFadeTime, 17.0f, VF_NULL, "sets the battle fade time in seconds ");
	REGISTER_CVAR(g_combatFadeTimeDelay, 7.0f, VF_NULL, "waiting time before the battle starts fading out, in seconds ");
	REGISTER_CVAR(g_battleRange, 50.0f, VF_NULL, "sets the battle range in meters ");

	// Assistance switches
	REGISTER_CVAR(aim_assistAimEnabled, 0, VF_NULL, "Enable/disable aim assitance on aim zooming");
	REGISTER_CVAR(aim_assistTriggerEnabled, 0, VF_NULL, "Enable/disable aim assistance on firing the weapon");
	REGISTER_CVAR(hit_assistSingleplayerEnabled, 0, VF_NULL, "Enable/disable minimum damage hit assistance");
	REGISTER_CVAR(hit_assistMultiplayerEnabled, 0, VF_NULL, "Enable/disable minimum damage hit assistance in multiplayer games");

	//movement cvars
#if !defined(_RELEASE)
	REGISTER_CVAR(v_debugMovement, 0, VF_CHEAT, "Cheat mode, freeze the vehicle and activate debug movement");    
	REGISTER_CVAR(v_debugMovementMoveVertically, 0.f, VF_CHEAT, "Add this value to the vertical position of the vehicle");    
	REGISTER_CVAR(v_debugMovementX, 0.f, VF_CHEAT, "Add this rotation to the x axis");    
	REGISTER_CVAR(v_debugMovementY, 0.f, VF_CHEAT, "Add this rotation to the y axis");    
	REGISTER_CVAR(v_debugMovementZ, 0.f, VF_CHEAT, "Add this rotation to the z axis");    
	REGISTER_CVAR(v_debugMovementSensitivity, 30.f, VF_CHEAT, "if v_debugMovement is set you can rotate the vehicle, this controls the speed");    
#endif

  REGISTER_CVAR(v_profileMovement, 0, VF_NULL, "Used to enable profiling of the current vehicle movement (1 to enable)");    
  REGISTER_CVAR(v_pa_surface, 1, VF_CHEAT, "Enables/disables vehicle surface particles");
  REGISTER_CVAR(v_wind_minspeed, 0.f, VF_CHEAT, "If non-zero, vehicle wind areas always set wind >= specified value");
  REGISTER_CVAR(v_draw_suspension, 0, VF_DUMPTODISK, "Enables/disables display of wheel suspension, for the vehicle that has v_profileMovement enabled");
  REGISTER_CVAR(v_draw_slip, 0, VF_DUMPTODISK, "Draw wheel slip status");  
  REGISTER_CVAR(v_invertPitchControl, VF_NULL, VF_DUMPTODISK, "Invert the pitch control for driving some vehicles, including the helicopter and the vtol");
  REGISTER_CVAR(v_sprintSpeed, 0.f, VF_NULL, "Set speed for acceleration measuring");
  REGISTER_CVAR(v_rockBoats, 1, VF_NULL, "Enable/disable boats idle rocking");  
  REGISTER_CVAR(v_dumpFriction, 0, VF_NULL, "Dump vehicle friction status");
  REGISTER_CVAR(v_debugSounds, 0, VF_NULL, "Enable/disable vehicle sound debug drawing");
  REGISTER_CVAR(v_debugMountedWeapon, 0, VF_NULL, "Enable/disable vehicle mounted weapon camera debug draw");

	pAltitudeLimitCVar = REGISTER_CVAR(v_altitudeLimit, v_altitudeLimitDefault(), VF_CHEAT, "Used to restrict the helicopter and VTOL movement from going higher than a set altitude.\nIf set to zero, the altitude limit is disabled.");
	pAltitudeLimitLowerOffsetCVar = REGISTER_CVAR(v_altitudeLimitLowerOffset, 0.1f, VF_CHEAT, "Used in conjunction with v_altitudeLimit to set the zone when gaining altitude start to be more difficult.");
  REGISTER_CVAR(v_help_tank_steering, 0, VF_NULL, "Enable tank steering help for AI");

	REGISTER_CVAR(v_stabilizeVTOL, 0.35f, VF_DUMPTODISK, "Specifies if the air movements should automatically stabilize");

  	
	REGISTER_CVAR(pl_swimBaseSpeed, 4.0f, VF_CHEAT, "Swimming base speed.");
	REGISTER_CVAR(pl_swimBackSpeedMul, 0.8f, VF_CHEAT, "Swimming backwards speed mul.");
	REGISTER_CVAR(pl_swimSideSpeedMul, 0.9f, VF_CHEAT, "Swimming sideways speed mul.");
	REGISTER_CVAR(pl_swimVertSpeedMul, 0.5f, VF_CHEAT, "Swimming vertical speed mul.");
	REGISTER_CVAR(pl_swimNormalSprintSpeedMul, 1.5f, VF_CHEAT, "Swimming Non-Speed sprint speed mul.");
	REGISTER_CVAR(pl_swimSpeedSprintSpeedMul, 2.5f, VF_CHEAT, "Swimming Speed sprint speed mul.");
	REGISTER_CVAR(pl_swimUpSprintSpeedMul, 2.0f, VF_CHEAT, "Swimming sprint while looking up (dolphin rocket).");
	REGISTER_CVAR(pl_swimJumpStrengthCost, 50.0f, VF_CHEAT, "Swimming strength shift+jump energy cost (dolphin rocket).");
	REGISTER_CVAR(pl_swimJumpStrengthSprintMul, 2.5f, VF_CHEAT, "Swimming strength shift+jump velocity mul (dolphin rocket).");
	REGISTER_CVAR(pl_swimJumpStrengthBaseMul, 1.0f, VF_CHEAT, "Swimming strength normal jump velocity mul (dolphin rocket).");
	REGISTER_CVAR(pl_swimJumpSpeedCost, 50.0f, VF_CHEAT, "Swimming speed shift+jump energy cost (dolphin rocket).");
	REGISTER_CVAR(pl_swimJumpSpeedSprintMul, 2.5f, VF_CHEAT, "Swimming speed shift+jump velocity mul (dolphin rocket).");
	REGISTER_CVAR(pl_swimJumpSpeedBaseMul, 1.0f, VF_CHEAT, "Swimming speed normal jump velocity mul (dolphin rocket).");

	REGISTER_CVAR(pl_fallDamage_Normal_SpeedSafe, 8.0f, VF_CHEAT, "Safe fall speed (in all modes, including strength jump on flat ground).");
	REGISTER_CVAR(pl_fallDamage_Normal_SpeedFatal, 13.7f, VF_CHEAT, "Fatal fall speed in armor mode (13.5 m/s after falling freely for ca 20m).");
	REGISTER_CVAR(pl_fallDamage_SpeedBias, 1.5f, VF_CHEAT, "Damage bias for medium fall speed: =1 linear, <1 more damage, >1 less damage.");
	REGISTER_CVAR(pl_debugFallDamage, 0, VF_CHEAT, "Enables console output of fall damage information.");
	

	REGISTER_CVAR(pl_zeroGSpeedMultNormal, 1.2f, VF_CHEAT, "Modify movement speed in zeroG, in normal mode.");
	REGISTER_CVAR(pl_zeroGSpeedMultNormalSprint, 1.7f, VF_CHEAT, "Modify movement speed in zeroG, in normal sprint.");
  REGISTER_CVAR(pl_zeroGSpeedMultSpeed, 1.7f, VF_CHEAT, "Modify movement speed in zeroG, in speed mode.");
	REGISTER_CVAR(pl_zeroGSpeedMultSpeedSprint, 5.0f, VF_CHEAT, "Modify movement speed in zeroG, in speed sprint.");
	REGISTER_CVAR(pl_zeroGUpDown, 1.0f, VF_NULL, "Scales the z-axis movement speed in zeroG.");
	REGISTER_CVAR(pl_zeroGBaseSpeed, 3.0f, VF_NULL, "Maximum player speed request limit for zeroG.");
	REGISTER_CVAR(pl_zeroGSpeedMaxSpeed, -1.0f, VF_NULL, "(DEPRECATED) Maximum player speed request limit for zeroG while in speed mode.");
	REGISTER_CVAR(pl_zeroGSpeedModeEnergyConsumption, 0.5f, VF_NULL, "Percentage consumed per second while speed sprinting in ZeroG.");
	REGISTER_CVAR(pl_zeroGDashEnergyConsumption, 0.25f, VF_NULL, "Percentage consumed when doing a dash in ZeroG.");
	REGISTER_CVAR(pl_zeroGSwitchableGyro, 0, VF_NULL, "MERGE/REVERT");
	REGISTER_CVAR(pl_zeroGEnableGBoots, 0, VF_NULL, "Switch G-Boots action on/off (if button assigned).");
	REGISTER_CVAR(pl_zeroGThrusterResponsiveness, 0.3f, VF_CHEAT, "Thrusting responsiveness.");
	REGISTER_CVAR(pl_zeroGFloatDuration, 1.25f, VF_CHEAT, "Floating duration until full stop (after stopped thrusting).");
	REGISTER_CVAR(pl_zeroGParticleTrail, 0, VF_NULL, "Enable particle trail when in zerog.");
	REGISTER_CVAR(pl_zeroGEnableGyroFade, 2, VF_CHEAT, "Enable fadeout of gyro-stabilizer for vertical view angles (2=disable speed fade as well).");
	REGISTER_CVAR(pl_zeroGGyroFadeAngleInner, 20.0f, VF_CHEAT, "ZeroG gyro inner angle (default is 20).");
	REGISTER_CVAR(pl_zeroGGyroFadeAngleOuter, 60.0f, VF_CHEAT, "ZeroG gyro outer angle (default is 60).");
	REGISTER_CVAR(pl_zeroGGyroFadeExp, 2.0f, VF_CHEAT, "ZeroG gyro angle bias (default is 2.0).");
	REGISTER_CVAR(pl_zeroGGyroStrength, 1.0f, VF_CHEAT, "ZeroG gyro strength (default is 1.0).");
	REGISTER_CVAR(pl_zeroGAimResponsiveness, 8.0f, VF_CHEAT, "ZeroG aim responsiveness vs. inertia (default is 8.0).");
	REGISTER_CVAR(pl_DebugFootstepSounds, 0, VF_CHEAT, "Toggles debug messages of footstep sounds.");

	// weapon system
	i_debuggun_1 = REGISTER_STRING("i_debuggun_1", "ai_statsTarget", VF_DUMPTODISK, "Command to execute on primary DebugGun fire");
	i_debuggun_2 = REGISTER_STRING("i_debuggun_2", "ag_debug", VF_DUMPTODISK, "Command to execute on secondary DebugGun fire");

	REGISTER_CVAR(tracer_min_distance, 4.0f, VF_NULL, "Distance at which to start scaling/lengthening tracers.");
	REGISTER_CVAR(tracer_max_distance, 50.0f, VF_NULL, "Distance at which to stop scaling/lengthening tracers.");
	REGISTER_CVAR(tracer_min_scale, 0.5f, VF_NULL, "Scale at min distance.");
	REGISTER_CVAR(tracer_max_scale, 5.0f, VF_NULL, "Scale at max distance.");
	REGISTER_CVAR(tracer_max_count, 32, VF_NULL, "Max number of active tracers.");
	REGISTER_CVAR(tracer_player_radiusSqr, 400.0f, VF_NULL, "Sqr Distance around player at which to start decelerate/acelerate tracer speed.");

	REGISTER_CVAR(i_debug_projectiles, 0, VF_CHEAT, "Displays info about projectile status, where available.");
	REGISTER_CVAR(i_auto_turret_target, 1, VF_CHEAT, "Enables/Disables auto turrets aquiring targets.");
	REGISTER_CVAR(i_auto_turret_target_tacshells, 0, VF_NULL, "Enables/Disables auto turrets aquiring TAC shells as targets");

	REGISTER_CVAR(i_debug_zoom_mods, 0, VF_CHEAT, "Use zoom mode spread/recoil mods");
  REGISTER_CVAR(i_debug_sounds, 0, VF_CHEAT, "Enable item sound debugging");
  REGISTER_CVAR(i_debug_turrets, 0, VF_CHEAT, 
    "Enable GunTurret debugging.\n"
    "Values:\n"
    "0:  off"
    "1:  basics\n"
    "2:  prediction\n"
    "3:  sweeping\n"
    "4:  searching\n"      
    "5:  deviation\n"    
    );
	REGISTER_CVAR(i_debug_mp_flowgraph, 0, VF_CHEAT, "Displays info on the MP flowgraph node");
  
  // quick game
  g_quickGame_map = REGISTER_STRING("g_quickGame_map","",VF_DUMPTODISK, "QuickGame option");
  g_quickGame_mode = REGISTER_STRING("g_quickGame_mode","DeathMatch", VF_DUMPTODISK, "QuickGame option");
  REGISTER_CVAR(g_quickGame_min_players,0,VF_DUMPTODISK,"QuickGame option");
  REGISTER_CVAR(g_quickGame_prefer_lan,0,VF_DUMPTODISK,"QuickGame option");
  REGISTER_CVAR(g_quickGame_prefer_favorites,0,VF_DUMPTODISK,"QuickGame option");
  REGISTER_CVAR(g_quickGame_prefer_my_country,0,VF_DUMPTODISK,"QuickGame option");
  REGISTER_CVAR(g_quickGame_ping1_level,80,VF_DUMPTODISK,"QuickGame option");
  REGISTER_CVAR(g_quickGame_ping2_level,170,VF_DUMPTODISK,"QuickGame option");

	REGISTER_CVAR(g_quickGame_debug,0,VF_CHEAT,"QuickGame option");
	
	REGISTER_CVAR(g_displayIgnoreList,1,VF_DUMPTODISK,"Display ignore list in chat tab.");
  REGISTER_CVAR(g_buddyMessagesIngame,1,VF_DUMPTODISK,"Output incoming buddy messages in chat while playing game.");

	REGISTER_CVAR(g_showPlayerState,0,VF_DUMPTODISK,"Display all CPlayer entities (0=disabled, 1=only players, 2=also AI)");

	REGISTER_INT("g_showIdleStats", 0,VF_NULL,"");

	// battledust
	REGISTER_CVAR(g_battleDust_enable, 1, VF_NULL, "Enable/Disable battledust");
	REGISTER_CVAR(g_battleDust_debug, 0, VF_NULL, "0: off, 1: text, 2: text+gfx");
	g_battleDust_effect = REGISTER_STRING("g_battleDust_effect", "misc.battledust.light", VF_NULL, "Sets the effect to use for battledust");
	
	REGISTER_CVAR(g_proneNotUsableWeapon_FixType, 1, VF_NULL, "Test various fixes for not selecting hurricane while prone");
	REGISTER_CVAR(g_proneAimAngleRestrict_Enable, 1, VF_NULL, "Test fix for matching aim restrictions between 1st and 3rd person");
	
	REGISTER_CVAR(ctrl_input_smoothing, 0.0f, VF_NULL, "Smooths rotation input in GDC demo.");
	
	REGISTER_CVAR(g_disable_throw, 0, VF_NULL, "Disable object throwing");
	REGISTER_CVAR(g_disable_pickup, 0, VF_NULL, "Disable picking objects up");
	REGISTER_CVAR(g_disable_grab, 0, VF_NULL, "Disable NPC grabbing");

	REGISTER_CVAR(g_tpview_control, 0, VF_NULL, "Enables control of 3rd person view switching through cvar (F1 will be disabled!)");
	REGISTER_CVAR(g_tpview_enable, 0, VF_NULL, "Enables 3rd person view if precedent cvar is true");	

	REGISTER_CVAR(sv_LoadAllLayersForResList, 0, 0, "Bypasses game object layer filtering to load all layers of objects for a multiplayer level");
	REGISTER_CVAR(sv_input_timeout, 0, VF_NULL, "Experimental timeout in ms to stop interpolating client inputs since last update.");

	REGISTER_CVAR(g_spectate_TeamOnly, 1, VF_NULL, "If true, you can only spectate players on your team");
	REGISTER_CVAR(g_claymore_limit, 3, VF_NULL, "Max claymores a player can place (recycled above this value)");
	REGISTER_CVAR(g_avmine_limit, 3, VF_NULL, "Max avmines a player can place (recycled above this value)");
	REGISTER_CVAR(g_debugMines, 0, VF_NULL, "Enable debug output for mines and claymores");

  REGISTER_CVAR(aim_assistCrosshairSize, 25, VF_CHEAT, "screen size used for crosshair aim assistance");
  REGISTER_CVAR(aim_assistCrosshairDebug, 0, VF_CHEAT, "debug crosshair aim assistance");

	REGISTER_CVAR(g_deathCam, 1, VF_NULL, "Enables / disables the MP death camera (shows the killer's location)");

	REGISTER_CVAR(sv_pacifist, 0, VF_NULL, "Pacifist mode (only works on dedicated server)");

	pVehicleQuality = pConsole->GetCVar("v_vehicle_quality");		assert(pVehicleQuality);

	REGISTER_COMMAND("g_Log_FPS", CmdLogFPS, VF_NULL, "logs current frame rate");
	REGISTER_COMMAND("g_Log_Frametime", CmdLogFrameTime, VF_NULL, "logs current frame time");
	REGISTER_COMMAND("g_Log_Drawcalls", CmdLogDrawCalls, VF_NULL, "logs current draw call count");
	REGISTER_COMMAND("g_Log_Memory", CmdLogMemory, VF_NULL, "logs currently used memory");
	REGISTER_COMMAND("g_Log_VisReg", CmdLogVisRegPos, VF_NULL, "logs current player and camera positions to use for visual regression");

	// stereo 3D framework
	REGISTER_CVAR(g_stereoIronsightWeaponDistance, 0.375f, 0, "Distance of convergence plane when in ironsight");
	REGISTER_CVAR(g_stereoIronsightEyeDistance  , 0.0064f , 0, "Distance of eyes when in ironsight");
	REGISTER_CVAR(g_stereoFrameworkEnable  , 1 , VF_NULL, "Enables the processing of the game stereo framework. (2=extra debug output)");

	REGISTER_CVAR(g_muzzleFlashCull, 1, VF_NULL, "Enable muzzle flash culling");
	REGISTER_CVAR(g_muzzleFlashCullDistance, 30000.0f, 0, "Culls distant muzzle flashes");
	REGISTER_CVAR(g_rejectEffectVisibilityCull, 1, VF_NULL, "Enable reject effect culling");
	REGISTER_CVAR(g_rejectEffectCullDistance, 25.f*25.f, 0, "Culls distant shell casing effects");

	REGISTER_CVAR(g_hitDeathReactions_enable, 1, 0, "Enables/Disables Hit/Death reaction system");
	REGISTER_CVAR(g_hitDeathReactions_useLuaDefaultFunctions, 0, 0, "If enabled, it'll use the default lua methods inside HitDeathReactions script instead of the default c++ version");
	REGISTER_CVAR(g_hitDeathReactions_disable_ai, 1, 0, "If enabled, it'll not allow to execute any AI instruction during the hit/death reaction");
	REGISTER_CVAR(g_hitDeathReactions_debug, 0, 0, "Enables/Disables debug information for hit and death reactions system");
	REGISTER_CVAR(g_hitDeathReactions_disableRagdoll, 0, 0, "Disables switching to ragdoll at the end of animations");
	REGISTER_CVAR(g_hitDeathReactions_logReactionAnimsOnLoading, eHDRLRAT_DontLog, 0, "Non-Release only CVar: Enables logging of animations used by non-animation graph-based reactions. 0: don't log, 1: log anim names, 2: log filepaths");
	REGISTER_CVAR(g_hitDeathReactions_streaming, gEnv->bMultiplayer ? eHDRSP_EntityLifespanBased : eHDRSP_ActorsAliveAndNotInPool, 0, "Enables/Disables reactionAnims streaming. 0: Disabled, 1: DBA Registering-based, 2: Entity lifespan-based");
	REGISTER_CVAR(g_animatorDebug, false, 0, "Animator Debug Info");

	REGISTER_CVAR(g_forceItemRespawnTimer, 0.f, 0, "Override the amount of time it takes to respawn an item (0=disable override)");
	REGISTER_CVAR(g_defaultItemRespawnTimer, 5.f, 0, "Default amount of time to respawn an item - used if the actual timer length is lost in a host migration");

  NetInputChainInitCVars();

	InitAIPerceptionCVars(pConsole);
}

//------------------------------------------------------------------------
void SCVars::ReleaseAIPerceptionCVars(IConsole* pConsole)
{
	pConsole->UnregisterVariable("ai_perception.movement_useSurfaceType");
	pConsole->UnregisterVariable("ai_perception.movement_movingSurfaceDefault");
	pConsole->UnregisterVariable("ai_perception.movement_standingRadiusDefault");
	pConsole->UnregisterVariable("ai_perception.movement_crouchRadiusDefault");
	pConsole->UnregisterVariable("ai_perception.movement_standingMovingMultiplier");
	pConsole->UnregisterVariable("ai_perception.movement_crouchMovingMultiplier");
}

//------------------------------------------------------------------------
void SCVars::ReleaseCVars()
{
	IConsole* pConsole = gEnv->pConsole;

	pConsole->UnregisterVariable("g_messageOfTheDay", true);
	pConsole->UnregisterVariable("g_serverImageUrl", true);
	pConsole->UnregisterVariable("g_useOnlineServiceForDedicated", true);
	pConsole->UnregisterVariable("g_ProcessOnlineCallbacks", true);
	pConsole->UnregisterVariable("gl_maxSessionNameTimeWithoutConnection", true);
	pConsole->UnregisterVariable("g_maxGameBrowserResults", true);
	pConsole->UnregisterVariable("g_MatchmakingVersion", true);
	pConsole->UnregisterVariable("g_MatchmakingBlock", true);

	pConsole->UnregisterVariable("gl_skip", true);
	pConsole->UnregisterVariable("gl_votingCloseTimeBeforeStart", true);
	pConsole->UnregisterVariable("gl_experimentalPlaylistRotationAdvance", true);
	pConsole->UnregisterVariable("gl_enablePlaylistVoting", true);
	pConsole->UnregisterVariable("gl_minPlaylistSizeToEnableVoting", true);
	pConsole->UnregisterVariable("gl_enableOfflinePlaylistVoting", true);
	pConsole->UnregisterVariable("gl_enableOfflineCountdown", true);
	pConsole->UnregisterVariable("gl_time", true);
	pConsole->UnregisterVariable("gl_checkDLCBeforeStartTime", true);
	pConsole->UnregisterVariable("gl_maxSessionNameTimeWithoutConnection", true);
	pConsole->UnregisterVariable("g_autoAssignTeams", true);
	pConsole->UnregisterVariable("g_hostMigrationResumeTime", true);
	pConsole->UnregisterVariable("g_hostMigrationUseAutoLobbyMigrateInPrivateGames", true);

#if !defined(_RELEASE)
	pConsole->UnregisterVariable("net_onlyListGameServersContainingText", true);
	pConsole->UnregisterVariable("net_nat_type", true);
	pConsole->UnregisterVariable("net_initLobbyServiceToLan", true);
#endif

	pConsole->UnregisterVariable("cl_fov", true);
	pConsole->UnregisterVariable("cl_bob", true);
	pConsole->UnregisterVariable("cl_tpvDist", true);
	pConsole->UnregisterVariable("cl_tpvYaw", true);
	pConsole->UnregisterVariable("cl_nearPlane", true);
	pConsole->UnregisterVariable("cl_sprintShake", true);
	pConsole->UnregisterVariable("cl_sensitivityZeroG", true);
	pConsole->UnregisterVariable("cl_sensitivity", true);
	pConsole->UnregisterVariable("cl_invertMouse", true);
	pConsole->UnregisterVariable("cl_invertController", true);
	pConsole->UnregisterVariable("cl_crouchToggle", true);
	pConsole->UnregisterVariable("cl_fpBody", true);

	pConsole->UnregisterVariable("cl_sprintToggle", true);
	pConsole->UnregisterVariable("cl_sprintTime", true);
	pConsole->UnregisterVariable("cl_sprintRestingTime", true);

	pConsole->UnregisterVariable("i_staticfiresounds", true);
	pConsole->UnregisterVariable("i_soundeffects", true);
	pConsole->UnregisterVariable("i_lighteffects", true);
	pConsole->UnregisterVariable("i_particleeffects", true);
	pConsole->UnregisterVariable("i_offset_front", true);
	pConsole->UnregisterVariable("i_offset_up", true);
	pConsole->UnregisterVariable("i_offset_right", true);
	pConsole->UnregisterVariable("i_unlimitedammo", true);
	pConsole->UnregisterVariable("i_iceeffects", true);

	pConsole->UnregisterVariable("cl_strengthscale", true);

	pConsole->UnregisterVariable("cl_motionBlur", true);
	pConsole->UnregisterVariable("cl_sprintBlur", true);
	pConsole->UnregisterVariable("cl_hitShake", true);
	pConsole->UnregisterVariable("cl_hitBlur", true);

	pConsole->UnregisterVariable("cl_righthand", true);
	pConsole->UnregisterVariable("cl_screeneffects", true);

	pConsole->UnregisterVariable("cl_cam_orbit", true);
	pConsole->UnregisterVariable("cl_cam_orbit_slide", true);
	pConsole->UnregisterVariable("cl_cam_orbit_slidespeed", true);
	pConsole->UnregisterVariable("cl_cam_orbit_offsetX", true);
	pConsole->UnregisterVariable("cl_cam_orbit_offsetZ", true);
	pConsole->UnregisterVariable("cl_cam_orbit_distance", true);

	pConsole->UnregisterVariable("cl_cam_debug", true);

	pConsole->UnregisterVariable("pl_inputAccel", true);

	pConsole->UnregisterVariable("cl_actorsafemode", true);	
	pConsole->UnregisterVariable("g_enableSpeedLean", true);

	pConsole->UnregisterVariable("int_zoomAmount", true);
	pConsole->UnregisterVariable("int_zoomInTime", true);
	pConsole->UnregisterVariable("int_moveZoomTime", true);
	pConsole->UnregisterVariable("int_zoomOutTime", true);

	pConsole->UnregisterVariable("aa_maxDist", true);

	pConsole->UnregisterVariable("hr_rotateFactor", true);
	pConsole->UnregisterVariable("hr_rotateTime", true);
	pConsole->UnregisterVariable("hr_dotAngle", true);
	pConsole->UnregisterVariable("hr_fovAmt", true);
	pConsole->UnregisterVariable("hr_fovTime", true);

	pConsole->UnregisterVariable("cl_debugFreezeShake", true);  
	pConsole->UnregisterVariable("cl_frozenSteps", true);  
	pConsole->UnregisterVariable("cl_frozenSensMin", true);
	pConsole->UnregisterVariable("cl_frozenSensMax", true);
	pConsole->UnregisterVariable("cl_frozenAngleMin", true);
	pConsole->UnregisterVariable("cl_frozenAngleMax", true);
	pConsole->UnregisterVariable("cl_frozenMouseMult", true);
	pConsole->UnregisterVariable("cl_frozenKeyMult", true);
	pConsole->UnregisterVariable("cl_frozenSoundDelta", true);
	
	pConsole->UnregisterVariable("g_frostDecay", true);

	pConsole->UnregisterVariable("g_stanceTransitionSpeed", true);

	pConsole->UnregisterVariable("g_playerHealthValue", true);
	pConsole->UnregisterVariable("g_walkMultiplier", true);
	
	pConsole->UnregisterVariable("g_pp_scale_income", true);
	pConsole->UnregisterVariable("g_pp_scale_price", true);

	pConsole->UnregisterVariable("g_radialBlur", true);
	pConsole->UnregisterVariable("g_PlayerFallAndPlay", true);
	pConsole->UnregisterVariable("g_fallAndPlayThreshold", true);
	pConsole->UnregisterVariable("g_inventoryNoLimits", true);

	pConsole->UnregisterVariable("g_enableAlternateIronSight",true);
	pConsole->UnregisterVariable("g_enableTracers", true);
	pConsole->UnregisterVariable("g_meleeWhileSprinting", true);

	pConsole->UnregisterVariable("g_timelimit", true);
	pConsole->UnregisterVariable("g_teamlock", true);
	pConsole->UnregisterVariable("g_roundlimit", true);
	pConsole->UnregisterVariable("g_preroundtime", true);
	pConsole->UnregisterVariable("g_suddendeathtime", true);
	pConsole->UnregisterVariable("g_roundtime", true);
	pConsole->UnregisterVariable("g_fraglimit", true);
	pConsole->UnregisterVariable("g_fraglead", true);
	pConsole->UnregisterVariable("g_debugNetPlayerInput", true);
	pConsole->UnregisterVariable("g_debug_fscommand", true);
	pConsole->UnregisterVariable("g_debugDirectMPMenu", true);
	pConsole->UnregisterVariable("g_skipIntro", true);
	pConsole->UnregisterVariable("g_resetActionmapOnStart", true);
	pConsole->UnregisterVariable("g_useProfile", true);
	pConsole->UnregisterVariable("g_startFirstTime", true);

	pConsole->UnregisterVariable("g_tk_punish", true);
	pConsole->UnregisterVariable("g_tk_punish_limit", true);

	pConsole->UnregisterVariable("g_godMode", true);
	pConsole->UnregisterVariable("g_detachCamera", true);

	pConsole->UnregisterVariable("g_debugCollisionDamage", true);
	pConsole->UnregisterVariable("g_debugHits", true);
	
	pConsole->UnregisterVariable("v_profileMovement", true);    
	pConsole->UnregisterVariable("v_pa_surface", true);
	pConsole->UnregisterVariable("v_wind_minspeed", true);
	pConsole->UnregisterVariable("v_draw_suspension", true);
	pConsole->UnregisterVariable("v_draw_slip", true);  
	pConsole->UnregisterVariable("v_invertPitchControl", true);
	pConsole->UnregisterVariable("v_sprintSpeed", true);
	pConsole->UnregisterVariable("v_rockBoats", true);  
  pConsole->UnregisterVariable("v_debugMountedWeapon", true);  
	pConsole->UnregisterVariable("v_zeroGSpeedMultSpeed", true);
	pConsole->UnregisterVariable("v_zeroGSpeedMultSpeedSprint", true);
	pConsole->UnregisterVariable("v_zeroGSpeedMultNormal", true);
	pConsole->UnregisterVariable("v_zeroGSpeedMultNormalSprint", true);
	pConsole->UnregisterVariable("v_zeroGUpDown", true);
	pConsole->UnregisterVariable("v_zeroGMaxSpeed", true);
	pConsole->UnregisterVariable("v_zeroGSpeedMaxSpeed", true);
	pConsole->UnregisterVariable("v_zeroGSpeedModeEnergyConsumption", true);
	pConsole->UnregisterVariable("v_zeroGSwitchableGyro", true);
	pConsole->UnregisterVariable("v_zeroGEnableGBoots", true);
	pConsole->UnregisterVariable("v_dumpFriction", true);
  pConsole->UnregisterVariable("v_debugSounds", true);
	pConsole->UnregisterVariable("v_altitudeLimit", true);
	pConsole->UnregisterVariable("v_altitudeLimitLowerOffset", true);
	pConsole->UnregisterVariable("v_airControlSensivity", true);

	// variables from CPlayer
	pConsole->UnregisterVariable("player_DrawIK", true);
	pConsole->UnregisterVariable("player_NoIK", true);
	pConsole->UnregisterVariable("g_enableIdleCheck", true);
	pConsole->UnregisterVariable("pl_debug_ladders", true);
	pConsole->UnregisterVariable("pl_debug_movement", true);
	pConsole->UnregisterVariable("pl_debug_filter", true);

	// alien debugging
	pConsole->UnregisterVariable("aln_debug_movement", true);
	pConsole->UnregisterVariable("aln_debug_filter", true);
	pConsole->UnregisterVariable("pl_AnimationTriggeredFootstepSounds", true);

	// animation triggered footsteps
	pConsole->UnregisterVariable("g_footstepSoundsFollowEntity", true);
	pConsole->UnregisterVariable("g_footstepSoundsDebug", true);
	pConsole->UnregisterVariable("g_footstepSoundMaxDistanceSq", true);

	// variables from CPlayerMovementController
	pConsole->UnregisterVariable("g_showIdleStats", true);
	pConsole->UnregisterVariable("g_debugaimlook", true);

	// Controller aim helper cvars
	pConsole->UnregisterVariable("aim_assistSearchBox", true);
	pConsole->UnregisterVariable("aim_assistMaxDistance", true);
	pConsole->UnregisterVariable("aim_assistSnapDistance", true);
	pConsole->UnregisterVariable("aim_assistVerticalScale", true);
	pConsole->UnregisterVariable("aim_assistSingleCoeff", true);
	pConsole->UnregisterVariable("aim_assistAutoCoeff", true);
	pConsole->UnregisterVariable("aim_assistRestrictionTimeout", true);

	pConsole->UnregisterVariable("ctrl_aspectCorrection", true);
	pConsole->UnregisterVariable("ctrl_Curve_X", true);
	pConsole->UnregisterVariable("ctrl_Curve_Z", true);
	pConsole->UnregisterVariable("ctrl_Coeff_X", true);
	pConsole->UnregisterVariable("ctrl_Coeff_Z", true);
	pConsole->UnregisterVariable("ctrl_ZoomMode", true);

	// Aim assitance switches
	pConsole->UnregisterVariable("aim_assistAimEnabled", true);
	pConsole->UnregisterVariable("aim_assistTriggerEnabled", true);
	pConsole->UnregisterVariable("hit_assistSingleplayerEnabled", true);
	pConsole->UnregisterVariable("hit_assistMultiplayerEnabled", true);
		
	// weapon system
	pConsole->UnregisterVariable("i_debuggun_1", true);
	pConsole->UnregisterVariable("i_debuggun_2", true);

	pConsole->UnregisterVariable("tracer_min_distance", true);
	pConsole->UnregisterVariable("tracer_max_distance", true);
	pConsole->UnregisterVariable("tracer_min_scale", true);
	pConsole->UnregisterVariable("tracer_max_scale", true);
	pConsole->UnregisterVariable("tracer_max_count", true);
	pConsole->UnregisterVariable("tracer_player_radiusSqr", true);

	pConsole->UnregisterVariable("i_debug_projectiles", true);
	pConsole->UnregisterVariable("i_auto_turret_target", true);
	pConsole->UnregisterVariable("i_auto_turret_target_tacshells", true);

  pConsole->UnregisterVariable("i_debug_zoom_mods", true);
	pConsole->UnregisterVariable("i_debug_mp_flowgraph", true);

  pConsole->UnregisterVariable("g_quickGame_map",true);
  pConsole->UnregisterVariable("g_quickGame_mode",true);
  pConsole->UnregisterVariable("g_quickGame_min_players",true);
  pConsole->UnregisterVariable("g_quickGame_prefer_lan",true);
  pConsole->UnregisterVariable("g_quickGame_prefer_favorites",true);
  pConsole->UnregisterVariable("g_quickGame_prefer_mycountry",true);
  pConsole->UnregisterVariable("g_quickGame_ping1_level",true);
  pConsole->UnregisterVariable("g_quickGame_ping2_level",true);
	pConsole->UnregisterVariable("g_quickGame_debug",true);
	pConsole->UnregisterVariable("g_skip_tutorial",true);

	pConsole->UnregisterVariable("g_displayIgnoreList",true);
  pConsole->UnregisterVariable("g_buddyMessagesIngame",true);

	pConsole->UnregisterVariable("g_showPlayerState",true);

  pConsole->UnregisterVariable("g_battleDust_enable", true);
  pConsole->UnregisterVariable("g_battleDust_debug", true);
	pConsole->UnregisterVariable("g_battleDust_effect", true);

  pConsole->UnregisterVariable("g_proneNotUsableWeapon_FixType", true);
	pConsole->UnregisterVariable("g_proneAimAngleRestrict_Enable", true);

  pConsole->UnregisterVariable("sv_voting_timeout",true);
  pConsole->UnregisterVariable("sv_voting_cooldown",true);
  pConsole->UnregisterVariable("sv_voting_ratio",true);
  pConsole->UnregisterVariable("sv_voting_team_ratio",true);

	pConsole->UnregisterVariable("g_spectate_TeamOnly", true);
	pConsole->UnregisterVariable("g_claymore_limit", true);
	pConsole->UnregisterVariable("g_avmine_limit", true);
	pConsole->UnregisterVariable("g_debugMines", true);

	pConsole->UnregisterVariable("aim_assistCrosshairSize", true);
  pConsole->UnregisterVariable("aim_assistCrosshairDebug", true);

	pConsole->UnregisterVariable("g_ColorGradingBlendTime", true);
	
	pConsole->UnregisterVariable("ctrl_input_smoothing", true);

	pConsole->UnregisterVariable("g_disable_throw", true);
	pConsole->UnregisterVariable("g_disable_pickup", true);
	pConsole->UnregisterVariable("g_disable_grab", true);

	pConsole->UnregisterVariable("g_tpview_enable", true);
	pConsole->UnregisterVariable("g_tpview_control", true);
	pConsole->UnregisterVariable("g_tpview_force_goc", true);

	pConsole->UnregisterVariable("autotest_enabled");
	pConsole->UnregisterVariable("autotest_state_setup");
	pConsole->UnregisterVariable("autotest_quit_when_done");

	pConsole->UnregisterVariable("designer_warning_enabled");

	//bitmap ui
	pConsole->UnregisterVariable("g_show_fullscreen_info", true);
	pConsole->UnregisterVariable("g_show_crosshair", true);
	pConsole->UnregisterVariable("g_show_crosshair_tp", true);

	// GOC
	pConsole->UnregisterVariable("goc_enable", true);
	pConsole->UnregisterVariable("goc_targetx", true);
	pConsole->UnregisterVariable("goc_targety", true);
	pConsole->UnregisterVariable("goc_targetz", true);

	pConsole->UnregisterVariable("pl_nightvisionModeBinocular", true);

	pConsole->UnregisterVariable("g_muzzleFlashCull", true);
	pConsole->UnregisterVariable("g_muzzleFlashCullDistance", true);
	pConsole->UnregisterVariable("g_rejectEffectVisibilityCull", true);
	pConsole->UnregisterVariable("g_rejectEffectCullDistance", true);

	pConsole->UnregisterVariable("g_hitDeathReactions_enable", true);
	pConsole->UnregisterVariable("g_hitDeathReactions_useLuaDefaultFunctions", true);
	pConsole->UnregisterVariable("g_hitDeathReactions_disable_ai", true);
	pConsole->UnregisterVariable("g_hitDeathReactions_debug", true);
	pConsole->UnregisterVariable("g_hitDeathReactions_disableRagdoll", true);
	pConsole->UnregisterVariable("g_hitDeathReactions_disableHitAnimatedCollisions", true);
	pConsole->UnregisterVariable("g_animatorDebug", true);

	ReleaseAIPerceptionCVars(pConsole);
}

//------------------------------------------------------------------------
void CGame::RegisterConsoleVars()
{
	assert(m_pConsole);

	if (m_pCVars)
	{
		m_pCVars->InitCVars(m_pConsole);    
	}
}


//------------------------------------------------------------------------
//the following define saved me a lot of typing
#define DECL_TEMP_STATIC(type, cvar, default, zero, start, getter)	\
	static type sTemp_##cvar = zero;							\
	sTemp_##cvar = start ?												\
		gEnv->pConsole->GetCVar(#cvar)->getter() :	\
		sTemp_##cvar;																\
	gEnv->pConsole->GetCVar(#cvar)->Set(start ?		\
		default :																		\
		sTemp_##cvar);															\


void CaptureVideo(bool start, IConsoleCmdArgs *pArgs)
{
	//g_cvars
	DECL_TEMP_STATIC(int, g_showBitmapUi, 0, 0, start, GetIVal);
	DECL_TEMP_STATIC(int, g_godMode, 1, 0, start, GetIVal);

	//timesteps
	DECL_TEMP_STATIC(float, p_fixed_timestep, 0.033333f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, t_FixedStep,  0.033333f, 0.0f, start, GetFVal);	

	//e_cvars
	DECL_TEMP_STATIC(int, e_Lods, 0, 0, start, GetIVal);
	DECL_TEMP_STATIC(int, e_LodsForceUse, 1, 0, start, GetIVal);
	DECL_TEMP_STATIC(int, e_LodMin, 0, 0, start, GetIVal);
	DECL_TEMP_STATIC(int, e_LodMax, 0, 0, start, GetIVal);
	DECL_TEMP_STATIC(int, e_LodMinTtris, 0, 0, start, GetIVal);
	DECL_TEMP_STATIC(float, e_LodRatio, 200.0f, 0.0f, start, GetFVal);
	//DECL_TEMP_STATIC(float, e_LodCompMaxSize, 0.0f, 0.0f, start, GetFVal);
	//DECL_TEMP_STATIC(int, e_ObjQuality, 0, 0, start, GetIVal);

	DECL_TEMP_STATIC(float, e_ViewDistRatio, 100.0f, 0.0f, start, GetFVal);
	//DECL_TEMP_STATIC(float, e_ViewDistCompMaxSize, 100.0f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, e_ViewDistRatioPortals, 100.0f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, e_ViewDistRatioDetail, 100.0f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, e_ViewDistCustomRatio, 100.0f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, e_ViewDistRatioVegetation, 100.0f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, e_GsmRange, 100.0f, 0.0f, start, GetFVal);
	DECL_TEMP_STATIC(float, e_LodDistShader, 100.0f, 0.0f, start, GetFVal);
		
	//capture_cvars
	gEnv->pConsole->ExecuteString("capture_file_format tga");
	//DECL_TEMP_STATIC(char*, capture_file_format, "tga", "jpg", start, GetString);	//fail?
	gEnv->pConsole->GetCVar("capture_file_format")->Set((int)start);
}
#undef DECL_TEMP_STATIC

void CmdStartVideoCapture(IConsoleCmdArgs *pArgs)
{
	CaptureVideo(true, pArgs);
}


void CmdEndVideoCapture(IConsoleCmdArgs *pArgs)
{
	CaptureVideo(false, pArgs);
}

//------------------------------------------------------------------------
void CmdGoto(IConsoleCmdArgs *pArgs)
{
	// feature is mostly useful for QA purposes, the editor has a similar feature, here we can call the editor command as well

	// todo:
	// * move to CryAction
	// * if in editor and game is not active it should move editor camera
	// * third person game should work by using player position
	// * level name could be part of the string

	const CCamera &rCam = gEnv->pRenderer->GetCamera();
	Matrix33 m = Matrix33(rCam.GetMatrix());

	int iArgCount = pArgs->GetArgCount();

	Ang3 aAngDeg = RAD2DEG(Ang3::GetAnglesXYZ(m));		// in degrees
	Vec3 vPos = rCam.GetPosition();

	if(iArgCount==1)
	{
		gEnv->pLog->LogWithType(ILog::eInputResponse,"$5GOTO %.3f %.3f %.3f %.3f %.3f %.3f",
			vPos.x, vPos.y, vPos.z, aAngDeg.x, aAngDeg.y, aAngDeg.z);
		return;
	}

	// complicated but maybe the best Entity we can move to the given spot
	IGame *pGame = gEnv->pGame;																								if(!pGame)return;
	IGameFramework *pGameFramework=pGame->GetIGameFramework();								if(!pGameFramework)return;
	IViewSystem *pViewSystem=pGameFramework->GetIViewSystem();								if(!pViewSystem)return;
	IView *pView=pViewSystem->GetActiveView();																if(!pView)return;
	IEntity *pEntity = gEnv->pEntitySystem->GetEntity(pView->GetLinkedId());	if(!pEntity)return;

	if((iArgCount==4 || iArgCount==7)
		&& sscanf(pArgs->GetArg(1),"%f",&vPos.x)==1
		&& sscanf(pArgs->GetArg(2),"%f",&vPos.y)==1
		&& sscanf(pArgs->GetArg(3),"%f",&vPos.z)==1)
	{
		Matrix34 tm = pEntity->GetWorldTM();

		tm.SetTranslation(vPos);

		if(iArgCount==7
			&& sscanf(pArgs->GetArg(4),"%f",&aAngDeg.x)==1
			&& sscanf(pArgs->GetArg(5),"%f",&aAngDeg.y)==1
			&& sscanf(pArgs->GetArg(6),"%f",&aAngDeg.z)==1)
		{
			tm.SetRotation33( Matrix33::CreateRotationXYZ(DEG2RAD(aAngDeg)) );
		}

		// if there is an editor
		char str[256];
		sprintf(str,"ED_GOTO %.f %.f %.f %.f %.f %.f",vPos.x,vPos.y,vPos.z,aAngDeg.x,aAngDeg.y,aAngDeg.z);
		gEnv->pConsole->ExecuteString(str,true);

		pEntity->SetWorldTM(tm);
		return;
	}

	gEnv->pLog->LogError("GOTO: Invalid arguments");
}

//------------------------------------------------------------------------
void CmdCamGoto(IConsoleCmdArgs *pArgs)
{
	// todo:
	// * move to CryAction along with CmdGoto	

	CCamera cam = gEnv->pRenderer->GetCamera();
	Matrix33 m = Matrix33(cam.GetMatrix());

	int iArgCount = pArgs->GetArgCount();

	Ang3 aAng = Ang3::GetAnglesXYZ(m);
	Ang3 aAngDeg = RAD2DEG(aAng);		// in degrees
	Vec3 vPos = cam.GetPosition();

	if(iArgCount==1)
	{
		gEnv->pLog->LogWithType(ILog::eInputResponse,"$5CAMGOTO %.3f %.3f %.3f %.3f %.3f %.3f",
			vPos.x, vPos.y, vPos.z, aAngDeg.x, aAngDeg.y, aAngDeg.z);
		return;
	}

	

	if((iArgCount==4 || iArgCount==7)
		&& sscanf(pArgs->GetArg(1),"%f",&vPos.x)==1
		&& sscanf(pArgs->GetArg(2),"%f",&vPos.y)==1
		&& sscanf(pArgs->GetArg(3),"%f",&vPos.z)==1)
	{
		cam.SetPosition(vPos);

		if(iArgCount==7
			&& sscanf(pArgs->GetArg(4),"%f",&aAngDeg.x)==1
			&& sscanf(pArgs->GetArg(5),"%f",&aAngDeg.y)==1
			&& sscanf(pArgs->GetArg(6),"%f",&aAngDeg.z)==1)
		{
			cam.SetAngles(aAngDeg);
		}

		IGame*			pGame			= gEnv ? gEnv->pGame : NULL;
		IGameFramework*	pGameFramework	= pGame ? pGame->GetIGameFramework() : NULL;
		IViewSystem*	pViewSystem		= pGameFramework ? pGameFramework->GetIViewSystem() : NULL;
		IView*			pView			= pViewSystem ? pViewSystem->GetActiveView() : NULL;
		ISystem*		pSystem			= gEnv ? gEnv->pSystem : NULL;
		IRenderer*		pRenderer		= gEnv ? gEnv->pRenderer : NULL;

		if(pView)
		{
			SViewParams viewParams = *pView->GetCurrentParams();
			viewParams.position = vPos;
			viewParams.rotation.SetRotationXYZ( aAng );
			pView->SetCurrentParams( viewParams );
		}
		else if(pSystem)
		{
			pSystem->SetViewCamera( cam );
		}
		else if(pRenderer)
		{
			pRenderer->SetCamera( cam );
		}
				
		return;
	}

	gEnv->pLog->LogError("CAMGOTO: Invalid arguments");
}

//------------------------------------------------------------------------
void CGame::RegisterConsoleCommands()
{
	assert(m_pConsole);

	REGISTER_COMMAND("quit", "System.Quit()", VF_RESTRICTEDMODE, "Quits the game");
	REGISTER_COMMAND("goto", CmdGoto, VF_CHEAT, 
		"Get or set the current position and orientation for the player\n"
		"Usage: goto\n"
		"Usage: goto x y z\n"
		"Usage: goto x y z wx wy wz");	
	REGISTER_COMMAND("camgoto", CmdCamGoto, VF_CHEAT, 
		"Get or set the current position and orientation for the camera\n"
		"Usage: camgoto\n"
		"Usage: camgoto x y z\n"
		"Usage: camgoto x y z wx wy wz");
	REGISTER_COMMAND("gotoe", "local e=System.GetEntityByName(%1); if (e) then g_localActor:SetWorldPos(e:GetWorldPos()); end", VF_CHEAT, "Set the position of a entity with an given name");
	REGISTER_COMMAND("freeze", "g_gameRules:SetFrozenAmount(g_localActor,1)", VF_NULL, "Freezes player");

	REGISTER_COMMAND("loadactionmap", CmdLoadActionmap, VF_NULL, "Loads a key configuration file");
	REGISTER_COMMAND("restartgame", CmdRestartGame, VF_NULL, "Restarts CryENGINE completely.");

	REGISTER_COMMAND("sv_say", CmdSay, VF_NULL, "Broadcasts a message to all clients.");

  REGISTER_COMMAND("g_reloadGameRules", CmdReloadGameRules, VF_NULL, "Reload GameRules script");

  REGISTER_COMMAND("g_nextlevel", CmdNextLevel,VF_NULL,"Switch to next level in rotation or restart current one.");

	REGISTER_COMMAND("preloadforstats","PreloadForStats()",VF_CHEAT,"Preload multiplayer assets for memory statistics.");

	REGISTER_COMMAND("StartVideoCapture", CmdStartVideoCapture, VF_NULL, "sets special settings for nice video captures and starts capturing.");
	REGISTER_COMMAND("EndVideoCapture", CmdEndVideoCapture, VF_NULL, "ends capturing and restores the original settings.");
}

//------------------------------------------------------------------------
void CGame::UnregisterConsoleCommands()
{
	assert(m_pConsole);

	m_pConsole->RemoveCommand("quit");
	m_pConsole->RemoveCommand("goto");	
	m_pConsole->RemoveCommand("camgoto");
	m_pConsole->RemoveCommand("freeze");

	m_pConsole->RemoveCommand("loadactionmap");
	m_pConsole->RemoveCommand("restartgame");

	m_pConsole->RemoveCommand("name");
	m_pConsole->RemoveCommand("team");
	m_pConsole->RemoveCommand("kill");
  m_pConsole->RemoveCommand("v_kill");
	m_pConsole->RemoveCommand("sv_restart");
	m_pConsole->RemoveCommand("sv_say");
	m_pConsole->RemoveCommand("i_reload");

	m_pConsole->RemoveCommand("dumpss");

	m_pConsole->RemoveCommand("g_reloadGameRules");
  m_pConsole->RemoveCommand("g_quickGame");
  m_pConsole->RemoveCommand("g_quickGameStop");

  m_pConsole->RemoveCommand("g_nextlevel");
  m_pConsole->RemoveCommand("g_vote");
  m_pConsole->RemoveCommand("g_startKickVoting");
  m_pConsole->RemoveCommand("g_startNextMapVoting");


	m_pConsole->RemoveCommand("g_battleDust_reload");
	m_pConsole->RemoveCommand("bulletTimeMode");
	m_pConsole->RemoveCommand("GOCMode");

	m_pConsole->RemoveCommand("ShowGODMode");
	m_pConsole->RemoveCommand("test_pathfinder");

	//video capturing
	m_pConsole->RemoveCommand("CmdStartVideoCapture");
	m_pConsole->RemoveCommand("CmdEndVideoCapture");

	m_pConsole->RemoveCommand("g_hitDeathReactions_reload");
	m_pConsole->RemoveCommand("g_hitDeathReactions_dumpAssetUsage");
}

#undef GetCommandLine

//------------------------------------------------------------------------
void CGame::CmdSay(IConsoleCmdArgs *pArgs)
{
	if (pArgs->GetArgCount()>1 && gEnv->bServer)
	{
		const char *msg=pArgs->GetCommandLine()+strlen(pArgs->GetArg(0))+1;
		g_pGame->GetGameRules()->SendTextMessage(eTextMessageServer, msg, eRMI_ToAllClients);

		if (!gEnv->IsClient())
			CryLogAlways("** Server: %s **", msg);
	}
}

//------------------------------------------------------------------------
void CGame::CmdLoadActionmap(IConsoleCmdArgs *pArgs)
{
	if(pArgs->GetArg(1))
		g_pGame->LoadActionMaps(pArgs->GetArg(1));
}

//------------------------------------------------------------------------
void CGame::CmdRestartGame(IConsoleCmdArgs *pArgs)
{
	GetISystem()->Relaunch(true);
	GetISystem()->Quit();
}

//------------------------------------------------------------------------
void CGame::CmdReloadGameRules(IConsoleCmdArgs *pArgs)
{
  if (gEnv->bMultiplayer)
    return;

  IGameRulesSystem* pGameRulesSystem = g_pGame->GetIGameFramework()->GetIGameRulesSystem();
  IGameRules* pGameRules = pGameRulesSystem->GetCurrentGameRules();
    
  const char* name = "SinglePlayer";
  IEntityClass* pEntityClass = 0; 
  
  if (pGameRules)    
  {
    pEntityClass = pGameRules->GetEntity()->GetClass();
    name = pEntityClass->GetName();
  }  
  else
    pEntityClass = gEnv->pEntitySystem->GetClassRegistry()->FindClass(name);

  if (pEntityClass)
  {
    pEntityClass->LoadScript(true);
  
    if (pGameRulesSystem->CreateGameRules(name))
      CryLog("reloaded GameRules <%s>", name);
    else
      GameWarning("reloading GameRules <%s> failed!", name);
  }  
}

void CGame::CmdNextLevel(IConsoleCmdArgs* pArgs)
{
  ILevelRotation *pLevelRotation = g_pGame->GetIGameFramework()->GetILevelSystem()->GetLevelRotation();
  if (pLevelRotation->GetLength())
    pLevelRotation->ChangeLevel(pArgs);
}