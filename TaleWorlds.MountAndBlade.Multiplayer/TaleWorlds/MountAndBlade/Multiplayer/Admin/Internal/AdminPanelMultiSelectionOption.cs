using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal
{
	// Token: 0x0200007A RID: 122
	internal class AdminPanelMultiSelectionOption : AdminPanelOption<IAdminPanelMultiSelectionItem>, IAdminPanelMultiSelectionOption, IAdminPanelOption<IAdminPanelMultiSelectionItem>, IAdminPanelOption
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x000105E5 File Offset: 0x0000E7E5
		public AdminPanelMultiSelectionOption(string uniqueId)
			: base(uniqueId)
		{
			this._availableOptions = new MBList<IAdminPanelMultiSelectionItem>();
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000105F9 File Offset: 0x0000E7F9
		protected override bool AreEqualValues(IAdminPanelMultiSelectionItem first, IAdminPanelMultiSelectionItem second)
		{
			return first == second;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00010600 File Offset: 0x0000E800
		protected override IAdminPanelMultiSelectionItem GetOptionValue(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions)
		{
			string strValue = optionType.GetStrValue(accessMode);
			for (int i = 0; i < this._availableOptions.Count; i++)
			{
				if (this._availableOptions[i].Value == strValue)
				{
					return this._availableOptions[i];
				}
			}
			return null;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00010652 File Offset: 0x0000E852
		protected override void OnValueChanged(IAdminPanelMultiSelectionItem previousValue, IAdminPanelMultiSelectionItem newValue)
		{
			this._selectedOption = newValue;
			base.OnValueChanged(previousValue, newValue);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00010663 File Offset: 0x0000E863
		protected override bool OnGetCanRevertToDefaultValue()
		{
			return this._availableOptions.Contains(base.DefaultValue);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00010678 File Offset: 0x0000E878
		public virtual AdminPanelMultiSelectionOption BuildAvailableOptions(MBReadOnlyList<IAdminPanelMultiSelectionItem> options)
		{
			this._availableOptions.Clear();
			if (options != null && options.Count > 0)
			{
				for (int i = 0; i < options.Count; i++)
				{
					this._availableOptions.Add(options[i]);
				}
			}
			this.OnRefresh();
			return this;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000106C8 File Offset: 0x0000E8C8
		public virtual AdminPanelMultiSelectionOption BuildAvailableOptions(MultiplayerOptions.OptionType optionType, bool buildDefaultValue = true)
		{
			this._availableOptions.Clear();
			string strValue = optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
			List<string> multiplayerOptionsList = MultiplayerOptions.Instance.GetMultiplayerOptionsList(optionType);
			if (multiplayerOptionsList != null && multiplayerOptionsList.Count > 0)
			{
				for (int i = 0; i < multiplayerOptionsList.Count; i++)
				{
					AdminPanelMultiSelectionItem adminPanelMultiSelectionItem = new AdminPanelMultiSelectionItem(multiplayerOptionsList[i], null, false, false, true);
					this._availableOptions.Add(adminPanelMultiSelectionItem);
					if (buildDefaultValue && adminPanelMultiSelectionItem.Value == strValue)
					{
						base.BuildDefaultValue(adminPanelMultiSelectionItem);
						base.BuildInitialValue(adminPanelMultiSelectionItem);
					}
				}
			}
			this.OnRefresh();
			return this;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00010756 File Offset: 0x0000E956
		public MBReadOnlyList<IAdminPanelMultiSelectionItem> GetAvailableOptions()
		{
			return this._availableOptions;
		}

		// Token: 0x0400011F RID: 287
		protected IAdminPanelMultiSelectionItem _selectedOption;

		// Token: 0x04000120 RID: 288
		protected MBList<IAdminPanelMultiSelectionItem> _availableOptions;
	}
}
