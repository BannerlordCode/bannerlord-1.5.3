using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000091 RID: 145
	[EngineClass("rglTableau_view")]
	public sealed class TableauView : SceneView
	{
		// Token: 0x06000D03 RID: 3331 RVA: 0x0000E9A1 File Offset: 0x0000CBA1
		internal TableauView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0000E9AA File Offset: 0x0000CBAA
		public static TableauView CreateTableauView(string viewName)
		{
			return EngineApplicationInterface.ITableauView.CreateTableauView(viewName);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0000E9B7 File Offset: 0x0000CBB7
		public void SetSortingEnabled(bool value)
		{
			EngineApplicationInterface.ITableauView.SetSortingEnabled(base.Pointer, value);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0000E9CA File Offset: 0x0000CBCA
		public void SetContinuousRendering(bool value)
		{
			EngineApplicationInterface.ITableauView.SetContinousRendering(base.Pointer, value);
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0000E9DD File Offset: 0x0000CBDD
		public void SetDoNotRenderThisFrame(bool value)
		{
			EngineApplicationInterface.ITableauView.SetDoNotRenderThisFrame(base.Pointer, value);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		public void SetDeleteAfterRendering(bool value)
		{
			EngineApplicationInterface.ITableauView.SetDeleteAfterRendering(base.Pointer, value);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0000EA03 File Offset: 0x0000CC03
		public static Texture AddTableau(string name, RenderTargetComponent.TextureUpdateEventHandler eventHandler, object objectRef, int tableauSizeX, int tableauSizeY)
		{
			Texture texture = Texture.CreateTableauTexture(name, eventHandler, objectRef, tableauSizeX, tableauSizeY);
			texture.TableauView.SetRenderOnDemand(false);
			return texture;
		}
	}
}
