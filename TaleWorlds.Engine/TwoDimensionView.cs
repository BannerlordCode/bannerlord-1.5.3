using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000098 RID: 152
	[EngineClass("rglTwo_dimension_view")]
	public sealed class TwoDimensionView : View
	{
		// Token: 0x06000D4C RID: 3404 RVA: 0x0000F110 File Offset: 0x0000D310
		internal TwoDimensionView(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0000F119 File Offset: 0x0000D319
		public static TwoDimensionView CreateTwoDimension(string viewName)
		{
			return EngineApplicationInterface.ITwoDimensionView.CreateTwoDimensionView(viewName);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0000F126 File Offset: 0x0000D326
		public void BeginFrame()
		{
			EngineApplicationInterface.ITwoDimensionView.BeginFrame(base.Pointer);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0000F138 File Offset: 0x0000D338
		public void EndFrame()
		{
			EngineApplicationInterface.ITwoDimensionView.EndFrame(base.Pointer);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0000F14A File Offset: 0x0000D34A
		public void Clear()
		{
			EngineApplicationInterface.ITwoDimensionView.Clear(base.Pointer);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0000F15C File Offset: 0x0000D35C
		public void CreateMeshFromDescription(WeakMaterial material, TwoDimensionMeshDrawData meshDrawData)
		{
			EngineApplicationInterface.ITwoDimensionView.AddNewMesh(base.Pointer, material.Pointer, ref meshDrawData);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0000F177 File Offset: 0x0000D377
		public bool CreateTextMeshFromCache(Material material, TwoDimensionTextMeshDrawData meshDrawData)
		{
			return EngineApplicationInterface.ITwoDimensionView.AddCachedTextMesh(base.Pointer, material.Pointer, ref meshDrawData);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0000F194 File Offset: 0x0000D394
		public void CreateTextMeshFromDescription(float[] vertices, float[] uvs, uint[] indices, int indexCount, Material material, TwoDimensionTextMeshDrawData meshDrawData)
		{
			EngineApplicationInterface.ITwoDimensionView.AddNewTextMesh(base.Pointer, vertices, uvs, indices, vertices.Length / 2, indexCount, material.Pointer, ref meshDrawData);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0000F1C4 File Offset: 0x0000D3C4
		public WeakMaterial GetOrCreateMaterial(Texture mainTexture, Texture overlayTexture)
		{
			return new WeakMaterial(EngineApplicationInterface.ITwoDimensionView.GetOrCreateMaterial(base.Pointer, (mainTexture != null) ? mainTexture.Pointer : UIntPtr.Zero, (overlayTexture != null) ? overlayTexture.Pointer : UIntPtr.Zero));
		}
	}
}
