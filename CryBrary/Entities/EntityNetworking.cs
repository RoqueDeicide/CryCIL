using System;
using System.Linq;
using System.Reflection;
using CryEngine.Extensions;
using CryEngine.Native;

namespace CryEngine.Entities
{
	public partial class Entity
	{
		public void RemoteInvocation(Action action, NetworkTarget netTarget, int channelId = -1)
		{
			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, null);
		}
		#region RemoteInvocation arg generics
		public void RemoteInvocation<T1>(Action<T1> action, NetworkTarget netTarget, T1 param1, int channelId = -1)
		{
			object[] args = new object[1];
			args[0] = param1;

			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, args);
		}

		public void RemoteInvocation<T1, T2>(Action<T1, T2> action, NetworkTarget netTarget, T1 param1, T2 param2,
											 int channelId = -1)
		{
			object[] args = new object[2];
			args[0] = param1;
			args[1] = param2;

			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, args);
		}

		public void RemoteInvocation<T1, T2, T3>(Action<T1, T2, T3> action, NetworkTarget netTarget, T1 param1, T2 param2,
												 T3 param3, int channelId = -1)
		{
			object[] args = new object[3];
			args[0] = param1;
			args[1] = param2;
			args[2] = param3;

			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, args);
		}

		public void RemoteInvocation<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, NetworkTarget netTarget, T1 param1,
													 T2 param2, T3 param3, T4 param4, int channelId = -1)
		{
			object[] args = new object[4];
			args[0] = param1;
			args[1] = param2;
			args[2] = param3;
			args[3] = param4;

			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, args);
		}

		public void RemoteInvocation<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, NetworkTarget netTarget, T1 param1,
														 T2 param2, T3 param3, T4 param4, T5 param5, int channelId = -1)
		{
			object[] args = new object[5];
			args[0] = param1;
			args[1] = param2;
			args[2] = param3;
			args[3] = param4;
			args[4] = param5;

			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, args);
		}

		public void RemoteInvocation<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, NetworkTarget netTarget,
															 T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, int channelId = -1)
		{
			object[] args = new object[6];
			args[0] = param1;
			args[1] = param2;
			args[2] = param3;
			args[3] = param4;
			args[4] = param5;
			args[5] = param6;

			this.RemoteInvocation(action.Target as EntityBase, action.Method, netTarget, channelId, args);
		}
		#endregion
		private void RemoteInvocation(EntityBase target, MethodInfo method, NetworkTarget netTarget, int channelId,
									  params object[] args)
		{
			if (!method.ContainsAttribute<RemoteInvocationAttribute>())
			{
#if RELEASE
				return;
#else
				throw new AttributeUsageException("Method did not contain RemoteInvocation attribute");
#endif
			}

#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (target == null)
				throw new RemoteInvocationException("Non-static method owner does not derive from EntityBase.");
#endif

			NativeEntityMethods.RemoteInvocation(this.Id, target.Id, method.Name, args, netTarget, channelId);
		}

		private static void OnRemoteInvocation(string methodName, object[] args, EntityId targetId)
		{
			var entity = Entity.Get(targetId);
			var localArgs = args ?? new object[0];

			var type = entity.GetType();
			while (type != null)
			{
				var types = from arg in localArgs
							where arg != null
							select arg.GetType();

				var methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
												null, types.ToArray(), null);
				if (methodInfo != null)
				{
#if RELEASE
					if (!methodInfo.ContainsAttribute<RemoteInvocationAttribute>())
						return;
#endif

					methodInfo.Invoke(entity, args);

					return;
				}

				type = type.BaseType;
			}
		}
	}
}