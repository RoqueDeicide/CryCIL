#ifndef __SCRIPTBIND_TIME_H__
#define __SCRIPTBIND_TIME_H__

#include <IMonoInterop.h>

class TimeInterop : public IMonoInterop
{
public:
	TimeInterop()
	{
		REGISTER_METHOD(SetTimeScale);
	}

	~TimeInterop() {}

protected:
	// IMonoScriptBind
	virtual const char *GetClassName() { return "TimeInterop"; }
	// ~IMonoScriptBind

	static void SetTimeScale(float scale)
	{
		gEnv->pTimer->SetTimeScale(scale);
	}
};

#endif //__SCRIPTBIND_TIME_H__