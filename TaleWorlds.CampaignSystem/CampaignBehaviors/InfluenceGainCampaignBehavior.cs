using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000425 RID: 1061
	public class InfluenceGainCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600436E RID: 17262 RVA: 0x0013D8AD File Offset: 0x0013BAAD
		public override void RegisterEvents()
		{
			CampaignEvents.OnPrisonerDonatedToSettlementEvent.AddNonSerializedListener(this, new Action<MobileParty, FlattenedTroopRoster, Settlement>(this.OnPrisonerDonatedToSettlement));
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0013D8C6 File Offset: 0x0013BAC6
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0013D8C8 File Offset: 0x0013BAC8
		private void OnPrisonerDonatedToSettlement(MobileParty donatingParty, FlattenedTroopRoster donatedPrisoners, Settlement donatedSettlement)
		{
			if (donatedSettlement.OwnerClan != Clan.PlayerClan || donatingParty.ActualClan != Clan.PlayerClan)
			{
				float num = 0f;
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in donatedPrisoners)
				{
					num += Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(donatingParty.Party, flattenedTroopRosterElement.Troop, donatedSettlement);
				}
				GainKingdomInfluenceAction.ApplyForDonatePrisoners(donatingParty, num);
			}
		}
	}
}
