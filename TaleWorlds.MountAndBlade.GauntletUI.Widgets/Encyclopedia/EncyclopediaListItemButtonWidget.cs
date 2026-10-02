using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x0200015E RID: 350
	public class EncyclopediaListItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060012A2 RID: 4770 RVA: 0x000338E6 File Offset: 0x00031AE6
		// (set) Token: 0x060012A3 RID: 4771 RVA: 0x000338EE File Offset: 0x00031AEE
		public TextWidget ListItemNameTextWidget { get; set; }

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060012A4 RID: 4772 RVA: 0x000338F7 File Offset: 0x00031AF7
		// (set) Token: 0x060012A5 RID: 4773 RVA: 0x000338FF File Offset: 0x00031AFF
		public TextWidget ListComparedValueTextWidget { get; set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060012A6 RID: 4774 RVA: 0x00033908 File Offset: 0x00031B08
		// (set) Token: 0x060012A7 RID: 4775 RVA: 0x00033910 File Offset: 0x00031B10
		public Brush InfoAvailableItemNameBrush { get; set; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060012A8 RID: 4776 RVA: 0x00033919 File Offset: 0x00031B19
		// (set) Token: 0x060012A9 RID: 4777 RVA: 0x00033921 File Offset: 0x00031B21
		public Brush InfoUnvailableItemNameBrush { get; set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060012AA RID: 4778 RVA: 0x0003392A File Offset: 0x00031B2A
		// (set) Token: 0x060012AB RID: 4779 RVA: 0x00033932 File Offset: 0x00031B32
		public bool IsInfoAvailable { get; set; }

		// Token: 0x060012AC RID: 4780 RVA: 0x0003393B File Offset: 0x00031B3B
		public EncyclopediaListItemButtonWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.OnThisLateUpdate), 1);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00033960 File Offset: 0x00031B60
		public void OnThisLateUpdate(float dt)
		{
			this.ListItemNameTextWidget.Brush = (this.IsInfoAvailable ? this.InfoAvailableItemNameBrush : this.InfoUnvailableItemNameBrush);
			this.ListComparedValueTextWidget.Brush = (this.IsInfoAvailable ? this.InfoAvailableItemNameBrush : this.InfoUnvailableItemNameBrush);
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x000339AF File Offset: 0x00031BAF
		// (set) Token: 0x060012AF RID: 4783 RVA: 0x000339B7 File Offset: 0x00031BB7
		[Editor(false)]
		public string ListItemId
		{
			get
			{
				return this._listItemId;
			}
			set
			{
				if (this._listItemId != value)
				{
					this._listItemId = value;
					base.OnPropertyChanged<string>(value, "ListItemId");
				}
			}
		}

		// Token: 0x04000885 RID: 2181
		private string _listItemId;
	}
}
