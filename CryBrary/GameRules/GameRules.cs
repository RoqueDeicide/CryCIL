using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CryEngine.Entities;
using CryEngine.Initialization;
using CryEngine.Extensions;
using CryEngine.Native;

namespace CryEngine
{
	/// <summary>
	/// This is the base GameRules interface. All game rules must implement this.
	/// </summary>
	public abstract class GameRules : EntityBase
	{
		internal override bool InternalInitialize(IScriptInitializationParams initParams)
		{
			Current = this;

			var gameRulesInitParams = (GameRulesInitializationParams)initParams;

			Id = gameRulesInitParams.id;
			this.SetIEntity(gameRulesInitParams.entityPtr);

			return base.InternalInitialize(initParams);
		}

		/// <summary>
		/// Gets the currently active game rules instance.
		/// </summary>
		public static GameRules Current { get; internal set; }
	}
}