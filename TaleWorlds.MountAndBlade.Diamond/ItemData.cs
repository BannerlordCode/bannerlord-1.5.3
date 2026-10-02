using System;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000124 RID: 292
	public class ItemData
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00008504 File Offset: 0x00006704
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x0000850C File Offset: 0x0000670C
		public string TypeId { get; set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x00008515 File Offset: 0x00006715
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0000851D File Offset: 0x0000671D
		public string ModifierId { get; set; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x00008526 File Offset: 0x00006726
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x0000852E File Offset: 0x0000672E
		public int? Index { get; set; }

		// Token: 0x06000698 RID: 1688 RVA: 0x00008537 File Offset: 0x00006737
		public void CopyItemData(ItemData itemdata)
		{
			this.TypeId = itemdata.TypeId;
			this.ModifierId = itemdata.ModifierId;
			this.Index = itemdata.Index;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0000855D File Offset: 0x0000675D
		private ItemType ItemType
		{
			get
			{
				return ItemList.GetItemTypeOf(this.TypeId);
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0000856C File Offset: 0x0000676C
		private static int GetInventoryItemTypeOfItem(ItemType itemType)
		{
			switch (itemType)
			{
			case ItemType.Horse:
				return 64;
			case ItemType.OneHandedWeapon:
				return 1;
			case ItemType.TwoHandedWeapon:
				return 1;
			case ItemType.Polearm:
				return 1;
			case ItemType.Arrows:
				return 1;
			case ItemType.Bolts:
				return 1;
			case ItemType.Shield:
				return 2;
			case ItemType.Bow:
				return 1;
			case ItemType.Crossbow:
				return 1;
			case ItemType.Thrown:
				return 1;
			case ItemType.Goods:
				return 256;
			case ItemType.HeadArmor:
				return 4;
			case ItemType.BodyArmor:
				return 8;
			case ItemType.LegArmor:
				return 16;
			case ItemType.HandArmor:
				return 32;
			case ItemType.Pistol:
				return 1;
			case ItemType.Musket:
				return 1;
			case ItemType.Bullets:
				return 1;
			case ItemType.Animal:
				return 1024;
			case ItemType.Book:
				return 512;
			case ItemType.Cape:
				return 2048;
			case ItemType.HorseHarness:
				return 128;
			}
			return 0;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00008623 File Offset: 0x00006823
		public bool CanItemToEquipmentDragPossible(int equipmentIndex)
		{
			return ItemData.CanItemToEquipmentDragPossible(this.TypeId, equipmentIndex);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00008634 File Offset: 0x00006834
		public static bool CanItemToEquipmentDragPossible(string itemTypeId, int equipmentIndex)
		{
			InventoryItemType inventoryItemTypeOfItem = (InventoryItemType)ItemData.GetInventoryItemTypeOfItem(ItemList.GetItemTypeOf(itemTypeId));
			bool flag = false;
			if (equipmentIndex == 0 || equipmentIndex == 1 || equipmentIndex == 2 || equipmentIndex == 3)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.Weapon || inventoryItemTypeOfItem == InventoryItemType.Shield;
			}
			else if (equipmentIndex == 5)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.HeadArmor;
			}
			else if (equipmentIndex == 6)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.BodyArmor;
			}
			else if (equipmentIndex == 7)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.LegArmor;
			}
			else if (equipmentIndex == 8)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.HandArmor;
			}
			else if (equipmentIndex == 9)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.Cape;
			}
			else if (equipmentIndex == 10)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.Horse;
			}
			else if (equipmentIndex == 11)
			{
				flag = inventoryItemTypeOfItem == InventoryItemType.HorseHarness;
			}
			return flag;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x000086C8 File Offset: 0x000068C8
		public int Price
		{
			get
			{
				return ItemData.GetPriceOf(this.TypeId, this.ModifierId);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x000086DB File Offset: 0x000068DB
		public bool IsValid
		{
			get
			{
				return ItemData.IsItemValid(this.TypeId, this.ModifierId);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000086EE File Offset: 0x000068EE
		public string ItemKey
		{
			get
			{
				return this.TypeId + "|" + this.ModifierId;
			}
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00008706 File Offset: 0x00006906
		public static int GetPriceOf(string itemId, string modifierId)
		{
			return ItemList.GetPriceOf(itemId, modifierId);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0000870F File Offset: 0x0000690F
		public static bool IsItemValid(string itemId, string modifierId)
		{
			return ItemList.IsItemValid(itemId, modifierId);
		}
	}
}
