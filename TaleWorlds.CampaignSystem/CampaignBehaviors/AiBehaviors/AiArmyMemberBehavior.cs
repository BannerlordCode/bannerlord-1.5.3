using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x0200048F RID: 1167
	public class AiArmyMemberBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x06004B1D RID: 19229 RVA: 0x0017B0D8 File Offset: 0x001792D8
		private float FollowingArmyLeaderMaxScore
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x0017B0DF File Offset: 0x001792DF
		private float FollowingArmyLeaderMinScore
		{
			get
			{
				return this.FollowingArmyLeaderMaxScore * 0.5f;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x0017B0ED File Offset: 0x001792ED
		private float ArmyLeaderIsUnreachableScore
		{
			get
			{
				return 0.02475f;
			}
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x0017B0F4 File Offset: 0x001792F4
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x0017B124 File Offset: 0x00179324
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x0017B128 File Offset: 0x00179328
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			for (int i = 0; i < siegeEvent.BesiegedSettlement.Parties.Count; i++)
			{
				if (siegeEvent.BesiegedSettlement.Parties[i].IsLordParty)
				{
					siegeEvent.BesiegedSettlement.Parties[i].SetMoveModeHold();
				}
			}
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x0017B180 File Offset: 0x00179380
		public void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.Army == null || mobileParty.Army.LeaderParty == mobileParty)
			{
				return;
			}
			if (mobileParty.AttachedTo == null)
			{
				if (mobileParty.Army.LeaderParty.CurrentSettlement != null && mobileParty.Army.LeaderParty.CurrentSettlement.IsUnderSiege && (mobileParty.Army.LeaderParty.CurrentSettlement.SiegeEvent.IsBlockadeActive || !mobileParty.HasNavalNavigationCapability))
				{
					return;
				}
				if (mobileParty.CurrentSettlement != null && mobileParty.CurrentSettlement.IsUnderSiege)
				{
					return;
				}
			}
			MobileParty.NavigationType navigationType = MobileParty.NavigationType.None;
			float num = float.MaxValue;
			bool flag = false;
			bool flag2 = false;
			if (mobileParty.Army.LeaderParty.CurrentSettlement != null)
			{
				SiegeEvent siegeEvent = mobileParty.Army.LeaderParty.CurrentSettlement.SiegeEvent;
				bool flag3 = siegeEvent == null;
				bool flag4 = mobileParty.HasNavalNavigationCapability && mobileParty.Army.LeaderParty.CurrentSettlement.HasPort && (siegeEvent == null || (!siegeEvent.IsBlockadeActive && mobileParty.HasNavalNavigationCapability));
				if (flag3)
				{
					AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, mobileParty.Army.LeaderParty.CurrentSettlement, false, out navigationType, out num, out flag2);
				}
				if (flag4)
				{
					MobileParty.NavigationType navigationType2;
					float num2;
					bool flag5;
					AiHelper.GetBestNavigationTypeAndAdjustedDistanceOfSettlementForMobileParty(mobileParty, mobileParty.Army.LeaderParty.CurrentSettlement, true, out navigationType2, out num2, out flag5);
					if (num2 < num)
					{
						navigationType = navigationType2;
						num = num2;
						flag2 = flag5;
						flag = true;
					}
				}
			}
			else
			{
				AiHelper.GetBestNavigationTypeAndDistanceOfMobilePartyForMobileParty(mobileParty, mobileParty.Army.LeaderParty, out navigationType, out num);
			}
			ValueTuple<AIBehaviorData, float> valueTuple;
			if (navigationType != MobileParty.NavigationType.None)
			{
				float num3 = this.FollowingArmyLeaderMaxScore;
				float num4 = 1f;
				float num5 = (mobileParty.Army.LeaderParty.IsMainParty ? Campaign.Current.Models.ArmyManagementCalculationModel.PlayerMobilePartySizeRatioToCallToArmy : Campaign.Current.Models.ArmyManagementCalculationModel.AIMobilePartySizeRatioToCallToArmy);
				if ((float)mobileParty.GetNumDaysForFoodToLast() < Campaign.Current.Models.ArmyManagementCalculationModel.MinimumNeededFoodInDaysToCallToArmy || mobileParty.PartySizeRatio < num5)
				{
					num3 = this.FollowingArmyLeaderMinScore;
					float num6 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(navigationType) * 0.5f;
					if (num6 > num)
					{
						num4 = MathF.Clamp(num6 / (num + 0.1f), 1f, this.FollowingArmyLeaderMaxScore / this.FollowingArmyLeaderMinScore);
					}
				}
				AIBehaviorData aibehaviorData = new AIBehaviorData(mobileParty.Army.LeaderParty, AiBehavior.EscortParty, navigationType, false, flag2, flag);
				float num7 = MathF.Clamp(num3 * num4, this.FollowingArmyLeaderMinScore, this.FollowingArmyLeaderMaxScore);
				valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, num7);
				p.AddBehaviorScore(in valueTuple);
				return;
			}
			AIBehaviorData aibehaviorData2 = new AIBehaviorData(mobileParty.Army.LeaderParty, AiBehavior.EscortParty, mobileParty.NavigationCapability, false, flag2, false);
			float armyLeaderIsUnreachableScore = this.ArmyLeaderIsUnreachableScore;
			valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData2, armyLeaderIsUnreachableScore);
			p.AddBehaviorScore(in valueTuple);
		}
	}
}
