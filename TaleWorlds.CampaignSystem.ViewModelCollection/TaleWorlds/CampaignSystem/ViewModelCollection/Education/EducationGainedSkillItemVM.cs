using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F8 RID: 248
	public class EducationGainedSkillItemVM : ViewModel
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x000577DB File Offset: 0x000559DB
		// (set) Token: 0x0600163A RID: 5690 RVA: 0x000577E3 File Offset: 0x000559E3
		public SkillObject SkillObj { get; private set; }

		// Token: 0x0600163B RID: 5691 RVA: 0x000577EC File Offset: 0x000559EC
		public EducationGainedSkillItemVM(SkillObject skill)
		{
			this.FocusPointGainList = new MBBindingList<BoolItemWithActionVM>();
			this.SkillObj = skill;
			this.SkillId = this.SkillObj.StringId;
			this.Skill = new EncyclopediaSkillVM(skill, 0);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x00057824 File Offset: 0x00055A24
		public void SetFocusValue(int gainedFromOtherStages, int gainedFromCurrentStage)
		{
			this.FocusPointGainList.Clear();
			for (int i = 0; i < gainedFromOtherStages; i++)
			{
				this.FocusPointGainList.Add(new BoolItemWithActionVM(null, false, null));
			}
			for (int j = 0; j < gainedFromCurrentStage; j++)
			{
				this.FocusPointGainList.Add(new BoolItemWithActionVM(null, true, null));
			}
			this.HasFocusIncreasedInCurrentStage = gainedFromCurrentStage > 0;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00057884 File Offset: 0x00055A84
		public void SetSkillValue(int gaintedFromOtherStages, int gainedFromCurrentStage)
		{
			this.SkillValueInt = gaintedFromOtherStages + gainedFromCurrentStage;
			this.HasSkillValueIncreasedInCurrentStage = gainedFromCurrentStage > 0;
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00057899 File Offset: 0x00055A99
		internal void ResetValues()
		{
			this.SetFocusValue(0, 0);
			this.SetSkillValue(0, 0);
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x0600163F RID: 5695 RVA: 0x000578AB File Offset: 0x00055AAB
		// (set) Token: 0x06001640 RID: 5696 RVA: 0x000578B3 File Offset: 0x00055AB3
		[DataSourceProperty]
		public string SkillId
		{
			get
			{
				return this._skillId;
			}
			set
			{
				if (value != this._skillId)
				{
					this._skillId = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillId");
				}
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001641 RID: 5697 RVA: 0x000578D6 File Offset: 0x00055AD6
		// (set) Token: 0x06001642 RID: 5698 RVA: 0x000578DE File Offset: 0x00055ADE
		[DataSourceProperty]
		public int SkillValueInt
		{
			get
			{
				return this._skillValueInt;
			}
			set
			{
				if (value != this._skillValueInt)
				{
					this._skillValueInt = value;
					base.OnPropertyChangedWithValue(value, "SkillValueInt");
				}
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001643 RID: 5699 RVA: 0x000578FC File Offset: 0x00055AFC
		// (set) Token: 0x06001644 RID: 5700 RVA: 0x00057904 File Offset: 0x00055B04
		[DataSourceProperty]
		public EncyclopediaSkillVM Skill
		{
			get
			{
				return this._skill;
			}
			set
			{
				if (value != this._skill)
				{
					this._skill = value;
					base.OnPropertyChangedWithValue<EncyclopediaSkillVM>(value, "Skill");
				}
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001645 RID: 5701 RVA: 0x00057922 File Offset: 0x00055B22
		// (set) Token: 0x06001646 RID: 5702 RVA: 0x0005792A File Offset: 0x00055B2A
		[DataSourceProperty]
		public bool HasFocusIncreasedInCurrentStage
		{
			get
			{
				return this._hasFocusIncreasedInCurrentStage;
			}
			set
			{
				if (value != this._hasFocusIncreasedInCurrentStage)
				{
					this._hasFocusIncreasedInCurrentStage = value;
					base.OnPropertyChangedWithValue(value, "HasFocusIncreasedInCurrentStage");
				}
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001647 RID: 5703 RVA: 0x00057948 File Offset: 0x00055B48
		// (set) Token: 0x06001648 RID: 5704 RVA: 0x00057950 File Offset: 0x00055B50
		[DataSourceProperty]
		public bool HasSkillValueIncreasedInCurrentStage
		{
			get
			{
				return this._hasSkillValueIncreasedInCurrentStage;
			}
			set
			{
				if (value != this._hasSkillValueIncreasedInCurrentStage)
				{
					this._hasSkillValueIncreasedInCurrentStage = value;
					base.OnPropertyChangedWithValue(value, "HasSkillValueIncreasedInCurrentStage");
				}
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001649 RID: 5705 RVA: 0x0005796E File Offset: 0x00055B6E
		// (set) Token: 0x0600164A RID: 5706 RVA: 0x00057976 File Offset: 0x00055B76
		[DataSourceProperty]
		public MBBindingList<BoolItemWithActionVM> FocusPointGainList
		{
			get
			{
				return this._focusPointGainList;
			}
			set
			{
				if (value != this._focusPointGainList)
				{
					this._focusPointGainList = value;
					base.OnPropertyChangedWithValue<MBBindingList<BoolItemWithActionVM>>(value, "FocusPointGainList");
				}
			}
		}

		// Token: 0x04000A10 RID: 2576
		private string _skillId;

		// Token: 0x04000A11 RID: 2577
		private EncyclopediaSkillVM _skill;

		// Token: 0x04000A12 RID: 2578
		private bool _hasFocusIncreasedInCurrentStage;

		// Token: 0x04000A13 RID: 2579
		private bool _hasSkillValueIncreasedInCurrentStage;

		// Token: 0x04000A14 RID: 2580
		private int _skillValueInt;

		// Token: 0x04000A15 RID: 2581
		private MBBindingList<BoolItemWithActionVM> _focusPointGainList;
	}
}
