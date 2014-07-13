using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	public enum EntityPhysicsEvents
	{
		OnCollisionLogged = 1 << 0,	// Logged events on lower byte.
		OnPostStepLogged = 1 << 1,
		OnStateChangeLogged = 1 << 2,
		OnCreateEntityPartLogged = 1 << 3,
		OnUpdateMeshLogged = 1 << 4,
		AllLogged = OnCollisionLogged | OnPostStepLogged | OnStateChangeLogged | OnCreateEntityPartLogged | OnUpdateMeshLogged,

		OnCollisionImmediate = 1 << 8,	// Immediate events on higher byte.
		OnPostStepImmediate = 1 << 9,
		OnStateChangeImmediate = 1 << 10,
		OnCreateEntityPartImmediate = 1 << 11,
		OnUpdateMeshImmediate = 1 << 12,
		AllImmediate = OnCollisionImmediate | OnPostStepImmediate | OnStateChangeImmediate | OnCreateEntityPartImmediate | OnUpdateMeshImmediate,
	}
}