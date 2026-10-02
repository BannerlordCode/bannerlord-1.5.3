using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000F1 RID: 241
	public class EncyclopediaSkillVM : ViewModel
	{
		// Token: 0x060015F3 RID: 5619 RVA: 0x00056610 File Offset: 0x00054810
		public EncyclopediaSkillVM(SkillObject skill, int skillValue)
		{
			this._skill = skill;
			this.SkillValue = skillValue;
			this.SkillId = skill.StringId;
			this.RefreshValues();
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x00056638 File Offset: 0x00054838
		public override void RefreshValues()
		{
			base.RefreshValues();
			string name = this._skill.Name.ToString();
			string desc = this._skill.Description.ToString();
			this.Hint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("STR1", name);
				GameTexts.SetVariable("STR2", desc);
				return GameTexts.FindText("str_string_newline_string", null).ToString();
			});
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060015F5 RID: 5621 RVA: 0x00056694 File Offset: 0x00054894
		// (set) Token: 0x060015F6 RID: 5622 RVA: 0x0005669C File Offset: 0x0005489C
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

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x000566BA File Offset: 0x000548BA
		// (set) Token: 0x060015F8 RID: 5624 RVA: 0x000566C2 File Offset: 0x000548C2
		[DataSourceProperty]
		public int SkillValue
		{
			get
			{
				return this._skillValue;
			}
			set
			{
				if (value != this._skillValue)
				{
					this._skillValue = value;
					base.OnPropertyChangedWithValue(value, "SkillValue");
				}
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060015F9 RID: 5625 RVA: 0x000566E0 File Offset: 0x000548E0
		// (set) Token: 0x060015FA RID: 5626 RVA: 0x000566E8 File Offset: 0x000548E8
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

		// Token: 0x040009EE RID: 2542
		private readonly SkillObject _skill;

		// Token: 0x040009EF RID: 2543
		private string _skillId;

		// Token: 0x040009F0 RID: 2544
		private int _skillValue;

		// Token: 0x040009F1 RID: 2545
		private BasicTooltipViewModel _hint;
	}
}
