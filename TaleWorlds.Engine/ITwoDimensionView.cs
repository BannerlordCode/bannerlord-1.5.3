using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000037 RID: 55
	[ApplicationInterfaceBase]
	internal interface ITwoDimensionView
	{
		// Token: 0x0600057E RID: 1406
		[EngineMethod("create_twodimension_view", false, null, false)]
		TwoDimensionView CreateTwoDimensionView(string viewName);

		// Token: 0x0600057F RID: 1407
		[EngineMethod("begin_frame", false, null, false)]
		void BeginFrame(UIntPtr pointer);

		// Token: 0x06000580 RID: 1408
		[EngineMethod("end_frame", false, null, false)]
		void EndFrame(UIntPtr pointer);

		// Token: 0x06000581 RID: 1409
		[EngineMethod("clear", false, null, false)]
		void Clear(UIntPtr pointer);

		// Token: 0x06000582 RID: 1410
		[EngineMethod("add_new_mesh", false, null, false)]
		void AddNewMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x06000583 RID: 1411
		[EngineMethod("add_new_quad_mesh", false, null, false)]
		void AddNewQuadMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x06000584 RID: 1412
		[EngineMethod("add_cached_text_mesh", false, null, false)]
		bool AddCachedTextMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x06000585 RID: 1413
		[EngineMethod("add_new_text_mesh", false, null, false)]
		void AddNewTextMesh(UIntPtr pointer, float[] vertices, float[] uvs, uint[] indices, int vertexCount, int indexCount, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x06000586 RID: 1414
		[EngineMethod("get_or_create_material", false, null, false)]
		UIntPtr GetOrCreateMaterial(UIntPtr pointer, UIntPtr mainTexture, UIntPtr overlayTexture);
	}
}
