using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000E0 RID: 224
	public struct TransferCommand
	{
		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x0006429C File Offset: 0x0006249C
		public Equipment FromSideEquipment
		{
			get
			{
				switch (this.FromSide)
				{
				case InventoryLogic.InventorySide.CivilianEquipment:
				{
					CharacterObject character = this.Character;
					if (character == null)
					{
						return null;
					}
					return character.FirstCivilianEquipment;
				}
				case InventoryLogic.InventorySide.BattleEquipment:
				{
					CharacterObject character2 = this.Character;
					if (character2 == null)
					{
						return null;
					}
					return character2.FirstBattleEquipment;
				}
				case InventoryLogic.InventorySide.StealthEquipment:
				{
					CharacterObject character3 = this.Character;
					if (character3 == null)
					{
						return null;
					}
					return character3.FirstStealthEquipment;
				}
				default:
					return null;
				}
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00064300 File Offset: 0x00062500
		public Equipment ToSideEquipment
		{
			get
			{
				switch (this.ToSide)
				{
				case InventoryLogic.InventorySide.CivilianEquipment:
				{
					CharacterObject character = this.Character;
					if (character == null)
					{
						return null;
					}
					return character.FirstCivilianEquipment;
				}
				case InventoryLogic.InventorySide.BattleEquipment:
				{
					CharacterObject character2 = this.Character;
					if (character2 == null)
					{
						return null;
					}
					return character2.FirstBattleEquipment;
				}
				case InventoryLogic.InventorySide.StealthEquipment:
				{
					CharacterObject character3 = this.Character;
					if (character3 == null)
					{
						return null;
					}
					return character3.FirstStealthEquipment;
				}
				default:
					return null;
				}
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x00064361 File Offset: 0x00062561
		// (set) Token: 0x06001584 RID: 5508 RVA: 0x00064369 File Offset: 0x00062569
		public InventoryLogic.InventorySide FromSide { get; private set; }

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00064372 File Offset: 0x00062572
		// (set) Token: 0x06001586 RID: 5510 RVA: 0x0006437A File Offset: 0x0006257A
		public InventoryLogic.InventorySide ToSide { get; private set; }

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001587 RID: 5511 RVA: 0x00064383 File Offset: 0x00062583
		// (set) Token: 0x06001588 RID: 5512 RVA: 0x0006438B File Offset: 0x0006258B
		public EquipmentIndex FromEquipmentIndex { get; private set; }

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001589 RID: 5513 RVA: 0x00064394 File Offset: 0x00062594
		// (set) Token: 0x0600158A RID: 5514 RVA: 0x0006439C File Offset: 0x0006259C
		public EquipmentIndex ToEquipmentIndex { get; private set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x000643A5 File Offset: 0x000625A5
		// (set) Token: 0x0600158C RID: 5516 RVA: 0x000643AD File Offset: 0x000625AD
		public int Amount { get; private set; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600158D RID: 5517 RVA: 0x000643B6 File Offset: 0x000625B6
		// (set) Token: 0x0600158E RID: 5518 RVA: 0x000643BE File Offset: 0x000625BE
		public ItemRosterElement ElementToTransfer { get; private set; }

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600158F RID: 5519 RVA: 0x000643C7 File Offset: 0x000625C7
		// (set) Token: 0x06001590 RID: 5520 RVA: 0x000643CF File Offset: 0x000625CF
		public CharacterObject Character { get; private set; }

		// Token: 0x06001591 RID: 5521 RVA: 0x000643D8 File Offset: 0x000625D8
		public static TransferCommand Transfer(int amount, InventoryLogic.InventorySide fromSide, InventoryLogic.InventorySide toSide, ItemRosterElement elementToTransfer, EquipmentIndex fromEquipmentIndex, EquipmentIndex toEquipmentIndex, CharacterObject character)
		{
			return new TransferCommand
			{
				FromSide = fromSide,
				ToSide = toSide,
				ElementToTransfer = elementToTransfer,
				FromEquipmentIndex = fromEquipmentIndex,
				ToEquipmentIndex = toEquipmentIndex,
				Character = character,
				Amount = amount
			};
		}
	}
}
