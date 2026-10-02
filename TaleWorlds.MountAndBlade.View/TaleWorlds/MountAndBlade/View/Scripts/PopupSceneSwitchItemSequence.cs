using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000060 RID: 96
	public class PopupSceneSwitchItemSequence : PopupSceneSequence
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x0001B8C5 File Offset: 0x00019AC5
		public override void OnInitialState()
		{
			this.AttachItem(this.InitialItem, this.InitialBodyPart);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001B8D9 File Offset: 0x00019AD9
		public override void OnPositiveState()
		{
			this.AttachItem(this.PositiveItem, this.PositiveBodyPart);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001B8ED File Offset: 0x00019AED
		public override void OnNegativeState()
		{
			this.AttachItem(this.NegativeItem, this.NegativeBodyPart);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001B904 File Offset: 0x00019B04
		private EquipmentIndex StringToEquipmentIndex(PopupSceneSwitchItemSequence.BodyPartIndex part)
		{
			switch (part)
			{
			case PopupSceneSwitchItemSequence.BodyPartIndex.None:
				return EquipmentIndex.None;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon0:
				return EquipmentIndex.WeaponItemBeginSlot;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon1:
				return EquipmentIndex.Weapon1;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon2:
				return EquipmentIndex.Weapon2;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Weapon3:
				return EquipmentIndex.Weapon3;
			case PopupSceneSwitchItemSequence.BodyPartIndex.ExtraWeaponSlot:
				return EquipmentIndex.ExtraWeaponSlot;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Head:
				return EquipmentIndex.NumAllWeaponSlots;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Body:
				return EquipmentIndex.Body;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Leg:
				return EquipmentIndex.Leg;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Gloves:
				return EquipmentIndex.Gloves;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Cape:
				return EquipmentIndex.Cape;
			case PopupSceneSwitchItemSequence.BodyPartIndex.Horse:
				return EquipmentIndex.ArmorItemEndSlot;
			case PopupSceneSwitchItemSequence.BodyPartIndex.HorseHarness:
				return EquipmentIndex.HorseHarness;
			default:
				return EquipmentIndex.None;
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001B96C File Offset: 0x00019B6C
		private void AttachItem(string itemName, PopupSceneSwitchItemSequence.BodyPartIndex bodyPart)
		{
			if (this._agentVisuals == null)
			{
				return;
			}
			EquipmentIndex equipmentIndex = this.StringToEquipmentIndex(bodyPart);
			if (equipmentIndex != EquipmentIndex.None)
			{
				AgentVisualsData copyAgentVisualsData = this._agentVisuals.GetCopyAgentVisualsData();
				Equipment equipment = this._agentVisuals.GetEquipment().Clone(false);
				if (itemName == "")
				{
					equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, default(EquipmentElement));
				}
				else
				{
					equipment.AddEquipmentToSlotWithoutAgent(equipmentIndex, new EquipmentElement(Game.Current.ObjectManager.GetObject<ItemObject>(itemName), null, null, false));
				}
				copyAgentVisualsData.RightWieldedItemIndex(0).LeftWieldedItemIndex(-1).Equipment(equipment);
				this._agentVisuals.Refresh(false, copyAgentVisualsData, false);
			}
		}

		// Token: 0x04000214 RID: 532
		public string InitialItem;

		// Token: 0x04000215 RID: 533
		public string PositiveItem;

		// Token: 0x04000216 RID: 534
		public string NegativeItem;

		// Token: 0x04000217 RID: 535
		public PopupSceneSwitchItemSequence.BodyPartIndex InitialBodyPart;

		// Token: 0x04000218 RID: 536
		public PopupSceneSwitchItemSequence.BodyPartIndex PositiveBodyPart;

		// Token: 0x04000219 RID: 537
		public PopupSceneSwitchItemSequence.BodyPartIndex NegativeBodyPart;

		// Token: 0x020000D6 RID: 214
		public enum BodyPartIndex
		{
			// Token: 0x040003CD RID: 973
			None,
			// Token: 0x040003CE RID: 974
			Weapon0,
			// Token: 0x040003CF RID: 975
			Weapon1,
			// Token: 0x040003D0 RID: 976
			Weapon2,
			// Token: 0x040003D1 RID: 977
			Weapon3,
			// Token: 0x040003D2 RID: 978
			ExtraWeaponSlot,
			// Token: 0x040003D3 RID: 979
			Head,
			// Token: 0x040003D4 RID: 980
			Body,
			// Token: 0x040003D5 RID: 981
			Leg,
			// Token: 0x040003D6 RID: 982
			Gloves,
			// Token: 0x040003D7 RID: 983
			Cape,
			// Token: 0x040003D8 RID: 984
			Horse,
			// Token: 0x040003D9 RID: 985
			HorseHarness
		}
	}
}
