using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F9 RID: 249
	public class EducationGainedAttributeItemVM : ViewModel
	{
		// Token: 0x0600164B RID: 5707 RVA: 0x00057994 File Offset: 0x00055B94
		public EducationGainedAttributeItemVM(CharacterAttribute attributeObj)
		{
			this._attributeObj = attributeObj;
			TextObject nameExtended = this._attributeObj.Name;
			TextObject desc = this._attributeObj.Description;
			this.Hint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("STR1", nameExtended);
				GameTexts.SetVariable("STR2", desc);
				return GameTexts.FindText("str_string_newline_string", null).ToString();
			});
			this.SetValue(0, 0);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x000579F5 File Offset: 0x00055BF5
		internal void ResetValues()
		{
			this.SetValue(0, 0);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x00057A00 File Offset: 0x00055C00
		public void SetValue(int gainedFromOtherStages, int gainedFromCurrentStage)
		{
			this.HasIncreasedInCurrentStage = gainedFromCurrentStage > 0;
			GameTexts.SetVariable("LEFT", this._attributeObj.Name);
			GameTexts.SetVariable("RIGHT", gainedFromOtherStages + gainedFromCurrentStage);
			this.NameText = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x00057A4F File Offset: 0x00055C4F
		// (set) Token: 0x0600164F RID: 5711 RVA: 0x00057A57 File Offset: 0x00055C57
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x00057A75 File Offset: 0x00055C75
		// (set) Token: 0x06001651 RID: 5713 RVA: 0x00057A7D File Offset: 0x00055C7D
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x00057AA0 File Offset: 0x00055CA0
		// (set) Token: 0x06001653 RID: 5715 RVA: 0x00057AA8 File Offset: 0x00055CA8
		[DataSourceProperty]
		public bool HasIncreasedInCurrentStage
		{
			get
			{
				return this._hasIncreasedInCurrentStage;
			}
			set
			{
				if (value != this._hasIncreasedInCurrentStage)
				{
					this._hasIncreasedInCurrentStage = value;
					base.OnPropertyChangedWithValue(value, "HasIncreasedInCurrentStage");
				}
			}
		}

		// Token: 0x04000A16 RID: 2582
		private readonly CharacterAttribute _attributeObj;

		// Token: 0x04000A17 RID: 2583
		private string _nameText;

		// Token: 0x04000A18 RID: 2584
		private bool _hasIncreasedInCurrentStage;

		// Token: 0x04000A19 RID: 2585
		private BasicTooltipViewModel _hint;
	}
}
