using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using CryEngine;
using CryEngine.Entities;
using CryEngine.Serialization;
using CryEngine.Initialization;
using NUnit.Framework;

namespace CryBrary.Tests.Serialization
{
	public class ScriptsTests
	{
		[Test]
		public void Serialize_Native_Scripts()
		{
			var serializer = new CrySerializer();

			using (var stream = new MemoryStream())
			{
				var scriptManager = new ScriptManager(true, "");

				scriptManager.AddScriptInstance(new NativeEntity(1, IntPtr.Zero), ScriptType.Entity);
				scriptManager.AddScriptInstance(new NativeEntity(2, IntPtr.Zero), ScriptType.Entity);
				scriptManager.AddScriptInstance(new NativeActor(3), ScriptType.Actor);

				serializer.Serialize(stream, scriptManager.Scripts);

				scriptManager.Scripts = serializer.Deserialize(stream) as List<CryScript>;
				Assert.NotNull(scriptManager.Scripts);
				Assert.AreEqual(2, scriptManager.Scripts.Count);

				var entityScript = scriptManager.FindScript(ScriptType.Entity, x => x.Type == typeof(NativeEntity));
				Assert.AreNotSame(default(CryScript), entityScript);
				Assert.NotNull(entityScript.ScriptInstances);

				Assert.AreEqual(1, (entityScript.ScriptInstances[0] as NativeEntity).Id);
				Assert.AreEqual(2, (entityScript.ScriptInstances[1] as NativeEntity).Id);

				var actorScript = scriptManager.FindScript(ScriptType.Entity, x => x.Type == typeof(NativeActor));
				Assert.AreNotSame(default(CryScript), actorScript);
				Assert.NotNull(actorScript.ScriptInstances);

				Assert.AreEqual(3, (actorScript.ScriptInstances[0] as NativeActor).Id);
			}
		}

		[Test]
		public void Serialize_CryScript_List()
		{
			var serializer = new CrySerializer();

			using (var stream = new MemoryStream())
			{
				var list = new List<CryScript>();

				CryScript script;
				if (CryScript.TryCreate(typeof(NativeActor), out script))
				{
					script.ScriptInstances = new List<CryScriptInstance>();
					script.ScriptInstances.Add(new NativeActor(759));
					script.ScriptInstances.Add(new NativeActor(5));

					list.Add(script);
				}

				if (CryScript.TryCreate(typeof(NativeEntity), out script))
				{
					script.ScriptInstances = new List<CryScriptInstance>();
					script.ScriptInstances.Add(new NativeEntity(987, IntPtr.Zero));
					script.ScriptInstances.Add(new NativeEntity(8, IntPtr.Zero));
					script.ScriptInstances.Add(null);

					list.Add(script);
				}

				serializer.Serialize(stream, list);

				serializer = new CrySerializer();
				list = serializer.Deserialize(stream) as List<CryScript>;

				Assert.NotNull(list);
				Assert.IsNotEmpty(list);

				Assert.AreEqual(2, list.Count);

				var nativeActorScript = list.ElementAt(0);
				Assert.NotNull(nativeActorScript.ScriptInstances);
				Assert.AreEqual(2, nativeActorScript.ScriptInstances.Count);

				Assert.NotNull(nativeActorScript.ScriptInstances.ElementAt(0));
				Assert.AreEqual(759, (nativeActorScript.ScriptInstances.ElementAt(0) as EntityBase).Id);

				Assert.NotNull(nativeActorScript.ScriptInstances.ElementAt(1));
				Assert.AreEqual(5, (nativeActorScript.ScriptInstances.ElementAt(1) as EntityBase).Id);

				var nativeEntityScript = list.ElementAt(1);

				Assert.NotNull(nativeEntityScript.ScriptInstances);
				Assert.AreEqual(3, nativeEntityScript.ScriptInstances.Count);

				Assert.NotNull(nativeEntityScript.ScriptInstances.ElementAt(0));
				Assert.AreEqual(987, (nativeEntityScript.ScriptInstances.ElementAt(0) as EntityBase).Id);

				Assert.NotNull(nativeEntityScript.ScriptInstances.ElementAt(1));
				Assert.AreEqual(8, (nativeEntityScript.ScriptInstances.ElementAt(1) as EntityBase).Id);

				Assert.Null(nativeEntityScript.ScriptInstances.ElementAt(2));
			}
		}

		public void DelayedMethod()
		{
		}

		private class TestEntity : Entity
		{
			public TestEntity()
			{
				delayedFunc = new DelayedFunc(MyDelayedFunc, 1337);
			}
			private void MyDelayedFunc()
			{
			}

			private DelayedFunc delayedFunc;
		}

		[Test]
		public void Entity_With_DelayedFunc()
		{
			var serializer = new CrySerializer();

			using (var stream = new MemoryStream())
			{
				var scriptManager = new ScriptManager(true, "");

				scriptManager.AddScriptInstance(new TestEntity(), ScriptType.Entity);

				serializer.Serialize(stream, scriptManager.Scripts);

				scriptManager.Scripts = serializer.Deserialize(stream) as List<CryScript>;
			}
		}
	}
}