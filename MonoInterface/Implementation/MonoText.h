#pragma once

#include "IMonoInterface.h"
#include "MonoClass.h"

struct MonoText : public IMonoText
{
private:
	union
	{
		MonoObject *obj;
		MonoString *str;
		mono::string mStr;
	};
	IMonoClass *klass;
public:

	MonoText(MonoString *text)
	{
		this->str = text;
		this->klass = MonoClassCache::Wrap(mono_object_get_class(this->obj));
	}

	MonoText(mono::string text)
	{
		this->mStr = text;
		this->klass = MonoClassCache::Wrap(mono_object_get_class(this->obj));
	}

	virtual bool Equals(IMonoText *other);

	virtual bool Equals(mono::string other);

	virtual void Intern();

	virtual wchar_t &At(int index);

	virtual int GetHashCode();

	virtual bool IsInterned();

	virtual mono::object Get();

	virtual void GetField(const char *name, void *value);

	virtual void SetField(const char *name, void *value);

	virtual IMonoProperty *GetProperty(const char *name);

	virtual IMonoEvent *GetEvent(const char *name);

	virtual IMonoClass *GetClass();

	virtual void *GetWrappedPointer();

	virtual const char *ToNativeUTF8();

	virtual const wchar_t *ToNativeUTF16();

	virtual void Update(mono::object newLocation);

};