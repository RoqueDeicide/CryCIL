#pragma once

#include "IMonoInterface.h"

struct MonoField : public IMonoField
{
private:
	MonoClassField *field;
	MonoFieldAttributes attrs;
	unsigned int offset;
	IMonoClass *klass;
	const char *name;
public:

	MonoField(MonoClassField *f, IMonoClass *parent)
		: field(f)
		, klass(parent)
	{
		this->attrs  = MonoFieldAttributes(mono_field_get_flags(this->field));
		this->offset = mono_field_get_offset(this->field);
		this->name   = mono_field_get_name(this->field);
	}

	const char         *GetName() const override;
	IMonoClass         *GetDeclaringClass() const override;
	unsigned int        GetOffset() const override;
	MonoFieldAttributes GetAttributes() const override;
	void               *GetWrappedPointer() const override;
	void                Get(mono::object obj, void *value) const override;
	void                Set(mono::object obj, void *value) const override;
};