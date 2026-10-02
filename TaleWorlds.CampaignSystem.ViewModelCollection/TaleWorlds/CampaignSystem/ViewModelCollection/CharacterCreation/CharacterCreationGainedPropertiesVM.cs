using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation
{
	// Token: 0x02000155 RID: 341
	public class CharacterCreationGainedPropertiesVM : ViewModel
	{
		// Token: 0x060020FC RID: 8444 RVA: 0x00076554 File Offset: 0x00074754
		public CharacterCreationGainedPropertiesVM(CharacterCreationManager characterCreationManager)
		{
			this._characterCreationManager = characterCreationManager;
			this._affectedAttributesMap = new Dictionary<CharacterAttribute, Tuple<int, int>>();
			this._affectedSkillMap = new Dictionary<SkillObject, Tuple<int, int>>();
			this.GainGroups = new MBBindingList<CharacterCreationGainGroupItemVM>();
			this.OtherSkills = new MBBindingList<CharacterCreationGainedSkillItemVM>();
			List<CharacterAttribute> list = Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this.GainGroups.Add(new CharacterCreationGainGroupItemVM(characterAttribute));
			}
			List<SkillObject> list2 = Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SkillObject skill = enumerator2.Current;
					Func<CharacterCreationGainedSkillItemVM, bool> <>9__1;
					if (!this.GainGroups.Any<CharacterCreationGainGroupItemVM>(delegate(CharacterCreationGainGroupItemVM attribute)
					{
						IEnumerable<CharacterCreationGainedSkillItemVM> skills = attribute.Skills;
						Func<CharacterCreationGainedSkillItemVM, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = (CharacterCreationGainedSkillItemVM attributeSkill) => attributeSkill.SkillId == skill.StringId);
						}
						return skills.Any<CharacterCreationGainedSkillItemVM>(func);
					}))
					{
						this.OtherSkills.Add(new CharacterCreationGainedSkillItemVM(skill));
					}
				}
			}
			this.GainedTraits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.UpdateValues();
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x00076690 File Offset: 0x00074890
		public void UpdateValues()
		{
			this._affectedAttributesMap.Clear();
			this._affectedSkillMap.Clear();
			this.GainGroups.ApplyActionOnAllItems(delegate(CharacterCreationGainGroupItemVM g)
			{
				g.ResetValues();
			});
			this.OtherSkills.ApplyActionOnAllItems(delegate(CharacterCreationGainedSkillItemVM s)
			{
				s.ResetValues();
			});
			this.PopulateInitialValues();
			this.PopulateGainedAttributeValues();
			this.PopulateGainedTraitValues();
			foreach (KeyValuePair<CharacterAttribute, Tuple<int, int>> keyValuePair in this._affectedAttributesMap)
			{
				this.GetItemFromAttribute(keyValuePair.Key).SetValue(keyValuePair.Value.Item1, keyValuePair.Value.Item2);
			}
			foreach (KeyValuePair<SkillObject, Tuple<int, int>> keyValuePair2 in this._affectedSkillMap)
			{
				this.GetItemFromSkill(keyValuePair2.Key).SetValue(keyValuePair2.Value.Item1, keyValuePair2.Value.Item2);
			}
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000767E8 File Offset: 0x000749E8
		private void PopulateInitialValues()
		{
			foreach (SkillObject skillObject in Skills.All)
			{
				int focus = Hero.MainHero.HeroDeveloper.GetFocus(skillObject);
				if (this._affectedSkillMap.ContainsKey(skillObject))
				{
					Tuple<int, int> tuple = this._affectedSkillMap[skillObject];
					this._affectedSkillMap[skillObject] = new Tuple<int, int>(tuple.Item1 + focus, 0);
				}
				else
				{
					this._affectedSkillMap.Add(skillObject, new Tuple<int, int>(focus, 0));
				}
			}
			foreach (CharacterAttribute characterAttribute in Attributes.All)
			{
				int attributeValue = Hero.MainHero.GetAttributeValue(characterAttribute);
				if (this._affectedAttributesMap.ContainsKey(characterAttribute))
				{
					Tuple<int, int> tuple2 = this._affectedAttributesMap[characterAttribute];
					this._affectedAttributesMap[characterAttribute] = new Tuple<int, int>(tuple2.Item1 + attributeValue, 0);
				}
				else
				{
					this._affectedAttributesMap.Add(characterAttribute, new Tuple<int, int>(attributeValue, 0));
				}
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x0007692C File Offset: 0x00074B2C
		private void PopulateGainedAttributeValues()
		{
			foreach (KeyValuePair<NarrativeMenu, NarrativeMenuOption> keyValuePair in this._characterCreationManager.SelectedOptions)
			{
				NarrativeMenu key = keyValuePair.Key;
				NarrativeMenuOption value = keyValuePair.Value;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				if (key == this._characterCreationManager.CurrentMenu)
				{
					num = value.Args.AttributeLevelToAdd;
				}
				else
				{
					num2 += value.Args.AttributeLevelToAdd;
				}
				if (value.Args.EffectedAttribute != null)
				{
					if (this._affectedAttributesMap.ContainsKey(value.Args.EffectedAttribute))
					{
						Tuple<int, int> tuple = this._affectedAttributesMap[value.Args.EffectedAttribute];
						this._affectedAttributesMap[value.Args.EffectedAttribute] = new Tuple<int, int>(tuple.Item1 + num2, tuple.Item2 + num);
					}
					else
					{
						this._affectedAttributesMap.Add(value.Args.EffectedAttribute, new Tuple<int, int>(num2, num));
					}
				}
				if (key == this._characterCreationManager.CurrentMenu)
				{
					num3 = value.Args.FocusToAdd;
				}
				else
				{
					num4 += value.Args.FocusToAdd;
				}
				foreach (SkillObject skillObject in value.Args.AffectedSkills)
				{
					if (this._affectedSkillMap.ContainsKey(skillObject))
					{
						Tuple<int, int> tuple2 = this._affectedSkillMap[skillObject];
						this._affectedSkillMap[skillObject] = new Tuple<int, int>(tuple2.Item1 + num4, tuple2.Item2 + num3);
					}
					else
					{
						this._affectedSkillMap.Add(skillObject, new Tuple<int, int>(num4, num3));
					}
				}
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00076B38 File Offset: 0x00074D38
		private void PopulateGainedTraitValues()
		{
			this.GainedTraits.Clear();
			foreach (KeyValuePair<NarrativeMenu, NarrativeMenuOption> keyValuePair in this._characterCreationManager.SelectedOptions)
			{
				NarrativeMenuOption value = keyValuePair.Value;
				if (value.Args.AffectedTraits != null && value.Args.AffectedTraits.Count > 0)
				{
					using (List<TraitObject>.Enumerator enumerator2 = value.Args.AffectedTraits.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							TraitObject effectedTrait = enumerator2.Current;
							if (this.GainedTraits.FirstOrDefault<EncyclopediaTraitItemVM>((EncyclopediaTraitItemVM t) => t.TraitId == effectedTrait.StringId) == null)
							{
								this.GainedTraits.Add(new EncyclopediaTraitItemVM(effectedTrait, 1));
							}
						}
					}
				}
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00076C44 File Offset: 0x00074E44
		private CharacterCreationGainedAttributeItemVM GetItemFromAttribute(CharacterAttribute attribute)
		{
			CharacterCreationGainGroupItemVM characterCreationGainGroupItemVM = this.GainGroups.SingleOrDefault<CharacterCreationGainGroupItemVM>((CharacterCreationGainGroupItemVM g) => g.AttributeObj == attribute);
			if (characterCreationGainGroupItemVM == null)
			{
				return null;
			}
			return characterCreationGainGroupItemVM.Attribute;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00076C80 File Offset: 0x00074E80
		private CharacterCreationGainedSkillItemVM GetItemFromSkill(SkillObject skill)
		{
			foreach (CharacterCreationGainGroupItemVM characterCreationGainGroupItemVM in this.GainGroups)
			{
				foreach (CharacterCreationGainedSkillItemVM characterCreationGainedSkillItemVM in characterCreationGainGroupItemVM.Skills)
				{
					if (characterCreationGainedSkillItemVM.SkillObj == skill)
					{
						return characterCreationGainedSkillItemVM;
					}
				}
			}
			foreach (CharacterCreationGainedSkillItemVM characterCreationGainedSkillItemVM2 in this.OtherSkills)
			{
				if (characterCreationGainedSkillItemVM2.SkillObj == skill)
				{
					return characterCreationGainedSkillItemVM2;
				}
			}
			return null;
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06002103 RID: 8451 RVA: 0x00076D50 File Offset: 0x00074F50
		// (set) Token: 0x06002104 RID: 8452 RVA: 0x00076D58 File Offset: 0x00074F58
		[DataSourceProperty]
		public MBBindingList<CharacterCreationGainGroupItemVM> GainGroups
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
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationGainGroupItemVM>>(value, "GainGroups");
				}
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06002105 RID: 8453 RVA: 0x00076D76 File Offset: 0x00074F76
		// (set) Token: 0x06002106 RID: 8454 RVA: 0x00076D7E File Offset: 0x00074F7E
		[DataSourceProperty]
		public MBBindingList<EncyclopediaTraitItemVM> GainedTraits
		{
			get
			{
				return this._gainedTraits;
			}
			set
			{
				if (value != this._gainedTraits)
				{
					this._gainedTraits = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaTraitItemVM>>(value, "GainedTraits");
				}
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06002107 RID: 8455 RVA: 0x00076D9C File Offset: 0x00074F9C
		// (set) Token: 0x06002108 RID: 8456 RVA: 0x00076DA4 File Offset: 0x00074FA4
		[DataSourceProperty]
		public MBBindingList<CharacterCreationGainedSkillItemVM> OtherSkills
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
					base.OnPropertyChangedWithValue<MBBindingList<CharacterCreationGainedSkillItemVM>>(value, "OtherSkills");
				}
			}
		}

		// Token: 0x04000F17 RID: 3863
		private readonly CharacterCreationManager _characterCreationManager;

		// Token: 0x04000F18 RID: 3864
		private readonly Dictionary<CharacterAttribute, Tuple<int, int>> _affectedAttributesMap;

		// Token: 0x04000F19 RID: 3865
		private readonly Dictionary<SkillObject, Tuple<int, int>> _affectedSkillMap;

		// Token: 0x04000F1A RID: 3866
		private MBBindingList<CharacterCreationGainGroupItemVM> _gainGroups;

		// Token: 0x04000F1B RID: 3867
		private MBBindingList<EncyclopediaTraitItemVM> _gainedTraits;

		// Token: 0x04000F1C RID: 3868
		private MBBindingList<CharacterCreationGainedSkillItemVM> _otherSkills;
	}
}
