using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200038E RID: 910
	public static class OrderOfBattleFormationExtensions
	{
		// Token: 0x06003498 RID: 13464 RVA: 0x000D9DB8 File Offset: 0x000D7FB8
		public unsafe static void Refresh(this Formation formation)
		{
			if (formation != null)
			{
				MovementOrder movementOrder = *formation.GetReadonlyMovementOrderReference();
				if (movementOrder.OrderEnum == MovementOrder.MovementOrderEnum.Stop)
				{
					OrderController.TryCancelStopOrder(formation);
				}
				formation.ApplyActionOnEachUnit(delegate(Agent u)
				{
					u.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				formation.SetMovementOrder(movementOrder);
			}
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x000D9E11 File Offset: 0x000D8011
		public static DeploymentFormationClass GetOrderOfBattleFormationClass(this FormationClass formationClass)
		{
			switch (formationClass)
			{
			case FormationClass.Infantry:
			case FormationClass.NumberOfDefaultFormations:
			case FormationClass.HeavyInfantry:
				return DeploymentFormationClass.Infantry;
			case FormationClass.Ranged:
				return DeploymentFormationClass.Ranged;
			case FormationClass.Cavalry:
			case FormationClass.LightCavalry:
			case FormationClass.HeavyCavalry:
				return DeploymentFormationClass.Cavalry;
			case FormationClass.HorseArcher:
				return DeploymentFormationClass.HorseArcher;
			default:
				return DeploymentFormationClass.Unset;
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000D9E44 File Offset: 0x000D8044
		public static List<FormationClass> GetFormationClasses(this DeploymentFormationClass orderOfBattleFormationClass)
		{
			List<FormationClass> list = new List<FormationClass>();
			switch (orderOfBattleFormationClass)
			{
			case DeploymentFormationClass.Infantry:
				list.Add(FormationClass.Infantry);
				break;
			case DeploymentFormationClass.Ranged:
				list.Add(FormationClass.Ranged);
				break;
			case DeploymentFormationClass.Cavalry:
				list.Add(FormationClass.Cavalry);
				break;
			case DeploymentFormationClass.HorseArcher:
				list.Add(FormationClass.HorseArcher);
				break;
			case DeploymentFormationClass.InfantryAndRanged:
				list.Add(FormationClass.Infantry);
				list.Add(FormationClass.Ranged);
				break;
			case DeploymentFormationClass.CavalryAndHorseArcher:
				list.Add(FormationClass.Cavalry);
				list.Add(FormationClass.HorseArcher);
				break;
			}
			return list;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000D9EBC File Offset: 0x000D80BC
		public static TextObject GetFilterName(this FormationFilterType filterType)
		{
			switch (filterType)
			{
			case FormationFilterType.Shield:
				return new TextObject("{=PSN8IaIg}Shields", null);
			case FormationFilterType.Spear:
				return new TextObject("{=f83FU4X6}Polearms", null);
			case FormationFilterType.Thrown:
				return new TextObject("{=Ea3K1PVR}Thrown Weapons", null);
			case FormationFilterType.Heavy:
				return new TextObject("{=Jw0GMgzv}Heavy Armors", null);
			case FormationFilterType.HighTier:
				return new TextObject("{=DzAkCzwd}High Tier", null);
			case FormationFilterType.LowTier:
				return new TextObject("{=qaPgbwZv}Low Tier", null);
			default:
				return new TextObject("{=w7Yrbi5t}Unset", null);
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000D9F40 File Offset: 0x000D8140
		public static TextObject GetFilterDescription(this FormationFilterType filterType)
		{
			switch (filterType)
			{
			case FormationFilterType.Unset:
				return new TextObject("{=Q1Ga032B}Don't give preference to any type of troop.", null);
			case FormationFilterType.Shield:
				return new TextObject("{=MVOPbhNj}Give preference to troops with Shields", null);
			case FormationFilterType.Spear:
				return new TextObject("{=K3Cr70PY}Give preference to troops with Polearms", null);
			case FormationFilterType.Thrown:
				return new TextObject("{=DWWa3aIb}Give preference to troops with Thrown Weapons", null);
			case FormationFilterType.Heavy:
				return new TextObject("{=ush8OHIw}Give preference to troops with Heavy Armors", null);
			case FormationFilterType.HighTier:
				return new TextObject("{=DRNDtkP2}Give preference to troops at higher tiers", null);
			case FormationFilterType.LowTier:
				return new TextObject("{=zbpCRmuJ}Give preference to troops at lower tiers", null);
			default:
				return new TextObject("{=w7Yrbi5t}Unset", null);
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000D9FD0 File Offset: 0x000D81D0
		public static TextObject GetClassName(this DeploymentFormationClass formationClass)
		{
			switch (formationClass)
			{
			case DeploymentFormationClass.Infantry:
				return GameTexts.FindText("str_troop_type_name", "Infantry");
			case DeploymentFormationClass.Ranged:
				return GameTexts.FindText("str_troop_type_name", "Ranged");
			case DeploymentFormationClass.Cavalry:
				return GameTexts.FindText("str_troop_type_name", "Cavalry");
			case DeploymentFormationClass.HorseArcher:
				return GameTexts.FindText("str_troop_type_name", "HorseArcher");
			case DeploymentFormationClass.InfantryAndRanged:
				return new TextObject("{=mBDj5uG5}Infantry and Ranged", null);
			case DeploymentFormationClass.CavalryAndHorseArcher:
				return new TextObject("{=FNLfNWH3}Cavalry and Horse Archer", null);
			default:
				return new TextObject("{=w7Yrbi5t}Unset", null);
			}
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000DA062 File Offset: 0x000D8262
		public static List<Agent> GetHeroAgents(this Team team)
		{
			return team.ActiveAgents.Where<Agent>((Agent a) => a.IsHero).ToList<Agent>();
		}
	}
}
