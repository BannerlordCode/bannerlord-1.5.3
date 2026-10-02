using System;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x0200047B RID: 1147
	public class CommentOnChangeVillageStateBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AC5 RID: 19141 RVA: 0x001799CF File Offset: 0x00177BCF
		public override void RegisterEvents()
		{
			CampaignEvents.VillageStateChanged.AddNonSerializedListener(this, new Action<Village, Village.VillageStates, Village.VillageStates, MobileParty>(this.OnVillageStateChanged));
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x001799E8 File Offset: 0x00177BE8
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x001799EC File Offset: 0x00177BEC
		private void OnVillageStateChanged(Village village, Village.VillageStates oldState, Village.VillageStates newState, MobileParty raiderParty)
		{
			if (newState != Village.VillageStates.Normal && raiderParty != null && (raiderParty.LeaderHero == Hero.MainHero || village.Owner.Settlement.OwnerClan.Leader == Hero.MainHero || village.Settlement.MapFaction.IsKingdomFaction || raiderParty.MapFaction.IsKingdomFaction))
			{
				LogEntry.AddLogEntry(new VillageStateChangedLogEntry(village, oldState, newState, raiderParty));
			}
		}
	}
}
