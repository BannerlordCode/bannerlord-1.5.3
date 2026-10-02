using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GatherArmy
{
	// Token: 0x02000152 RID: 338
	public class GatherArmyTupleButtonWidget : ButtonWidget
	{
		// Token: 0x0600120B RID: 4619 RVA: 0x00032612 File Offset: 0x00030812
		public GatherArmyTupleButtonWidget(UIContext context)
			: base(context)
		{
			base.OverrideDefaultStateSwitchingEnabled = true;
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00032622 File Offset: 0x00030822
		protected override void HandleClick()
		{
			if (!this.IsTransferDisabled && (this.IsInCart || this.IsEligible))
			{
				base.HandleClick();
			}
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00032644 File Offset: 0x00030844
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsTransferDisabled || (!this.IsInCart && !this.IsEligible))
			{
				this.SetState("Disabled");
				return;
			}
			if (this.IsInCart)
			{
				this.SetState("Selected");
				return;
			}
			if (base.IsPressed)
			{
				this.SetState("Pressed");
				return;
			}
			if (base.IsHovered)
			{
				this.SetState("Hovered");
				return;
			}
			this.SetState("Default");
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x000326C3 File Offset: 0x000308C3
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x000326CB File Offset: 0x000308CB
		[Editor(false)]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (this._isInCart != value)
				{
					this._isInCart = value;
					base.OnPropertyChanged(value, "IsInCart");
				}
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x000326E9 File Offset: 0x000308E9
		// (set) Token: 0x06001211 RID: 4625 RVA: 0x000326F1 File Offset: 0x000308F1
		[Editor(false)]
		public bool IsEligible
		{
			get
			{
				return this._isEligible;
			}
			set
			{
				if (this._isEligible != value)
				{
					this._isEligible = value;
					base.OnPropertyChanged(value, "IsEligible");
				}
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001212 RID: 4626 RVA: 0x0003270F File Offset: 0x0003090F
		// (set) Token: 0x06001213 RID: 4627 RVA: 0x00032717 File Offset: 0x00030917
		[Editor(false)]
		public bool IsTransferDisabled
		{
			get
			{
				return this._isTransferDisabled;
			}
			set
			{
				if (this._isTransferDisabled != value)
				{
					this._isTransferDisabled = value;
					base.OnPropertyChanged(value, "IsTransferDisabled");
				}
			}
		}

		// Token: 0x04000845 RID: 2117
		private bool _isInCart;

		// Token: 0x04000846 RID: 2118
		private bool _isEligible;

		// Token: 0x04000847 RID: 2119
		private bool _isTransferDisabled;
	}
}
