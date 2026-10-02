using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B0 RID: 176
	public class MultiplayerAdminPanelActionOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x000355D9 File Offset: 0x000337D9
		public MultiplayerAdminPanelActionOptionVM(IAdminPanelAction option)
			: base(null)
		{
			this._action = option;
			this.IsActionOption = true;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x000355F0 File Offset: 0x000337F0
		public override void RefreshValues()
		{
			base.RefreshValues();
			IAdminPanelAction action = this._action;
			base.OptionTitle = ((action != null) ? action.Name : null) ?? string.Empty;
			IAdminPanelAction action2 = this._action;
			base.OptionDescription = ((action2 != null) ? action2.Description : null) ?? string.Empty;
			IAdminPanelAction action3 = this._action;
			if (!string.IsNullOrEmpty((action3 != null) ? action3.Description : null))
			{
				base.DescriptionHint = new HintViewModel(new TextObject("{=!}" + this._action.Description, null), null);
				return;
			}
			base.DescriptionHint = null;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00035690 File Offset: 0x00033890
		public override void UpdateValues()
		{
			base.UpdateValues();
			IAdminPanelAction action = this._action;
			base.IsFilteredOut = action != null && !action.GetIsAvailable();
			string empty = string.Empty;
			IAdminPanelAction action2 = this._action;
			base.IsDisabled = action2 != null && action2.GetIsDisabled(out empty);
			if (!string.IsNullOrEmpty(empty))
			{
				base.DisabledHint = new HintViewModel(new TextObject("{=!}" + empty, null), null);
				return;
			}
			base.DisabledHint = null;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0003570B File Offset: 0x0003390B
		public void ExecuteAction()
		{
			this._action.OnActionExecuted();
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00035718 File Offset: 0x00033918
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x00035720 File Offset: 0x00033920
		[DataSourceProperty]
		public bool IsActionOption
		{
			get
			{
				return this._isActionOption;
			}
			set
			{
				if (value != this._isActionOption)
				{
					this._isActionOption = value;
					base.OnPropertyChangedWithValue(value, "IsActionOption");
				}
			}
		}

		// Token: 0x040007F8 RID: 2040
		private readonly IAdminPanelAction _action;

		// Token: 0x040007F9 RID: 2041
		private bool _isActionOption;
	}
}
