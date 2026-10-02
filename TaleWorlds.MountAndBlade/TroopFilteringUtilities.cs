using System;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000398 RID: 920
	public static class TroopFilteringUtilities
	{
		// Token: 0x0600353C RID: 13628 RVA: 0x000DC208 File Offset: 0x000DA408
		public static TroopTraitsMask GetFilter(bool isMounted, bool isRanged, bool isMelee, bool hasHeavyArmor, bool hasThrown, bool hasSpear, bool hasShield)
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (hasHeavyArmor)
			{
				troopTraitsMask |= TroopTraitsMask.Armor;
			}
			if (hasThrown)
			{
				troopTraitsMask |= TroopTraitsMask.Thrown;
			}
			if (hasSpear)
			{
				troopTraitsMask |= TroopTraitsMask.Spear;
			}
			if (hasShield)
			{
				troopTraitsMask |= TroopTraitsMask.Shield;
			}
			if (isMelee)
			{
				troopTraitsMask |= TroopTraitsMask.Melee;
			}
			if (isRanged)
			{
				troopTraitsMask |= TroopTraitsMask.Ranged;
			}
			if (isMounted)
			{
				troopTraitsMask |= TroopTraitsMask.Mount;
			}
			return troopTraitsMask;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x000DC250 File Offset: 0x000DA450
		public static TroopTraitsMask GetFilter(params FormationClass[] formationClasses)
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (formationClasses.Length == 1)
			{
				switch (formationClasses[0])
				{
				case FormationClass.Infantry:
					troopTraitsMask = TroopTraitsMask.Melee;
					break;
				case FormationClass.Ranged:
					troopTraitsMask = TroopTraitsMask.Ranged;
					break;
				case FormationClass.Cavalry:
					troopTraitsMask = TroopTraitsMask.Melee | TroopTraitsMask.Mount;
					break;
				case FormationClass.HorseArcher:
					troopTraitsMask = TroopTraitsMask.Ranged | TroopTraitsMask.Mount;
					break;
				}
			}
			else if (formationClasses.Length == 2)
			{
				if (formationClasses[0] == FormationClass.Infantry && formationClasses[1] == FormationClass.Ranged)
				{
					troopTraitsMask = TroopTraitsMask.Melee | TroopTraitsMask.Ranged;
				}
				if (formationClasses[0] == FormationClass.Cavalry && formationClasses[1] == FormationClass.HorseArcher)
				{
					troopTraitsMask = TroopTraitsMask.Melee | TroopTraitsMask.Ranged | TroopTraitsMask.Mount;
				}
			}
			return troopTraitsMask;
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x000DC2B4 File Offset: 0x000DA4B4
		public static TroopTraitsMask GetFilter(params FormationFilterType[] filterTypes)
		{
			TroopTraitsMask troopTraitsMask = TroopTraitsMask.None;
			if (filterTypes.Length != 0)
			{
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Heavy))
				{
					troopTraitsMask |= TroopTraitsMask.Armor;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Shield))
				{
					troopTraitsMask |= TroopTraitsMask.Shield;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Thrown))
				{
					troopTraitsMask |= TroopTraitsMask.Thrown;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.Spear))
				{
					troopTraitsMask |= TroopTraitsMask.Spear;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.HighTier))
				{
					troopTraitsMask |= TroopTraitsMask.HighTier;
				}
				if (filterTypes.Any<FormationFilterType>((FormationFilterType f) => f == FormationFilterType.LowTier))
				{
					troopTraitsMask |= TroopTraitsMask.LowTier;
				}
			}
			return troopTraitsMask;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x000DC3D8 File Offset: 0x000DA5D8
		public static void GetPriorityFunction(TroopTraitsMask filter, out Func<Agent, int> priorityFunc)
		{
			priorityFunc = delegate(Agent agent)
			{
				if (agent == null || agent.Character == null)
				{
					return TroopFilteringUtilities.GetMaxPriority(filter);
				}
				return TroopFilteringUtilities.GetTroopPriority(agent.GetTraitsMask(), agent.Character.GetBattleTier(), filter);
			};
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x000DC400 File Offset: 0x000DA600
		public static void GetPriorityFunction(TroopTraitsMask filter, out Func<IAgentOriginBase, int> priorityFunc)
		{
			priorityFunc = delegate(IAgentOriginBase agentOrigin)
			{
				if (agentOrigin == null || agentOrigin.Troop == null)
				{
					return TroopFilteringUtilities.GetMaxPriority(filter);
				}
				return TroopFilteringUtilities.GetTroopPriority(agentOrigin.GetTraitsMask(), agentOrigin.Troop.GetBattleTier(), filter);
			};
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x000DC428 File Offset: 0x000DA628
		public static int GetTroopPriority(TroopTraitsMask troopMask, int battleTier, TroopTraitsMask filter)
		{
			int num = 1;
			if ((filter & TroopTraitsMask.HighTier) != TroopTraitsMask.None)
			{
				num += battleTier;
			}
			if ((filter & TroopTraitsMask.LowTier) != TroopTraitsMask.None)
			{
				num += 7 - battleTier;
			}
			TroopTraitsMask troopTraitsMask = filter & troopMask;
			if ((troopTraitsMask & TroopTraitsMask.Shield) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Spear) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Thrown) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Armor) != TroopTraitsMask.None)
			{
				num += 10;
			}
			if ((troopTraitsMask & TroopTraitsMask.Melee) != TroopTraitsMask.None || (troopTraitsMask & TroopTraitsMask.Ranged) != TroopTraitsMask.None)
			{
				num += 100;
			}
			if ((troopTraitsMask & TroopTraitsMask.Mount) != TroopTraitsMask.None)
			{
				num += 1000;
			}
			return num;
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x000DC4A0 File Offset: 0x000DA6A0
		public static int GetMaxPriority(TroopTraitsMask filter)
		{
			return 1 + (((filter & TroopTraitsMask.HighTier) != TroopTraitsMask.None) ? 7 : 0) + (((filter & TroopTraitsMask.LowTier) != TroopTraitsMask.None) ? 7 : 0) + (((filter & TroopTraitsMask.Shield) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Spear) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Thrown) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Armor) != TroopTraitsMask.None) ? 10 : 0) + (((filter & TroopTraitsMask.Melee) != TroopTraitsMask.None || (filter & TroopTraitsMask.Ranged) != TroopTraitsMask.None) ? 100 : 0) + (((filter & TroopTraitsMask.Mount) != TroopTraitsMask.None) ? 1000 : 0);
		}

		// Token: 0x04001687 RID: 5767
		public const int MinPriority = 1;

		// Token: 0x04001688 RID: 5768
		public const int EquipmentPriority = 10;

		// Token: 0x04001689 RID: 5769
		public const int EngagementTypePriority = 100;

		// Token: 0x0400168A RID: 5770
		public const int MountedPriority = 1000;
	}
}
