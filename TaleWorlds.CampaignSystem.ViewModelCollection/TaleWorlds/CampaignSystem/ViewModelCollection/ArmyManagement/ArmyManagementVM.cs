using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ArmyManagement
{
	// Token: 0x02000164 RID: 356
	public class ArmyManagementVM : ViewModel
	{
		// Token: 0x06002299 RID: 8857 RVA: 0x0007B078 File Offset: 0x00079278
		public ArmyManagementVM(Action onClose)
		{
			this._onClose = onClose;
			this._itemComparer = new ArmyManagementVM.ManagementItemComparer();
			this.PartyList = new MBBindingList<ArmyManagementItemVM>();
			this.PartiesInCart = new MBBindingList<ArmyManagementItemVM>();
			this._partiesToRemove = new MBBindingList<ArmyManagementItemVM>();
			this._currentParties = new List<MobileParty>();
			this.CohesionHint = new BasicTooltipViewModel();
			this.FoodHint = new HintViewModel();
			this.MoraleHint = new HintViewModel();
			this.BoostCohesionHint = new HintViewModel();
			this.DisbandArmyHint = new HintViewModel();
			this.DoneHint = new HintViewModel();
			this.TutorialNotification = new ElementNotificationVM();
			this.CanConfirm = false;
			this.CanAffordInfluenceCost = true;
			this.PlayerHasArmy = MobileParty.MainParty.Army != null;
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				if (mobileParty.LeaderHero != null && mobileParty.MapFaction == Hero.MainHero.MapFaction && mobileParty.LeaderHero != Hero.MainHero && !mobileParty.IsCaravan)
				{
					this.PartyList.Add(new ArmyManagementItemVM(new Action<ArmyManagementItemVM>(this.OnAddToCart), new Action<ArmyManagementItemVM>(this.OnRemove), new Action<ArmyManagementItemVM>(this.OnFocus), mobileParty));
				}
			}
			this._mainPartyItem = new ArmyManagementItemVM(null, null, null, Hero.MainHero.PartyBelongedTo)
			{
				IsAlreadyWithPlayer = true,
				IsMainHero = true,
				IsInCart = true
			};
			this.PartiesInCart.Add(this._mainPartyItem);
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartyList)
			{
				if (MobileParty.MainParty.Army != null && armyManagementItemVM.Party.Army == MobileParty.MainParty.Army && armyManagementItemVM.Party != MobileParty.MainParty)
				{
					armyManagementItemVM.Cost = 0;
					armyManagementItemVM.IsAlreadyWithPlayer = true;
					armyManagementItemVM.IsInCart = true;
					this.PartiesInCart.Add(armyManagementItemVM);
				}
			}
			if (MobileParty.MainParty.Army != null)
			{
				this.CohesionBoostCost = Campaign.Current.Models.ArmyManagementCalculationModel.GetCohesionBoostInfluenceCost(MobileParty.MainParty.Army, 10);
			}
			this._initialInfluence = Hero.MainHero.Clan.Influence;
			this.OnRefresh();
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.ArmyManagement));
			this.SortControllerVM = new ArmyManagementSortControllerVM(this._partyList);
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			this.RefreshValues();
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0007B334 File Offset: 0x00079534
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = GameTexts.FindText("str_army_management", null).ToString();
			this.BoostTitleText = GameTexts.FindText("str_boost_cohesion", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.DistanceText = GameTexts.FindText("str_distance", null).ToString();
			this.CostText = GameTexts.FindText("str_cost", null).ToString();
			this.StrengthText = GameTexts.FindText("str_men", null).ToString();
			this.LordsText = GameTexts.FindText("str_leader", null).ToString();
			this.ClanText = GameTexts.FindText("str_clan", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.OwnerText = GameTexts.FindText("str_party", null).ToString();
			this.DisbandArmyText = GameTexts.FindText("str_disband_army", null).ToString();
			this.ShipCountText = new TextObject("{=7Q8ufo5X}Ships", null).ToString();
			this._playerDoesntHaveEnoughInfluenceStr = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null).ToString();
			GameTexts.SetVariable("TOTAL_INFLUENCE", MathF.Round(Hero.MainHero.Clan.Influence));
			this.TotalInfluence = GameTexts.FindText("str_total_influence", null).ToString();
			GameTexts.SetVariable("NUMBER", 10);
			this.CohesionBoostAmountText = GameTexts.FindText("str_plus_with_number", null).ToString();
			this.PartyList.ApplyActionOnAllItems(delegate(ArmyManagementItemVM x)
			{
				x.RefreshValues();
			});
			this.PartiesInCart.ApplyActionOnAllItems(delegate(ArmyManagementItemVM x)
			{
				x.RefreshValues();
			});
			this.TutorialNotification.RefreshValues();
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x0007B530 File Offset: 0x00079730
		private void CalculateCohesion()
		{
			if (MobileParty.MainParty.Army != null)
			{
				this.Cohesion = (int)MobileParty.MainParty.Army.Cohesion;
				this.NewCohesion = MathF.Min(this.Cohesion + this._boostedCohesion, 100);
				ArmyManagementCalculationModel armyManagementCalculationModel = Campaign.Current.Models.ArmyManagementCalculationModel;
				this._currentParties.Clear();
				foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart)
				{
					if (!armyManagementItemVM.Party.IsMainParty)
					{
						this._currentParties.Add(armyManagementItemVM.Party);
						if (!armyManagementItemVM.IsAlreadyWithPlayer)
						{
							this.NewCohesion = armyManagementCalculationModel.CalculateNewCohesion(MobileParty.MainParty.Army, armyManagementItemVM.Party.Party, this.NewCohesion, 1);
						}
					}
				}
			}
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x0007B620 File Offset: 0x00079820
		private void OnFocus(ArmyManagementItemVM focusedItem)
		{
			this.FocusedItem = focusedItem;
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0007B62C File Offset: 0x0007982C
		private void OnAddToCart(ArmyManagementItemVM armyItem)
		{
			if (!this.PartiesInCart.Contains(armyItem))
			{
				this.PartiesInCart.Add(armyItem);
				armyItem.IsInCart = true;
				Game.Current.EventManager.TriggerEvent<PartyAddedToArmyByPlayerEvent>(new PartyAddedToArmyByPlayerEvent(armyItem.Party));
				if (this._partiesToRemove.Contains(armyItem))
				{
					this._partiesToRemove.Remove(armyItem);
				}
				if (armyItem.IsAlreadyWithPlayer)
				{
					armyItem.CanJoinBackWithoutCost = false;
				}
				this.TotalCost += armyItem.Cost;
			}
			this.OnRefresh();
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x0007B6B8 File Offset: 0x000798B8
		private void OnRemove(ArmyManagementItemVM armyItem)
		{
			if (this.PartiesInCart.Contains(armyItem))
			{
				this.PartiesInCart.Remove(armyItem);
				armyItem.IsInCart = false;
				this._partiesToRemove.Add(armyItem);
				if (armyItem.IsAlreadyWithPlayer)
				{
					armyItem.CanJoinBackWithoutCost = true;
				}
				this.TotalCost -= armyItem.Cost;
			}
			this.OnRefresh();
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x0007B71C File Offset: 0x0007991C
		private void UpdateCanConfirm()
		{
			if (!this.CanAffordInfluenceCost)
			{
				this.CanConfirm = false;
				this.DoneHint.HintText = GameTexts.FindText("str_warning_you_dont_have_enough_influence", null).CopyTextObject();
			}
			else if (this.PartiesInCart.Count == 1 && this.PartiesInCart[0].IsMainHero)
			{
				this.CanConfirm = this.CanDisbandArmy;
				if (!this.CanConfirm)
				{
					this.DoneHint.HintText = new TextObject("{=aUq1M6Wa}You need more than 1 party to create an army", null);
				}
			}
			else
			{
				this.CanConfirm = true;
			}
			if (this.CanConfirm)
			{
				this.DoneHint.HintText = TextObject.GetEmpty();
			}
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x0007B7C4 File Offset: 0x000799C4
		private void ApplyCohesionChange()
		{
			if (MobileParty.MainParty.Army != null)
			{
				int num = this.NewCohesion - this.Cohesion;
				MobileParty.MainParty.Army.BoostCohesionWithInfluence((float)num, this._influenceSpentForCohesionBoosting);
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x0007B804 File Offset: 0x00079A04
		private void OnBoostCohesion()
		{
			if (this.CanBoostCohesion)
			{
				this.TotalCost += this.CohesionBoostCost;
				this._boostedCohesion += 10;
				this._influenceSpentForCohesionBoosting += this.CohesionBoostCost;
				this.OnRefresh();
			}
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x0007B854 File Offset: 0x00079A54
		private void OnRefresh()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			float num4 = 0f;
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart)
			{
				num2++;
				num += (int)armyManagementItemVM.Party.Party.EstimatedStrength;
				if (armyManagementItemVM.IsAlreadyWithPlayer)
				{
					num4 += armyManagementItemVM.Party.Food;
					num3 += (int)armyManagementItemVM.Party.Morale;
				}
			}
			this.TotalStrength = num;
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_total_cost", null).ToString());
			this.TotalCostText = GameTexts.FindText("str_LEFT_colon", null).ToString();
			GameTexts.SetVariable("LEFT", this.TotalCost.ToString());
			GameTexts.SetVariable("RIGHT", ((int)Hero.MainHero.Clan.Influence).ToString());
			this.TotalCostNumbersText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			GameTexts.SetVariable("NUM", num2);
			this.TotalLords = GameTexts.FindText("str_NUM_lords", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_strength", null).ToString());
			this.TotalStrengthText = GameTexts.FindText("str_LEFT_colon", null).ToString();
			this.CanCreateArmy = (float)this.TotalCost <= Hero.MainHero.Clan.Influence && num2 > 1;
			bool flag;
			if (MobileParty.MainParty.Army != null)
			{
				if (this._partiesToRemove.Count > 0)
				{
					flag = this.PartiesInCart.Count<ArmyManagementItemVM>((ArmyManagementItemVM p) => p.IsAlreadyWithPlayer) >= 1;
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
			this.PlayerHasArmy = flag;
			this.CalculateCohesion();
			this.CanBoostCohesion = this.PlayerHasArmy && this.NewCohesion + 10 <= 100;
			if (this.CanBoostCohesion)
			{
				TextObject textObject = new TextObject("{=nNZ1ZtTE}Add {BOOSTAMOUNT} cohesion to your army", null);
				textObject.SetTextVariable("BOOSTAMOUNT", 10);
				this.BoostCohesionHint.HintText = textObject;
			}
			else if (this.NewCohesion + 10 > 100)
			{
				TextObject textObject2 = new TextObject("{=rsHPaaYZ}Cohesion needs to be lower than {MINAMOUNT} to boost", null);
				textObject2.SetTextVariable("MINAMOUNT", 90);
				this.BoostCohesionHint.HintText = textObject2;
			}
			else
			{
				this.BoostCohesionHint.HintText = new TextObject("{=Ioiqzz4E}You need to be in an army to boost cohesion", null);
			}
			if (MobileParty.MainParty.Army != null)
			{
				this.CohesionText = GameTexts.FindText("str_cohesion", null).ToString();
				num3 += (int)MobileParty.MainParty.Morale;
				num4 += MobileParty.MainParty.Food;
			}
			this.MoraleText = num3.ToString();
			this.FoodText = MathF.Round(num4, 1).ToString();
			this.PartiesInCart.Sort(this._itemComparer);
			TextObject textObject3;
			this.CanDisbandArmy = this.GetCanDisbandArmyWithReason(out textObject3);
			this.DisbandArmyHint.HintText = textObject3;
			this.UpdateCanConfirm();
			this.UpdateTooltips();
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x0007BB84 File Offset: 0x00079D84
		private bool GetCanDisbandArmyWithReason(out TextObject disabledReason)
		{
			if (MobileParty.MainParty.Army == null)
			{
				disabledReason = new TextObject("{=iSZTOeYH}No army to disband.", null);
				return false;
			}
			if (MobileParty.MainParty.MapEvent != null)
			{
				disabledReason = new TextObject("{=uipNpzVw}Cannot disband the army right now.", null);
				return false;
			}
			if (PlayerSiege.PlayerSiegeEvent != null)
			{
				disabledReason = GameTexts.FindText("str_action_disabled_reason_siege", null);
				return false;
			}
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0007BBF4 File Offset: 0x00079DF4
		private void UpdateTooltips()
		{
			if (this.PlayerHasArmy)
			{
				this.CohesionHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetArmyCohesionTooltip(PartyBase.MainParty.MobileParty.Army));
				PartyBase.MainParty.MobileParty.Army.RecalculateArmyMorale();
				MathF.Round(PartyBase.MainParty.MobileParty.Army.Morale, 1).ToString("0.0");
				MBTextManager.SetTextVariable("BASE_EFFECT", MathF.Round(MobileParty.MainParty.Morale, 1).ToString("0.0"), false);
				MBTextManager.SetTextVariable("STR1", "", false);
				MBTextManager.SetTextVariable("STR2", "", false);
				MBTextManager.SetTextVariable("ARMY_MORALE", MobileParty.MainParty.Army.Morale, 2);
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.Parties)
				{
					MBTextManager.SetTextVariable("STR1", GameTexts.FindText("str_STR1_STR2", null).ToString(), false);
					MBTextManager.SetTextVariable("PARTY_NAME", mobileParty.Name, false);
					MBTextManager.SetTextVariable("PARTY_MORALE", (int)mobileParty.Morale);
					MBTextManager.SetTextVariable("STR2", GameTexts.FindText("str_new_morale_item_line", null), false);
				}
				MBTextManager.SetTextVariable("ARMY_MORALE_ITEMS", GameTexts.FindText("str_STR1_STR2", null).ToString(), false);
				this.MoraleHint.HintText = GameTexts.FindText("str_army_morale_tooltip", null);
			}
			else
			{
				GameTexts.SetVariable("reg1", (int)MobileParty.MainParty.Morale);
				this.MoraleHint.HintText = GameTexts.FindText("str_morale_reg1", null);
			}
			MBTextManager.SetTextVariable("newline", "\n", false);
			MBTextManager.SetTextVariable("DAILY_FOOD_CONSUMPTION", MobileParty.MainParty.FoodChange, 2);
			this.FoodHint.HintText = GameTexts.FindText("str_food_consumption_tooltip", null);
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x0007BE08 File Offset: 0x0007A008
		public void ExecuteDone()
		{
			if (!this.CanAffordInfluenceCost)
			{
				return;
			}
			if (this.PartiesInCart.Count == 1 && this.PartiesInCart[0].IsMainHero)
			{
				this.ExecuteDisbandArmy();
				return;
			}
			if (this.NewCohesion > this.Cohesion)
			{
				this.ApplyCohesionChange();
			}
			if (this.PartiesInCart.Count > 1 && MobileParty.MainParty.MapFaction.IsKingdomFaction)
			{
				if (MobileParty.MainParty.Army == null)
				{
					((Kingdom)MobileParty.MainParty.MapFaction).CreateArmy(Hero.MainHero, Hero.MainHero.HomeSettlement, Army.ArmyTypes.Defender, null);
				}
				foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart)
				{
					if (armyManagementItemVM.Party != MobileParty.MainParty)
					{
						armyManagementItemVM.Party.Army = MobileParty.MainParty.Army;
					}
				}
				ChangeClanInfluenceAction.Apply(Clan.PlayerClan, (float)(-(float)(this.TotalCost - this._influenceSpentForCohesionBoosting)));
			}
			if (this._partiesToRemove.Count > 0)
			{
				bool flag = false;
				foreach (ArmyManagementItemVM armyManagementItemVM2 in this._partiesToRemove)
				{
					if (armyManagementItemVM2.Party == MobileParty.MainParty)
					{
						armyManagementItemVM2.Party.Army = null;
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (ArmyManagementItemVM armyManagementItemVM3 in this._partiesToRemove)
					{
						Army army = MobileParty.MainParty.Army;
						if (army != null && army.Parties.Contains(armyManagementItemVM3.Party))
						{
							armyManagementItemVM3.Party.Army = null;
						}
					}
				}
				this._partiesToRemove.Clear();
			}
			this._onClose();
			CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x0007C014 File Offset: 0x0007A214
		public void ExecuteCancel()
		{
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, this._initialInfluence - Clan.PlayerClan.Influence);
			this._onClose();
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x0007C03C File Offset: 0x0007A23C
		public void ExecuteReset()
		{
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart.ToList<ArmyManagementItemVM>())
			{
				this.OnRemove(armyManagementItemVM);
				armyManagementItemVM.UpdateEligibility();
			}
			this.PartiesInCart.Add(this._mainPartyItem);
			foreach (ArmyManagementItemVM armyManagementItemVM2 in this.PartyList)
			{
				if (armyManagementItemVM2.IsAlreadyWithPlayer)
				{
					this.PartiesInCart.Add(armyManagementItemVM2);
					armyManagementItemVM2.IsInCart = true;
					armyManagementItemVM2.CanJoinBackWithoutCost = false;
				}
			}
			this.NewCohesion = this.Cohesion;
			ChangeClanInfluenceAction.Apply(Clan.PlayerClan, this._initialInfluence - Clan.PlayerClan.Influence);
			this.TotalCost = 0;
			this._boostedCohesion = 0;
			this._influenceSpentForCohesionBoosting = 0;
			this._partiesToRemove.Clear();
			this.OnRefresh();
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x0007C150 File Offset: 0x0007A350
		public void ExecuteDisbandArmy()
		{
			if (this.CanDisbandArmy)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=ViYdZUbQ}Disband Army", null).ToString(), new TextObject("{=kqeA8rjL}Are you sure you want to disband your army?", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					this.DisbandArmy();
				}, null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x0007C1CD File Offset: 0x0007A3CD
		public void ExecuteBoostCohesionManual()
		{
			this.OnBoostCohesion();
			Game.Current.EventManager.TriggerEvent<ArmyCohesionBoostedByPlayerEvent>(new ArmyCohesionBoostedByPlayerEvent());
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x0007C1EC File Offset: 0x0007A3EC
		private void DisbandArmy()
		{
			foreach (ArmyManagementItemVM armyManagementItemVM in this.PartiesInCart.ToList<ArmyManagementItemVM>())
			{
				this.OnRemove(armyManagementItemVM);
			}
			this.ExecuteDone();
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x0007C24C File Offset: 0x0007A44C
		private void OnCloseBoost()
		{
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.ArmyManagement));
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x0007C264 File Offset: 0x0007A464
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (obj.NewNotificationElementID != this._latestTutorialElementID)
			{
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = string.Empty;
				}
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._latestTutorialElementID != null)
				{
					this.TutorialNotification.ElementID = this._latestTutorialElementID;
				}
			}
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x0007C2C4 File Offset: 0x0007A4C4
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey != null)
			{
				cancelInputKey.OnFinalize();
			}
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM resetInputKey = this.ResetInputKey;
			if (resetInputKey != null)
			{
				resetInputKey.OnFinalize();
			}
			InputKeyItemVM removeInputKey = this.RemoveInputKey;
			if (removeInputKey == null)
			{
				return;
			}
			removeInputKey.OnFinalize();
		}

		// Token: 0x17000BE9 RID: 3049
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x0007C335 File Offset: 0x0007A535
		// (set) Token: 0x060022AF RID: 8879 RVA: 0x0007C33D File Offset: 0x0007A53D
		[DataSourceProperty]
		public ElementNotificationVM TutorialNotification
		{
			get
			{
				return this._tutorialNotification;
			}
			set
			{
				if (value != this._tutorialNotification)
				{
					this._tutorialNotification = value;
					base.OnPropertyChangedWithValue<ElementNotificationVM>(value, "TutorialNotification");
				}
			}
		}

		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x0007C35B File Offset: 0x0007A55B
		// (set) Token: 0x060022B1 RID: 8881 RVA: 0x0007C363 File Offset: 0x0007A563
		[DataSourceProperty]
		public ArmyManagementSortControllerVM SortControllerVM
		{
			get
			{
				return this._sortControllerVM;
			}
			set
			{
				if (value != this._sortControllerVM)
				{
					this._sortControllerVM = value;
					base.OnPropertyChangedWithValue<ArmyManagementSortControllerVM>(value, "SortControllerVM");
				}
			}
		}

		// Token: 0x17000BEB RID: 3051
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x0007C381 File Offset: 0x0007A581
		// (set) Token: 0x060022B3 RID: 8883 RVA: 0x0007C389 File Offset: 0x0007A589
		[DataSourceProperty]
		public string BoostTitleText
		{
			get
			{
				return this._boostTitleText;
			}
			set
			{
				if (value != this._boostTitleText)
				{
					this._boostTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "BoostTitleText");
				}
			}
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x0007C3AC File Offset: 0x0007A5AC
		// (set) Token: 0x060022B5 RID: 8885 RVA: 0x0007C3B4 File Offset: 0x0007A5B4
		[DataSourceProperty]
		public string DisbandArmyText
		{
			get
			{
				return this._disbandArmyText;
			}
			set
			{
				if (value != this._disbandArmyText)
				{
					this._disbandArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandArmyText");
				}
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060022B6 RID: 8886 RVA: 0x0007C3D7 File Offset: 0x0007A5D7
		// (set) Token: 0x060022B7 RID: 8887 RVA: 0x0007C3DF File Offset: 0x0007A5DF
		[DataSourceProperty]
		public string CohesionBoostAmountText
		{
			get
			{
				return this._cohesionBoostAmountText;
			}
			set
			{
				if (value != this._cohesionBoostAmountText)
				{
					this._cohesionBoostAmountText = value;
					base.OnPropertyChangedWithValue<string>(value, "CohesionBoostAmountText");
				}
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x0007C402 File Offset: 0x0007A602
		// (set) Token: 0x060022B9 RID: 8889 RVA: 0x0007C40A File Offset: 0x0007A60A
		[DataSourceProperty]
		public string DistanceText
		{
			get
			{
				return this._distanceText;
			}
			set
			{
				if (value != this._distanceText)
				{
					this._distanceText = value;
					base.OnPropertyChangedWithValue<string>(value, "DistanceText");
				}
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x0007C42D File Offset: 0x0007A62D
		// (set) Token: 0x060022BB RID: 8891 RVA: 0x0007C435 File Offset: 0x0007A635
		[DataSourceProperty]
		public string CostText
		{
			get
			{
				return this._costText;
			}
			set
			{
				if (value != this._costText)
				{
					this._costText = value;
					base.OnPropertyChangedWithValue<string>(value, "CostText");
				}
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x0007C458 File Offset: 0x0007A658
		// (set) Token: 0x060022BD RID: 8893 RVA: 0x0007C460 File Offset: 0x0007A660
		[DataSourceProperty]
		public string OwnerText
		{
			get
			{
				return this._ownerText;
			}
			set
			{
				if (value != this._ownerText)
				{
					this._ownerText = value;
					base.OnPropertyChangedWithValue<string>(value, "OwnerText");
				}
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x0007C483 File Offset: 0x0007A683
		// (set) Token: 0x060022BF RID: 8895 RVA: 0x0007C48B File Offset: 0x0007A68B
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._strengthText;
			}
			set
			{
				if (value != this._strengthText)
				{
					this._strengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthText");
				}
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x0007C4AE File Offset: 0x0007A6AE
		// (set) Token: 0x060022C1 RID: 8897 RVA: 0x0007C4B6 File Offset: 0x0007A6B6
		[DataSourceProperty]
		public string ShipCountText
		{
			get
			{
				return this._shipCountText;
			}
			set
			{
				if (value != this._shipCountText)
				{
					this._shipCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipCountText");
				}
			}
		}

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x0007C4D9 File Offset: 0x0007A6D9
		// (set) Token: 0x060022C3 RID: 8899 RVA: 0x0007C4E1 File Offset: 0x0007A6E1
		[DataSourceProperty]
		public string LordsText
		{
			get
			{
				return this._lordsText;
			}
			set
			{
				if (value != this._lordsText)
				{
					this._lordsText = value;
					base.OnPropertyChangedWithValue<string>(value, "LordsText");
				}
			}
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x060022C4 RID: 8900 RVA: 0x0007C504 File Offset: 0x0007A704
		// (set) Token: 0x060022C5 RID: 8901 RVA: 0x0007C50C File Offset: 0x0007A70C
		[DataSourceProperty]
		public string TotalInfluence
		{
			get
			{
				return this._totalInfluence;
			}
			set
			{
				if (value != this._totalInfluence)
				{
					this._totalInfluence = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalInfluence");
				}
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x0007C52F File Offset: 0x0007A72F
		// (set) Token: 0x060022C7 RID: 8903 RVA: 0x0007C537 File Offset: 0x0007A737
		[DataSourceProperty]
		public int TotalStrength
		{
			get
			{
				return this._totalStrength;
			}
			set
			{
				if (value != this._totalStrength)
				{
					this._totalStrength = value;
					base.OnPropertyChangedWithValue(value, "TotalStrength");
				}
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x0007C555 File Offset: 0x0007A755
		// (set) Token: 0x060022C9 RID: 8905 RVA: 0x0007C560 File Offset: 0x0007A760
		[DataSourceProperty]
		public int TotalCost
		{
			get
			{
				return this._totalCost;
			}
			set
			{
				if (value != this._totalCost)
				{
					this._totalCost = value;
					this.CanAffordInfluenceCost = this.TotalCost <= 0 || (float)this.TotalCost <= Hero.MainHero.Clan.Influence;
					base.OnPropertyChangedWithValue(value, "TotalCost");
				}
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x0007C5B6 File Offset: 0x0007A7B6
		// (set) Token: 0x060022CB RID: 8907 RVA: 0x0007C5BE File Offset: 0x0007A7BE
		[DataSourceProperty]
		public string TotalLords
		{
			get
			{
				return this._totalLords;
			}
			set
			{
				if (value != this._totalLords)
				{
					this._totalLords = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalLords");
				}
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x060022CC RID: 8908 RVA: 0x0007C5E1 File Offset: 0x0007A7E1
		// (set) Token: 0x060022CD RID: 8909 RVA: 0x0007C5E9 File Offset: 0x0007A7E9
		[DataSourceProperty]
		public bool CanCreateArmy
		{
			get
			{
				return this._canCreateArmy;
			}
			set
			{
				if (value != this._canCreateArmy)
				{
					this._canCreateArmy = value;
					base.OnPropertyChangedWithValue(value, "CanCreateArmy");
				}
			}
		}

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x0007C607 File Offset: 0x0007A807
		// (set) Token: 0x060022CF RID: 8911 RVA: 0x0007C60F File Offset: 0x0007A80F
		[DataSourceProperty]
		public bool CanBoostCohesion
		{
			get
			{
				return this._canBoostCohesion;
			}
			set
			{
				if (value != this._canBoostCohesion)
				{
					this._canBoostCohesion = value;
					base.OnPropertyChangedWithValue(value, "CanBoostCohesion");
				}
			}
		}

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x0007C62D File Offset: 0x0007A82D
		// (set) Token: 0x060022D1 RID: 8913 RVA: 0x0007C635 File Offset: 0x0007A835
		[DataSourceProperty]
		public bool CanDisbandArmy
		{
			get
			{
				return this._canDisbandArmy;
			}
			set
			{
				if (value != this._canDisbandArmy)
				{
					this._canDisbandArmy = value;
					base.OnPropertyChangedWithValue(value, "CanDisbandArmy");
				}
			}
		}

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x0007C653 File Offset: 0x0007A853
		// (set) Token: 0x060022D3 RID: 8915 RVA: 0x0007C65B File Offset: 0x0007A85B
		[DataSourceProperty]
		public bool CanConfirm
		{
			get
			{
				return this._canConfirm;
			}
			set
			{
				if (value != this._canConfirm)
				{
					this._canConfirm = value;
					base.OnPropertyChangedWithValue(value, "CanConfirm");
				}
			}
		}

		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x0007C679 File Offset: 0x0007A879
		// (set) Token: 0x060022D5 RID: 8917 RVA: 0x0007C681 File Offset: 0x0007A881
		[DataSourceProperty]
		public bool CanAffordInfluenceCost
		{
			get
			{
				return this._canAffordInfluenceCost;
			}
			set
			{
				if (value != this._canAffordInfluenceCost)
				{
					this._canAffordInfluenceCost = value;
					base.OnPropertyChangedWithValue(value, "CanAffordInfluenceCost");
				}
			}
		}

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x0007C69F File Offset: 0x0007A89F
		// (set) Token: 0x060022D7 RID: 8919 RVA: 0x0007C6A7 File Offset: 0x0007A8A7
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

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x0007C6CA File Offset: 0x0007A8CA
		// (set) Token: 0x060022D9 RID: 8921 RVA: 0x0007C6D2 File Offset: 0x0007A8D2
		[DataSourceProperty]
		public string ClanText
		{
			get
			{
				return this._clanText;
			}
			set
			{
				if (value != this._clanText)
				{
					this._clanText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClanText");
				}
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x0007C6F5 File Offset: 0x0007A8F5
		// (set) Token: 0x060022DB RID: 8923 RVA: 0x0007C6FD File Offset: 0x0007A8FD
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x0007C720 File Offset: 0x0007A920
		// (set) Token: 0x060022DD RID: 8925 RVA: 0x0007C728 File Offset: 0x0007A928
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

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x0007C74B File Offset: 0x0007A94B
		// (set) Token: 0x060022DF RID: 8927 RVA: 0x0007C753 File Offset: 0x0007A953
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

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x060022E0 RID: 8928 RVA: 0x0007C776 File Offset: 0x0007A976
		// (set) Token: 0x060022E1 RID: 8929 RVA: 0x0007C77E File Offset: 0x0007A97E
		[DataSourceProperty]
		public ArmyManagementItemVM FocusedItem
		{
			get
			{
				return this._focusedItem;
			}
			set
			{
				if (value != this._focusedItem)
				{
					this._focusedItem = value;
					base.OnPropertyChangedWithValue<ArmyManagementItemVM>(value, "FocusedItem");
				}
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x0007C79C File Offset: 0x0007A99C
		// (set) Token: 0x060022E3 RID: 8931 RVA: 0x0007C7A4 File Offset: 0x0007A9A4
		[DataSourceProperty]
		public MBBindingList<ArmyManagementItemVM> PartyList
		{
			get
			{
				return this._partyList;
			}
			set
			{
				if (value != this._partyList)
				{
					this._partyList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ArmyManagementItemVM>>(value, "PartyList");
				}
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x0007C7C2 File Offset: 0x0007A9C2
		// (set) Token: 0x060022E5 RID: 8933 RVA: 0x0007C7CA File Offset: 0x0007A9CA
		[DataSourceProperty]
		public MBBindingList<ArmyManagementItemVM> PartiesInCart
		{
			get
			{
				return this._partiesInCart;
			}
			set
			{
				if (value != this._partiesInCart)
				{
					this._partiesInCart = value;
					base.OnPropertyChangedWithValue<MBBindingList<ArmyManagementItemVM>>(value, "PartiesInCart");
				}
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0007C7E8 File Offset: 0x0007A9E8
		// (set) Token: 0x060022E7 RID: 8935 RVA: 0x0007C7F0 File Offset: 0x0007A9F0
		[DataSourceProperty]
		public string TotalStrengthText
		{
			get
			{
				return this._totalStrengthText;
			}
			set
			{
				if (value != this._totalStrengthText)
				{
					this._totalStrengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalStrengthText");
				}
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x0007C813 File Offset: 0x0007AA13
		// (set) Token: 0x060022E9 RID: 8937 RVA: 0x0007C81B File Offset: 0x0007AA1B
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

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x0007C83E File Offset: 0x0007AA3E
		// (set) Token: 0x060022EB RID: 8939 RVA: 0x0007C846 File Offset: 0x0007AA46
		[DataSourceProperty]
		public string TotalCostNumbersText
		{
			get
			{
				return this._totalCostNumbersText;
			}
			set
			{
				if (value != this._totalCostNumbersText)
				{
					this._totalCostNumbersText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalCostNumbersText");
				}
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x0007C869 File Offset: 0x0007AA69
		// (set) Token: 0x060022ED RID: 8941 RVA: 0x0007C871 File Offset: 0x0007AA71
		[DataSourceProperty]
		public string CohesionText
		{
			get
			{
				return this._cohesionText;
			}
			set
			{
				if (value != this._cohesionText)
				{
					this._cohesionText = value;
					base.OnPropertyChangedWithValue<string>(value, "CohesionText");
				}
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x0007C894 File Offset: 0x0007AA94
		// (set) Token: 0x060022EF RID: 8943 RVA: 0x0007C89C File Offset: 0x0007AA9C
		[DataSourceProperty]
		public int Cohesion
		{
			get
			{
				return this._cohesion;
			}
			set
			{
				if (value != this._cohesion)
				{
					this._cohesion = value;
					base.OnPropertyChangedWithValue(value, "Cohesion");
				}
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x0007C8BA File Offset: 0x0007AABA
		// (set) Token: 0x060022F1 RID: 8945 RVA: 0x0007C8C2 File Offset: 0x0007AAC2
		[DataSourceProperty]
		public int CohesionBoostCost
		{
			get
			{
				return this._cohesionBoostCost;
			}
			set
			{
				if (value != this._cohesionBoostCost)
				{
					this._cohesionBoostCost = value;
					base.OnPropertyChangedWithValue(value, "CohesionBoostCost");
				}
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x0007C8E0 File Offset: 0x0007AAE0
		// (set) Token: 0x060022F3 RID: 8947 RVA: 0x0007C8E8 File Offset: 0x0007AAE8
		[DataSourceProperty]
		public bool PlayerHasArmy
		{
			get
			{
				return this._playerHasArmy;
			}
			set
			{
				if (value != this._playerHasArmy)
				{
					this._playerHasArmy = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasArmy");
				}
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x060022F4 RID: 8948 RVA: 0x0007C906 File Offset: 0x0007AB06
		// (set) Token: 0x060022F5 RID: 8949 RVA: 0x0007C90E File Offset: 0x0007AB0E
		[DataSourceProperty]
		public string MoraleText
		{
			get
			{
				return this._moraleText;
			}
			set
			{
				if (value != this._moraleText)
				{
					this._moraleText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoraleText");
				}
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x060022F6 RID: 8950 RVA: 0x0007C931 File Offset: 0x0007AB31
		// (set) Token: 0x060022F7 RID: 8951 RVA: 0x0007C939 File Offset: 0x0007AB39
		[DataSourceProperty]
		public string FoodText
		{
			get
			{
				return this._foodText;
			}
			set
			{
				if (value != this._foodText)
				{
					this._foodText = value;
					base.OnPropertyChangedWithValue<string>(value, "FoodText");
				}
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x060022F8 RID: 8952 RVA: 0x0007C95C File Offset: 0x0007AB5C
		// (set) Token: 0x060022F9 RID: 8953 RVA: 0x0007C964 File Offset: 0x0007AB64
		[DataSourceProperty]
		public int NewCohesion
		{
			get
			{
				return this._newCohesion;
			}
			set
			{
				if (value != this._newCohesion)
				{
					this._newCohesion = value;
					base.OnPropertyChangedWithValue(value, "NewCohesion");
				}
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060022FA RID: 8954 RVA: 0x0007C982 File Offset: 0x0007AB82
		// (set) Token: 0x060022FB RID: 8955 RVA: 0x0007C98A File Offset: 0x0007AB8A
		[DataSourceProperty]
		public BasicTooltipViewModel CohesionHint
		{
			get
			{
				return this._cohesionHint;
			}
			set
			{
				if (value != this._cohesionHint)
				{
					this._cohesionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CohesionHint");
				}
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x0007C9A8 File Offset: 0x0007ABA8
		// (set) Token: 0x060022FD RID: 8957 RVA: 0x0007C9B0 File Offset: 0x0007ABB0
		[DataSourceProperty]
		public HintViewModel MoraleHint
		{
			get
			{
				return this._moraleHint;
			}
			set
			{
				if (value != this._moraleHint)
				{
					this._moraleHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "MoraleHint");
				}
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x0007C9CE File Offset: 0x0007ABCE
		// (set) Token: 0x060022FF RID: 8959 RVA: 0x0007C9D6 File Offset: 0x0007ABD6
		[DataSourceProperty]
		public HintViewModel BoostCohesionHint
		{
			get
			{
				return this._boostCohesionHint;
			}
			set
			{
				if (value != this._boostCohesionHint)
				{
					this._boostCohesionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "BoostCohesionHint");
				}
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06002300 RID: 8960 RVA: 0x0007C9F4 File Offset: 0x0007ABF4
		// (set) Token: 0x06002301 RID: 8961 RVA: 0x0007C9FC File Offset: 0x0007ABFC
		[DataSourceProperty]
		public HintViewModel DisbandArmyHint
		{
			get
			{
				return this._disbandArmyHint;
			}
			set
			{
				if (value != this._disbandArmyHint)
				{
					this._disbandArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "DisbandArmyHint");
				}
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06002302 RID: 8962 RVA: 0x0007CA1A File Offset: 0x0007AC1A
		// (set) Token: 0x06002303 RID: 8963 RVA: 0x0007CA22 File Offset: 0x0007AC22
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

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x0007CA40 File Offset: 0x0007AC40
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x0007CA48 File Offset: 0x0007AC48
		[DataSourceProperty]
		public HintViewModel FoodHint
		{
			get
			{
				return this._foodHint;
			}
			set
			{
				if (value != this._foodHint)
				{
					this._foodHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FoodHint");
				}
			}
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0007CA66 File Offset: 0x0007AC66
		public void SetResetInputKey(HotKey hotKey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0007CA75 File Offset: 0x0007AC75
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0007CA84 File Offset: 0x0007AC84
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0007CA93 File Offset: 0x0007AC93
		public void SetRemoveInputKey(HotKey hotKey)
		{
			this.RemoveInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x0007CAA2 File Offset: 0x0007ACA2
		// (set) Token: 0x0600230B RID: 8971 RVA: 0x0007CAAA File Offset: 0x0007ACAA
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

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x0007CAC8 File Offset: 0x0007ACC8
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x0007CAD0 File Offset: 0x0007ACD0
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

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x0007CAEE File Offset: 0x0007ACEE
		// (set) Token: 0x0600230F RID: 8975 RVA: 0x0007CAF6 File Offset: 0x0007ACF6
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

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x0007CB14 File Offset: 0x0007AD14
		// (set) Token: 0x06002311 RID: 8977 RVA: 0x0007CB1C File Offset: 0x0007AD1C
		[DataSourceProperty]
		public InputKeyItemVM RemoveInputKey
		{
			get
			{
				return this._removeInputKey;
			}
			set
			{
				if (value != this._removeInputKey)
				{
					this._removeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RemoveInputKey");
					foreach (ArmyManagementItemVM armyManagementItemVM in this.PartyList)
					{
						armyManagementItemVM.RemoveInputKey = value;
					}
				}
			}
		}

		// Token: 0x04000FD4 RID: 4052
		private readonly Action _onClose;

		// Token: 0x04000FD5 RID: 4053
		private readonly ArmyManagementItemVM _mainPartyItem;

		// Token: 0x04000FD6 RID: 4054
		private readonly ArmyManagementVM.ManagementItemComparer _itemComparer;

		// Token: 0x04000FD7 RID: 4055
		private readonly float _initialInfluence;

		// Token: 0x04000FD8 RID: 4056
		private string _latestTutorialElementID;

		// Token: 0x04000FD9 RID: 4057
		private string _playerDoesntHaveEnoughInfluenceStr;

		// Token: 0x04000FDA RID: 4058
		private const int _cohesionBoostAmount = 10;

		// Token: 0x04000FDB RID: 4059
		private int _influenceSpentForCohesionBoosting;

		// Token: 0x04000FDC RID: 4060
		private int _boostedCohesion;

		// Token: 0x04000FDD RID: 4061
		private string _titleText;

		// Token: 0x04000FDE RID: 4062
		private string _boostTitleText;

		// Token: 0x04000FDF RID: 4063
		private string _cancelText;

		// Token: 0x04000FE0 RID: 4064
		private string _doneText;

		// Token: 0x04000FE1 RID: 4065
		private bool _canCreateArmy;

		// Token: 0x04000FE2 RID: 4066
		private bool _canBoostCohesion;

		// Token: 0x04000FE3 RID: 4067
		private List<MobileParty> _currentParties;

		// Token: 0x04000FE4 RID: 4068
		private ArmyManagementItemVM _focusedItem;

		// Token: 0x04000FE5 RID: 4069
		private MBBindingList<ArmyManagementItemVM> _partyList;

		// Token: 0x04000FE6 RID: 4070
		private MBBindingList<ArmyManagementItemVM> _partiesInCart;

		// Token: 0x04000FE7 RID: 4071
		private MBBindingList<ArmyManagementItemVM> _partiesToRemove;

		// Token: 0x04000FE8 RID: 4072
		private ArmyManagementSortControllerVM _sortControllerVM;

		// Token: 0x04000FE9 RID: 4073
		private int _totalStrength;

		// Token: 0x04000FEA RID: 4074
		private int _totalCost;

		// Token: 0x04000FEB RID: 4075
		private int _cohesion;

		// Token: 0x04000FEC RID: 4076
		private int _cohesionBoostCost;

		// Token: 0x04000FED RID: 4077
		private string _cohesionText;

		// Token: 0x04000FEE RID: 4078
		private int _newCohesion;

		// Token: 0x04000FEF RID: 4079
		private string _totalStrengthText;

		// Token: 0x04000FF0 RID: 4080
		private string _totalCostText;

		// Token: 0x04000FF1 RID: 4081
		private string _totalCostNumbersText;

		// Token: 0x04000FF2 RID: 4082
		private string _totalInfluence;

		// Token: 0x04000FF3 RID: 4083
		private string _totalLords;

		// Token: 0x04000FF4 RID: 4084
		private string _costText;

		// Token: 0x04000FF5 RID: 4085
		private string _strengthText;

		// Token: 0x04000FF6 RID: 4086
		private string _shipCountText;

		// Token: 0x04000FF7 RID: 4087
		private string _lordsText;

		// Token: 0x04000FF8 RID: 4088
		private string _distanceText;

		// Token: 0x04000FF9 RID: 4089
		private string _clanText;

		// Token: 0x04000FFA RID: 4090
		private string _ownerText;

		// Token: 0x04000FFB RID: 4091
		private string _nameText;

		// Token: 0x04000FFC RID: 4092
		private string _disbandArmyText;

		// Token: 0x04000FFD RID: 4093
		private string _cohesionBoostAmountText;

		// Token: 0x04000FFE RID: 4094
		private bool _playerHasArmy;

		// Token: 0x04000FFF RID: 4095
		private bool _canDisbandArmy;

		// Token: 0x04001000 RID: 4096
		private bool _canConfirm;

		// Token: 0x04001001 RID: 4097
		private bool _canAffordInfluenceCost;

		// Token: 0x04001002 RID: 4098
		private string _moraleText;

		// Token: 0x04001003 RID: 4099
		private string _foodText;

		// Token: 0x04001004 RID: 4100
		private BasicTooltipViewModel _cohesionHint;

		// Token: 0x04001005 RID: 4101
		private HintViewModel _moraleHint;

		// Token: 0x04001006 RID: 4102
		private HintViewModel _foodHint;

		// Token: 0x04001007 RID: 4103
		private HintViewModel _boostCohesionHint;

		// Token: 0x04001008 RID: 4104
		private HintViewModel _disbandArmyHint;

		// Token: 0x04001009 RID: 4105
		private HintViewModel _doneHint;

		// Token: 0x0400100A RID: 4106
		public ElementNotificationVM _tutorialNotification;

		// Token: 0x0400100B RID: 4107
		private InputKeyItemVM _resetInputKey;

		// Token: 0x0400100C RID: 4108
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400100D RID: 4109
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400100E RID: 4110
		private InputKeyItemVM _removeInputKey;

		// Token: 0x020002FB RID: 763
		public class ManagementItemComparer : IComparer<ArmyManagementItemVM>
		{
			// Token: 0x06002895 RID: 10389 RVA: 0x0008751C File Offset: 0x0008571C
			public int Compare(ArmyManagementItemVM x, ArmyManagementItemVM y)
			{
				if (x.IsMainHero)
				{
					return -1;
				}
				return y.IsAlreadyWithPlayer.CompareTo(x.IsAlreadyWithPlayer);
			}
		}
	}
}
