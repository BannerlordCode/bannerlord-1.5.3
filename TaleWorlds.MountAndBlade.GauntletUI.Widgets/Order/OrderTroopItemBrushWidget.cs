using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000075 RID: 117
	public class OrderTroopItemBrushWidget : BrushWidget
	{
		// Token: 0x0600064F RID: 1615 RVA: 0x00012D9C File Offset: 0x00010F9C
		public OrderTroopItemBrushWidget(UIContext context)
			: base(context)
		{
			base.AddState("Selected");
			base.AddState("Disabled");
			this.UpdateBrush();
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00012DC8 File Offset: 0x00010FC8
		private void SelectionStateChanged()
		{
			this.UpdateBackgroundState();
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00012DD0 File Offset: 0x00010FD0
		private void SelectableStateChanged()
		{
			this.UpdateBackgroundState();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00012DD8 File Offset: 0x00010FD8
		private void CurrentMemberCountChanged()
		{
			this.UpdateBackgroundState();
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00012DE0 File Offset: 0x00010FE0
		private void UpdateBackgroundState()
		{
			if (this.CurrentMemberCount <= 0 || !this.IsSelectable)
			{
				this.SetState("Disabled");
				return;
			}
			this.SetState(this.IsSelected ? "Selected" : "Default");
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00012E19 File Offset: 0x00011019
		private void UpdateBrush()
		{
			if (this.MeleeCardBrush == null || this.RangedCardBrush == null)
			{
				return;
			}
			if (this.HasAmmo)
			{
				base.Brush = this.RangedCardBrush;
			}
			else
			{
				base.Brush = this.MeleeCardBrush;
			}
			this.UpdateBackgroundState();
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00012E54 File Offset: 0x00011054
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00012E5C File Offset: 0x0001105C
		[Editor(false)]
		public int CurrentMemberCount
		{
			get
			{
				return this._currentMemberCount;
			}
			set
			{
				if (this._currentMemberCount != value)
				{
					this._currentMemberCount = value;
					base.OnPropertyChanged(value, "CurrentMemberCount");
					this.CurrentMemberCountChanged();
				}
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00012E80 File Offset: 0x00011080
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x00012E88 File Offset: 0x00011088
		[Editor(false)]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (this._isSelectable != value)
				{
					this._isSelectable = value;
					base.OnPropertyChanged(value, "IsSelectable");
					this.SelectableStateChanged();
				}
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00012EAC File Offset: 0x000110AC
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00012EB4 File Offset: 0x000110B4
		[Editor(false)]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					base.OnPropertyChanged(value, "IsSelected");
					this.SelectionStateChanged();
				}
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00012ED8 File Offset: 0x000110D8
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x00012EE0 File Offset: 0x000110E0
		[Editor(false)]
		public bool HasAmmo
		{
			get
			{
				return this._hasAmmo;
			}
			set
			{
				if (this._hasAmmo != value)
				{
					this._hasAmmo = value;
					base.OnPropertyChanged(value, "HasAmmo");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00012F04 File Offset: 0x00011104
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x00012F0C File Offset: 0x0001110C
		[Editor(false)]
		public Brush RangedCardBrush
		{
			get
			{
				return this._rangedCardBrush;
			}
			set
			{
				if (value != this._rangedCardBrush)
				{
					this._rangedCardBrush = value;
					base.OnPropertyChanged<Brush>(value, "RangedCardBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00012F30 File Offset: 0x00011130
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x00012F38 File Offset: 0x00011138
		[Editor(false)]
		public Brush MeleeCardBrush
		{
			get
			{
				return this._meleeCardBrush;
			}
			set
			{
				if (value != this._meleeCardBrush)
				{
					this._meleeCardBrush = value;
					base.OnPropertyChanged<Brush>(value, "MeleeCardBrush");
					this.UpdateBrush();
				}
			}
		}

		// Token: 0x040002B2 RID: 690
		private int _currentMemberCount;

		// Token: 0x040002B3 RID: 691
		private bool _isSelectable;

		// Token: 0x040002B4 RID: 692
		private bool _isSelected;

		// Token: 0x040002B5 RID: 693
		private bool _hasAmmo = true;

		// Token: 0x040002B6 RID: 694
		private Brush _rangedCardBrush;

		// Token: 0x040002B7 RID: 695
		private Brush _meleeCardBrush;
	}
}
