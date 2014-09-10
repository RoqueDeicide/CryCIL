#ifndef __SCRIPTBIND_TIME_H__
#define __SCRIPTBIND_TIME_H__

#include <IMonoScriptBind.h>

class CScriptbind_Time : public IMonoScriptBind
{
public:
	CScriptbind_Time()
	{
		REGISTER_METHOD(SetTimeScale);
	}

	~CScriptbind_Time() {}

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