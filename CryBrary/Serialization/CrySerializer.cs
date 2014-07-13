using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using CryEngine.Extensions;
using CryEngine.Utilities;

namespace CryEngine.Serialization
{
	[CLSCompliant(false)]
	public class CrySerializer : IFormatter, ICrySerialize
	{
		public static string SerializeToString(object graph)
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();
				serializer.Serialize(stream, graph);

				var streamReader = new StreamReader(stream);
				return streamReader.ReadToEnd();
			}
		}

		public static object DeserializeFromString(string data)
		{
			var byteArray = System.Text.Encoding.ASCII.GetBytes(data);
			using (var stream = new MemoryStream(byteArray))
			{
				var serializer = new CrySerializer();
				return serializer.Deserialize(stream);
			}
		}

		private StreamWriter Writer { get; set; }
		private StreamReader Reader { get; set; }
		private FormatterConverter Converter { get; set; }

		/// <summary>
		/// We store a dictionary of all serialized objects in order to not create new instances of
		/// types with identical hash codes. (same objects)
		/// </summary>
		private Dictionary<int, ObjectReference> ObjectReferences { get; set; }

		/// <summary>
		/// Toggle debug mode, logs information on possible serialization issues. Automatically
		/// turned on if mono_realtimeScriptingDebug is set to 1.
		/// </summary>
		public bool IsDebugModeEnabled { get; set; }

		public CrySerializer()
		{
			Converter = new FormatterConverter();
			ObjectReferences = new Dictionary<int, ObjectReference>();

#if !UNIT_TESTING
			var debugCVar = CVar.Get("mono_realtimeScriptingDebug");
			if (debugCVar != null)
				IsDebugModeEnabled = (debugCVar.IVal != 0);
#endif
		}

		public void Serialize(Stream stream, object graph)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (stream == null)
				throw new ArgumentNullException("stream");
			if (graph == null)
				throw new ArgumentNullException("graph");
