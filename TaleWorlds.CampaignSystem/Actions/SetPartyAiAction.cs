using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.Actions
{
	// Token: 0x020004EF RID: 1263
	public static class SetPartyAiAction
	{
		// Token: 0x06004DE7 RID: 19943 RVA: 0x0018A404 File Offset: 0x00188604
		private static void ApplyInternal(MobileParty owner, Settlement settlement, MobileParty mobileParty, CampaignVec2 position, SetPartyAiAction.SetPartyAiActionDetail detail, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPort)
		{
			if (detail == SetPartyAiAction.SetPartyAiActionDetail.GoToSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.GoToSettlement || owner.TargetSettlement != settlement || navigationType != owner.DesiredAiNavigationType || owner.IsTargetingPort != isTargetingPort || owner.StartTransitionNextFrameToExitFromPort != isFromPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveGoToSettlement(settlement, navigationType, isTargetingPort);
				}
				if (owner.Army != null && owner.Army.LeaderParty == owner)
				{
					owner.Army.ArmyType = Army.ArmyTypes.Defender;
					owner.Army.AiBehaviorObject = settlement;
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.PatrolAroundSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.PatrolAroundPoint || owner.TargetSettlement != settlement || navigationType != owner.DesiredAiNavigationType || owner.IsTargetingPort != isTargetingPort || owner.StartTransitionNextFrameToExitFromPort != isFromPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMovePatrolAroundSettlement(settlement, navigationType, isTargetingPort);
				}
				if (owner.Army != null && owner.Army.LeaderParty == owner)
				{
					owner.Army.ArmyType = Army.ArmyTypes.Defender;
					owner.Army.AiBehaviorObject = settlement;
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.RaidSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.RaidSettlement || owner.TargetSettlement != settlement || navigationType != owner.DesiredAiNavigationType || owner.StartTransitionNextFrameToExitFromPort != isFromPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveRaidSettlement(settlement, navigationType, isTargetingPort);
					if (owner.Army != null && owner.Army.LeaderParty == owner)
					{
						owner.Army.ArmyType = Army.ArmyTypes.Raider;
						owner.Army.AiBehaviorObject = settlement;
						return;
					}
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.BesiegeSettlement)
			{
				if (owner.DefaultBehavior != AiBehavior.BesiegeSettlement || owner.TargetSettlement != settlement || navigationType != owner.DesiredAiNavigationType || owner.StartTransitionNextFrameToExitFromPort != isFromPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveBesiegeSettlement(settlement, navigationType);
					if (owner.Army != null && owner.Army.LeaderParty == owner)
					{
						owner.Army.ArmyType = Army.ArmyTypes.Besieger;
						owner.Army.AiBehaviorObject = settlement;
						return;
					}
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.GoAroundParty)
			{
				if (owner.DefaultBehavior != AiBehavior.GoAroundParty || owner != mobileParty || navigationType != owner.DesiredAiNavigationType || owner.StartTransitionNextFrameToExitFromPort != isFromPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveGoAroundParty(mobileParty, navigationType);
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.EngageParty)
			{
				if (owner.DefaultBehavior != AiBehavior.EngageParty || owner != mobileParty || navigationType != owner.DesiredAiNavigationType || owner.StartTransitionNextFrameToExitFromPort != isFromPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveEngageParty(mobileParty, navigationType);
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.DefendParty)
			{
				if (owner.DefaultBehavior != AiBehavior.DefendSettlement || owner != mobileParty || navigationType != owner.DesiredAiNavigationType || owner.StartTransitionNextFrameToExitFromPort != isFromPort || owner.IsTargetingPort != isTargetingPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveDefendSettlement(settlement, isTargetingPort, navigationType);
					if (owner.Army != null && owner.Army.LeaderParty == owner)
					{
						owner.Army.ArmyType = Army.ArmyTypes.Defender;
						owner.Army.AiBehaviorObject = settlement;
						return;
					}
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.EscortParty)
			{
				if (owner.DefaultBehavior != AiBehavior.EscortParty || owner.TargetParty != mobileParty || navigationType != owner.DesiredAiNavigationType || owner.StartTransitionNextFrameToExitFromPort != isFromPort || owner.IsTargetingPort != isTargetingPort)
				{
					if (isFromPort && !owner.IsTransitionInProgress)
					{
						owner.StartTransitionNextFrameToExitFromPort = true;
					}
					owner.SetMoveEscortParty(mobileParty, navigationType, isTargetingPort);
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.MoveToNearestLand)
			{
				if (owner.DefaultBehavior != AiBehavior.MoveToNearestLandOrPort)
				{
					owner.SetMoveToNearestLand(settlement);
					return;
				}
			}
			else if (detail == SetPartyAiAction.SetPartyAiActionDetail.PatrolAroundPoint && (owner.DefaultBehavior != AiBehavior.PatrolAroundPoint || navigationType != owner.DesiredAiNavigationType))
			{
				owner.SetMovePatrolAroundPoint(position, navigationType);
			}
		}

		// Token: 0x06004DE8 RID: 19944 RVA: 0x0018A7CC File Offset: 0x001889CC
		public static void GetActionForVisitingSettlement(MobileParty owner, Settlement settlement, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPort)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, null, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.GoToSettlement, navigationType, isFromPort, isTargetingPort);
		}

		// Token: 0x06004DE9 RID: 19945 RVA: 0x0018A7E0 File Offset: 0x001889E0
		public static void GetActionForPatrollingAroundSettlement(MobileParty owner, Settlement settlement, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPort)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, null, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.PatrolAroundSettlement, navigationType, isFromPort, isTargetingPort);
		}

		// Token: 0x06004DEA RID: 19946 RVA: 0x0018A7F4 File Offset: 0x001889F4
		public static void GetActionForPatrollingAroundPoint(MobileParty owner, CampaignVec2 position, MobileParty.NavigationType navigationType, bool isFromPort)
		{
			SetPartyAiAction.ApplyInternal(owner, null, null, position, SetPartyAiAction.SetPartyAiActionDetail.PatrolAroundPoint, navigationType, isFromPort, false);
		}

		// Token: 0x06004DEB RID: 19947 RVA: 0x0018A803 File Offset: 0x00188A03
		public static void GetActionForRaidingSettlement(MobileParty owner, Settlement settlement, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPort)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, null, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.RaidSettlement, navigationType, isFromPort, isTargetingPort);
		}

		// Token: 0x06004DEC RID: 19948 RVA: 0x0018A817 File Offset: 0x00188A17
		public static void GetActionForBesiegingSettlement(MobileParty owner, Settlement settlement, MobileParty.NavigationType navigationType, bool isFromPort)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, null, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.BesiegeSettlement, navigationType, isFromPort, false);
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0018A82A File Offset: 0x00188A2A
		public static void GetActionForEngagingParty(MobileParty owner, MobileParty mobileParty, MobileParty.NavigationType navigationType, bool isFromPort)
		{
			SetPartyAiAction.ApplyInternal(owner, null, mobileParty, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.EngageParty, navigationType, isFromPort, false);
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x0018A83D File Offset: 0x00188A3D
		public static void GetActionForGoingAroundParty(MobileParty owner, MobileParty mobileParty, MobileParty.NavigationType navigationType, bool isFromPort)
		{
			SetPartyAiAction.ApplyInternal(owner, null, mobileParty, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.GoAroundParty, navigationType, isFromPort, false);
		}

		// Token: 0x06004DEF RID: 19951 RVA: 0x0018A850 File Offset: 0x00188A50
		public static void GetActionForDefendingSettlement(MobileParty owner, Settlement settlement, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPort)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, null, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.DefendParty, navigationType, isFromPort, isTargetingPort);
		}

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0018A864 File Offset: 0x00188A64
		public static void GetActionForEscortingParty(MobileParty owner, MobileParty mobileParty, MobileParty.NavigationType navigationType, bool isFromPort, bool isTargetingPort)
		{
			SetPartyAiAction.ApplyInternal(owner, null, mobileParty, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.EscortParty, navigationType, isFromPort, isTargetingPort);
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0018A878 File Offset: 0x00188A78
		public static void GetActionForMovingToNearestLand(MobileParty owner, Settlement settlement)
		{
			SetPartyAiAction.ApplyInternal(owner, settlement, null, CampaignVec2.Zero, SetPartyAiAction.SetPartyAiActionDetail.MoveToNearestLand, MobileParty.NavigationType.Naval, false, false);
		}

		// Token: 0x020008F0 RID: 2288
		private enum SetPartyAiActionDetail
		{
			// Token: 0x040026B9 RID: 9913
			GoToSettlement,
			// Token: 0x040026BA RID: 9914
			PatrolAroundSettlement,
			// Token: 0x040026BB RID: 9915
			PatrolAroundPoint,
			// Token: 0x040026BC RID: 9916
			RaidSettlement,
			// Token: 0x040026BD RID: 9917
			BesiegeSettlement,
			// Token: 0x040026BE RID: 9918
			EngageParty,
			// Token: 0x040026BF RID: 9919
			GoAroundParty,
			// Token: 0x040026C0 RID: 9920
			DefendParty,
			// Token: 0x040026C1 RID: 9921
			EscortParty,
			// Token: 0x040026C2 RID: 9922
			MoveToNearestLand
		}
	}
}
