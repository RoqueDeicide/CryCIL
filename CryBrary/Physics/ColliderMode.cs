using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryEngine.Physics
{
	/// <summary>
	/// Enumeration of collision modes.
	/// </summary>
	public enum ColliderMode
	{
		/// <summary>
		/// Collision behavior is undefined.
		/// </summary>
		Undefined = 0,
		/// <summary>
		/// Collisions are disabled on the entity.
		/// </summary>
		Disabled,
		/// <summary>
		/// Entity can only collide with the ground.
		/// </summary>
		GroundedOnly,
		/// <summary>
		/// Entity can be pushed by collisions.
		/// </summary>
		Pushable,
		/// <summary>
		/// Entity cannot be pushed by collisions.
		/// </summary>
		NonPushable,
		/// <summary>
		/// Entity can only push the players when colliding.
		/// </summary>
		PushesPlayersOnly,
		/// <summary>
		/// For spectators.
		/// </summary>
		Spectator,
		/// <summary>
		/// Number of collider modes.
		/// </summary>
		Count,
		/// <summary>
		/// 256
		/// </summary>
		Ff = 0xFF
	}
}