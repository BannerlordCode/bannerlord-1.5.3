using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.GameMenu
{
	// Token: 0x02000156 RID: 342
	public class GameMenuTroopSelectionItemButtonWidget : ButtonWidget
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x00032905 File Offset: 0x00030B05
		// (set) Token: 0x0600122A RID: 4650 RVA: 0x0003290D File Offset: 0x00030B0D
		public ButtonWidget AddButtonWidget { get; set; }

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600122B RID: 4651 RVA: 0x00032916 File Offset: 0x00030B16
		// (set) Token: 0x0600122C RID: 4652 RVA: 0x0003291E File Offset: 0x00030B1E
		public ButtonWidget RemoveButtonWidget { get; set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x00032927 File Offset: 0x00030B27
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x0003292F File Offset: 0x00030B2F
		public Widget CheckmarkVisualWidget { get; set; }

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00032938 File Offset: 0x00030B38
		// (set) Token: 0x06001230 RID: 4656 RVA: 0x00032940 File Offset: 0x00030B40
		public Widget AddRemoveControls { get; set; }

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x00032949 File Offset: 0x00030B49
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x00032951 File Offset: 0x00030B51
		public Widget HeroHealthParent { get; set; }

		// Token: 0x06001233 RID: 4659 RVA: 0x0003295A File Offset: 0x00030B5A
		public GameMenuTroopSelectionItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0003296C File Offset: 0x00030B6C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.AddButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnAdd));
				this.RemoveButtonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnRemove));
				this._initialized = true;
			}
			if (this._isDirty)
			{
				this.Refresh();
			}
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000329D5 File Offset: 0x00030BD5
		private void OnRemove(Widget obj)
		{
			base.EventFired("Remove", Array.Empty<object>());
			this.Refresh();
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000329ED File Offset: 0x00030BED
		private void OnAdd(Widget obj)
		{
			base.EventFired("Add", Array.Empty<object>());
			this.Refresh();
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x00032A05 File Offset: 0x00030C05
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.CurrentAmount == 0)
			{
				base.EventFired("Add", Array.Empty<object>());
			}
			else
			{
				base.EventFired("Remove", Array.Empty<object>());
			}
			this.Refresh();
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00032A40 File Offset: 0x00030C40
		private void Refresh()
		{
			if (this.CheckmarkVisualWidget == null || this.AddRemoveControls == null || this.AddButtonWidget == null || this.RemoveButtonWidget == null)
			{
				return;
			}
			if (this.MaxAmount == 0)
			{
				base.DoNotAcceptEvents = false;
				base.DoNotPassEventsToChildren = true;
				this.CheckmarkVisualWidget.IsHidden = this.CurrentAmount == 0;
				this.AddRemoveControls.IsHidden = true;
				this.AddButtonWidget.IsHidden = true;
				this.RemoveButtonWidget.IsHidden = true;
				base.IsDisabled = true;
				base.DominantSelectedState = this.IsLocked;
				this.HeroHealthParent.IsHidden = !this.IsTroopHero;
				if (this.IsLocked)
				{
					base.IsDisabled = this.CurrentAmount <= 0;
					base.DoNotPassEventsToChildren = true;
					base.DoNotAcceptEvents = true;
				}
			}
			else if (this.MaxAmount == 1)
			{
				base.DoNotAcceptEvents = false;
				base.DoNotPassEventsToChildren = true;
				this.CheckmarkVisualWidget.IsHidden = this.CurrentAmount == 0;
				this.AddRemoveControls.IsHidden = true;
				this.AddButtonWidget.IsHidden = true;
				this.RemoveButtonWidget.IsHidden = true;
				base.IsDisabled = (this.IsRosterFull && this.CurrentAmount <= 0) || this.IsLocked;
				base.DominantSelectedState = this.IsLocked;
				this.HeroHealthParent.IsHidden = !this.IsTroopHero;
				if (this.IsLocked)
				{
					base.IsDisabled = this.CurrentAmount <= 0;
					base.DoNotPassEventsToChildren = true;
					base.DoNotAcceptEvents = true;
				}
			}
			else
			{
				base.DoNotAcceptEvents = true;
				base.DoNotPassEventsToChildren = false;
				this.CheckmarkVisualWidget.IsHidden = true;
				this.AddRemoveControls.IsHidden = false;
				this.HeroHealthParent.IsHidden = true;
				this.AddButtonWidget.IsHidden = false;
				this.RemoveButtonWidget.IsHidden = false;
				this.AddButtonWidget.IsDisabled = this.IsRosterFull || this.CurrentAmount >= this.MaxAmount;
				this.RemoveButtonWidget.IsDisabled = this.CurrentAmount <= 0;
				if (this.IsLocked)
				{
					base.IsDisabled = false;
					base.DoNotPassEventsToChildren = true;
					base.DoNotAcceptEvents = true;
				}
			}
			base.GamepadNavigationIndex = (this.AddRemoveControls.IsVisible ? (-1) : 0);
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x00032C94 File Offset: 0x00030E94
		// (set) Token: 0x0600123A RID: 4666 RVA: 0x00032C9C File Offset: 0x00030E9C
		public bool IsRosterFull
		{
			get
			{
				return this._isRosterFull;
			}
			set
			{
				if (this._isRosterFull != value)
				{
					this._isRosterFull = value;
					base.OnPropertyChanged(value, "IsRosterFull");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x00032CC1 File Offset: 0x00030EC1
		// (set) Token: 0x0600123C RID: 4668 RVA: 0x00032CC9 File Offset: 0x00030EC9
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (this._isLocked != value)
				{
					this._isLocked = value;
					base.OnPropertyChanged(value, "IsLocked");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x00032CEE File Offset: 0x00030EEE
		// (set) Token: 0x0600123E RID: 4670 RVA: 0x00032CF6 File Offset: 0x00030EF6
		public bool IsTroopHero
		{
			get
			{
				return this._isTroopHero;
			}
			set
			{
				if (this._isTroopHero != value)
				{
					this._isTroopHero = value;
					base.OnPropertyChanged(value, "IsTroopHero");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00032D1B File Offset: 0x00030F1B
		// (set) Token: 0x06001240 RID: 4672 RVA: 0x00032D23 File Offset: 0x00030F23
		public int CurrentAmount
		{
			get
			{
				return this._currentAmount;
			}
			set
			{
				if (this._currentAmount != value)
				{
					this._currentAmount = value;
					base.OnPropertyChanged(value, "CurrentAmount");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00032D48 File Offset: 0x00030F48
		// (set) Token: 0x06001242 RID: 4674 RVA: 0x00032D50 File Offset: 0x00030F50
		public int MaxAmount
		{
			get
			{
				return this._maxAmount;
			}
			set
			{
				if (this._maxAmount != value)
				{
					this._maxAmount = value;
					base.OnPropertyChanged(value, "MaxAmount");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x04000854 RID: 2132
		private bool _initialized;

		// Token: 0x04000855 RID: 2133
		private bool _isDirty = true;

		// Token: 0x04000856 RID: 2134
		private int _maxAmount;

		// Token: 0x04000857 RID: 2135
		private int _currentAmount;

		// Token: 0x04000858 RID: 2136
		private bool _isRosterFull;

		// Token: 0x04000859 RID: 2137
		private bool _isLocked;

		// Token: 0x0400085A RID: 2138
		private bool _isTroopHero;
	}
}
