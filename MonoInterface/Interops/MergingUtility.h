#pragma once
#include <CryAnimation/IFacialAnimation.h>

// Defines some utility functionality for merging various objects in facial animation system.


//! Used when merging objects when any conflicts must be resolved by overwriting existing data.
inline MergeCollisionAction overwriteStrategy(const char *)
{
	return MergeCollisionActionOverwrite;
}

//! Used when merging objects when any conflicts must be resolved by discarding incoming data.
inline MergeCollisionAction notOverwriteStrategy(const char *)
{
	return MergeCollisionActionNoOverwrite;
}

inline Functor1wRet<const char*, MergeCollisionAction> CreateMergingFunctor(bool overwrite)
{
	// This is a functor object that works with non-member functions.
	CBFunctionTranslator1wRet<const char *, MergeCollisionAction, MergeCollisionAction(*)(const char *)>
		funcObj
		(
			overwrite
				? overwriteStrategy
				: notOverwriteStrategy
		);

	return funcObj;
}