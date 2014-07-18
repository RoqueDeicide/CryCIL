#pragma once
#include <IMonoScriptBind.h>

struct Buffer32;
struct Buffer64;
struct Buffer128;
struct Buffer256;
struct Buffer512;

class Scriptbind_CryMarshal : public IMonoScriptBind
{
public:
	Scriptbind_CryMarshal();
	~Scriptbind_CryMarshal();

	virtual const char *GetClassName() { return "NativeMemoryHandlingMethods"; };

	// Allocate an array of bytes of given length. Returns a pointer to the first byte.
	// Allocated memory should not be referenced from unmanaged code, unless you have access
	// to instance of INativeMemoryWrapper type that works with this memory and
	// that object has not disposed of the memory cluster.
	static void *AllocateMemory(unsigned __int64 size);
	// Frees memory that has been allocated.
	static void *FreeMemory(void * pointer);
	// Gets one byte.
	static unsigned char GetByte(void * pointer, unsigned __int64 index);
	// Gets two bytes.
	static unsigned short Get2Byte(void * pointer, unsigned __int64 index);
	// Gets four bytes.
	static unsigned long Get4Byte(void * pointer, unsigned __int64 index);
	// Gets eight bytes.
	static double Get8Byte(void * pointer, unsigned __int64 index);
	// Hope you check for boundaries on your side, because none of that is done here.
	static Buffer32 Get32Bytes(void * pointer, unsigned __int64 shift);
	static Buffer64 Get64Bytes(void * pointer, unsigned __int64 shift);
	static Buffer128 Get128Bytes(void * pointer, unsigned __int64 shift);
	static Buffer256 Get256Bytes(void * pointer, unsigned __int64 shift);
	static Buffer512 Get512Bytes(void * pointer, unsigned __int64 shift);

	static void SetByte(void* pointer, unsigned __int64 shift, unsigned char value);
	static void Set2Bytes(void* pointer, unsigned __int64 shift, unsigned short value);
	static void Set4Bytes(void* pointer, unsigned __int64 shift, unsigned int value);
	static void Set8Bytes(void* pointer, unsigned __int64 shift, unsigned __int64 value);
	static void Set32Bytes(void* pointer, unsigned __int64 shift, Buffer32 value);
	static void Set64Bytes(void* pointer, unsigned __int64 shift, Buffer64 value);
	static void Set128Bytes(void* pointer, unsigned __int64 shift, Buffer128 value);
	static void Set256Bytes(void* pointer, unsigned __int64 shift, Buffer256 value);
	static void Set512Bytes(void* pointer, unsigned __int64 shift, Buffer512 value);

	static void Set4BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, unsigned __int32 value);
	static void Set8BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, unsigned __int64 value);
	static void Set32BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer32 value);
	static void Set64BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer64 value);
	static void Set128BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer128 value);
	static void Set256BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer256 value);
	static void Set512BytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, Buffer512 value);
	static void SetBytesPartial(void* pointer, unsigned __int64 shift, unsigned __int64 count, void* data);
};

struct Buffer32
{
	unsigned char data[32];
};
struct Buffer64
{
	unsigned char data[64];
};
struct Buffer128
{
	unsigned char data[128];
};
struct Buffer256
{
	unsigned char data[256];
};
struct Buffer512
{
	unsigned char data[512];
};