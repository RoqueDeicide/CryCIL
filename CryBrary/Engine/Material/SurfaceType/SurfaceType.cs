using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CryEngine.Native;

namespace CryEngine
{
	public class SurfaceType
	{
		#region Statics
		internal static SurfaceType TryGet(IntPtr surfaceTypePtr)
		{
			if (surfaceTypePtr == IntPtr.Zero)
				return null;

			foreach (var storedSurfaceType in surfaceTypes)
			{
				if (storedSurfaceType.Handle == surfaceTypePtr)
					return storedSurfaceType;
			}

			var surfaceType = new SurfaceType(surfaceTypePtr);
			surfaceTypes.Add(surfaceType);

			return surfaceType;
		}

		public static SurfaceType Get(int id)
		{
			return TryGet(MaterialInterop.GetSurfaceTypeById(id));
		}

		public static SurfaceType Get(string name)
		{
			return TryGet(MaterialInterop.GetSurfaceTypeByName(name));
		}

		private static readonly List<SurfaceType> surfaceTypes = new List<SurfaceType>();
		#endregion

		public SurfaceType(IntPtr materialPtr)
		{
			Handle = materialPtr;
		}

		/// <summary>
		/// Gets the unique identifier of this surface type. Maximum of 65535 simultaneous surface
		/// types can exist.
		/// </summary>
		public Int32 Id { get { return System.Convert.ToInt32(MaterialInterop.GetSurfaceTypeId(Handle)); } }

		/// <summary>
		/// Gets the unique name of this surface type.
		/// </summary>
		public string Name { get { return MaterialInterop.GetSurfaceTypeName(Handle); } }

		/// <summary>
		/// Gets the type name of this surface type.
		/// </summary>
		public string TypeName { get { return MaterialInterop.GetSurfaceTypeTypeName(Handle); } }

		/// <summary>
		/// Gets flags assigned to this surface type.
		/// </summary>
		public SurfaceTypeFlags Flags { get { return MaterialInterop.GetSurfaceTypeFlags(Handle); } }

		/// <summary>
		/// Gets the parameters assigned to this surface type.
		/// </summary>
		public SurfaceTypeParams Parameters { get { return MaterialInterop.GetSurfaceTypeParams(Handle); } }

		/// <summary>
		/// Gets or sets the native ISurfaceType pointer.
		/// </summary>
		internal IntPtr Handle { get; set; }
	}
}