#include "stdafx.h"
#include "API_ImplementationHeaders.h"

typedef struct
{
	unsigned int length;
	int lower_bound;
} MonoArrayBounds;

struct _MonoArray
{
	MonoObject obj;
	/* bounds is NULL for szarrays */
	MonoArrayBounds *bounds;
	/* total number of elements of the array */
	unsigned int max_length;
	/* we use double to ensure proper alignment on platforms that need it */
	double vector[MONO_ZERO_LEN_ARRAY];
};

void *MonoArrayWrapper::Item(int index)
{
	return mono_array_addr_with_size(GetWrappedArray, this->GetElementSize(), index);
}

int MonoArrayWrapper::GetLength(int dimensionIndex)
{
	if (!this->arrayPtr->bounds)
	{
		return this->arrayPtr->max_length;
	}
	return this->arrayPtr->bounds[dimensionIndex].length;
}

int MonoArrayWrapper::GetLowerBound(int dimensionIndex)
{
	if (!this->arrayPtr->bounds)
	{
		return 0;
	}
	return this->arrayPtr->bounds[dimensionIndex].lower_bound;
}

int MonoArrayWrapper::GetRank()
{
	return mono_class_get_rank(mono_object_get_class((MonoObject *)this->arrayPtr));
}


//! Returns number of elements in the array.
int MonoArrayWrapper::GetSize()
{
	return this->arrayPtr->max_length;
}
//! Returns the size of one element in the memory.
int MonoArrayWrapper::GetElementSize()
{
	if (this->elementSize == -1)
	{
		this->elementSize =
			mono_array_element_size(mono_object_get_class((MonoObject *)this->GetWrappedPointer()));
	}
	return this->elementSize;
}
//! Gets the wrapper for class that represents elements of the array.
IMonoClass *MonoArrayWrapper::GetElementClass()
{
	if (this->klass == nullptr)
	{
		this->klass = MonoClassCache::Wrap
		(
			mono_class_get_element_class
			(
				mono_object_get_class((MonoObject *)this->GetWrappedPointer())
			)
		);
	}
	return this->klass;
}

void *MonoArrayWrapper::GetWrappedPointer()
{
	return this->arrayPtr;
}

mono::object MonoArrayWrapper::Get()
{
	return (mono::object)this->arrayPtr;
}

mono::object MonoArrayWrapper::CallMethod(const char *name, IMonoArray *args)
{
	MonoObject *obj = (MonoObject *)this->Get();
	MonoMethod *method =
		mono_class_get_method_from_name(mono_object_get_class((MonoObject *)this->arrayPtr), name, args->Length);
	MonoObject *exception = nullptr;
	MonoObject *result = mono_runtime_invoke_array
		(method, obj, (MonoArray *)args->GetWrappedPointer(), &exception);
	if (exception)
	{
		MonoEnv->HandleException((mono::object)exception);
	}
	else
	{
		return (mono::object)result;
	}
	return nullptr;
}

void MonoArrayWrapper::GetField(const char *name, void *value)
{
	return this->GetClass()->GetField(this->Get(), name, value);
}

void MonoArrayWrapper::SetField(const char *name, void *value)
{
	this->GetClass()->SetField(this->Get(), name, value);
}

IMonoProperty *MonoArrayWrapper::GetProperty(const char *name)
{
	return this->GetClass()->GetProperty(name);
}

IMonoEvent *MonoArrayWrapper::GetEvent(const char *name)
{
	return this->GetClass()->GetEvent(name);
}

IMonoClass *MonoArrayWrapper::GetClass()
{
	return MonoClassCache::Wrap(mono_object_get_class((MonoObject *)this->arrayPtr));
}

void *MonoArrayWrapper::UnboxObject()
{
	gEnv->pLog->LogError("Attempt to unbox an array object was made.");
	return nullptr;
}
