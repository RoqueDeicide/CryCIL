#include "stdafx.h"

#include "CheckingBasics.h"

#define _KI_KEYBOARD_BASE	0
#define _KI_MOUSE_BASE			256
#define _KI_XINPUT_BASE		512
#define _KI_ORBIS_BASE				1024
#define _KI_SYS_BASE				2048

TYPE_MIRROR enum KeyId
{
	eKI_Escape_check = KI_KEYBOARD_BASE,
	eKI_1_check,
	eKI_2_check,
	eKI_3_check,
	eKI_4_check,
	eKI_5_check,
	eKI_6_check,
	eKI_7_check,
	eKI_8_check,
	eKI_9_check,
	eKI_0_check,
	eKI_Minus_check,
	eKI_Equals_check,
	eKI_Backspace_check,
	eKI_Tab_check,
	eKI_Q_check,
	eKI_W_check,
	eKI_E_check,
	eKI_R_check,
	eKI_T_check,
	eKI_Y_check,
	eKI_U_check,
	eKI_I_check,
	eKI_O_check,
	eKI_P_check,
	eKI_LBracket_check,
	eKI_RBracket_check,
	eKI_Enter_check,
	eKI_LCtrl_check,
	eKI_A_check,
	eKI_S_check,
	eKI_D_check,
	eKI_F_check,
	eKI_G_check,
	eKI_H_check,
	eKI_J_check,
	eKI_K_check,
	eKI_L_check,
	eKI_Semicolon_check,
	eKI_Apostrophe_check,
	eKI_Tilde_check,
	eKI_LShift_check,
	eKI_Backslash_check,
	eKI_Z_check,
	eKI_X_check,
	eKI_C_check,
	eKI_V_check,
	eKI_B_check,
	eKI_N_check,
	eKI_M_check,
	eKI_Comma_check,
	eKI_Period_check,
	eKI_Slash_check,
	eKI_RShift_check,
	eKI_NP_Multiply_check,
	eKI_LAlt_check,
	eKI_Space_check,
	eKI_CapsLock_check,
	eKI_F1_check,
	eKI_F2_check,
	eKI_F3_check,
	eKI_F4_check,
	eKI_F5_check,
	eKI_F6_check,
	eKI_F7_check,
	eKI_F8_check,
	eKI_F9_check,
	eKI_F10_check,
	eKI_NumLock_check,
	eKI_ScrollLock_check,
	eKI_NP_7_check,
	eKI_NP_8_check,
	eKI_NP_9_check,
	eKI_NP_Substract_check,
	eKI_NP_4_check,
	eKI_NP_5_check,
	eKI_NP_6_check,
	eKI_NP_Add_check,
	eKI_NP_1_check,
	eKI_NP_2_check,
	eKI_NP_3_check,
	eKI_NP_0_check,
	eKI_F11_check,
	eKI_F12_check,
	eKI_F13_check,
	eKI_F14_check,
	eKI_F15_check,
	eKI_Colon_check,
	eKI_Underline_check,
	eKI_NP_Enter_check,
	eKI_RCtrl_check,
	eKI_NP_Period_check,
	eKI_NP_Divide_check,
	eKI_Print_check,
	eKI_RAlt_check,
	eKI_Pause_check,
	eKI_Home_check,
	eKI_Up_check,
	eKI_PgUp_check,
	eKI_Left_check,
	eKI_Right_check,
	eKI_End_check,
	eKI_Down_check,
	eKI_PgDn_check,
	eKI_Insert_check,
	eKI_Delete_check,
	eKI_LWin_check,
	eKI_RWin_check,
	eKI_Apps_check,
	eKI_OEM_102_check,

	// Mouse.
	eKI_Mouse1_check = KI_MOUSE_BASE,
	eKI_Mouse2_check,
	eKI_Mouse3_check,
	eKI_Mouse4_check,
	eKI_Mouse5_check,
	eKI_Mouse6_check,
	eKI_Mouse7_check,
	eKI_Mouse8_check,
	eKI_MouseWheelUp_check,
	eKI_MouseWheelDown_check,
	eKI_MouseX_check,
	eKI_MouseY_check,
	eKI_MouseZ_check,
	eKI_MouseXAbsolute_check,
	eKI_MouseYAbsolute_check,
	eKI_MouseLast_check,

	// XBox controller.
	eKI_XI_DPadUp_check = KI_XINPUT_BASE,
	eKI_XI_DPadDown_check,
	eKI_XI_DPadLeft_check,
	eKI_XI_DPadRight_check,
	eKI_XI_Start_check,
	eKI_XI_Back_check,
	eKI_XI_ThumbL_check,
	eKI_XI_ThumbR_check,
	eKI_XI_ShoulderL_check,
	eKI_XI_ShoulderR_check,
	eKI_XI_A_check,
	eKI_XI_B_check,
	eKI_XI_X_check,
	eKI_XI_Y_check,
	eKI_XI_TriggerL_check,
	eKI_XI_TriggerR_check,
	eKI_XI_ThumbLX_check,
	eKI_XI_ThumbLY_check,
	eKI_XI_ThumbLUp_check,
	eKI_XI_ThumbLDown_check,
	eKI_XI_ThumbLLeft_check,
	eKI_XI_ThumbLRight_check,
	eKI_XI_ThumbRX_check,
	eKI_XI_ThumbRY_check,
	eKI_XI_ThumbRUp_check,
	eKI_XI_ThumbRDown_check,
	eKI_XI_ThumbRLeft_check,
	eKI_XI_ThumbRRight_check,
	eKI_XI_TriggerLBtn_check,
	eKI_XI_TriggerRBtn_check,
	eKI_XI_Connect_check,			// should be deprecated because all devices can be connected, use eKI_SYS_ConnectDevice instead
	eKI_XI_Disconnect_check,	// should be deprecated because all devices can be disconnected, use eKI_SYS_DisconnectDevice instead

	// Orbis controller.
	eKI_Orbis_Select_check = KI_ORBIS_BASE,
	eKI_Orbis_L3_check,
	eKI_Orbis_R3_check,
	eKI_Orbis_Start_check,
	eKI_Orbis_Up_check,
	eKI_Orbis_Right_check,
	eKI_Orbis_Down_check,
	eKI_Orbis_Left_check,
	eKI_Orbis_L2_check,
	eKI_Orbis_R2_check,
	eKI_Orbis_L1_check,
	eKI_Orbis_R1_check,
	eKI_Orbis_Triangle_check,
	eKI_Orbis_Circle_check,
	eKI_Orbis_Cross_check,
	eKI_Orbis_Square_check,
	eKI_Orbis_StickLX_check,
	eKI_Orbis_StickLY_check,
	eKI_Orbis_StickRX_check,
	eKI_Orbis_StickRY_check,
	eKI_Orbis_RotX_check,
	eKI_Orbis_RotY_check,
	eKI_Orbis_RotZ_check,
	eKI_Orbis_RotX_KeyL_check,
	eKI_Orbis_RotX_KeyR_check,
	eKI_Orbis_RotZ_KeyD_check,
	eKI_Orbis_RotZ_KeyU_check,

	// Orbis specific
	eKI_Orbis_Touch_check,

	// Normal inputs should be added above
	// eKI_SYS_COMMIT and below will be ignored by input blocking functionality
	eKI_SYS_Commit_check = KI_SYS_BASE,
	eKI_SYS_ConnectDevice_check,
	eKI_SYS_DisconnectDevice_check,

	// Terminator.
	eKI_Unknown_check = 0xffffffff
};

