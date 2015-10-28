using CryCil.Engine.Logic;
using CryCil.Engine.Models.StaticObjects;
using CryCil.Graphics;
using CryCil.Utilities;

namespace CSharpSamples
{
	public static class MeshRecoloring
	{
		public static unsafe void Recolor(CryEntity entity)
		{
			// Get the render mesh.
			StaticObject staticObject = entity.Slots[0].BoundStaticObject;

			if (!staticObject.IsValid)
			{
				return;
			}

			CryRenderMesh renderMesh = staticObject.RenderMesh;

			if (!renderMesh.IsValid)
			{
				return;
			}

			// Lock the mesh to prevent multiple threads from accessing at the same time.
			renderMesh.LockForThreadAccess();

			// Get the pointer to the colors.
			StridedPointer colorsPointer = renderMesh.GetColorsPointer(RenderMeshAccessFlags.SystemUpdate);

			// Brighten up everything.
			int vertexCount = renderMesh.VertexCount;
			for (int i = 0; i < vertexCount; i++)
			{
				// Acquire the pointer to the current color.
				ColorByte* color = (ColorByte*)colorsPointer.GetElement(i);
				unchecked
				{
					byte value = color->Red;
					color->Red = value == byte.MaxValue ? byte.MaxValue : (byte)(value + 1);
					value = color->Blue;
					color->Blue = value == byte.MaxValue ? byte.MaxValue : (byte)(value + 1);
					value = color->Green;
					color->Green = value == byte.MaxValue ? byte.MaxValue : (byte)(value + 1);
				}
			}

			// Unlock the general vertex stream.
			renderMesh.UnlockStream(RenderMeshStreamIds.General);

			// Unlock the rest of the mesh.
			renderMesh.UnLockForThreadAccess();
		}
	}
}