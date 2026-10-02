using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007A RID: 122
	public static class LobbyCosmeticExtensions
	{
		// Token: 0x06000C5B RID: 3163 RVA: 0x000262C7 File Offset: 0x000244C7
		public static ItemObject.ItemTypeEnum ToItemTypeEnum(this MPArmoryCosmeticsVM.ClothingCategory cosmeticCategory)
		{
			switch (cosmeticCategory)
			{
			case MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin:
				return ItemObject.ItemTypeEnum.Invalid;
			case MPArmoryCosmeticsVM.ClothingCategory.HeadArmor:
				return ItemObject.ItemTypeEnum.HeadArmor;
			case MPArmoryCosmeticsVM.ClothingCategory.Cape:
				return ItemObject.ItemTypeEnum.Cape;
			case MPArmoryCosmeticsVM.ClothingCategory.BodyArmor:
				return ItemObject.ItemTypeEnum.BodyArmor;
			case MPArmoryCosmeticsVM.ClothingCategory.HandArmor:
				return ItemObject.ItemTypeEnum.HandArmor;
			case MPArmoryCosmeticsVM.ClothingCategory.LegArmor:
				return ItemObject.ItemTypeEnum.LegArmor;
			default:
				return ItemObject.ItemTypeEnum.Invalid;
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000262FC File Offset: 0x000244FC
		public static EquipmentIndex GetCosmeticEquipmentIndex(this ItemObject itemObject)
		{
			if (itemObject == null)
			{
				return EquipmentIndex.None;
			}
			ItemObject.ItemTypeEnum type = itemObject.Type;
			if (type <= ItemObject.ItemTypeEnum.HandArmor)
			{
				if (type == ItemObject.ItemTypeEnum.Horse)
				{
					return EquipmentIndex.ArmorItemEndSlot;
				}
				switch (type)
				{
				case ItemObject.ItemTypeEnum.HeadArmor:
					return EquipmentIndex.NumAllWeaponSlots;
				case ItemObject.ItemTypeEnum.BodyArmor:
					return EquipmentIndex.Body;
				case ItemObject.ItemTypeEnum.LegArmor:
					return EquipmentIndex.Leg;
				case ItemObject.ItemTypeEnum.HandArmor:
					return EquipmentIndex.Gloves;
				}
			}
			else
			{
				if (type == ItemObject.ItemTypeEnum.Cape)
				{
					return EquipmentIndex.Cape;
				}
				if (type == ItemObject.ItemTypeEnum.HorseHarness)
				{
					return EquipmentIndex.HorseHarness;
				}
			}
			return EquipmentIndex.None;
		}
	}
}
