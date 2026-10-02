using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper.PerkSelection
{
	// Token: 0x0200014D RID: 333
	public class PerkSelectionItemVM : ViewModel
	{
		// Token: 0x06002091 RID: 8337 RVA: 0x0007531A File Offset: 0x0007351A
		public PerkSelectionItemVM(PerkObject perk, Action<PerkSelectionItemVM> onSelection)
		{
			this.Perk = perk;
			this._onSelection = onSelection;
			this.RefreshValues();
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x00075338 File Offset: 0x00073538
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PickText = new TextObject("{=1CXlqb2U}Pick:", null).ToString();
			this.PerkName = this.Perk.Name.ToString();
			this.PerkDescription = this.Perk.Description.ToString();
			TextObject combinedPerkRoleText = CampaignUIHelper.GetCombinedPerkRoleText(this.Perk);
			this.PerkRole = ((combinedPerkRoleText != null) ? combinedPerkRoleText.ToString() : null) ?? "";
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000753B3 File Offset: 0x000735B3
		public void ExecuteSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x000753C1 File Offset: 0x000735C1
		// (set) Token: 0x06002095 RID: 8341 RVA: 0x000753C9 File Offset: 0x000735C9
		[DataSourceProperty]
		public string PickText
		{
			get
			{
				return this._pickText;
			}
			set
			{
				if (value != this._pickText)
				{
					this._pickText = value;
					base.OnPropertyChangedWithValue<string>(value, "PickText");
				}
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x000753EC File Offset: 0x000735EC
		// (set) Token: 0x06002097 RID: 8343 RVA: 0x000753F4 File Offset: 0x000735F4
		[DataSourceProperty]
		public string PerkName
		{
			get
			{
				return this._perkName;
			}
			set
			{
				if (value != this._perkName)
				{
					this._perkName = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkName");
				}
			}
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x00075417 File Offset: 0x00073617
		// (set) Token: 0x06002099 RID: 8345 RVA: 0x0007541F File Offset: 0x0007361F
		[DataSourceProperty]
		public string PerkDescription
		{
			get
			{
				return this._perkDescription;
			}
			set
			{
				if (value != this._perkDescription)
				{
					this._perkDescription = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkDescription");
				}
			}
		}

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x00075442 File Offset: 0x00073642
		// (set) Token: 0x0600209B RID: 8347 RVA: 0x0007544A File Offset: 0x0007364A
		[DataSourceProperty]
		public string PerkRole
		{
			get
			{
				return this._perkRole;
			}
			set
			{
				if (value != this._perkRole)
				{
					this._perkRole = value;
					base.OnPropertyChangedWithValue<string>(value, "PerkRole");
				}
			}
		}

		// Token: 0x04000EEC RID: 3820
		private readonly Action<PerkSelectionItemVM> _onSelection;

		// Token: 0x04000EED RID: 3821
		public readonly PerkObject Perk;

		// Token: 0x04000EEE RID: 3822
		private string _pickText;

		// Token: 0x04000EEF RID: 3823
		private string _perkName;

		// Token: 0x04000EF0 RID: 3824
		private string _perkDescription;

		// Token: 0x04000EF1 RID: 3825
		private string _perkRole;
	}
}
