using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200013E RID: 318
	public class DefaultPartyShipLimitModel : PartyShipLimitModel
	{
		// Token: 0x060019D8 RID: 6616 RVA: 0x00080EA2 File Offset: 0x0007F0A2
		public override int GetIdealShipNumber(MobileParty mobileParty)
		{
			return 0;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00080EA5 File Offset: 0x0007F0A5
		public override int GetIdealShipNumber(Clan clan)
		{
			return 0;
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00080EA8 File Offset: 0x0007F0A8
		public override float GetShipPriority(MobileParty mobileParty, Ship ship, bool isSelling)
		{
			return 0f;
		}
	}
}
