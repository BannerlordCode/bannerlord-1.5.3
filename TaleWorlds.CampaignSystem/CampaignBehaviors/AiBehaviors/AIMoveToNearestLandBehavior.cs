using System;
using Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.AiBehaviors
{
	// Token: 0x02000493 RID: 1171
	internal class AIMoveToNearestLandBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B3F RID: 19263 RVA: 0x0017D2DB File Offset: 0x0017B4DB
		public override void RegisterEvents()
		{
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.AiHourlyTick));
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x0017D2F4 File Offset: 0x0017B4F4
		private void AiHourlyTick(MobileParty mobileParty, PartyThinkParams p)
		{
			if (mobileParty.IsCurrentlyAtSea && mobileParty.CurrentSettlement == null)
			{
				float estimatedSafeSailDuration = Campaign.Current.Models.CampaignShipDamageModel.GetEstimatedSafeSailDuration(mobileParty);
				Settlement settlement = null;
				if (mobileParty.HasLandNavigationCapability)
				{
					int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(MobileParty.NavigationType.All);
					CampaignVec2 nearestFaceCenterForPositionWithPath = Campaign.Current.MapSceneWrapper.GetNearestFaceCenterForPositionWithPath(mobileParty.CurrentNavigationFace, true, Campaign.MapDiagonal / 2f, invalidTerrainTypesForNavigationType);
					float num2;
					float num = DistanceHelper.FindClosestDistanceFromMobilePartyToPoint(mobileParty, nearestFaceCenterForPositionWithPath, MobileParty.NavigationType.All, out num2);
					if (num > 0f && num < Campaign.MapDiagonal)
					{
						float num3 = (mobileParty.IsLordParty ? Campaign.Current.EstimatedAverageLordPartyNavalSpeed : (mobileParty.IsCaravan ? Campaign.Current.EstimatedAverageCaravanPartyNavalSpeed : (mobileParty.IsBandit ? Campaign.Current.EstimatedAverageBanditPartyNavalSpeed : (mobileParty.IsVillager ? Campaign.Current.EstimatedAverageVillagerPartyNavalSpeed : (Campaign.Current.EstimatedMaximumLordPartySpeedExceptPlayer * 0.5f)))));
						float num4 = num / num3 / estimatedSafeSailDuration;
						if (num4 > 0.75f)
						{
							float num5 = 2f * num4;
							if (settlement != null && mobileParty.DefaultBehavior == AiBehavior.MoveToNearestLandOrPort && mobileParty.TargetSettlement == settlement)
							{
								num5 *= 1.2f;
							}
							ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(new AIBehaviorData(settlement, AiBehavior.MoveToNearestLandOrPort, MobileParty.NavigationType.All, false, false, false), num5);
							p.AddBehaviorScore(in valueTuple);
						}
					}
				}
			}
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x0017D456 File Offset: 0x0017B656
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x040014EB RID: 5355
		private const int MoveToNearestLandMaximumScore = 2;

		// Token: 0x040014EC RID: 5356
		private const float RatioThreshold = 0.75f;
	}
}
