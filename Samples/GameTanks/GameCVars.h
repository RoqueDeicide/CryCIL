#ifndef __GAMECVARS_H__
#define __GAMECVARS_H__

struct SAIPerceptionCVars
{
	int			movement_useSurfaceType;
	float		movement_movingSurfaceDefault;
	float		movement_standingRadiusDefault;
	float		movement_crouchRadiusDefault;
	float		movement_standingMovingMultiplier;
	float		movement_crouchMovingMultiplier;
};

struct SCVars
{	
	static const float v_altitudeLimitDefault()
	{
		return 600.0f;
	}

	SAIPerceptionCVars ai_perception;

	float cl_fov;
	float cl_bob;
	float cl_headBob;
	float cl_headBobLimit;
	float cl_tpvDist;
	float cl_tpvYaw;
	float cl_nearPlane;
	float cl_sprintShake;
	float cl_sensitivityZeroG;
	float cl_sensitivity;
	float cl_strengthscale;
	int		cl_invertMouse;
	int		cl_invertController;
	int		cl_crouchToggle;
	int		cl_fpBody;	
	int		cl_debugSwimming;
	int		cl_sprintToggle;
	float	cl_sprintTime;
	float	cl_sprintRestingTime;

	int		cl_camModify;
	float cl_camKeepX;
	float cl_camKeepY;
	float cl_camKeepZ;
	float cl_camOffsetX;
	float cl_camOffsetY;
	float cl_camOffsetZ;
	float cl_camRotateX;
	float cl_camRotateY;
	float cl_camRotateZ;
	float cl_camTranslateX;
	float cl_camTranslateY;
	float cl_camTranslateZ;


	// the following CVars are for the new free Third Person Cam
	//////////////////////////////////////////////////////////////////////////
	int		cl_cam_orbit;	// switch between the old and the new camera
	int		cl_cam_orbit_slide;
	float cl_cam_orbit_slidespeed;
	float cl_cam_orbit_offsetX;
	float cl_cam_orbit_offsetZ;
	float cl_cam_orbit_distance;

	int		cl_cam_debug;

	int		cl_enable_tree_transparency;
	int		cl_fake_first_person;
	float cl_cam_clipping_offset_distance;
	float	cl_cam_min_distance;
	float cl_cam_yaw_input_inertia;
	float cl_cam_pitch_input_inertia;

	// camera rotation
	float cl_cam_rotation_speed;
	int		cl_cam_rotation_enable_acceleration;
	float cl_cam_rotation_acceleration_time_yaw;
	float cl_cam_rotation_acceleration_time_pitch;
	float cl_cam_mouseYawScale;
	float cl_cam_mousePitchScale;

	// camera tracking
	int		cl_cam_tracking;
	int		cl_cam_tracking_allow_pitch;
	float cl_cam_tracking_rotation_speed;
	float cl_cam_tracking_volume;

	// lazy cam
	int		cl_cam_enable_lazy_cam;
	float cl_cam_lazy_cam_distance_XY;
	float cl_cam_lazy_cam_distance_Z;
	float cl_cam_lazy_cam_time_XY;
	float cl_cam_lazy_cam_time_Z;

	// auto-follow
	float cl_cam_auto_follow_rate;
	float cl_cam_auto_follow_threshold;
	float	cl_cam_auto_follow_movement_speed;

	// View damping and lookahead
	float cl_tpvDeltaDist;
	float cl_tpvDeltaVel;
	float cl_tpvDeltaDistNav;
	float cl_tpvDeltaVelNav;
	float cl_tpvPosDamping;
	float cl_tpvAngDamping;
	float cl_tpvPosDampingNav;
	float cl_tpvAngDampingNav;
	float cl_tpvFOVDamping;

	// nav
	int		cl_joy_nav_useHforV;
	float cl_joy_nav_speedV;
	float cl_joy_nav_sensitivityV;
	float cl_joy_nav_maxV;
	float cl_joy_nav_speedH;
	float cl_joy_nav_sensitivityH;
	float cl_joy_nav_maxH;
	float cl_cam_keyLeftRightMove_SmoothingTime;

