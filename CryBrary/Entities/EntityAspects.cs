using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryEngine
{
	/// <summary>
	/// Enumeration of entity aspects.
	/// </summary>
	[CLSCompliant(false)]
	public enum EntityAspects : uint
	{
		// 0x01u // aspect 0
		Script = 0x02u, // aspect 1
		// 0x04u // aspect 2
		Physics = 0x08u, // aspect 3
		GameClientStatic = 0x10u, // aspect 4
		GameServerStatic = 0x20u, // aspect 5
		GameClientDynamic = 0x40u, // aspect 6
		GameServerDynamic = 0x80u, // aspect 7

		GameClientA = 0x0100u, // aspect 8
		GameServerA = 0x0200u, // aspect 9
		GameClientB = 0x0400u, // aspect 10
		GameServerB = 0x0800u, // aspect 11
		GameClientC = 0x1000u, // aspect 12
		GameServerC = 0x2000u, // aspect 13
		GameClientD = 0x4000u, // aspect 14
		GameClientE = 0x8000u, // aspect 15

		GameClientF = 0x00010000u, // aspect 16
		GameClientG = 0x00020000u, // aspect 17
		GameClientH = 0x00040000u, // aspect 18
		GameClientI = 0x00080000u, // aspect 19
		GameClientJ = 0x00100000u, // aspect 20
		GameServerD = 0x00200000u, // aspect 21
		GameClientK = 0x00400000u, // aspect 22
		Aspect23 = 0x00800000u, // aspect 23
		Aspect24 = 0x01000000u, // aspect 24
		Aspect25 = 0x02000000u, // aspect 25
		Aspect26 = 0x04000000u, // aspect 26
		Aspect27 = 0x08000000u, // aspect 27
		Aspect28 = 0x10000000u, // aspect 28
		Aspect29 = 0x20000000u, // aspect 29
		Aspect30 = 0x40000000u, // aspect 30
		Aspect31 = 0x80000000u, // aspect 31
	}
}