#define CHECK_DEFINE(x, y) static_assert (x == y, #x" define has been changed.")
#define CHECK_ENUM(x) static_assert (KeyId::x ## _check == EKeyId::x, "EKeyId enumeration has been changed.")

inline void Check()
{
	CHECK_DEFINE(KI_KEYBOARD_BASE, _KI_KEYBOARD_BASE);
	CHECK_DEFINE(KI_MOUSE_BASE,    _KI_MOUSE_BASE);
	CHECK_DEFINE(KI_XINPUT_BASE,   _KI_XINPUT_BASE);
	CHECK_DEFINE(KI_ORBIS_BASE,    _KI_ORBIS_BASE);
	CHECK_DEFINE(KI_SYS_BASE,      _KI_SYS_BASE);

	CHECK_ENUM(eKI_Escape);
	CHECK_ENUM(eKI_1);
	CHECK_ENUM(eKI_2);
	CHECK_ENUM(eKI_3);
	CHECK_ENUM(eKI_4);
	CHECK_ENUM(eKI_5);
	CHECK_ENUM(eKI_6);
	CHECK_ENUM(eKI_7);
	CHECK_ENUM(eKI_8);
	CHECK_ENUM(eKI_9);
	CHECK_ENUM(eKI_0);
	CHECK_ENUM(eKI_Minus);
	CHECK_ENUM(eKI_Equals);
	CHECK_ENUM(eKI_Backspace);
	CHECK_ENUM(eKI_Tab);
	CHECK_ENUM(eKI_Q);
	CHECK_ENUM(eKI_W);
	CHECK_ENUM(eKI_E);
	CHECK_ENUM(eKI_R);
	CHECK_ENUM(eKI_T);
	CHECK_ENUM(eKI_Y);
	CHECK_ENUM(eKI_U);
	CHECK_ENUM(eKI_I);
	CHECK_ENUM(eKI_O);
	CHECK_ENUM(eKI_P);
	CHECK_ENUM(eKI_LBracket);
	CHECK_ENUM(eKI_RBracket);
	CHECK_ENUM(eKI_Enter);
	CHECK_ENUM(eKI_LCtrl);
	CHECK_ENUM(eKI_A);
	CHECK_ENUM(eKI_S);
	CHECK_ENUM(eKI_D);
	CHECK_ENUM(eKI_F);
	CHECK_ENUM(eKI_G);
	CHECK_ENUM(eKI_H);
	CHECK_ENUM(eKI_J);
	CHECK_ENUM(eKI_K);
	CHECK_ENUM(eKI_L);
	CHECK_ENUM(eKI_Semicolon);
	CHECK_ENUM(eKI_Apostrophe);
	CHECK_ENUM(eKI_Tilde);
	CHECK_ENUM(eKI_LShift);
	CHECK_ENUM(eKI_Backslash);
	CHECK_ENUM(eKI_Z);
	CHECK_ENUM(eKI_X);
	CHECK_ENUM(eKI_C);
	CHECK_ENUM(eKI_V);
	CHECK_ENUM(eKI_B);
	CHECK_ENUM(eKI_N);
	CHECK_ENUM(eKI_M);
	CHECK_ENUM(eKI_Comma);
	CHECK_ENUM(eKI_Period);
	CHECK_ENUM(eKI_Slash);
	CHECK_ENUM(eKI_RShift);
	CHECK_ENUM(eKI_NP_Multiply);
	CHECK_ENUM(eKI_LAlt);
	CHECK_ENUM(eKI_Space);
	CHECK_ENUM(eKI_CapsLock);
	CHECK_ENUM(eKI_F1);
	CHECK_ENUM(eKI_F2);
	CHECK_ENUM(eKI_F3);
	CHECK_ENUM(eKI_F4);
	CHECK_ENUM(eKI_F5);
	CHECK_ENUM(eKI_F6);
	CHECK_ENUM(eKI_F7);
	CHECK_ENUM(eKI_F8);
	CHECK_ENUM(eKI_F9);
	CHECK_ENUM(eKI_F10);
	CHECK_ENUM(eKI_NumLock);
	CHECK_ENUM(eKI_ScrollLock);
	CHECK_ENUM(eKI_NP_7);
	CHECK_ENUM(eKI_NP_8);
	CHECK_ENUM(eKI_NP_9);
	CHECK_ENUM(eKI_NP_Substract);
	CHECK_ENUM(eKI_NP_4);
	CHECK_ENUM(eKI_NP_5);
	CHECK_ENUM(eKI_NP_6);
	CHECK_ENUM(eKI_NP_Add);
	CHECK_ENUM(eKI_NP_1);
	CHECK_ENUM(eKI_NP_2);
	CHECK_ENUM(eKI_NP_3);
	CHECK_ENUM(eKI_NP_0);
	CHECK_ENUM(eKI_F11);
	CHECK_ENUM(eKI_F12);
	CHECK_ENUM(eKI_F13);
	CHECK_ENUM(eKI_F14);
	CHECK_ENUM(eKI_F15);
	CHECK_ENUM(eKI_Colon);
	CHECK_ENUM(eKI_Underline);
	CHECK_ENUM(eKI_NP_Enter);
	CHECK_ENUM(eKI_RCtrl);
	CHECK_ENUM(eKI_NP_Period);
	CHECK_ENUM(eKI_NP_Divide);
	CHECK_ENUM(eKI_Print);
	CHECK_ENUM(eKI_RAlt);
	CHECK_ENUM(eKI_Pause);
	CHECK_ENUM(eKI_Home);
	CHECK_ENUM(eKI_Up);
	CHECK_ENUM(eKI_PgUp);
	CHECK_ENUM(eKI_Left);
	CHECK_ENUM(eKI_Right);
	CHECK_ENUM(eKI_End);
	CHECK_ENUM(eKI_Down);
	CHECK_ENUM(eKI_PgDn);
	CHECK_ENUM(eKI_Insert);
	CHECK_ENUM(eKI_Delete);
	CHECK_ENUM(eKI_LWin);
	CHECK_ENUM(eKI_RWin);
	CHECK_ENUM(eKI_Apps);
	CHECK_ENUM(eKI_OEM_102);

	CHECK_ENUM(eKI_Mouse1);
	CHECK_ENUM(eKI_Mouse2);
	CHECK_ENUM(eKI_Mouse3);
	CHECK_ENUM(eKI_Mouse4);
	CHECK_ENUM(eKI_Mouse5);
	CHECK_ENUM(eKI_Mouse6);
	CHECK_ENUM(eKI_Mouse7);
	CHECK_ENUM(eKI_Mouse8);
	CHECK_ENUM(eKI_MouseWheelUp);
	CHECK_ENUM(eKI_MouseWheelDown);
	CHECK_ENUM(eKI_MouseX);
	CHECK_ENUM(eKI_MouseY);
	CHECK_ENUM(eKI_MouseZ);
	CHECK_ENUM(eKI_MouseXAbsolute);
	CHECK_ENUM(eKI_MouseYAbsolute);
	CHECK_ENUM(eKI_MouseLast);

	CHECK_ENUM(eKI_XI_DPadUp);
	CHECK_ENUM(eKI_XI_DPadDown);
	CHECK_ENUM(eKI_XI_DPadLeft);
	CHECK_ENUM(eKI_XI_DPadRight);
	CHECK_ENUM(eKI_XI_Start);
	CHECK_ENUM(eKI_XI_Back);
	CHECK_ENUM(eKI_XI_ThumbL);
	CHECK_ENUM(eKI_XI_ThumbR);
	CHECK_ENUM(eKI_XI_ShoulderL);
	CHECK_ENUM(eKI_XI_ShoulderR);
	CHECK_ENUM(eKI_XI_A);
	CHECK_ENUM(eKI_XI_B);
	CHECK_ENUM(eKI_XI_X);
	CHECK_ENUM(eKI_XI_Y);
	CHECK_ENUM(eKI_XI_TriggerL);
	CHECK_ENUM(eKI_XI_TriggerR);
	CHECK_ENUM(eKI_XI_ThumbLX);
	CHECK_ENUM(eKI_XI_ThumbLY);
	CHECK_ENUM(eKI_XI_ThumbLUp);
	CHECK_ENUM(eKI_XI_ThumbLDown);
	CHECK_ENUM(eKI_XI_ThumbLLeft);
	CHECK_ENUM(eKI_XI_ThumbLRight);
	CHECK_ENUM(eKI_XI_ThumbRX);
	CHECK_ENUM(eKI_XI_ThumbRY);
	CHECK_ENUM(eKI_XI_ThumbRUp);
	CHECK_ENUM(eKI_XI_ThumbRDown);
	CHECK_ENUM(eKI_XI_ThumbRLeft);
	CHECK_ENUM(eKI_XI_ThumbRRight);
	CHECK_ENUM(eKI_XI_TriggerLBtn);
	CHECK_ENUM(eKI_XI_TriggerRBtn);
	CHECK_ENUM(eKI_XI_Connect);
	CHECK_ENUM(eKI_XI_Disconnect);

	CHECK_ENUM(eKI_Orbis_Select);
	CHECK_ENUM(eKI_Orbis_L3);
	CHECK_ENUM(eKI_Orbis_R3);
	CHECK_ENUM(eKI_Orbis_Start);
	CHECK_ENUM(eKI_Orbis_Up);
	CHECK_ENUM(eKI_Orbis_Right);
	CHECK_ENUM(eKI_Orbis_Down);
	CHECK_ENUM(eKI_Orbis_Left);
	CHECK_ENUM(eKI_Orbis_L2);
	CHECK_ENUM(eKI_Orbis_R2);
	CHECK_ENUM(eKI_Orbis_L1);
	CHECK_ENUM(eKI_Orbis_R1);
	CHECK_ENUM(eKI_Orbis_Triangle);
	CHECK_ENUM(eKI_Orbis_Circle);
	CHECK_ENUM(eKI_Orbis_Cross);
	CHECK_ENUM(eKI_Orbis_Square);
	CHECK_ENUM(eKI_Orbis_StickLX);
	CHECK_ENUM(eKI_Orbis_StickLY);
	CHECK_ENUM(eKI_Orbis_StickRX);
	CHECK_ENUM(eKI_Orbis_StickRY);
	CHECK_ENUM(eKI_Orbis_RotX);
	CHECK_ENUM(eKI_Orbis_RotY);
	CHECK_ENUM(eKI_Orbis_RotZ);
	CHECK_ENUM(eKI_Orbis_RotX_KeyL);
	CHECK_ENUM(eKI_Orbis_RotX_KeyR);
	CHECK_ENUM(eKI_Orbis_RotZ_KeyD);
	CHECK_ENUM(eKI_Orbis_RotZ_KeyU);

	CHECK_ENUM(eKI_Orbis_Touch);

	CHECK_ENUM(eKI_SYS_Commit);
	CHECK_ENUM(eKI_SYS_ConnectDevice);
	CHECK_ENUM(eKI_SYS_DisconnectDevice);

	CHECK_ENUM(eKI_Unknown);
}