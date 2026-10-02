using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000F3 RID: 243
	public class EncyclopediaTroopTreeNodeVM : ViewModel
	{
		// Token: 0x06001604 RID: 5636 RVA: 0x000567E0 File Offset: 0x000549E0
		public EncyclopediaTroopTreeNodeVM(CharacterObject rootCharacter, CharacterObject activeCharacter, bool isAlternativeUpgrade, PerkObject alternativeUpgradePerk = null)
		{
			this.Branch = new MBBindingList<EncyclopediaTroopTreeNodeVM>();
			this.IsActiveUnit = rootCharacter == activeCharacter;
			this.IsAlternativeUpgrade = isAlternativeUpgrade;
			if (alternativeUpgradePerk != null && this.IsAlternativeUpgrade)
			{
				this.AlternativeUpgradeTooltip = new BasicTooltipViewModel(delegate
				{
					TextObject textObject = new TextObject("{=LVJKy6a8}This troop requires {PERK_NAME} ({PERK_SKILL}) perk to upgrade.", null);
					textObject.SetTextVariable("PERK_NAME", alternativeUpgradePerk.Name);
					textObject.SetTextVariable("PERK_SKILL", alternativeUpgradePerk.Skill.Name);
					return textObject.ToString();
				});
			}
			this.Unit = new EncyclopediaUnitVM(rootCharacter, this.IsActiveUnit);
			foreach (CharacterObject characterObject in rootCharacter.UpgradeTargets)
			{
				if (characterObject == rootCharacter)
				{
					Debug.FailedAssert("A character cannot be it's own upgrade target!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\Encyclopedia\\Items\\EncyclopediaTroopTreeNodeVM.cs", ".ctor", 36);
				}
				else if (Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject)).IsValidEncyclopediaItem(characterObject))
				{
					bool flag = rootCharacter.Culture.IsBandit && !characterObject.Culture.IsBandit;
					PerkObject perkObject;
					Campaign.Current.Models.PartyTroopUpgradeModel.DoesPartyHaveRequiredPerksForUpgrade(PartyBase.MainParty, rootCharacter, characterObject, out perkObject);
					this.Branch.Add(new EncyclopediaTroopTreeNodeVM(characterObject, activeCharacter, flag, perkObject));
				}
			}
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x00056906 File Offset: 0x00054B06
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Branch.ApplyActionOnAllItems(delegate(EncyclopediaTroopTreeNodeVM x)
			{
				x.RefreshValues();
			});
			this.Unit.RefreshValues();
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001606 RID: 5638 RVA: 0x00056943 File Offset: 0x00054B43
		// (set) Token: 0x06001607 RID: 5639 RVA: 0x0005694B File Offset: 0x00054B4B
		[DataSourceProperty]
		public bool IsActiveUnit
		{
			get
			{
				return this._isActiveUnit;
			}
			set
			{
				if (value != this._isActiveUnit)
				{
					this._isActiveUnit = value;
					base.OnPropertyChangedWithValue(value, "IsActiveUnit");
				}
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001608 RID: 5640 RVA: 0x00056969 File Offset: 0x00054B69
		// (set) Token: 0x06001609 RID: 5641 RVA: 0x00056971 File Offset: 0x00054B71
		[DataSourceProperty]
		public bool IsAlternativeUpgrade
		{
			get
			{
				return this._isAlternativeUpgrade;
			}
			set
			{
				if (value != this._isAlternativeUpgrade)
				{
					this._isAlternativeUpgrade = value;
					base.OnPropertyChangedWithValue(value, "IsAlternativeUpgrade");
				}
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x0600160A RID: 5642 RVA: 0x0005698F File Offset: 0x00054B8F
		// (set) Token: 0x0600160B RID: 5643 RVA: 0x00056997 File Offset: 0x00054B97
		[DataSourceProperty]
		public MBBindingList<EncyclopediaTroopTreeNodeVM> Branch
		{
			get
			{
				return this._branch;
			}
			set
			{
				if (value != this._branch)
				{
					this._branch = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaTroopTreeNodeVM>>(value, "Branch");
				}
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x0600160C RID: 5644 RVA: 0x000569B5 File Offset: 0x00054BB5
		// (set) Token: 0x0600160D RID: 5645 RVA: 0x000569BD File Offset: 0x00054BBD
		[DataSourceProperty]
		public EncyclopediaUnitVM Unit
		{
			get
			{
				return this._unit;
			}
			set
			{
				if (value != this._unit)
				{
					this._unit = value;
					base.OnPropertyChangedWithValue<EncyclopediaUnitVM>(value, "Unit");
				}
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x0600160E RID: 5646 RVA: 0x000569DB File Offset: 0x00054BDB
		// (set) Token: 0x0600160F RID: 5647 RVA: 0x000569E3 File Offset: 0x00054BE3
		[DataSourceProperty]
		public BasicTooltipViewModel AlternativeUpgradeTooltip
		{
			get
			{
				return this._alternativeUpgradeTooltip;
			}
			set
			{
				if (value != this._alternativeUpgradeTooltip)
				{
					this._alternativeUpgradeTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "AlternativeUpgradeTooltip");
				}
			}
		}

		// Token: 0x040009F6 RID: 2550
		private MBBindingList<EncyclopediaTroopTreeNodeVM> _branch;

		// Token: 0x040009F7 RID: 2551
		private EncyclopediaUnitVM _unit;

		// Token: 0x040009F8 RID: 2552
		private bool _isActiveUnit;

		// Token: 0x040009F9 RID: 2553
		private bool _isAlternativeUpgrade;

		// Token: 0x040009FA RID: 2554
		private BasicTooltipViewModel _alternativeUpgradeTooltip;
	}
}
