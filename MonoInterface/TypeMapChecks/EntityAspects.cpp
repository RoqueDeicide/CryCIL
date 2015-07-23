#include "stdafx.h"

#include "CheckingBasics.h"

#include "IGameObject.h"

TYPE_MIRROR enum EntityAspects
{
	eEA_All_check = NET_ASPECT_ALL,
	// 0x01u                       // aspect 0
	eEA_Script_check = 0x02u, // aspect 1
	// 0x04u                       // aspect 2
	eEA_Physics_check = 0x08u, // aspect 3
	eEA_GameClientStatic_check = 0x10u, // aspect 4
	eEA_GameServerStatic_check = 0x20u, // aspect 5
	eEA_GameClientDynamic_check = 0x40u, // aspect 6
	eEA_GameServerDynamic_check = 0x80u, // aspect 7
	eEA_GameClientA_check = 0x0100u, // aspect 8
	eEA_GameServerA_check = 0x0200u, // aspect 9
	eEA_GameClientB_check = 0x0400u, // aspect 10
	eEA_GameServerB_check = 0x0800u, // aspect 11
	eEA_GameClientC_check = 0x1000u, // aspect 12
	eEA_GameServerC_check = 0x2000u, // aspect 13
	eEA_GameClientD_check = 0x4000u, // aspect 14
	eEA_GameClientE_check = 0x8000u, // aspect 15
	eEA_GameClientF_check = 0x00010000u, // aspect 16
	eEA_GameClientG_check = 0x00020000u, // aspect 17
	eEA_GameClientH_check = 0x00040000u, // aspect 18
	eEA_GameClientI_check = 0x00080000u, // aspect 19
	eEA_GameClientJ_check = 0x00100000u, // aspect 20
	eEA_GameServerD_check = 0x00200000u, // aspect 21
	eEA_GameClientK_check = 0x00400000u, // aspect 22
	eEA_GameClientL_check = 0x00800000u, // aspect 23
	eEA_GameClientM_check = 0x01000000u, // aspect 24
	eEA_GameClientN_check = 0x02000000u, // aspect 25
	eEA_GameClientO_check = 0x04000000u, // aspect 26
	eEA_GameClientP_check = 0x08000000u, // aspect 27
	eEA_GameServerE_check = 0x10000000u, // aspect 28
	eEA_Aspect29_check = 0x20000000u, // aspect 29
	eEA_Aspect30_check = 0x40000000u, // aspect 30
	eEA_Aspect31_check = 0x80000000u, // aspect 31
};

#define CHECK_ENUM(x) static_assert (EntityAspects::x ## _check == EEntityAspects::x, "EEntityAspects enumeration has been changed.")

inline void Check()
{
	CHECK_ENUM(eEA_All);
	CHECK_ENUM(eEA_Script);
	CHECK_ENUM(eEA_Physics);
	CHECK_ENUM(eEA_GameClientStatic);
	CHECK_ENUM(eEA_GameServerStatic);
	CHECK_ENUM(eEA_GameClientDynamic);
	CHECK_ENUM(eEA_GameServerDynamic);
	CHECK_ENUM(eEA_GameClientA);
	CHECK_ENUM(eEA_GameServerA);
	CHECK_ENUM(eEA_GameClientB);
	CHECK_ENUM(eEA_GameServerB);
	CHECK_ENUM(eEA_GameClientC);
	CHECK_ENUM(eEA_GameServerC);
	CHECK_ENUM(eEA_GameClientD);
	CHECK_ENUM(eEA_GameClientE);
	CHECK_ENUM(eEA_GameClientF);
	CHECK_ENUM(eEA_GameClientG);
	CHECK_ENUM(eEA_GameClientH);
	CHECK_ENUM(eEA_GameClientI);
	CHECK_ENUM(eEA_GameClientJ);
	CHECK_ENUM(eEA_GameServerD);
	CHECK_ENUM(eEA_GameClientK);
	CHECK_ENUM(eEA_GameClientL);
	CHECK_ENUM(eEA_GameClientM);
	CHECK_ENUM(eEA_GameClientN);
	CHECK_ENUM(eEA_GameClientO);
	CHECK_ENUM(eEA_GameClientP);
	CHECK_ENUM(eEA_GameServerE);
	CHECK_ENUM(eEA_Aspect29);
	CHECK_ENUM(eEA_Aspect30);
	CHECK_ENUM(eEA_Aspect31);
}