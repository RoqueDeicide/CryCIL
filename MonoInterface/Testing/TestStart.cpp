#include "stdafx.h"

IMonoAssembly *mainTestingAssembly;

#include "TestStart.h"
#include "TestAssemblies.h"
#include "TestClasses.h"

void BeginTheTest()
{

	CryLogAlways("TEST: ");

	TestAssemblies();

	TestClasses();


}