	//old stuff
	float cl_cam_PitchMax;
	float cl_cam_PitchMin;

	float cl_tpvHOffCombatBias;
	float cl_tpvVOffCombatBias;
	int		cl_tpvNavSwitchVCentreEnabled;
	float cl_tpvNavSwitchVCentreValue;

	// end Third Person Cam
	//////////////////////////////////////////////////////////////////////////


	ICVar* 	ca_GameControlledStrafingPtr;
	float pl_curvingSlowdownSpeedScale;
	float ac_enableProceduralLeaning;

	float cl_shallowWaterSpeedMulPlayer;
	float cl_shallowWaterSpeedMulAI;
	float cl_shallowWaterDepthLo;
	float cl_shallowWaterDepthHi;

	int		cl_debugFreezeShake;
	float cl_frozenSteps;
	float cl_frozenSensMin;
	float cl_frozenSensMax;
	float cl_frozenAngleMin;
	float cl_frozenAngleMax;
	float cl_frozenMouseMult;
	float cl_frozenKeyMult;
	float cl_frozenSoundDelta;

	int		g_ProcessOnlineCallbacks;
	int		g_maxGameBrowserResults;
	int		g_MatchmakingVersion;
	int		g_MatchmakingBlock;
	
	int		g_autoAssignTeams;

	float g_hostMigrationResumeTime;
	int   g_hostMigrationUseAutoLobbyMigrateInPrivateGames;

	ICVar *g_messageOfTheDay;
	ICVar *g_serverImageUrl;

#if !defined(_RELEASE)
	ICVar* net_onlyListGameServersContainingText;
	ICVar* net_nat_type;
	int    net_initLobbyServiceToLan;
#endif

	int		gl_skip;
	float gl_votingCloseTimeBeforeStart;
	int		gl_experimentalPlaylistRotationAdvance;
	int		gl_enablePlaylistVoting;
	int		gl_minPlaylistSizeToEnableVoting;
	int		gl_enableOfflinePlaylistVoting;
	int		gl_enableOfflineCountdown;
	float gl_time;
	float gl_checkDLCBeforeStartTime;
	float gl_maxSessionNameTimeWithoutConnection;

	//bitmap ui overlays
	int		g_showBitmapUi;
	int		g_show_fullscreen_info;
	int		g_show_crosshair;
	int		g_show_crosshair_tp;

	// 3d hud
	float g_hud3d_cameraDistance;
	float g_hud3d_cameraOffsetZ;
	int g_hud3D_cameraOverride;

	int g_showHud;


#if USE_CRYLOBBY_GAMESPY
	// GameSpy credentials
	int g_gamespy_loginUI;
	ICVar*g_gamespy_email;
	ICVar*g_gamespy_accountnumber;
	ICVar*g_gamespy_unique_nick;
	ICVar*g_gamespy_password;
	ICVar*g_gamespy_cdkey;
	ICVar*g_gamespy_catalog_version;
	ICVar*g_gamespy_server_region;

#if !defined(_RELEASE)
#define g_gamespy_catalog_token  g_gamespy_catalog_tokenCVar->GetString()
	ICVar *g_gamespy_catalog_tokenCVar;
#define g_gamespy_catalog_region  g_gamespy_catalog_regionCVar->GetString()
	ICVar*g_gamespy_catalog_regionCVar;
#else
#define g_gamespy_catalog_token  g_gamespy_catalog_tokenStr
	const char *g_gamespy_catalog_tokenStr;
#define g_gamespy_catalog_region  g_gamespy_catalog_regionStr
	const char *g_gamespy_catalog_regionStr;
#endif

	int g_officialServer;
#endif
	int		dt_enable;
	float dt_time;
	float dt_meleeTime;

	int sv_LoadAllLayersForResList;

	int   sv_input_timeout;

	float hr_rotateFactor;
	float hr_rotateTime;
	float hr_dotAngle;
	float hr_fovAmt;
	float hr_fovTime;

	int		i_staticfiresounds;
	int		i_soundeffects;
	int		i_lighteffects;
	int		i_particleeffects;
	int		i_rejecteffects;
	float i_offset_front;
	float i_offset_up;
	float i_offset_right;
	int		i_unlimitedammo;
	int   i_iceeffects;

