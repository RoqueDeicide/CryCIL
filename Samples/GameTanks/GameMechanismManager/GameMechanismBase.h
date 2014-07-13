#ifndef __CGAMEMECHANISMBASE_H__
#define __CGAMEMECHANISMBASE_H__

#include "GameMechanismEvents.h"

#pragma warning(disable : 4355) // Allow use of 'this' in variable initialization

// NB: Specifying the wrong class name in the REGISTER_GAME_MECHANISM macro will now fail to compile, so it should be
// impossible (or trickier at least) to accidentally register your game mechanism instance with the wrong name. [TF]
#define REGISTER_GAME_MECHANISM(classType) CGameMechanismBase((this == (classType *) NULL) ? NULL : (# classType))

class CGameMechanismBase
{
	public:
	struct SLinkedListPointers
	{
		CGameMechanismBase * m_nextMechanism;
		CGameMechanismBase * m_prevMechanism;
	};

	CGameMechanismBase(const char * className);
	virtual ~CGameMechanismBase();
	virtual void Update(float dt) = 0;
	virtual void Inform(EGameMechanismEvent gmEvent, const SGameMechanismEventData * data) {}

	ILINE SLinkedListPointers * GetLinkedListPointers()
	{
		return & m_linkedListPointers;
	}

	ILINE const char * GetName()
	{
		return m_className;
	}

	private:
	SLinkedListPointers m_linkedListPointers;
	const char * m_className;
};

#endif //__CGAMEMECHANISMBASE_H__