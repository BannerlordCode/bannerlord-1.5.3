using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MBHelpers;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001FE RID: 510
	public class CustomBattleMoraleModel : BattleMoraleModel
	{
		// Token: 0x06001E07 RID: 7687 RVA: 0x0006651C File Offset: 0x0006471C
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentIncapacitated(Agent affectedAgent, AgentState affectedAgentState, Agent affectorAgent, in KillingBlow killingBlow)
		{
			float battleImportance = affectedAgent.GetBattleImportance();
			Team team = affectedAgent.Team;
			BattleSideEnum battleSideEnum = ((team != null) ? team.Side : BattleSideEnum.None);
			float num = this.CalculateCasualtiesFactor(battleSideEnum);
			SkillObject relevantSkillFromWeaponClass = WeaponComponentData.GetRelevantSkillFromWeaponClass((WeaponClass)killingBlow.WeaponClass);
			bool flag = relevantSkillFromWeaponClass == DefaultSkills.Bow || relevantSkillFromWeaponClass == DefaultSkills.Crossbow || relevantSkillFromWeaponClass == DefaultSkills.Throwing;
			bool flag2 = relevantSkillFromWeaponClass == DefaultSkills.OneHanded || relevantSkillFromWeaponClass == DefaultSkills.TwoHanded || relevantSkillFromWeaponClass == DefaultSkills.Polearm;
			bool flag3 = killingBlow.WeaponRecordWeaponFlags.HasAnyFlag(WeaponFlags.AffectsArea | WeaponFlags.AffectsAreaBig | WeaponFlags.MultiplePenetration);
			float num2 = 0.75f;
			if (flag3)
			{
				num2 = 0.25f;
				if (killingBlow.WeaponRecordWeaponFlags.HasAllFlags(WeaponFlags.Burning | WeaponFlags.MultiplePenetration))
				{
					num2 += num2 * 0.25f;
				}
			}
			else if (flag)
			{
				num2 = 0.5f;
			}
			num2 = Math.Max(0f, num2);
			FactoredNumber factoredNumber = new FactoredNumber(battleImportance * 3f * num2);
			FactoredNumber factoredNumber2 = new FactoredNumber(battleImportance * 4f * num2 * num);
			Formation formation = affectedAgent.Formation;
			BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation);
			if (activeBanner != null)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMoraleShock, activeBanner, ref factoredNumber2);
			}
			Formation formation2 = affectorAgent.Formation;
			BannerComponent activeBanner2 = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation2);
			if (activeBanner2 != null && affectorAgent.Character.DefaultFormationClass == FormationClass.Infantry && flag2)
			{
				BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.IncreasedMoraleShockByMeleeTroops, activeBanner2, ref factoredNumber);
			}
			return new ValueTuple<float, float>(MathF.Max(factoredNumber2.ResultNumber, 0f), MathF.Max(factoredNumber.ResultNumber, 0f));
		}

		// Token: 0x06001E08 RID: 7688 RVA: 0x000666AC File Offset: 0x000648AC
		[return: TupleElementNames(new string[] { "affectedSideMaxMoraleLoss", "affectorSideMaxMoraleGain" })]
		public override ValueTuple<float, float> CalculateMaxMoraleChangeDueToAgentPanicked(Agent agent)
		{
			float battleImportance = agent.GetBattleImportance();
			Team team = agent.Team;
			BattleSideEnum battleSideEnum = ((team != null) ? team.Side : BattleSideEnum.None);
			float num = this.CalculateCasualtiesFactor(battleSideEnum);
			float num2 = battleImportance * 2f;
			float num3 = battleImportance * num * 1.1f;
			if (agent.Character != null)
			{
				FactoredNumber factoredNumber = new FactoredNumber(num3);
				Formation formation = agent.Formation;
				BannerComponent activeBanner = MissionGameModels.Current.BattleBannerBearersModel.GetActiveBanner(formation);
				if (activeBanner != null)
				{
					BannerHelper.AddBannerBonusForBanner(DefaultBannerEffects.DecreasedMoraleShock, activeBanner, ref factoredNumber);
				}
				num3 = factoredNumber.ResultNumber;
			}
			return new ValueTuple<float, float>(MathF.Max(num3, 0f), MathF.Max(num2, 0f));
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0006674B File Offset: 0x0006494B
		public override float CalculateMoraleChangeToCharacter(Agent agent, float maxMoraleChange)
		{
			return maxMoraleChange / MathF.Max(1f, agent.Character.GetMoraleResistance());
		}

		// Token: 0x06001E0A RID: 7690 RVA: 0x00066764 File Offset: 0x00064964
		public override float GetEffectiveInitialMorale(Agent agent, float baseMorale)
		{
			return baseMorale;
		}

		// Token: 0x06001E0B RID: 7691 RVA: 0x00066767 File Offset: 0x00064967
		public override bool CanPanicDueToMorale(Agent agent)
		{
			return true;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0006676C File Offset: 0x0006496C
		public override float CalculateCasualtiesFactor(BattleSideEnum battleSide)
		{
			float num = 1f;
			if (Mission.Current != null && battleSide != BattleSideEnum.None)
			{
				float removedAgentRatioForSide = Mission.Current.GetRemovedAgentRatioForSide(battleSide);
				num += removedAgentRatioForSide * 2f;
				num = MathF.Max(0f, num);
			}
			return num;
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x000667B0 File Offset: 0x000649B0
		public override float GetAverageMorale(Formation formation)
		{
			float num = 0f;
			int num2 = 0;
			if (formation != null)
			{
				using (List<IFormationUnit>.Enumerator enumerator = formation.Arrangement.GetAllUnits().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Agent agent;
						if ((agent = enumerator.Current as Agent) != null && agent.IsActive() && agent.IsHuman && agent.IsAIControlled)
						{
							num2++;
							num += agent.GetMorale();
						}
					}
				}
			}
			if (num2 > 0)
			{
				return MBMath.ClampFloat(num / (float)num2, 0f, 100f);
			}
			return 0f;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x00066858 File Offset: 0x00064A58
		public override float CalculateMoraleChangeOnShipSunk(IShipOrigin shipOrigin)
		{
			return 0f;
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x0006685F File Offset: 0x00064A5F
		public override float CalculateMoraleOnRamming(Agent agent, IShipOrigin rammingShip, IShipOrigin rammedShip)
		{
			return agent.GetMorale();
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x00066867 File Offset: 0x00064A67
		public override float CalculateMoraleOnShipsConnected(Agent agent, IShipOrigin ownerShip, IShipOrigin targetShip)
		{
			return agent.GetMorale();
		}
	}
}
