#pragma once

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