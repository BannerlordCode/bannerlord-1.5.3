using System;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x0200002D RID: 45
	public class ResourceTextureProvider : TextureProvider
	{
		// Token: 0x0600034F RID: 847 RVA: 0x0000EF26 File Offset: 0x0000D126
		protected override Texture OnGetTextureForRender(TwoDimensionContext twoDimensionContext, string name)
		{
			return twoDimensionContext.LoadTexture(name);
		}
	}
}
