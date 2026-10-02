using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000EF RID: 239
	public class EncyclopediaShipSlotVM : ViewModel
	{
		// Token: 0x060015E0 RID: 5600 RVA: 0x000563FA File Offset: 0x000545FA
		public EncyclopediaShipSlotVM(string slotId, bool isAvailable)
		{
			this.SlotTypeId = slotId;
			this.IsAvailable = isAvailable;
			this.RefreshValues();
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x00056416 File Offset: 0x00054616
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = GameTexts.FindText("str_ship_slot_type", this.SlotTypeId).ToString();
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x00056439 File Offset: 0x00054639
		// (set) Token: 0x060015E3 RID: 5603 RVA: 0x00056441 File Offset: 0x00054641
		[DataSourceProperty]
		public string SlotTypeId
		{
			get
			{
				return this._slotTypeId;
			}
			set
			{
				if (value != this._slotTypeId)
				{
					this._slotTypeId = value;
					base.OnPropertyChangedWithValue<string>(value, "SlotTypeId");
				}
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x00056464 File Offset: 0x00054664
		// (set) Token: 0x060015E5 RID: 5605 RVA: 0x0005646C File Offset: 0x0005466C
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060015E6 RID: 5606 RVA: 0x0005648F File Offset: 0x0005468F
		// (set) Token: 0x060015E7 RID: 5607 RVA: 0x00056497 File Offset: 0x00054697
		[DataSourceProperty]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAvailable");
				}
			}
		}

		// Token: 0x040009E6 RID: 2534
		private string _slotTypeId;

		// Token: 0x040009E7 RID: 2535
		private string _name;

		// Token: 0x040009E8 RID: 2536
		private bool _isAvailable;
	}
}
