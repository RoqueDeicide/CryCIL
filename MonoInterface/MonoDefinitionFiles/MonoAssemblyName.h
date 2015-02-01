#pragma once

#define MONO_PUBLIC_KEY_TOKEN_LENGTH	17
struct _MonoAssemblyName
{
	const char *name;
	const char *culture;
	const char *hash_value;
	const mono_byte* public_key;
	// string of 16 hex chars + 1 NULL
	mono_byte public_key_token[MONO_PUBLIC_KEY_TOKEN_LENGTH];
	uint32_t hash_alg;
	uint32_t hash_len;
	uint32_t flags;
	uint16_t major, minor, build, revision, arch;
};