using System;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x020000FE RID: 254
	public class CraftingAvailableHeroItemVM : ViewModel
	{
		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x0005886D File Offset: 0x00056A6D
		public Hero Hero { get; }

		// Token: 0x0600169F RID: 5791 RVA: 0x00058878 File Offset: 0x00056A78
		public CraftingAvailableHeroItemVM(Hero hero, Action<CraftingAvailableHeroItemVM> onSelection)
		{
			this._onSelection = onSelection;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this.Hero = hero;
			this.HeroData = new HeroVM(this.Hero, false);
			this.Hint = new BasicTooltipViewModel(() => CampaignUIHelper.GetCraftingHeroTooltip(this.Hero, this._craftingOrder));
			this.CraftingPerks = new MBBindingList<CraftingPerkVM>();
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x000588DD File Offset: 0x00056ADD
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.HeroData.RefreshValues();
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x000588F0 File Offset: 0x00056AF0
		public void RefreshStamina()
		{
			this.CurrentStamina = (float)this._craftingBehavior.GetHeroCraftingStamina(this.Hero);
			this.MaxStamina = this._craftingBehavior.GetMaxHeroCraftingStamina(this.Hero);
			int num = (int)(this.CurrentStamina / (float)this.MaxStamina * 100f);
			GameTexts.SetVariable("NUMBER", num);
			this.StaminaPercentage = GameTexts.FindText("str_NUMBER_percent", null).ToString();
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00058963 File Offset: 0x00056B63
		public void RefreshOrderAvailability(CraftingOrder order)
		{
			this._craftingOrder = order;
			if (order != null)
			{
				this.IsDisabled = !order.IsOrderAvailableForHero(this.Hero);
				return;
			}
			this.IsDisabled = false;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0005898C File Offset: 0x00056B8C
		public void RefreshSkills()
		{
			this.SmithySkillLevel = this.Hero.GetSkillValue(DefaultSkills.Crafting);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x000589A4 File Offset: 0x00056BA4
		public void RefreshPerks()
		{
			this.CraftingPerks.Clear();
			foreach (PerkObject perkObject in PerkObject.All)
			{
				if (perkObject.Skill == DefaultSkills.Crafting && this.Hero.GetPerkValue(perkObject))
				{
					this.CraftingPerks.Add(new CraftingPerkVM(perkObject));
				}
			}
			this.PerksText = ((this.CraftingPerks.Count > 0) ? new TextObject("{=8lCWWK9G}Smithing Perks", null).ToString() : new TextObject("{=WHRq5Dp0}No Smithing Perks", null).ToString());
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00058A5C File Offset: 0x00056C5C
		public void ExecuteSelection()
		{
			Action<CraftingAvailableHeroItemVM> onSelection = this._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060016A6 RID: 5798 RVA: 0x00058A6F File Offset: 0x00056C6F
		// (set) Token: 0x060016A7 RID: 5799 RVA: 0x00058A77 File Offset: 0x00056C77
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x00058A95 File Offset: 0x00056C95
		// (set) Token: 0x060016A9 RID: 5801 RVA: 0x00058A9D File Offset: 0x00056C9D
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

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x00058ABB File Offset: 0x00056CBB
		// (set) Token: 0x060016AB RID: 5803 RVA: 0x00058AC3 File Offset: 0x00056CC3
		[DataSourceProperty]
		public HeroVM HeroData
		{
			get
			{
				return this._heroData;
			}
			set
			{
				if (value != this._heroData)
				{
					this._heroData = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "HeroData");
				}
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x00058AE1 File Offset: 0x00056CE1
		// (set) Token: 0x060016AD RID: 5805 RVA: 0x00058AE9 File Offset: 0x00056CE9
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

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x00058B07 File Offset: 0x00056D07
		// (set) Token: 0x060016AF RID: 5807 RVA: 0x00058B0F File Offset: 0x00056D0F
		[DataSourceProperty]
		public float CurrentStamina
		{
			get
			{
				return this._currentStamina;
			}
			set
			{
				if (value != this._currentStamina)
				{
					this._currentStamina = value;
					base.OnPropertyChangedWithValue(value, "CurrentStamina");
				}
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x060016B0 RID: 5808 RVA: 0x00058B2D File Offset: 0x00056D2D
		// (set) Token: 0x060016B1 RID: 5809 RVA: 0x00058B35 File Offset: 0x00056D35
		[DataSourceProperty]
		public int MaxStamina
		{
			get
			{
				return this._maxStamina;
			}
			set
			{
				if (value != this._maxStamina)
				{
					this._maxStamina = value;
					base.OnPropertyChangedWithValue(value, "MaxStamina");
				}
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x00058B53 File Offset: 0x00056D53
		// (set) Token: 0x060016B3 RID: 5811 RVA: 0x00058B5B File Offset: 0x00056D5B
		[DataSourceProperty]
		public string StaminaPercentage
		{
			get
			{
				return this._staminaPercentage;
			}
			set
			{
				if (value != this._staminaPercentage)
				{
					this._staminaPercentage = value;
					base.OnPropertyChangedWithValue<string>(value, "StaminaPercentage");
				}
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x060016B4 RID: 5812 RVA: 0x00058B7E File Offset: 0x00056D7E
		// (set) Token: 0x060016B5 RID: 5813 RVA: 0x00058B86 File Offset: 0x00056D86
		[DataSourceProperty]
		public int SmithySkillLevel
		{
			get
			{
				return this._smithySkillLevel;
			}
			set
			{
				if (value != this._smithySkillLevel)
				{
					this._smithySkillLevel = value;
					base.OnPropertyChangedWithValue(value, "SmithySkillLevel");
				}
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x00058BA4 File Offset: 0x00056DA4
		// (set) Token: 0x060016B7 RID: 5815 RVA: 0x00058BAC File Offset: 0x00056DAC
		[DataSourceProperty]
		public MBBindingList<CraftingPerkVM> CraftingPerks
		{
			get
			{
				return this._craftingPerks;
			}
			set
			{
				if (value != this._craftingPerks)
				{
					this._craftingPerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingPerkVM>>(value, "CraftingPerks");
				}
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00058BCA File Offset: 0x00056DCA
		// (set) Token: 0x060016B9 RID: 5817 RVA: 0x00058BD2 File Offset: 0x00056DD2
		[DataSourceProperty]
		public string PerksText
		{
			get
			{
				return this._perksText;
			}
			set
			{
				if (value != this._perksText)
				{
					this._perksText = value;
					base.OnPropertyChangedWithValue<string>(value, "PerksText");
				}
			}
		}

		// Token: 0x04000A4B RID: 2635
		private readonly Action<CraftingAvailableHeroItemVM> _onSelection;

		// Token: 0x04000A4C RID: 2636
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000A4D RID: 2637
		private CraftingOrder _craftingOrder;

		// Token: 0x04000A4E RID: 2638
		private HeroVM _heroData;

		// Token: 0x04000A4F RID: 2639
		private BasicTooltipViewModel _hint;

		// Token: 0x04000A50 RID: 2640
		private float _currentStamina;

		// Token: 0x04000A51 RID: 2641
		private int _maxStamina;

		// Token: 0x04000A52 RID: 2642
		private string _staminaPercentage;

		// Token: 0x04000A53 RID: 2643
		private bool _isDisabled;

		// Token: 0x04000A54 RID: 2644
		private bool _isSelected;

		// Token: 0x04000A55 RID: 2645
		private int _smithySkillLevel;

		// Token: 0x04000A56 RID: 2646
		private MBBindingList<CraftingPerkVM> _craftingPerks;

		// Token: 0x04000A57 RID: 2647
		private string _perksText;
	}
}
