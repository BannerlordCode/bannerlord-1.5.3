using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.Inventory
{
	// Token: 0x020000DF RID: 223
	public class TransferCommandResult
	{
		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00064198 File Offset: 0x00062398
		public Equipment ResultSideEquipment
		{
			get
			{
				switch (this.ResultSide)
				{
				case InventoryLogic.InventorySide.CivilianEquipment:
				{
					CharacterObject transferCharacter = this.TransferCharacter;
					if (transferCharacter == null)
					{
						return null;
					}
					return transferCharacter.FirstCivilianEquipment;
				}
				case InventoryLogic.InventorySide.BattleEquipment:
				{
					CharacterObject transferCharacter2 = this.TransferCharacter;
					if (transferCharacter2 == null)
					{
						return null;
					}
					return transferCharacter2.FirstBattleEquipment;
				}
				case InventoryLogic.InventorySide.StealthEquipment:
				{
					CharacterObject transferCharacter3 = this.TransferCharacter;
					if (transferCharacter3 == null)
					{
						return null;
					}
					return transferCharacter3.FirstStealthEquipment;
				}
				default:
					return null;
				}
			}
		}

		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x000641F9 File Offset: 0x000623F9
		// (set) Token: 0x06001574 RID: 5492 RVA: 0x00064201 File Offset: 0x00062401
		public CharacterObject TransferCharacter { get; private set; }

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x0006420A File Offset: 0x0006240A
		// (set) Token: 0x06001576 RID: 5494 RVA: 0x00064212 File Offset: 0x00062412
		public InventoryLogic.InventorySide ResultSide { get; private set; }

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001577 RID: 5495 RVA: 0x0006421B File Offset: 0x0006241B
		// (set) Token: 0x06001578 RID: 5496 RVA: 0x00064223 File Offset: 0x00062423
		public ItemRosterElement EffectedItemRosterElement { get; private set; }

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001579 RID: 5497 RVA: 0x0006422C File Offset: 0x0006242C
		// (set) Token: 0x0600157A RID: 5498 RVA: 0x00064234 File Offset: 0x00062434
		public int EffectedNumber { get; private set; }

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0006423D File Offset: 0x0006243D
		// (set) Token: 0x0600157C RID: 5500 RVA: 0x00064245 File Offset: 0x00062445
		public int FinalNumber { get; private set; }

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600157D RID: 5501 RVA: 0x0006424E File Offset: 0x0006244E
		// (set) Token: 0x0600157E RID: 5502 RVA: 0x00064256 File Offset: 0x00062456
		public EquipmentIndex EffectedEquipmentIndex { get; private set; }

		// Token: 0x0600157F RID: 5503 RVA: 0x0006425F File Offset: 0x0006245F
		public TransferCommandResult()
		{
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00064267 File Offset: 0x00062467
		public TransferCommandResult(InventoryLogic.InventorySide resultSide, ItemRosterElement effectedItemRosterElement, int effectedNumber, int finalNumber, EquipmentIndex effectedEquipmentIndex, CharacterObject transferCharacter)
		{
			this.ResultSide = resultSide;
			this.EffectedItemRosterElement = effectedItemRosterElement;
			this.EffectedNumber = effectedNumber;
			this.FinalNumber = finalNumber;
			this.EffectedEquipmentIndex = effectedEquipmentIndex;
			this.TransferCharacter = transferCharacter;
		}
	}
}
