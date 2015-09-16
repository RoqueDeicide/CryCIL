using System;

namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Represents objects that can be used to provide foreign data to certain CryEngine subsystems.
	/// </summary>
	public interface IForeignDataProvider
	{
		/// <summary>
		/// Gets or sets the pointer to the piece of data that can be given to CryEngine object as a
		/// foreign data.
		/// </summary>
		/// <remarks>Setter in this property is used for reverse conversion.</remarks>
		ForeignData ForeignData { get; }
		/// <summary>
		/// Assigns foreign data to this object.
		/// </summary>
		/// <remarks>
		/// This method doesn't need to be implemented when <see cref="ForeignDataId"/> returns
		/// <see cref="ForeignDataIds.MultiId"/>.
		/// </remarks>
		/// <param name="handle">A foreign data handle to be assigned to this object.</param>
		void SetForeignData(IntPtr handle);
		/// <summary>
		/// Gets the identifier of this type of foreign data.
		/// </summary>
		/// <remarks>It is not recommended to make this</remarks>
		ForeignDataIds ForeignDataId { get; }
	}
}