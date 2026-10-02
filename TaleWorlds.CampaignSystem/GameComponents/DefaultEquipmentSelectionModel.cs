using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200011C RID: 284
	public class DefaultEquipmentSelectionModel : EquipmentSelectionModel
	{
		// Token: 0x06001889 RID: 6281 RVA: 0x00076A58 File Offset: 0x00074C58
		public override Equipment GetEquipmentForHeroComeOfAge(Hero hero, Equipment.EquipmentType equipmentType)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate;
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, equipmentType);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00076A70 File Offset: 0x00074C70
		public override Equipment GetEquipmentForHeroReachesTeenAge(Hero hero)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate | EquipmentCategories.IsTeenagerEquipmentTemplate;
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, Equipment.EquipmentType.Civilian);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00076A8C File Offset: 0x00074C8C
		public override Equipment GetEquipmentForDeliveredOffspring(Hero hero)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate | EquipmentCategories.IsChildEquipmentTemplate;
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, Equipment.EquipmentType.Civilian);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00076AA4 File Offset: 0x00074CA4
		public override Equipment GetEquipmentForCompanionWhenTurningToLord(Hero companionHero, Equipment.EquipmentType equipmentType)
		{
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate;
			return this.GetSuitableEquipmentSet(companionHero, equipmentCategories, equipmentType);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x00076ABC File Offset: 0x00074CBC
		public override Equipment GetEquipmentForInitialChildrenGeneration(Hero hero)
		{
			bool flag = hero.Age < (float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge;
			EquipmentCategories equipmentCategories = EquipmentCategories.IsLordTemplate;
			if (flag)
			{
				equipmentCategories |= EquipmentCategories.IsChildEquipmentTemplate;
			}
			else
			{
				equipmentCategories |= EquipmentCategories.IsTeenagerEquipmentTemplate;
			}
			return this.GetSuitableEquipmentSet(hero, equipmentCategories, Equipment.EquipmentType.Civilian);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00076B00 File Offset: 0x00074D00
		public override ValueTuple<Equipment, Equipment> GetEquipmentsForChangingRuler(Hero newRuler, Hero oldRuler, Equipment.EquipmentType equipmentType)
		{
			Equipment equipment = null;
			Equipment equipment2 = null;
			if (newRuler != Hero.MainHero)
			{
				equipment = this.GetSuitableEquipmentSet(newRuler, EquipmentCategories.IsKingdomRulerTemplate, equipmentType);
			}
			if (oldRuler != null && oldRuler.IsActive && oldRuler != Hero.MainHero)
			{
				equipment2 = this.GetSuitableEquipmentSet(oldRuler, EquipmentCategories.IsLordTemplate, equipmentType);
			}
			return new ValueTuple<Equipment, Equipment>(equipment, equipment2);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x00076B48 File Offset: 0x00074D48
		private bool IsRosterAppropriateForHeroAsTemplate(MBEquipmentRoster equipmentRoster, CultureObject culture, EquipmentCategories customFlags)
		{
			bool flag = false;
			if (equipmentRoster.EquipmentCulture == culture && equipmentRoster.EquipmentCategories == customFlags)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00076B6C File Offset: 0x00074D6C
		private Equipment GetSuitableEquipmentSet(Hero hero, EquipmentCategories customFlags, Equipment.EquipmentType equipmentType)
		{
			MBList<Equipment> mblist = new MBList<Equipment>();
			if (hero.IsFemale)
			{
				customFlags |= EquipmentCategories.IsFemaleTemplate;
			}
			foreach (MBEquipmentRoster mbequipmentRoster in MBEquipmentRosterExtensions.All)
			{
				if (this.IsRosterAppropriateForHeroAsTemplate(mbequipmentRoster, hero.Culture, customFlags))
				{
					foreach (Equipment equipment in mbequipmentRoster.AllEquipments)
					{
						if (equipment.ItemEquipmentType == equipmentType)
						{
							mblist.Add(equipment);
						}
					}
				}
			}
			return mblist.GetRandomElement<Equipment>();
		}
	}
}
