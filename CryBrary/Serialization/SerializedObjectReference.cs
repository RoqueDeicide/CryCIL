using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Reflection;
using System.Threading;
using System.Linq;

using CryEngine.Extensions;
using CryEngine.Mathematics;
using CryEngine.Utilities;

namespace CryEngine.Serialization
{
	public class ObjectReference
	{
		static ObjectReference()
		{
			mForbiddenTypes = new[]
			{
				typeof(Stream),
				typeof(StreamWriter),
				typeof(Thread)
			};
		}

		/// <summary>
		/// Types that we can't serialize
		/// </summary>
		private static readonly Type[] mForbiddenTypes;

		public ObjectReference(string name, SerializationType type)
		{
			this.Name = name;
			this.SerializationType = type;
		}

		public ObjectReference(string name, object value)
		{
			this.Name = name;
			Value = value;

			Type valueType = this.Value != null ? this.Value.GetType() : null;
			if (valueType == null)
				this.SerializationType = SerializationType.Null;
			else if (valueType == typeof(IntPtr))
				this.SerializationType = SerializationType.IntPtr;
			else if (valueType.IsPrimitive)
			{
				if (this.Value is int && UnusedMarker.IsUnused((int)this.Value))
					this.SerializationType = SerializationType.UnusedMarker;
				else if (this.Value is uint && UnusedMarker.IsUnused((uint)this.Value))
					this.SerializationType = SerializationType.UnusedMarker;
				else if (this.Value is float && UnusedMarker.IsUnused((float)this.Value))
					this.SerializationType = SerializationType.UnusedMarker;
				else
					this.SerializationType = SerializationType.Any;
			}
			else if (valueType == typeof(string))
				this.SerializationType = SerializationType.String;
			else if (valueType.IsArray)
				this.SerializationType = SerializationType.Array;
			else if (valueType.IsEnum)
				this.SerializationType = SerializationType.Enum;
			else if (valueType.Implements<IList>() || valueType.Implements<IDictionary>())
			{
				this.SerializationType =
					valueType.IsGenericType
					? SerializationType.GenericEnumerable
					: SerializationType.Enumerable;
			}
			else
			{
				if (this.Value is Type)
					this.SerializationType = SerializationType.Type;
				else if (valueType.Implements<Delegate>())
					this.SerializationType = SerializationType.Delegate;
				else if (valueType.Implements<MemberInfo>())
					this.SerializationType = SerializationType.MemberInfo;
				else
				{
					if (this.Value is Vector3 && UnusedMarker.IsUnused((Vector3)this.Value))
						this.SerializationType = SerializationType.UnusedMarker;
					else if (mForbiddenTypes.Contains(valueType))
						this.SerializationType = SerializationType.Null;
					else
						this.SerializationType = SerializationType.Object;
				}
			}
		}

		public string Name { get; private set; }

		public object Value { get; set; }

		public SerializationType SerializationType { get; set; }

		public bool AllowNull { get; set; }
	}
}