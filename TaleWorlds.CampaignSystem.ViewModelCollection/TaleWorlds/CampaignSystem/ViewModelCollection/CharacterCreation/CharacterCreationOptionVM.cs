using System;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x0200015A RID: 346
	public class CharacterCreationOptionVM : ViewModel
	{
		// Token: 0x0600213A RID: 8506 RVA: 0x00077579 File Offset: 0x00075779
		public CharacterCreationOptionVM(Action<CharacterCreationOptionVM> onSelect, NarrativeMenuOption option)
		{
			this._onSelect = onSelect;
			this.Option = option;
			this.RefreshValues();
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x00077598 File Offset: 0x00075798
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ActionText = this.Option.Text.ToString();
			this.PositiveEffectText = this.Option.PositiveEffectText.ToString();
			this.DescriptionText = this.Option.DescriptionText.ToString();
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000775ED File Offset: 0x000757ED
		public void ExecuteSelect()
		{
			Action<CharacterCreationOptionVM> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this);
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600213D RID: 8509 RVA: 0x00077600 File Offset: 0x00075800
		// (set) Token: 0x0600213E RID: 8510 RVA: 0x00077608 File Offset: 0x00075808
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x00077626 File Offset: 0x00075826
		// (set) Token: 0x06002140 RID: 8512 RVA: 0x0007762E File Offset: 0x0007582E
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x00077651 File Offset: 0x00075851
		// (set) Token: 0x06002142 RID: 8514 RVA: 0x00077659 File Offset: 0x00075859
		[DataSourceProperty]
		public string PositiveEffectText
		{
			get
			{
				return this._positiveEffectText;
			}
			set
			{
				if (value != this._positiveEffectText)
				{
					this._positiveEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "PositiveEffectText");
				}
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x0007767C File Offset: 0x0007587C
		// (set) Token: 0x06002144 RID: 8516 RVA: 0x00077684 File Offset: 0x00075884
		[DataSourceProperty]
		public string NegativeEffectText
		{
			get
			{
				return this._negativeEffectText;
			}
			set
			{
				if (value != this._negativeEffectText)
				{
					this._negativeEffectText = value;
					base.OnPropertyChangedWithValue<string>(value, "NegativeEffectText");
				}
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x000776A7 File Offset: 0x000758A7
		// (set) Token: 0x06002146 RID: 8518 RVA: 0x000776AF File Offset: 0x000758AF
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x04000F30 RID: 3888
		public readonly NarrativeMenuOption Option;

		// Token: 0x04000F31 RID: 3889
		private readonly Action<CharacterCreationOptionVM> _onSelect;

		// Token: 0x04000F32 RID: 3890
		private bool _isSelected;

		// Token: 0x04000F33 RID: 3891
		private string _actionText;

		// Token: 0x04000F34 RID: 3892
		private string _positiveEffectText;

		// Token: 0x04000F35 RID: 3893
		private string _negativeEffectText;

		// Token: 0x04000F36 RID: 3894
		private string _descriptionText;
	}
}
