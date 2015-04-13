using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CryCil.RunTime;

namespace CryCil.Engine.DebugServices
{
	/// <summary>
	/// Defines functions that create simple objects for debug purposes
	/// </summary>
	/// <threadsafety>
	/// <para>
	/// To make sure that multiple threads cannot access the persistent debug objects while rendering the
	/// critical section will be entered for each of them, use <c>lock</c> state or <see cref="Monitor"/>
	/// functionality to lock your object while editing it.
	/// </para>
	/// <para>
	/// The critical section for the object that is removed from the rendering queue will be exited before
	/// <see cref="RenderingOver"/> event is raised, so you can safely lock the object while in the event
	/// handler.
	/// </para>
	/// </threadsafety>
	public static class DebugEngine
	{
		#region Fields
		private static readonly LinkedList<DebugObject> objs;
		#endregion
		#region Properties

		#endregion
		#region Events
		/// <summary>
		/// Occurs after one of the objects is rendered for the last time.
		/// </summary>
		public static event EventHandler<DebugEngineEventArgs> RenderingOver;
		#endregion
		#region Construction
		static DebugEngine()
		{
			objs = new LinkedList<DebugObject>();
			MonoInterface.Updated += RenderObjects;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Adds an object to the world.
		/// </summary>
		/// <remarks>Given object will be rendered at least once.</remarks>
		/// <param name="obj">     Object to add.</param>
		/// <param name="lifeTime">Length of the life-time of the object.</param>
		public static void Add(DebugObject obj, TimeSpan lifeTime)
		{
			Add(obj, (float)lifeTime.TotalSeconds);
		}
		/// <summary>
		/// Adds an object to the world.
		/// </summary>
		/// <remarks>Given object will be rendered at least once.</remarks>
		/// <param name="obj">     Object to add.</param>
		/// <param name="lifeTime">Length of the life-time of the object in seconds.</param>
		public static void Add(DebugObject obj, float lifeTime)
		{
			obj.LifeTime = lifeTime;
			objs.AddLast(obj);
		}
		#endregion
		#region Utilities
		private static void OnRenderingOver(DebugObject obj)
		{
			if (RenderingOver != null) RenderingOver(null, new DebugEngineEventArgs(obj));
		}
		private static void RenderObjects()
		{
			if (objs.First == null)
			{
				return;
			}

			for (var current = objs.First; current != null; current = current.Next)
			{
				DebugObject debugObject = current.Value;

				Monitor.Enter(debugObject);

				try
				{
					// Render the object.
					debugObject.Render();

					// Update remaining time.
					debugObject.TimeRemaining -= (float)Time.Frame.TotalSeconds;

					// Check the remaining time.
					if (debugObject.TimeRemaining <= 0)
					{
						// Set object's remaining time and life time to 0 in case the same object will be
						// added later.
						debugObject.lifeTime = 0.0f;
						debugObject.TimeRemaining = 0.0f;

						// Remove the object.
						var previous = current.Previous;
						objs.Remove(current);
						current = previous;

						// Exiting the critical section here to make sure there are no deadlocks in the
						// event handlers.
						Monitor.Exit(debugObject);

						// Raise the event.
						OnRenderingOver(debugObject);

						// Check, if iteration can stop now.
						if (current == null)
						{
							// No more objects.
							return;
						}
					}
					else
					{
						// Normal exit here.
						Monitor.Exit(debugObject);
					}
				}
				catch (Exception)
				{
					// Just in case the exception gets thrown after exiting critical section.
					if (Monitor.IsEntered(debugObject))
					{
						Monitor.Exit(debugObject);
					}
					throw;
				}
			}

			AuxiliaryGeometry.Flush();
		}
		#endregion
	}
}