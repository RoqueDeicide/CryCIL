using System;

namespace CryEngine.RunTime.Testing
{
	/// <summary>
	/// Indicates that a class should be loaded by the unit tester.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class TestCollectionAttribute : Attribute
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public TestCollectionAttribute()
		{
		}

		public TestCollectionAttribute(string name)
		{
			this.Name = name;
		}

		public TestCollectionAttribute(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}
	}

	/// <summary>
	/// Indicates that a method should be run as a test. Requires that the method belong to a class
	/// decorated with a TestCollectionAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class TestAttribute : Attribute
	{
		public string Name { get; set; }
		public string Description { get; set; }

		public TestAttribute()
		{
		}

		public TestAttribute(string name)
		{
			this.Name = name;
		}

		public TestAttribute(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}
	}

	/// <summary>
	/// Indicates that a test should be ignored. Ignored tests will not be reported as a success; they will
	/// be reported as ignored.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class IgnoreTestAttribute : Attribute { }
}