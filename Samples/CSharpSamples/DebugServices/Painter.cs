using System;
using System.Linq;
using CryCil;
using CryCil.Engine.DebugServices;
using CryCil.Graphics;

namespace CSharpSamples.DebugServices
{
	public static class Painter
	{
		public static void DrawSampleGraphics()
		{
			// Lets say, we have an explosion and a wind.

			var center = new Vector3(100, 120, 40); // Center of the explosion.
			var painter = new DebubPainter("Explosion with wind")
			{
				Timeout = 10,
				Color = Colors.DarkRed
			};

			// We gonna represent explosion and its damage radius with a sphere.
			painter.Sphere(center, 4);

			// And the direction of the wind that will blow that particle effects away is an arrow:
			painter.Color = Colors.LightBlue;
			painter.Arrow(center, 0.1f, Vector3.Forward);
		}
	}
}