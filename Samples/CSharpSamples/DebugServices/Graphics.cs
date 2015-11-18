using System;
using System.Diagnostics.CodeAnalysis;
using CryCil;
using CryCil.Engine.DebugServices;
using CryCil.Graphics;

namespace CSharpSamples.DebugServices
{
	[SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Reviewed. This is a sample.")]
	public static class Graphics
	{
		public static void DrawSampleGraphics()
		{
			// Lets say, we have an explosion and a wind.

			var center = new Vector3(100, 120, 40); // Center of the explosion.
			DebugGraphics.BeginDrawing("Explosion with wind", true);

			// We gonna represent explosion and its damage radius with a sphere.
			DebugGraphics.DrawSphere(center, 4, Colors.DarkRed, 10);

			// And the direction of the wind that will blow that particle effects away is an arrow:
			DebugGraphics.DrawDirection(center, 0.1f, Vector3.Forward, Colors.LightBlue, 10);
		}
	}
}