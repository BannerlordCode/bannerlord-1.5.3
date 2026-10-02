using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000169 RID: 361
	public class DefaultTroopSupplierProbabilityModel : TroopSupplierProbabilityModel
	{
		// Token: 0x06001B92 RID: 7058 RVA: 0x0008E90C File Offset: 0x0008CB0C
		public override void EnqueueTroopSpawnProbabilitiesAccordingToUnitSpawnPrioritization(MapEventParty battleParty, FlattenedTroopRoster priorityTroops, bool includePlayer, int sizeOfSide, bool forcePriorityTroops, List<ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>> priorityList)
		{
			UnitSpawnPrioritizations unitSpawnPrioritizations = UnitSpawnPrioritizations.HighLevel;
			MapEvent battle = PlayerEncounter.Battle;
			bool flag = battle != null && battle.IsSiegeAmbush;
			if (battleParty.Party == PartyBase.MainParty && !flag)
			{
				unitSpawnPrioritizations = Game.Current.UnitSpawnPrioritization;
			}
			if (includePlayer)
			{
				List<KeyValuePair<int, FlattenedTroopRosterElement>> list = new List<KeyValuePair<int, FlattenedTroopRosterElement>>();
				List<KeyValuePair<int, FlattenedTroopRosterElement>> list2 = new List<KeyValuePair<int, FlattenedTroopRosterElement>>();
				List<KeyValuePair<int, FlattenedTroopRosterElement>> list3 = new List<KeyValuePair<int, FlattenedTroopRosterElement>>();
				List<KeyValuePair<int, FlattenedTroopRosterElement>> list4 = new List<KeyValuePair<int, FlattenedTroopRosterElement>>();
				int[] array = new int[8];
				for (int i = 0; i < 8; i++)
				{
					array[i] = 0;
				}
				int num = 0;
				foreach (FlattenedTroopRosterElement flattenedTroopRosterElement in battleParty.Troops)
				{
					if (this.CanTroopJoinBattle(flattenedTroopRosterElement, includePlayer))
					{
						int num2 = 0;
						switch (unitSpawnPrioritizations)
						{
						case UnitSpawnPrioritizations.Default:
							num2 = -num;
							break;
						case UnitSpawnPrioritizations.HighLevel:
							num2 = flattenedTroopRosterElement.Troop.Level;
							break;
						case UnitSpawnPrioritizations.LowLevel:
							num2 = -flattenedTroopRosterElement.Troop.Level;
							break;
						case UnitSpawnPrioritizations.Homogeneous:
						{
							int formationClass = (int)flattenedTroopRosterElement.Troop.GetFormationClass();
							num2 = -array[formationClass];
							array[formationClass]++;
							break;
						}
						}
						bool isHero = flattenedTroopRosterElement.Troop.IsHero;
						if (isHero && flattenedTroopRosterElement.Troop.IsPlayerCharacter)
						{
							num2 = int.MaxValue;
						}
						bool flag2 = false;
						if (priorityTroops != null)
						{
							foreach (FlattenedTroopRosterElement flattenedTroopRosterElement2 in priorityTroops)
							{
								if (flattenedTroopRosterElement2.Troop == flattenedTroopRosterElement.Troop)
								{
									flag2 = true;
									break;
								}
							}
						}
						if (flag2)
						{
							priorityTroops.Remove(priorityTroops.FindIndexOfCharacter(flattenedTroopRosterElement.Troop));
							if (isHero)
							{
								list3.Add(new KeyValuePair<int, FlattenedTroopRosterElement>(num2, flattenedTroopRosterElement));
							}
							else
							{
								list4.Add(new KeyValuePair<int, FlattenedTroopRosterElement>(num2, flattenedTroopRosterElement));
							}
						}
						else if (isHero)
						{
							list2.Add(new KeyValuePair<int, FlattenedTroopRosterElement>(num2, flattenedTroopRosterElement));
						}
						else
						{
							list.Add(new KeyValuePair<int, FlattenedTroopRosterElement>(num2, flattenedTroopRosterElement));
						}
					}
					num++;
				}
				if (unitSpawnPrioritizations == UnitSpawnPrioritizations.Homogeneous)
				{
					for (int j = 0; j < list.Count; j++)
					{
						KeyValuePair<int, FlattenedTroopRosterElement> keyValuePair = list[j];
						int formationClass2 = (int)keyValuePair.Value.Troop.GetFormationClass();
						int num3 = keyValuePair.Key * list.Count / array[formationClass2];
						list[j] = new KeyValuePair<int, FlattenedTroopRosterElement>(num3, keyValuePair.Value);
					}
				}
				list3 = list3.OrderByQ<KeyValuePair<int, FlattenedTroopRosterElement>, int>((KeyValuePair<int, FlattenedTroopRosterElement> x) => x.Key).ToList<KeyValuePair<int, FlattenedTroopRosterElement>>();
				list4 = list4.OrderByQ<KeyValuePair<int, FlattenedTroopRosterElement>, int>((KeyValuePair<int, FlattenedTroopRosterElement> x) => x.Key).ToList<KeyValuePair<int, FlattenedTroopRosterElement>>();
				list2 = list2.OrderByQ<KeyValuePair<int, FlattenedTroopRosterElement>, int>((KeyValuePair<int, FlattenedTroopRosterElement> x) => x.Key).ToList<KeyValuePair<int, FlattenedTroopRosterElement>>();
				list = list.OrderByQ<KeyValuePair<int, FlattenedTroopRosterElement>, int>((KeyValuePair<int, FlattenedTroopRosterElement> x) => x.Key).ToList<KeyValuePair<int, FlattenedTroopRosterElement>>();
				for (int k = 0; k < list3.Count; k++)
				{
					priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(list3[k].Value, battleParty, 3f + (float)(k + 1) / (float)list3.Count));
				}
				for (int l = 0; l < list4.Count; l++)
				{
					priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(list4[l].Value, battleParty, 2f + (float)(l + 1) / (float)list4.Count));
				}
				for (int m = 0; m < list2.Count; m++)
				{
					priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(list2[m].Value, battleParty, 1f + (float)(m + 1) / (float)list2.Count));
				}
				for (int n = 0; n < list.Count; n++)
				{
					priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(list[n].Value, battleParty, (float)(n + 1) / (float)list.Count));
				}
				return;
			}
			foreach (FlattenedTroopRosterElement flattenedTroopRosterElement3 in battleParty.Troops)
			{
				if (this.CanTroopJoinBattle(flattenedTroopRosterElement3, includePlayer))
				{
					priorityList.Add(new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(flattenedTroopRosterElement3, battleParty, 2.1474836E+09f));
				}
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0008EDD4 File Offset: 0x0008CFD4
		private bool CanTroopJoinBattle(FlattenedTroopRosterElement troopRoster, bool includePlayer)
		{
			return !troopRoster.IsWounded && !troopRoster.IsRouted && !troopRoster.IsKilled && (includePlayer || !troopRoster.Troop.IsPlayerCharacter);
		}
	}
}
