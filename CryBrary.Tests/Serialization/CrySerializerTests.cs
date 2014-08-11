using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryEngine.Mathematics;
using CryEngine.Serialization;
using NUnit.Framework;

namespace CryBrary.Tests.Serialization
{
	public class CrySerializerTests
	{
		public class TestClass
		{
			public class NestedClass
			{
				public NestedClass() { }

				public NestedClass(NestedEnum enumVal)
				{
					NestedEnum = enumVal;
				}

				public NestedEnum NestedEnum { get; set; }
			}

			public enum NestedEnum
			{
				Nested_Quite,
				Nested_NotQuite
			}

			public string String { get; set; }
			public int Integer { get; set; }
			public bool Boolean { get; set; }

			public string NullString { get; set; }

			public NestedClass nestedClass { get; set; }
		}

		private static TestClass SetupTestClass()
		{
			var testClass = new TestClass { Integer = 3, String = "testString", Boolean = true, nestedClass = new TestClass.NestedClass(TestClass.NestedEnum.Nested_NotQuite) };

			return testClass;
		}

		[Test]
		public void TestClass_With_MemoryStream()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();
				serializer.Serialize(stream, SetupTestClass());

				serializer = new CrySerializer();

				var testClass = serializer.Deserialize(stream) as TestClass;
				Assert.NotNull(testClass);

				Assert.True(testClass.Boolean);
				Assert.AreEqual(3, testClass.Integer);
				Assert.AreEqual("testString", testClass.String);

				Assert.NotNull(testClass.nestedClass);

