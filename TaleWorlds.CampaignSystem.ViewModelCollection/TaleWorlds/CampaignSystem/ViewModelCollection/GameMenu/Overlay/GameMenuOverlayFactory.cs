using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameMenus;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BF RID: 191
	public static class GameMenuOverlayFactory
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x0004ADB5 File Offset: 0x00048FB5
		public static void RegisterProvider(IGameMenuOverlayProvider provider)
		{
			GameMenuOverlayFactory._providers.Add(provider);
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0004ADC2 File Offset: 0x00048FC2
		public static void UnregisterProvider(IGameMenuOverlayProvider provider)
		{
			GameMenuOverlayFactory._providers.Remove(provider);
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0004ADD0 File Offset: 0x00048FD0
		public static GameMenuOverlay GetOverlay(GameMenu.MenuOverlayType menuOverlayType)
		{
			for (int i = GameMenuOverlayFactory._providers.Count - 1; i >= 0; i--)
			{
				GameMenuOverlay overlay = GameMenuOverlayFactory._providers[i].GetOverlay(menuOverlayType);
				if (overlay != null)
				{
					return overlay;
				}
			}
			return null;
		}

		// Token: 0x04000861 RID: 2145
		private static List<IGameMenuOverlayProvider> _providers = new List<IGameMenuOverlayProvider>();
	}
}
