using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000404 RID: 1028
	public class DesertionCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004091 RID: 16529 RVA: 0x0011B3FE File Offset: 0x001195FE
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.DailyTickParty));
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x0011B417 File Offset: 0x00119617
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x0011B41C File Offset: 0x0011961C
		private void DailyTickParty(MobileParty mobileParty)
		{
			if (mobileParty.IsActive && !mobileParty.IsCurrentlyAtSea && !mobileParty.IsDisbanding && mobileParty.Party.MapEvent == null && (mobileParty.IsLordParty || mobileParty.IsCaravan || mobileParty.IsGarrison) && mobileParty.MemberRoster.TotalRegulars > 0)
			{
				DesertionCampaignBehavior.CheckDesertionForParty(mobileParty);
			}
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x0011B47C File Offset: 0x0011967C
		private static void CheckDesertionForParty(MobileParty mobileParty)
		{
			TroopRoster troopsToDesert = Campaign.Current.Models.PartyDesertionModel.GetTroopsToDesert(mobileParty);
			if (troopsToDesert.Count > 0)
			{
				int numberOfAllMembers = mobileParty.Party.NumberOfAllMembers;
				foreach (TroopRosterElement troopRosterElement in troopsToDesert.GetTroopRoster())
				{
					mobileParty.MemberRoster.AddToCounts(troopRosterElement.Character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, 0, true, -1);
				}
				int numberOfAllMembers2 = mobileParty.Party.NumberOfAllMembers;
				if ((float)troopsToDesert.TotalManCount > (float)numberOfAllMembers * 0.4f && numberOfAllMembers > 10)
				{
					Debug.Print(string.Format("[High Desertion Alert]  Deserted troop count for party: {0} is: {1}, remaining troop count: {2}, all member count before desertion: {3}", new object[] { mobileParty.StringId, troopsToDesert.TotalManCount, numberOfAllMembers2, numberOfAllMembers }), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				CampaignEventDispatcher.Instance.OnTroopsDeserted(mobileParty, troopsToDesert);
				if (numberOfAllMembers2 == 0)
				{
					DestroyPartyAction.Apply(null, mobileParty);
				}
			}
		}
	}
}
