using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000F4 RID: 244
	public class EncyclopediaUnitEquipmentSetSelectorItemVM : SelectorItemVM
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06001610 RID: 5648 RVA: 0x00056A01 File Offset: 0x00054C01
		// (set) Token: 0x06001611 RID: 5649 RVA: 0x00056A09 File Offset: 0x00054C09
		public Equipment EquipmentSet { get; private set; }

		// Token: 0x06001612 RID: 5650 RVA: 0x00056A12 File Offset: 0x00054C12
		public EncyclopediaUnitEquipmentSetSelectorItemVM(Equipment equipmentSet, string name = "")
			: base(name)
		{
			this.EquipmentSet = equipmentSet;
			this.LeftEquipmentList = new MBBindingList<CharacterEquipmentItemVM>();
			this.RightEquipmentList = new MBBindingList<CharacterEquipmentItemVM>();
			this.RefreshEquipment();
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x00056A40 File Offset: 0x00054C40
		private void RefreshEquipment()
		{
			this.LeftEquipmentList.Clear();
			this.LeftEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.NumAllWeaponSlots].Item));
			this.LeftEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Cape].Item));
			this.LeftEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Body].Item));
			this.LeftEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Gloves].Item));
			this.LeftEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Leg].Item));
			this.LeftEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.ArmorItemEndSlot].Item));
			this.RightEquipmentList.Clear();
			this.RightEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.WeaponItemBeginSlot].Item));
			this.RightEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Weapon1].Item));
			this.RightEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Weapon2].Item));
			this.RightEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.Weapon3].Item));
			this.RightEquipmentList.Add(new CharacterEquipmentItemVM(this.EquipmentSet[EquipmentIndex.ExtraWeaponSlot].Item));
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x00056BF1 File Offset: 0x00054DF1
		// (set) Token: 0x06001615 RID: 5653 RVA: 0x00056BF9 File Offset: 0x00054DF9
		[DataSourceProperty]
		public MBBindingList<CharacterEquipmentItemVM> LeftEquipmentList
		{
			get
			{
				return this._leftEquipmentList;
			}
			set
			{
				if (value != this._leftEquipmentList)
				{
					this._leftEquipmentList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterEquipmentItemVM>>(value, "LeftEquipmentList");
				}
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x00056C17 File Offset: 0x00054E17
		// (set) Token: 0x06001617 RID: 5655 RVA: 0x00056C1F File Offset: 0x00054E1F
		[DataSourceProperty]
		public MBBindingList<CharacterEquipmentItemVM> RightEquipmentList
		{
			get
			{
				return this._rightEquipmentList;
			}
			set
			{
				if (value != this._rightEquipmentList)
				{
					this._rightEquipmentList = value;
					base.OnPropertyChangedWithValue<MBBindingList<CharacterEquipmentItemVM>>(value, "RightEquipmentList");
				}
			}
		}

		// Token: 0x040009FC RID: 2556
		private MBBindingList<CharacterEquipmentItemVM> _leftEquipmentList;

		// Token: 0x040009FD RID: 2557
		private MBBindingList<CharacterEquipmentItemVM> _rightEquipmentList;
	}
}
