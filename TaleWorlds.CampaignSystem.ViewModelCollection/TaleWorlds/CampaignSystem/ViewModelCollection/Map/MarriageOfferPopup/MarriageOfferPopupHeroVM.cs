using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MarriageOfferPopup
{
	// Token: 0x02000039 RID: 57
	public class MarriageOfferPopupHeroVM : ViewModel
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0001E3E9 File Offset: 0x0001C5E9
		public Hero Hero { get; }

		// Token: 0x0600058E RID: 1422 RVA: 0x0001E3F1 File Offset: 0x0001C5F1
		public MarriageOfferPopupHeroVM(Hero hero)
		{
			this.Hero = hero;
			this.Model = new HeroViewModel(CharacterViewModel.StanceTypes.None);
			this.FillHeroInformation();
			this.CreateClanBanner();
			this.RefreshValues();
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001E420 File Offset: 0x0001C620
		public void Update()
		{
			TextObject textObject;
			if (!this._modelCreated && !CampaignUIHelper.IsHeroInformationHidden(this.Hero, out textObject))
			{
				this._modelCreated = true;
				this.CreateHeroModel();
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001E454 File Offset: 0x0001C654
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.EncyclopediaLinkWithName = this.Hero.EncyclopediaLinkWithName.ToString();
			this.AgeString = ((int)this.Hero.Age).ToString();
			this.OccupationString = CampaignUIHelper.GetHeroOccupationName(this.Hero);
			this.Relation = (int)this.Hero.GetRelationWithPlayer();
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001E4BC File Offset: 0x0001C6BC
		public override void OnFinalize()
		{
			BannerImageIdentifierVM clanBanner = this.ClanBanner;
			if (clanBanner != null)
			{
				clanBanner.OnFinalize();
			}
			HeroViewModel model = this.Model;
			if (model != null)
			{
				model.OnFinalize();
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

		// Token: 0x06000592 RID: 1426 RVA: 0x0001E5B4 File Offset: 0x0001C7B4
		public void ExecuteHeroLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.Hero.EncyclopediaLink);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001E5D0 File Offset: 0x0001C7D0
		public void ExecuteClanLink()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.Hero.Clan.EncyclopediaLink);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001E5F1 File Offset: 0x0001C7F1
		private void CreateClanBanner()
		{
			this.ClanName = this.Hero.Clan.Name.ToString();
			this.ClanBanner = new BannerImageIdentifierVM(this.Hero.ClanBanner, true);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0001E628 File Offset: 0x0001C828
		private void CreateHeroModel()
		{
			this.Model.FillFrom(this.Hero, -1, true, true);
			this.Model.SetEquipment(EquipmentIndex.ArmorItemEndSlot, default(EquipmentElement));
			this.Model.SetEquipment(EquipmentIndex.HorseHarness, default(EquipmentElement));
			this.Model.SetEquipment(EquipmentIndex.NumAllWeaponSlots, default(EquipmentElement));
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001E68C File Offset: 0x0001C88C
		private void FillHeroInformation()
		{
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

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x0001E82C File Offset: 0x0001CA2C
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x0001E834 File Offset: 0x0001CA34
		[DataSourceProperty]
		public string EncyclopediaLinkWithName
		{
			get
			{
				return this._encyclopediaLinkWithName;
			}
			set
			{
				if (value != this._encyclopediaLinkWithName)
				{
					this._encyclopediaLinkWithName = value;
					base.OnPropertyChangedWithValue<string>(value, "EncyclopediaLinkWithName");
				}
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001E857 File Offset: 0x0001CA57
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x0001E85F File Offset: 0x0001CA5F
		[DataSourceProperty]
		public string AgeString
		{
			get
			{
				return this._ageString;
			}
			set
			{
				if (value != this._ageString)
				{
					this._ageString = value;
					base.OnPropertyChangedWithValue<string>(value, "AgeString");
				}
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001E882 File Offset: 0x0001CA82
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x0001E88A File Offset: 0x0001CA8A
		[DataSourceProperty]
		public string OccupationString
		{
			get
			{
				return this._occupationString;
			}
			set
			{
				if (value != this._occupationString)
				{
					this._occupationString = value;
					base.OnPropertyChangedWithValue<string>(value, "OccupationString");
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001E8AD File Offset: 0x0001CAAD
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x0001E8B5 File Offset: 0x0001CAB5
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (value != this._relation)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001E8D3 File Offset: 0x0001CAD3
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0001E8DB File Offset: 0x0001CADB
		[DataSourceProperty]
		public string ClanName
		{
			get
			{
				return this._clanName;
			}
			set
			{
				if (value != this._clanName)
				{
					this._clanName = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanName");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001E8FE File Offset: 0x0001CAFE
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x0001E906 File Offset: 0x0001CB06
		[DataSourceProperty]
		public BannerImageIdentifierVM ClanBanner
		{
			get
			{
				return this._clanBanner;
			}
			set
			{
				if (value != this._clanBanner)
				{
					this._clanBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ClanBanner");
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001E924 File Offset: 0x0001CB24
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x0001E92C File Offset: 0x0001CB2C
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

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001E94A File Offset: 0x0001CB4A
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x0001E952 File Offset: 0x0001CB52
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

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001E970 File Offset: 0x0001CB70
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x0001E978 File Offset: 0x0001CB78
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001E996 File Offset: 0x0001CB96
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x0001E99E File Offset: 0x0001CB9E
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

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x0001E9BC File Offset: 0x0001CBBC
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
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

		// Token: 0x04000261 RID: 609
		private bool _modelCreated;

		// Token: 0x04000263 RID: 611
		private string _encyclopediaLinkWithName;

		// Token: 0x04000264 RID: 612
		private string _ageString;

		// Token: 0x04000265 RID: 613
		private string _occupationString;

		// Token: 0x04000266 RID: 614
		private int _relation;

		// Token: 0x04000267 RID: 615
		private string _clanName;

		// Token: 0x04000268 RID: 616
		private BannerImageIdentifierVM _clanBanner;

		// Token: 0x04000269 RID: 617
		private HeroViewModel _model;

		// Token: 0x0400026A RID: 618
		private MBBindingList<EncyclopediaTraitItemVM> _traits;

		// Token: 0x0400026B RID: 619
		private MBBindingList<MarriageOfferPopupHeroAttributeVM> _attributes;

		// Token: 0x0400026C RID: 620
		private MBBindingList<EncyclopediaSkillVM> _otherSkills;

		// Token: 0x0400026D RID: 621
		private bool _hasOtherSkills;
	}
}
