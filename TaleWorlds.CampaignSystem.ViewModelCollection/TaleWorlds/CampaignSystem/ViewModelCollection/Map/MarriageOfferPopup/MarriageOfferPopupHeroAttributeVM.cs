using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup
{
	// Token: 0x02000038 RID: 56
	public class MarriageOfferPopupHeroAttributeVM : ViewModel
	{
		// Token: 0x06000586 RID: 1414 RVA: 0x0001E230 File Offset: 0x0001C430
		public MarriageOfferPopupHeroAttributeVM(Hero hero, CharacterAttribute attribute)
		{
			this._hero = hero;
			this._attribute = attribute;
			this.FillSkillsList();
			this.RefreshValues();
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001E254 File Offset: 0x0001C454
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = GameTexts.FindText("str_STR1_space_STR2", null);
			textObject.SetTextVariable("STR1", this._attribute.Name);
			TextObject textObject2 = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject2.SetTextVariable("STR", this._hero.GetAttributeValue(this._attribute));
			textObject.SetTextVariable("STR2", textObject2);
			this._attributeText = textObject.ToString();
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001E2CC File Offset: 0x0001C4CC
		private void FillSkillsList()
		{
			this._attributeSkills = new MBBindingList<EncyclopediaSkillVM>();
			using (List<SkillObject>.Enumerator enumerator = Skills.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (!CampaignUIHelper.GetIsNavalSkill(skill) && skill.Attributes.FirstOrDefault<CharacterAttribute>() == this._attribute && !this._attributeSkills.Any<EncyclopediaSkillVM>((EncyclopediaSkillVM s) => s.SkillId == skill.StringId))
					{
						this._attributeSkills.Add(new EncyclopediaSkillVM(skill, this._hero.GetSkillValue(skill)));
					}
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001E398 File Offset: 0x0001C598
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x0001E3A0 File Offset: 0x0001C5A0
		[DataSourceProperty]
		public string AttributeText
		{
			get
			{
				return this._attributeText;
			}
			set
			{
				if (value != this._attributeText)
				{
					this._attributeText = value;
					base.OnPropertyChangedWithValue<string>(value, "AttributeText");
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001E3C3 File Offset: 0x0001C5C3
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x0001E3CB File Offset: 0x0001C5CB
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> AttributeSkills
		{
			get
			{
				return this._attributeSkills;
			}
			set
			{
				if (value != this._attributeSkills)
				{
					this._attributeSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "AttributeSkills");
				}
			}
		}

		// Token: 0x0400025D RID: 605
		private readonly Hero _hero;

		// Token: 0x0400025E RID: 606
		private readonly CharacterAttribute _attribute;

		// Token: 0x0400025F RID: 607
		private string _attributeText;

		// Token: 0x04000260 RID: 608
		private MBBindingList<EncyclopediaSkillVM> _attributeSkills;
	}
}
