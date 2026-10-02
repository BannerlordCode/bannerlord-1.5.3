using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000033 RID: 51
	[ApplicationInterfaceBase]
	internal interface ITextureView
	{
		// Token: 0x0600054D RID: 1357
		[EngineMethod("create_texture_view", false, null, false)]
		TextureView CreateTextureView();

		// Token: 0x0600054E RID: 1358
		[EngineMethod("set_texture", false, null, true)]
		void SetTexture(UIntPtr pointer, UIntPtr texture_ptr);
	}
}
