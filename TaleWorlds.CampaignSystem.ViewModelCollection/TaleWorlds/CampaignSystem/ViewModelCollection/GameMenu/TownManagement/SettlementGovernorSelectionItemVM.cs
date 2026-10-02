using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.TownManagement
{
	// Token: 0x020000AA RID: 170
	public class SettlementGovernorSelectionItemVM : ViewModel
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x00042404 File Offset: 0x00040604
		public Hero Governor { get; }

		// Token: 0x06001014 RID: 4116 RVA: 0x0004240C File Offset: 0x0004060C
		public SettlementGovernorSelectionItemVM(Hero governor, Action<SettlementGovernorSelectionItemVM> onSelection)
		{
			this.Governor = governor;
			this._onSelection = onSelection;
			if (governor != null)
			{
				this.Visual = new CharacterImageIdentifierVM(CampaignUIHelper.GetCharacterCode(this.Governor.CharacterObject, true));
				this.GovernorHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetHeroGovernorEffectsTooltip(this.Governor, Settlement.CurrentSettlement));
			}
			else
			{
				this.Visual = new CharacterImageIdentifierVM(null);
				this.GovernorHint = new BasicTooltipViewModel();
			}
			this.RefreshValues();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00042484 File Offset: 0x00040684
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this.Governor != null)
			{
				this.Name = this.Governor.Name.ToString();
				return;
			}
			this.Visual = new CharacterImageIdentifierVM(null);
			this.Name = new TextObject("{=koX9okuG}None", null).ToString();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000424D8 File Offset: 0x000406D8
		public void OnSelection()
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			Hero hero;
			if (currentSettlement == null)
			{
				hero = null;
			}
			else
			{
				Town town = currentSettlement.Town;
				hero = ((town != null) ? town.Governor : null);
			}
			Hero hero2 = hero;
			bool flag = this.Governor == null;
			if (hero2 != this.Governor && (!flag || hero2 != null))
			{
				ValueTuple<TextObject, TextObject> governorSelectionConfirmationPopupTexts = CampaignUIHelper.GetGovernorSelectionConfirmationPopupTexts(hero2, this.Governor, currentSettlement);
				InformationManager.ShowInquiry(new InquiryData(governorSelectionConfirmationPopupTexts.Item1.ToString(), governorSelectionConfirmationPopupTexts.Item2.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					this._onSelection(this);
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x00042589 File Offset: 0x00040789
		// (set) Token: 0x06001018 RID: 4120 RVA: 0x00042591 File Offset: 0x00040791
		[DataSourceProperty]
		public CharacterImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				if (value != this._visual)
				{
					this._visual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "Visual");
				}
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001019 RID: 4121 RVA: 0x000425AF File Offset: 0x000407AF
		// (set) Token: 0x0600101A RID: 4122 RVA: 0x000425B7 File Offset: 0x000407B7
		[DataSourceProperty]
		public BasicTooltipViewModel GovernorHint
		{
			get
			{
				return this._governorHint;
			}
			set
			{
				if (value != this._governorHint)
				{
					this._governorHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "GovernorHint");
				}
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x000425D5 File Offset: 0x000407D5
		// (set) Token: 0x0600101C RID: 4124 RVA: 0x000425DD File Offset: 0x000407DD
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

		// Token: 0x0400074A RID: 1866
		private readonly Action<SettlementGovernorSelectionItemVM> _onSelection;

		// Token: 0x0400074C RID: 1868
		private CharacterImageIdentifierVM _visual;

		// Token: 0x0400074D RID: 1869
		private string _name;

		// Token: 0x0400074E RID: 1870
		private BasicTooltipViewModel _governorHint;
	}
}
