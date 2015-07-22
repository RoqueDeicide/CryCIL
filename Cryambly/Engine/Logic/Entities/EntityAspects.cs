namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of aspects of the entity that can be synchronized via network.
	/// </summary>
	public enum EntityAspects : uint
	{
		/// <summary>
		/// Represents all aspects. Used to ensure all aspects are synchronized at once.
		/// </summary>
		All = uint.MaxValue,
		// 0x01u // aspect 0

		/// <summary>
		/// Script aspect. Not used in CryCIL.
		/// </summary>
		Script = 0x02u,
		// 0x04u // aspect 2

		/// <summary>
		/// Physics aspect.
		/// </summary>
		Physics = 0x08u,
		/// <summary>
		/// </summary>
		GameClientStatic = 0x10u, // aspect 4
		/// <summary>
		/// </summary>
		GameServerStatic = 0x20u, // aspect 5
		/// <summary>
		/// </summary>
		GameClientDynamic = 0x40u, // aspect 6
		/// <summary>
		/// </summary>
		GameServerDynamic = 0x80u, // aspect 7
		/// <summary>
		/// </summary>
		GameClientA = 0x0100u, // aspect 8
		/// <summary>
		/// </summary>
		GameServerA = 0x0200u, // aspect 9
		/// <summary>
		/// </summary>
		GameClientB = 0x0400u, // aspect 10
		/// <summary>
		/// </summary>
		GameServerB = 0x0800u, // aspect 11
		/// <summary>
		/// </summary>
		GameClientC = 0x1000u, // aspect 12
		/// <summary>
		/// </summary>
		GameServerC = 0x2000u, // aspect 13
		/// <summary>
		/// </summary>
		GameClientD = 0x4000u, // aspect 14
		/// <summary>
		/// </summary>
		GameClientE = 0x8000u, // aspect 15
		/// <summary>
		/// </summary>
		GameClientF = 0x00010000u, // aspect 16
		/// <summary>
		/// </summary>
		GameClientG = 0x00020000u, // aspect 17
		/// <summary>
		/// </summary>
		GameClientH = 0x00040000u, // aspect 18
		/// <summary>
		/// </summary>
		GameClientI = 0x00080000u, // aspect 19
		/// <summary>
		/// </summary>
		GameClientJ = 0x00100000u, // aspect 20
		/// <summary>
		/// </summary>
		GameServerD = 0x00200000u, // aspect 21
		/// <summary>
		/// </summary>
		GameClientK = 0x00400000u, // aspect 22
		/// <summary>
		/// </summary>
		GameClientL = 0x00800000u, // aspect 23
		/// <summary>
		/// </summary>
		GameClientM = 0x01000000u, // aspect 24
		/// <summary>
		/// </summary>
		GameClientN = 0x02000000u, // aspect 25
		/// <summary>
		/// </summary>
		GameClientO = 0x04000000u, // aspect 26
		/// <summary>
		/// </summary>
		GameClientP = 0x08000000u, // aspect 27
		/// <summary>
		/// </summary>
		GameServerE = 0x10000000u, // aspect 28
		/// <summary>
		/// </summary>
		Aspect29 = 0x20000000u, // aspect 29
		/// <summary>
		/// </summary>
		Aspect30 = 0x40000000u, // aspect 30
		/// <summary>
		/// </summary>
		Aspect31 = 0x80000000u // aspect 31
	}
}