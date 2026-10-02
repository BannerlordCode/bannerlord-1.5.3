using System;
using StoryMode.StoryModePhases;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x0200004A RID: 74
	public class StoryModeTargetScoreCalculatingModel : TargetScoreCalculatingModel
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x000198E0 File Offset: 0x00017AE0
		public override float TravelingToAssignmentFactor
		{
			get
			{
				return base.BaseModel.TravelingToAssignmentFactor;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x000198ED File Offset: 0x00017AED
		public override float BesiegingFactor
		{
			get
			{
				return base.BaseModel.BesiegingFactor;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x000198FA File Offset: 0x00017AFA
		public override float AssaultingTownFactor
		{
			get
			{
				return base.BaseModel.AssaultingTownFactor;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00019907 File Offset: 0x00017B07
		public override float RaidingFactor
		{
			get
			{
				return base.BaseModel.RaidingFactor;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00019914 File Offset: 0x00017B14
		public override float DefendingFactor
		{
			get
			{
				return base.BaseModel.DefendingFactor;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00019921 File Offset: 0x00017B21
		public override float GetDefensivePatrollingFactor(bool isNavalPatrolling)
		{
			return base.BaseModel.GetDefensivePatrollingFactor(isNavalPatrolling);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001992F File Offset: 0x00017B2F
		public override float GetOffensivePatrollingFactor(bool isNavalPatrolling)
		{
			return base.BaseModel.GetOffensivePatrollingFactor(isNavalPatrolling);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001993D File Offset: 0x00017B3D
		public override float CalculateDefensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty)
		{
			return base.BaseModel.CalculateDefensivePatrollingScoreForSettlement(settlement, isTargetingPort, mobileParty);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001994D File Offset: 0x00017B4D
		public override float CalculateOffensivePatrollingScoreForSettlement(Settlement settlement, bool isTargetingPort, MobileParty mobileParty)
		{
			return base.BaseModel.CalculateOffensivePatrollingScoreForSettlement(settlement, isTargetingPort, mobileParty);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001995D File Offset: 0x00017B5D
		public override float CurrentObjectiveValue(MobileParty mobileParty)
		{
			return base.BaseModel.CurrentObjectiveValue(mobileParty);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001996C File Offset: 0x00017B6C
		public override float GetTargetScoreForFaction(Settlement targetSettlement, Army.ArmyTypes missionType, MobileParty mobileParty, float ourStrength)
		{
			if (missionType == Army.ArmyTypes.Raider && targetSettlement != null && targetSettlement.StringId == "village_ES3_2" && TutorialPhase.Instance != null && !TutorialPhase.Instance.IsCompleted)
			{
				return 0f;
			}
			return base.BaseModel.GetTargetScoreForFaction(targetSettlement, missionType, mobileParty, ourStrength);
		}
	}
}
