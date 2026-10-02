using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000013 RID: 19
	public class SPScoreboardSkillItemVM : ViewModel
	{
		// Token: 0x06000173 RID: 371 RVA: 0x00005D28 File Offset: 0x00003F28
		public SPScoreboardSkillItemVM(SkillObject skill, int initialValue)
		{
			this.Skill = skill;
			this._initialValue = initialValue;
			this._newValue = initialValue;
			this.SkillId = skill.StringId;
			this.Description = "(" + initialValue + ")";
			this.RefreshValues();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005D7D File Offset: 0x00003F7D
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Level = this.Skill.Name.ToString();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005D9C File Offset: 0x00003F9C
		public void UpdateSkill(int newValue)
		{
			this._newValue = newValue;
			this.Description = string.Concat(new object[]
			{
				"+",
				newValue - this._initialValue,
				"(",
				newValue,
				")"
			});
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005DF2 File Offset: 0x00003FF2
		public bool IsValid()
		{
			return this._newValue > this._initialValue;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00005E02 File Offset: 0x00004002
		// (set) Token: 0x06000178 RID: 376 RVA: 0x00005E0A File Offset: 0x0000400A
		[DataSourceProperty]
		public string Level
		{
			get
			{
				return this._level;
			}
			set
			{
				if (value != this._level)
				{
					this._level = value;
					base.OnPropertyChangedWithValue<string>(value, "Level");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00005E2D File Offset: 0x0000402D
		// (set) Token: 0x0600017A RID: 378 RVA: 0x00005E35 File Offset: 0x00004035
		[DataSourceProperty]
		public string SkillId
		{
			get
			{
				return this._imagePath;
			}
			set
			{
				if (value != this._imagePath)
				{
					this._imagePath = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillId");
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00005E58 File Offset: 0x00004058
		// (set) Token: 0x0600017C RID: 380 RVA: 0x00005E60 File Offset: 0x00004060
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x040000AA RID: 170
		public SkillObject Skill;

		// Token: 0x040000AB RID: 171
		private readonly int _initialValue;

		// Token: 0x040000AC RID: 172
		private int _newValue;

		// Token: 0x040000AD RID: 173
		private string _level;

		// Token: 0x040000AE RID: 174
		private string _imagePath;

		// Token: 0x040000AF RID: 175
		private string _description;
	}
}
