#pragma once

// Defines types that can be used as headers for various arrays of data.

//! Represents an object that contains information about a reference-counted array of data.
//!
//! @typeparam DataType           Type of data that is contained in the array.
//! @typeparam ReferenceCountType Type of object that represents a number of live references to the array.
//! @typeparam CountType          Type of objects that represent number of objects in the array.
//! @typeparam nullTerminated     Indicates whether this array is terminated by a special 'null' object.
template<typename DataType, typename ReferenceCountType, typename CountType, bool nullTerminated>
class RefCountedImmutableArrayHeaderTemplate
{
	ReferenceCountType referenceCount;
	CountType          length;
	CountType          capacity;
	public:
		//! Gets the number of live references to the data that is associated with this header.
		__declspec(property(get = GetReferenceCount, put = SetReferenceCount)) ReferenceCountType ReferenceCount;
		ReferenceCountType GetReferenceCount() const
		{
			return this->referenceCount;
		}
		void SetReferenceCount(ReferenceCountType value)
		{
			this->referenceCount = value;
		}
		//! Gets or sets the number of valid objects that are stored in the array.
		__declspec(property(get = GetLength, put = SetLength)) CountType Length;
		CountType GetLength() const
		{
			return this->length;
		}
		void SetLength(CountType value)
		{
			this->length = value;
		}
		//! Gets or sets the number of objects that can fit into memory block that was allocated for this array.
		__declspec(property(get = GetCapacity, put = SetCapacity)) CountType Capacity;
		CountType GetCapacity() const
		{
			return this->capacity;
		}
		void SetCapacity(CountType value)
		{
			this->capacity = value;
		}
		//! Gets number of bytes that this array is taking up in the heap.
		__declspec(property(get = GetAllocatedMemory)) size_t AllocatedMemory;
		size_t GetAllocatedMemory() const
		{
			size_t n = sizeof(DataType);
			n *= this->capacity;
			if (nullTerminated)
			{
				n += sizeof(DataType);
			}
			n += sizeof(RefCountedImmutableArrayHeaderTemplate);
			return n;
		}
		//! Gets the pointer to the first element in the array.
		__declspec(property(get = GetElements)) DataType *Elements;
		DataType *GetElements()
		{
			return reinterpret_cast<DataType *>(this + 1);
		}
		//! Gets the pointer to the first element in the array.
		__declspec(property(get = GetElements)) const DataType *ElementsRO;
		const DataType *GetElements() const
		{
			return reinterpret_cast<DataType *>(this + 1);
		}

		//! Increases number of live references to the array.
		void RegisterReference()
		{
#ifdef CRYCIL_MODULE
			CryInterlockedIncrement(&this->referenceCount);
#else
			this->referenceCount++;
#endif // CRYCIL_MODULE

		}
		//! Decreases number of live references to the array.
		//!
		//! @returns Number of references to the array after decrementing the count.
		int UnregisterReference()
		{
#ifdef CRYCIL_MODULE
			return CryInterlockedDecrement(&this->referenceCount);
#else
			return --this->referenceCount;
#endif // CRYCIL_MODULE
		}

		//! Gets the pointer to the global variable that represents an empty array.
		//!
		//! This function can be used with empty objects to avoid using extra memory for them.
		static RefCountedImmutableArrayHeaderTemplate *EmptyHeader();
};

//! Represents an empty array.
template<typename DataType, typename ReferenceCountType, typename CountType, bool nullTerminated>
struct EmptyArrayObject
{
	typedef RefCountedImmutableArrayHeaderTemplate<DataType, ReferenceCountType, CountType, true> HeaderType;

	HeaderType header;

	EmptyArrayObject()
	{
		memset(this, 0, sizeof(EmptyArrayObject));
		this->header.UnregisterReference();         // This sets the reference count to -1.
	}

	HeaderType *GetHeader()
	{
		return &this->header;
	}
};

//! Represents an empty null-terminated array.
template<typename DataType, typename ReferenceCountType, typename CountType>
struct EmptyArrayObject<DataType, ReferenceCountType, CountType, true>
{
	typedef RefCountedImmutableArrayHeaderTemplate<DataType, ReferenceCountType, CountType, true> HeaderType;

	HeaderType header;
	DataType   terminator;

	EmptyArrayObject()
	{
		memset(this, 0, sizeof(EmptyArrayObject));
		this->header.UnregisterReference();         // This sets the reference count to -1.
	}

	HeaderType *GetHeader()
	{
		return &this->header;
	}
};

template<typename DataType, typename ReferenceCountType, typename CountType, bool nullTerminated>
inline RefCountedImmutableArrayHeaderTemplate<DataType, ReferenceCountType, CountType, nullTerminated> *
RefCountedImmutableArrayHeaderTemplate<DataType, ReferenceCountType, CountType, nullTerminated>::EmptyHeader()
{
	static EmptyArrayObject<DataType, ReferenceCountType, CountType, nullTerminated> emptyArray;
	return emptyArray.GetHeader();
}

template<typename DataType>
using SimpleArrayHeader = RefCountedImmutableArrayHeaderTemplate<DataType, int, size_t, false>;
template<typename DataType>
using NullTerminatedArrayHeader = RefCountedImmutableArrayHeaderTemplate<DataType, int, size_t, true>;