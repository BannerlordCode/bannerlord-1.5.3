using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GatherArmy
{
	// Token: 0x02000150 RID: 336
	public class BoostCohesionPopupWidget : Widget
	{
		// Token: 0x060011FD RID: 4605 RVA: 0x00032449 File Offset: 0x00030649
		public BoostCohesionPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00032454 File Offset: 0x00030654
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.ClosePopupButton != null && !this.ClosePopupButton.ClickEventHandlers.Contains(new Action<Widget>(this.ClosePopup)))
			{
				this.ClosePopupButton.ClickEventHandlers.Add(new Action<Widget>(this.ClosePopup));
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x000324AA File Offset: 0x000306AA
		public void ClosePopup(Widget widget)
		{
			base.ParentWidget.IsVisible = false;
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001200 RID: 4608 RVA: 0x000324B8 File Offset: 0x000306B8
		// (set) Token: 0x06001201 RID: 4609 RVA: 0x000324C0 File Offset: 0x000306C0
		[Editor(false)]
		public ButtonWidget ClosePopupButton
		{
			get
			{
				return this._closePopupButton;
			}
			set
			{
				if (this._closePopupButton != value)
				{
					this._closePopupButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "ClosePopupButton");
				}
			}
		}

		// Token: 0x04000841 RID: 2113
		private ButtonWidget _closePopupButton;
	}
}
