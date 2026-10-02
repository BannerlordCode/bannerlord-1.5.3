using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010F RID: 271
	public class SliderPopupWidget : Widget
	{
		// Token: 0x06000E76 RID: 3702 RVA: 0x0002809A File Offset: 0x0002629A
		public SliderPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x000280A4 File Offset: 0x000262A4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.SliderValueTextWidget != null && this.ReserveAmountSlider != null)
			{
				this.SliderValueTextWidget.Text = this.ReserveAmountSlider.ValueInt.ToString();
			}
			if (base.ParentWidget.IsVisible && base.EventManager.LatestMouseDownWidget != this && base.EventManager.LatestMouseDownWidget != base.ParentWidget && base.EventManager.LatestMouseDownWidget != this.PopupParentWidget && !base.CheckIsMyChildRecursive(base.EventManager.LatestMouseDownWidget))
			{
				base.EventFired("ClosePopup", Array.Empty<object>());
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0002814A File Offset: 0x0002634A
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x00028152 File Offset: 0x00026352
		[Editor(false)]
		public Widget PopupParentWidget
		{
			get
			{
				return this._popupParentWidget;
			}
			set
			{
				if (this._popupParentWidget != value)
				{
					this._popupParentWidget = value;
					base.OnPropertyChanged<Widget>(value, "PopupParentWidget");
				}
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x00028170 File Offset: 0x00026370
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x00028178 File Offset: 0x00026378
		[Editor(false)]
		public ButtonWidget ClosePopupWidget
		{
			get
			{
				return this._closePopupWidget;
			}
			set
			{
				if (this._closePopupWidget != value)
				{
					this._closePopupWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ClosePopupWidget");
				}
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00028196 File Offset: 0x00026396
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x0002819E File Offset: 0x0002639E
		[Editor(false)]
		public TextWidget SliderValueTextWidget
		{
			get
			{
				return this._sliderValueTextWidget;
			}
			set
			{
				if (this._sliderValueTextWidget != value)
				{
					this._sliderValueTextWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "SliderValueTextWidget");
				}
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x000281BC File Offset: 0x000263BC
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x000281C4 File Offset: 0x000263C4
		[Editor(false)]
		public SliderWidget ReserveAmountSlider
		{
			get
			{
				return this._reserveAmountSlider;
			}
			set
			{
				if (this._reserveAmountSlider != value)
				{
					this._reserveAmountSlider = value;
					base.OnPropertyChanged<SliderWidget>(value, "ReserveAmountSlider");
				}
			}
		}

		// Token: 0x04000694 RID: 1684
		private ButtonWidget _closePopupWidget;

		// Token: 0x04000695 RID: 1685
		private TextWidget _sliderValueTextWidget;

		// Token: 0x04000696 RID: 1686
		private SliderWidget _reserveAmountSlider;

		// Token: 0x04000697 RID: 1687
		private Widget _popupParentWidget;
	}
}
