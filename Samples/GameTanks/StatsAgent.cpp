////////////////////////////////////////////////////////////////////////////
//
//  CryEngine Source File.
//  Copyright (C), Crytek, 1999-2009.
// -------------------------------------------------------------------------
//  File name:   StatsAgent.cpp
//  Version:     v1.00
//  Created:     06/10/2009 by Steve Barnett.
//  Description: This the declaration for CStatsAgent
// -------------------------------------------------------------------------
//  History:
//
////////////////////////////////////////////////////////////////////////////

#include "StdAfx.h"

#include "ProjectDefines.h"
#include "ICmdLine.h"

#if defined(ENABLE_STATS_AGENT)

#include "StatsAgent.h"

#	include <cstdlib>
#	include <cstring>







bool CStatsAgent::s_pipeOpen = false;









void CStatsAgent::CreatePipe( const ICmdLineArg* pPipeName )
{


























}

void CStatsAgent::ClosePipe( void )
{
	if ( s_pipeOpen  )
	{





		s_pipeOpen = false;
	}
}

void CStatsAgent::Update( void )
{
	if ( s_pipeOpen )
	{























































































	}
}







































































































#endif	// defined(USE_STATS_AGENT)
