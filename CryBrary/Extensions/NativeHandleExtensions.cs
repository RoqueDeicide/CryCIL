using System;
using CryEngine;
using CryEngine.Actors;
using CryEngine.Entities;
using CryEngine.Entities.Advanced;
using CryEngine.Utilities;

namespace CryEngine.Native
{
	public static class NativeHandleExtensions
	{
		#region Actor
		public static IntPtr GetIActor(this ActorBase actor)
		{
			if (actor.IsDestroyed)
				throw new ScriptInstanceDestroyedException("Attempted to access native actor handle on a destroyed script");

			return actor.ActorHandle;
		}

		public static void SetIActor(this ActorBase actor, IntPtr handle)
		{
			actor.ActorHandle = handle;
		}
		#endregion
		#region Game Object
		public static IntPtr GetIGameObject(this GameObject gameObject)
		{
			return gameObject.Handle;
		}

		public static void SetIGameObject(this GameObject gameObject, IntPtr handle)
		{
			gameObject.Handle = handle;
		}
		#endregion
		#region Entity
		public static IntPtr GetIEntity(this EntityBase entity)
		{
			if (entity.IsDestroyed)
				throw new ScriptInstanceDestroyedException("Attempted to access native entity handle on a destroyed script");
			if (entity.EntityHandle == IntPtr.Zero)
				throw new NullPointerException("Attempted to utilize null entity pointer");

			return entity.EntityHandle;
		}

		public static void SetIEntity(this EntityBase entity, IntPtr handle)
		{
			entity.EntityHandle = handle;
		}

		public static IntPtr GetIEntityLink(this EntityLink link)
		{
			return link.Handle;
		}

		public static void SetIEntityLink(this EntityLink link, IntPtr handle)
		{
			link.Handle = handle;
		}

		public static IntPtr GetIAnimatedCharacter(this EntityBase entity)
		{
			if (entity.IsDestroyed)
				throw new ScriptInstanceDestroyedException(
					"Attempted to access native animated character handle on a destroyed entity");
			if (entity.AnimatedCharacterHandle == IntPtr.Zero)
			{
				var animatedCharacterExtension = entity.GameObject.AcquireExtension("AnimatedCharacter");

				entity.SetIAnimatedCharacter(animatedCharacterExtension.Handle);
			}

			return entity.AnimatedCharacterHandle;
		}

		public static void SetIAnimatedCharacter(this EntityBase entity, IntPtr handle)
		{
			entity.AnimatedCharacterHandle = handle;
		}
		#endregion
		#region Material
		public static IntPtr GetIMaterial(this Material mat)
		{
			return mat.Handle;
		}

		public static void SetIMaterial(this Material mat, IntPtr handle)
		{
			mat.Handle = handle;
		}
		#endregion
		#region Lua ScriptTable
		public static IntPtr GetIScriptTable(this Lua.ScriptTable scriptTable)
		{
			return scriptTable.Handle;
		}

		public static void SetIScriptTable(this Lua.ScriptTable scriptTable, IntPtr handle)
		{
			scriptTable.Handle = handle;
		}
		#endregion
		#region SurfaceType
		public static IntPtr GetISurfaceType(this SurfaceType surfaceType)
		{
			return surfaceType.Handle;
		}

		public static void SetISurfaceType(this SurfaceType surfaceType, IntPtr handle)
		{
			surfaceType.Handle = handle;
		}
		#endregion
		#region CrySerialize
		public static IntPtr GetISerialize(this Serialization.CrySerialize serialize)
		{
			return serialize.Handle;
		}

		public static void SetISerialize(this Serialization.CrySerialize serialize, IntPtr handle)
		{
			serialize.Handle = handle;
		}
		#endregion
		#region FlowNode
		public static IntPtr GetIFlowNode(this Flowgraph.FlowNode node)
		{
			return node.Handle;
		}

		public static void SetIFlowNode(this Flowgraph.FlowNode node, IntPtr handle)
		{
			node.Handle = handle;
		}
		#endregion
		#region Physical Entity
		public static IntPtr GetIPhysicalEntity(this Physics.PhysicalEntity physicalEntity)
		{
			return physicalEntity.Handle;
		}

		public static void SetIPhysicalEntity(this Physics.PhysicalEntity physicalEntity, IntPtr handle)
		{
			physicalEntity.Handle = handle;
		}
		#endregion
	}
}