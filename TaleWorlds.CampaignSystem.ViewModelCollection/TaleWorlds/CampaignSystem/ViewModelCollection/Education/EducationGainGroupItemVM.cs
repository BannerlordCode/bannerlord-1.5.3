using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F7 RID: 247
	public class EducationGainGroupItemVM : ViewModel
	{
		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00057660 File Offset: 0x00055860
		// (set) Token: 0x06001632 RID: 5682 RVA: 0x00057668 File Offset: 0x00055868
		public CharacterAttribute AttributeObj { get; private set; }

		// Token: 0x06001633 RID: 5683 RVA: 0x00057674 File Offset: 0x00055874
		public EducationGainGroupItemVM(CharacterAttribute attributeObj)
		{
			this.AttributeObj = attributeObj;
			this.Skills = new MBBindingList<EducationGainedSkillItemVM>();
			this.Attribute = new EducationGainedAttributeItemVM(this.AttributeObj);
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (!CampaignUIHelper.GetIsNavalSkill(skill) && skill.Attributes.FirstOrDefault<CharacterAttribute>() == this.AttributeObj && !this.Skills.Any<EducationGainedSkillItemVM>((EducationGainedSkillItemVM s) => s.SkillObj == skill))
					{
						this.Skills.Add(new EducationGainedSkillItemVM(skill));
					}
				}
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x00057758 File Offset: 0x00055958
		public void ResetValues()
		{
			this.Attribute.ResetValues();
			this.Skills.ApplyActionOnAllItems(delegate(EducationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001635 RID: 5685 RVA: 0x0005778F File Offset: 0x0005598F
		// (set) Token: 0x06001636 RID: 5686 RVA: 0x00057797 File Offset: 0x00055997
		[DataSourceProperty]
		public MBBindingList<EducationGainedSkillItemVM> Skills
		{
			get
			{
				return this._skills;
			}
			set
			{
				if (value != this._skills)
				{
					this._skills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationGainedSkillItemVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x000577B5 File Offset: 0x000559B5
		// (set) Token: 0x06001638 RID: 5688 RVA: 0x000577BD File Offset: 0x000559BD
		[DataSourceProperty]
		public EducationGainedAttributeItemVM Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				if (value != this._attribute)
				{
					this._attribute = value;
					base.OnPropertyChangedWithValue<EducationGainedAttributeItemVM>(value, "Attribute");
				}
			}
		}

		// Token: 0x04000A0D RID: 2573
		private MBBindingList<EducationGainedSkillItemVM> _skills;

		// Token: 0x04000A0E RID: 2574
		private EducationGainedAttributeItemVM _attribute;
	}
}
