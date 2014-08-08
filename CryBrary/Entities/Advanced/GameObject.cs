using System;
using System.Collections.Generic;
using System.Linq;
using CryEngine.Native;

namespace CryEngine.Entities.Advanced
{
	/// <summary>
	/// Represents an interface with CryEngine game object interface.
	/// </summary>
	public class GameObject
	{
		#region Statics
		static GameObject()
		{
			GameObjects = new List<GameObject>();
		}
		/// <summary>
		/// Gets game object associated with the entity.
		/// </summary>
		/// <param name="id">Identifier of the entity which game object is required.</param>
		/// <returns><see cref="GameObject" /> for given entity.</returns>
		public static GameObject Get(EntityId id)
		{
			var handle = NativeGameObjectMethods.GetGameObject(id);
			if (handle == IntPtr.Zero)
				return null;

			var gameObject = GameObjects.FirstOrDefault(x => x.Handle == handle);
			if (gameObject == null)
			{
				gameObject = new GameObject(handle);

				GameObjects.Add(gameObject);
			}

			return gameObject;
		}

		private static List<GameObject> GameObjects { get; set; }
		#endregion
		private GameObject(IntPtr handle)
		{
			this.SetIGameObject(handle);

			this.Extensions = new List<GameObjectExtension>();
		}
		/// <summary>
		/// Sets a new profile for an aspect.
		/// </summary>
		/// <param name="aspect">
		/// <see cref="EntityAspects" /> object that designates aspect to set new profile for.
		/// </param>
		/// <param name="profile">New profile data.</param>
		/// <param name="fromNetwork">Indicates whether</param>
		/// <returns>True, if successful.</returns>
		[CLSCompliant(false)]
		public bool SetAspectProfile(EntityAspects aspect, ushort profile, bool fromNetwork = false)
		{
			return NativeGameObjectMethods.SetAspectProfile(this.Handle, aspect, profile, fromNetwork);
		}
		/// <summary>
		/// En/disables sending physics event to this game object.
		/// </summary>
		/// <param name="enable">Indicates whether event must enabled or disabled.</param>
		/// <param name="physicsEvent">
		/// <see cref="EntityPhysicsEvents" /> object that designates the event.
		/// </param>
		public void EnablePhysicsEvent(bool enable, EntityPhysicsEvents physicsEvent)
		{
			NativeGameObjectMethods.EnablePhysicsEvent(this.Handle, enable, physicsEvent);
		}
		/// <summary></summary>
		/// <param name="physicsEvent"></param>
		/// <returns></returns>
		public bool WantsPhysicsEvent(EntityPhysicsEvents physicsEvent)
		{
			return NativeGameObjectMethods.WantsPhysicsEvent(this.Handle, physicsEvent);
		}
		/// <summary>
		/// Tries to get the extension.
		/// </summary>
		/// <param name="name">Name of the extension to attempt to acquire.</param>
		/// <returns>
		/// The extension wrapper object if successful, null if returned handle was null.
		/// </returns>
		public GameObjectExtension QueryExtension(string name)
		{
			return this.TryGetExtension(NativeGameObjectMethods.QueryExtension(this.Handle, name));
		}
		/// <summary>
		/// Forces return of a wrapper object for an extension.
		/// </summary>
		/// <remarks>
		/// New extension is instantiated if needed.
		/// </remarks>
		/// <param name="name">Name of the extension.</param>
		/// <returns>The extension wrapper object.</returns>
		public GameObjectExtension AcquireExtension(string name)
		{
			return this.TryGetExtension(NativeGameObjectMethods.AcquireExtension(this.Handle, name));
		}
		/// <summary>
		/// Release a previously acquired extension
		/// </summary>
		/// <param name="name">Name of the extension.</param>
		public void ReleaseExtension(string name)
		{
			NativeGameObjectMethods.ReleaseExtension(this.Handle, name);
		}
		/// <summary>
		/// Activates the extension.
		/// </summary>
		/// <param name="name">Name of the extension to activate.</param>
		/// <returns>True, if successful.</returns>
		public bool ActivateExtension(string name)
		{
			return NativeGameObjectMethods.ActivateExtension(this.Handle, name);
		}
		/// <summary>
		/// Deactivates the extension.
		/// </summary>
		/// <param name="name">Name of the extension to deactivate.</param>
		public void DeactivateExtension(string name)
		{
			NativeGameObjectMethods.DeactivateExtension(this.Handle, name);
		}

