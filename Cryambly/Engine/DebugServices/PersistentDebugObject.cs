﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Graphics;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Base class for objects that can be rendered using <see cref="PersistentDebug"/> functionality.
	/// </summary>
	public abstract class PersistentDebugObject
	{
		#region Fields
		internal float lifeTime;
		#endregion
		#region Properties
		/// <summary>
		/// Gets or sets a color of the object.
		/// </summary>
		public ColorByte Color { get; set; }
		/// <summary>
		/// When implemented in derived class, gets default rendering flags that can be used to render the
		/// object using <see cref="AuxiliaryGeometry"/> API.
		/// </summary>
		public abstract AuxiliaryGeometryRenderFlags DefaultRenderingFlags { get; }
		/// <summary>
		/// Gets or sets life time of this object.
		/// </summary>
		/// <remarks>
		/// When set, difference between current value and a new one will be deducted from
		/// <see cref="TimeRemaining"/>.
		/// </remarks>
		public float LifeTime
		{
			get { return this.lifeTime; }
			set
			{
				float delta = this.lifeTime - value;
				this.lifeTime = value;
				this.TimeRemaining -= delta;
			}
		}
		/// <summary>
		/// Gets the time that is remaining until this object will be removed from the rendering queue.
		/// </summary>
		public float TimeRemaining { get; internal set; }
		#endregion
		#region Events
		#endregion
		#region Interface
		/// <summary>
		/// When implemented in derived class, renders this object.
		/// </summary>
		public abstract void Render();
		#endregion
		#region Utilities
		#endregion
	}
}