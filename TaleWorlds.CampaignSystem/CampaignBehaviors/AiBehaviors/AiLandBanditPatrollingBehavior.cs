using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000491 RID: 1169
	public class AiLandBanditPatrollingBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B2A RID: 19242 RVA: 0x0017BD38 File Offset: 0x00179F38
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x0017BD51 File Offset: 0x00179F51
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x0017BD54 File Offset: 0x00179F54
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (!mobileParty.IsBandit)
			{
				return;
			}
			if (mobileParty.IsBanditBossParty)
			{
				return;
			}
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsHideout)
			{
				if (mobileParty.CurrentSettlement.Parties.CountQ<MobileParty>((MobileParty x) => x.IsBandit && !x.IsBanditBossParty) <= Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt + 1)
				{
					return;
				}
			}
			MobileParty.NavigationType navigationType = MobileParty.NavigationType.Default;
			if (!mobileParty.HasLandNavigationCapability)
			{
				return;
			}
			AIBehaviorData aibehaviorData = new AIBehaviorData(mobileParty.HomeSettlement, AiBehavior.PatrolAroundPoint, navigationType, false, false, false);
			float num = 1f;
			if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsHideout && (mobileParty.CurrentSettlement.MapFaction == mobileParty.MapFaction || mobileParty.CurrentSettlement.Hideout.IsInfested))
			{
				float num2 = (float)mobileParty.CurrentSettlement.Parties.CountQ<MobileParty>((MobileParty x) => x.IsBandit && !x.IsBanditBossParty);
				int numberOfMinimumBanditPartiesInAHideoutToInfestIt = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
				int numberOfMaximumBanditPartiesInEachHideout = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesInEachHideout;
				num = (num2 - (float)numberOfMinimumBanditPartiesInAHideoutToInfestIt) / (float)(numberOfMaximumBanditPartiesInEachHideout - numberOfMinimumBanditPartiesInAHideoutToInfestIt);
			}
			float num3 = ((mobileParty.CurrentSettlement != null) ? (MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat) : 0.5f);
			float num4 = 0.5f * num * num3;
			if (num > 0f)
			{
				ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, num4);
				p.AddBehaviorScore(in valueTuple);
			}
		}
	}
}
