/*************************************************************************
Crytek Source File.
Copyright (C), Crytek Studios, 2001-2004.
-------------------------------------------------------------------------
DesignerWarning.h
	- basic message box in windows to allow designers to be told something is wrong with their setup

-	[10/11/2009] : Created by James Bamford

*************************************************************************/

#ifndef __DESIGNER_WARNING_H__
#define __DESIGNER_WARNING_H__

//#define DESIGNER_WARNING_ENABLED (1 && (defined(WIN32) || defined(WIN64)))		
#if defined(WIN32) 
#define DESIGNER_WARNING_ENABLED 1  // needs a release build define to hook in here 
#elif defined(WIN64)
#define DESIGNER_WARNING_ENABLED 1  // needs a release build define to hook in here 
#else
#define DESIGNER_WARNING_ENABLED 0
#endif

#if DESIGNER_WARNING_ENABLED

#define DesignerWarning(cond, ...) ((!(cond)) && DesignerWarningFail(#cond, string().Format(__VA_ARGS__).c_str()))
#define DesignerWarningFail(condText, messageText) DesignerWarningFunc(string().Format("CONDITION:\n%s\n\nMESSAGE:\n%s", condText, messageText))
int DesignerWarningFunc(const char * message);
int GetNumDesignerWarningsHit();

#else // DESIGNER_WARNING_ENABLED

#define DesignerWarning(cond, ...) (0)
#define DesignerWarningFail(condText, messageText) (0)
#define GetNumDesignerWarningsHit() (0)

#endif // DESIGNER_WARNING_ENABLED

#endif // __DESIGNER_WARNING_H__
