#pragma once

#include "IMonoInterface.h"

#include "IFlowSystem.h"

struct MonoFlowData
{
	union
	{
		int Integer32;
		float Float;
		uint EntityId;
		float Vector3[3];
		const char *text;
		bool Bool;
	};
	EFlowDataTypes DataType;

	MonoFlowData(TFlowInputData &data)
	{
		EFlowDataTypes type = data.GetType();
		this->DataType = type;
		switch (type)
		{
		case eFDT_Int:
			this->Integer32 = *data.GetPtr<int>();
			break;
		case eFDT_Float:
			this->Float = *data.GetPtr<float>();
			break;
		case eFDT_EntityId:
			this->EntityId = *data.GetPtr<unsigned int>();
			break;
		case eFDT_Vec3:
		{
			Vec3 vector = *data.GetPtr<Vec3>();
			this->Vector3[0] = vector.x;
			this->Vector3[1] = vector.y;
			this->Vector3[2] = vector.z;
			break;
		}
		case eFDT_String:
			this->text = *data.GetPtr<string>();
			break;
		case eFDT_Bool:
			this->Bool = *data.GetPtr<bool>();
			break;
		default:
			break;
		}
	}

	TFlowInputData ToInputData()
	{
		TFlowInputData data;

		switch (this->DataType)
		{
		case EFlowDataTypes::eFDT_Bool:
			data = TFlowInputData(this->Bool, true);
			break;
		case EFlowDataTypes::eFDT_EntityId:
			data = TFlowInputData(this->EntityId, true);
			break;
		case EFlowDataTypes::eFDT_Float:
			data = TFlowInputData(this->Float, true);
			break;
		case EFlowDataTypes::eFDT_Int:
			data = TFlowInputData(this->Integer32, true);
			break;
		case EFlowDataTypes::eFDT_String:
			data = TFlowInputData(string(this->text), true);
			break;
		case EFlowDataTypes::eFDT_Vec3:
		{
			Vec3 v = *(Vec3 *)this->Vector3;
			data = TFlowInputData(v, true);
			break;
		}
		case EFlowDataTypes::eFDT_Void:
			data = TFlowInputData(SFlowSystemVoid(), true);
			break;
		default:
			break;
		}
		return data;
	}
};

struct MonoFlowNode : public IFlowNode
{
private:
	int refCount;
	MonoGCHandle objHandle;
	bool targetsEntity;
public:
	MonoFlowNode(TFlowNodeTypeId typeId, IFlowNode::SActivationInfo *info, bool &cancel);
	//! Signals managed object to release the resources it held.
	virtual ~MonoFlowNode();

	//! Reference counting.
	virtual void AddRef() { this->refCount++; }
	//! Reference counting.
	//!
	//! We delete this node when the flow graph releases it completely: we are not supposed to keep any references to it
	//! from the managed code.
	virtual void Release() { if (--this->refCount <= 0) delete this; }

	virtual IFlowNodePtr Clone(SActivationInfo *actInfo) { return this; }

	virtual void GetConfiguration(SFlowNodeConfig&);

	virtual bool SerializeXML(SActivationInfo *actInfo, const XmlNodeRef& root, bool reading);
	virtual void Serialize(SActivationInfo *actInfo, TSerialize ser);
	virtual void PostSerialize(SActivationInfo *actInfo);

	virtual void ProcessEvent(EFlowEvent event, SActivationInfo *actInfo);

	virtual void GetMemoryUsage(ICrySizer * s) const;

};