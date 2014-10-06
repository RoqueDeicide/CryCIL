using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using CryEngine.Entities;
using CryEngine.Extensions;
using CryEngine.Logic.Entities;
using CryEngine.Mathematics;
using CryEngine.Utilities;

namespace CryEngine.RunTime.Serialization
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
			this.Converter = new FormatterConverter();
			this.ObjectReferences = new Dictionary<int, ObjectReference>();

#if !UNIT_TESTING
			var debugCVar = CVar.Get("mono_realtimeScriptingDebug");
			if (debugCVar != null)
				this.IsDebugModeEnabled = (debugCVar.IVal != 0);
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

			this.Writer = new StreamWriter(stream) { AutoFlush = true };
			this.Reader = null;

			this.ObjectReferences.Clear();
			this.m_currentLine = 0;

			this.StartWrite(new ObjectReference("root", graph));
			stream.Seek(0, SeekOrigin.Begin);
		}

		private void WriteLine(object value)
		{
			this.m_currentLine++;
			this.Writer.WriteLine(value);
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
				foreach (var pair in this.ObjectReferences)
				{
					if (pair.Value.Value.Equals(objectReference.Value))
					{
						this.WriteReference(objectReference, pair.Key);
						return true;
					}
				}

				this.ObjectReferences.Add(this.m_currentLine, objectReference);
			}

			return false;
		}

		/// <summary>
		/// Starts writing the specified reference.
		/// </summary>
		/// <param name="objectReference"></param>
		public void StartWrite(ObjectReference objectReference)
		{
			this.WriteLine(objectReference.Name);

			if (this.TryWriteReference(objectReference))
				return;

			this.WriteLine((int)objectReference.SerializationType);

			switch (objectReference.SerializationType)
			{
				case SerializationType.Null:
					break;
				case SerializationType.IntPtr:
					this.WriteIntPtr(objectReference);
					break;
				case SerializationType.Any:
					this.WriteAny(objectReference);
					break;
				case SerializationType.String:
					this.WriteString(objectReference);
					break;
				case SerializationType.Array:
					this.WriteArray(objectReference);
					break;
				case SerializationType.Enumerable:
					this.WriteEnumerable(objectReference);
					break;
				case SerializationType.GenericEnumerable:
					this.WriteGenericEnumerable(objectReference);
					break;
				case SerializationType.Enum:
					this.WriteEnum(objectReference);
					break;
				case SerializationType.Type:
					this.WriteType(objectReference);
					break;
				case SerializationType.Delegate:
					this.WriteDelegate(objectReference);
					break;
				case SerializationType.MemberInfo:
					this.WriteMemberInfo(objectReference);
					break;
				case SerializationType.Object:
					this.WriteObject(objectReference);
					break;
				case SerializationType.UnusedMarker:
					this.WriteUnusedMarker(objectReference);
					break;
			}
		}

		private void WriteIntPtr(ObjectReference objectReference)
		{
			this.WriteLine(((IntPtr)objectReference.Value).ToInt64());
		}

// ReSharper disable UnusedParameter.Local
		private void WriteReference(ObjectReference objReference, int line)
