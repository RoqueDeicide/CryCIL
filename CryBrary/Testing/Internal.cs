using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CryEngine.Extensions;

namespace CryEngine.Testing
{
	public sealed class TestCollection
	{
		public object Instance { get; set; }
		public IEnumerable<MethodInfo> Tests { get; set; }

		public TestCollectionResult Run()
		{
			var type = Instance.GetType();
			var name = type.GetAttribute<TestCollectionAttribute>().Name ?? type.Name;
			return new TestCollectionResult { Name = name, Results = Tests.Select(test => RunTest(test)).ToList() };
		}

		private TestResultInfo RunTest(MethodInfo test)
		{
			var attr = test.GetAttribute<TestAttribute>();
			var testInfo = new TestResultInfo { Name = attr.Name, Description = attr.Description };

			if (test.ContainsAttribute<IgnoreTestAttribute>())
			{
				testInfo.Result = TestResult.Ignored;
				return testInfo;
			}

			try
			{
				test.Invoke(Instance, null);
				testInfo.Result = TestResult.Success;
				return testInfo;
			}
			catch (Exception ex)
			{
				// The main exception will always be a TargetInvocationException because we invoke
				// via reflection
				var inner = ex.InnerException;
				var trace = new StackTrace(inner, true);

				testInfo.Exception = inner;
				testInfo.Stack = trace;
				testInfo.Result = TestResult.Failure;
				return testInfo;
			}
		}
	}
}