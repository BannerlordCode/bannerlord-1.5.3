using System;
using Helpers;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000441 RID: 1089
	public class NPCEquipmentsCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004611 RID: 17937 RVA: 0x00154DAB File Offset: 0x00152FAB
		public override void RegisterEvents()
		{
			CampaignEvents.RulingClanChanged.AddNonSerializedListener(this, new Action<Kingdom, Clan>(this.OnRulingClanChanged));
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x00154DC4 File Offset: 0x00152FC4
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x00154DC8 File Offset: 0x00152FC8
		private void OnRulingClanChanged(Kingdom kingdom, Clan oldRulingClan)
		{
			Hero leader = kingdom.Leader;
			Hero hero = ((oldRulingClan != null) ? oldRulingClan.Leader : null);
			ValueTuple<Equipment, Equipment> equipmentsForChangingRuler = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentsForChangingRuler(leader, hero, Equipment.EquipmentType.Civilian);
			ValueTuple<Equipment, Equipment> equipmentsForChangingRuler2 = Campaign.Current.Models.EquipmentSelectionModel.GetEquipmentsForChangingRuler(leader, hero, Equipment.EquipmentType.Battle);
			if (leader != Hero.MainHero)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(leader, equipmentsForChangingRuler2.Item1);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(leader, equipmentsForChangingRuler.Item1);
			}
			if (hero != null && hero.IsActive && hero != Hero.MainHero)
			{
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentsForChangingRuler2.Item2);
				EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, equipmentsForChangingRuler.Item2);
			}
		}
	}
}
