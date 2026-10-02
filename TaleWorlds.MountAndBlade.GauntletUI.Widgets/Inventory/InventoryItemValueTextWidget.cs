using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x02000145 RID: 325
	public class InventoryItemValueTextWidget : TextWidget
	{
		// Token: 0x06001121 RID: 4385 RVA: 0x0002F2E9 File Offset: 0x0002D4E9
		public InventoryItemValueTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x0002F2F4 File Offset: 0x0002D4F4
		private void HandleVisuals()
		{
			if (!this._firstHandled)
			{
				this.RegisterBrushStatesOfWidget();
				this._firstHandled = true;
			}
			switch (this.ProfitType)
			{
			case -2:
				this.SetState("VeryBad");
				return;
			case -1:
				this.SetState("Bad");
				return;
			case 0:
				this.SetState("Default");
				return;
			case 1:
				this.SetState("Good");
				return;
			case 2:
				this.SetState("VeryGood");
				return;
			default:
				return;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x0002F376 File Offset: 0x0002D576
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x0002F37E File Offset: 0x0002D57E
		[Editor(false)]
		public int ProfitType
		{
			get
			{
				return this._profitType;
			}
			set
			{
				if (this._profitType != value)
				{
					this._profitType = value;
					base.OnPropertyChanged(value, "ProfitType");
					this.HandleVisuals();
				}
			}
		}

		// Token: 0x040007C2 RID: 1986
		private bool _firstHandled;

		// Token: 0x040007C3 RID: 1987
		private int _profitType;
	}
}
