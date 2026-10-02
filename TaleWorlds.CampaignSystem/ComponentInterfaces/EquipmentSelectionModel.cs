using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000201 RID: 513
	public abstract class EquipmentSelectionModel : MBGameModel<EquipmentSelectionModel>
	{
		// Token: 0x06002013 RID: 8211
		public abstract Equipment GetEquipmentForHeroComeOfAge(Hero hero, Equipment.EquipmentType equipmentType);

		// Token: 0x06002014 RID: 8212
		public abstract Equipment GetEquipmentForHeroReachesTeenAge(Hero hero);

		// Token: 0x06002015 RID: 8213
		public abstract Equipment GetEquipmentForInitialChildrenGeneration(Hero hero);

		// Token: 0x06002016 RID: 8214
		public abstract Equipment GetEquipmentForDeliveredOffspring(Hero hero);

		// Token: 0x06002017 RID: 8215
		public abstract ValueTuple<Equipment, Equipment> GetEquipmentsForChangingRuler(Hero newRuler, Hero oldRuler, Equipment.EquipmentType equipmentType);

		// Token: 0x06002018 RID: 8216
		public abstract Equipment GetEquipmentForCompanionWhenTurningToLord(Hero companionHero, Equipment.EquipmentType equipmentType);
	}
}
