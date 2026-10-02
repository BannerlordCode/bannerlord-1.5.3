using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000157 RID: 343
	public class CharacterCreationGainedSkillItemVM : ViewModel
	{
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002111 RID: 8465 RVA: 0x00076F37 File Offset: 0x00075137
		// (set) Token: 0x06002112 RID: 8466 RVA: 0x00076F3F File Offset: 0x0007513F
		public SkillObject SkillObj { get; private set; }

		// Token: 0x06002113 RID: 8467 RVA: 0x00076F48 File Offset: 0x00075148
		public CharacterCreationGainedSkillItemVM(SkillObject skill)
		{
			this.FocusPointGainList = new MBBindingList<BoolItemWithActionVM>();
			this.SkillObj = skill;
			this.SkillId = this.SkillObj.StringId;
			this.Skill = new EncyclopediaSkillVM(skill, 0);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x00076F80 File Offset: 0x00075180
		public void SetValue(int gainedFromOtherStages, int gainedFromCurrentStage)
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
			this.HasIncreasedInCurrentStage = gainedFromCurrentStage > 0;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x00076FE0 File Offset: 0x000751E0
		internal void ResetValues()
		{
			this.SetValue(0, 0);
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x00076FEA File Offset: 0x000751EA
		// (set) Token: 0x06002117 RID: 8471 RVA: 0x00076FF2 File Offset: 0x000751F2
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

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x00077015 File Offset: 0x00075215
		// (set) Token: 0x06002119 RID: 8473 RVA: 0x0007701D File Offset: 0x0007521D
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

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x0007703B File Offset: 0x0007523B
		// (set) Token: 0x0600211B RID: 8475 RVA: 0x00077043 File Offset: 0x00075243
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

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x00077061 File Offset: 0x00075261
		// (set) Token: 0x0600211D RID: 8477 RVA: 0x00077069 File Offset: 0x00075269
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

		// Token: 0x04000F21 RID: 3873
		private string _skillId;

		// Token: 0x04000F22 RID: 3874
		private EncyclopediaSkillVM _skill;

		// Token: 0x04000F23 RID: 3875
		private bool _hasIncreasedInCurrentStage;

		// Token: 0x04000F24 RID: 3876
		private MBBindingList<BoolItemWithActionVM> _focusPointGainList;
	}
}
