using System;
using System.Linq;

namespace CryCil.Engine.Logic
{
	/// <summary>
	/// Enumeration of policies that define when to automatically activate entities.
	/// </summary>
	public enum EntityUpdatePolicy
	{
		/// <summary>
		/// Never (de-)activate the entity.
		/// </summary>
		/// <remarks>
		/// This is a default policy. When used the entity will only be activated by the user.
		/// </remarks>
		Never,
		/// <summary>
		/// Only have the entity active when it is within specified range from active camera.
		/// </summary>
		/// <remarks>
		/// Doesn't look like this one is used anywhere and there is no indication that it actually works.
		/// </remarks>
		InRange,
		/// <summary>
		/// Only have the entity active when it's potentially visible.
		/// </summary>
		/// <remarks>
		/// Doesn't look like this one is used anywhere and there is no indication that it actually works.
		/// </remarks>
		PotentiallyVisible,
		/// <summary>
		/// Only have the entity active when it's visible.
		/// </summary>
		/// <remarks>
		/// Doesn't look like this one is used anywhere and there is no indication that it actually works.
		/// </remarks>
		Visible,
		/// <summary>
		/// Only have the entity active, when its physics proxy is awake.
		/// </summary>
		Physics,
		/// <summary>
		/// Only have the entity active, when its physics proxy is awake and its visible.
		/// </summary>
		/// <remarks>Despite the description this policy is the same as <see cref="Physics"/>.</remarks>
		PhysicsVisible,
		/// <summary>
		/// Always have the entity active.
		/// </summary>
		/// <remarks>
		/// Doesn't look like this one is used anywhere and there is no indication that it actually works.
		/// </remarks>
		Always,
	}
}