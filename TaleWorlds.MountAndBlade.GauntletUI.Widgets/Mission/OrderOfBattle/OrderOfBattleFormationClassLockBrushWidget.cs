using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.OrderOfBattle
{
	// Token: 0x020000ED RID: 237
	public class OrderOfBattleFormationClassLockBrushWidget : BrushWidget
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x00021C50 File Offset: 0x0001FE50
		public OrderOfBattleFormationClassLockBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00021C59 File Offset: 0x0001FE59
		private void OnLockStateSet()
		{
			if (this.IsLocked)
			{
				base.Brush = this.LockedBrush;
			}
			else
			{
				base.Brush = this.UnlockedBrush;
			}
			this._isInitialStateSet = true;
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00021C84 File Offset: 0x0001FE84
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x00021C8C File Offset: 0x0001FE8C
		[Editor(false)]
		public bool IsLocked
		{
			get
			{
				return this._isLocked;
			}
			set
			{
				if (value != this._isLocked || !this._isInitialStateSet)
				{
					this._isLocked = value;
					base.OnPropertyChanged(value, "IsLocked");
					this.OnLockStateSet();
				}
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00021CB8 File Offset: 0x0001FEB8
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00021CC0 File Offset: 0x0001FEC0
		[Editor(false)]
		public Brush LockedBrush
		{
			get
			{
				return this._lockedBrush;
			}
			set
			{
				if (value != this._lockedBrush)
				{
					this._lockedBrush = value;
					base.OnPropertyChanged<Brush>(value, "LockedBrush");
				}
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00021CDE File Offset: 0x0001FEDE
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00021CE6 File Offset: 0x0001FEE6
		[Editor(false)]
		public Brush UnlockedBrush
		{
			get
			{
				return this._unlockedBrush;
			}
			set
			{
				if (value != this._unlockedBrush)
				{
					this._unlockedBrush = value;
					base.OnPropertyChanged<Brush>(value, "UnlockedBrush");
				}
			}
		}

		// Token: 0x0400058B RID: 1419
		private bool _isInitialStateSet;

		// Token: 0x0400058C RID: 1420
		private bool _isLocked;

		// Token: 0x0400058D RID: 1421
		private Brush _lockedBrush;

		// Token: 0x0400058E RID: 1422
		private Brush _unlockedBrush;
	}
}