	float int_zoomAmount;
	float int_zoomInTime;
	float int_moveZoomTime;
	float int_zoomOutTime;

	float pl_inputAccel;
	int pl_nightvisionModeBinocular;

	int		g_godMode;
	int		g_detachCamera;
	int		g_enableSpeedLean;
	int   g_difficultyLevel;
	int		g_difficultyHintSystem;
	float g_difficultyRadius;
	int		g_difficultyRadiusThreshold;
	int		g_difficultySaveThreshold;
	float	g_playerHealthValue;
	float g_walkMultiplier;


	float g_frostDecay;
	float g_stanceTransitionSpeed;
	int   g_debugaimlook;
	int		g_enableIdleCheck;
	int		g_playerRespawns;
	float g_playerLowHealthThreshold;
	int		g_punishFriendlyDeaths;
	int		g_enableMPStealthOMeter;
	int   g_meleeWhileSprinting;

	float g_forceItemRespawnTimer;
	float g_defaultItemRespawnTimer;

	// animation triggered footsteps
	int   g_footstepSoundsDebug;
	int   g_footstepSoundsFollowEntity;
	float g_footstepSoundMaxDistanceSq;

	float g_fallAndPlayThreshold;

	int g_inventoryNoLimits;
	int sv_pacifist;

	int g_empStyle;

	float g_pp_scale_income;
	float g_pp_scale_price;
	float g_energy_scale_income;
	float g_energy_scale_price;

	float g_dofset_minScale;
	float g_dofset_maxScale;
	float g_dofset_limitScale;

	float g_dof_minHitScale;
	float g_dof_maxHitScale;
	float g_dof_sampleAngle;
	float g_dof_minAdjustSpeed;
	float g_dof_maxAdjustSpeed;
	float g_dof_averageAdjustSpeed;
	float g_dof_distAppart;
	int		g_dof_ironsight;

	float	g_ColorGradingBlendTime;

	// explosion culling
	int		g_ec_enable;
	float g_ec_radiusScale;
	float g_ec_volume;
	float g_ec_extent;
	int		g_ec_removeThreshold;

	float g_radialBlur;
	int		g_playerFallAndPlay;

	float g_timelimit;
	int		g_teamlock;
	float g_roundtime;
	int		g_preroundtime;
	int		g_suddendeathtime;
	int		g_roundlimit;
	int		g_fraglimit;
  int		g_fraglead;
  float g_friendlyfireratio;
  int   g_revivetime; 
  int   g_autoteambalance;
	int   g_minplayerlimit;
	int   g_minteamlimit;
	int		g_mpSpeedRechargeDelay;

	int   g_tk_punish;
	int		g_tk_punish_limit;

	int   g_debugNetPlayerInput;
	int   g_debugCollisionDamage;
	int   g_debugHits;

	int		g_debug_fscommand;
	int		g_debugDirectMPMenu;
	int		g_skipIntro;
	int		g_resetActionmapOnStart;
	int		g_useProfile;
	int		g_startFirstTime;
	float g_cutsceneSkipDelay;

	int   g_enableTracers;
	int		g_enableAlternateIronSight;

	float	g_ragdollMinTime;
	float	g_ragdollUnseenTime;
	float	g_ragdollPollTime;
	float	g_ragdollDistance;	

	int		pl_debug_ladders;
	float	pl_ladder_animOffset;
	int		pl_testGroundAlignOverride;


	int		pl_debug_movement;
	int		pl_debug_jumping;
	ICVar*pl_debug_filter;

	// PLAYERPREDICTION
	float pl_velocityInterpAirControlScale;
	int pl_velocityInterpSynchJump;
	int pl_debugInterpolation;
	float pl_velocityInterpAirDeltaFactor;
	float pl_velocityInterpPathCorrection;
	int pl_velocityInterpAlwaysSnap;

	float pl_netAimLerpFactor;
	float pl_netSerialiseMaxSpeed;

