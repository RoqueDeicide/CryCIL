#include "stdafx.h"

IMonoAssembly *mainTestingAssembly;

#include "TestStart.h"
#include "TestAssemblies.h"

void BeginTheTest()
{

	CryLogAlways("TEST: ");

	TestAssemblies();


}