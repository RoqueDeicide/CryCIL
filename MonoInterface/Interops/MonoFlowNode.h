#pragma once

#include "IMonoInterface.h"

#include "CryFlowGraph/IFlowSystem.h"

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

	MonoFlowData()
		: DataType(EFlowDataTypes::eFDT_Void)
	{
	}
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
		case eFDT_Bool:
			data = TFlowInputData(this->Bool, true);
			break;
		case eFDT_EntityId:
			data = TFlowInputData(this->EntityId, true);
			break;
		case eFDT_Float:
			data = TFlowInputData(this->Float, true);
			break;
		case eFDT_Int:
			data = TFlowInputData(this->Integer32, true);
			break;
		case eFDT_String:
			data = TFlowInputData(string(this->text), true);
			break;
		case eFDT_Vec3:
		{
			Vec3 v = *reinterpret_cast<Vec3 *>(this->Vector3);
			data = TFlowInputData(v, true);
			break;
		}
		case eFDT_Void:
			data = TFlowInputData(SFlowSystemVoid(), true);
			break;
		default:
			break;
		}
		return data;
	}
};

//! Represents an abstraction layer between Flow System and CryCIL.
struct MonoFlowNode : public IFlowNode
{
private:
	int refCount;
	MonoGCHandle objHandle;
	bool targetsEntity;
	SFlowNodeConfig nodeConfig;
public:
	MonoFlowNode(TFlowNodeTypeId typeId, SActivationInfo *info, bool &cancel);
	//! Signals managed object to release the resources it held.
	virtual ~MonoFlowNode();

	//! Reference counting.
	virtual void AddRef() override { this->refCount++; }
	//! Reference counting.
	//!
	//! We delete this node when the flow graph releases it completely: we are not supposed to keep any references to it
	//! from the managed code.
	virtual void Release() override { if (--this->refCount <= 0) delete this; }

	virtual IFlowNodePtr Clone(SActivationInfo *) override { return this; }

	virtual void GetConfiguration(SFlowNodeConfig&) override;

	virtual bool SerializeXML(SActivationInfo *actInfo, const XmlNodeRef& root, bool reading) override;
	virtual void Serialize(SActivationInfo *actInfo, TSerialize ser) override;
	virtual void PostSerialize(SActivationInfo *actInfo) override;

	virtual void ProcessEvent(EFlowEvent event, SActivationInfo *actInfo) override;

	virtual void GetMemoryUsage(ICrySizer * s) const override;
	//! Deactivates the node, so it doesn't work anymore.
	//!
	//! This exists just in case Mono run-time gets shut down before flow graph system.
	void Deactivate() { this->objHandle.Separate(); }
};