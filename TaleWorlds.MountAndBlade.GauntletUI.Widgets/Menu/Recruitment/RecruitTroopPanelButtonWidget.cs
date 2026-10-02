using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Recruitment
{
	// Token: 0x02000110 RID: 272
	public class RecruitTroopPanelButtonWidget : ButtonWidget
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x000281E2 File Offset: 0x000263E2
		public RecruitTroopPanelButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000281EC File Offset: 0x000263EC
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.IsTroopEmpty)
			{
				if (this.PlayerHasEnoughRelation)
				{
					this.SetState("EmptyEnoughRelation");
				}
				else
				{
					this.SetState("EmptyNoRelation");
				}
			}
			else if (this.CanBeRecruited)
			{
				this.SetState("Available");
			}
			else
			{
				this.SetState("Unavailable");
			}
			if (!this.PlayerHasEnoughRelation && !this.IsTroopEmpty && this.CharacterImageWidget != null)
			{
				this.CharacterImageWidget.Brush.ValueFactor = -50f;
				this.CharacterImageWidget.Brush.SaturationFactor = -100f;
			}
			if (this.CharacterImageWidget != null)
			{
				this.CharacterImageWidget.IsHidden = this.IsTroopEmpty;
			}
			this.RemoveFromCartButton.SetState(((base.IsHovered || base.IsPressed || base.IsSelected) && ((!this.IsTroopEmpty && this.PlayerHasEnoughRelation) || this.IsInCart)) ? "Hovered" : "Default");
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x000282EC File Offset: 0x000264EC
		private bool IsMouseOverWidget()
		{
			Vector2 globalPosition = base.GlobalPosition;
			return this.IsBetween(base.EventManager.MousePosition.X, globalPosition.X, globalPosition.X + base.Size.X) && this.IsBetween(base.EventManager.MousePosition.Y, globalPosition.Y, globalPosition.Y + base.Size.Y);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00028360 File Offset: 0x00026560
		private bool IsBetween(float number, float min, float max)
		{
			return number >= min && number <= max;
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0002836F File Offset: 0x0002656F
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x00028377 File Offset: 0x00026577
		[Editor(false)]
		public bool CanBeRecruited
		{
			get
			{
				return this._canBeRecruited;
			}
			set
			{
				if (this._canBeRecruited != value)
				{
					this._canBeRecruited = value;
					base.OnPropertyChanged(value, "CanBeRecruited");
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00028395 File Offset: 0x00026595
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x0002839D File Offset: 0x0002659D
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

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x000283BB File Offset: 0x000265BB
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x000283C3 File Offset: 0x000265C3
		[Editor(false)]
		public ButtonWidget RemoveFromCartButton
		{
			get
			{
				return this._removeFromCartButton;
			}
			set
			{
				if (this._removeFromCartButton != value)
				{
					this._removeFromCartButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "RemoveFromCartButton");
				}
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x000283E1 File Offset: 0x000265E1
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x000283E9 File Offset: 0x000265E9
		[Editor(false)]
		public ImageIdentifierWidget CharacterImageWidget
		{
			get
			{
				return this._characterImageWidget;
			}
			set
			{
				if (this._characterImageWidget != value)
				{
					this._characterImageWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "CharacterImageWidget");
				}
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x00028407 File Offset: 0x00026607
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x0002840F File Offset: 0x0002660F
		[Editor(false)]
		public bool IsTroopEmpty
		{
			get
			{
				return this._isTroopEmpty;
			}
			set
			{
				if (this._isTroopEmpty != value)
				{
					this._isTroopEmpty = value;
					base.OnPropertyChanged(value, "IsTroopEmpty");
				}
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0002842D File Offset: 0x0002662D
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x00028435 File Offset: 0x00026635
		[Editor(false)]
		public bool PlayerHasEnoughRelation
		{
			get
			{
				return this._playerHasEnoughRelation;
			}
			set
			{
				if (this._playerHasEnoughRelation != value)
				{
					this._playerHasEnoughRelation = value;
					base.OnPropertyChanged(value, "PlayerHasEnoughRelation");
				}
			}
		}

		// Token: 0x04000698 RID: 1688
		private bool _canBeRecruited;

		// Token: 0x04000699 RID: 1689
		private bool _isInCart;

		// Token: 0x0400069A RID: 1690
		private bool _playerHasEnoughRelation;

		// Token: 0x0400069B RID: 1691
		private bool _isTroopEmpty;

		// Token: 0x0400069C RID: 1692
		private ButtonWidget _removeFromCartButton;

		// Token: 0x0400069D RID: 1693
		private ImageIdentifierWidget _characterImageWidget;
	}
}
