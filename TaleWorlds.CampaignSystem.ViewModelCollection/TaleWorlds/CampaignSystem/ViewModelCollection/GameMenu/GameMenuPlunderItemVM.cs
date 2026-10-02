using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu
{
	// Token: 0x020000A3 RID: 163
	public class GameMenuPlunderItemVM : ViewModel
	{
		// Token: 0x06000F6C RID: 3948 RVA: 0x0003FF77 File Offset: 0x0003E177
		public GameMenuPlunderItemVM(EquipmentElement item, int amount = 1)
		{
			this.Item = item;
			this.Amount = amount;
			this.Visual = new ItemImageIdentifierVM(item.Item, "");
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x0003FFA4 File Offset: 0x0003E1A4
		public void ExecuteBeginTooltip()
		{
			if (this.Item.Item != null)
			{
				InformationManager.ShowTooltip(typeof(ItemObject), new object[] { this.Item });
			}
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0003FFE4 File Offset: 0x0003E1E4
		public void ExecuteEndTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0003FFEB File Offset: 0x0003E1EB
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x0003FFF3 File Offset: 0x0003E1F3
		[DataSourceProperty]
		public ItemImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x00040011 File Offset: 0x0003E211
		// (set) Token: 0x06000F72 RID: 3954 RVA: 0x00040019 File Offset: 0x0003E219
		[DataSourceProperty]
		public int Amount
		{
			get
			{
				return this._amount;
			}
			set
			{
				if (value != this._amount)
				{
					this._amount = value;
					base.OnPropertyChangedWithValue(value, "Amount");
				}
			}
		}

		// Token: 0x040006F3 RID: 1779
		public readonly EquipmentElement Item;

		// Token: 0x040006F4 RID: 1780
		private ItemImageIdentifierVM _visual;

		// Token: 0x040006F5 RID: 1781
		private int _amount;
	}
}
