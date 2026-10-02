using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000156 RID: 342
	public class CharacterCreationGainGroupItemVM : ViewModel
	{
		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x00076DC2 File Offset: 0x00074FC2
		// (set) Token: 0x0600210A RID: 8458 RVA: 0x00076DCA File Offset: 0x00074FCA
		public CharacterAttribute AttributeObj { get; private set; }

		// Token: 0x0600210B RID: 8459 RVA: 0x00076DD4 File Offset: 0x00074FD4
		public CharacterCreationGainGroupItemVM(CharacterAttribute attributeObj)
		{
			this.AttributeObj = attributeObj;
			this.Skills = new MBBindingList<CharacterCreationGainedSkillItemVM>();
			this.Attribute = new CharacterCreationGainedAttributeItemVM(this.AttributeObj);
			List<SkillObject> list = TaleWorlds.CampaignSystem.Extensions.Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (!CampaignUIHelper.GetIsNavalSkill(skill) && skill.Attributes.FirstOrDefault<CharacterAttribute>() == attributeObj && !this.Skills.Any<CharacterCreationGainedSkillItemVM>((CharacterCreationGainedSkillItemVM s) => s.SkillObj == skill))
					{
						this.Skills.Add(new CharacterCreationGainedSkillItemVM(skill));
					}
				}
			}
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00076EB4 File Offset: 0x000750B4
		public void ResetValues()
		{
			this.Attribute.ResetValues();
			this.Skills.ApplyActionOnAllItems(delegate(CharacterCreationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x00076EEB File Offset: 0x000750EB
		// (set) Token: 0x0600210E RID: 8462 RVA: 0x00076EF3 File Offset: 0x000750F3
		[DataSourceProperty]
		public MBBindingList<CharacterCreationGainedSkillItemVM> Skills
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
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationGainedSkillItemVM>>(value, "Skills");
				}
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x00076F11 File Offset: 0x00075111
		// (set) Token: 0x06002110 RID: 8464 RVA: 0x00076F19 File Offset: 0x00075119
		[DataSourceProperty]
		public CharacterCreationGainedAttributeItemVM Attribute
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
					base.OnPropertyChangedWithValue<CharacterCreationGainedAttributeItemVM>(value, "Attribute");
				}
			}
		}

		// Token: 0x04000F1E RID: 3870
		private MBBindingList<CharacterCreationGainedSkillItemVM> _skills;

		// Token: 0x04000F1F RID: 3871
		private CharacterCreationGainedAttributeItemVM _attribute;
	}
}
