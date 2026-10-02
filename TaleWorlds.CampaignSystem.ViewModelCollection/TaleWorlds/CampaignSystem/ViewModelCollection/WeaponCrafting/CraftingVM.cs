using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.CraftingSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Refinement;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.Smelting;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign;
using TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign.Order;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x02000104 RID: 260
	public class CraftingVM : ViewModel
	{
		// Token: 0x060016FD RID: 5885 RVA: 0x000592A4 File Offset: 0x000574A4
		public CraftingVM(Crafting crafting, Action onClose, Action resetCamera, Action onWeaponCrafted, Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> getItemUsageSetFlags)
		{
			this._crafting = crafting;
			this._onClose = onClose;
			this._resetCamera = resetCamera;
			this._onWeaponCrafted = onWeaponCrafted;
			this._craftingBehavior = Campaign.Current.GetCampaignBehavior<ICraftingCampaignBehavior>();
			this._getItemUsageSetFlags = getItemUsageSetFlags;
			this.AvailableCharactersForSmithing = new MBBindingList<CraftingAvailableHeroItemVM>();
			this.MainActionHint = new BasicTooltipViewModel();
			this.TutorialNotification = new ElementNotificationVM();
			this.CameraControlKeys = new MBBindingList<InputKeyItemVM>();
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
			{
				IEnumerable<Hero> availableHeroesForCrafting = CraftingHelper.GetAvailableHeroesForCrafting();
				Hero activeCraftingHero = this._craftingBehavior.GetActiveCraftingHero();
				foreach (Hero hero in availableHeroesForCrafting)
				{
					CraftingAvailableHeroItemVM craftingAvailableHeroItemVM = new CraftingAvailableHeroItemVM(hero, new Action<CraftingAvailableHeroItemVM>(this.UpdateCraftingHero));
					this.AvailableCharactersForSmithing.Add(craftingAvailableHeroItemVM);
					if (hero == activeCraftingHero)
					{
						this.CurrentCraftingHero = craftingAvailableHeroItemVM;
					}
				}
				if (this.CurrentCraftingHero == null)
				{
					this.CurrentCraftingHero = this.AvailableCharactersForSmithing.FirstOrDefault<CraftingAvailableHeroItemVM>();
				}
			}
			else
			{
				this.CurrentCraftingHero = new CraftingAvailableHeroItemVM(Hero.MainHero, new Action<CraftingAvailableHeroItemVM>(this.UpdateCraftingHero));
			}
			this.UpdateCurrentMaterialsAvailable();
			this.Smelting = new SmeltingVM(new Action(this.OnSmeltItemSelection), new Action(this.UpdateAll));
			this.Refinement = new RefinementVM(new Action(this.OnRefinementSelectionChange), new Func<CraftingAvailableHeroItemVM>(this.GetCurrentCraftingHero));
			this.WeaponDesign = new WeaponDesignVM(this._crafting, this._craftingBehavior, new Action(this.OnRequireUpdateFromWeaponDesign), this._onWeaponCrafted, new Func<CraftingAvailableHeroItemVM>(this.GetCurrentCraftingHero), new Action<CraftingOrder>(this.RefreshHeroAvailabilities), this._getItemUsageSetFlags);
			this.CraftingHeroPopup = new CraftingHeroPopupVM(new Func<MBBindingList<CraftingAvailableHeroItemVM>>(this.GetCraftingHeroes));
			this.UpdateCraftingPerks();
			this.ExecuteSwitchToCrafting();
			this.RefreshValues();
			Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000594A8 File Offset: 0x000576A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneLbl = GameTexts.FindText("str_done", null).ToString();
			this.CancelLbl = GameTexts.FindText("str_exit", null).ToString();
			this.ResetCameraHint = new HintViewModel(GameTexts.FindText("str_reset_camera", null), null);
			this.CraftingHint = new HintViewModel(GameTexts.FindText("str_crafting", null), null);
			this.RefiningHint = new HintViewModel(GameTexts.FindText("str_refining", null), null);
			this.SmeltingHint = new HintViewModel(GameTexts.FindText("str_smelting", null), null);
			this.RefinementText = GameTexts.FindText("str_crafting_category_refinement", null).ToString();
			this.CraftingText = GameTexts.FindText("str_crafting_category_crafting", null).ToString();
			this.SmeltingText = GameTexts.FindText("str_crafting_category_smelting", null).ToString();
			this.SelectItemToSmeltText = new TextObject("{=rUeWBOOi}Select an item to smelt", null).ToString();
			this.SelectItemToRefineText = new TextObject("{=BqLsZhhr}Select an item to refine", null).ToString();
			this.TutorialNotification.RefreshValues();
			this._availableCharactersForSmithing.ApplyActionOnAllItems(delegate(CraftingAvailableHeroItemVM x)
			{
				x.RefreshValues();
			});
			this._playerCurrentMaterials.ApplyActionOnAllItems(delegate(CraftingResourceItemVM x)
			{
				x.RefreshValues();
			});
			CraftingAvailableHeroItemVM currentCraftingHero = this._currentCraftingHero;
			if (currentCraftingHero != null)
			{
				currentCraftingHero.RefreshValues();
			}
			this.CraftingHeroPopup.RefreshValues();
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x0005962C File Offset: 0x0005782C
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.WeaponDesign.OnFinalize();
			this.CraftingHeroPopup.OnFinalize();
			InputKeyItemVM confirmInputKey = this.ConfirmInputKey;
			if (confirmInputKey != null)
			{
				confirmInputKey.OnFinalize();
			}
			InputKeyItemVM exitInputKey = this.ExitInputKey;
			if (exitInputKey != null)
			{
				exitInputKey.OnFinalize();
			}
			InputKeyItemVM previousTabInputKey = this.PreviousTabInputKey;
			if (previousTabInputKey != null)
			{
				previousTabInputKey.OnFinalize();
			}
			InputKeyItemVM nextTabInputKey = this.NextTabInputKey;
			if (nextTabInputKey != null)
			{
				nextTabInputKey.OnFinalize();
			}
			foreach (InputKeyItemVM inputKeyItemVM in this.CameraControlKeys)
			{
				if (inputKeyItemVM != null)
				{
					inputKeyItemVM.OnFinalize();
				}
			}
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			EventManager eventManager = game.EventManager;
			if (eventManager == null)
			{
				return;
			}
			eventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00059704 File Offset: 0x00057904
		private void OnRequireUpdateFromWeaponDesign()
		{
			CraftingVM.OnItemRefreshedDelegate onItemRefreshed = this.OnItemRefreshed;
			if (onItemRefreshed != null)
			{
				onItemRefreshed(true);
			}
			this.UpdateAll();
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x0005971E File Offset: 0x0005791E
		public void OnCraftingLogicRefreshed(Crafting newCraftingLogic)
		{
			this._crafting = newCraftingLogic;
			this.WeaponDesign.OnCraftingLogicRefreshed(newCraftingLogic);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00059734 File Offset: 0x00057934
		private void UpdateCurrentMaterialCosts()
		{
			for (int i = 0; i < 9; i++)
			{
				this.PlayerCurrentMaterials[i].ResourceAmount = MobileParty.MainParty.ItemRoster.GetItemNumber(this.PlayerCurrentMaterials[i].ResourceItem);
				this.PlayerCurrentMaterials[i].ResourceChangeAmount = 0;
			}
			if (this.IsInSmeltingMode)
			{
				if (this.Smelting.CurrentSelectedItem != null)
				{
					int[] smeltingOutputForItem = Campaign.Current.Models.SmithingModel.GetSmeltingOutputForItem(this.Smelting.CurrentSelectedItem.EquipmentElement.Item);
					for (int j = 0; j < 9; j++)
					{
						this.PlayerCurrentMaterials[j].ResourceChangeAmount = smeltingOutputForItem[j];
					}
					return;
				}
			}
			else
			{
				if (this.IsInRefinementMode)
				{
					RefinementActionItemVM currentSelectedAction = this.Refinement.CurrentSelectedAction;
					if (currentSelectedAction == null)
					{
						return;
					}
					Crafting.RefiningFormula refineFormula = currentSelectedAction.RefineFormula;
					SmithingModel smithingModel = Campaign.Current.Models.SmithingModel;
					for (int k = 0; k < 9; k++)
					{
						this.PlayerCurrentMaterials[k].ResourceChangeAmount = 0;
						if (smithingModel.GetCraftingMaterialItem(refineFormula.Input1) == this.PlayerCurrentMaterials[k].ResourceItem)
						{
							this.PlayerCurrentMaterials[k].ResourceChangeAmount -= refineFormula.Input1Count;
						}
						else if (smithingModel.GetCraftingMaterialItem(refineFormula.Input2) == this.PlayerCurrentMaterials[k].ResourceItem)
						{
							this.PlayerCurrentMaterials[k].ResourceChangeAmount -= refineFormula.Input2Count;
						}
						else if (smithingModel.GetCraftingMaterialItem(refineFormula.Output) == this.PlayerCurrentMaterials[k].ResourceItem)
						{
							this.PlayerCurrentMaterials[k].ResourceChangeAmount += refineFormula.OutputCount;
						}
						else if (smithingModel.GetCraftingMaterialItem(refineFormula.Output2) == this.PlayerCurrentMaterials[k].ResourceItem)
						{
							this.PlayerCurrentMaterials[k].ResourceChangeAmount += refineFormula.Output2Count;
						}
					}
					int[] array = new int[9];
					foreach (CraftingResourceItemVM craftingResourceItemVM in currentSelectedAction.InputMaterials)
					{
						array[(int)craftingResourceItemVM.ResourceMaterial] -= craftingResourceItemVM.ResourceAmount;
					}
					using (IEnumerator<CraftingResourceItemVM> enumerator = currentSelectedAction.OutputMaterials.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CraftingResourceItemVM craftingResourceItemVM2 = enumerator.Current;
							array[(int)craftingResourceItemVM2.ResourceMaterial] += craftingResourceItemVM2.ResourceAmount;
						}
						return;
					}
				}
				int[] smithingCostsForWeaponDesign = Campaign.Current.Models.SmithingModel.GetSmithingCostsForWeaponDesign(this._crafting.CurrentWeaponDesign);
				for (int l = 0; l < 9; l++)
				{
					this.PlayerCurrentMaterials[l].ResourceChangeAmount = smithingCostsForWeaponDesign[l];
				}
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00059A70 File Offset: 0x00057C70
		private void UpdateCurrentMaterialsAvailable()
		{
			if (this.PlayerCurrentMaterials == null)
			{
				this.PlayerCurrentMaterials = new MBBindingList<CraftingResourceItemVM>();
				for (int i = 0; i < 9; i++)
				{
					this.PlayerCurrentMaterials.Add(new CraftingResourceItemVM((CraftingMaterials)i, 0, 0));
				}
			}
			for (int j = 0; j < 9; j++)
			{
				ItemObject craftingMaterialItem = Campaign.Current.Models.SmithingModel.GetCraftingMaterialItem((CraftingMaterials)j);
				this.PlayerCurrentMaterials[j].ResourceAmount = MobileParty.MainParty.ItemRoster.GetItemNumber(craftingMaterialItem);
			}
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00059AF4 File Offset: 0x00057CF4
		private void UpdateAll()
		{
			this.UpdateCurrentMaterialCosts();
			this.UpdateCurrentMaterialsAvailable();
			this.RefreshEnableMainAction();
			this.UpdateCraftingStamina();
			this.UpdateCraftingSkills();
			CraftingOrder craftingOrder;
			if (!this.IsInCraftingMode)
			{
				craftingOrder = null;
			}
			else
			{
				CraftingOrderItemVM activeCraftingOrder = this.WeaponDesign.ActiveCraftingOrder;
				craftingOrder = ((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null);
			}
			this.RefreshHeroAvailabilities(craftingOrder);
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00059B48 File Offset: 0x00057D48
		private void UpdateCraftingSkills()
		{
			foreach (CraftingAvailableHeroItemVM craftingAvailableHeroItemVM in this.AvailableCharactersForSmithing)
			{
				craftingAvailableHeroItemVM.RefreshSkills();
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00059B94 File Offset: 0x00057D94
		private void UpdateCraftingStamina()
		{
			foreach (CraftingAvailableHeroItemVM craftingAvailableHeroItemVM in this.AvailableCharactersForSmithing)
			{
				craftingAvailableHeroItemVM.RefreshStamina();
			}
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00059BE0 File Offset: 0x00057DE0
		private void UpdateCraftingPerks()
		{
			foreach (CraftingAvailableHeroItemVM craftingAvailableHeroItemVM in this.AvailableCharactersForSmithing)
			{
				craftingAvailableHeroItemVM.RefreshPerks();
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00059C2C File Offset: 0x00057E2C
		private void RefreshHeroAvailabilities(CraftingOrder order)
		{
			foreach (CraftingAvailableHeroItemVM craftingAvailableHeroItemVM in this.AvailableCharactersForSmithing)
			{
				craftingAvailableHeroItemVM.RefreshOrderAvailability(order);
			}
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00059C78 File Offset: 0x00057E78
		private void RefreshEnableMainAction()
		{
			if (Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				this.IsMainActionEnabled = true;
				return;
			}
			this.IsMainActionEnabled = true;
			if (!this.HaveEnergy())
			{
				this.IsMainActionEnabled = false;
				if (this.MainActionHint != null)
				{
					this.MainActionHint = new BasicTooltipViewModel(() => new TextObject("{=PRE5RKpp}You must rest and spend time before you can do this action.", null).ToString());
				}
			}
			else if (!this.HaveMaterialsNeeded())
			{
				this.IsMainActionEnabled = false;
				if (this.MainActionHint != null)
				{
					this.MainActionHint = new BasicTooltipViewModel(() => new TextObject("{=gduqxfck}You don't have all required materials!", null).ToString());
				}
			}
			if (this.IsInSmeltingMode)
			{
				this.IsMainActionEnabled = this.IsMainActionEnabled && this.Smelting.IsAnyItemSelected;
				this.IsSmeltingItemSelected = this.Smelting.IsAnyItemSelected;
				if (!this.IsSmeltingItemSelected && this.MainActionHint != null)
				{
					this.MainActionHint = new BasicTooltipViewModel(() => new TextObject("{=SzuCFlNq}No item selected.", null).ToString());
					return;
				}
			}
			else if (this.IsInRefinementMode)
			{
				this.IsMainActionEnabled = this.IsMainActionEnabled && this.Refinement.IsValidRefinementActionSelected;
				this.IsRefinementItemSelected = this.Refinement.IsValidRefinementActionSelected;
				if (!this.IsRefinementItemSelected && this.MainActionHint != null)
				{
					this.MainActionHint = new BasicTooltipViewModel(() => new TextObject("{=SzuCFlNq}No item selected.", null).ToString());
					return;
				}
			}
			else
			{
				if (this.WeaponDesign != null)
				{
					if (!this.WeaponDesign.HaveUnlockedAllSelectedPieces())
					{
						this.IsMainActionEnabled = false;
						if (this.MainActionHint != null)
						{
							this.MainActionHint = new BasicTooltipViewModel(() => new TextObject("{=Wir2xZIg}You haven't unlocked some of the selected pieces.", null).ToString());
						}
					}
					else if (!this.WeaponDesign.CanCompleteOrder())
					{
						this.IsMainActionEnabled = false;
						if (this.MainActionHint != null)
						{
							CraftingVM.<>c__DisplayClass21_0 CS$<>8__locals1 = new CraftingVM.<>c__DisplayClass21_0();
							CraftingVM.<>c__DisplayClass21_0 CS$<>8__locals2 = CS$<>8__locals1;
							CraftingOrderItemVM activeCraftingOrder = this.WeaponDesign.ActiveCraftingOrder;
							CS$<>8__locals2.order = ((activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null);
							CS$<>8__locals1.item = this._crafting.GetCurrentCraftedItemObject(false, null);
							this.MainActionHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetOrderCannotBeCompletedReasonTooltip(CS$<>8__locals1.order, CS$<>8__locals1.item));
						}
					}
				}
				if (this.IsMainActionEnabled && this.MainActionHint != null)
				{
					this.MainActionHint = new BasicTooltipViewModel();
				}
			}
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00059EEB File Offset: 0x000580EB
		private bool HaveEnergy()
		{
			CraftingAvailableHeroItemVM currentCraftingHero = this.CurrentCraftingHero;
			return ((currentCraftingHero != null) ? currentCraftingHero.Hero : null) == null || this._craftingBehavior.GetHeroCraftingStamina(this.CurrentCraftingHero.Hero) > 10;
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00059F1D File Offset: 0x0005811D
		private bool HaveMaterialsNeeded()
		{
			return !this.PlayerCurrentMaterials.Any<CraftingResourceItemVM>((CraftingResourceItemVM m) => m.ResourceChangeAmount + m.ResourceAmount < 0);
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00059F4C File Offset: 0x0005814C
		public void UpdateCraftingHero(CraftingAvailableHeroItemVM newHero)
		{
			this.CurrentCraftingHero = newHero;
			CraftingHeroPopupVM craftingHeroPopup = this.CraftingHeroPopup;
			if (craftingHeroPopup != null && craftingHeroPopup.IsVisible)
			{
				this.CraftingHeroPopup.ExecuteClosePopup();
			}
			this.WeaponDesign.OnCraftingHeroChanged(newHero);
			this.Refinement.OnCraftingHeroChanged(newHero);
			this.Smelting.OnCraftingHeroChanged(newHero);
			this.RefreshEnableMainAction();
			this.UpdateCraftingSkills();
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00059FB0 File Offset: 0x000581B0
		[return: TupleElementNames(new string[] { "isConfirmSuccessful", "isMainActionExecuted" })]
		public ValueTuple<bool, bool> ExecuteConfirm()
		{
			CraftingHistoryVM craftingHistory = this.WeaponDesign.CraftingHistory;
			if (craftingHistory != null && craftingHistory.IsVisible)
			{
				if (this.WeaponDesign.CraftingHistory.SelectedDesign != null)
				{
					this.WeaponDesign.CraftingHistory.ExecuteDone();
					return new ValueTuple<bool, bool>(true, false);
				}
			}
			else
			{
				CraftingOrderPopupVM craftingOrderPopup = this.WeaponDesign.CraftingOrderPopup;
				if (craftingOrderPopup != null && !craftingOrderPopup.IsVisible)
				{
					WeaponClassSelectionPopupVM weaponClassSelectionPopup = this.WeaponDesign.WeaponClassSelectionPopup;
					if (weaponClassSelectionPopup != null && !weaponClassSelectionPopup.IsVisible)
					{
						CraftingHeroPopupVM craftingHeroPopup = this.CraftingHeroPopup;
						if (craftingHeroPopup != null && !craftingHeroPopup.IsVisible)
						{
							if (this.WeaponDesign.IsInFinalCraftingStage)
							{
								if (this.WeaponDesign.CraftingResultPopup.CanConfirm)
								{
									this.WeaponDesign.CraftingResultPopup.ExecuteFinalizeCrafting();
									return new ValueTuple<bool, bool>(true, false);
								}
							}
							else if (this.IsMainActionEnabled)
							{
								this.ExecuteMainAction();
								return new ValueTuple<bool, bool>(true, true);
							}
						}
					}
				}
			}
			return new ValueTuple<bool, bool>(false, false);
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0005A0AC File Offset: 0x000582AC
		public void ExecuteCancel()
		{
			CraftingHistoryVM craftingHistory = this.WeaponDesign.CraftingHistory;
			if (craftingHistory != null && craftingHistory.IsVisible)
			{
				this.WeaponDesign.CraftingHistory.ExecuteCancel();
				return;
			}
			CraftingHeroPopupVM craftingHeroPopup = this.CraftingHeroPopup;
			if (craftingHeroPopup != null && craftingHeroPopup.IsVisible)
			{
				this.CraftingHeroPopup.ExecuteClosePopup();
				return;
			}
			CraftingOrderPopupVM craftingOrderPopup = this.WeaponDesign.CraftingOrderPopup;
			if (craftingOrderPopup != null && craftingOrderPopup.IsVisible)
			{
				this.WeaponDesign.CraftingOrderPopup.ExecuteCloseWithoutSelection();
				return;
			}
			WeaponClassSelectionPopupVM weaponClassSelectionPopup = this.WeaponDesign.WeaponClassSelectionPopup;
			if (weaponClassSelectionPopup != null && weaponClassSelectionPopup.IsVisible)
			{
				this.WeaponDesign.WeaponClassSelectionPopup.ExecuteClosePopup();
				return;
			}
			if (this.WeaponDesign.IsInFinalCraftingStage)
			{
				if (this.WeaponDesign.CraftingResultPopup.CanConfirm)
				{
					this.WeaponDesign.CraftingResultPopup.ExecuteFinalizeCrafting();
					return;
				}
			}
			else
			{
				this.Smelting.SaveItemLockStates();
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x0005A1A4 File Offset: 0x000583A4
		public void ExecuteMainAction()
		{
			if (this.IsInSmeltingMode)
			{
				this.Smelting.TrySmeltingSelectedItems(this.CurrentCraftingHero.Hero);
			}
			else if (this.IsInRefinementMode)
			{
				this.Refinement.ExecuteSelectedRefinement(this.CurrentCraftingHero.Hero);
			}
			else if (Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				CraftingState craftingState;
				if ((craftingState = GameStateManager.Current.ActiveState as CraftingState) != null)
				{
					ItemObject currentCraftedItemObject = craftingState.CraftingLogic.GetCurrentCraftedItemObject(true, null);
					ItemObject itemObject = MBObjectManager.Instance.GetObject<ItemObject>(currentCraftedItemObject.WeaponDesign.HashedCode) ?? MBObjectManager.Instance.RegisterObject<ItemObject>(currentCraftedItemObject);
					PartyBase.MainParty.ItemRoster.AddToCounts(itemObject, 1);
					this.WeaponDesign.IsInFinalCraftingStage = false;
				}
			}
			else
			{
				if (!this.HaveMaterialsNeeded() || !this.HaveEnergy())
				{
					return;
				}
				CraftingAvailableHeroItemVM currentCraftingHero = this.GetCurrentCraftingHero();
				Hero hero = ((currentCraftingHero != null) ? currentCraftingHero.Hero : null);
				ItemModifier craftedWeaponModifier = Campaign.Current.Models.SmithingModel.GetCraftedWeaponModifier(this._crafting.CurrentWeaponDesign, hero);
				this._craftingBehavior.SetCurrentItemModifier(craftedWeaponModifier);
				if (this.WeaponDesign.IsInOrderMode)
				{
					WeaponDesignVM weaponDesign = this.WeaponDesign;
					ICraftingCampaignBehavior craftingBehavior = this._craftingBehavior;
					Hero hero2 = hero;
					CraftingOrderItemVM activeCraftingOrder = this.WeaponDesign.ActiveCraftingOrder;
					weaponDesign.CraftedItemObject = craftingBehavior.CreateCraftedWeaponInCraftingOrderMode(hero2, (activeCraftingOrder != null) ? activeCraftingOrder.CraftingOrder : null, this._crafting.CurrentWeaponDesign);
				}
				else
				{
					this.WeaponDesign.CraftedItemObject = this._craftingBehavior.CreateCraftedWeaponInFreeBuildMode(hero, this._crafting.CurrentWeaponDesign, this._craftingBehavior.GetCurrentItemModifier());
				}
				this.WeaponDesign.IsInFinalCraftingStage = true;
				this.WeaponDesign.CreateCraftingResultPopup();
				Action onWeaponCrafted = this._onWeaponCrafted;
				if (onWeaponCrafted != null)
				{
					onWeaponCrafted();
				}
			}
			if (!this.IsInSmeltingMode)
			{
				this.UpdateAll();
			}
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x0005A376 File Offset: 0x00058576
		public void ExecuteResetCamera()
		{
			this._resetCamera();
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0005A383 File Offset: 0x00058583
		private CraftingAvailableHeroItemVM GetCurrentCraftingHero()
		{
			return this.CurrentCraftingHero;
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0005A38B File Offset: 0x0005858B
		private MBBindingList<CraftingAvailableHeroItemVM> GetCraftingHeroes()
		{
			return this.AvailableCharactersForSmithing;
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0005A393 File Offset: 0x00058593
		public void SetConfirmInputKey(HotKey hotKey)
		{
			this.ConfirmInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0005A3A2 File Offset: 0x000585A2
		public void SetExitInputKey(HotKey hotKey)
		{
			this.ExitInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0005A3B1 File Offset: 0x000585B1
		public void SetPreviousTabInputKey(HotKey hotKey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0005A3C0 File Offset: 0x000585C0
		public void SetNextTabInputKey(HotKey hotKey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0005A3D0 File Offset: 0x000585D0
		public void AddCameraControlInputKey(HotKey hotKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(hotKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x0005A3F4 File Offset: 0x000585F4
		public void AddCameraControlInputKey(GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromGameKey(gameKey, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x0005A418 File Offset: 0x00058618
		public void AddCameraControlInputKey(GameAxisKey gameAxisKey)
		{
			TextObject textObject = GameTexts.FindText("str_key_name", "CraftingHotkeyCategory_" + gameAxisKey.Id);
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromForcedID(gameAxisKey.AxisKey.ToString(), textObject, true);
			this.CameraControlKeys.Add(inputKeyItemVM);
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x0005A45F File Offset: 0x0005865F
		// (set) Token: 0x0600171B RID: 5915 RVA: 0x0005A467 File Offset: 0x00058667
		public InputKeyItemVM ConfirmInputKey
		{
			get
			{
				return this._confirmInputKey;
			}
			set
			{
				if (value != this._confirmInputKey)
				{
					this._confirmInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ConfirmInputKey");
				}
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x0005A485 File Offset: 0x00058685
		// (set) Token: 0x0600171D RID: 5917 RVA: 0x0005A48D File Offset: 0x0005868D
		public InputKeyItemVM ExitInputKey
		{
			get
			{
				return this._exitInputKey;
			}
			set
			{
				if (value != this._exitInputKey)
				{
					this._exitInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ExitInputKey");
				}
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x0005A4AB File Offset: 0x000586AB
		// (set) Token: 0x0600171F RID: 5919 RVA: 0x0005A4B3 File Offset: 0x000586B3
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousTabInputKey");
				}
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x0005A4D1 File Offset: 0x000586D1
		// (set) Token: 0x06001721 RID: 5921 RVA: 0x0005A4D9 File Offset: 0x000586D9
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextTabInputKey");
				}
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x0005A4F7 File Offset: 0x000586F7
		// (set) Token: 0x06001723 RID: 5923 RVA: 0x0005A4FF File Offset: 0x000586FF
		[DataSourceProperty]
		public MBBindingList<InputKeyItemVM> CameraControlKeys
		{
			get
			{
				return this._cameraControlKeys;
			}
			set
			{
				if (value != this._cameraControlKeys)
				{
					this._cameraControlKeys = value;
					base.OnPropertyChangedWithValue<MBBindingList<InputKeyItemVM>>(value, "CameraControlKeys");
				}
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x0005A51D File Offset: 0x0005871D
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x0005A525 File Offset: 0x00058725
		public bool CanSwitchTabs
		{
			get
			{
				return this._canSwitchTabs;
			}
			set
			{
				if (value != this._canSwitchTabs)
				{
					this._canSwitchTabs = value;
					base.OnPropertyChangedWithValue(value, "CanSwitchTabs");
				}
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x0005A543 File Offset: 0x00058743
		// (set) Token: 0x06001727 RID: 5927 RVA: 0x0005A54B File Offset: 0x0005874B
		public bool AreGamepadControlHintsEnabled
		{
			get
			{
				return this._areGamepadControlHintsEnabled;
			}
			set
			{
				if (value != this._areGamepadControlHintsEnabled)
				{
					this._areGamepadControlHintsEnabled = value;
					base.OnPropertyChangedWithValue(value, "AreGamepadControlHintsEnabled");
				}
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001728 RID: 5928 RVA: 0x0005A569 File Offset: 0x00058769
		// (set) Token: 0x06001729 RID: 5929 RVA: 0x0005A571 File Offset: 0x00058771
		[DataSourceProperty]
		public MBBindingList<CraftingResourceItemVM> PlayerCurrentMaterials
		{
			get
			{
				return this._playerCurrentMaterials;
			}
			set
			{
				if (value != this._playerCurrentMaterials)
				{
					this._playerCurrentMaterials = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingResourceItemVM>>(value, "PlayerCurrentMaterials");
				}
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x0600172A RID: 5930 RVA: 0x0005A58F File Offset: 0x0005878F
		// (set) Token: 0x0600172B RID: 5931 RVA: 0x0005A597 File Offset: 0x00058797
		[DataSourceProperty]
		public MBBindingList<CraftingAvailableHeroItemVM> AvailableCharactersForSmithing
		{
			get
			{
				return this._availableCharactersForSmithing;
			}
			set
			{
				if (value != this._availableCharactersForSmithing)
				{
					this._availableCharactersForSmithing = value;
					base.OnPropertyChangedWithValue<MBBindingList<CraftingAvailableHeroItemVM>>(value, "AvailableCharactersForSmithing");
				}
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x0005A5B5 File Offset: 0x000587B5
		// (set) Token: 0x0600172D RID: 5933 RVA: 0x0005A5C0 File Offset: 0x000587C0
		[DataSourceProperty]
		public CraftingAvailableHeroItemVM CurrentCraftingHero
		{
			get
			{
				return this._currentCraftingHero;
			}
			set
			{
				if (value != this._currentCraftingHero)
				{
					if (this._currentCraftingHero != null)
					{
						this._currentCraftingHero.IsSelected = false;
					}
					this._currentCraftingHero = value;
					if (this._currentCraftingHero != null)
					{
						this._currentCraftingHero.IsSelected = true;
					}
					ICraftingCampaignBehavior craftingBehavior = this._craftingBehavior;
					CraftingAvailableHeroItemVM currentCraftingHero = this._currentCraftingHero;
					craftingBehavior.SetActiveCraftingHero((currentCraftingHero != null) ? currentCraftingHero.Hero : null);
					base.OnPropertyChangedWithValue<CraftingAvailableHeroItemVM>(value, "CurrentCraftingHero");
				}
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x0600172E RID: 5934 RVA: 0x0005A62E File Offset: 0x0005882E
		// (set) Token: 0x0600172F RID: 5935 RVA: 0x0005A636 File Offset: 0x00058836
		[DataSourceProperty]
		public CraftingHeroPopupVM CraftingHeroPopup
		{
			get
			{
				return this._craftingHeroPopup;
			}
			set
			{
				if (value != this._craftingHeroPopup)
				{
					this._craftingHeroPopup = value;
					base.OnPropertyChangedWithValue<CraftingHeroPopupVM>(value, "CraftingHeroPopup");
				}
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06001730 RID: 5936 RVA: 0x0005A654 File Offset: 0x00058854
		// (set) Token: 0x06001731 RID: 5937 RVA: 0x0005A65C File Offset: 0x0005885C
		[DataSourceProperty]
		public string CurrentCategoryText
		{
			get
			{
				return this._currentCategoryText;
			}
			set
			{
				if (value != this._currentCategoryText)
				{
					this._currentCategoryText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCategoryText");
				}
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x0005A67F File Offset: 0x0005887F
		// (set) Token: 0x06001733 RID: 5939 RVA: 0x0005A687 File Offset: 0x00058887
		[DataSourceProperty]
		public string CraftingText
		{
			get
			{
				return this._craftingText;
			}
			set
			{
				if (value != this._craftingText)
				{
					this._craftingText = value;
					base.OnPropertyChangedWithValue<string>(value, "CraftingText");
				}
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x0005A6AA File Offset: 0x000588AA
		// (set) Token: 0x06001735 RID: 5941 RVA: 0x0005A6B2 File Offset: 0x000588B2
		[DataSourceProperty]
		public string SmeltingText
		{
			get
			{
				return this._smeltingText;
			}
			set
			{
				if (value != this._smeltingText)
				{
					this._smeltingText = value;
					base.OnPropertyChangedWithValue<string>(value, "SmeltingText");
				}
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x0005A6D5 File Offset: 0x000588D5
		// (set) Token: 0x06001737 RID: 5943 RVA: 0x0005A6DD File Offset: 0x000588DD
		[DataSourceProperty]
		public string RefinementText
		{
			get
			{
				return this._refinementText;
			}
			set
			{
				if (value != this._refinementText)
				{
					this._refinementText = value;
					base.OnPropertyChangedWithValue<string>(value, "RefinementText");
				}
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06001738 RID: 5944 RVA: 0x0005A700 File Offset: 0x00058900
		// (set) Token: 0x06001739 RID: 5945 RVA: 0x0005A708 File Offset: 0x00058908
		[DataSourceProperty]
		public string MainActionText
		{
			get
			{
				return this._mainActionText;
			}
			set
			{
				if (value != this._mainActionText)
				{
					this._mainActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "MainActionText");
				}
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600173A RID: 5946 RVA: 0x0005A72B File Offset: 0x0005892B
		// (set) Token: 0x0600173B RID: 5947 RVA: 0x0005A733 File Offset: 0x00058933
		[DataSourceProperty]
		public bool IsMainActionEnabled
		{
			get
			{
				return this._isMainActionEnabled;
			}
			set
			{
				if (value != this._isMainActionEnabled)
				{
					this._isMainActionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMainActionEnabled");
				}
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600173C RID: 5948 RVA: 0x0005A751 File Offset: 0x00058951
		// (set) Token: 0x0600173D RID: 5949 RVA: 0x0005A759 File Offset: 0x00058959
		[DataSourceProperty]
		public int ItemValue
		{
			get
			{
				return this._itemValue;
			}
			set
			{
				if (value != this._itemValue)
				{
					this._itemValue = value;
					base.OnPropertyChangedWithValue(value, "ItemValue");
				}
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x0005A777 File Offset: 0x00058977
		// (set) Token: 0x0600173F RID: 5951 RVA: 0x0005A77F File Offset: 0x0005897F
		[DataSourceProperty]
		public HintViewModel CraftingHint
		{
			get
			{
				return this._craftingHint;
			}
			set
			{
				if (value != this._craftingHint)
				{
					this._craftingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CraftingHint");
				}
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x0005A79D File Offset: 0x0005899D
		// (set) Token: 0x06001741 RID: 5953 RVA: 0x0005A7A5 File Offset: 0x000589A5
		[DataSourceProperty]
		public HintViewModel RefiningHint
		{
			get
			{
				return this._refiningHint;
			}
			set
			{
				if (value != this._refiningHint)
				{
					this._refiningHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RefiningHint");
				}
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x0005A7C3 File Offset: 0x000589C3
		// (set) Token: 0x06001743 RID: 5955 RVA: 0x0005A7CB File Offset: 0x000589CB
		[DataSourceProperty]
		public HintViewModel SmeltingHint
		{
			get
			{
				return this._smeltingHint;
			}
			set
			{
				if (value != this._smeltingHint)
				{
					this._smeltingHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SmeltingHint");
				}
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x0005A7E9 File Offset: 0x000589E9
		// (set) Token: 0x06001745 RID: 5957 RVA: 0x0005A7F1 File Offset: 0x000589F1
		[DataSourceProperty]
		public HintViewModel ResetCameraHint
		{
			get
			{
				return this._resetCameraHint;
			}
			set
			{
				if (value != this._resetCameraHint)
				{
					this._resetCameraHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ResetCameraHint");
				}
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x0005A80F File Offset: 0x00058A0F
		// (set) Token: 0x06001747 RID: 5959 RVA: 0x0005A817 File Offset: 0x00058A17
		[DataSourceProperty]
		public BasicTooltipViewModel MainActionHint
		{
			get
			{
				return this._mainActionHint;
			}
			set
			{
				if (value != this._mainActionHint)
				{
					this._mainActionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "MainActionHint");
				}
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x0005A835 File Offset: 0x00058A35
		// (set) Token: 0x06001749 RID: 5961 RVA: 0x0005A83D File Offset: 0x00058A3D
		[DataSourceProperty]
		public string DoneLbl
		{
			get
			{
				return this._doneLbl;
			}
			set
			{
				if (value != this._doneLbl)
				{
					this._doneLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneLbl");
				}
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x0005A860 File Offset: 0x00058A60
		// (set) Token: 0x0600174B RID: 5963 RVA: 0x0005A868 File Offset: 0x00058A68
		[DataSourceProperty]
		public string CancelLbl
		{
			get
			{
				return this._cancelLbl;
			}
			set
			{
				if (value != this._cancelLbl)
				{
					this._cancelLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelLbl");
				}
			}
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0005A88C File Offset: 0x00058A8C
		public void ExecuteSwitchToCrafting()
		{
			this.IsInSmeltingMode = false;
			this.IsInCraftingMode = true;
			this.IsInRefinementMode = false;
			this.CurrentCategoryText = new TextObject("{=POjDNVW3}Forging", null).ToString();
			this.MainActionText = GameTexts.FindText("str_crafting_category_crafting", null).ToString();
			CraftingVM.OnItemRefreshedDelegate onItemRefreshed = this.OnItemRefreshed;
			if (onItemRefreshed != null)
			{
				onItemRefreshed(true);
			}
			this.UpdateCurrentMaterialCosts();
			this.UpdateAll();
			WeaponDesignVM weaponDesign = this.WeaponDesign;
			if (weaponDesign == null)
			{
				return;
			}
			weaponDesign.ChangeModeIfHeroIsUnavailable();
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0005A908 File Offset: 0x00058B08
		public void ExecuteSwitchToSmelting()
		{
			this.IsInSmeltingMode = true;
			this.IsInCraftingMode = false;
			this.IsInRefinementMode = false;
			this.CurrentCategoryText = new TextObject("{=4cU98rkg}Smelting", null).ToString();
			this.MainActionText = GameTexts.FindText("str_crafting_category_smelting", null).ToString();
			CraftingVM.OnItemRefreshedDelegate onItemRefreshed = this.OnItemRefreshed;
			if (onItemRefreshed != null)
			{
				onItemRefreshed(false);
			}
			this.UpdateCurrentMaterialCosts();
			this.Smelting.RefreshList();
			this.UpdateAll();
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0005A980 File Offset: 0x00058B80
		public void ExecuteSwitchToRefinement()
		{
			this.IsInSmeltingMode = false;
			this.IsInCraftingMode = false;
			this.IsInRefinementMode = true;
			this.CurrentCategoryText = new TextObject("{=p7raHA9x}Refinement", null).ToString();
			this.MainActionText = GameTexts.FindText("str_crafting_category_refinement", null).ToString();
			CraftingVM.OnItemRefreshedDelegate onItemRefreshed = this.OnItemRefreshed;
			if (onItemRefreshed != null)
			{
				onItemRefreshed(false);
			}
			this.UpdateCurrentMaterialCosts();
			this.Refinement.RefreshRefinementActionsList(this.CurrentCraftingHero.Hero);
			this.UpdateAll();
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0005AA02 File Offset: 0x00058C02
		private void OnRefinementSelectionChange()
		{
			this.UpdateCurrentMaterialCosts();
			this.RefreshEnableMainAction();
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0005AA10 File Offset: 0x00058C10
		private void OnSmeltItemSelection()
		{
			this.UpdateCurrentMaterialCosts();
			this.RefreshEnableMainAction();
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0005AA1E File Offset: 0x00058C1E
		public void SetCurrentDesignManually(CraftingTemplate craftingTemplate, ValueTuple<CraftingPiece, int>[] pieces)
		{
			if (!this.IsInCraftingMode)
			{
				this.ExecuteSwitchToCrafting();
			}
			this.WeaponDesign.SetDesignManually(craftingTemplate, pieces, true);
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x0005AA3C File Offset: 0x00058C3C
		// (set) Token: 0x06001753 RID: 5971 RVA: 0x0005AA44 File Offset: 0x00058C44
		[DataSourceProperty]
		public SmeltingVM Smelting
		{
			get
			{
				return this._smelting;
			}
			set
			{
				if (value != this._smelting)
				{
					this._smelting = value;
					base.OnPropertyChangedWithValue<SmeltingVM>(value, "Smelting");
				}
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x0005AA62 File Offset: 0x00058C62
		// (set) Token: 0x06001755 RID: 5973 RVA: 0x0005AA6A File Offset: 0x00058C6A
		[DataSourceProperty]
		public WeaponDesignVM WeaponDesign
		{
			get
			{
				return this._weaponDesign;
			}
			set
			{
				if (value != this._weaponDesign)
				{
					this._weaponDesign = value;
					base.OnPropertyChangedWithValue<WeaponDesignVM>(value, "WeaponDesign");
				}
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x0005AA88 File Offset: 0x00058C88
		// (set) Token: 0x06001757 RID: 5975 RVA: 0x0005AA90 File Offset: 0x00058C90
		[DataSourceProperty]
		public RefinementVM Refinement
		{
			get
			{
				return this._refinement;
			}
			set
			{
				if (value != this._refinement)
				{
					this._refinement = value;
					base.OnPropertyChangedWithValue<RefinementVM>(value, "Refinement");
				}
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06001758 RID: 5976 RVA: 0x0005AAAE File Offset: 0x00058CAE
		// (set) Token: 0x06001759 RID: 5977 RVA: 0x0005AAB6 File Offset: 0x00058CB6
		[DataSourceProperty]
		public bool IsInCraftingMode
		{
			get
			{
				return this._isInCraftingMode;
			}
			set
			{
				if (value != this._isInCraftingMode)
				{
					this._isInCraftingMode = value;
					base.OnPropertyChangedWithValue(value, "IsInCraftingMode");
				}
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x0005AAD4 File Offset: 0x00058CD4
		// (set) Token: 0x0600175B RID: 5979 RVA: 0x0005AADC File Offset: 0x00058CDC
		[DataSourceProperty]
		public bool IsInSmeltingMode
		{
			get
			{
				return this._isInSmeltingMode;
			}
			set
			{
				if (value != this._isInSmeltingMode)
				{
					this._isInSmeltingMode = value;
					base.OnPropertyChangedWithValue(value, "IsInSmeltingMode");
				}
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600175C RID: 5980 RVA: 0x0005AAFA File Offset: 0x00058CFA
		// (set) Token: 0x0600175D RID: 5981 RVA: 0x0005AB02 File Offset: 0x00058D02
		[DataSourceProperty]
		public bool IsInRefinementMode
		{
			get
			{
				return this._isInRefinementMode;
			}
			set
			{
				if (value != this._isInRefinementMode)
				{
					this._isInRefinementMode = value;
					base.OnPropertyChangedWithValue(value, "IsInRefinementMode");
				}
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600175E RID: 5982 RVA: 0x0005AB20 File Offset: 0x00058D20
		// (set) Token: 0x0600175F RID: 5983 RVA: 0x0005AB28 File Offset: 0x00058D28
		[DataSourceProperty]
		public bool IsSmeltingItemSelected
		{
			get
			{
				return this._isSmeltingItemSelected;
			}
			set
			{
				if (value != this._isSmeltingItemSelected)
				{
					this._isSmeltingItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSmeltingItemSelected");
				}
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06001760 RID: 5984 RVA: 0x0005AB46 File Offset: 0x00058D46
		// (set) Token: 0x06001761 RID: 5985 RVA: 0x0005AB4E File Offset: 0x00058D4E
		[DataSourceProperty]
		public bool IsRefinementItemSelected
		{
			get
			{
				return this._isRefinementItemSelected;
			}
			set
			{
				if (value != this._isRefinementItemSelected)
				{
					this._isRefinementItemSelected = value;
					base.OnPropertyChangedWithValue(value, "IsRefinementItemSelected");
				}
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x0005AB6C File Offset: 0x00058D6C
		// (set) Token: 0x06001763 RID: 5987 RVA: 0x0005AB74 File Offset: 0x00058D74
		[DataSourceProperty]
		public string SelectItemToSmeltText
		{
			get
			{
				return this._selectItemToSmeltText;
			}
			set
			{
				if (value != this._selectItemToSmeltText)
				{
					this._selectItemToSmeltText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectItemToSmeltText");
				}
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x0005AB97 File Offset: 0x00058D97
		// (set) Token: 0x06001765 RID: 5989 RVA: 0x0005AB9F File Offset: 0x00058D9F
		[DataSourceProperty]
		public string SelectItemToRefineText
		{
			get
			{
				return this._selectItemToRefineText;
			}
			set
			{
				if (value != this._selectItemToRefineText)
				{
					this._selectItemToRefineText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectItemToRefineText");
				}
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x0005ABC2 File Offset: 0x00058DC2
		// (set) Token: 0x06001767 RID: 5991 RVA: 0x0005ABCA File Offset: 0x00058DCA
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

		// Token: 0x06001768 RID: 5992 RVA: 0x0005ABE8 File Offset: 0x00058DE8
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

		// Token: 0x04000A78 RID: 2680
		private const int _minimumRequiredStamina = 10;

		// Token: 0x04000A79 RID: 2681
		public CraftingVM.OnItemRefreshedDelegate OnItemRefreshed;

		// Token: 0x04000A7A RID: 2682
		private readonly Func<WeaponComponentData, ItemObject.ItemUsageSetFlags> _getItemUsageSetFlags;

		// Token: 0x04000A7B RID: 2683
		private readonly ICraftingCampaignBehavior _craftingBehavior;

		// Token: 0x04000A7C RID: 2684
		private readonly Action _onClose;

		// Token: 0x04000A7D RID: 2685
		private readonly Action _resetCamera;

		// Token: 0x04000A7E RID: 2686
		private readonly Action _onWeaponCrafted;

		// Token: 0x04000A7F RID: 2687
		private Crafting _crafting;

		// Token: 0x04000A80 RID: 2688
		private InputKeyItemVM _confirmInputKey;

		// Token: 0x04000A81 RID: 2689
		private InputKeyItemVM _exitInputKey;

		// Token: 0x04000A82 RID: 2690
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x04000A83 RID: 2691
		private InputKeyItemVM _nextTabInputKey;

		// Token: 0x04000A84 RID: 2692
		private MBBindingList<InputKeyItemVM> _cameraControlKeys;

		// Token: 0x04000A85 RID: 2693
		private bool _canSwitchTabs;

		// Token: 0x04000A86 RID: 2694
		private bool _areGamepadControlHintsEnabled;

		// Token: 0x04000A87 RID: 2695
		private string _doneLbl;

		// Token: 0x04000A88 RID: 2696
		private string _cancelLbl;

		// Token: 0x04000A89 RID: 2697
		private HintViewModel _resetCameraHint;

		// Token: 0x04000A8A RID: 2698
		private HintViewModel _smeltingHint;

		// Token: 0x04000A8B RID: 2699
		private HintViewModel _craftingHint;

		// Token: 0x04000A8C RID: 2700
		private HintViewModel _refiningHint;

		// Token: 0x04000A8D RID: 2701
		private BasicTooltipViewModel _mainActionHint;

		// Token: 0x04000A8E RID: 2702
		private int _itemValue = -1;

		// Token: 0x04000A8F RID: 2703
		private string _currentCategoryText;

		// Token: 0x04000A90 RID: 2704
		private string _mainActionText;

		// Token: 0x04000A91 RID: 2705
		private string _craftingText;

		// Token: 0x04000A92 RID: 2706
		private string _smeltingText;

		// Token: 0x04000A93 RID: 2707
		private string _refinementText;

		// Token: 0x04000A94 RID: 2708
		private bool _isMainActionEnabled;

		// Token: 0x04000A95 RID: 2709
		private MBBindingList<CraftingAvailableHeroItemVM> _availableCharactersForSmithing;

		// Token: 0x04000A96 RID: 2710
		private CraftingAvailableHeroItemVM _currentCraftingHero;

		// Token: 0x04000A97 RID: 2711
		private MBBindingList<CraftingResourceItemVM> _playerCurrentMaterials;

		// Token: 0x04000A98 RID: 2712
		private CraftingHeroPopupVM _craftingHeroPopup;

		// Token: 0x04000A99 RID: 2713
		private bool _isInSmeltingMode;

		// Token: 0x04000A9A RID: 2714
		private bool _isInCraftingMode;

		// Token: 0x04000A9B RID: 2715
		private bool _isInRefinementMode;

		// Token: 0x04000A9C RID: 2716
		private SmeltingVM _smelting;

		// Token: 0x04000A9D RID: 2717
		private RefinementVM _refinement;

		// Token: 0x04000A9E RID: 2718
		private WeaponDesignVM _weaponDesign;

		// Token: 0x04000A9F RID: 2719
		private bool _isSmeltingItemSelected;

		// Token: 0x04000AA0 RID: 2720
		private bool _isRefinementItemSelected;

		// Token: 0x04000AA1 RID: 2721
		private string _selectItemToSmeltText;

		// Token: 0x04000AA2 RID: 2722
		private string _selectItemToRefineText;

		// Token: 0x04000AA3 RID: 2723
		public ElementNotificationVM _tutorialNotification;

		// Token: 0x04000AA4 RID: 2724
		private string _latestTutorialElementID;

		// Token: 0x02000260 RID: 608
		// (Invoke) Token: 0x06002674 RID: 9844
		public delegate void OnItemRefreshedDelegate(bool isItemVisible);
	}
}
