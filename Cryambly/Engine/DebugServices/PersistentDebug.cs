using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.RunTime;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Defines functions that create simple objects for debug purposes
	/// </summary>
	public static class PersistentDebug
	{
		#region Fields
		private static readonly LinkedList<PersistentDebugObject> objs;
		#endregion
		#region Properties

		#endregion
		#region Events
		/// <summary>
		/// Occurs after one of the objects is rendered for the last time.
		/// </summary>
		public static event EventHandler<PersistenDebugEventArgs> RenderingOver;
		#endregion
		#region Construction
		static PersistentDebug()
		{
			objs = new LinkedList<PersistentDebugObject>();
			MonoInterface.Instance.Updated += RenderObjects;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds an object to the world.
		/// </summary>
		/// <remarks>Given object will be rendered at least once.</remarks>
		/// <param name="obj">     Object to add.</param>
		/// <param name="lifeTime">Length of the life-time of the object.</param>
		public static void Add(PersistentDebugObject obj, TimeSpan lifeTime)
		{
			Add(obj, (float)lifeTime.TotalSeconds);
		}
		/// <summary>
		/// Adds an object to the world.
		/// </summary>
		/// <remarks>Given object will be rendered at least once.</remarks>
		/// <param name="obj">     Object to add.</param>
		/// <param name="lifeTime">Length of the life-time of the object in seconds.</param>
		public static void Add(PersistentDebugObject obj, float lifeTime)
		{
			obj.LifeTime = lifeTime;
			objs.AddLast(obj);
		}
		#endregion
		#region Utilities
		private static void OnRenderingOver(PersistentDebugObject obj)
		{
			if (RenderingOver != null) RenderingOver(null, new PersistenDebugEventArgs(obj));
		}
		private static void RenderObjects(object sender, EventArgs eventArgs)
		{
			if (objs.First == null)
			{
				return;
			}

			for (var current = objs.First; current != null; current = current.Next)
			{
				PersistentDebugObject debugObject = current.Value;

				// Render the object.
				debugObject.Render();

				// Update remaining time.
				debugObject.TimeRemaining -= (float)Time.Frame.TotalSeconds;

				// Check the remaining time.
				if (debugObject.TimeRemaining <= 0)
				{
					// Set object's remaining time and life time to 0 in case the same object will be added
					// later.
					debugObject.lifeTime = 0.0f;
					debugObject.TimeRemaining = 0.0f;

					// Remove the object.
					var previous = current.Previous;
					objs.Remove(current);
					current = previous;

					// Raise the event.
					OnRenderingOver(debugObject);

					// Check, if iteration can stop now.
					if (current == null)
					{
						// No more objects.
						return;
					}
				}
			}
		}
		#endregion
	}
}