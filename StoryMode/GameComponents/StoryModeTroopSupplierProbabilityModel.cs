using System;
using System.Collections.Generic;
using System.Linq;
using StoryMode.StoryModeObjects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace StoryMode.GameComponents
{
	// Token: 0x0200004B RID: 75
	public class StoryModeTroopSupplierProbabilityModel : TroopSupplierProbabilityModel
	{
		// Token: 0x06000483 RID: 1155 RVA: 0x000199C4 File Offset: 0x00017BC4
		public override void EnqueueTroopSpawnProbabilitiesAccordingToUnitSpawnPrioritization(MapEventParty battleParty, FlattenedTroopRoster priorityTroops, bool includePlayers, int sizeOfSide, bool forcePriorityTroops, List<ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>> priorityList)
		{
			int count = priorityList.Count;
			base.BaseModel.EnqueueTroopSpawnProbabilitiesAccordingToUnitSpawnPrioritization(battleParty, priorityTroops, includePlayers, sizeOfSide, forcePriorityTroops, priorityList);
			Settlement currentSettlement = Settlement.CurrentSettlement;
			if (currentSettlement != null && currentSettlement.IsHideout && priorityTroops != null)
			{
				if (!StoryModeManager.Current.MainStoryLine.TutorialPhase.IsCompleted)
				{
					for (int i = count; i < priorityList.Count; i++)
					{
						CharacterObject character2 = priorityList[i].Item1.Troop;
						if (character2 == StoryModeHeroes.Radagos.CharacterObject && priorityTroops.All<FlattenedTroopRosterElement>((FlattenedTroopRosterElement t) => t.Troop != character2))
						{
							priorityList[i] = new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(priorityList[i].Item1, priorityList[i].Item2, 0.01f);
							return;
						}
					}
					return;
				}
				for (int j = 0; j < priorityList.Count; j++)
				{
					CharacterObject character = priorityList[j].Item1.Troop;
					if (character == StoryModeHeroes.RadagosHenchman.CharacterObject && priorityTroops.All<FlattenedTroopRosterElement>((FlattenedTroopRosterElement t) => t.Troop != character))
					{
						priorityList[j] = new ValueTuple<FlattenedTroopRosterElement, MapEventParty, float>(priorityList[j].Item1, priorityList[j].Item2, 0.01f);
						return;
					}
				}
			}
		}
	}
}
