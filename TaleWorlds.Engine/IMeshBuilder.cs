using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000024 RID: 36
	[ApplicationInterfaceBase]
	internal interface IMeshBuilder
	{
		// Token: 0x06000204 RID: 516
		[EngineMethod("create_tiling_window_mesh", false, null, false)]
		Mesh CreateTilingWindowMesh(string baseMeshName, ref Vec2 meshSizeMin, ref Vec2 meshSizeMax, ref Vec2 borderThickness, ref Vec2 backgroundBorderThickness);

		// Token: 0x06000205 RID: 517
		[EngineMethod("create_tiling_button_mesh", false, null, false)]
		Mesh CreateTilingButtonMesh(string baseMeshName, ref Vec2 meshSizeMin, ref Vec2 meshSizeMax, ref Vec2 borderThickness);

		// Token: 0x06000206 RID: 518
		[EngineMethod("finalize_mesh_builder", false, null, false)]
		Mesh FinalizeMeshBuilder(int num_vertices, Vec3[] vertices, int num_face_corners, MeshBuilder.FaceCorner[] faceCorners, int num_faces, MeshBuilder.Face[] faces);
	}
}
