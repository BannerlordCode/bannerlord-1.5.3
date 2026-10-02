using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticCategory
{
	// Token: 0x02000082 RID: 130
	public class MPArmoryClothingCosmeticCategoryVM : MPArmoryCosmeticCategoryBaseVM
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000D18 RID: 3352 RVA: 0x000286A4 File Offset: 0x000268A4
		// (remove) Token: 0x06000D19 RID: 3353 RVA: 0x000286D8 File Offset: 0x000268D8
		public static event Action<MPArmoryClothingCosmeticCategoryVM> OnSelected;

		// Token: 0x06000D1A RID: 3354 RVA: 0x0002870B File Offset: 0x0002690B
		public MPArmoryClothingCosmeticCategoryVM(MPArmoryCosmeticsVM.ClothingCategory clothingCategory)
			: base(CosmeticsManager.CosmeticType.Clothing)
		{
			this._defaultCosmeticIDs = new List<string>();
			this.ClothingCategory = clothingCategory;
			base.CosmeticCategoryName = clothingCategory.ToString();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00028739 File Offset: 0x00026939
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.AvailableCosmetics.ApplyActionOnAllItems(delegate(MPArmoryCosmeticItemBaseVM c)
			{
				c.RefreshValues();
			});
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0002876B File Offset: 0x0002696B
		protected override void ExecuteSelectCategory()
		{
			Action<MPArmoryClothingCosmeticCategoryVM> onSelected = MPArmoryClothingCosmeticCategoryVM.OnSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00028780 File Offset: 0x00026980
		private void AddDefaultItem(ItemObject item)
		{
			MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM = new MPArmoryCosmeticClothingItemVM(new ClothingCosmeticElement(item.StringId, CosmeticsManager.CosmeticRarity.Default, 0, new List<string>(), new List<Tuple<string, string>>()), string.Empty)
			{
				IsUnlocked = true,
				IsUnequippable = false
			};
			ItemObject.ItemTypeEnum itemTypeEnum = this.ClothingCategory.ToItemTypeEnum();
			if (itemTypeEnum == ItemObject.ItemTypeEnum.Invalid || itemTypeEnum == item.ItemType)
			{
				base.AvailableCosmetics.Add(mparmoryCosmeticClothingItemVM);
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x000287E4 File Offset: 0x000269E4
		public void SetDefaultEquipments(Equipment equipment)
		{
			base.AvailableCosmetics.Clear();
			this._defaultCosmeticIDs.Clear();
			if (equipment != null)
			{
				for (EquipmentIndex equipmentIndex = EquipmentIndex.NumAllWeaponSlots; equipmentIndex < EquipmentIndex.ArmorItemEndSlot; equipmentIndex++)
				{
					ItemObject item = equipment[equipmentIndex].Item;
					if (item != null)
					{
						this._defaultCosmeticIDs.Add(equipment[equipmentIndex].Item.StringId);
						this.AddDefaultItem(item);
					}
				}
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00028850 File Offset: 0x00026A50
		public void ReplaceCosmeticWithDefaultItem(MPArmoryCosmeticClothingItemVM cosmetic, MPArmoryCosmeticsVM.ClothingCategory clothingCategory, MultiplayerClassDivisions.MPHeroClass selectedClass, List<string> ownedCosmetics)
		{
			bool flag = cosmetic.ClothingCategory == clothingCategory || clothingCategory == MPArmoryCosmeticsVM.ClothingCategory.ClothingCategoriesBegin;
			ClothingCosmeticElement clothingCosmeticElement;
			bool flag2 = (clothingCosmeticElement = cosmetic.Cosmetic as ClothingCosmeticElement) != null && (clothingCosmeticElement.ReplaceItemsId.Any<string>((string c) => this._defaultCosmeticIDs.Contains(c)) || clothingCosmeticElement.ReplaceItemless.Any<Tuple<string, string>>((Tuple<string, string> r) => r.Item1 == selectedClass.StringId)) && !base.AvailableCosmetics.Contains(cosmetic);
			if (flag && flag2)
			{
				base.AvailableCosmetics.Add(cosmetic);
				cosmetic.IsUnlocked = (ownedCosmetics != null && ownedCosmetics.Contains(cosmetic.CosmeticID)) || cosmetic.Cosmetic.IsFree;
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0002890C File Offset: 0x00026B0C
		public void OnEquipmentRefreshed(EquipmentIndex equipmentIndex)
		{
			foreach (MPArmoryCosmeticItemBaseVM mparmoryCosmeticItemBaseVM in base.AvailableCosmetics)
			{
				MPArmoryCosmeticClothingItemVM mparmoryCosmeticClothingItemVM;
				if ((mparmoryCosmeticClothingItemVM = mparmoryCosmeticItemBaseVM as MPArmoryCosmeticClothingItemVM) != null && mparmoryCosmeticClothingItemVM.EquipmentElement.Item.GetCosmeticEquipmentIndex() == equipmentIndex)
				{
					mparmoryCosmeticItemBaseVM.IsUsed = false;
				}
			}
		}

		// Token: 0x040005ED RID: 1517
		public readonly MPArmoryCosmeticsVM.ClothingCategory ClothingCategory;

		// Token: 0x040005EE RID: 1518
		private List<string> _defaultCosmeticIDs;
	}
}