// ReSharper restore UnusedParameter.Local
		{
			this.WriteLine(SerializationType.Reference);
			this.WriteLine(line);
		}

		private void WriteAny(ObjectReference objectReference)
		{
			this.WriteType(objectReference.Value.GetType());
			this.WriteLine(objectReference.Value);
		}

		private void WriteString(ObjectReference objectReference)
		{
			this.WriteLine(objectReference.Value);
		}

		private void WriteEnum(ObjectReference objectReference)
		{
			this.WriteType(objectReference.Value.GetType());
			this.WriteLine(objectReference.Value);
		}

		private void WriteArray(ObjectReference objectReference)
		{
			var array = objectReference.Value as Array;
			var numElements = array.Length;
			this.WriteLine(numElements);

			this.WriteType(array.GetType().GetElementType());

			for (int i = 0; i < numElements; i++)
				this.StartWrite(new ObjectReference(i.ToString(), array.GetValue(i)));
		}

		private void WriteEnumerable(ObjectReference objectReference)
		{
			var array = ((IEnumerable)objectReference.Value).Cast<object>().ToArray();
			var numElements = array.Length;
			this.WriteLine(numElements);

			this.WriteType(GetIEnumerableElementType(objectReference.Value.GetType()));

			for (int i = 0; i < numElements; i++)
				this.StartWrite(new ObjectReference(i.ToString(CultureInfo.InvariantCulture), array[i]));
		}

		private void WriteGenericEnumerable(ObjectReference objectReference)
		{
			var enumerable = ((IEnumerable)objectReference.Value).Cast<object>().ToArray();

			this.WriteLine(enumerable.Count());

			var type = objectReference.Value.GetType();
			this.WriteType(type);

			if (type.Implements<IDictionary>())
			{
				int i = 0;
				foreach (var element in enumerable)
				{
					this.StartWrite(new ObjectReference("key_" + i, element.GetType().GetProperty("Key").GetValue(element, null)));
					this.StartWrite(new ObjectReference("value_" + i, element.GetType().GetProperty("Value").GetValue(element, null)));
					i++;
				}
			}
			else
			{
				for (int i = 0; i < enumerable.Count(); i++)
					this.StartWrite(new ObjectReference(i.ToString(), enumerable.ElementAt(i)));
			}
		}

		private void WriteObject(ObjectReference objectReference)
		{
			var type = objectReference.Value.GetType();
			this.WriteType(type);

			if (type.Implements<ICrySerializable>())
			{
				var crySerializable = objectReference.Value as ICrySerializable;
				crySerializable.Serialize(this);

				return;
			}

			while (type != null)
			{
				var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				this.WriteLine(fields.Length);
				foreach (var field in fields)
					this.StartWrite(new ObjectReference(field.Name, field.GetValue(objectReference.Value)));

				var events = type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				this.WriteLine(events.Length);
				foreach (var eventInfo in events)
				{
					this.WriteLine(eventInfo.Name);
					this.WriteType(eventInfo.EventHandlerType);

					var eventFieldInfo = type.GetField(eventInfo.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
					var eventFieldValue = (Delegate)eventFieldInfo.GetValue(objectReference.Value);
					if (eventFieldValue != null)
					{
						var delegates = eventFieldValue.GetInvocationList();

						this.WriteLine(delegates.Length);
						foreach (var eventDelegate in delegates)
							this.WriteDelegate(eventDelegate);
					}
					else
						this.WriteLine(0);
				}

				type = type.BaseType;
			}
		}

		private void WriteMemberInfo(ObjectReference objectReference)
		{
			var memberInfo = objectReference.Value as MemberInfo;
			this.WriteMemberInfo(memberInfo);
		}

		private void WriteMemberInfo(MemberInfo memberInfo)
		{
			this.WriteLine(memberInfo.Name);
			this.WriteType(memberInfo.ReflectedType);
			this.WriteLine(memberInfo.MemberType);

			if (memberInfo.MemberType == MemberTypes.Method)
			{
				var methodInfo = memberInfo as MethodInfo;

				var parameters = methodInfo.GetParameters();
				this.WriteLine(parameters.Length);
				foreach (var parameter in parameters)
					this.WriteType(parameter.ParameterType);
			}
		}

		private void WriteDelegate(ObjectReference objectReference)
		{
			this.WriteDelegate(objectReference.Value as Delegate);
		}

		private void WriteDelegate(Delegate _delegate)
		{
			this.WriteType(_delegate.GetType());
			this.WriteMemberInfo(_delegate.Method);
			if (_delegate.Target != null)
			{
				this.WriteLine("target");
				this.StartWrite(new ObjectReference("delegateTarget", _delegate.Target));
			}
			else
				this.WriteLine("null_target");
		}

		private void WriteUnusedMarker(ObjectReference objectReference)
		{
			this.WriteType(objectReference.Value.GetType());
		}

		private void WriteType(ObjectReference objectReference)
		{
			this.WriteType(objectReference.Value as Type);
		}

		private void WriteType(Type type)
		{
			this.WriteLine(type.IsGenericType);

			if (type.IsGenericType)
			{
				this.WriteLine(type.GetGenericTypeDefinition().FullName);

				var genericArgs = type.GetGenericArguments();
				this.WriteLine(genericArgs.Length);
				foreach (var genericArg in genericArgs)
					this.WriteType(genericArg);
			}
			else
				this.WriteLine(type.FullName);
		}

		public object Deserialize(Stream stream)
		{
#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (stream == null || stream.Length == 0)
				throw new ArgumentNullException("stream");
#endif

			this.Reader = new StreamReader(stream);
			this.Writer = null;

			this.ObjectReferences.Clear();
			this.m_currentLine = 0;

			return this.StartRead().Value;
		}

		private string ReadLine()
		{
			this.m_currentLine++;

			return this.Reader.ReadLine();
		}

		/// <summary>
		/// Starts reading an reference that was written with <see cref="StartWrite" />.
		/// </summary>
		/// <returns></returns>
		public ObjectReference StartRead()
		{
			var name = this.ReadLine();

			int line = this.m_currentLine;

			var serializationType = (SerializationType)Enum.Parse(typeof(SerializationType), this.ReadLine());
			var objReference = new ObjectReference(name, serializationType);

			if (serializationType > SerializationType.ReferenceTypes)
				this.ObjectReferences.Add(line, objReference);

			switch (serializationType)
			{
				case SerializationType.Null: break;
				case SerializationType.Reference: this.ReadReference(objReference); break;
				case SerializationType.Object: this.ReadObject(objReference); break;
				case SerializationType.Array: this.ReadArray(objReference); break;
				case SerializationType.GenericEnumerable: this.ReadGenericEnumerable(objReference); break;
				case SerializationType.Enumerable: this.ReadEnumerable(objReference); break;
				case SerializationType.Enum: this.ReadEnum(objReference); break;
				case SerializationType.Any: this.ReadAny(objReference); break;
				case SerializationType.String: this.ReadString(objReference); break;
				case SerializationType.MemberInfo: this.ReadMemberInfo(objReference); break;
				case SerializationType.Type: this.ReadType(objReference); break;
				case SerializationType.Delegate: this.ReadDelegate(objReference); break;
				case SerializationType.IntPtr: this.ReadIntPtr(objReference); break;
				case SerializationType.UnusedMarker: this.ReadUnusedMarker(objReference); break;
			}

#if !(RELEASE && RELEASE_DISABLE_CHECKS)
			if (!objReference.AllowNull && objReference.Value == null && serializationType != SerializationType.Null)
				throw new SerializationException(string.Format("Failed to deserialize object of type {0} {1} at line {2}!", objReference.SerializationType, objReference.Name, line));
#endif

			return objReference;
		}

		private void ReadIntPtr(ObjectReference objReference)
		{
			objReference.Value = new IntPtr(Int64.Parse(this.ReadLine()));
		}

		private void ReadReference(ObjectReference objReference)
		{
			int referenceLine = int.Parse(this.ReadLine());

			ObjectReference originalReference;
			if (!this.ObjectReferences.TryGetValue(referenceLine, out originalReference))
				throw new SerializationException(string.Format("Failed to obtain reference {0} at line {1}! Last line was {2})", objReference.Name, referenceLine, this.m_currentLine));

			objReference.Value = originalReference.Value;
			objReference.AllowNull = originalReference.AllowNull;
		}

		private void ReadObject(ObjectReference objReference)
		{
			var type = this.ReadType();

			objReference.Value = FormatterServices.GetUninitializedObject(type);

			if (type.Implements<ICrySerializable>())
			{
				var crySerializable = objReference.Value as ICrySerializable;
				crySerializable.Serialize(this);

				return;
			}

			while (type != null)
			{
				var numFields = int.Parse(this.ReadLine());
				for (int i = 0; i < numFields; i++)
				{
					var fieldReference = this.StartRead();

					if (objReference.Value == null)
						continue;

					var fieldInfo = type.GetField(fieldReference.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

					if (fieldInfo != null)
						fieldInfo.SetValue(objReference.Value, fieldReference.Value);
					else if (this.IsDebugModeEnabled)
						throw new MissingFieldException(string.Format("Failed to find field {0} in type {1}", fieldReference.Name, type.Name));
				}

				var numEvents = int.Parse(this.ReadLine());

				for (int i = 0; i < numEvents; i++)
				{
					var eventName = this.ReadLine();

					var eventInfo = type.GetEvent(eventName);
// ReSharper disable UnusedVariable
					var eventHandlerType = this.ReadType();
// ReSharper restore UnusedVariable

					var numDelegates = Int32.Parse(this.ReadLine());
					for (int iDelegate = 0; iDelegate < numDelegates; iDelegate++)
					{
						var foundDelegate = this.ReadDelegate();

						eventInfo.AddEventHandler(objReference.Value, foundDelegate);
					}
				}

				type = type.BaseType;
			}
		}

		private void ReadArray(ObjectReference objReference)
		{
			var numElements = int.Parse(this.ReadLine());
			var type = this.ReadType();

			objReference.Value = Array.CreateInstance(type, numElements);
			var array = objReference.Value as Array;

			for (int i = 0; i != numElements; ++i)
				array.SetValue(this.StartRead().Value, i);
		}

		private void ReadEnumerable(ObjectReference objReference)
		{
			var numElements = int.Parse(this.ReadLine());
			var type = this.ReadType();

			objReference.Value = Array.CreateInstance(type, numElements);
			var array = objReference.Value as Array;

			for (int i = 0; i != numElements; ++i)
				array.SetValue(this.StartRead().Value, i);
		}

		private void ReadGenericEnumerable(ObjectReference objReference)
		{
			int elements = int.Parse(this.ReadLine());

			var type = this.ReadType();

			objReference.Value = Activator.CreateInstance(type);

			if (type.Implements<IDictionary>())
			{
				var dict = objReference.Value as IDictionary;

				for (int i = 0; i < elements; i++)
				{
					var key = this.StartRead().Value;
					var value = this.StartRead().Value;

					dict.Add(key, value);
				}
			}
			else if (type.Implements<IList>())
			{
				var list = objReference.Value as IList;

				for (int i = 0; i < elements; i++)
					if (list != null) list.Add(this.StartRead().Value);
			}
			else if (type.ImplementsGeneric(typeof(ISet<>)) || type.ImplementsGeneric(typeof(ICollection<>)))
			{
// ReSharper disable UnusedVariable
				var set = objReference.Value;
// ReSharper restore UnusedVariable

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
					addMethod.Invoke(objReference.Value, new [] { this.StartRead().Value });
				}
			}
			else if (this.IsDebugModeEnabled)
				throw new SerializationException(string.Format("Failed to serialize generic enumerable of type {0}, not supported by implementation", type.Name));
		}

		private void ReadAny(ObjectReference objReference)
		{
			var type = this.ReadType();
			string valueString = this.ReadLine();

			objReference.Value =
				!string.IsNullOrEmpty(valueString)
				? this.Converter.Convert(valueString, type)
				: 0;
		}

		private void ReadString(ObjectReference objReference)
		{
			objReference.Value = this.ReadLine();
		}

		private void ReadEnum(ObjectReference objReference)
		{
			var type = this.ReadType();
			string valueString = this.ReadLine();

			objReference.Value = Enum.Parse(type, valueString);
		}

		private void ReadMemberInfo(ObjectReference objReference)
		{
			objReference.Value = this.ReadMemberInfo();
		}

		private MemberInfo ReadMemberInfo()
		{
			var memberName = this.ReadLine();

			var reflectedType = this.ReadType();
			var memberType = (MemberTypes)Enum.Parse(typeof(MemberTypes), this.ReadLine());

			const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
			switch (memberType)
			{
				case MemberTypes.Method:
					{
						var parameterCount = Int32.Parse(this.ReadLine());
						var parameters = new Type[parameterCount];

						for (int i = 0; i < parameterCount; i++)
							parameters[i] = this.ReadType();

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
			objReference.Value = this.ReadDelegate();
		}

		private Delegate ReadDelegate()
		{
			var delegateType = this.ReadType();
			var methodInfo = (MethodInfo)this.ReadMemberInfo();

			return this.ReadLine() == "target"
				? Delegate.CreateDelegate(delegateType, this.StartRead().Value, methodInfo)
				: Delegate.CreateDelegate(delegateType, methodInfo);
		}

		private void ReadUnusedMarker(ObjectReference objReference)
		{
			var type = this.ReadType();
			if (type == typeof(int))
				objReference.Value = UnusedMarker.Integer;
			if (type == typeof(uint))
				objReference.Value = UnusedMarker.UnsignedInteger;
			else if (type == typeof(float))
				objReference.Value = UnusedMarker.Float;
			else if (type == typeof(Vector3))
				objReference.Value = UnusedMarker.Vector3;
		}

		private void ReadType(ObjectReference objReference)
		{
			objReference.Value = this.ReadType();
		}

		private Type ReadType()
		{
			bool isGeneric = bool.Parse(this.ReadLine());

			var type = this.GetType(this.ReadLine());

			if (isGeneric)
			{
				var numGenericArgs = int.Parse(this.ReadLine());
				var genericArgs = new Type[numGenericArgs];
				for (int i = 0; i < numGenericArgs; i++)
					genericArgs[i] = this.ReadType();

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
				var ownerType = this.GetType(splitString.FirstOrDefault());

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
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (string)this.StartRead().Value;
		}

		public void Value(string name, ref int obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (int)this.StartRead().Value;
		}

		public void Value(string name, ref uint obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (uint)this.StartRead().Value;
		}

		public void Value(string name, ref bool obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (bool)this.StartRead().Value;
		}

		public void Value(string name, ref EntityId obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (EntityId)this.StartRead().Value;
		}

		public void Value(string name, ref float obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (float)this.StartRead().Value;
		}

		public void Value(string name, ref Vector3 obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (Vector3)this.StartRead().Value;
		}

		public void Value(string name, ref Quaternion obj, string policy = null)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (Quaternion)this.StartRead().Value;
		}

		public void EnumValue(string name, ref int obj, int first, int last)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (int)this.StartRead().Value;
		}

		public void EnumValue(string name, ref uint obj, uint first, uint last)
		{
			if (this.IsWriting)
				this.StartWrite(new ObjectReference(name, obj));
			else
				obj = (uint)this.StartRead().Value;
		}

		public void FlagPartialRead() { throw new NotImplementedException(); }

		public bool IsReading { get { return this.Reader != null; } }
		public bool IsWriting { get { return this.Writer != null; } }

		public SerializationTarget Target { get { return SerializationTarget.RealtimeScripting; } }
		#endregion

		private int m_currentLine;

		public SerializationBinder Binder { get { return null; } set { } }
		public ISurrogateSelector SurrogateSelector { get { return null; } set { } }
		public StreamingContext Context { get { return new StreamingContext(StreamingContextStates.Persistence); } set { } }
	}
}