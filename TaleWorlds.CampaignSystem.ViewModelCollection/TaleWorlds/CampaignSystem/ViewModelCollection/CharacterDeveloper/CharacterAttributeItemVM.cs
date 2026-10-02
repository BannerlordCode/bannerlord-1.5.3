using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper
{
	// Token: 0x02000146 RID: 326
	public class CharacterAttributeItemVM : ViewModel
	{
		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x00071A49 File Offset: 0x0006FC49
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x00071A51 File Offset: 0x0006FC51
		public CharacterAttribute AttributeType { get; private set; }

		// Token: 0x06001F5E RID: 8030 RVA: 0x00071A5C File Offset: 0x0006FC5C
		public CharacterAttributeItemVM(Hero hero, CharacterAttribute currAtt, CharacterDeveloperHeroItemVM developerVM, Action<CharacterAttributeItemVM> onInpectAttribute, Action<CharacterAttributeItemVM> onAddAttributePoint)
		{
			this._hero = hero;
			this._developer = this._hero.HeroDeveloper;
			this._characterVM = developerVM;
			this.AttributeType = currAtt;
			this._onInpectAttribute = onInpectAttribute;
			this._onAddAttributePoint = onAddAttributePoint;
			this._initialAttValue = this._characterVM.CharacterAttributes.GetPropertyValue(currAtt);
			this.AttributeValue = this._initialAttValue;
			this.BoundSkills = new MBBindingList<AttributeBoundSkillItemVM>();
			this.RefreshWithCurrentValues();
			this.RefreshValues();
			this.UnspentAttributePoints = this._characterVM.UnspentAttributePoints;
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x00071AF0 File Offset: 0x0006FCF0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.AttributeType.Abbreviation.ToString();
			string text = this.AttributeType.Description.ToString();
			GameTexts.SetVariable("STR1", text);
			GameTexts.SetVariable("ATTRIBUTE_NAME", this.AttributeType.Name);
			TextObject textObject = GameTexts.FindText("str_skill_attribute_bound_skills", null);
			textObject.SetTextVariable("IS_SOCIAL", (this.AttributeType == DefaultCharacterAttributes.Social) ? 1 : 0);
			GameTexts.SetVariable("STR2", textObject);
			this.Description = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			TextObject textObject2 = GameTexts.FindText("str_skill_attribute_increase_description", null);
			textObject2.SetTextVariable("IS_SOCIAL", (this.AttributeType == DefaultCharacterAttributes.Social) ? 1 : 0);
			GameTexts.SetVariable("NUMBER", this.UnspentAttributePoints);
			this.UnspentAttributePointsText = GameTexts.FindText("str_free_attribute_points", null).ToString();
			this.IncreaseHelpText = textObject2.ToString();
			this.BoundSkills.Clear();
			List<SkillObject> list = Skills.All.ToList<SkillObject>();
			list.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SkillObject skill = enumerator.Current;
					if (skill.Attributes.Contains(this.AttributeType) && !this.BoundSkills.Any<AttributeBoundSkillItemVM>((AttributeBoundSkillItemVM s) => s.SkillId == skill.StringId))
					{
						this.BoundSkills.Add(new AttributeBoundSkillItemVM(skill));
					}
				}
			}
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x00071CA0 File Offset: 0x0006FEA0
		public void ExecuteInspectAttribute()
		{
			Action<CharacterAttributeItemVM> onInpectAttribute = this._onInpectAttribute;
			if (onInpectAttribute == null)
			{
				return;
			}
			onInpectAttribute(this);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x00071CB3 File Offset: 0x0006FEB3
		public void ExecuteAddAttributePoint()
		{
			Action<CharacterAttributeItemVM> onAddAttributePoint = this._onAddAttributePoint;
			if (onAddAttributePoint != null)
			{
				onAddAttributePoint(this);
			}
			this.UnspentAttributePoints = this._characterVM.UnspentAttributePoints;
			this.RefreshWithCurrentValues();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x00071CDE File Offset: 0x0006FEDE
		public void Reset()
		{
			this.RefreshWithCurrentValues();
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x00071CE8 File Offset: 0x0006FEE8
		public void RefreshWithCurrentValues()
		{
			this.UnspentAttributePoints = this._characterVM.UnspentAttributePoints;
			this.AttributeValue = this._characterVM.CharacterAttributes.GetPropertyValue(this.AttributeType);
			this.CanAddPoint = this.AttributeValue < Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute && this._characterVM.UnspentAttributePoints > 0;
			this.IsAttributeAtMax = this.AttributeValue >= Campaign.Current.Models.CharacterDevelopmentModel.MaxAttribute;
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x00071D7C File Offset: 0x0006FF7C
		public void Commit()
		{
			for (int i = 0; i < this.AttributeValue - this._initialAttValue; i++)
			{
				this._developer.AddAttribute(this.AttributeType, 1, true);
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x06001F65 RID: 8037 RVA: 0x00071DB4 File Offset: 0x0006FFB4
		// (set) Token: 0x06001F66 RID: 8038 RVA: 0x00071DBC File Offset: 0x0006FFBC
		[DataSourceProperty]
		public MBBindingList<AttributeBoundSkillItemVM> BoundSkills
		{
			get
			{
				return this._boundSkills;
			}
			set
			{
				if (value != this._boundSkills)
				{
					this._boundSkills = value;
					base.OnPropertyChangedWithValue<MBBindingList<AttributeBoundSkillItemVM>>(value, "BoundSkills");
				}
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x00071DDA File Offset: 0x0006FFDA
		// (set) Token: 0x06001F68 RID: 8040 RVA: 0x00071DE2 File Offset: 0x0006FFE2
		[DataSourceProperty]
		public int AttributeValue
		{
			get
			{
				return this._atttributeValue;
			}
			set
			{
				if (value != this._atttributeValue)
				{
					this._atttributeValue = value;
					base.OnPropertyChangedWithValue(value, "AttributeValue");
				}
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x06001F69 RID: 8041 RVA: 0x00071E00 File Offset: 0x00070000
		// (set) Token: 0x06001F6A RID: 8042 RVA: 0x00071E08 File Offset: 0x00070008
		[DataSourceProperty]
		public int UnspentAttributePoints
		{
			get
			{
				return this._unspentAttributePoints;
			}
			set
			{
				if (value != this._unspentAttributePoints)
				{
					this._unspentAttributePoints = value;
					base.OnPropertyChangedWithValue(value, "UnspentAttributePoints");
					GameTexts.SetVariable("NUMBER", value);
					this.UnspentAttributePointsText = GameTexts.FindText("str_free_attribute_points", null).ToString();
				}
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x06001F6B RID: 8043 RVA: 0x00071E47 File Offset: 0x00070047
		// (set) Token: 0x06001F6C RID: 8044 RVA: 0x00071E4F File Offset: 0x0007004F
		[DataSourceProperty]
		public string UnspentAttributePointsText
		{
			get
			{
				return this._unspentAttributePointsText;
			}
			set
			{
				if (value != this._unspentAttributePointsText)
				{
					this._unspentAttributePointsText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnspentAttributePointsText");
				}
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x00071E72 File Offset: 0x00070072
		// (set) Token: 0x06001F6E RID: 8046 RVA: 0x00071E7A File Offset: 0x0007007A
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

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x06001F6F RID: 8047 RVA: 0x00071E9D File Offset: 0x0007009D
		// (set) Token: 0x06001F70 RID: 8048 RVA: 0x00071EA5 File Offset: 0x000700A5
		[DataSourceProperty]
		public string NameExtended
		{
			get
			{
				return this._nameExtended;
			}
			set
			{
				if (value != this._nameExtended)
				{
					this._nameExtended = value;
					base.OnPropertyChangedWithValue<string>(value, "NameExtended");
				}
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x06001F71 RID: 8049 RVA: 0x00071EC8 File Offset: 0x000700C8
		// (set) Token: 0x06001F72 RID: 8050 RVA: 0x00071ED0 File Offset: 0x000700D0
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

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x00071EF3 File Offset: 0x000700F3
		// (set) Token: 0x06001F74 RID: 8052 RVA: 0x00071EFB File Offset: 0x000700FB
		[DataSourceProperty]
		public string IncreaseHelpText
		{
			get
			{
				return this._increaseHelpText;
			}
			set
			{
				if (value != this._increaseHelpText)
				{
					this._increaseHelpText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncreaseHelpText");
				}
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x00071F1E File Offset: 0x0007011E
		// (set) Token: 0x06001F76 RID: 8054 RVA: 0x00071F26 File Offset: 0x00070126
		[DataSourceProperty]
		public bool IsInspecting
		{
			get
			{
				return this._isInspecting;
			}
			set
			{
				if (value != this._isInspecting)
				{
					this._isInspecting = value;
					base.OnPropertyChangedWithValue(value, "IsInspecting");
				}
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x00071F44 File Offset: 0x00070144
		// (set) Token: 0x06001F78 RID: 8056 RVA: 0x00071F4C File Offset: 0x0007014C
		[DataSourceProperty]
		public bool IsAttributeAtMax
		{
			get
			{
				return this._isAttributeAtMax;
			}
			set
			{
				if (value != this._isAttributeAtMax)
				{
					this._isAttributeAtMax = value;
					base.OnPropertyChangedWithValue(value, "IsAttributeAtMax");
				}
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x00071F6A File Offset: 0x0007016A
		// (set) Token: 0x06001F7A RID: 8058 RVA: 0x00071F72 File Offset: 0x00070172
		[DataSourceProperty]
		public bool CanAddPoint
		{
			get
			{
				return this._canAddPoint;
			}
			set
			{
				if (value != this._canAddPoint)
				{
					this._canAddPoint = value;
					base.OnPropertyChangedWithValue(value, "CanAddPoint");
				}
			}
		}

		// Token: 0x04000E5F RID: 3679
		private readonly Hero _hero;

		// Token: 0x04000E61 RID: 3681
		private readonly HeroDeveloper _developer;

		// Token: 0x04000E62 RID: 3682
		private readonly int _initialAttValue;

		// Token: 0x04000E63 RID: 3683
		private readonly Action<CharacterAttributeItemVM> _onInpectAttribute;

		// Token: 0x04000E64 RID: 3684
		private readonly Action<CharacterAttributeItemVM> _onAddAttributePoint;

		// Token: 0x04000E65 RID: 3685
		private readonly CharacterDeveloperHeroItemVM _characterVM;

		// Token: 0x04000E66 RID: 3686
		private int _atttributeValue;

		// Token: 0x04000E67 RID: 3687
		private int _unspentAttributePoints;

		// Token: 0x04000E68 RID: 3688
		private string _unspentAttributePointsText;

		// Token: 0x04000E69 RID: 3689
		private bool _canAddPoint;

		// Token: 0x04000E6A RID: 3690
		private bool _isInspecting;

		// Token: 0x04000E6B RID: 3691
		private bool _isAttributeAtMax;

		// Token: 0x04000E6C RID: 3692
		private string _name;

		// Token: 0x04000E6D RID: 3693
		private string _nameExtended;

		// Token: 0x04000E6E RID: 3694
		private string _description;

		// Token: 0x04000E6F RID: 3695
		private string _increaseHelpText;

		// Token: 0x04000E70 RID: 3696
		private MBBindingList<AttributeBoundSkillItemVM> _boundSkills;
	}
}
