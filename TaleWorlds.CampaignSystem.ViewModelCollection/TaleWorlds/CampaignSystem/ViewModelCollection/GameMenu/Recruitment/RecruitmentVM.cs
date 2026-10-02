using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Recruitment
{
	// Token: 0x020000B6 RID: 182
	public class RecruitmentVM : ViewModel
	{
		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x0004542E File Offset: 0x0004362E
		// (set) Token: 0x06001131 RID: 4401 RVA: 0x00045436 File Offset: 0x00043636
		public bool IsQuitting { get; private set; }

		// Token: 0x06001132 RID: 4402 RVA: 0x00045440 File Offset: 0x00043640
		public RecruitmentVM()
		{
			this.VolunteerList = new MBBindingList<RecruitVolunteerVM>();
			this.TroopsInCart = new MBBindingList<RecruitVolunteerTroopVM>();
			this.RefreshValues();
			if (Settlement.CurrentSettlement != null)
			{
				this.RefreshScreen();
			}
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			RecruitVolunteerTroopVM.OnFocused = (Action<RecruitVolunteerTroopVM>)Delegate.Combine(RecruitVolunteerTroopVM.OnFocused, new Action<RecruitVolunteerTroopVM>(this.OnVolunteerTroopFocusChanged));
			RecruitVolunteerOwnerVM.OnFocused = (Action<RecruitVolunteerOwnerVM>)Delegate.Combine(RecruitVolunteerOwnerVM.OnFocused, new Action<RecruitVolunteerOwnerVM>(this.OnVolunteerOwnerFocusChanged));
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00045510 File Offset: 0x00043710
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PartyWageHint = new HintViewModel(GameTexts.FindText("str_weekly_wage", null), null);
			this.TotalWealthHint = new HintViewModel(GameTexts.FindText("str_wealth", null), null);
			this.TotalCostHint = new HintViewModel(GameTexts.FindText("str_total_cost", null), null);
			this.PartyCapacityHint = new HintViewModel();
			this.PartySpeedHint = new BasicTooltipViewModel();
			this.RemainingFoodHint = new HintViewModel();
			this.DoneHint = new HintViewModel();
			this.ResetHint = new HintViewModel(GameTexts.FindText("str_reset", null), null);
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.TitleText = GameTexts.FindText("str_recruitment", null).ToString();
			this._recruitAllTextObject = GameTexts.FindText("str_recruit_all", null);
			this.ResetAllText = GameTexts.FindText("str_reset_all", null).ToString();
			this.CancelText = GameTexts.FindText("str_party_cancel", null).ToString();
			this._playerDoesntHaveEnoughMoneyStr = GameTexts.FindText("str_warning_you_dont_have_enough_money", null).ToString();
			this._playerIsOverPartyLimitStr = GameTexts.FindText("str_party_size_limit_exceeded", null).ToString();
			this.VolunteerList.ApplyActionOnAllItems(delegate(RecruitVolunteerVM x)
			{
				x.RefreshValues();
			});
			this.TroopsInCart.ApplyActionOnAllItems(delegate(RecruitVolunteerTroopVM x)
			{
				x.RefreshValues();
			});
			this.SetRecruitAllHint();
			this.UpdateRecruitAllProperties();
			if (Settlement.CurrentSettlement != null)
			{
				this.RefreshScreen();
			}
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000456B0 File Offset: 0x000438B0
		public void RefreshScreen()
		{
			this.VolunteerList.Clear();
			this.TroopsInCart.Clear();
			int num = 0;
			this.InitialPartySize = PartyBase.MainParty.NumberOfAllMembers;
			this.RefreshPartyProperties();
			foreach (Hero hero in Settlement.CurrentSettlement.Notables)
			{
				if (hero.CanHaveRecruits)
				{
					MBTextManager.SetTextVariable("INDIVIDUAL_NAME", hero.Name, false);
					List<CharacterObject> volunteerTroopsOfHeroForRecruitment = HeroHelper.GetVolunteerTroopsOfHeroForRecruitment(hero);
					RecruitVolunteerVM recruitVolunteerVM = new RecruitVolunteerVM(hero, volunteerTroopsOfHeroForRecruitment, new Action<RecruitVolunteerVM, RecruitVolunteerTroopVM>(this.OnRecruit), new Action<RecruitVolunteerVM, RecruitVolunteerTroopVM>(this.OnRemoveFromCart));
					this.VolunteerList.Add(recruitVolunteerVM);
					num++;
				}
			}
			this.TotalWealth = Hero.MainHero.Gold;
			this.UpdateRecruitAllProperties();
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00045798 File Offset: 0x00043998
		private void OnRecruit(RecruitVolunteerVM recruitNotable, RecruitVolunteerTroopVM recruitTroop)
		{
			if (!recruitTroop.CanBeRecruited)
			{
				return;
			}
			recruitNotable.OnRecruitMoveToCart(recruitTroop);
			recruitTroop.CanBeRecruited = false;
			this.TroopsInCart.Add(recruitTroop);
			recruitTroop.IsInCart = true;
			CampaignEventDispatcher.Instance.OnPlayerStartRecruitment(recruitTroop.Character);
			this.RefreshPartyProperties();
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000457E8 File Offset: 0x000439E8
		private void RefreshPartyProperties()
		{
			int num = this.TroopsInCart.Sum<RecruitVolunteerTroopVM>((RecruitVolunteerTroopVM t) => t.Wage);
			this.PartyWage = MobileParty.MainParty.TotalWage;
			if (num > 0)
			{
				this.PartyWageText = CampaignUIHelper.GetValueChangeText((float)this.PartyWage, (float)num, "F0");
			}
			else
			{
				this.PartyWageText = this.PartyWage.ToString();
			}
			double num2 = 0.0;
			if (this.TroopsInCart.Count > 0)
			{
				int num3 = 0;
				int num4 = 0;
				using (IEnumerator<RecruitVolunteerTroopVM> enumerator = this.TroopsInCart.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Character.IsMounted)
						{
							num4++;
						}
						else
						{
							num3++;
						}
					}
				}
				ExplainedNumber explainedNumber = Campaign.Current.Models.PartySpeedCalculatingModel.CalculateBaseSpeed(MobileParty.MainParty, false, num3, num4);
				ExplainedNumber explainedNumber2 = Campaign.Current.Models.PartySpeedCalculatingModel.CalculateFinalSpeed(MobileParty.MainParty, explainedNumber);
				ExplainedNumber explainedNumber3 = Campaign.Current.Models.PartySpeedCalculatingModel.CalculateBaseSpeed(MobileParty.MainParty, false, 0, 0);
				ExplainedNumber explainedNumber4 = Campaign.Current.Models.PartySpeedCalculatingModel.CalculateFinalSpeed(MobileParty.MainParty, explainedNumber3);
				num2 = (double)(MathF.Round(explainedNumber2.ResultNumber, 1) - MathF.Round(explainedNumber4.ResultNumber, 1));
			}
			this.PartySpeedText = MobileParty.MainParty.Speed.ToString("0.0");
			this.PartySpeedHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetPartySpeedTooltip(false));
			if (num2 != 0.0)
			{
				this.PartySpeedText = CampaignUIHelper.GetValueChangeText(MobileParty.MainParty.Speed, (float)num2, "0.0");
			}
			int partySizeLimit = PartyBase.MainParty.PartySizeLimit;
			this.CurrentPartySize = PartyBase.MainParty.NumberOfAllMembers + this.TroopsInCart.Count;
			this.PartyCapacity = partySizeLimit;
			this.IsPartyCapacityWarningEnabled = this.CurrentPartySize > this.PartyCapacity;
			GameTexts.SetVariable("LEFT", this.CurrentPartySize.ToString());
			GameTexts.SetVariable("RIGHT", partySizeLimit.ToString());
			this.PartyCapacityText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			this.PartyCapacityHint.HintText = new TextObject("{=!}" + PartyBase.MainParty.PartySizeLimitExplainer.ToString(), null);
			float food = MobileParty.MainParty.Food;
			this.RemainingFoodText = MathF.Round(food, 1).ToString();
			float foodChange = MobileParty.MainParty.FoodChange;
			int totalFoodAtInventory = MobileParty.MainParty.TotalFoodAtInventory;
			int numDaysForFoodToLast = MobileParty.MainParty.GetNumDaysForFoodToLast();
			MBTextManager.SetTextVariable("DAY_NUM", numDaysForFoodToLast);
			this.RemainingFoodHint.HintText = GameTexts.FindText("str_food_consumption_tooltip", null);
			this.RemainingFoodHint.HintText.SetTextVariable("DAILY_FOOD_CONSUMPTION", foodChange, 2);
			this.RemainingFoodHint.HintText.SetTextVariable("REMAINING_DAYS", GameTexts.FindText("str_party_food_left", null));
			this.RemainingFoodHint.HintText.SetTextVariable("TOTAL_FOOD_AMOUNT", ((double)totalFoodAtInventory + 0.01 * (double)PartyBase.MainParty.RemainingFoodPercentage).ToString("0.00"));
			this.RemainingFoodHint.HintText.SetTextVariable("TOTAL_FOOD", totalFoodAtInventory);
			int num5 = this.TroopsInCart.Sum<RecruitVolunteerTroopVM>((RecruitVolunteerTroopVM t) => t.Cost);
			this.TotalCostText = num5.ToString();
			bool flag = num5 <= Hero.MainHero.Gold;
			this.IsDoneEnabled = flag;
			this.DoneHint.HintText = new TextObject("{=!}" + this.GetDoneHint(flag), null);
			this.UpdateRecruitAllProperties();
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00045C0C File Offset: 0x00043E0C
		public void ExecuteDone()
		{
			if (this.CurrentPartySize <= this.PartyCapacity)
			{
				this.OnDone();
				return;
			}
			GameTexts.SetVariable("newline", "\n");
			string text = GameTexts.FindText("str_party_over_limit_troops", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(new TextObject("{=uJro3Bua}Over Limit", null).ToString(), text, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				this.OnDone();
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00045CA8 File Offset: 0x00043EA8
		private void OnDone()
		{
			this.RefreshPartyProperties();
			int num = this.TroopsInCart.Sum<RecruitVolunteerTroopVM>((RecruitVolunteerTroopVM t) => t.Cost);
			if (num > Hero.MainHero.Gold)
			{
				Debug.FailedAssert("Execution shouldn't come here. The checks should happen before", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\GameMenu\\Recruitment\\RecruitmentVM.cs", "OnDone", 229);
				return;
			}
			foreach (RecruitVolunteerTroopVM recruitVolunteerTroopVM in this.TroopsInCart)
			{
				recruitVolunteerTroopVM.Owner.OwnerHero.VolunteerTypes[recruitVolunteerTroopVM.Index] = null;
				MobileParty.MainParty.MemberRoster.AddToCounts(recruitVolunteerTroopVM.Character, 1, false, 0, 0, true, -1);
				CampaignEventDispatcher.Instance.OnUnitRecruited(recruitVolunteerTroopVM.Character, 1);
			}
			GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, null, num, true);
			if (num > 0)
			{
				MBTextManager.SetTextVariable("GOLD_AMOUNT", MathF.Abs(num));
				InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_gold_removed_with_icon", null).ToString(), "event:/ui/notification/coins_negative"));
			}
			this.Deactivate();
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00045DD4 File Offset: 0x00043FD4
		public void ExecuteForceQuit()
		{
			if (!this.IsQuitting)
			{
				this.IsQuitting = true;
				if (this.TroopsInCart.Count > 0)
				{
					InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_quit", null).ToString(), GameTexts.FindText("str_quit_question", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						this.ExecuteReset();
						this.ExecuteDone();
						this.IsQuitting = false;
					}, delegate
					{
						this.IsQuitting = false;
					}, "", 0f, null, null, null), true, false);
					return;
				}
				this.Deactivate();
			}
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00045E7C File Offset: 0x0004407C
		public void ExecuteReset()
		{
			for (int i = this.TroopsInCart.Count - 1; i >= 0; i--)
			{
				this.TroopsInCart[i].ExecuteRemoveFromCart();
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00045EB4 File Offset: 0x000440B4
		public void ExecuteRecruitAll()
		{
			foreach (RecruitVolunteerVM recruitVolunteerVM in this.VolunteerList.ToList<RecruitVolunteerVM>())
			{
				foreach (RecruitVolunteerTroopVM recruitVolunteerTroopVM in recruitVolunteerVM.Troops.ToList<RecruitVolunteerTroopVM>())
				{
					recruitVolunteerTroopVM.ExecuteRecruit();
				}
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00045F48 File Offset: 0x00044148
		public void Deactivate()
		{
			this.ExecuteReset();
			this.Enabled = false;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00045F58 File Offset: 0x00044158
		public override void OnFinalize()
		{
			base.OnFinalize();
			RecruitVolunteerTroopVM.OnFocused = (Action<RecruitVolunteerTroopVM>)Delegate.Remove(RecruitVolunteerTroopVM.OnFocused, new Action<RecruitVolunteerTroopVM>(this.OnVolunteerTroopFocusChanged));
			RecruitVolunteerOwnerVM.OnFocused = (Action<RecruitVolunteerOwnerVM>)Delegate.Remove(RecruitVolunteerOwnerVM.OnFocused, new Action<RecruitVolunteerOwnerVM>(this.OnVolunteerOwnerFocusChanged));
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.CancelInputKey.OnFinalize();
			this.DoneInputKey.OnFinalize();
			this.ResetInputKey.OnFinalize();
			this.RecruitAllInputKey.OnFinalize();
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00045FF4 File Offset: 0x000441F4
		private void OnRemoveFromCart(RecruitVolunteerVM recruitNotable, RecruitVolunteerTroopVM recruitTroop)
		{
			if (this.TroopsInCart.Any<RecruitVolunteerTroopVM>((RecruitVolunteerTroopVM r) => r == recruitTroop))
			{
				recruitNotable.OnRecruitRemovedFromCart(recruitTroop);
				recruitTroop.CanBeRecruited = true;
				recruitTroop.IsInCart = false;
				recruitTroop.IsHiglightEnabled = false;
				this.TroopsInCart.Remove(recruitTroop);
				this.RefreshPartyProperties();
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0004606F File Offset: 0x0004426F
		private static bool IsBitSet(int num, int bit)
		{
			return 1 == ((num >> bit) & 1);
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x0004607C File Offset: 0x0004427C
		private string GetDoneHint(bool doesPlayerHasEnoughMoney)
		{
			if (!doesPlayerHasEnoughMoney)
			{
				return this._playerDoesntHaveEnoughMoneyStr;
			}
			return null;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00046089 File Offset: 0x00044289
		private void SetRecruitAllHint()
		{
			this.RecruitAllHint = new BasicTooltipViewModel(delegate
			{
				GameTexts.SetVariable("HOTKEY", this.GetRecruitAllKey());
				GameTexts.SetVariable("TEXT", GameTexts.FindText("str_recruit_all", null));
				return GameTexts.FindText("str_hotkey_with_hint", null).ToString();
			});
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000460A4 File Offset: 0x000442A4
		private void UpdateRecruitAllProperties()
		{
			int numberOfAvailableRecruits = this.GetNumberOfAvailableRecruits();
			GameTexts.SetVariable("STR", numberOfAvailableRecruits);
			GameTexts.SetVariable("STR1", this._recruitAllTextObject);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_STR_in_parentheses", null));
			this.RecruitAllText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.CanRecruitAll = numberOfAvailableRecruits > 0;
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00046108 File Offset: 0x00044308
		private int GetNumberOfAvailableRecruits()
		{
			int num = 0;
			foreach (RecruitVolunteerVM recruitVolunteerVM in this.VolunteerList)
			{
				foreach (RecruitVolunteerTroopVM recruitVolunteerTroopVM in recruitVolunteerVM.Troops)
				{
					if (!recruitVolunteerTroopVM.IsInCart && recruitVolunteerTroopVM.CanBeRecruited)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00046198 File Offset: 0x00044398
		private void OnVolunteerTroopFocusChanged(RecruitVolunteerTroopVM volunteer)
		{
			this.FocusedVolunteerTroop = volunteer;
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x000461A1 File Offset: 0x000443A1
		private void OnVolunteerOwnerFocusChanged(RecruitVolunteerOwnerVM owner)
		{
			this.FocusedVolunteerOwner = owner;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x000461AC File Offset: 0x000443AC
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null && this._isAvailableTroopsHighlightApplied)
				{
					this.SetAvailableTroopsHighlightState(false);
					this._isAvailableTroopsHighlightApplied = false;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null && !this._isAvailableTroopsHighlightApplied && this._latestTutorialElementID == "AvailableTroops")
				{
					this.SetAvailableTroopsHighlightState(true);
					this._isAvailableTroopsHighlightApplied = true;
				}
			}
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00046228 File Offset: 0x00044428
		private void SetAvailableTroopsHighlightState(bool state)
		{
			foreach (RecruitVolunteerVM recruitVolunteerVM in this.VolunteerList)
			{
				foreach (RecruitVolunteerTroopVM recruitVolunteerTroopVM in recruitVolunteerVM.Troops)
				{
					if (recruitVolunteerTroopVM.Wage < Hero.MainHero.Gold && recruitVolunteerTroopVM.PlayerHasEnoughRelation && !recruitVolunteerTroopVM.IsTroopEmpty)
					{
						recruitVolunteerTroopVM.IsHiglightEnabled = state;
					}
				}
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000462CC File Offset: 0x000444CC
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x000462D4 File Offset: 0x000444D4
		[DataSourceProperty]
		public HintViewModel ResetHint
		{
			get
			{
				return this._resetHint;
			}
			set
			{
				if (value != this._resetHint)
				{
					this._resetHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetHint");
				}
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x000462F2 File Offset: 0x000444F2
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x000462FA File Offset: 0x000444FA
		[DataSourceProperty]
		public RecruitVolunteerTroopVM FocusedVolunteerTroop
		{
			get
			{
				return this._focusedVolunteerTroop;
			}
			set
			{
				if (value != this._focusedVolunteerTroop)
				{
					this._focusedVolunteerTroop = value;
					base.OnPropertyChangedWithValue<RecruitVolunteerTroopVM>(value, "FocusedVolunteerTroop");
				}
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x00046318 File Offset: 0x00044518
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x00046320 File Offset: 0x00044520
		[DataSourceProperty]
		public RecruitVolunteerOwnerVM FocusedVolunteerOwner
		{
			get
			{
				return this._focusedVolunteerOwner;
			}
			set
			{
				if (value != this._focusedVolunteerOwner)
				{
					this._focusedVolunteerOwner = value;
					base.OnPropertyChangedWithValue<RecruitVolunteerOwnerVM>(value, "FocusedVolunteerOwner");
				}
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x0004633E File Offset: 0x0004453E
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x00046346 File Offset: 0x00044546
		[DataSourceProperty]
		public HintViewModel PartyWageHint
		{
			get
			{
				return this._partyWageHint;
			}
			set
			{
				if (value != this._partyWageHint)
				{
					this._partyWageHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PartyWageHint");
				}
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x00046364 File Offset: 0x00044564
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x0004636C File Offset: 0x0004456C
		[DataSourceProperty]
		public HintViewModel PartyCapacityHint
		{
			get
			{
				return this._partyCapacityHint;
			}
			set
			{
				if (value != this._partyCapacityHint)
				{
					this._partyCapacityHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "PartyCapacityHint");
				}
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x0004638A File Offset: 0x0004458A
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x00046392 File Offset: 0x00044592
		[DataSourceProperty]
		public BasicTooltipViewModel PartySpeedHint
		{
			get
			{
				return this._partySpeedHint;
			}
			set
			{
				if (value != this._partySpeedHint)
				{
					this._partySpeedHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "PartySpeedHint");
				}
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x000463B0 File Offset: 0x000445B0
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x000463B8 File Offset: 0x000445B8
		[DataSourceProperty]
		public HintViewModel RemainingFoodHint
		{
			get
			{
				return this._remainingFoodHint;
			}
			set
			{
				if (value != this._remainingFoodHint)
				{
					this._remainingFoodHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RemainingFoodHint");
				}
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x000463D6 File Offset: 0x000445D6
		// (set) Token: 0x06001157 RID: 4439 RVA: 0x000463DE File Offset: 0x000445DE
		[DataSourceProperty]
		public HintViewModel TotalWealthHint
		{
			get
			{
				return this._totalWealthHint;
			}
			set
			{
				if (value != this._totalWealthHint)
				{
					this._totalWealthHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TotalWealthHint");
				}
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x000463FC File Offset: 0x000445FC
		// (set) Token: 0x06001159 RID: 4441 RVA: 0x00046404 File Offset: 0x00044604
		[DataSourceProperty]
		public HintViewModel TotalCostHint
		{
			get
			{
				return this._totalCostHint;
			}
			set
			{
				if (value != this._totalCostHint)
				{
					this._totalCostHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "TotalCostHint");
				}
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x00046422 File Offset: 0x00044622
		// (set) Token: 0x0600115B RID: 4443 RVA: 0x0004642A File Offset: 0x0004462A
		[DataSourceProperty]
		public HintViewModel DoneHint
		{
			get
			{
				return this._doneHint;
			}
			set
			{
				if (value != this._doneHint)
				{
					this._doneHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DoneHint");
				}
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00046448 File Offset: 0x00044648
		// (set) Token: 0x0600115D RID: 4445 RVA: 0x00046450 File Offset: 0x00044650
		[DataSourceProperty]
		public BasicTooltipViewModel RecruitAllHint
		{
			get
			{
				return this._recruitAllHint;
			}
			set
			{
				if (value != this._recruitAllHint)
				{
					this._recruitAllHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RecruitAllHint");
				}
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x0004646E File Offset: 0x0004466E
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x00046476 File Offset: 0x00044676
		[DataSourceProperty]
		public int PartyWage
		{
			get
			{
				return this._partyWage;
			}
			set
			{
				if (value != this._partyWage)
				{
					this._partyWage = value;
					base.OnPropertyChangedWithValue(value, "PartyWage");
				}
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x00046494 File Offset: 0x00044694
		// (set) Token: 0x06001161 RID: 4449 RVA: 0x0004649C File Offset: 0x0004469C
		[DataSourceProperty]
		public string PartyCapacityText
		{
			get
			{
				return this._partyCapacityText;
			}
			set
			{
				if (value != this._partyCapacityText)
				{
					this._partyCapacityText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyCapacityText");
				}
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x000464BF File Offset: 0x000446BF
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x000464C7 File Offset: 0x000446C7
		[DataSourceProperty]
		public string PartyWageText
		{
			get
			{
				return this._partyWageText;
			}
			set
			{
				if (value != this._partyWageText)
				{
					this._partyWageText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyWageText");
				}
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x000464EA File Offset: 0x000446EA
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x000464F2 File Offset: 0x000446F2
		[DataSourceProperty]
		public string RecruitAllText
		{
			get
			{
				return this._recruitAllText;
			}
			set
			{
				if (value != this._recruitAllText)
				{
					this._recruitAllText = value;
					base.OnPropertyChangedWithValue<string>(value, "RecruitAllText");
				}
			}
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x00046515 File Offset: 0x00044715
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x0004651D File Offset: 0x0004471D
		[DataSourceProperty]
		public string PartySpeedText
		{
			get
			{
				return this._partySpeedText;
			}
			set
			{
				if (value != this._partySpeedText)
				{
					this._partySpeedText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartySpeedText");
				}
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x00046540 File Offset: 0x00044740
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x00046548 File Offset: 0x00044748
		[DataSourceProperty]
		public string ResetAllText
		{
			get
			{
				return this._resetAllText;
			}
			set
			{
				if (value != this._resetAllText)
				{
					this._resetAllText = value;
					base.OnPropertyChangedWithValue<string>(value, "ResetAllText");
				}
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x0004656B File Offset: 0x0004476B
		// (set) Token: 0x0600116B RID: 4459 RVA: 0x00046573 File Offset: 0x00044773
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x00046596 File Offset: 0x00044796
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x0004659E File Offset: 0x0004479E
		[DataSourceProperty]
		public string RemainingFoodText
		{
			get
			{
				return this._remainingFoodText;
			}
			set
			{
				if (value != this._remainingFoodText)
				{
					this._remainingFoodText = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingFoodText");
				}
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x000465C1 File Offset: 0x000447C1
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x000465C9 File Offset: 0x000447C9
		[DataSourceProperty]
		public string TotalCostText
		{
			get
			{
				return this._totalCostText;
			}
			set
			{
				if (value != this._totalCostText)
				{
					this._totalCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalCostText");
				}
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x000465EC File Offset: 0x000447EC
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x000465F4 File Offset: 0x000447F4
		[DataSourceProperty]
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (value != this._enabled)
				{
					this._enabled = value;
					base.OnPropertyChangedWithValue(value, "Enabled");
				}
			}
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x00046614 File Offset: 0x00044814
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x0004661C File Offset: 0x0004481C
		[DataSourceProperty]
		public bool IsDoneEnabled
		{
			get
			{
				return this._isDoneEnabled;
			}
			set
			{
				if (value != this._isDoneEnabled)
				{
					this._isDoneEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsDoneEnabled");
				}
			}
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x0004663A File Offset: 0x0004483A
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00046642 File Offset: 0x00044842
		[DataSourceProperty]
		public bool IsPartyCapacityWarningEnabled
		{
			get
			{
				return this._isPartyCapacityWarningEnabled;
			}
			set
			{
				if (value != this._isPartyCapacityWarningEnabled)
				{
					this._isPartyCapacityWarningEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPartyCapacityWarningEnabled");
				}
			}
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00046660 File Offset: 0x00044860
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00046668 File Offset: 0x00044868
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0004668B File Offset: 0x0004488B
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x00046693 File Offset: 0x00044893
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x000466B6 File Offset: 0x000448B6
		// (set) Token: 0x0600117B RID: 4475 RVA: 0x000466BE File Offset: 0x000448BE
		[DataSourceProperty]
		public bool CanRecruitAll
		{
			get
			{
				return this._canRecruitAll;
			}
			set
			{
				if (value != this._canRecruitAll)
				{
					this._canRecruitAll = value;
					base.OnPropertyChangedWithValue(value, "CanRecruitAll");
				}
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x000466DC File Offset: 0x000448DC
		// (set) Token: 0x0600117D RID: 4477 RVA: 0x000466E4 File Offset: 0x000448E4
		[DataSourceProperty]
		public int TotalWealth
		{
			get
			{
				return this._totalWealth;
			}
			set
			{
				if (value != this._totalWealth)
				{
					this._totalWealth = value;
					base.OnPropertyChangedWithValue(value, "TotalWealth");
				}
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x0600117E RID: 4478 RVA: 0x00046702 File Offset: 0x00044902
		// (set) Token: 0x0600117F RID: 4479 RVA: 0x0004670A File Offset: 0x0004490A
		[DataSourceProperty]
		public int PartyCapacity
		{
			get
			{
				return this._partyCapacity;
			}
			set
			{
				if (value != this._partyCapacity)
				{
					this._partyCapacity = value;
					base.OnPropertyChangedWithValue(value, "PartyCapacity");
				}
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001180 RID: 4480 RVA: 0x00046728 File Offset: 0x00044928
		// (set) Token: 0x06001181 RID: 4481 RVA: 0x00046730 File Offset: 0x00044930
		[DataSourceProperty]
		public int InitialPartySize
		{
			get
			{
				return this._initialPartySize;
			}
			set
			{
				if (value != this._initialPartySize)
				{
					this._initialPartySize = value;
					base.OnPropertyChangedWithValue(value, "InitialPartySize");
				}
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x0004674E File Offset: 0x0004494E
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x00046756 File Offset: 0x00044956
		[DataSourceProperty]
		public int CurrentPartySize
		{
			get
			{
				return this._currentPartySize;
			}
			set
			{
				if (value != this._currentPartySize)
				{
					this._currentPartySize = value;
					base.OnPropertyChangedWithValue(value, "CurrentPartySize");
				}
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x00046774 File Offset: 0x00044974
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x0004677C File Offset: 0x0004497C
		[DataSourceProperty]
		public MBBindingList<RecruitVolunteerVM> VolunteerList
		{
			get
			{
				return this._volunteerList;
			}
			set
			{
				if (value != this._volunteerList)
				{
					this._volunteerList = value;
					base.OnPropertyChangedWithValue<MBBindingList<RecruitVolunteerVM>>(value, "VolunteerList");
				}
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x0004679A File Offset: 0x0004499A
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x000467A2 File Offset: 0x000449A2
		[DataSourceProperty]
		public MBBindingList<RecruitVolunteerTroopVM> TroopsInCart
		{
			get
			{
				return this._troopsInCart;
			}
			set
			{
				if (value != this._troopsInCart)
				{
					this._troopsInCart = value;
					base.OnPropertyChangedWithValue<MBBindingList<RecruitVolunteerTroopVM>>(value, "TroopsInCart");
				}
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000467C0 File Offset: 0x000449C0
		public void SetGetKeyTextFromKeyIDFunc(Func<string, TextObject> getKeyTextFromKeyId)
		{
			this._getKeyTextFromKeyId = getKeyTextFromKeyId;
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000467C9 File Offset: 0x000449C9
		private string GetRecruitAllKey()
		{
			if (this.RecruitAllInputKey == null || this._getKeyTextFromKeyId == null)
			{
				return string.Empty;
			}
			return this._getKeyTextFromKeyId(this.RecruitAllInputKey.KeyID).ToString();
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x000467FC File Offset: 0x000449FC
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0004680B File Offset: 0x00044A0B
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0004681A File Offset: 0x00044A1A
		public void SetRecruitAllInputKey(HotKey hotKey)
		{
			this.RecruitAllInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.SetRecruitAllHint();
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0004682F File Offset: 0x00044A2F
		public void SetResetInputKey(HotKey hotKey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x0004683E File Offset: 0x00044A3E
		// (set) Token: 0x0600118F RID: 4495 RVA: 0x00046846 File Offset: 0x00044A46
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x00046864 File Offset: 0x00044A64
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x0004686C File Offset: 0x00044A6C
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x0004688A File Offset: 0x00044A8A
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x00046892 File Offset: 0x00044A92
		[DataSourceProperty]
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x000468B0 File Offset: 0x00044AB0
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x000468B8 File Offset: 0x00044AB8
		[DataSourceProperty]
		public InputKeyItemVM RecruitAllInputKey
		{
			get
			{
				return this._recruitAllInputKey;
			}
			set
			{
				if (value != this._recruitAllInputKey)
				{
					this._recruitAllInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RecruitAllInputKey");
				}
			}
		}

		// Token: 0x040007CF RID: 1999
		private TextObject _recruitAllTextObject;

		// Token: 0x040007D0 RID: 2000
		private string _playerDoesntHaveEnoughMoneyStr;

		// Token: 0x040007D1 RID: 2001
		private string _playerIsOverPartyLimitStr;

		// Token: 0x040007D2 RID: 2002
		private Func<string, TextObject> _getKeyTextFromKeyId;

		// Token: 0x040007D3 RID: 2003
		private bool _isAvailableTroopsHighlightApplied;

		// Token: 0x040007D4 RID: 2004
		private string _latestTutorialElementID;

		// Token: 0x040007D5 RID: 2005
		private bool _enabled;

		// Token: 0x040007D6 RID: 2006
		private bool _isDoneEnabled;

		// Token: 0x040007D7 RID: 2007
		private bool _isPartyCapacityWarningEnabled;

		// Token: 0x040007D8 RID: 2008
		private bool _canRecruitAll;

		// Token: 0x040007D9 RID: 2009
		private string _titleText;

		// Token: 0x040007DA RID: 2010
		private string _doneText;

		// Token: 0x040007DB RID: 2011
		private string _recruitAllText;

		// Token: 0x040007DC RID: 2012
		private string _resetAllText;

		// Token: 0x040007DD RID: 2013
		private string _cancelText;

		// Token: 0x040007DE RID: 2014
		private int _totalWealth;

		// Token: 0x040007DF RID: 2015
		private int _partyCapacity;

		// Token: 0x040007E0 RID: 2016
		private int _initialPartySize;

		// Token: 0x040007E1 RID: 2017
		private int _currentPartySize;

		// Token: 0x040007E2 RID: 2018
		private MBBindingList<RecruitVolunteerVM> _volunteerList;

		// Token: 0x040007E3 RID: 2019
		private MBBindingList<RecruitVolunteerTroopVM> _troopsInCart;

		// Token: 0x040007E4 RID: 2020
		private int _partyWage;

		// Token: 0x040007E5 RID: 2021
		private string _partyCapacityText = "";

		// Token: 0x040007E6 RID: 2022
		private string _partyWageText = "";

		// Token: 0x040007E7 RID: 2023
		private string _partySpeedText = "";

		// Token: 0x040007E8 RID: 2024
		private string _remainingFoodText = "";

		// Token: 0x040007E9 RID: 2025
		private string _totalCostText = "";

		// Token: 0x040007EA RID: 2026
		private RecruitVolunteerTroopVM _focusedVolunteerTroop;

		// Token: 0x040007EB RID: 2027
		private RecruitVolunteerOwnerVM _focusedVolunteerOwner;

		// Token: 0x040007EC RID: 2028
		private HintViewModel _partyWageHint;

		// Token: 0x040007ED RID: 2029
		private HintViewModel _partyCapacityHint;

		// Token: 0x040007EE RID: 2030
		private BasicTooltipViewModel _partySpeedHint;

		// Token: 0x040007EF RID: 2031
		private HintViewModel _remainingFoodHint;

		// Token: 0x040007F0 RID: 2032
		private HintViewModel _totalWealthHint;

		// Token: 0x040007F1 RID: 2033
		private HintViewModel _totalCostHint;

		// Token: 0x040007F2 RID: 2034
		private HintViewModel _resetHint;

		// Token: 0x040007F3 RID: 2035
		private HintViewModel _doneHint;

		// Token: 0x040007F4 RID: 2036
		private BasicTooltipViewModel _recruitAllHint;

		// Token: 0x040007F5 RID: 2037
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x040007F6 RID: 2038
		private InputKeyItemVM _doneInputKey;

		// Token: 0x040007F7 RID: 2039
		private InputKeyItemVM _resetInputKey;

		// Token: 0x040007F8 RID: 2040
		private InputKeyItemVM _recruitAllInputKey;
	}
}
