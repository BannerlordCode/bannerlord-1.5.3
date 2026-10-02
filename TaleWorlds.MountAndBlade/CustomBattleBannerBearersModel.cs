using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000210 RID: 528
	public class CustomBattleBannerBearersModel : BattleBannerBearersModel
	{
		// Token: 0x06001E99 RID: 7833 RVA: 0x00068FA8 File Offset: 0x000671A8
		public override int GetMinimumFormationTroopCountToBearBanners()
		{
			return 2;
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x00068FAB File Offset: 0x000671AB
		public override float GetBannerInteractionDistance(Agent interactingAgent)
		{
			if (!interactingAgent.HasMount)
			{
				return 1.5f;
			}
			return 3f;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x00068FC0 File Offset: 0x000671C0
		public override bool CanBannerBearerProvideEffectToFormation(Agent agent, Formation formation)
		{
			return agent.Formation == formation || (agent.IsPlayerControlled && agent.Team == formation.Team);
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x00068FE8 File Offset: 0x000671E8
		public override bool CanAgentPickUpAnyBanner(Agent agent)
		{
			return agent.IsHuman && agent.Banner == null && agent.CanBeAssignedForScriptedMovement() && (agent.CommonAIComponent == null || !agent.CommonAIComponent.IsPanicked) && (agent.HumanAIComponent == null || !agent.HumanAIComponent.IsInImportantCombatAction());
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0006903C File Offset: 0x0006723C
		public override bool CanAgentBecomeBannerBearer(Agent agent)
		{
			if (CustomBattleBannerBearersModel._missionSpawnLogic == null)
			{
				CustomBattleBannerBearersModel._missionSpawnLogic = Mission.Current.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			}
			if (CustomBattleBannerBearersModel._missionSpawnLogic != null)
			{
				Formation formation = agent.Formation;
				Team team = ((formation != null) ? formation.Team : null);
				if (team != null)
				{
					BasicCharacterObject generalCharacterOfSide = CustomBattleBannerBearersModel._missionSpawnLogic.GetGeneralCharacterOfSide(team.Side);
					return agent.IsHuman && !agent.IsMainAgent && !agent.IsHero && agent.IsAIControlled && agent.Character != generalCharacterOfSide;
				}
			}
			return false;
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000690C0 File Offset: 0x000672C0
		public override int GetAgentBannerBearingPriority(Agent agent)
		{
			if (!this.CanAgentBecomeBannerBearer(agent))
			{
				return 0;
			}
			if (agent.Formation != null)
			{
				bool calculateHasSignificantNumberOfMounted = agent.Formation.CalculateHasSignificantNumberOfMounted;
				if ((calculateHasSignificantNumberOfMounted && !agent.HasMount) || (!calculateHasSignificantNumberOfMounted && agent.HasMount))
				{
					return 0;
				}
			}
			if (agent.Banner != null)
			{
				return int.MaxValue;
			}
			int num = Math.Min(agent.Character.Level / 4 + 1, CustomBattleBannerBearersModel.BannerBearerPriorityPerTier.Length - 1);
			return CustomBattleBannerBearersModel.BannerBearerPriorityPerTier[num];
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x00069138 File Offset: 0x00067338
		public override bool CanFormationDeployBannerBearers(Formation formation)
		{
			BannerBearerLogic bannerBearerLogic = base.BannerBearerLogic;
			return bannerBearerLogic != null && formation.CountOfUnits >= this.GetMinimumFormationTroopCountToBearBanners() && bannerBearerLogic.GetFormationBanner(formation) != null && formation.UnitsWithoutLooseDetachedOnes.Count<IFormationUnit>(delegate(IFormationUnit unit)
			{
				Agent agent;
				return (agent = unit as Agent) != null && this.CanAgentBecomeBannerBearer(agent);
			}) > 0;
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x00069182 File Offset: 0x00067382
		public override int GetDesiredNumberOfBannerBearersForFormation(Formation formation)
		{
			if (!this.CanFormationDeployBannerBearers(formation))
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x00069190 File Offset: 0x00067390
		public override ItemObject GetBannerBearerReplacementWeapon(BasicCharacterObject agentCharacter)
		{
			if (CustomBattleBannerBearersModel.ReplacementWeapons == null)
			{
				CustomBattleBannerBearersModel.ReplacementWeapons = MBObjectManager.Instance.GetObjectTypeList<ItemObject>().Where<ItemObject>(delegate(ItemObject item)
				{
					if (item.PrimaryWeapon != null)
					{
						WeaponComponentData primaryWeapon = item.PrimaryWeapon;
						return primaryWeapon.WeaponClass == WeaponClass.OneHandedSword;
					}
					return false;
				}).ToList<ItemObject>();
			}
			if (CustomBattleBannerBearersModel.ReplacementWeapons.IsEmpty<ItemObject>())
			{
				return null;
			}
			IEnumerable<ItemObject> enumerable = CustomBattleBannerBearersModel.ReplacementWeapons.Where<ItemObject>((ItemObject item) => item.Culture != null && item.Culture == agentCharacter.Culture);
			List<ValueTuple<int, ItemObject>> list = new List<ValueTuple<int, ItemObject>>();
			int minTierDifference = int.MaxValue;
			foreach (ItemObject itemObject in enumerable)
			{
				int num = MathF.Ceiling(((float)agentCharacter.Level - 5f) / 5f);
				num = MathF.Min(MathF.Max(num, 0), 7);
				int num2 = MathF.Abs(itemObject.Tier - (ItemObject.ItemTiers)num);
				if (num2 < minTierDifference)
				{
					minTierDifference = num2;
				}
				list.Add(new ValueTuple<int, ItemObject>(num2, itemObject));
			}
			return list.Where<ValueTuple<int, ItemObject>>(([TupleElementNames(new string[] { "TierDifference", "Weapon" })] ValueTuple<int, ItemObject> tuple) => tuple.Item1 == minTierDifference).GetRandomElementInefficiently<ValueTuple<int, ItemObject>>().Item2;
		}

		// Token: 0x04000A6D RID: 2669
		private static readonly int[] BannerBearerPriorityPerTier = new int[] { 0, 1, 3, 5, 6, 4, 2 };

		// Token: 0x04000A6E RID: 2670
		private static List<ItemObject> ReplacementWeapons = null;

		// Token: 0x04000A6F RID: 2671
		private static DefaultBattleMissionAgentSpawnLogic _missionSpawnLogic;
	}
}
