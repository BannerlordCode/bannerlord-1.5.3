using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics;
using TaleWorlds.MountAndBlade.Diamond.Cosmetics.CosmeticTypes;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory.CosmeticItem
{
	// Token: 0x0200007E RID: 126
	public class MPArmoryCosmeticClothingItemVM : MPArmoryCosmeticItemBaseVM
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x00027614 File Offset: 0x00025814
		public EquipmentElement EquipmentElement { get; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0002761C File Offset: 0x0002581C
		public MPArmoryCosmeticsVM.ClothingCategory ClothingCategory { get; }

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x00027624 File Offset: 0x00025824
		public ClothingCosmeticElement ClothingCosmeticElement { get; }

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002762C File Offset: 0x0002582C
		public MPArmoryCosmeticClothingItemVM(CosmeticElement cosmetic, string cosmeticID)
			: base(cosmetic, cosmeticID, CosmeticsManager.CosmeticType.Clothing)
		{
			ItemObject @object = MBObjectManager.Instance.GetObject<ItemObject>(cosmetic.Id);
			this.EquipmentElement = new EquipmentElement(@object, null, null, false);
			base.Icon = new ItemImageIdentifierVM(@object, "");
			this.ClothingCategory = this.GetCosmeticCategory();
			this.ClothingCosmeticElement = cosmetic as ClothingCosmeticElement;
			base.ItemType = (int)this.EquipmentElement.Item.ItemType;
			this.RefreshValues();
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x000276AC File Offset: 0x000258AC
		public override void RefreshValues()
		{
			base.RefreshValues();
			ItemObject item = this.EquipmentElement.Item;
			base.Name = ((item != null) ? item.Name.ToString() : null);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x000276E4 File Offset: 0x000258E4
		private MPArmoryCosmeticsVM.ClothingCategory GetCosmeticCategory()
		{
			ItemObject.ItemTypeEnum type = this.EquipmentElement.Item.Type;
			switch (type)
			{
			case ItemObject.ItemTypeEnum.HeadArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.HeadArmor;
			case ItemObject.ItemTypeEnum.BodyArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.BodyArmor;
			case ItemObject.ItemTypeEnum.LegArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.LegArmor;
			case ItemObject.ItemTypeEnum.HandArmor:
				return MPArmoryCosmeticsVM.ClothingCategory.HandArmor;
			default:
				if (type != ItemObject.ItemTypeEnum.Cape)
				{
					return MPArmoryCosmeticsVM.ClothingCategory.Invalid;
				}
				return MPArmoryCosmeticsVM.ClothingCategory.Cape;
			}
		}
	}
}
