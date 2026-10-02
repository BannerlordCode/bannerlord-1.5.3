using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.HeirSelectionPopup
{
	// Token: 0x02000066 RID: 102
	public class HeirSelectionPopupHeroVM : ViewModel
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x000231F8 File Offset: 0x000213F8
		public Hero Hero { get; }

		// Token: 0x0600073B RID: 1851 RVA: 0x00023200 File Offset: 0x00021400
		public HeirSelectionPopupHeroVM(Hero hero)
		{
			this.Hero = hero;
			this.FillHeroInformation();
			this.CreateImageIdentifier();
			this.CreateHeroModel();
			this.RefreshValues();
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x00023228 File Offset: 0x00021428
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Hero.Name.ToString();
			this.Culture = this.Hero.Culture.Name.ToString();
			this.Occupation = CampaignUIHelper.GetHeroOccupationName(this.Hero);
			this.RelationToMainHero = CampaignUIHelper.GetHeroRelationToHeroText(this.Hero, Hero.MainHero, true).ToString();
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002329C File Offset: 0x0002149C
		public override void OnFinalize()
		{
			HeroViewModel model = this.Model;
			if (model != null)
			{
				model.OnFinalize();
			}
			CharacterImageIdentifierVM imageIdentifier = this.ImageIdentifier;
			if (imageIdentifier != null)
			{
				imageIdentifier.OnFinalize();
			}
			MBBindingList<EncyclopediaTraitItemVM> traits = this.Traits;
			if (traits != null)
			{
				traits.ApplyActionOnAllItems(delegate(EncyclopediaTraitItemVM x)
				{
					x.OnFinalize();
				});
			}
			MBBindingList<EncyclopediaTraitItemVM> traits2 = this.Traits;
			if (traits2 != null)
			{
				traits2.Clear();
			}
			MBBindingList<MarriageOfferPopupHeroAttributeVM> attributes = this.Attributes;
			if (attributes != null)
			{
				attributes.ApplyActionOnAllItems(delegate(MarriageOfferPopupHeroAttributeVM x)
				{
					x.OnFinalize();
				});
			}
			MBBindingList<MarriageOfferPopupHeroAttributeVM> attributes2 = this.Attributes;
			if (attributes2 != null)
			{
				attributes2.Clear();
			}
			MBBindingList<EncyclopediaSkillVM> otherSkills = this.OtherSkills;
			if (otherSkills != null)
			{
				otherSkills.ApplyActionOnAllItems(delegate(EncyclopediaSkillVM x)
				{
					x.OnFinalize();
				});
			}
			MBBindingList<EncyclopediaSkillVM> otherSkills2 = this.OtherSkills;
			if (otherSkills2 != null)
			{
				otherSkills2.Clear();
			}
			base.OnFinalize();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00023394 File Offset: 0x00021594
		private void CreateImageIdentifier()
		{
			this.ImageIdentifier = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this.Hero.CharacterObject));
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x000233B4 File Offset: 0x000215B4
		private void CreateHeroModel()
		{
			this.Model = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.Model.FillFrom(this.Hero, -1, false, true);
			this.Model.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.Model.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.Model.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00023424 File Offset: 0x00021624
		private void FillHeroInformation()
		{
			this.Age = (int)this.Hero.Age;
			this.Traits = new MBBindingList<EncyclopediaTraitItemVM>();
			this.Attributes = new MBBindingList<MarriageOfferPopupHeroAttributeVM>();
			this.OtherSkills = new MBBindingList<EncyclopediaSkillVM>();
			List<CharacterAttribute> list = TaleWorlds.CampaignSystem.Extensions.Attributes.All.ToList<CharacterAttribute>();
			list.Sort(CampaignUIHelper.CharacterAttributeComparerInstance);
			foreach (CharacterAttribute characterAttribute in list)
			{
				this.Attributes.Add(new MarriageOfferPopupHeroAttributeVM(this.Hero, characterAttribute));
			}
			List<SkillObject> list2 = Skills.All.ToList<SkillObject>();
			list2.Sort(CampaignUIHelper.SkillObjectComparerInstance);
			using (List<SkillObject>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					SkillObject skill = enumerator2.Current;
					Func<EncyclopediaSkillVM, bool> <>9__1;
					if (!this.Attributes.Any<MarriageOfferPopupHeroAttributeVM>(delegate(MarriageOfferPopupHeroAttributeVM attribute)
					{
						IEnumerable<EncyclopediaSkillVM> attributeSkills = attribute.AttributeSkills;
						Func<EncyclopediaSkillVM, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = (EncyclopediaSkillVM attributeSkill) => attributeSkill.SkillId == skill.StringId);
						}
						return attributeSkills.Any<EncyclopediaSkillVM>(func);
					}))
					{
						this.OtherSkills.Add(new EncyclopediaSkillVM(skill, this.Hero.GetSkillValue(skill)));
					}
				}
			}
			this.HasOtherSkills = this.OtherSkills.Count > 0;
			foreach (TraitObject traitObject in CampaignUIHelper.GetHeroTraits())
			{
				if (this.Hero.GetTraitLevel(traitObject) != 0)
				{
					this.Traits.Add(new EncyclopediaTraitItemVM(traitObject, this.Hero));
				}
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x000235D4 File Offset: 0x000217D4
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x000235DC File Offset: 0x000217DC
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x000235FF File Offset: 0x000217FF
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x00023607 File Offset: 0x00021807
		[DataSourceProperty]
		public int Age
		{
			get
			{
				return this._age;
			}
			set
			{
				if (value != this._age)
				{
					this._age = value;
					base.OnPropertyChangedWithValue(value, "Age");
				}
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x00023625 File Offset: 0x00021825
		// (set) Token: 0x06000746 RID: 1862 RVA: 0x0002362D File Offset: 0x0002182D
		[DataSourceProperty]
		public string Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value != this._culture)
				{
					this._culture = value;
					base.OnPropertyChangedWithValue<string>(value, "Culture");
				}
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x00023650 File Offset: 0x00021850
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x00023658 File Offset: 0x00021858
		[DataSourceProperty]
		public string Occupation
		{
			get
			{
				return this._occupation;
			}
			set
			{
				if (value != this._occupation)
				{
					this._occupation = value;
					base.OnPropertyChangedWithValue<string>(value, "Occupation");
				}
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0002367B File Offset: 0x0002187B
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x00023683 File Offset: 0x00021883
		[DataSourceProperty]
		public string RelationToMainHero
		{
			get
			{
				return this._relationToMainHero;
			}
			set
			{
				if (value != this._relationToMainHero)
				{
					this._relationToMainHero = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationToMainHero");
				}
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x000236A6 File Offset: 0x000218A6
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x000236AE File Offset: 0x000218AE
		[DataSourceProperty]
		public HeroViewModel Model
		{
			get
			{
				return this._model;
			}
			set
			{
				if (value != this._model)
				{
					this._model = value;
					base.OnPropertyChangedWithValue<HeroViewModel>(value, "Model");
				}
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x000236CC File Offset: 0x000218CC
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x000236D4 File Offset: 0x000218D4
		[DataSourceProperty]
		public CharacterImageIdentifierVM ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (value != this._imageIdentifier)
				{
					this._imageIdentifier = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x000236F2 File Offset: 0x000218F2
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x000236FA File Offset: 0x000218FA
		[DataSourceProperty]
		public MBBindingList<EncyclopediaTraitItemVM> Traits
		{
			get
			{
				return this._traits;
			}
			set
			{
				if (value != this._traits)
				{
					this._traits = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaTraitItemVM>>(value, "Traits");
				}
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00023718 File Offset: 0x00021918
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x00023720 File Offset: 0x00021920
		[DataSourceProperty]
		public MBBindingList<MarriageOfferPopupHeroAttributeVM> Attributes
		{
			get
			{
				return this._attributes;
			}
			set
			{
				if (value != this._attributes)
				{
					this._attributes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MarriageOfferPopupHeroAttributeVM>>(value, "Attributes");
				}
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0002373E File Offset: 0x0002193E
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x00023746 File Offset: 0x00021946
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00023764 File Offset: 0x00021964
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0002376C File Offset: 0x0002196C
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSkillVM> OtherSkills
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
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSkillVM>>(value, "OtherSkills");
				}
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0002378A File Offset: 0x0002198A
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x00023792 File Offset: 0x00021992
		[DataSourceProperty]
		public bool HasOtherSkills
		{
			get
			{
				return this._hasOtherSkills;
			}
			set
			{
				if (value != this._hasOtherSkills)
				{
					this._hasOtherSkills = value;
					base.OnPropertyChangedWithValue(value, "HasOtherSkills");
				}
			}
		}

		// Token: 0x0400031D RID: 797
		private string _name;

		// Token: 0x0400031E RID: 798
		private int _age;

		// Token: 0x0400031F RID: 799
		private string _culture;

		// Token: 0x04000320 RID: 800
		private string _occupation;

		// Token: 0x04000321 RID: 801
		private string _relationToMainHero;

		// Token: 0x04000322 RID: 802
		private HeroViewModel _model;

		// Token: 0x04000323 RID: 803
		private CharacterImageIdentifierVM _imageIdentifier;

		// Token: 0x04000324 RID: 804
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x04000325 RID: 805
		private MBBindingList<MarriageOfferPopupHeroAttributeVM> _attributes;

		// Token: 0x04000326 RID: 806
		private bool _isSelected;

		// Token: 0x04000327 RID: 807
		private MBBindingList<EncyclopediaSkillVM> _otherSkills;

		// Token: 0x04000328 RID: 808
		private bool _hasOtherSkills;
	}
}
