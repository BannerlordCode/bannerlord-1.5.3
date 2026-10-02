using System;
using TaleWorlds.CampaignSystem.GameMenus;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000C1 RID: 193
	public interface IGameMenuOverlayProvider
	{
		// Token: 0x060012C2 RID: 4802
		GameMenuOverlay GetOverlay(GameMenu.MenuOverlayType menuOverlayType);
	}
}
