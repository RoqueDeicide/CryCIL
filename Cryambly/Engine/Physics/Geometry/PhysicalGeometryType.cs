namespace CryCil.Engine.Physics
{
	/// <summary>
	/// Enumeration of geometry physicalization types. Used mostly be static objects.
	/// </summary>
	public enum PhysicalGeometryType
	{
		/// <summary>
		/// Specifies that geometry is not physicalized.
		/// </summary>
		None = -1,
		/// <summary>
		/// Specifies that geometry is physicalized normally.
		/// </summary>
		Default = 0x1000 + 0,
		/// <summary>
		/// Specifies that geometry is physicalized, but it won't participate in collisions.
		/// </summary>
		NoCollide = 0x1000 + 1,
		/// <summary>
		/// Specifies that geometry is physicalized, but it's only presence in physical world is expressed with collisions.
		/// </summary>
		Obstruct = 0x1000 + 2,
		/// <summary>
		/// Specifies that only NoDraw geometry is physicalized normally, the rest isn't.
		/// </summary>
		DefaultProxy = 0x1000 + 0x100
	}
}