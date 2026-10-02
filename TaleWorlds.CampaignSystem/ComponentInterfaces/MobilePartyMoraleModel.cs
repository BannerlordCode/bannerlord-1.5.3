using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DA RID: 474
	public abstract class MobilePartyMoraleModel : MBGameModel<MobilePartyMoraleModel>
	{
		// Token: 0x06001F09 RID: 7945
		public abstract float CalculateMoraleChange(MobileParty party);

		// Token: 0x06001F0A RID: 7946
		public abstract TextObject GetMoraleTooltipText(MobileParty party);
	}
}