	int pl_serialisePhysVel;
	float pl_clientInertia;
	float pl_playerErrorSnapDistSquare;
	// ~PLAYERPREDICTION

	int		aln_debug_movement;
	ICVar*aln_debug_filter;
	int pl_DebugFootstepSounds;

#if !defined(_RELEASE)
	int   v_debugMovement;
	float v_debugMovementMoveVertically;
	float v_debugMovementX;
	float v_debugMovementY;
	float v_debugMovementZ;
	float v_debugMovementSensitivity;
#endif

	int   v_profileMovement;  
	int   v_draw_suspension;
	int   v_draw_slip;
	int   v_pa_surface;    
	int   v_invertPitchControl;  
	float v_wind_minspeed; 
	float v_sprintSpeed;
	int   v_dumpFriction;
	int   v_rockBoats;
  int   v_debugSounds;
	float v_altitudeLimit;
	ICVar* pAltitudeLimitCVar;
	float v_altitudeLimitLowerOffset;
	ICVar* pAltitudeLimitLowerOffsetCVar;
	float v_airControlSensivity;
	float v_stabilizeVTOL;
	int   v_help_tank_steering;
  int   v_debugMountedWeapon;
	ICVar* pVehicleQuality;

	float pl_swimBaseSpeed;
	float pl_swimBackSpeedMul;
	float pl_swimSideSpeedMul;
	float pl_swimVertSpeedMul;
	float pl_swimNormalSprintSpeedMul;
	float pl_swimSpeedSprintSpeedMul;
	float pl_swimUpSprintSpeedMul;
	float pl_swimJumpStrengthCost;
	float pl_swimJumpStrengthSprintMul;
	float pl_swimJumpStrengthBaseMul;
	float pl_swimJumpSpeedCost;
	float pl_swimJumpSpeedSprintMul;
	float pl_swimJumpSpeedBaseMul;

	float pl_fallDamage_Normal_SpeedSafe;
	float pl_fallDamage_Normal_SpeedFatal;
	float pl_fallDamage_Strength_SpeedSafe;
	float pl_fallDamage_Strength_SpeedFatal;
	float pl_fallDamage_SpeedBias;
	int pl_debugFallDamage;
	
  float pl_zeroGSpeedMultSpeed;
	float pl_zeroGSpeedMultSpeedSprint;
	float pl_zeroGSpeedMultNormal;
	float pl_zeroGSpeedMultNormalSprint;
	float pl_zeroGUpDown;
	float pl_zeroGBaseSpeed;
	float pl_zeroGSpeedMaxSpeed;
	float pl_zeroGSpeedModeEnergyConsumption;
	float	pl_zeroGDashEnergyConsumption;
	int		pl_zeroGSwitchableGyro;
	int		pl_zeroGEnableGBoots;
	float pl_zeroGThrusterResponsiveness;
	float pl_zeroGFloatDuration;
	int		pl_zeroGParticleTrail;
	int		pl_zeroGEnableGyroFade;
	float pl_zeroGGyroFadeAngleInner;
	float pl_zeroGGyroFadeAngleOuter;
	float pl_zeroGGyroFadeExp;
	float pl_zeroGGyroStrength;
	float pl_zeroGAimResponsiveness;


	int		ctrl_aspectCorrection;
	float ctrl_Curve_X;
	float ctrl_Curve_Z;
	float ctrl_Coeff_X;
	float ctrl_Coeff_Z;
	int		ctrl_ZoomMode;

	float aim_assistSearchBox;
	float aim_assistMaxDistance;
	float aim_assistSnapDistance;
	float aim_assistVerticalScale;
	float aim_assistSingleCoeff;
	float aim_assistAutoCoeff;
	float aim_assistRestrictionTimeout;

	int aim_assistAimEnabled;
	int aim_assistTriggerEnabled;
	int hit_assistSingleplayerEnabled;
	int hit_assistMultiplayerEnabled;

  int aim_assistCrosshairSize;
  int aim_assistCrosshairDebug;
		
  float g_stereoIronsightWeaponDistance;
  float g_stereoIronsightEyeDistance;
  int   g_stereoFrameworkEnable;

