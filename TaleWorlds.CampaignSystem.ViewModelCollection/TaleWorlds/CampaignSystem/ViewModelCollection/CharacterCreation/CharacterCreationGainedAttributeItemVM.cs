using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000158 RID: 344
	public class CharacterCreationGainedAttributeItemVM : ViewModel
	{
		// Token: 0x0600211E RID: 8478 RVA: 0x00077088 File Offset: 0x00075288
		public CharacterCreationGainedAttributeItemVM(CharacterAttribute attributeObj)
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

		// Token: 0x0600211F RID: 8479 RVA: 0x000770E9 File Offset: 0x000752E9
		internal void ResetValues()
		{
			this.SetValue(0, 0);
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000770F4 File Offset: 0x000752F4
		public void SetValue(int gainedFromOtherStages, int gainedFromCurrentStage)
		{
			this.HasIncreasedInCurrentStage = gainedFromCurrentStage > 0;
			GameTexts.SetVariable("LEFT", this._attributeObj.Name);
			GameTexts.SetVariable("RIGHT", gainedFromOtherStages + gainedFromCurrentStage);
			this.NameText = GameTexts.FindText("str_LEFT_colon_RIGHT_wSpaceAfterColon", null).ToString();
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002121 RID: 8481 RVA: 0x00077143 File Offset: 0x00075343
		// (set) Token: 0x06002122 RID: 8482 RVA: 0x0007714B File Offset: 0x0007534B
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

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x00077169 File Offset: 0x00075369
		// (set) Token: 0x06002124 RID: 8484 RVA: 0x00077171 File Offset: 0x00075371
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

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002125 RID: 8485 RVA: 0x00077194 File Offset: 0x00075394
		// (set) Token: 0x06002126 RID: 8486 RVA: 0x0007719C File Offset: 0x0007539C
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

		// Token: 0x04000F25 RID: 3877
		private readonly CharacterAttribute _attributeObj;

		// Token: 0x04000F26 RID: 3878
		private string _nameText;

		// Token: 0x04000F27 RID: 3879
		private bool _hasIncreasedInCurrentStage;

		// Token: 0x04000F28 RID: 3880
		private BasicTooltipViewModel _hint;
	}
}
