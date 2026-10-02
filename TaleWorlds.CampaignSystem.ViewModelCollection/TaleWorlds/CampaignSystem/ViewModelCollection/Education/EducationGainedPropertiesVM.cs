using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Education
{
	// Token: 0x020000F6 RID: 246
	public class EducationGainedPropertiesVM : ViewModel
	{
		// Token: 0x06001627 RID: 5671 RVA: 0x00056DCC File Offset: 0x00054FCC
		public EducationGainedPropertiesVM(Hero child, int pageCount)
		{
			this._child = child;
			this._pageCount = pageCount;
			this._educationBehavior = Campaign.Current.GetCampaignBehavior<IEducationLogic>();
			this._affectedSkillFocusMap = new Dictionary<SkillObject, Tuple<int, int>>();
			this._affectedSkillValueMap = new Dictionary<SkillObject, Tuple<int, int>>();
			this._affectedAttributesMap = new Dictionary<CharacterAttribute, Tuple<int, int>>();
			this.GainGroups = new MBBindingList<EducationGainGroupItemVM>();
			this.OtherSkills = new MBBindingList<EducationGainedSkillItemVM>();
			List<CharacterAttribute> list = Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this.GainGroups.Add(new EducationGainGroupItemVM(characterAttribute));
			}
			List<SkillObject> list2 = Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SkillObject skill = enumerator2.Current;
					Func<EducationGainedSkillItemVM, bool> <>9__1;
					if (!this.GainGroups.Any<EducationGainGroupItemVM>(delegate(EducationGainGroupItemVM attribute)
					{
						IEnumerable<EducationGainedSkillItemVM> skills = attribute.Skills;
						Func<EducationGainedSkillItemVM, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = (EducationGainedSkillItemVM attributeSkill) => attributeSkill.SkillId == skill.StringId);
						}
						return skills.Any<EducationGainedSkillItemVM>(func);
					}))
					{
						this.OtherSkills.Add(new EducationGainedSkillItemVM(skill));
					}
				}
			}
			this.UpdateWithSelections(new List<string>(), -1);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00056F24 File Offset: 0x00055124
		internal void UpdateWithSelections(List<string> selectedOptions, int currentPageIndex)
		{
			this._affectedAttributesMap.Clear();
			this._affectedSkillFocusMap.Clear();
			this._affectedSkillValueMap.Clear();
			this.GainGroups.ApplyActionOnAllItems(delegate(EducationGainGroupItemVM g)
			{
				g.ResetValues();
			});
			this.OtherSkills.ApplyActionOnAllItems(delegate(EducationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
			this.PopulateInitialValues();
			this.PopulateGainedAttributeValues(selectedOptions, currentPageIndex);
			foreach (KeyValuePair<CharacterAttribute, Tuple<int, int>> keyValuePair in this._affectedAttributesMap)
			{
				this.GetItemFromAttribute(keyValuePair.Key).SetValue(keyValuePair.Value.Item1, keyValuePair.Value.Item2);
			}
			foreach (KeyValuePair<SkillObject, Tuple<int, int>> keyValuePair2 in this._affectedSkillFocusMap)
			{
				this.GetItemFromSkill(keyValuePair2.Key).SetFocusValue(keyValuePair2.Value.Item1, keyValuePair2.Value.Item2);
			}
			foreach (KeyValuePair<SkillObject, Tuple<int, int>> keyValuePair3 in this._affectedSkillValueMap)
			{
				this.GetItemFromSkill(keyValuePair3.Key).SetSkillValue(keyValuePair3.Value.Item1, keyValuePair3.Value.Item2);
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000570E8 File Offset: 0x000552E8
		private void PopulateInitialValues()
		{
			foreach (SkillObject skillObject in Skills.All)
			{
				int focus = this._child.HeroDeveloper.GetFocus(skillObject);
				if (this._affectedSkillFocusMap.ContainsKey(skillObject))
				{
					Tuple<int, int> tuple = this._affectedSkillFocusMap[skillObject];
					this._affectedSkillFocusMap[skillObject] = new Tuple<int, int>(tuple.Item1 + focus, 0);
				}
				else
				{
					this._affectedSkillFocusMap.Add(skillObject, new Tuple<int, int>(focus, 0));
				}
				int skillValue = this._child.GetSkillValue(skillObject);
				if (this._affectedSkillValueMap.ContainsKey(skillObject))
				{
					Tuple<int, int> tuple2 = this._affectedSkillValueMap[skillObject];
					this._affectedSkillValueMap[skillObject] = new Tuple<int, int>(tuple2.Item1 + skillValue, 0);
				}
				else
				{
					this._affectedSkillValueMap.Add(skillObject, new Tuple<int, int>(skillValue, 0));
				}
			}
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				int attributeValue = this._child.GetAttributeValue(characterAttribute);
				if (this._affectedAttributesMap.ContainsKey(characterAttribute))
				{
					Tuple<int, int> tuple3 = this._affectedAttributesMap[characterAttribute];
					this._affectedAttributesMap[characterAttribute] = new Tuple<int, int>(tuple3.Item1 + attributeValue, 0);
				}
				else
				{
					this._affectedAttributesMap.Add(characterAttribute, new Tuple<int, int>(attributeValue, 0));
				}
			}
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00057290 File Offset: 0x00055490
		private void PopulateGainedAttributeValues(List<string> selectedOptions, int currentPageIndex)
		{
			bool flag = currentPageIndex == this._pageCount - 1;
			for (int i = 0; i < selectedOptions.Count; i++)
			{
				string text = selectedOptions[i];
				TextObject textObject;
				TextObject textObject2;
				TextObject textObject3;
				ValueTuple<CharacterAttribute, int>[] array;
				ValueTuple<SkillObject, int>[] array2;
				ValueTuple<SkillObject, int>[] array3;
				EducationCampaignBehavior.EducationCharacterProperties[] array4;
				this._educationBehavior.GetOptionProperties(this._child, text, selectedOptions, out textObject, out textObject2, out textObject3, out array, out array2, out array3, out array4);
				bool flag2 = i == currentPageIndex;
				if (array != null)
				{
					foreach (ValueTuple<CharacterAttribute, int> valueTuple in array)
					{
						Tuple<int, int> tuple = this._affectedAttributesMap[valueTuple.Item1];
						int num = (flag2 ? valueTuple.Item2 : (flag ? (tuple.Item2 + valueTuple.Item2) : 0));
						int num2 = (flag2 ? tuple.Item1 : (flag ? tuple.Item1 : (tuple.Item1 + valueTuple.Item2)));
						this._affectedAttributesMap[valueTuple.Item1] = new Tuple<int, int>(num2, num);
					}
				}
				if (array2 != null)
				{
					foreach (ValueTuple<SkillObject, int> valueTuple2 in array2)
					{
						Tuple<int, int> tuple2 = this._affectedSkillValueMap[valueTuple2.Item1];
						int num3 = (flag2 ? valueTuple2.Item2 : (flag ? (tuple2.Item2 + valueTuple2.Item2) : 0));
						int num4 = (flag2 ? tuple2.Item1 : (flag ? tuple2.Item1 : (tuple2.Item1 + valueTuple2.Item2)));
						this._affectedSkillValueMap[valueTuple2.Item1] = new Tuple<int, int>(num4, num3);
					}
				}
				if (array3 != null)
				{
					foreach (ValueTuple<SkillObject, int> valueTuple3 in array3)
					{
						Tuple<int, int> tuple3 = this._affectedSkillFocusMap[valueTuple3.Item1];
						int num5 = (flag2 ? valueTuple3.Item2 : (flag ? (tuple3.Item2 + valueTuple3.Item2) : 0));
						int num6 = (flag2 ? tuple3.Item1 : (flag ? tuple3.Item1 : (tuple3.Item1 + valueTuple3.Item2)));
						num6 = Math.Min(num6, 5);
						num5 = Math.Min(num5, 5 - num6);
						this._affectedSkillFocusMap[valueTuple3.Item1] = new Tuple<int, int>(num6, num5);
					}
				}
			}
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00057508 File Offset: 0x00055708
		private EducationGainedAttributeItemVM GetItemFromAttribute(CharacterAttribute attribute)
		{
			EducationGainGroupItemVM educationGainGroupItemVM = this.GainGroups.SingleOrDefault<EducationGainGroupItemVM>((EducationGainGroupItemVM g) => g.AttributeObj == attribute);
			if (educationGainGroupItemVM == null)
			{
				return null;
			}
			return educationGainGroupItemVM.Attribute;
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x00057544 File Offset: 0x00055744
		private EducationGainedSkillItemVM GetItemFromSkill(SkillObject skill)
		{
			foreach (EducationGainGroupItemVM educationGainGroupItemVM in this.GainGroups)
			{
				foreach (EducationGainedSkillItemVM educationGainedSkillItemVM in educationGainGroupItemVM.Skills)
				{
					if (educationGainedSkillItemVM.SkillObj == skill)
					{
						return educationGainedSkillItemVM;
					}
				}
			}
			foreach (EducationGainedSkillItemVM educationGainedSkillItemVM2 in this.OtherSkills)
			{
				if (educationGainedSkillItemVM2.SkillObj == skill)
				{
					return educationGainedSkillItemVM2;
				}
			}
			return null;
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x00057614 File Offset: 0x00055814
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x0005761C File Offset: 0x0005581C
		[DataSourceProperty]
		public MBBindingList<EducationGainGroupItemVM> GainGroups
		{
			get
			{
				return this._gainGroups;
			}
			set
			{
				if (value != this._gainGroups)
				{
					this._gainGroups = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationGainGroupItemVM>>(value, "GainGroups");
				}
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x0600162F RID: 5679 RVA: 0x0005763A File Offset: 0x0005583A
		// (set) Token: 0x06001630 RID: 5680 RVA: 0x00057642 File Offset: 0x00055842
		[DataSourceProperty]
		public MBBindingList<EducationGainedSkillItemVM> OtherSkills
		{
			get
			{
				return this._otherSkills;
			}
			set
			{
				if (value != this._otherSkills)
				{
					this._otherSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<EducationGainedSkillItemVM>>(value, "OtherSkills");
				}
			}
		}

		// Token: 0x04000A04 RID: 2564
		private readonly Hero _child;

		// Token: 0x04000A05 RID: 2565
		private readonly int _pageCount;

		// Token: 0x04000A06 RID: 2566
		private readonly IEducationLogic _educationBehavior;

		// Token: 0x04000A07 RID: 2567
		private readonly Dictionary<CharacterAttribute, Tuple<int, int>> _affectedAttributesMap;

		// Token: 0x04000A08 RID: 2568
		private readonly Dictionary<SkillObject, Tuple<int, int>> _affectedSkillFocusMap;

		// Token: 0x04000A09 RID: 2569
		private readonly Dictionary<SkillObject, Tuple<int, int>> _affectedSkillValueMap;

		// Token: 0x04000A0A RID: 2570
		private MBBindingList<EducationGainGroupItemVM> _gainGroups;

		// Token: 0x04000A0B RID: 2571
		private MBBindingList<EducationGainedSkillItemVM> _otherSkills;
	}
}
