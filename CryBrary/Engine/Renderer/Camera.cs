using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryEngine.Mathematics;
using CryEngine.Native;

namespace CryEngine
{
	public sealed class Camera
	{
		public static Camera Current
		{
			get
			{
				return TryGet(RendererInterop.GetViewCamera());
			}
		}

		internal static Camera TryGet(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
				return null;

			var camera = cameras.FirstOrDefault(x => x.Handle == handle);
			if (camera != null)
				return camera;

			camera = new Camera();
			camera.Handle = handle;

			cameras.Add(camera);

			return camera;
		}

		private static readonly List<Camera> cameras = new List<Camera>();

		public Matrix34 Matrix { get { return RendererInterop.GetCameraMatrix(Handle); } set { RendererInterop.SetCameraMatrix(Handle, value); } }
		public Vector3 Position { get { return RendererInterop.GetCameraPosition(Handle); } set { RendererInterop.SetCameraPosition(Handle, value); } }

		public float FieldOfView { get { return RendererInterop.GetCameraFieldOfView(Handle); } }

		private IntPtr Handle { get; set; }
	}
}