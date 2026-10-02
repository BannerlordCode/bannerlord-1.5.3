using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x02000192 RID: 402
	public class BarterItemCountTextWidget : TextWidget
	{
		// Token: 0x060014DE RID: 5342 RVA: 0x00038E87 File Offset: 0x00037087
		public BarterItemCountTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x00038E90 File Offset: 0x00037090
		// (set) Token: 0x060014E0 RID: 5344 RVA: 0x00038E98 File Offset: 0x00037098
		[Editor(false)]
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				if (this._count != value)
				{
					this._count = value;
					base.OnPropertyChanged(value, "Count");
					base.IntText = value;
					base.IsVisible = value > 1;
				}
			}
		}

		// Token: 0x04000982 RID: 2434
		private int _count;
	}
}
