using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using CryCil.Engine.Logic;
using CryCil.RunTime;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Marks functions that can extract objects from foreign data objects.
	/// </summary>
	/// <remarks>
	/// Functions that are decorated with this attribute must be static, they must accept only 2
	/// parameters: first of type <see cref="IntPtr"/> and second of type <see cref="ForeignDataIds"/> and
	/// it must return an object of type that can be extracted from foreign data.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public sealed class ForeignDataExtractorAttribute : Attribute
	{
		/// <summary>
		/// Gets the identifier of the foreign data type that must be passed to the function in order for
		/// the object to be extracted.
		/// </summary>
		public ForeignDataIds Id { get; private set; }
		/// <summary>
		/// Creates a new object of this type.
		/// </summary>
		/// <param name="id">
		/// An identifier of foreign data type. Cannot be equal to <see cref="ForeignDataIds.MultiId"/>.
		/// </param>
		public ForeignDataExtractorAttribute(ForeignDataIds id)
		{
			this.Id = id;
		}
	}
	/// <summary>
	/// Encapsulates a pointer that can be passed to CryEngine physics subsystem.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct ForeignData
	{
		#region Fields
		private IntPtr handle;
		private ForeignDataIds id;
		internal static readonly ForeignData Unused = new ForeignData
		{
			handle = UnusedValue.Pointer,
			id = (ForeignDataIds)UnusedValue.Int32
		};
		#endregion
		#region Properties
		/// <summary>
		/// Gets identifier of type of foreign data that is contained in this object.
		/// </summary>
		public ForeignDataIds Id
		{
			get { return this.id; }
		}
		/// <summary>
		/// Attempts to extract an object of type <see cref="CryEntity"/> from this object.
		/// </summary>
		/// <returns>
		/// A valid object of type <see cref="CryEntity"/> if <see cref="Id"/> is equal to
		/// <see cref="ForeignDataIds.Entity"/>, an invalid one otherwise.
		/// </returns>
		public CryEntity Entity
		{
			get { return new CryEntity(this.id == ForeignDataIds.Entity ? this.handle : new IntPtr()); }
		}
		#endregion
		#region Construction
		/// <summary>
		/// Creates a new instance of this type.
		/// </summary>
		/// <param name="foreignDataHandle">A pointer to foreign data.</param>
		/// <param name="foreignDataId">    Identifier of the type of foreign data.</param>
		/// <exception cref="ArgumentException">
		/// Cannot create object of type <see cref="ForeignData"/> using
		/// <see cref="ForeignDataIds.MultiId"/> as an identifier.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// Identifier of custom foreign data has to be greater or equal to
		/// <see cref="ForeignDataIds.User"/>.
		/// </exception>
		public ForeignData(IntPtr foreignDataHandle, ForeignDataIds foreignDataId)
		{
			if (foreignDataId == ForeignDataIds.MultiId)
			{
				throw new ArgumentException("Cannot create object of type ForeignData using MultiId as an identifier.");
			}
			if (foreignDataId > ForeignDataIds.RagDoll && foreignDataId < ForeignDataIds.User)
			{
				throw new ArgumentOutOfRangeException("foreignDataId", "Identifier of custom foreign data has to be " +
																	   "greater or equal to ForeignDataIds.User.");
			}
			this.handle = foreignDataHandle;
			this.id = foreignDataId;
		}
		internal ForeignData(CryEntity entity)
		{
			this.handle = entity.Handle;
			this.id = entity.ForeignDataId;
		}
		#endregion
		#region Interface
		/// <summary>
		/// Attempts to extract an object of user-defined foreign data type from this object.
		/// </summary>
		/// <typeparam name="UserForeignDataType">
		/// Type of object that would return this object if converted to foreign data.
		/// </typeparam>
		/// <returns>
		/// A valid object of type <typeparamref name="UserForeignDataType"/> if extracted successfully,
		/// otherwise a result of <c>default()</c> with <see cref="UserForeignDataType"/> as an argument is
		/// returned.
		/// </returns>
		public UserForeignDataType User<UserForeignDataType>()
			where UserForeignDataType : IForeignDataProvider, new()
		{
			UserForeignDataType obj = new UserForeignDataType();
			if (obj.ForeignDataId == this.id)
			{
				obj.SetForeignData(this.handle);
			}
			if (obj.ForeignDataId == ForeignDataIds.MultiId)
			{
				Delegate extractorDelegate;
				if (ForeignDataTypeRegistry.Extractors.TryGetValue(this.id, out extractorDelegate))
				{
					ForeignDataExtractor<UserForeignDataType> extractor =
						extractorDelegate as ForeignDataExtractor<UserForeignDataType>;
					Debug.Assert(extractor != null);
					obj = extractor(this.handle, this.id);
				}
				else
				{
					string foreignDataTypeIdName =
						Enum.GetName(typeof(ForeignDataIds), this.id) ?? string.Format("UserType[{0}]", this.id);
					string message =
						string.Format("Type {0} doesn't define a valid extractor method for foreign data type identifier {1}.",
									  typeof(UserForeignDataType).FullName, foreignDataTypeIdName);
					throw new NotSupportedException(message);
				}
			}
			return obj;
		}
		#endregion
	}
	internal delegate UserForeignDataType ForeignDataExtractor<out UserForeignDataType>(IntPtr handle, ForeignDataIds id)
	where UserForeignDataType : IForeignDataProvider;
	internal static class ForeignDataTypeRegistry
	{
		internal static SortedList<ForeignDataIds, Delegate> Extractors;
		static ForeignDataTypeRegistry()
		{
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
			Extractors = new SortedList<ForeignDataIds, Delegate>
			(
				(
					from assembly in MonoInterface.CryCilAssemblies
					from type in assembly.GetTypes()
					from method in type.GetMethods(bindingFlags)
					where method.ContainsAttribute<ForeignDataExtractorAttribute>()
					let parameters = method.GetParameters()
					where parameters.Length == 2
					where parameters[0].ParameterType == typeof(IntPtr)
					where parameters[1].ParameterType == typeof(ForeignDataIds)
					where method.ReturnType.Implements<IForeignDataProvider>()
					select method
				).ToDictionary
				(
					method => method.GetAttribute<ForeignDataExtractorAttribute>().Id,
					method => method.CreateDelegate(typeof(ForeignDataExtractor<>).MakeGenericType(method.ReturnType))
				)
			);
		}
	}
}