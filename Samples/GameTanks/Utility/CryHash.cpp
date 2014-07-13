#include "StdAfx.h"
#include "CryHash.h"
#define DEFAULT_HASH_SEED      40503 // This is a large 16 bit prime number (perfect for seeding)

CryHash HashStringSeed( const char* string, const uint32 seed )
{
	const char*     p;
	CryHash hash = seed;
	for (p = string; *p != '\0'; p++)
	{
		hash += *p;
		hash += (hash << 10);
		hash ^= (hash >> 6);
	}
	hash += (hash << 3);
	hash ^= (hash >> 11);
	hash += (hash << 15);
	return hash;
}

CryHash HashString( const char* string )
{
	return HashStringSeed( string, DEFAULT_HASH_SEED );
}


CryHashStringId CryHashStringId::GetIdForName( const char* _name )
{
	CRY_ASSERT(_name);

	return CryHashStringId(HashString(_name));
}