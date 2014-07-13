using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;

using CryEngine.Extensions;
using CryEngine.Utilities;

namespace CryEngine.Serialization
{
	public class ObjectReference
	{
		static ObjectReference()
		{
			m_forbiddenTypes = new Type[]
			{
				typeof(Stream),
				typeof(StreamWriter),
				typeof(Thread)
			};
		}

		/// <summary>
		/// Types that we can't serialize
		/// </summary>
		private static Type[] m_forbiddenTypes;

		public ObjectReference(string name, SerializationType type)
		{
			m_name = name;
			m_serializationType = type;
		}

		public ObjectReference(string name, object value)
		{
			m_name = name;
			Value = value;

			Type valueType = m_value != null ? m_value.GetType() : null;
			if (valueType == null)
				m_serializationType = SerializationType.Null;
			else if (valueType == typeof(IntPtr))
				m_serializationType = SerializationType.IntPtr;
			else if (valueType.IsPrimitive)
			{
				if (m_value is int && UnusedMarker.IsUnused((int)m_value))
					m_serializationType = SerializationType.UnusedMarker;
				else if (m_value is uint && UnusedMarker.IsUnused((uint)m_value))
					m_serializationType = SerializationType.UnusedMarker;
				else if (m_value is float && UnusedMarker.IsUnused((float)m_value))
					m_serializationType = SerializationType.UnusedMarker;
				else
					m_serializationType = SerializationType.Any;
			}
			else if (valueType == typeof(string))
				m_serializationType = SerializationType.String;
			else if (valueType.IsArray)
				m_serializationType = SerializationType.Array;
			else if (valueType.IsEnum)
				m_serializationType = SerializationType.Enum;
			else if (valueType.Implements<IList>() || valueType.Implements<IDictionary>())
			{
				if (valueType.IsGenericType)
					m_serializationType = SerializationType.GenericEnumerable;
				else
					m_serializationType = SerializationType.Enumerable;
			}
			else
			{
				if (m_value is Type)
					m_serializationType = SerializationType.Type;
				else if (valueType.Implements<Delegate>())
					m_serializationType = SerializationType.Delegate;
				else if (valueType.Implements<MemberInfo>())
					m_serializationType = SerializationType.MemberInfo;
				else
				{
					if (m_value is Vec3 && UnusedMarker.IsUnused((Vec3)m_value))
						m_serializationType = SerializationType.UnusedMarker;
					else if (m_forbiddenTypes.Contains(valueType))
						m_serializationType = SerializationType.Null;
					else
						m_serializationType = SerializationType.Object;
				}
			}
		}

		private string m_name;
		public string Name { get { return m_name; } }

		private object m_value;
		public object Value { get { return m_value; } set { m_value = value; } }

		private SerializationType m_serializationType;
		public SerializationType SerializationType { get { return m_serializationType; } set { m_serializationType = value; } }

		public bool AllowNull { get; set; }
	}
}