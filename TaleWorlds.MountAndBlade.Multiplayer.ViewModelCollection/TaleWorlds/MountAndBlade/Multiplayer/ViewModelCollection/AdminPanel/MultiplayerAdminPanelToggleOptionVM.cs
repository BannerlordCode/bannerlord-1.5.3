using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B6 RID: 182
	public class MultiplayerAdminPanelToggleOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x06001141 RID: 4417 RVA: 0x00036372 File Offset: 0x00034572
		public MultiplayerAdminPanelToggleOptionVM(IAdminPanelOption<bool> option)
			: base(option)
		{
			this._option = option;
			this.ToggleValue = this._option.GetValue();
			this.IsToggleOption = true;
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x0003639A File Offset: 0x0003459A
		public override void UpdateValues()
		{
			base.UpdateValues();
			this.ToggleValue = this._option.GetValue();
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x000363B3 File Offset: 0x000345B3
		public void ExecuteToggle()
		{
			this.ToggleValue = !this.ToggleValue;
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x000363C4 File Offset: 0x000345C4
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x000363CC File Offset: 0x000345CC
		[DataSourceProperty]
		public bool IsToggleOption
		{
			get
			{
				return this._isToggleOption;
			}
			set
			{
				if (value != this._isToggleOption)
				{
					this._isToggleOption = value;
					base.OnPropertyChangedWithValue(value, "IsToggleOption");
				}
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x000363EA File Offset: 0x000345EA
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x000363F2 File Offset: 0x000345F2
		[DataSourceProperty]
		public bool ToggleValue
		{
			get
			{
				return this._toggleValue;
			}
			set
			{
				if (value != this._toggleValue)
				{
					this._toggleValue = value;
					base.OnPropertyChangedWithValue(value, "ToggleValue");
					this._option.SetValue(value);
				}
			}
		}

		// Token: 0x0400081C RID: 2076
		private new readonly IAdminPanelOption<bool> _option;

		// Token: 0x0400081D RID: 2077
		private bool _isToggleOption;

		// Token: 0x0400081E RID: 2078
		private bool _toggleValue;
	}
}
