using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000391 RID: 913
	public static class ScreenFadeController
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x000DA13C File Offset: 0x000D833C
		public static bool IsFadeActive
		{
			get
			{
				return ScreenFadeController._handler != null && ScreenFadeController._handler.GetScreenFadeState() > ScreenFadeController.ScreenFadeState.None;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060034AB RID: 13483 RVA: 0x000DA154 File Offset: 0x000D8354
		public static bool IsFadingOut
		{
			get
			{
				return ScreenFadeController._handler != null && ScreenFadeController._handler.GetScreenFadeState() == ScreenFadeController.ScreenFadeState.FadingOut;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060034AC RID: 13484 RVA: 0x000DA16C File Offset: 0x000D836C
		public static bool IsFadingIn
		{
			get
			{
				return ScreenFadeController._handler != null && ScreenFadeController._handler.GetScreenFadeState() == ScreenFadeController.ScreenFadeState.FadingIn;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x060034AD RID: 13485 RVA: 0x000DA184 File Offset: 0x000D8384
		public static bool IsFadedOut
		{
			get
			{
				return ScreenFadeController._handler != null && ScreenFadeController._handler.GetScreenFadeState() == ScreenFadeController.ScreenFadeState.FadedOut;
			}
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000DA19C File Offset: 0x000D839C
		public static void RegisterHandler(IScreenFadeHandler handler)
		{
			if (ScreenFadeController._handler == null)
			{
				ScreenFadeController._handler = handler;
				return;
			}
			Debug.FailedAssert("ScreenFade handler already registered!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\ScreenFadeController.cs", "RegisterHandler", 30);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000DA1C2 File Offset: 0x000D83C2
		public static void BeginFadeOutAndIn(float fadeOutDuration = 0.5f, float blackOutDuration = 0.5f, float fadeInDuration = 0.5f)
		{
			IScreenFadeHandler handler = ScreenFadeController._handler;
			if (handler == null)
			{
				return;
			}
			handler.BeginFadeOutAndIn(fadeOutDuration, blackOutDuration, fadeInDuration);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000DA1D6 File Offset: 0x000D83D6
		public static void BeginFadeOut(float fadeOutDuration = 0.5f)
		{
			IScreenFadeHandler handler = ScreenFadeController._handler;
			if (handler == null)
			{
				return;
			}
			handler.BeginFadeOut(fadeOutDuration);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000DA1E8 File Offset: 0x000D83E8
		public static void BeginFadeIn(float fadeInDuration = 0.5f)
		{
			IScreenFadeHandler handler = ScreenFadeController._handler;
			if (handler == null)
			{
				return;
			}
			handler.BeginFadeIn(fadeInDuration);
		}

		// Token: 0x0400165C RID: 5724
		private static IScreenFadeHandler _handler;

		// Token: 0x0200066A RID: 1642
		public enum ScreenFadeState
		{
			// Token: 0x04002232 RID: 8754
			None,
			// Token: 0x04002233 RID: 8755
			FadingOut,
			// Token: 0x04002234 RID: 8756
			FadedOut,
			// Token: 0x04002235 RID: 8757
			FadingIn
		}
	}
}
