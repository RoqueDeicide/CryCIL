using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine.Advanced
{
	public class GameObject
	{
		#region Statics
		static GameObject()
		{
			GameObjects = new List<GameObject>();
		}

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

			Extensions = new List<GameObjectExtension>();
		}

		[CLSCompliant(false)]
		public bool SetAspectProfile(EntityAspects aspect, ushort profile, bool fromNetwork = false)
		{
			return NativeGameObjectMethods.SetAspectProfile(Handle, aspect, profile, fromNetwork);
		}

		public void EnablePhysicsEvent(bool enable, EntityPhysicsEvents physicsEvent)
		{
			NativeGameObjectMethods.EnablePhysicsEvent(Handle, enable, physicsEvent);
		}

		public bool WantsPhysicsEvent(EntityPhysicsEvents physicsEvent)
		{
			return NativeGameObjectMethods.WantsPhysicsEvent(Handle, physicsEvent);
		}

		/// <summary>
		/// Query extension. returns 0 if extension is not there.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GameObjectExtension QueryExtension(string name)
		{
			return TryGetExtension(NativeGameObjectMethods.QueryExtension(Handle, name));
		}

		/// <summary>
		/// Forcefully get a pointer to an extension (may instantiate if needed)
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public GameObjectExtension AcquireExtension(string name)
		{
			return TryGetExtension(NativeGameObjectMethods.AcquireExtension(Handle, name));
		}

		/// <summary>
		/// Release a previously acquired extension
		/// </summary>
		/// <param name="name"></param>
		public void ReleaseExtension(string name)
		{
			NativeGameObjectMethods.ReleaseExtension(Handle, name);
		}

		/// <summary>
		/// Turn an extension on
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool ActivateExtension(string name)
		{
			return NativeGameObjectMethods.ActivateExtension(Handle, name);
		}

		/// <summary>
		/// Turn an extension off
		/// </summary>
		/// <param name="name"></param>
		public void DeactivateExtension(string name)
		{
			NativeGameObjectMethods.DeactivateExtension(Handle, name);
		}

		private GameObjectExtension TryGetExtension(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;

			var extension = Extensions.FirstOrDefault(x => x.Handle == handle);
			if (extension == null)
			{
				extension = new GameObjectExtension();
				extension.Handle = handle;
				extension.Owner = this;

				Extensions.Add(extension);
			}

			return extension;
		}

		public void NotifyNetworkStateChange(int aspect)
		{
			NativeGameObjectMethods.ChangedNetworkState(Handle, aspect);
		}

		public PrePhysicsUpdateMode PrePhysicsUpdateMode
		{
			set { NativeGameObjectMethods.EnablePrePhysicsUpdates(Handle, value); }
		}

		public bool BindToNetwork(BindToNetworkMode mode = BindToNetworkMode.Normal)
		{
			return NativeGameObjectMethods.BindToNetwork(Handle, mode);
		}

		private List<GameObjectExtension> Extensions { get; set; }

		internal IntPtr Handle { get; set; }
	}
}