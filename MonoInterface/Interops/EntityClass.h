#pragma once

#include "IMonoInterface.h"

#include <IEntityClass.h>
#include <IGameObjectSystem.h>

//! Represents implementation of IEntityEventHandler type that doesn't do anything.
//!
//! Entity events lost their importance since FlowGraph was introduced.
struct NullEntityEventHandler : public IEntityEventHandler
{
	virtual void RefreshEvents() override {}
	virtual void LoadEntityXMLEvents(IEntity* entity, const XmlNodeRef& xml) override {}
	virtual int GetEventCount() const override { return 0; }
	virtual bool GetEventInfo(int index, SEventInfo& info) const override { return true; }
	virtual void SendEvent(IEntity* entity, const char* eventName) override {}
};
//! Represents a value of an entity property that should be assigned to the entity when it's created.
struct QueuedProperty
{
	const char *name;			//!< Name of the property.
	int type;					//!< Type of the property.
	const char *value;			//!< Text representation of the value of the property.
	int index;					//!< Index of the property for fast access.
};
//! Represents information about an entity property with default value.
struct MonoEntityProperty
{
	IEntityPropertyHandler::SPropertyInfo info;		//!< Standard CryEngine entity property info object.
	const char *defaultValue;						//!< Default value of the property.
	//! Default constructor.
	MonoEntityProperty()
		: defaultValue("")
	{
	}
	//! Move constructor.
	MonoEntityProperty(MonoEntityProperty &&other)
		: info(std::move(other.info))
		, defaultValue(std::move(other.defaultValue))
	{}
	//! Copy constructor.
	MonoEntityProperty(const MonoEntityProperty &other)
		: info(other.info)
		, defaultValue(other.defaultValue)
	{}
	//! Swap constructor.
	MonoEntityProperty(MonoEntityProperty &other)
	{
		std::swap(this->info, other.info);
		std::swap(this->defaultValue, other.defaultValue);
	}
	SWAP_ASSIGNMENT MonoEntityProperty &operator =(MonoEntityProperty &other)
	{
		std::swap(this->info, other.info);
		std::swap(this->defaultValue, other.defaultValue);
	}
	~MonoEntityProperty()
	{
		SAFE_DELETE(this->info.description);
		SAFE_DELETE(this->info.editType);
		SAFE_DELETE(this->info.name);
		SAFE_DELETE(this->defaultValue);
	}
};
//! Manages editor properties for CryCIL entities.
//!
//! Editor properties are the properties that can be assigned through Sandbox editor interface.
struct MonoEntityPropertyHandler : public IEntityPropertyHandler
{
private:
	//! This is a sorted list of property values that are will be assigned to the relevant entities when those are created.
	SortedList<EntityId, List<QueuedProperty>> queuedProperties;
	//! A list of properties defined for this entity class.
	List<MonoEntityProperty> definedProperties;
public:
	//! Creates new instance of this type.
	//!
	//! @param properties A list of properties that defined for the entity.
	MonoEntityPropertyHandler(List<MonoEntityProperty> properties)
		: definedProperties(properties)
	{
	}
	SortedList<EntityId, List<QueuedProperty>> *GetQueuedProperties() { return &this->queuedProperties; }
	virtual ~MonoEntityPropertyHandler() {}
	//! Doesn't do anything. Not sure what this method is supposed to do.
	virtual void RefreshProperties() override {}
	//! Loads properties from Xml and assigns them to an entity.
	virtual void LoadEntityXMLProperties(IEntity *entity, const XmlNodeRef& xml) override;
	//! Doesn't do anything.
	virtual void LoadArchetypeXMLProperties(const char *archetypeName, const XmlNodeRef &xml) override {}
	//! Doesn't do anything.
	virtual void InitArchetypeEntity(IEntity *entity, const char* archetypeName, const SEntitySpawnParams &spawnParams) override;
	//! Gets the number of properties defined in this entity class.
	virtual int GetPropertyCount() const override;
	//! Gets information about a property.
	//!
	//! @param index Zero-based index of the property to get.
	//! @param info  A reference to the object that will contain information about the property if this method concludes
	//!              successfully.
	//!
	//! @returns True, if this method concluded successfully.
	virtual bool GetPropertyInfo(int index, SPropertyInfo &info) const override;
	//! Assigns a value to the entity property.
	//!
	//! @param entity A pointer to the entity which property needs to be set.
	//! @param index  Zero-based index of the property to set.
	//! @param value  A new value for the property.
	virtual void SetProperty(IEntity *entity, int index, const char *value) override;
	//! Gets the value of the property.
	//!
	//! @param entity A pointer to the entity which property's value we need.
	//! @param index  Zero-based index of the property to get.
	//!
	//! @returns A text representation of the value of the property.
	virtual const char *GetProperty(IEntity *entity, int index) const override;
	//! Gets default value of the property.
	virtual const char *GetDefaultProperty(int index) const override;
	//! Returns 0.
	virtual uint32 GetScriptFlags() const override {}
	//! Doesn't do anything.
	virtual void PropertiesChanged(IEntity *entity) override {}

};

//! Provides additional data for initialization of CryCIL entities.
struct MonoEntityClassUserData : IGameObjectSystem::SEntitySpawnParamsForGameObjectWithPreactivatedExtension
{
	MonoEntityClassUserData()
	{
		this->m_type = eSpawnParamsType_Custom;
		this->hookFunction = CreateEntityAbstractionLayer;
	}

	static bool CreateEntityAbstractionLayer(IEntity *entity, IGameObject *gameObject, void *userData);
};