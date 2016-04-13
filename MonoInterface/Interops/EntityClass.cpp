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

		bool set = false;
		for (size_t j = 0; j < this->definedProperties.Length; j++)
		{
			if (strcmp(this->definedProperties[j].info.name, name) == 0)
			{
				this->SetProperty(entity, j, value);
				set = true;
				break;
			}
		}
		if (!set)
		{
			gEnv->pLog->LogWarning("An unknown entity property [%s] was found in Xml node for entity of type %s",
								   name, entity->GetClass()->GetName());
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
		bool queued = false;
		for (auto &current : props)
		{
			if (strcmp(current.name, propInfo.name) == 0)
			{
				current.value = value;
				queued = true;
				break;
			}
		}
		if (!queued)
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