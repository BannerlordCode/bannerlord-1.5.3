using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000481 RID: 1153
	public class CommentOnDestroyMobilePartyBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004ADE RID: 19166 RVA: 0x00179CEA File Offset: 0x00177EEA
		public override void RegisterEvents()
		{
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x00179D03 File Offset: 0x00177F03
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AE0 RID: 19168 RVA: 0x00179D08 File Offset: 0x00177F08
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			Hero hero = ((destroyerParty != null) ? destroyerParty.LeaderHero : null);
			IFaction faction = ((destroyerParty != null) ? destroyerParty.MapFaction : null);
			if (hero == Hero.MainHero || mobileParty.LeaderHero == Hero.MainHero || (faction != null && mobileParty.MapFaction != null && faction.IsKingdomFaction && mobileParty.MapFaction.IsKingdomFaction))
			{
				LogEntry.AddLogEntry(new DestroyMobilePartyLogEntry(mobileParty, destroyerParty));
			}
		}
	}
}
