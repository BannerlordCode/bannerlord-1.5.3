using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001A RID: 26
	public class DropdownItemButtonWidget : ButtonWidget
	{
		// Token: 0x0600015E RID: 350 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public DropdownItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005DD0 File Offset: 0x00003FD0
		protected override void RefreshState()
		{
			if (!this.CanBeSelected && !base.OverrideDefaultStateSwitchingEnabled)
			{
				this.SetState("Disabled");
				if (base.UpdateChildrenStates)
				{
					for (int i = 0; i < base.ChildCount; i++)
					{
						base.GetChild(i).SetState("Disabled");
					}
				}
				return;
			}
			base.RefreshState();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00005E29 File Offset: 0x00004029
		protected override void HandleClick()
		{
			if (!this.CanBeSelected)
			{
				return;
			}
			base.HandleClick();
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00005E3A File Offset: 0x0000403A
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00005E42 File Offset: 0x00004042
		[Editor(false)]
		public bool CanBeSelected
		{
			get
			{
				return this._canBeSelected;
			}
			set
			{
				if (this._canBeSelected != value)
				{
					this._canBeSelected = value;
					base.OnPropertyChanged(value, "CanBeSelected");
					this.RefreshState();
				}
			}
		}

		// Token: 0x040000A3 RID: 163
		private bool _canBeSelected = true;
	}
}
