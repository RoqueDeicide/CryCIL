#include "stdafx.h"
#include "CryMarshal.h"

Scriptbind_CryMarshal::Scriptbind_CryMarshal()
{
	REGISTER_METHOD(AllocateMemory);
	// Frees memory that has been allocated.
	REGISTER_METHOD(FreeMemory);
	// Gets one byte.
	REGISTER_METHOD(GetByte);
	// Gets two bytes.
	REGISTER_METHOD(Get2Byte);
	// Gets four bytes.
	REGISTER_METHOD(Get4Byte);
	// Gets eight bytes.
	REGISTER_METHOD(Get8Byte);
	// Hope you check for boundaries on your side, because none of that is done here.
	REGISTER_METHOD(Get32Bytes);
	REGISTER_METHOD(Get64Bytes);
	REGISTER_METHOD(Get128Bytes);
	REGISTER_METHOD(Get256Bytes);
	REGISTER_METHOD(Get512Bytes);

	REGISTER_METHOD(SetByte);
	REGISTER_METHOD(Set2Bytes);
	REGISTER_METHOD(Set4Bytes);
	REGISTER_METHOD(Set8Bytes);
	REGISTER_METHOD(Set32Bytes);
	REGISTER_METHOD(Set64Bytes);
	REGISTER_METHOD(Set128Bytes);
	REGISTER_METHOD(Set256Bytes);
	REGISTER_METHOD(Set512Bytes);

	REGISTER_METHOD(Set4BytesPartial);
	REGISTER_METHOD(Set8BytesPartial);
	REGISTER_METHOD(Set32BytesPartial);
	REGISTER_METHOD(Set64BytesPartial);
	REGISTER_METHOD(Set128BytesPartial);
	REGISTER_METHOD(Set256BytesPartial);
	REGISTER_METHOD(Set512BytesPartial);
}

Scriptbind_CryMarshal::~Scriptbind_CryMarshal()
{}

void * Scriptbind_CryMarshal::AllocateMemory(unsigned __int64 size)
{
#ifdef WIN64
	return malloc(size);
#else
	return malloc((unsigned int)size);
#endif
}

void * Scriptbind_CryMarshal::FreeMemory(void * pointer)
{
	free(pointer);
}

unsigned char Scriptbind_CryMarshal::GetByte(void * pointer, unsigned __int64 index)
{
	return ((unsigned char*)pointer)[index];
}

unsigned short Scriptbind_CryMarshal::Get2Byte(void * pointer, unsigned __int64 index)
{
	// What we do here and in other Get<NumberOfBytes>Bytes methods is this sequence of actions:
	// 1) Cast pointer to pointer to 1-byte long elements.
	// 2) Advance pointer to position we need.
	// 3) Cast pointer to pointer to type we want.
	// 4) Acquire value of the object pointer points to.
	// 5) Return that value.
	return *((unsigned short*)((unsigned char*)pointer + index));
}

unsigned long Scriptbind_CryMarshal::Get4Byte(void * pointer, unsigned __int64 index)
{
	return *((unsigned long*)((unsigned char*)pointer + index));
}

double Scriptbind_CryMarshal::Get8Byte(void * pointer, unsigned __int64 index)
{
	return *((double*)((unsigned char*)pointer + index));
}

Buffer32 Scriptbind_CryMarshal::Get32Bytes(void * pointer, unsigned __int64 shift)
{
	return *((Buffer32*)((unsigned char*)pointer + shift));
}

Buffer64 Scriptbind_CryMarshal::Get64Bytes(void * pointer, unsigned __int64 shift)
{
	return *((Buffer64*)((unsigned char*)pointer + shift));
}

Buffer128 Scriptbind_CryMarshal::Get128Bytes(void * pointer, unsigned __int64 shift)
{
	return *((Buffer128*)((unsigned char*)pointer + shift));
}

Buffer256 Scriptbind_CryMarshal::Get256Bytes(void * pointer, unsigned __int64 shift)
{
	return *((Buffer256*)((unsigned char*)pointer + shift));
}

Buffer512 Scriptbind_CryMarshal::Get512Bytes(void * pointer, unsigned __int64 shift)
{
	return *((Buffer512*)((unsigned char*)pointer + shift));
}

void Scriptbind_CryMarshal::SetByte(void* pointer, unsigned __int64 shift, unsigned __int8 value)
{
	*((unsigned __int8 *)pointer + shift) = value;
}

void Scriptbind_CryMarshal::Set2Bytes(void* pointer, unsigned __int64 shift, unsigned __int16 value)
{
	*((unsigned __int16 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set4Bytes(void* pointer, unsigned __int64 shift, unsigned __int32 value)
{
	*((unsigned __int32 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set8Bytes(void* pointer, unsigned __int64 shift, unsigned __int64 value)
{
	*((unsigned __int64 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set32Bytes(void* pointer, unsigned __int64 shift, Buffer32 value)
{
	*((Buffer32 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set64Bytes(void* pointer, unsigned __int64 shift, Buffer64 value)
{
	*((Buffer64 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set128Bytes(void* pointer, unsigned __int64 shift, Buffer128 value)
{
	*((Buffer128 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set256Bytes(void* pointer, unsigned __int64 shift, Buffer256 value)
{
	*((Buffer256 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set512Bytes(void* pointer, unsigned __int64 shift, Buffer512 value)
{
	*((Buffer512 *)((unsigned __int8 *)pointer + shift)) = value;
}

void Scriptbind_CryMarshal::Set4BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, unsigned __int32 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::Set8BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, unsigned __int64 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::Set32BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer32 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::Set64BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer64 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::Set128BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer128 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::Set256BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer256 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::Set512BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer512 value)
{
	SetBytesPartial(pointer, shift, count, &value);
}
void Scriptbind_CryMarshal::SetBytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, void* data)
{
	memcpy(((unsigned __int8 *)pointer + shift), static_cast<unsigned __int8 *>(data), count);
}