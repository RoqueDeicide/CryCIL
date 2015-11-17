using System;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of categories of entity aspects that can be synchronized via network. It doesn't serve
	/// any purpose other then providing information.
	/// </summary>
	public enum EntityAspectCategories
	{
		/// <summary>
		/// Multi-profile aspects can have up to 8 different profiles (synchronization formats), these can
		/// be used to change what data gets synchronized depending on the current profile.
		/// </summary>
		MultiProfile,
		/// <summary>
		/// Client aspects represent the data that the client can be allowed to change by the server.
		/// </summary>
		Client,
		/// <summary>
		/// Server aspects represent the data that is changed by the server, so client are not allowed to
		/// trigger synchronization of those.
		/// </summary>
		Server,
		/// <summary>
		/// Static aspects can be used to synchronize data that rarely changes. There is nothing in
		/// CryEngine that can enforce this behavior, however, so it's really only about making the code
		/// more descriptive.
		/// </summary>
		Static,
		/// <summary>
		/// Dynamic aspects can be used to synchronize data that frequently changes. There is nothing in
		/// CryEngine that can enforce this behavior, however, so it's really only about making the code
		/// more descriptive.
		/// </summary>
		Dynamic,
		/// <summary>
		/// Custom aspects can be used to synchronize any kind of data.
		/// </summary>
		Custom
	}
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
		/// <remarks>
		/// This aspect is used by CryEngine Lua script system and is never at any point used anywhere
		/// else.
		/// </remarks>
		Script = 0x02u,
		// 0x04u // aspect 2

		/// <summary>
		/// Physics aspect.
		/// </summary>
		/// <remarks>
		/// When this aspect is changed the entity's physics proxy will always get synchronized before
		/// <see cref="MonoNetEntity.SynchronizeWithNetwork"/> is called.
		/// </remarks>
		/// <seealso cref="EntityAspectCategories.MultiProfile"/>
		Physics = 0x08u,
		/// <summary>
		/// Static client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Static"/>
		GameClientStatic = 0x10u, // aspect 4
		/// <summary>
		/// Static server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Static"/>
		/// <seealso cref="EntityAspectCategories.MultiProfile"/>
		GameServerStatic = 0x20u, // aspect 5
		/// <summary>
		/// Dynamic client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Dynamic"/>
		GameClientDynamic = 0x40u, // aspect 6
		/// <summary>
		/// Dynamic server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Dynamic"/>
		GameServerDynamic = 0x80u, // aspect 7
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientA = 0x0100u, // aspect 8
		/// <summary>
		/// Custom server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameServerA = 0x0200u, // aspect 9
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		GameClientB = 0x0400u, // aspect 10
		/// <summary>
		/// Custom server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameServerB = 0x0800u, // aspect 11
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		GameClientC = 0x1000u, // aspect 12
		/// <summary>
		/// Custom server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameServerC = 0x2000u, // aspect 13
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientD = 0x4000u, // aspect 14
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientE = 0x8000u, // aspect 15
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientF = 0x00010000u, // aspect 16
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientG = 0x00020000u, // aspect 17
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientH = 0x00040000u, // aspect 18
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientI = 0x00080000u, // aspect 19
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientJ = 0x00100000u, // aspect 20
		/// <summary>
		/// Custom server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameServerD = 0x00200000u, // aspect 21
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientK = 0x00400000u, // aspect 22
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientL = 0x00800000u, // aspect 23
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientM = 0x01000000u, // aspect 24
		/// <summary>
		/// Custom client data aspect. This aspect is very special, because the data that is associated
		/// with it will not be synchronized with clients that don't have authority over the entity.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientN = 0x02000000u, // aspect 25
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientO = 0x04000000u, // aspect 26
		/// <summary>
		/// Custom client data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameClientP = 0x08000000u, // aspect 27
		/// <summary>
		/// Custom server data aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Server"/>
		/// <seealso cref="EntityAspectCategories.Custom"/>
		GameServerE = 0x10000000u, // aspect 28
		/// <summary>
		/// Not used.
		/// </summary>
		Aspect29 = 0x20000000u, // aspect 29
		/// <summary>
		/// Not used.
		/// </summary>
		Aspect30 = 0x40000000u, // aspect 30
		/// <summary>
		/// Player update aspect.
		/// </summary>
		/// <seealso cref="EntityAspectCategories.Client"/>
		Aspect31 = 0x80000000u // aspect 31
	}
}