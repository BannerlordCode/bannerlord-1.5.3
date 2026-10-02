using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016E RID: 366
	public class CraftingScreenWidget : Widget
	{
		// Token: 0x0600135F RID: 4959 RVA: 0x00035061 File Offset: 0x00033261
		public CraftingScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0003506C File Offset: 0x0003326C
		private void OnMainAction(Widget widget)
		{
			if (this.IsInCraftingMode)
			{
				base.Context.TwoDimensionContext.PlaySound("crafting/craft_success");
				return;
			}
			if (this.IsInRefinementMode)
			{
				base.Context.TwoDimensionContext.PlaySound("crafting/refine_success");
				return;
			}
			if (this.IsInSmeltingMode)
			{
				base.Context.TwoDimensionContext.PlaySound("crafting/smelt_success");
			}
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x000350D2 File Offset: 0x000332D2
		private void OnFinalAction(Widget widget)
		{
			if (this.NewCraftedWeaponPopupWidget != null && this.IsInCraftingMode)
			{
				bool isVisible = this.NewCraftedWeaponPopupWidget.IsVisible;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x000350F0 File Offset: 0x000332F0
		// (set) Token: 0x06001363 RID: 4963 RVA: 0x000350F8 File Offset: 0x000332F8
		[Editor(false)]
		public bool IsInCraftingMode
		{
			get
			{
				return this._isInCraftingMode;
			}
			set
			{
				if (this._isInCraftingMode != value)
				{
					this._isInCraftingMode = value;
					base.OnPropertyChanged(value, "IsInCraftingMode");
				}
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x00035116 File Offset: 0x00033316
		// (set) Token: 0x06001365 RID: 4965 RVA: 0x0003511E File Offset: 0x0003331E
		[Editor(false)]
		public bool IsInRefinementMode
		{
			get
			{
				return this._isInRefinementMode;
			}
			set
			{
				if (this._isInRefinementMode != value)
				{
					this._isInRefinementMode = value;
					base.OnPropertyChanged(value, "IsInRefinementMode");
				}
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x0003513C File Offset: 0x0003333C
		// (set) Token: 0x06001367 RID: 4967 RVA: 0x00035144 File Offset: 0x00033344
		[Editor(false)]
		public bool IsInSmeltingMode
		{
			get
			{
				return this._isInSmeltingMode;
			}
			set
			{
				if (this._isInSmeltingMode != value)
				{
					this._isInSmeltingMode = value;
					base.OnPropertyChanged(value, "IsInSmeltingMode");
				}
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x00035162 File Offset: 0x00033362
		// (set) Token: 0x06001369 RID: 4969 RVA: 0x0003516C File Offset: 0x0003336C
		[Editor(false)]
		public ButtonWidget MainActionButtonWidget
		{
			get
			{
				return this._mainActionButtonWidget;
			}
			set
			{
				if (this._mainActionButtonWidget != value)
				{
					this._mainActionButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "MainActionButtonWidget");
					if (!value.ClickEventHandlers.Contains(new Action<Widget>(this.OnMainAction)))
					{
						value.ClickEventHandlers.Add(new Action<Widget>(this.OnMainAction));
					}
				}
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x000351C5 File Offset: 0x000333C5
		// (set) Token: 0x0600136B RID: 4971 RVA: 0x000351D0 File Offset: 0x000333D0
		[Editor(false)]
		public ButtonWidget FinalCraftButtonWidget
		{
			get
			{
				return this._mainActionButtonWidget;
			}
			set
			{
				if (this._finalCraftButtonWidget != value)
				{
					this._finalCraftButtonWidget = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FinalCraftButtonWidget");
					if (!value.ClickEventHandlers.Contains(new Action<Widget>(this.OnFinalAction)))
					{
						value.ClickEventHandlers.Add(new Action<Widget>(this.OnFinalAction));
					}
				}
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x00035229 File Offset: 0x00033429
		// (set) Token: 0x0600136D RID: 4973 RVA: 0x00035231 File Offset: 0x00033431
		[Editor(false)]
		public Widget NewCraftedWeaponPopupWidget
		{
			get
			{
				return this._newCraftedWeaponPopupWidget;
			}
			set
			{
				if (this._newCraftedWeaponPopupWidget != value)
				{
					this._newCraftedWeaponPopupWidget = value;
					base.OnPropertyChanged<Widget>(value, "NewCraftedWeaponPopupWidget");
				}
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x0003524F File Offset: 0x0003344F
		// (set) Token: 0x0600136F RID: 4975 RVA: 0x00035257 File Offset: 0x00033457
		[Editor(false)]
		public Widget CraftingOrderPopupWidget
		{
			get
			{
				return this._craftingOrdersPopupWidget;
			}
			set
			{
				if (this._craftingOrdersPopupWidget != value)
				{
					this._craftingOrdersPopupWidget = value;
					base.OnPropertyChanged<Widget>(value, "CraftingOrderPopupWidget");
				}
			}
		}

		// Token: 0x040008D2 RID: 2258
		private ButtonWidget _mainActionButtonWidget;

		// Token: 0x040008D3 RID: 2259
		private ButtonWidget _finalCraftButtonWidget;

		// Token: 0x040008D4 RID: 2260
		private bool _isInCraftingMode;

		// Token: 0x040008D5 RID: 2261
		private bool _isInRefinementMode;

		// Token: 0x040008D6 RID: 2262
		private bool _isInSmeltingMode;

		// Token: 0x040008D7 RID: 2263
		private Widget _newCraftedWeaponPopupWidget;

		// Token: 0x040008D8 RID: 2264
		private Widget _craftingOrdersPopupWidget;
	}
}
