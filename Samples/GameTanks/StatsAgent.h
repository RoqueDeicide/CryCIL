////////////////////////////////////////////////////////////////////////////
//
//  CryEngine Source File.
//  Copyright (C), Crytek, 1999-2009.
// -------------------------------------------------------------------------
//  File name:   StatsAgent.h
//  Version:     v1.00
//  Created:     06/10/2009 by Steve Barnett.
//  Description: This the declaration for CStatsAgent
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////

#ifndef __SCRIPTUTILS_H__
#define __SCRIPTUTILS_H__

#pragma once

class ICmdLineArg;

class CStatsAgent
{
public:
	static void CreatePipe( const ICmdLineArg* pPipeName );
	static void ClosePipe( void );
	static void Update( void );

protected:
	static bool s_pipeOpen;









private:
	CStatsAgent( void ) {} // Prevent instantiation

};

#endif
