using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000392 RID: 914
	public interface IScreenFadeHandler
	{
		// Token: 0x060034B2 RID: 13490
		void BeginFadeOutAndIn(float fadeOutDuration = 0.5f, float blackOutDuration = 0.5f, float fadeInDuration = 0.5f);

		// Token: 0x060034B3 RID: 13491
		void BeginFadeOut(float fadeOutDuration = 0.5f);

		// Token: 0x060034B4 RID: 13492
		void BeginFadeIn(float fadeInDuration = 0.5f);

		// Token: 0x060034B5 RID: 13493
		ScreenFadeController.ScreenFadeState GetScreenFadeState();
	}
}
