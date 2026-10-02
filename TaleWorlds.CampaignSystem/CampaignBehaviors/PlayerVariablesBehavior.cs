using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000455 RID: 1109
	public class PlayerVariablesBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600479D RID: 18333 RVA: 0x0015FD78 File Offset: 0x0015DF78
		public override void RegisterEvents()
		{
			CampaignEvents.PlayerDesertedBattleEvent.AddNonSerializedListener(this, new Action<int>(this.OnPlayerDesertedBattle));
			CampaignEvents.VillageLooted.AddNonSerializedListener(this, new Action<Village>(this.OnVillageLooted));
			CampaignEvents.OnPlayerBattleEndEvent.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0015FDCA File Offset: 0x0015DFCA
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x0015FDCC File Offset: 0x0015DFCC
		private void OnPlayerDesertedBattle(int sacrificedMenCount)
		{
			SkillLevelingManager.OnTacticsUsed(MobileParty.MainParty, (float)(sacrificedMenCount * 50));
			TraitLevelingHelper.OnTroopsSacrificed();
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0015FDE2 File Offset: 0x0015DFE2
		private void OnVillageLooted(Village village)
		{
			if (PlayerEncounter.Current != null && PlayerEncounter.PlayerIsAttacker && PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.Village == village)
			{
				TraitLevelingHelper.OnVillageRaided();
			}
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x0015FE0B File Offset: 0x0015E00B
		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			TraitLevelingHelper.OnBattleWon(mapEvent, mapEvent.GetPlayerBattleContributionRate());
		}
	}
}
