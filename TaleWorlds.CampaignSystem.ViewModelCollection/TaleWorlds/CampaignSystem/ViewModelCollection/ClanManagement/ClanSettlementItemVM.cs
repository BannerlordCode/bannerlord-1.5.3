using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement
{
	// Token: 0x02000132 RID: 306
	public class ClanSettlementItemVM : ViewModel
	{
		// Token: 0x06001C86 RID: 7302 RVA: 0x00067F18 File Offset: 0x00066118
		public ClanSettlementItemVM(Settlement settlement, Action<ClanSettlementItemVM> onSelection, Action onShowSendMembers, ITeleportationCampaignBehavior teleportationBehavior)
		{
			this.Settlement = settlement;
			this._onSelection = onSelection;
			this._onShowSendMembers = onShowSendMembers;
			this._teleportationBehavior = teleportationBehavior;
			this.IsFortification = settlement.IsFortification;
			SettlementComponent settlementComponent = settlement.SettlementComponent;
			this.FileName = ((settlementComponent == null) ? "placeholder" : (settlementComponent.BackgroundMeshName + "_t"));
			this.ItemProperties = new MBBindingList<SelectableFiefItemPropertyVM>();
			this.ProfitItemProperties = new MBBindingList<ProfitItemPropertyVM>();
			this.TotalProfit = new ProfitItemPropertyVM(GameTexts.FindText("str_profit", null).ToString(), 0, ProfitItemPropertyVM.PropertyType.None, null, null);
			this.ImageName = ((settlementComponent != null) ? settlementComponent.WaitMeshName : "");
			this.VillagesOwned = new MBBindingList<ClanSettlementItemVM>();
			this.Notables = new MBBindingList<HeroVM>();
			this.Members = new MBBindingList<HeroVM>();
			this._patrolsBehavior = Campaign.Current.GetCampaignBehavior<IPatrolPartiesCampaignBehavior>();
			this.RefreshValues();
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00068000 File Offset: 0x00066200
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.VillagesText = GameTexts.FindText("str_villages", null).ToString();
			this.NotablesText = GameTexts.FindText("str_center_notables", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.Name = this.Settlement.Name.ToString();
			this.UpdateProperties();
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00068071 File Offset: 0x00066271
		protected virtual ClanSettlementItemVM CreateSettlementItem(Settlement settlement, Action<ClanSettlementItemVM> onSelection, Action onShowSendMembers, ITeleportationCampaignBehavior teleportationBehavior)
		{
			return new ClanSettlementItemVM(settlement, onSelection, onShowSendMembers, teleportationBehavior);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x0006807D File Offset: 0x0006627D
		public void OnSettlementSelection()
		{
			this._onSelection(this);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x0006808B File Offset: 0x0006628B
		public void ExecuteLink()
		{
			MBInformationManager.HideInformations();
			Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000680AC File Offset: 0x000662AC
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000680B3 File Offset: 0x000662B3
		public void ExecuteOpenTooltip()
		{
			InformationManager.ShowTooltip(typeof(Settlement), new object[] { this.Settlement });
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000680D3 File Offset: 0x000662D3
		public void ExecuteSendMembers()
		{
			Action onShowSendMembers = this._onShowSendMembers;
			if (onShowSendMembers == null)
			{
				return;
			}
			onShowSendMembers();
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000680E5 File Offset: 0x000662E5
		private void OnGovernorChanged(Hero oldHero, Hero newHero)
		{
			ChangeGovernorAction.Apply(this.Settlement.Town, newHero);
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000680F8 File Offset: 0x000662F8
		private bool IsGovernorAssignable(Hero oldHero, Hero newHero)
		{
			return newHero.IsActive && newHero.GovernorOf == null;
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x00068110 File Offset: 0x00066310
		protected virtual void UpdateProperties()
		{
			this.ItemProperties.Clear();
			this.VillagesOwned.Clear();
			this.Notables.Clear();
			this.Members.Clear();
			foreach (Village village in this.Settlement.BoundVillages)
			{
				this.VillagesOwned.Add(this.CreateSettlementItem(village.Settlement, null, null, null));
			}
			this.HasNotables = !this.Settlement.Notables.IsEmpty<Hero>();
			foreach (Hero hero in this.Settlement.Notables)
			{
				this.Notables.Add(new HeroVM(hero, false));
			}
			foreach (Hero hero2 in this.Settlement.HeroesWithoutParty.Where<Hero>((Hero h) => h.Clan == Clan.PlayerClan))
			{
				this.Members.Add(new HeroVM(hero2, false));
			}
			this.HasGovernor = false;
			if (!this.Settlement.IsVillage)
			{
				Town town = this.Settlement.Town;
				Hero hero3 = ((((town != null) ? town.Governor : null) != null) ? this.Settlement.Town.Governor : CampaignUIHelper.GetTeleportingGovernor(this.Settlement, this._teleportationBehavior));
				this.HasGovernor = hero3 != null;
				this.Governor = (this.HasGovernor ? new HeroVM(hero3, false) : null);
			}
			this.IsFortification = this.Settlement.IsFortification;
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownWallsTooltip(this.Settlement.Town));
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_walls", null).ToString(), this.Settlement.Town.GetWallLevel().ToString(), 0, SelectableItemPropertyVM.PropertyType.Wall, basicTooltipViewModel, false));
			}
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel2 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownGarrisonTooltip(this.Settlement.Town));
				int num = (int)SettlementHelper.GetGarrisonChangeExplainedNumber(this.Settlement.Town).ResultNumber;
				Collection<SelectableFiefItemPropertyVM> itemProperties = this.ItemProperties;
				string text = GameTexts.FindText("str_garrison", null).ToString();
				MobileParty garrisonParty = this.Settlement.Town.GarrisonParty;
				itemProperties.Add(new SelectableFiefItemPropertyVM(text, ((garrisonParty != null) ? garrisonParty.Party.NumberOfAllMembers.ToString() : null) ?? "0", num, SelectableItemPropertyVM.PropertyType.Garrison, basicTooltipViewModel2, false));
			}
			int num2 = (int)this.Settlement.Militia;
			List<TooltipProperty> militiaHint = (this.Settlement.IsVillage ? CampaignUIHelper.GetVillageMilitiaTooltip(this.Settlement.Village) : CampaignUIHelper.GetTownMilitiaTooltip(this.Settlement.Town));
			int num3 = ((this.Settlement.Town != null) ? ((int)this.Settlement.Town.MilitiaChange) : ((int)this.Settlement.Village.MilitiaChange));
			this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_militia", null).ToString(), num2.ToString(), num3, SelectableItemPropertyVM.PropertyType.Militia, new BasicTooltipViewModel(() => militiaHint), false));
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel3 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownFoodTooltip(this.Settlement.Town));
				int num4 = (int)this.Settlement.Town.FoodChange;
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_food_stocks", null).ToString(), ((int)this.Settlement.Town.FoodStocks).ToString(), num4, SelectableItemPropertyVM.PropertyType.Food, basicTooltipViewModel3, false));
			}
			if (this.Settlement.IsFortification)
			{
				int num5 = ((this.Settlement.Town != null) ? ((int)this.Settlement.Town.ProsperityChange) : ((int)this.Settlement.Village.HearthChange));
				BasicTooltipViewModel basicTooltipViewModel4;
				if (this.Settlement.Town != null)
				{
					basicTooltipViewModel4 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownProsperityTooltip(this.Settlement.Town));
				}
				else
				{
					basicTooltipViewModel4 = new BasicTooltipViewModel(() => CampaignUIHelper.GetVillageProsperityTooltip(this.Settlement.Village));
				}
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_prosperity", null).ToString(), string.Format("{0:0.##}", this.Settlement.Town.Prosperity), num5, SelectableItemPropertyVM.PropertyType.Prosperity, basicTooltipViewModel4, false));
			}
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel5 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownLoyaltyTooltip(this.Settlement.Town));
				int num6 = (int)this.Settlement.Town.LoyaltyChange;
				bool flag = this.Settlement.IsTown && this.Settlement.Town.Loyalty < (float)Campaign.Current.Models.SettlementLoyaltyModel.RebelliousStateStartLoyaltyThreshold;
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_loyalty", null).ToString(), string.Format("{0:0.#}", this.Settlement.Town.Loyalty), num6, SelectableItemPropertyVM.PropertyType.Loyalty, basicTooltipViewModel5, flag));
			}
			if (this.Settlement.Town != null)
			{
				BasicTooltipViewModel basicTooltipViewModel6 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownSecurityTooltip(this.Settlement.Town));
				int num7 = (int)this.Settlement.Town.SecurityChange;
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_security", null).ToString(), string.Format("{0:0.#}", this.Settlement.Town.Security), num7, SelectableItemPropertyVM.PropertyType.Security, basicTooltipViewModel6, false));
			}
			if (this.Settlement.IsTown)
			{
				BasicTooltipViewModel basicTooltipViewModel7 = new BasicTooltipViewModel(() => CampaignUIHelper.GetTownPatrolTooltip(this.Settlement.Town));
				this.ItemProperties.Add(new SelectableFiefItemPropertyVM(GameTexts.FindText("str_patrol", null).ToString(), this._patrolsBehavior.GetSettlementPatrolStatus(this.Settlement).ToString(), 0, SelectableItemPropertyVM.PropertyType.Patrol, basicTooltipViewModel7, false));
			}
			TextObject textObject;
			this.IsSendMembersEnabled = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject);
			TextObject textObject2 = new TextObject("{=PTGsYoPc}Assign your clan members to {SETTLEMENT_NAME}", null);
			textObject2.SetTextVariable("SETTLEMENT_NAME", this.Settlement.Name.ToString());
			this.SendMembersHint = new HintViewModel(this.IsSendMembersEnabled ? textObject2 : textObject, null);
			this.UpdateProfitProperties();
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000687D8 File Offset: 0x000669D8
		protected virtual void UpdateProfitProperties()
		{
			this.ProfitItemProperties.Clear();
			if (this.Settlement.Town != null)
			{
				Town town = this.Settlement.Town;
				ClanFinanceModel clanFinanceModel = Campaign.Current.Models.ClanFinanceModel;
				int num = 0;
				int num2 = (int)Campaign.Current.Models.SettlementTaxModel.CalculateTownTax(town, false).ResultNumber;
				int num3 = (int)clanFinanceModel.CalculateTownIncomeFromTariffs(Clan.PlayerClan, town, false).ResultNumber;
				int num4 = clanFinanceModel.CalculateTownIncomeFromProjects(town);
				if (num2 != 0)
				{
					this.ProfitItemProperties.Add(new ProfitItemPropertyVM(new TextObject("{=qeclv74c}Taxes", null).ToString(), num2, ProfitItemPropertyVM.PropertyType.Tax, null, null));
					num += num2;
				}
				if (num3 != 0)
				{
					this.ProfitItemProperties.Add(new ProfitItemPropertyVM(new TextObject("{=eIgC6YGp}Tariffs", null).ToString(), num3, ProfitItemPropertyVM.PropertyType.Tariff, null, null));
					num += num3;
				}
				if (town.GarrisonParty != null && town.GarrisonParty.IsActive)
				{
					int totalWage = town.GarrisonParty.TotalWage;
					if (totalWage != 0)
					{
						this.ProfitItemProperties.Add(new ProfitItemPropertyVM(new TextObject("{=5dkPxmZG}Garrison Wages", null).ToString(), -totalWage, ProfitItemPropertyVM.PropertyType.Garrison, null, null));
						num -= totalWage;
					}
				}
				foreach (Village village in town.Villages)
				{
					int num5 = clanFinanceModel.CalculateVillageIncome(Clan.PlayerClan, village, false);
					if (num5 != 0)
					{
						this.ProfitItemProperties.Add(new ProfitItemPropertyVM(village.Name.ToString(), num5, ProfitItemPropertyVM.PropertyType.Village, null, null));
						num += num5;
					}
				}
				if (num4 != 0)
				{
					Collection<ProfitItemPropertyVM> profitItemProperties = this.ProfitItemProperties;
					string text = new TextObject("{=J8ddrAOf}Governor Effects", null).ToString();
					int num6 = num4;
					ProfitItemPropertyVM.PropertyType propertyType = ProfitItemPropertyVM.PropertyType.Governor;
					HeroVM governor = this.Governor;
					profitItemProperties.Add(new ProfitItemPropertyVM(text, num6, propertyType, (governor != null) ? governor.ImageIdentifier : null, null));
					num += num4;
				}
				this.TotalProfit.Value = num;
			}
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x000689D4 File Offset: 0x00066BD4
		private bool IsSettlementSlotAssignable(Hero oldHero, Hero newHero)
		{
			return (oldHero == null || !oldHero.IsHumanPlayerCharacter) && !newHero.IsHumanPlayerCharacter && newHero.IsActive && (newHero.PartyBelongedTo == null || newHero.PartyBelongedTo.LeaderHero != newHero) && newHero.PartyBelongedToAsPrisoner == null;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x00068A23 File Offset: 0x00066C23
		private void ExecuteOpenSettlementPage()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x00068A3F File Offset: 0x00066C3F
		// (set) Token: 0x06001C95 RID: 7317 RVA: 0x00068A47 File Offset: 0x00066C47
		[DataSourceProperty]
		public HeroVM Governor
		{
			get
			{
				return this._governor;
			}
			set
			{
				if (value != this._governor)
				{
					this._governor = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Governor");
				}
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x00068A65 File Offset: 0x00066C65
		// (set) Token: 0x06001C97 RID: 7319 RVA: 0x00068A6D File Offset: 0x00066C6D
		[DataSourceProperty]
		public MBBindingList<SelectableFiefItemPropertyVM> ItemProperties
		{
			get
			{
				return this._itemProperties;
			}
			set
			{
				if (value != this._itemProperties)
				{
					this._itemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<SelectableFiefItemPropertyVM>>(value, "ItemProperties");
				}
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x00068A8B File Offset: 0x00066C8B
		// (set) Token: 0x06001C99 RID: 7321 RVA: 0x00068A93 File Offset: 0x00066C93
		[DataSourceProperty]
		public MBBindingList<ProfitItemPropertyVM> ProfitItemProperties
		{
			get
			{
				return this._profitItemProperties;
			}
			set
			{
				if (value != this._profitItemProperties)
				{
					this._profitItemProperties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ProfitItemPropertyVM>>(value, "ProfitItemProperties");
				}
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x00068AB1 File Offset: 0x00066CB1
		// (set) Token: 0x06001C9B RID: 7323 RVA: 0x00068AB9 File Offset: 0x00066CB9
		[DataSourceProperty]
		public ProfitItemPropertyVM TotalProfit
		{
			get
			{
				return this._totalProfit;
			}
			set
			{
				if (value != this._totalProfit)
				{
					this._totalProfit = value;
					base.OnPropertyChangedWithValue<ProfitItemPropertyVM>(value, "TotalProfit");
				}
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x00068AD7 File Offset: 0x00066CD7
		// (set) Token: 0x06001C9D RID: 7325 RVA: 0x00068ADF File Offset: 0x00066CDF
		[DataSourceProperty]
		public string FileName
		{
			get
			{
				return this._fileName;
			}
			set
			{
				if (value != this._fileName)
				{
					this._fileName = value;
					base.OnPropertyChangedWithValue<string>(value, "FileName");
				}
			}
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x00068B02 File Offset: 0x00066D02
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x00068B0A File Offset: 0x00066D0A
		[DataSourceProperty]
		public string ImageName
		{
			get
			{
				return this._imageName;
			}
			set
			{
				if (value != this._imageName)
				{
					this._imageName = value;
					base.OnPropertyChangedWithValue<string>(value, "ImageName");
				}
			}
		}

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x00068B2D File Offset: 0x00066D2D
		// (set) Token: 0x06001CA1 RID: 7329 RVA: 0x00068B35 File Offset: 0x00066D35
		[DataSourceProperty]
		public string VillagesText
		{
			get
			{
				return this._villagesText;
			}
			set
			{
				if (value != this._villagesText)
				{
					this._villagesText = value;
					base.OnPropertyChangedWithValue<string>(value, "VillagesText");
				}
			}
		}

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x00068B58 File Offset: 0x00066D58
		// (set) Token: 0x06001CA3 RID: 7331 RVA: 0x00068B60 File Offset: 0x00066D60
		[DataSourceProperty]
		public string NotablesText
		{
			get
			{
				return this._notablesText;
			}
			set
			{
				if (value != this._notablesText)
				{
					this._notablesText = value;
					base.OnPropertyChangedWithValue<string>(value, "NotablesText");
				}
			}
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x00068B83 File Offset: 0x00066D83
		// (set) Token: 0x06001CA5 RID: 7333 RVA: 0x00068B8B File Offset: 0x00066D8B
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x00068BAE File Offset: 0x00066DAE
		// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x00068BB6 File Offset: 0x00066DB6
		[DataSourceProperty]
		public bool IsFortification
		{
			get
			{
				return this._isFortification;
			}
			set
			{
				if (value != this._isFortification)
				{
					this._isFortification = value;
					base.OnPropertyChangedWithValue(value, "IsFortification");
				}
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x00068BD4 File Offset: 0x00066DD4
		// (set) Token: 0x06001CA9 RID: 7337 RVA: 0x00068BDC File Offset: 0x00066DDC
		[DataSourceProperty]
		public bool HasGovernor
		{
			get
			{
				return this._hasGovernor;
			}
			set
			{
				if (value != this._hasGovernor)
				{
					this._hasGovernor = value;
					base.OnPropertyChangedWithValue(value, "HasGovernor");
				}
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x00068BFA File Offset: 0x00066DFA
		// (set) Token: 0x06001CAB RID: 7339 RVA: 0x00068C02 File Offset: 0x00066E02
		[DataSourceProperty]
		public bool HasNotables
		{
			get
			{
				return this._hasNotables;
			}
			set
			{
				if (value != this._hasNotables)
				{
					this._hasNotables = value;
					base.OnPropertyChangedWithValue(value, "HasNotables");
				}
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x00068C20 File Offset: 0x00066E20
		// (set) Token: 0x06001CAD RID: 7341 RVA: 0x00068C28 File Offset: 0x00066E28
		[DataSourceProperty]
		public bool IsSendMembersEnabled
		{
			get
			{
				return this._isSendMembersEnabled;
			}
			set
			{
				if (value != this._isSendMembersEnabled)
				{
					this._isSendMembersEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsSendMembersEnabled");
				}
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x00068C46 File Offset: 0x00066E46
		// (set) Token: 0x06001CAF RID: 7343 RVA: 0x00068C4E File Offset: 0x00066E4E
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

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x00068C6C File Offset: 0x00066E6C
		// (set) Token: 0x06001CB1 RID: 7345 RVA: 0x00068C74 File Offset: 0x00066E74
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

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x00068C97 File Offset: 0x00066E97
		// (set) Token: 0x06001CB3 RID: 7347 RVA: 0x00068C9F File Offset: 0x00066E9F
		[DataSourceProperty]
		public MBBindingList<ClanSettlementItemVM> VillagesOwned
		{
			get
			{
				return this._villagesOwned;
			}
			set
			{
				if (value != this._villagesOwned)
				{
					this._villagesOwned = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSettlementItemVM>>(value, "VillagesOwned");
				}
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x00068CBD File Offset: 0x00066EBD
		// (set) Token: 0x06001CB5 RID: 7349 RVA: 0x00068CC5 File Offset: 0x00066EC5
		[DataSourceProperty]
		public MBBindingList<HeroVM> Notables
		{
			get
			{
				return this._notables;
			}
			set
			{
				if (value != this._notables)
				{
					this._notables = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Notables");
				}
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x00068CE3 File Offset: 0x00066EE3
		// (set) Token: 0x06001CB7 RID: 7351 RVA: 0x00068CEB File Offset: 0x00066EEB
		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Members");
				}
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x00068D09 File Offset: 0x00066F09
		// (set) Token: 0x06001CB9 RID: 7353 RVA: 0x00068D11 File Offset: 0x00066F11
		[DataSourceProperty]
		public HintViewModel SendMembersHint
		{
			get
			{
				return this._sendMembersHint;
			}
			set
			{
				if (value != this._sendMembersHint)
				{
					this._sendMembersHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SendMembersHint");
				}
			}
		}

		// Token: 0x04000D08 RID: 3336
		private readonly Action<ClanSettlementItemVM> _onSelection;

		// Token: 0x04000D09 RID: 3337
		private readonly Action _onShowSendMembers;

		// Token: 0x04000D0A RID: 3338
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000D0B RID: 3339
		private readonly IPatrolPartiesCampaignBehavior _patrolsBehavior;

		// Token: 0x04000D0C RID: 3340
		public readonly Settlement Settlement;

		// Token: 0x04000D0D RID: 3341
		private string _name;

		// Token: 0x04000D0E RID: 3342
		private HeroVM _governor;

		// Token: 0x04000D0F RID: 3343
		private string _fileName;

		// Token: 0x04000D10 RID: 3344
		private string _imageName;

		// Token: 0x04000D11 RID: 3345
		private string _villagesText;

		// Token: 0x04000D12 RID: 3346
		private string _notablesText;

		// Token: 0x04000D13 RID: 3347
		private string _membersText;

		// Token: 0x04000D14 RID: 3348
		private bool _isFortification;

		// Token: 0x04000D15 RID: 3349
		private bool _isSelected;

		// Token: 0x04000D16 RID: 3350
		private bool _hasGovernor;

		// Token: 0x04000D17 RID: 3351
		private bool _hasNotables;

		// Token: 0x04000D18 RID: 3352
		private bool _isSendMembersEnabled;

		// Token: 0x04000D19 RID: 3353
		private MBBindingList<SelectableFiefItemPropertyVM> _itemProperties;

		// Token: 0x04000D1A RID: 3354
		private MBBindingList<ProfitItemPropertyVM> _profitItemProperties;

		// Token: 0x04000D1B RID: 3355
		private ProfitItemPropertyVM _totalProfit;

		// Token: 0x04000D1C RID: 3356
		private MBBindingList<ClanSettlementItemVM> _villagesOwned;

		// Token: 0x04000D1D RID: 3357
		private MBBindingList<HeroVM> _notables;

		// Token: 0x04000D1E RID: 3358
		private MBBindingList<HeroVM> _members;

		// Token: 0x04000D1F RID: 3359
		private HintViewModel _sendMembersHint;
	}
}
