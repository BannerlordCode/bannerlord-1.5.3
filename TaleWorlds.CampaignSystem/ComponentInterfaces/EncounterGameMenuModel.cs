using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C4 RID: 452
	public abstract class EncounterGameMenuModel : MBGameModel<EncounterGameMenuModel>
	{
		// Token: 0x06001E6B RID: 7787
		public abstract string GetEncounterMenu(PartyBase attackerParty, PartyBase defenderParty, out bool startBattle, out bool joinBattle);

		// Token: 0x06001E6C RID: 7788
		public abstract string GetRaidCompleteMenu();

		// Token: 0x06001E6D RID: 7789
		public abstract string GetNewPartyJoinMenu(MobileParty newParty);

		// Token: 0x06001E6E RID: 7790
		public abstract string GetGenericStateMenu();

		// Token: 0x06001E6F RID: 7791
		public abstract bool IsPlunderMenu(string menuId);
	}
}