				Assert.AreEqual(testClass.nestedClass.NestedEnum, TestClass.NestedEnum.Nested_NotQuite);
			}
		}

		[Test]
		public void String_With_MemoryStream()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();
				serializer.Serialize(stream, "Test str1ng_I5 V37y tEsTy%‹Œm´ð!");

				serializer = new CrySerializer();

				var testString = serializer.Deserialize(stream) as string;

				Assert.AreEqual("Test str1ng_I5 V37y tEsTy%‹Œm´ð!", testString);
			}
		}

		[Test]
		public void List_With_MemoryStream()
		{
			using (var stream = new MemoryStream())
			{
				var list = new List<string> { "test1", "test2" };

				var serializer = new CrySerializer();
				serializer.Serialize(stream, list);

				serializer = new CrySerializer();

				var deserialized = serializer.Deserialize(stream) as List<string>;

				Assert.NotNull(deserialized);
				Assert.AreEqual(2, deserialized.Count());

				Assert.AreEqual("test1", deserialized.ElementAt(0));
				Assert.AreEqual("test2", deserialized.ElementAt(1));
			}
		}

		[Test]
		public void Dictionary_With_MemoryStream()
		{
			using (var stream = new MemoryStream())
			{
				var dictionary = new Dictionary<string, int> { { "test1", 1 }, { "test2", 2 } };

				var serializer = new CrySerializer();
				serializer.Serialize(stream, dictionary);

				serializer = new CrySerializer();

				var deserializedDictionary = serializer.Deserialize(stream) as Dictionary<string, int>;
				Assert.NotNull(deserializedDictionary);

				Assert.AreEqual(2, deserializedDictionary.Count);

				Assert.AreEqual(2, deserializedDictionary.Count);

				var firstKey = deserializedDictionary.First().Key;
				Assert.AreEqual("test1", firstKey);
				Assert.AreEqual(1, deserializedDictionary[firstKey]);

				var secondKey = deserializedDictionary.ElementAt(1).Key;
				Assert.AreEqual("test2", secondKey);
				Assert.AreEqual(2, deserializedDictionary[secondKey]);
			}
		}

		[Test]
		public void Object_Array_With_MemoryStream()
		{
			using (var stream = new MemoryStream())
			{
				var list = new List<object> { "testString", 1337, true };

				var serializer = new CrySerializer();
				serializer.Serialize(stream, list.ToArray());

				serializer = new CrySerializer();

				var array = serializer.Deserialize(stream) as object[];
				Assert.NotNull(array);
				Assert.IsNotEmpty(array);

				Assert.AreEqual("testString", array.ElementAt(0));
				Assert.AreEqual(1337, array.ElementAt(1));
				Assert.AreEqual(true, array.ElementAt(2));
			}
		}

		[Test]
		public void String_Array_With_MemoryStream()
		{
			using (var stream = new MemoryStream())
			{
				var list = new List<object> { "first_string", "second_string", "third_string" };

				var serializer = new CrySerializer();
				serializer.Serialize(stream, list.ToArray());

				serializer = new CrySerializer();

				var array = serializer.Deserialize(stream) as object[];
				Assert.NotNull(array);
				Assert.IsNotEmpty(array);

				Assert.AreEqual("first_string", array.ElementAt(0));
				Assert.AreEqual("second_string", array.ElementAt(1));
				Assert.AreEqual("third_string", array.ElementAt(2));
			}
		}

		private class Multiple_Reference_Test_Class
		{
			public Multiple_Reference_Test_Class()
			{
				ClassWithTestClassReference = new Class_Containing_Reference();
				TestClassReference = ClassWithTestClassReference.TestClass;

				TestClassSeperate = SetupTestClass();
			}

			public class Class_Containing_Reference
			{
				public Class_Containing_Reference()
				{
					TestClass = SetupTestClass();
				}

				public TestClass TestClass { get; private set; }
			}

			public Class_Containing_Reference ClassWithTestClassReference { get; private set; }
			public TestClass TestClassReference { get; private set; }

			public TestClass TestClassSeperate { get; private set; }
		}

		[Test]
		public void Reference_Object_Serialization()
		{
			using (var stream = new MemoryStream())
			{
				var referenceTestClass = new Multiple_Reference_Test_Class();

				var serializer = new CrySerializer();
				serializer.Serialize(stream, referenceTestClass);

				serializer = new CrySerializer();

				referenceTestClass = serializer.Deserialize(stream) as Multiple_Reference_Test_Class;

				Assert.AreNotSame(referenceTestClass.ClassWithTestClassReference, referenceTestClass.TestClassSeperate);
				Assert.AreEqual(referenceTestClass.ClassWithTestClassReference.TestClass, referenceTestClass.TestClassReference);
				/*Assert.AreEqual(referenceTestClass.ClassWithTestClassReference.TestClass, referenceTestClass.TestClassReference, "Objects were not the same; expected hash code: {0} but was: {1}",
					referenceTestClass.ClassWithTestClassReference.GetHashCode(), referenceTestClass.TestClassReference.GetHashCode());*/
			}
		}

		private class Class_With_MemberInfo_Member
		{
			public Class_With_MemberInfo_Member()
			{
				MethodInfo = GetType().GetMethod("Method");
				FieldInfo = GetType().GetField("booleanField");

				booleanField = true;
			}

			public void Method() { }

			public System.Reflection.MethodInfo MethodInfo { get; private set; }
			public System.Reflection.FieldInfo FieldInfo { get; private set; }

			public bool booleanField;
		}

		[Test]
		public void Class_With_MemberInfo_Members()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				serializer.Serialize(stream, new Class_With_MemberInfo_Member());

				serializer = new CrySerializer();

				var memberInfoClass = serializer.Deserialize(stream) as Class_With_MemberInfo_Member;

				Assert.NotNull(memberInfoClass);

				Assert.AreSame(memberInfoClass.GetType().GetMethod("Method"), memberInfoClass.MethodInfo);
				Assert.AreSame(memberInfoClass.GetType().GetField("booleanField"), memberInfoClass.FieldInfo);

				Assert.True(memberInfoClass.booleanField);
			}
		}

		public interface Interface
		{
			void PureVirtualMethod();
		}

		public abstract class BaseClass : Interface
		{
			public virtual void NonoverriddenMethod()
			{
			}

			public virtual void OverriddenMethod()
			{
			}

			public void PureVirtualMethod()
			{
			}

			public bool BooleanProperty { get; set; }
			public int IntegerProperty { get; set; }
		}

		public class Class_Inherit_From_BaseClass : BaseClass
		{
			public override void OverriddenMethod()
			{
			}

			public string StringProperty { get; set; }
		}

		public class Class_Inherit_From_Class : Class_Inherit_From_BaseClass
		{
			public Class_Inherit_From_Class()
			{
				BooleanProperty = true;
				IntegerProperty = 13;
				StringProperty = "TestString";

				this.Vector3Property = new Vector3(1, 2, 3);
			}

			public Vector3 Vector3Property { get; set; }
		}

		[Test]
		public void Derivation()
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				serializer.Serialize(stream, new Class_Inherit_From_Class());

				serializer = new CrySerializer();

				var inheritClass = serializer.Deserialize(stream) as Class_Inherit_From_Class;

				Assert.NotNull(inheritClass);
				Assert.True(inheritClass.BooleanProperty);
				Assert.AreEqual(13, inheritClass.IntegerProperty);
				Assert.AreEqual("TestString", inheritClass.StringProperty);
				Assert.AreEqual(new Vector3(1, 2, 3), inheritClass.Vector3Property);
			}
		}

		[Test]
		public void GenericEnumerableception()
		{
			var dictionary = new Dictionary<int, List<TestClass>>();
			for (int i = 0; i < 10; i++)
			{
				var list = new List<TestClass> { SetupTestClass(), null };

				dictionary.Add(i, list);
			}

			using (var stream = new MemoryStream())
			{
				var serializer = new CrySerializer();

				serializer.Serialize(stream, dictionary);

				serializer = new CrySerializer();

				var deserializedDictionary = serializer.Deserialize(stream) as Dictionary<int, List<TestClass>>;

				Assert.NotNull(deserializedDictionary);
			}
		}
	}
}