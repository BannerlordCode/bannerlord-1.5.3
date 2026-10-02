using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.InitialMenu
{
	// Token: 0x0200004B RID: 75
	public class InitialMenuOptionVM : ViewModel
	{
		// Token: 0x0600063A RID: 1594 RVA: 0x000170D6 File Offset: 0x000152D6
		public InitialMenuOptionVM(InitialStateOption initialStateOption)
		{
			this.InitialStateOption = initialStateOption;
			this.DisabledHint = new HintViewModel(initialStateOption.IsDisabledAndReason().Item2, null);
			this.EnabledHint = new HintViewModel(initialStateOption.EnabledHint, null);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00017114 File Offset: 0x00015314
		public void ExecuteAction()
		{
			InitialState initialState = GameStateManager.Current.ActiveState as InitialState;
			if (initialState != null)
			{
				initialState.OnExecutedInitialStateOption(this.InitialStateOption);
				this.InitialStateOption.DoAction();
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001714B File Offset: 0x0001534B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DisabledHint.HintText = this.InitialStateOption.IsDisabledAndReason().Item2;
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600063D RID: 1597 RVA: 0x00017173 File Offset: 0x00015373
		// (set) Token: 0x0600063E RID: 1598 RVA: 0x0001717B File Offset: 0x0001537B
		[DataSourceProperty]
		public HintViewModel DisabledHint
		{
			get
			{
				return this._disabledHint;
			}
			set
			{
				if (value != this._disabledHint)
				{
					this._disabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisabledHint");
				}
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00017199 File Offset: 0x00015399
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x000171A1 File Offset: 0x000153A1
		[DataSourceProperty]
		public HintViewModel EnabledHint
		{
			get
			{
				return this._enabledHint;
			}
			set
			{
				if (value != this._enabledHint)
				{
					this._enabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EnabledHint");
				}
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x000171BF File Offset: 0x000153BF
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this.InitialStateOption.Name.ToString();
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x000171D1 File Offset: 0x000153D1
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this.InitialStateOption.IsDisabledAndReason().Item1;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x000171E8 File Offset: 0x000153E8
		[DataSourceProperty]
		public bool IsHidden
		{
			get
			{
				Func<bool> isHidden = this.InitialStateOption.IsHidden;
				return isHidden != null && isHidden();
			}
		}

		// Token: 0x040002CA RID: 714
		public readonly InitialStateOption InitialStateOption;

		// Token: 0x040002CB RID: 715
		private HintViewModel _disabledHint;

		// Token: 0x040002CC RID: 716
		private HintViewModel _enabledHint;
	}
}
