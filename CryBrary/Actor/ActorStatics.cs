using System;
using CryEngine.Entities;
using CryEngine.Mathematics;
using CryEngine.Native;
using CryEngine.Initialization;

namespace CryEngine
{
	public abstract partial class Actor
	{
		/// <summary>
		/// Gets the actor with the specified channel id if it exists.
		/// </summary>
		/// <param name="channelId">channelId of the actor you want to obtain</param>
		/// <returns>Actor if found, null otherwise</returns>
		public static ActorBase Get(int channelId)
		{
			var actor = Get<ActorBase>(channelId);
			if (actor != null)
				return actor;

			var entityInfo = NativeActorMethods.GetActorInfoByChannelId((ushort)channelId);
			if (entityInfo.Id != 0)
				return CreateNativeActor(entityInfo);

			return null;
		}

		/// <summary>
		/// Gets the actor with the specified channel id if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="channelId">channelId of the actor you want to obtain</param>
		/// <returns>Actor if found, null otherwise</returns>
		public static T Get<T>(int channelId) where T : ActorBase
		{
			return ScriptManager.Instance.Find<T>(ScriptType.Actor, x => x.ChannelId == channelId);
		}

		/// <summary>
		/// Gets the actor with the specified entity id if it exists.
		/// </summary>
		/// <param name="actorId">entityId of the actor you want to obtain</param>
		/// <returns>Actor if found, null otherwise</returns>
		public static ActorBase Get(EntityId actorId)
		{
			var actor = Get<ActorBase>(actorId);
			if (actor != null)
				return actor;

			// Couldn't find a CryMono entity, check if a non-managed one exists.
			var actorInfo = NativeActorMethods.GetActorInfoById(actorId);
			if (actorInfo.Id != 0)
				return CreateNativeActor(actorInfo);

			return null;
		}

		/// <summary>
		/// Gets the actor with the specified entity id if it exists.
		/// </summary>
		/// <param name="actorId">entityId of the actor you want to obtain</param>
		/// <returns>Actor if found, null otherwise</returns>
		public static T Get<T>(EntityId actorId) where T : ActorBase
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (actorId == 0)
				throw new ArgumentException("actorId cannot be 0!");
#endif

			return ScriptManager.Instance.Find<T>(ScriptType.Actor, x => x.Id == actorId);
		}

		internal static ActorBase CreateNativeActor(ActorInitializationParams actorInfo)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (actorInfo.Id == 0)
				throw new ArgumentException("actorInfo.Id cannot be 0!");
			if (actorInfo.ActorPtr == IntPtr.Zero)
				throw new ArgumentException("actorInfo.ActorPtr cannot be 0!");
			if (actorInfo.EntityPtr == IntPtr.Zero)
				throw new ArgumentException("actorInfo.EntityPtr cannot be 0!");
#endif

			var nativeActor = new NativeActor(actorInfo);
			ScriptManager.Instance.AddScriptInstance(nativeActor, ScriptType.Actor);

			return nativeActor;
		}

		/// <summary>
		/// Gets the player actor in use on this PC.
		/// </summary>
		public static ActorBase LocalClient
		{
			get
			{
				var clientActorId = NativeActorMethods.GetClientActorId();
				if (clientActorId == 0)
					return null;

				return Get(new EntityId(clientActorId));
			}
		}

		/// <summary>
		/// Spawns a new actor
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="channelId"></param>
		/// <param name="name"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static T Create<T>(int channelId, string name = "Dude", Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null) where T : ActorBase, new()
		{
			return CreateCommon<T>(typeof(T).Name, channelId, name, pos, rot, scale);
		}

		/// <summary>
		/// Spawns a new actor
		/// </summary>
		/// <param name="actorType"></param>
		/// <param name="channelId"></param>
		/// <param name="name"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static ActorBase Create(Type actorType, int channelId, string name = "Dude", Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null)
		{
			return CreateCommon<ActorBase>(actorType.Name, channelId, name, pos, rot, scale);
		}

		/// <summary>
		/// Spawns a new actor
		/// </summary>
		/// <param name="className"></param>
		/// <param name="channelId"></param>
		/// <param name="name"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="scale"></param>
		/// <returns></returns>
		public static ActorBase Create(string className, int channelId, string name = "Dude", Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null)
		{
			return CreateCommon<ActorBase>(className, channelId, name, pos, rot, scale);
		}

		internal static T CreateCommon<T>(string className, int channelId, string name = "Dude", Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null) where T : ActorBase
		{
			var actor = Get<T>(channelId);
			if (actor != null)
				return actor;

			var info = NativeActorMethods.CreateActor(channelId, name, className, pos ?? new Vector3(0, 0, 0), rot ?? Quaternion.Identity, scale ?? new Vector3(1, 1, 1));
			if (info.Id == 0)
				throw new Exception("Actor creation failed, make sure your IActor implementation is registered with the same name as your managed actor class.");

			return Get<T>(channelId);
		}

		/// <summary>
		/// Removes an actor by entity id.
		/// </summary>
		/// <param name="id"></param>
		public static void Remove(EntityId id)
		{
			NativeActorMethods.RemoveActor(id);
		}

		/// <summary>
		/// Removes an actor by channelId
		/// </summary>
		/// <param name="channelId"></param>
		public static void Remove(int channelId)
		{
			var actorInfo = NativeActorMethods.GetActorInfoByChannelId((ushort)channelId);
			if (actorInfo.Id != 0)
				NativeActorMethods.RemoveActor(actorInfo.Id);
		}
	}
}