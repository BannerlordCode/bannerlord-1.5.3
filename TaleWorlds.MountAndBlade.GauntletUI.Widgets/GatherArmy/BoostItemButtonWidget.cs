using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GatherArmy
{
	// Token: 0x02000151 RID: 337
	public class BoostItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x000324DE File Offset: 0x000306DE
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x000324E6 File Offset: 0x000306E6
		public BoostCohesionPopupWidget ParentPopupWidget { get; private set; }

		// Token: 0x06001204 RID: 4612 RVA: 0x000324EF File Offset: 0x000306EF
		public BoostItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00032500 File Offset: 0x00030700
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.BoostCurrencyIconWidget != null)
			{
				int boostCurrencyType = this.BoostCurrencyType;
				if (boostCurrencyType != 0)
				{
					if (boostCurrencyType == 1)
					{
						this.BoostCurrencyIconWidget.SetState("Influence");
					}
				}
				else
				{
					this.BoostCurrencyIconWidget.SetState("Gold");
				}
			}
			if (this.ParentPopupWidget == null)
			{
				this.ParentPopupWidget = this.FindParentPopupWidget();
				if (this.ParentPopupWidget != null)
				{
					this.ClickEventHandlers.Add(new Action<Widget>(this.ParentPopupWidget.ClosePopup));
				}
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00032588 File Offset: 0x00030788
		private BoostCohesionPopupWidget FindParentPopupWidget()
		{
			Widget widget = this;
			while (widget != base.EventManager.Root && this.ParentPopupWidget == null)
			{
				if (widget is BoostCohesionPopupWidget)
				{
					return widget as BoostCohesionPopupWidget;
				}
				widget = widget.ParentWidget;
			}
			return null;
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06001207 RID: 4615 RVA: 0x000325C6 File Offset: 0x000307C6
		// (set) Token: 0x06001208 RID: 4616 RVA: 0x000325CE File Offset: 0x000307CE
		[Editor(false)]
		public int BoostCurrencyType
		{
			get
			{
				return this._boostCurrencyType;
			}
			set
			{
				if (this._boostCurrencyType != value)
				{
					this._boostCurrencyType = value;
					base.OnPropertyChanged(value, "BoostCurrencyType");
				}
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x000325EC File Offset: 0x000307EC
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x000325F4 File Offset: 0x000307F4
		[Editor(false)]
		public Widget BoostCurrencyIconWidget
		{
			get
			{
				return this._boostCurrencyIconWidget;
			}
			set
			{
				if (this._boostCurrencyIconWidget != value)
				{
					this._boostCurrencyIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "BoostCurrencyIconWidget");
				}
			}
		}

		// Token: 0x04000843 RID: 2115
		private int _boostCurrencyType = -1;

		// Token: 0x04000844 RID: 2116
		private Widget _boostCurrencyIconWidget;
	}
}