		private GameObjectExtension TryGetExtension(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;

			var extension = this.Extensions.FirstOrDefault(x => x.Handle == handle);
			if (extension == null)
			{
				extension = new GameObjectExtension
				{
					Handle = handle,
					Owner = this
				};

				this.Extensions.Add(extension);
			}

			return extension;
		}

		public void NotifyNetworkStateChange(int aspect)
		{
			NativeGameObjectMethods.ChangedNetworkState(this.Handle, aspect);
		}
		/// <summary>
		/// Sets mode for pre-physics updates.
		/// </summary>
		public PrePhysicsUpdateMode PrePhysicsUpdateMode
		{
			set { NativeGameObjectMethods.EnablePrePhysicsUpdates(this.Handle, value); }
		}
		/// <summary>
		/// Binds the game object to the network.
		/// </summary>
		/// <remarks>
		/// Allows the entity to be synchronized over the network.
		/// </remarks>
		/// <param name="mode">Binding mode.</param>
		/// <returns>True, if successful.</returns>
		public bool BindToNetwork(BindToNetworkMode mode = BindToNetworkMode.Normal)
		{
			return NativeGameObjectMethods.BindToNetwork(this.Handle, mode);
		}

		private List<GameObjectExtension> Extensions { get; set; }

		internal IntPtr Handle { get; set; }
	}
	/// <summary>
	/// Enumeration of types of network binding.
	/// </summary>
	public enum BindToNetworkMode
	{
		Normal = 0,
		Force,
		NowInitialized
	}
	/// <summary>
	/// Enumeration of physics events that can happen to an entity.
	/// </summary>
	public enum EntityPhysicsEvents
	{
		/// <summary>
		/// Flag that defines entity collision event that is logged.
		/// </summary>
		OnCollisionLogged = 1 << 0, // Logged events on lower byte.
		/// <summary>
		/// Flag that defines entity post step event that is logged.
		/// </summary>
		OnPostStepLogged = 1 << 1,
		/// <summary>
		/// Flag that defines physical entity state change event that is logged.
		/// </summary>
		OnStateChangeLogged = 1 << 2,
		/// <summary>
		/// Flag that defines entity part creation event that is logged.
		/// </summary>
		OnCreateEntityPartLogged = 1 << 3,
		/// <summary>
		/// Flag that defines dynamic physical mesh update event that is logged.
		/// </summary>
		OnUpdateMeshLogged = 1 << 4,
		/// <summary>
		/// Flag that defines all logged events.
		/// </summary>
		AllLogged =
			OnCollisionLogged | OnPostStepLogged | OnStateChangeLogged | OnCreateEntityPartLogged |
			OnUpdateMeshLogged,
		/// <summary>
		/// Flag that defines entity collision event.
		/// </summary>
		OnCollisionImmediate = 1 << 8, // Immediate events on higher byte.
		/// <summary>
		/// Flag that defines entity post step event.
		/// </summary>
		OnPostStepImmediate = 1 << 9,
		/// <summary>
		/// Flag that defines physical entity state change event.
		/// </summary>
		OnStateChangeImmediate = 1 << 10,
		/// <summary>
		/// Flag that defines entity part creation event.
		/// </summary>
		OnCreateEntityPartImmediate = 1 << 11,
		/// <summary>
		/// Flag that defines dynamic physical mesh update event.
		/// </summary>
		OnUpdateMeshImmediate = 1 << 12,
		/// <summary>
		/// Flag that defines all immediate events.
		/// </summary>
		AllImmediate =
			OnCollisionImmediate | OnPostStepImmediate | OnStateChangeImmediate | OnCreateEntityPartImmediate |
			OnUpdateMeshImmediate,
	}
}