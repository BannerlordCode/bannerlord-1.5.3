using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000113 RID: 275
	public class OverlayBaseWidget : Widget
	{
		// Token: 0x06000EBE RID: 3774 RVA: 0x00028B78 File Offset: 0x00026D78
		public OverlayBaseWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00028B81 File Offset: 0x00026D81
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00028B89 File Offset: 0x00026D89
		[Editor(false)]
		public OverlayPopupWidget PopupWidget
		{
			get
			{
				return this._popupWidget;
			}
			set
			{
				if (this._popupWidget != value)
				{
					this._popupWidget = value;
					base.OnPropertyChanged<OverlayPopupWidget>(value, "PopupWidget");
				}
			}
		}

		// Token: 0x040006B6 RID: 1718
		private OverlayPopupWidget _popupWidget;
	}
}
