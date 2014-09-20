#ifndef __MONOCVARS_H__
#define __MONOCVARS_H__

struct SCVars
{
	int mono_exceptionsTriggerMessageBoxes;
	int mono_exceptionsTriggerFatalErrors;

	int mono_generateMdbIfPdbIsPresent;

	int mono_entityDeleteExtensionOnNetworkBindFailure;

	int mono_log;

	SCVars()
	{
		memset(this, 0, sizeof(SCVars));
		InitCVars(gEnv->pConsole);
	}

	~SCVars() { ReleaseCVars(); }

	void InitCVars(IConsole *pConsole);
	void ReleaseCVars();
};

#endif //__MONOCVARS_H__