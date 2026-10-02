using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000094 RID: 148
	[EngineClass("rglTexture_view")]
	public sealed class TextureView : View
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x0000EE73 File Offset: 0x0000D073
		internal TextureView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0000EE7C File Offset: 0x0000D07C
		public static TextureView CreateTextureView()
		{
			return EngineApplicationInterface.ITextureView.CreateTextureView();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0000EE88 File Offset: 0x0000D088
		public void SetTexture(Texture texture)
		{
			EngineApplicationInterface.ITextureView.SetTexture(base.Pointer, texture.Pointer);
		}
	}
}
