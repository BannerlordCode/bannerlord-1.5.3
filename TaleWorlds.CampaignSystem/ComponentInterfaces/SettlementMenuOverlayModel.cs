using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D0 RID: 464
	public abstract class SettlementMenuOverlayModel : MBGameModel<SettlementMenuOverlayModel>
	{
		// Token: 0x06001EB0 RID: 7856
		public abstract Dictionary<Hero, bool> GetOverlayHeroes();
	}
}
