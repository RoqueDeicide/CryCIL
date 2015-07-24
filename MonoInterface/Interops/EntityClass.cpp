#include "stdafx.h"

#include "EntityClass.h"
#include "EntityExtension.h"

void MonoEntityPropertyHandler::LoadEntityXMLProperties(IEntity* entity, const XmlNodeRef& xml)
{
	XmlNodeRef props = xml->findChild("Properties");
	if (!props)
	{
		return;
	}

	int propCount = props->getNumAttributes();
	for (int i = 0; i < propCount; i++)
	{
		const char *name;
		const char *value;

		props->getAttributeByIndex(i, &name, &value);

		int index = this->definedProperties.IndexOf([name](MonoEntityProperty p) { return strcmp(p.info.name, name) != 0; });
		if (index == -1)
		{
			gEnv->pLog->LogWarning("An unknown entity property [%s] was found in Xml node for entity of type %s",
								   name, entity->GetClass()->GetName());
		}
		else
		{
			this->SetProperty(entity, index, value);
		}
	}
}

int MonoEntityPropertyHandler::GetPropertyCount() const
{
	return this->definedProperties.Length;
}

bool MonoEntityPropertyHandler::GetPropertyInfo(int index, SPropertyInfo &info) const
{
	if (index < 0 || index >= this->definedProperties.Length)
	{
		return false;
	}

	info = this->definedProperties[index].info;
	return true;
}

void MonoEntityPropertyHandler::SetProperty(IEntity *entity, int index, const char *value)
{
	if (index < 0 || index > this->definedProperties.Length)
	{
		return;
	}

	EntityId id = entity->GetId();
	auto ext = QueryMonoEntityExtension(entity, id);

	IEntityPropertyHandler::SPropertyInfo propInfo = this->definedProperties[index].info;

	if (ext && ext->IsInitialized())
	{
		ext->SetPropertyValue(index, value);
	}
	else
	{
		// The game hasn't started yet, so we have to save the value for later.
		if (!this->queuedProperties.Contains(id))
		{
			this->queuedProperties.Add(id, List<QueuedProperty>(10));
		}
		List<QueuedProperty> &props = this->queuedProperties.At(id);
		if (QueuedProperty *q = props.Find([&propInfo](QueuedProperty &prop) { return strcmp(prop.name, propInfo.name) == 0; }))
		{
			// The value for the property was already queued, so we just update it.
			q->value = value;
		}
		else
		{
			QueuedProperty qp;
			qp.name = propInfo.name;
			qp.value = value;
			props.Add(qp);
		}
	}
}

const char *MonoEntityPropertyHandler::GetProperty(IEntity *entity, int index) const
{
	if (!entity)
	{
		return "";
	}
	if (index < 0 || index >= this->definedProperties.Length)
	{
		return "";
	}

	MonoEntityExtension *ext = QueryMonoEntityExtension(entity);
	if (!ext)
	{
		return "";
	}

	return ext->GetPropertyValue(index);
}

const char *MonoEntityPropertyHandler::GetDefaultProperty(int index) const
{
	if (index < 0 || index >= this->definedProperties.Length)
	{
		return "";
	}

	return this->definedProperties[index].defaultValue;
}