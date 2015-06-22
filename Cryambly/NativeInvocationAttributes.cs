using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryCil.Annotations;
using CryCil.RunTime;

namespace CryCil
{
	/// <summary>
	/// Marks a method that is invoked from native code using unmanaged thunk.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[MeansImplicitUse]
	public sealed class UnmanagedThunkAttribute : Attribute
	{
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		public UnmanagedThunkAttribute()
		{
			this.Comment = "";
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="comment">A comment about the context of the method's invocation.</param>
		public UnmanagedThunkAttribute([NotNull] string comment)
		{
			Comment = comment;
		}
		/// <summary>
		/// A comment about the context of the method's invocation.
		/// </summary>
		[NotNull]
		public string Comment { get; private set; }
	}
	/// <summary>
	/// Marks a method that is invoked from native code using a raw thunk.
	/// </summary>
	/// <remarks>
	/// Methods that are invoked through raw thunks have to have try/catch block that detects all errors
	/// and handles them in some way (by invoking <see cref="MonoInterface.DisplayException"/> for
	/// instance) , otherwise any unhandled exception will crash the program.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[MeansImplicitUse]
	public sealed class RawThunkAttribute : Attribute
	{
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		public RawThunkAttribute()
		{
			this.Comment = "";
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="comment">A comment about the context of the method's invocation.</param>
		public RawThunkAttribute([NotNull] string comment)
		{
			Comment = comment;
		}
		/// <summary>
		/// A comment about the context of the method's invocation.
		/// </summary>
		[NotNull]
		public string Comment { get; private set; }
	}
	/// <summary>
	/// Marks a method that is invoked from native code using mono_runtime_invoke that can be called via
	/// IMonoFunction's derivatives.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	[MeansImplicitUse]
	public sealed class RuntimeInvokeAttribute : Attribute
	{
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		public RuntimeInvokeAttribute()
		{
			this.Comment = "";
		}
		/// <summary>
		/// Creates new instance of this type.
		/// </summary>
		/// <param name="comment">A comment about the context of the method's invocation.</param>
		public RuntimeInvokeAttribute([NotNull] string comment)
		{
			Comment = comment;
		}
		/// <summary>
		/// A comment about the context of the method's invocation.
		/// </summary>
		[NotNull]
		public string Comment { get; private set; }
	}
}