  float g_combatFadeTime;
	float g_combatFadeTimeDelay;
	float g_battleRange;

	ICVar*i_debuggun_1;
	ICVar*i_debuggun_2;

	float	tracer_min_distance;
	float	tracer_max_distance;
	float	tracer_min_scale;
	float	tracer_max_scale;
	int		tracer_max_count;
	float	tracer_player_radiusSqr;
	int		i_debug_projectiles;
	int		i_auto_turret_target;
	int		i_auto_turret_target_tacshells;
	int		i_debug_zoom_mods;
  int   i_debug_turrets;
  int   i_debug_sounds;
	int		i_debug_mp_flowgraph;
  
  ICVar*  g_quickGame_map;
  ICVar*  g_quickGame_mode;
  int     g_quickGame_min_players;
  int     g_quickGame_prefer_lan;
  int     g_quickGame_prefer_favorites;
  int     g_quickGame_prefer_my_country;
  int     g_quickGame_ping1_level;
  int     g_quickGame_ping2_level;
  int     g_quickGame_debug;
	int			g_skip_tutorial;

  int     g_displayIgnoreList;
  int     g_buddyMessagesIngame;

	int     g_showPlayerState;

  int			g_battleDust_enable;
	int			g_battleDust_debug;
	ICVar*  g_battleDust_effect;

	int			g_proneNotUsableWeapon_FixType;
	int			g_proneAimAngleRestrict_Enable;
	int			g_enableFriendlyFallAndPlay;

	int			g_spectate_TeamOnly;
	int			g_claymore_limit;
	int			g_avmine_limit;
	int			g_debugMines;
	int			g_deathCam;
	

	float		ctrl_input_smoothing;
	
	int			g_disable_throw;
	int			g_disable_pickup;
	int			g_disable_grab;

	int			g_tpview_control;
	int			g_tpview_enable;	

	int			cl_player_landing_forcefeedback;	// switch between the old and the new camera

	int watch_enabled;
	float watch_text_render_start_pos_x;
	float watch_text_render_start_pos_y;
	float watch_text_render_size;
	float watch_text_render_lineSpacing;
	float watch_text_render_fxscale;

	int autotest_enabled;
	ICVar* autotest_state_setup;
	int autotest_quit_when_done;
	int designer_warning_enabled;
	int designer_warning_level_resources;

	int g_enableSlimCheckpoints;

	//MUZZLE FLASH
	float	g_muzzleFlashCullDistance;
	int		g_muzzleFlashCull;

	int		g_rejectEffectVisibilityCull;
	float	g_rejectEffectCullDistance;

	// Hit Death Reactions CVars
	int			g_hitDeathReactions_enable;
	int			g_hitDeathReactions_useLuaDefaultFunctions;
	int			g_hitDeathReactions_disable_ai;
	int			g_hitDeathReactions_debug;
	int			g_hitDeathReactions_disableRagdoll;
	int		  g_animatorDebug;

	int	g_useOnlineLobbyService;

	enum EHitDeathReactionsLogReactionAnimsType
	{
			eHDRLRAT_DontLog = 0,
			eHDRLRAT_LogAnimNames,
			eHDRLRAT_LogFilePaths,
	};
	int			g_hitDeathReactions_logReactionAnimsOnLoading;

	enum EHitDeathReactionsStreamingPolicy
	{
			eHDRSP_Disabled = 0,
			eHDRSP_ActorsAliveAndNotInPool,		// the assets are locked if at least one of the actors using the profile is alive and not in the pool
			eHDRSP_EntityLifespanBased,				// the assets are requested/released whenever the entities using them are spawned/removed
	};
	int			g_hitDeathReactions_streaming;
	// ~Hit Death Reactions CVars

	SCVars()
	{
		memset(this,0,sizeof(SCVars));
	}

	~SCVars() { ReleaseCVars(); }

	void InitCVars(IConsole *pConsole);
	void ReleaseCVars();

	void InitAIPerceptionCVars(IConsole *pConsole);
	void ReleaseAIPerceptionCVars(IConsole* pConsole);
};

#endif //__GAMECVARS_H__
