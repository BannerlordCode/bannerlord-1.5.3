using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000147 RID: 327
	public class AttributeBoundSkillItemVM : ViewModel
	{
		// Token: 0x06001F7B RID: 8059 RVA: 0x00071F90 File Offset: 0x00070190
		public AttributeBoundSkillItemVM(SkillObject skill)
		{
			this.Name = skill.Name.ToString();
			this.SkillId = skill.StringId;
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x00071FB5 File Offset: 0x000701B5
		// (set) Token: 0x06001F7D RID: 8061 RVA: 0x00071FBD File Offset: 0x000701BD
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06001F7E RID: 8062 RVA: 0x00071FE0 File Offset: 0x000701E0
		// (set) Token: 0x06001F7F RID: 8063 RVA: 0x00071FE8 File Offset: 0x000701E8
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

		// Token: 0x04000E71 RID: 3697
		private string _name;

		// Token: 0x04000E72 RID: 3698
		private string _skillId;
	}
}
