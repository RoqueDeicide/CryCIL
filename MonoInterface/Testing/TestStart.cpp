#include "stdafx.h"

const IMonoAssembly *mainTestingAssembly;

#include "TestStart.h"
#include "TestAssemblies.h"
#include "TestClasses.h"
#include "TestObjects.h"

void BeginTheTest()
{
	CryLogAlways("TEST:");

	TestAssemblies();

	TestClasses();

	TestObjects();
}