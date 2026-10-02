using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000143 RID: 323
	public class InventoryItemPreviewWidget : Widget
	{
		// Token: 0x060010DF RID: 4319 RVA: 0x0002E910 File Offset: 0x0002CB10
		public InventoryItemPreviewWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0002E919 File Offset: 0x0002CB19
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x0002E921 File Offset: 0x0002CB21
		[Editor(false)]
		public bool IsPreviewOpen
		{
			get
			{
				return this._isPreviewOpen;
			}
			set
			{
				if (this._isPreviewOpen != value)
				{
					this._isPreviewOpen = value;
					base.IsVisible = value;
					base.OnPropertyChanged(value, "IsPreviewOpen");
				}
			}
		}

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0002E946 File Offset: 0x0002CB46
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0002E94E File Offset: 0x0002CB4E
		[Editor(false)]
		public ItemTableauWidget ItemTableau
		{
			get
			{
				return this._itemTableau;
			}
			set
			{
				if (this._itemTableau != value)
				{
					this._itemTableau = value;
					base.OnPropertyChanged<ItemTableauWidget>(value, "ItemTableau");
				}
			}
		}

		// Token: 0x040007A5 RID: 1957
		private ItemTableauWidget _itemTableau;

		// Token: 0x040007A6 RID: 1958
		private bool _isPreviewOpen;
	}
}