#endif

			Writer = new StreamWriter(stream) { AutoFlush = true };
			Reader = null;

			ObjectReferences.Clear();
			m_currentLine = 0;

			StartWrite(new ObjectReference("root", graph));
			stream.Seek(0, SeekOrigin.Begin);
		}

		private void WriteLine(object value)
		{
			m_currentLine++;
			Writer.WriteLine(value);
		}

		/// <summary>
		/// Checks if this object has already been serialized.
		/// </summary>
		/// <param name="objectReference"></param>
		/// <returns>true if object had already been serialized.</returns>
		private bool TryWriteReference(ObjectReference objectReference)
		{
			if (objectReference.SerializationType > SerializationType.ReferenceTypes)
			{
				foreach (var pair in ObjectReferences)
				{
					if (pair.Value.Value.Equals(objectReference.Value))
					{
						WriteReference(objectReference, pair.Key);
						return true;
					}
				}

				ObjectReferences.Add(m_currentLine, objectReference);
			}

			return false;
		}

		/// <summary>
		/// Starts writing the specified reference.
		/// </summary>
		/// <param name="objectReference"></param>
		public void StartWrite(ObjectReference objectReference)
		{
			WriteLine(objectReference.Name);

			if (TryWriteReference(objectReference))
				return;

			WriteLine((int)objectReference.SerializationType);

			switch (objectReference.SerializationType)
			{
				case SerializationType.Null:
					break;
				case SerializationType.IntPtr:
					WriteIntPtr(objectReference);
					break;
				case SerializationType.Any:
					WriteAny(objectReference);
					break;
				case SerializationType.String:
					WriteString(objectReference);
					break;
				case SerializationType.Array:
					WriteArray(objectReference);
					break;
				case SerializationType.Enumerable:
					WriteEnumerable(objectReference);
					break;
				case SerializationType.GenericEnumerable:
					WriteGenericEnumerable(objectReference);
					break;
				case SerializationType.Enum:
					WriteEnum(objectReference);
					break;
				case SerializationType.Type:
					WriteType(objectReference);
					break;
				case SerializationType.Delegate:
					WriteDelegate(objectReference);
					break;
				case SerializationType.MemberInfo:
					WriteMemberInfo(objectReference);
					break;
				case SerializationType.Object:
					WriteObject(objectReference);
					break;
				case SerializationType.UnusedMarker:
					WriteUnusedMarker(objectReference);
					break;
			}
		}

		private void WriteIntPtr(ObjectReference objectReference)
		{
			WriteLine(((IntPtr)objectReference.Value).ToInt64());
		}

		private void WriteReference(ObjectReference objReference, int line)
		{
			WriteLine(SerializationType.Reference);
			WriteLine(line);
		}

		private void WriteAny(ObjectReference objectReference)
		{
			WriteType(objectReference.Value.GetType());
			WriteLine(objectReference.Value);
		}

		private void WriteString(ObjectReference objectReference)
		{
			WriteLine(objectReference.Value);
		}

		private void WriteEnum(ObjectReference objectReference)
		{
			WriteType(objectReference.Value.GetType());
			WriteLine(objectReference.Value);
		}

		private void WriteArray(ObjectReference objectReference)
		{
			var array = objectReference.Value as Array;
			var numElements = array.Length;
			WriteLine(numElements);

			WriteType(array.GetType().GetElementType());

			for (int i = 0; i < numElements; i++)
				StartWrite(new ObjectReference(i.ToString(), array.GetValue(i)));
		}

		private void WriteEnumerable(ObjectReference objectReference)
		{
			var array = (objectReference.Value as IEnumerable).Cast<object>();
			var numElements = array.Count();
			WriteLine(numElements);

			WriteType(GetIEnumerableElementType(objectReference.Value.GetType()));

			for (int i = 0; i < numElements; i++)
				StartWrite(new ObjectReference(i.ToString(), array.ElementAt(i)));
		}

		private void WriteGenericEnumerable(ObjectReference objectReference)
		{
			var enumerable = (objectReference.Value as IEnumerable).Cast<object>();

			WriteLine(enumerable.Count());

			var type = objectReference.Value.GetType();
			WriteType(type);

			if (type.Implements<IDictionary>())
			{
				int i = 0;
				foreach (var element in enumerable)
				{
					StartWrite(new ObjectReference("key_" + i.ToString(), element.GetType().GetProperty("Key").GetValue(element, null)));
					StartWrite(new ObjectReference("value_" + i.ToString(), element.GetType().GetProperty("Value").GetValue(element, null)));
					i++;
				}
			}
			else
			{
				for (int i = 0; i < enumerable.Count(); i++)
					StartWrite(new ObjectReference(i.ToString(), enumerable.ElementAt(i)));
			}
		}

		private void WriteObject(ObjectReference objectReference)
		{
			var type = objectReference.Value.GetType();
			WriteType(type);

			if (type.Implements<ICrySerializable>())
			{
				var crySerializable = objectReference.Value as ICrySerializable;
				crySerializable.Serialize(this);

				return;
			}

			while (type != null)
			{
				var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				WriteLine(fields.Length);
				foreach (var field in fields)
					StartWrite(new ObjectReference(field.Name, field.GetValue(objectReference.Value)));

				var events = type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				WriteLine(events.Length);
				foreach (var eventInfo in events)
				{
					WriteLine(eventInfo.Name);
					WriteType(eventInfo.EventHandlerType);

					var eventFieldInfo = type.GetField(eventInfo.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
					var eventFieldValue = (Delegate)eventFieldInfo.GetValue(objectReference.Value);
					if (eventFieldValue != null)
					{
						var delegates = eventFieldValue.GetInvocationList();

						WriteLine(delegates.Length);
						foreach (var eventDelegate in delegates)
							WriteDelegate(eventDelegate);
					}
					else
						WriteLine(0);
				}

				type = type.BaseType;
			}
		}

		private void WriteMemberInfo(ObjectReference objectReference)
		{
			var memberInfo = objectReference.Value as MemberInfo;
			WriteMemberInfo(memberInfo);
		}

		private void WriteMemberInfo(MemberInfo memberInfo)
		{
			WriteLine(memberInfo.Name);
			WriteType(memberInfo.ReflectedType);
			WriteLine(memberInfo.MemberType);

			if (memberInfo.MemberType == MemberTypes.Method)
			{
				var methodInfo = memberInfo as MethodInfo;

				var parameters = methodInfo.GetParameters();
				WriteLine(parameters.Length);
				foreach (var parameter in parameters)
					WriteType(parameter.ParameterType);
			}
		}

		private void WriteDelegate(ObjectReference objectReference)
		{
			WriteDelegate(objectReference.Value as Delegate);
		}

		private void WriteDelegate(Delegate _delegate)
		{
			WriteType(_delegate.GetType());
			WriteMemberInfo(_delegate.Method);
			if (_delegate.Target != null)
			{
				WriteLine("target");
				StartWrite(new ObjectReference("delegateTarget", _delegate.Target));
			}
			else
				WriteLine("null_target");
		}

		private void WriteUnusedMarker(ObjectReference objectReference)
		{
			WriteType(objectReference.Value.GetType());
		}

		private void WriteType(ObjectReference objectReference)
		{
			WriteType(objectReference.Value as Type);
		}

		private void WriteType(Type type)
		{
			WriteLine(type.IsGenericType);

			if (type.IsGenericType)
			{
				WriteLine(type.GetGenericTypeDefinition().FullName);

				var genericArgs = type.GetGenericArguments();
				WriteLine(genericArgs.Length);
				foreach (var genericArg in genericArgs)
					WriteType(genericArg);
			}
			else
				WriteLine(type.FullName);
		}

		public object Deserialize(Stream stream)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (stream == null || stream.Length == 0)
				throw new ArgumentNullException("stream");
#endif

			Reader = new StreamReader(stream);
			Writer = null;

			ObjectReferences.Clear();
			m_currentLine = 0;

			return StartRead().Value;
		}

		private string ReadLine()
		{
			m_currentLine++;

			return Reader.ReadLine();
		}

		/// <summary>
		/// Starts reading an reference that was written with <see cref="StartWrite" />.
		/// </summary>
		/// <returns></returns>
		public ObjectReference StartRead()
		{
			var name = ReadLine();

			int line = m_currentLine;

			var serializationType = (SerializationType)Enum.Parse(typeof(SerializationType), ReadLine());
			var objReference = new ObjectReference(name, serializationType);

			if (serializationType > SerializationType.ReferenceTypes)
				ObjectReferences.Add(line, objReference);

			switch (serializationType)
			{
				case SerializationType.Null: break;
				case SerializationType.Reference: ReadReference(objReference); break;
				case SerializationType.Object: ReadObject(objReference); break;
				case SerializationType.Array: ReadArray(objReference); break;
				case SerializationType.GenericEnumerable: ReadGenericEnumerable(objReference); break;
				case SerializationType.Enumerable: ReadEnumerable(objReference); break;
				case SerializationType.Enum: ReadEnum(objReference); break;
				case SerializationType.Any: ReadAny(objReference); break;
				case SerializationType.String: ReadString(objReference); break;
				case SerializationType.MemberInfo: ReadMemberInfo(objReference); break;
				case SerializationType.Type: ReadType(objReference); break;
				case SerializationType.Delegate: ReadDelegate(objReference); break;
				case SerializationType.IntPtr: ReadIntPtr(objReference); break;
				case SerializationType.UnusedMarker: ReadUnusedMarker(objReference); break;
			}

#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!objReference.AllowNull && objReference.Value == null && serializationType != SerializationType.Null)
				throw new SerializationException(string.Format("Failed to deserialize object of type {0} {1} at line {2}!", objReference.SerializationType, objReference.Name, line));
#endif

			return objReference;
		}

		private void ReadIntPtr(ObjectReference objReference)
		{
			objReference.Value = new IntPtr(Int64.Parse(ReadLine()));
		}

		private void ReadReference(ObjectReference objReference)
		{
			int referenceLine = int.Parse(ReadLine());

			ObjectReference originalReference;
			if (!ObjectReferences.TryGetValue(referenceLine, out originalReference))
				throw new SerializationException(string.Format("Failed to obtain reference {0} at line {1}! Last line was {2})", objReference.Name, referenceLine, m_currentLine));

			objReference.Value = originalReference.Value;
			objReference.AllowNull = originalReference.AllowNull;
		}

		private void ReadObject(ObjectReference objReference)
		{
			var type = ReadType();

			objReference.Value = FormatterServices.GetUninitializedObject(type);

			if (type.Implements<ICrySerializable>())
			{
				var crySerializable = objReference.Value as ICrySerializable;
				crySerializable.Serialize(this);

				return;
			}

			while (type != null)
			{
				var numFields = int.Parse(ReadLine());
				for (int i = 0; i < numFields; i++)
				{
					var fieldReference = StartRead();

					if (objReference.Value == null)
						continue;

					var fieldInfo = type.GetField(fieldReference.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

					if (fieldInfo != null)
						fieldInfo.SetValue(objReference.Value, fieldReference.Value);
					else if (IsDebugModeEnabled)
						throw new MissingFieldException(string.Format("Failed to find field {0} in type {1}", fieldReference.Name, type.Name));
				}

				var numEvents = int.Parse(ReadLine());

				for (int i = 0; i < numEvents; i++)
				{
					var eventName = ReadLine();

					var eventInfo = type.GetEvent(eventName);
					var eventHandlerType = ReadType();

					var numDelegates = Int32.Parse(ReadLine());
					for (int iDelegate = 0; iDelegate < numDelegates; iDelegate++)
					{
						var foundDelegate = ReadDelegate();

						eventInfo.AddEventHandler(objReference.Value, foundDelegate);
					}
				}

				type = type.BaseType;
			}
		}

		private void ReadArray(ObjectReference objReference)
		{
			var numElements = int.Parse(ReadLine());
			var type = ReadType();

			objReference.Value = Array.CreateInstance(type, numElements);
			var array = objReference.Value as Array;

			for (int i = 0; i != numElements; ++i)
				array.SetValue(StartRead().Value, i);
		}

		private void ReadEnumerable(ObjectReference objReference)
		{
			var numElements = int.Parse(ReadLine());
			var type = ReadType();

			objReference.Value = Array.CreateInstance(type, numElements);
			var array = objReference.Value as Array;

			for (int i = 0; i != numElements; ++i)
				array.SetValue(StartRead().Value, i);
		}

		private void ReadGenericEnumerable(ObjectReference objReference)
		{
			int elements = int.Parse(ReadLine());

			var type = ReadType();

			objReference.Value = Activator.CreateInstance(type);

			if (type.Implements<IDictionary>())
			{
				var dict = objReference.Value as IDictionary;

				for (int i = 0; i < elements; i++)
				{
					var key = StartRead().Value;
					var value = StartRead().Value;

					dict.Add(key, value);
				}
			}
			else if (type.Implements<IList>())
			{
				var list = objReference.Value as IList;

				for (int i = 0; i < elements; i++)
					list.Add(StartRead().Value);
			}
			else if (type.ImplementsGeneric(typeof(ISet<>)) || type.ImplementsGeneric(typeof(ICollection<>)))
			{
				var set = objReference.Value;

				MethodInfo addMethod = null;
				var baseType = type;

				while (baseType != null)
				{
					addMethod = type.GetMethod("Add", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
					if (addMethod != null)
						break;

					baseType = baseType.BaseType;
				}

				for (int i = 0; i < elements; i++)
				{
					addMethod.Invoke(objReference.Value, new object[] { StartRead().Value });
				}
			}
			else if (IsDebugModeEnabled)
				throw new SerializationException(string.Format("Failed to serialize generic enumerable of type {0}, not supported by implementation", type.Name));
		}

		private void ReadAny(ObjectReference objReference)
		{
			var type = ReadType();
			string valueString = ReadLine();

			if (!string.IsNullOrEmpty(valueString))
				objReference.Value = Converter.Convert(valueString, type);
			else
				objReference.Value = 0;
		}

		private void ReadString(ObjectReference objReference)
		{
			objReference.Value = ReadLine();
		}

		private void ReadEnum(ObjectReference objReference)
		{
			var type = ReadType();
			string valueString = ReadLine();

			objReference.Value = Enum.Parse(type, valueString);
		}

		private void ReadMemberInfo(ObjectReference objReference)
		{
			objReference.Value = ReadMemberInfo();
		}

		private MemberInfo ReadMemberInfo()
		{
			var memberName = ReadLine();

			var reflectedType = ReadType();
			var memberType = (MemberTypes)Enum.Parse(typeof(MemberTypes), ReadLine());

			var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
			switch (memberType)
			{
				case MemberTypes.Method:
					{
						var parameterCount = Int32.Parse(ReadLine());
						var parameters = new Type[parameterCount];

						for (int i = 0; i < parameterCount; i++)
							parameters[i] = ReadType();

						return reflectedType.GetMethod(memberName, bindingFlags, null, parameters, null);
					}
				case MemberTypes.Field:
					return reflectedType.GetField(memberName, bindingFlags);
				case MemberTypes.Property:
					return reflectedType.GetProperty(memberName, bindingFlags);
			}

			return null;
		}

		private void ReadDelegate(ObjectReference objReference)
		{
			objReference.Value = ReadDelegate();
		}

		private Delegate ReadDelegate()
		{
			var delegateType = ReadType();
			var methodInfo = ReadMemberInfo() as MethodInfo;

			if (ReadLine() == "target")
				return Delegate.CreateDelegate(delegateType, StartRead().Value, methodInfo);
			else
				return Delegate.CreateDelegate(delegateType, methodInfo);
		}

		private void ReadUnusedMarker(ObjectReference objReference)
		{
			var type = ReadType();
			if (type == typeof(int))
				objReference.Value = UnusedMarker.Integer;
			if (type == typeof(uint))
				objReference.Value = UnusedMarker.UnsignedInteger;
			else if (type == typeof(float))
				objReference.Value = UnusedMarker.Float;
			else if (type == typeof(Vec3))
				objReference.Value = UnusedMarker.Vec3;
		}

		private void ReadType(ObjectReference objReference)
		{
			objReference.Value = ReadType();
		}

		private Type ReadType()
		{
			bool isGeneric = bool.Parse(ReadLine());

			var type = GetType(ReadLine());

			if (isGeneric)
			{
				var numGenericArgs = int.Parse(ReadLine());
				var genericArgs = new Type[numGenericArgs];
				for (int i = 0; i < numGenericArgs; i++)
					genericArgs[i] = ReadType();

				return type.MakeGenericType(genericArgs);
			}

			return type;
		}

		private Type GetType(string typeName)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (typeName == null)
				throw new ArgumentNullException("typeName");
			if (typeName.Length == 0)
				throw new ArgumentException("typeName cannot have zero length");
#endif

			if (typeName.Contains('+'))
			{
				var splitString = typeName.Split('+');
				var ownerType = GetType(splitString.FirstOrDefault());

				return ownerType.Assembly.GetType(typeName);
			}

			Type type = Type.GetType(typeName);
			if (type != null)
				return type;

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(typeName);
				if (type != null)
					return type;
			}

			throw new TypeLoadException(string.Format("Could not localize type with name {0}", typeName));
		}

		private static Type GetIEnumerableElementType(Type enumerableType)
		{
			Type type = enumerableType.GetElementType();
			if (type != null)
				return type;

			// Not an array type, we've got to use an alternate method to get the type of elements
			// contained within.
			if (enumerableType.IsGenericType && enumerableType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				type = enumerableType.GetGenericArguments()[0];
			else
			{
				Type[] interfaces = enumerableType.GetInterfaces();
				foreach (Type i in interfaces)
					if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
						type = i.GetGenericArguments()[0];
			}

			return type;
		}

		#region ICrySerializable
		public void BeginGroup(string name) { throw new NotImplementedException(); }
		public void EndGroup() { throw new NotImplementedException(); }

		public void Value(string name, ref string obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (string)StartRead().Value;
		}

		public void Value(string name, ref int obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (int)StartRead().Value;
		}

		public void Value(string name, ref uint obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (uint)StartRead().Value;
		}

		public void Value(string name, ref bool obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (bool)StartRead().Value;
		}

		public void Value(string name, ref EntityId obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (EntityId)StartRead().Value;
		}

		public void Value(string name, ref float obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (float)StartRead().Value;
		}

		public void Value(string name, ref Vec3 obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (Vec3)StartRead().Value;
		}

		public void Value(string name, ref Quat obj, string policy = null)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (Quat)StartRead().Value;
		}

		public void EnumValue(string name, ref int obj, int first, int last)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (int)StartRead().Value;
		}

		public void EnumValue(string name, ref uint obj, uint first, uint last)
		{
			if (IsWriting)
				StartWrite(new ObjectReference(name, obj));
			else
				obj = (uint)StartRead().Value;
		}

		public void FlagPartialRead() { throw new NotImplementedException(); }

		public bool IsReading { get { return Reader != null; } }
		public bool IsWriting { get { return Writer != null; } }

		public SerializationTarget Target { get { return SerializationTarget.RealtimeScripting; } }
		#endregion

		private int m_currentLine;

		public SerializationBinder Binder { get { return null; } set { } }
		public ISurrogateSelector SurrogateSelector { get { return null; } set { } }
		public StreamingContext Context { get { return new StreamingContext(StreamingContextStates.Persistence); } set { } }
	}
}