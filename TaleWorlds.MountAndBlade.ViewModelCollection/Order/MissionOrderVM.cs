using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001C RID: 28
	public class MissionOrderVM : ViewModel
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009022 File Offset: 0x00007222
		public MissionOrderVM.CursorStates CursorState
		{
			get
			{
				OrderSetVM selectedOrderSet = this.SelectedOrderSet;
				if (((selectedOrderSet != null) ? selectedOrderSet.OrderIconId : null) == "order_type_facing")
				{
					return MissionOrderVM.CursorStates.Face;
				}
				return MissionOrderVM.CursorStates.Move;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00009045 File Offset: 0x00007245
		public Team Team
		{
			get
			{
				return Mission.Current.PlayerTeam;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00009051 File Offset: 0x00007251
		public OrderController OrderController
		{
			get
			{
				return this.Mission.PlayerTeam.PlayerOrderController;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00009063 File Offset: 0x00007263
		// (set) Token: 0x0600024E RID: 590 RVA: 0x0000906B File Offset: 0x0000726B
		public bool IsTroopPlacingActive
		{
			get
			{
				return this._isTroopPlacingActive;
			}
			set
			{
				this._isTroopPlacingActive = value;
				this._callbacks.SetSuspendTroopPlacer(!value);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00009088 File Offset: 0x00007288
		public bool PlayerHasAnyTroopUnderThem
		{
			get
			{
				return this.Team.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.PlayerOwner == Agent.Main && f.CountOfUnits > 0);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000090B9 File Offset: 0x000072B9
		private Mission Mission
		{
			get
			{
				return Mission.Current;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000251 RID: 593 RVA: 0x000090C0 File Offset: 0x000072C0
		// (set) Token: 0x06000252 RID: 594 RVA: 0x000090C8 File Offset: 0x000072C8
		public OrderSetVM SelectedOrderSet { get; private set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000253 RID: 595 RVA: 0x000090D1 File Offset: 0x000072D1
		// (set) Token: 0x06000254 RID: 596 RVA: 0x000090D9 File Offset: 0x000072D9
		public bool DisplayedOrderMessageForLastOrder { get; private set; }

		// Token: 0x06000255 RID: 597 RVA: 0x000090E4 File Offset: 0x000072E4
		public MissionOrderVM(OrderController orderController, bool isDeployment, bool isMultiplayer)
		{
			this._isMultiplayer = isMultiplayer;
			this.IsDeployment = isDeployment;
			this._orderKeys = new Dictionary<int, InputKeyItemVM>();
			this.OrderSets = new MBBindingList<OrderSetVM>();
			this.DeploymentController = new MissionOrderDeploymentControllerVM(this);
			this.TroopController = this.CreateTroopController(this.OrderController);
			this.Team.OnFormationAIActiveBehaviorChanged += this.TeamOnFormationAIActiveBehaviorChanged;
			this.RefreshValues();
			this.Mission.OnMainAgentChanged += this.MissionOnMainAgentChanged;
			this.UpdateCanUseShortcuts(this._isMultiplayer);
			this._slowMotionSoundEventGlobalIndex = SoundManager.GetEventGlobalIndex("event:/ui/mission/slow_motion");
			this.RegisterEvents();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000919A File Offset: 0x0000739A
		protected virtual MissionOrderTroopControllerVM CreateTroopController(OrderController orderController)
		{
			return new MissionOrderTroopControllerVM(this, this.IsDeployment, new Action(this.OnTransferFinished));
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000091B4 File Offset: 0x000073B4
		public void SetCallbacks(MissionOrderCallbacks callbacks)
		{
			this._callbacks = callbacks;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x000091C0 File Offset: 0x000073C0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ReturnText = new TextObject("{=EmVbbIUc}Return", null).ToString();
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
			{
				o.RefreshValues();
			});
			this.TroopController.RefreshValues();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00009220 File Offset: 0x00007420
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Mission.OnMainAgentChanged -= this.MissionOnMainAgentChanged;
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
			{
				o.OnFinalize();
			});
			this.DeploymentController.OnFinalize();
			this.TroopController.OnFinalize();
			for (int i = 0; i < this._orderKeys.Count; i++)
			{
				this._orderKeys[i].OnFinalize();
			}
			foreach (OrderSetVM orderSetVM in this._orderSets)
			{
				orderSetVM.OnFinalize();
			}
			if (this._slowMotionSoundEvent != null)
			{
				this._slowMotionSoundEvent.Release();
				this._slowMotionSoundEvent = null;
			}
			this.InputRestrictions = null;
			this.UnregisterEvents();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009318 File Offset: 0x00007518
		private void RegisterEvents()
		{
			OrderTroopItemVM.OnSelectionChange += this.OnTroopItemSelectionStateChanged;
			OrderSetVM.OnSelectionStateChanged += this.OnOrderSetSelectionStateChanged;
			OrderItemVM.OnExecuteOrder += this.OnOrderExecuted;
			TransferTroopsVisualOrder.OnTransferStarted += this.OnTransferStarted;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000938C File Offset: 0x0000758C
		private void UnregisterEvents()
		{
			OrderTroopItemVM.OnSelectionChange -= this.OnTroopItemSelectionStateChanged;
			OrderSetVM.OnSelectionStateChanged -= this.OnOrderSetSelectionStateChanged;
			OrderItemVM.OnExecuteOrder -= this.OnOrderExecuted;
			TransferTroopsVisualOrder.OnTransferStarted -= this.OnTransferStarted;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000093FD File Offset: 0x000075FD
		private void OnGamepadActiveChanged()
		{
			if (this.IsToggleOrderShown)
			{
				this.TryCloseToggleOrder(false);
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009410 File Offset: 0x00007610
		private void OnOrderSetSelectionStateChanged(OrderSetVM orderSet, bool isSelected)
		{
			if (this.SelectedOrderSet == orderSet)
			{
				OrderSetVM selectedOrderSet = this.SelectedOrderSet;
				if (selectedOrderSet != null && selectedOrderSet.IsSelected == isSelected)
				{
					return;
				}
			}
			if (this.SelectedOrderSet != null)
			{
				this.SelectedOrderSet.IsSelected = false;
				this.SelectedOrderSet = null;
			}
			if (orderSet != null && isSelected)
			{
				this.SelectedOrderSet = orderSet;
				this.SelectedOrderSet.IsSelected = true;
			}
			this.IsAnyOrderSetActive = this.SelectedOrderSet != null;
			this.UpdateOrderShortcuts();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000948C File Offset: 0x0000768C
		public void OnOrderExecuted(OrderItemVM orderItem)
		{
			if (this.IsToggleOrderShown)
			{
				this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
				{
					o.OnOrderExecuted(orderItem);
				});
			}
			List<TextObject> list = new List<TextObject>();
			if (!(orderItem.Order is ReturnVisualOrder))
			{
				foreach (OrderTroopItemVM orderTroopItemVM in this.TroopController.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM item) => item.IsSelected))
				{
					list.Add(orderTroopItemVM.GetVisibleNameOfFormationForMessage());
				}
			}
			if (!list.IsEmpty<TextObject>() && !this.DisplayedOrderMessageForLastOrder)
			{
				orderItem.RefreshState();
				TextObject textObject = new TextObject("{=ApD0xQXT}{STR1}: {STR2}", null);
				textObject.SetTextVariable("STR1", GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, false));
				textObject.SetTextVariable("STR2", orderItem.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				this.DisplayedOrderMessageForLastOrder = true;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000095B4 File Offset: 0x000077B4
		private void PopulateOrderSets()
		{
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
			{
				o.OnFinalize();
			});
			this.OrderSets.Clear();
			MBReadOnlyList<VisualOrderSet> orders = VisualOrderFactory.GetOrders();
			this.HasAnyCascadingOrders = false;
			for (int i = 0; i < orders.Count; i++)
			{
				OrderSetVM orderSetVM = new OrderSetVM(this.OrderController, orders[i]);
				this.OrderSets.Add(orderSetVM);
				if (!orderSetVM.HasSingleOrder)
				{
					this.HasAnyCascadingOrders = true;
				}
			}
			this.UpdateOrderShortcuts();
			if (this._isMultiplayer)
			{
				this.UpdateCanUseShortcuts(true);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009658 File Offset: 0x00007858
		private void UpdateOrderShortcuts()
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"), false);
			inputKeyItemVM.SetForcedVisibility(new bool?(false));
			if (this.SelectedOrderSet != null)
			{
				for (int i = 0; i < this.OrderSets.Count; i++)
				{
					OrderSetVM orderSetVM = this.OrderSets[i];
					if (orderSetVM == this.SelectedOrderSet)
					{
						for (int j = 0; j < orderSetVM.Orders.Count; j++)
						{
							OrderItemVM orderItemVM = orderSetVM.Orders[j];
							InputKeyItemVM inputKeyItemVM2;
							if (orderItemVM.Order is ReturnVisualOrder)
							{
								orderItemVM.SetShortcutKey(this._returnKey);
							}
							else if (this._orderKeys.TryGetValue(j, out inputKeyItemVM2))
							{
								orderItemVM.SetShortcutKey(inputKeyItemVM2);
							}
						}
					}
					else
					{
						for (int k = 0; k < orderSetVM.Orders.Count; k++)
						{
							orderSetVM.Orders[k].SetShortcutKey(inputKeyItemVM);
						}
					}
					orderSetVM.SetShortcutKey(inputKeyItemVM);
				}
			}
			else
			{
				for (int l = 0; l < this.OrderSets.Count; l++)
				{
					OrderSetVM orderSetVM2 = this.OrderSets[l];
					InputKeyItemVM inputKeyItemVM3;
					if (orderSetVM2.HasSingleOrder && orderSetVM2.Orders[0].Order is ReturnVisualOrder)
					{
						orderSetVM2.SetShortcutKey(this._returnKey);
					}
					else if (this._orderKeys.TryGetValue(l, out inputKeyItemVM3))
					{
						orderSetVM2.SetShortcutKey(inputKeyItemVM3);
					}
				}
			}
			inputKeyItemVM.OnFinalize();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000097D5 File Offset: 0x000079D5
		private void TeamOnFormationAIActiveBehaviorChanged(Formation formation)
		{
			if (formation.IsAIControlled)
			{
				if (this._modifiedAIFormations.IndexOf(formation) < 0)
				{
					this._modifiedAIFormations.Add(formation);
				}
				this._delayValueForAIFormationModifications = 3;
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00009804 File Offset: 0x00007A04
		private void DisplayFormationAIFeedback()
		{
			this._delayValueForAIFormationModifications = Math.Max(0, this._delayValueForAIFormationModifications - 1);
			if (this._delayValueForAIFormationModifications == 0 && this._modifiedAIFormations.Count > 0)
			{
				for (int i = 0; i < this._modifiedAIFormations.Count; i++)
				{
					Formation formation = this._modifiedAIFormations[i];
					if (((formation != null) ? formation.AI.ActiveBehavior : null) != null && formation.FormationIndex < FormationClass.NumberOfRegularFormations)
					{
						MissionOrderVM.DisplayFormationAIFeedbackAux(this._modifiedAIFormations);
					}
					else
					{
						this._modifiedAIFormations[i] = null;
					}
				}
				this._modifiedAIFormations.Clear();
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000098A0 File Offset: 0x00007AA0
		private static void DisplayFormationAIFeedbackAux(List<Formation> formations)
		{
			Dictionary<FormationClass, TextObject> dictionary = new Dictionary<FormationClass, TextObject>();
			Type type = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < formations.Count; i++)
			{
				Formation formation = formations[i];
				if (((formation != null) ? formation.AI.ActiveBehavior : null) != null && (type == null || type == formation.AI.ActiveBehavior.GetType()))
				{
					type = formation.AI.ActiveBehavior.GetType();
					switch (formation.AI.Side)
					{
					case FormationAI.BehaviorSide.Left:
						flag = true;
						break;
					case FormationAI.BehaviorSide.Middle:
						flag3 = true;
						break;
					case FormationAI.BehaviorSide.Right:
						flag2 = true;
						break;
					}
					if (!dictionary.ContainsKey(formation.PhysicalClass))
					{
						TextObject localizedName = formation.PhysicalClass.GetLocalizedName();
						TextObject textObject = GameTexts.FindText("str_troop_group_name_definite", null);
						textObject.SetTextVariable("FORMATION_CLASS", localizedName);
						dictionary.Add(formation.PhysicalClass, textObject);
					}
					formations[i] = null;
				}
			}
			if (dictionary.Count == 1)
			{
				MBTextManager.SetTextVariable("IS_PLURAL", 0);
				MBTextManager.SetTextVariable("TROOP_NAMES_BEGIN", TextObject.GetEmpty(), false);
				MBTextManager.SetTextVariable("TROOP_NAMES_END", dictionary.First<KeyValuePair<FormationClass, TextObject>>().Value, false);
			}
			else
			{
				MBTextManager.SetTextVariable("IS_PLURAL", 1);
				TextObject value = dictionary.Last<KeyValuePair<FormationClass, TextObject>>().Value;
				TextObject textObject2;
				if (dictionary.Count == 2)
				{
					textObject2 = dictionary.First<KeyValuePair<FormationClass, TextObject>>().Value;
				}
				else
				{
					textObject2 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
					textObject2.SetTextVariable("LEFT", dictionary.First<KeyValuePair<FormationClass, TextObject>>().Value);
					textObject2.SetTextVariable("RIGHT", dictionary.Last<KeyValuePair<FormationClass, TextObject>>().Value);
					for (int j = 2; j < dictionary.Count - 1; j++)
					{
						TextObject textObject3 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
						textObject3.SetTextVariable("LEFT", textObject2);
						textObject3.SetTextVariable("RIGHT", dictionary.Values.ElementAt<TextObject>(j));
						textObject2 = textObject3;
					}
				}
				MBTextManager.SetTextVariable("TROOP_NAMES_BEGIN", textObject2, false);
				MBTextManager.SetTextVariable("TROOP_NAMES_END", value, false);
			}
			bool flag4 = (flag ? 1 : 0) + (flag3 ? 1 : 0) + (flag2 ? 1 : 0) > 1;
			MBTextManager.SetTextVariable("IS_LEFT", flag4 ? 2 : (flag ? 1 : 0));
			MBTextManager.SetTextVariable("IS_MIDDLE", (!flag4 && flag3) ? 1 : 0);
			MBTextManager.SetTextVariable("IS_RIGHT", (!flag4 && flag2) ? 1 : 0);
			string name = type.Name;
			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_formation_ai_behavior_text", name).ToString()));
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00009B60 File Offset: 0x00007D60
		private void OnTroopItemSelectionStateChanged(OrderTroopItemVM troopItem, bool isSelected)
		{
			for (int i = 0; i < this.TroopController.TroopList.Count; i++)
			{
				this.TroopController.TroopList[i].IsTargetRelevant = this.TroopController.TroopList[i].IsSelected;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00009BB4 File Offset: 0x00007DB4
		public virtual void OnOrderLayoutTypeChanged()
		{
			this.TroopController = this.CreateTroopController(this.OrderController);
			this.OrderSets.Clear();
			this.TroopController.UpdateTroops();
			this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM x)
			{
				this.TroopController.SetTroopActiveOrders(x);
			});
			this.TroopController.OnFiltersSet(this._filterData);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009C18 File Offset: 0x00007E18
		public void OpenToggleOrder(bool fromHold, bool displayMessage = true)
		{
			if (this.IsToggleOrderShown)
			{
				return;
			}
			if (this.OrderController.SelectedFormations.Count == 0)
			{
				this.OrderController.SelectAllFormations(false);
			}
			this.PopulateOrderSets();
			if (this.CheckCanBeOpened(displayMessage))
			{
				Mission.Current.IsOrderMenuOpen = true;
				this.IsToggleOrderShown = true;
				this.TroopController.UpdateTroops();
				this.TroopController.IsTransferActive = false;
				if (this.OrderController.SelectedFormations.IsEmpty<Formation>())
				{
					this.TroopController.SelectAllFormations(true);
				}
				if (Input.IsGamepadActive)
				{
					if (this.TroopController.TroopList.All<OrderTroopItemVM>((OrderTroopItemVM t) => !t.IsSelectionHighlightActive))
					{
						OrderTroopItemVM orderTroopItemVM = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>();
						if (orderTroopItemVM != null)
						{
							orderTroopItemVM.IsSelectionHighlightActive = true;
						}
					}
				}
				this.SetActiveOrders();
				this.OnOrderShownToggle();
				this.DisplayedOrderMessageForLastOrder = false;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009D0C File Offset: 0x00007F0C
		private bool CheckCanBeOpened(bool displayMessage = false)
		{
			if (Agent.Main == null)
			{
				if (displayMessage)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=GMhOZGnb}Cannot issue order while dead.", null).ToString()));
				}
				return false;
			}
			if (Mission.Current.Mode != MissionMode.Deployment && !Agent.Main.IsPlayerControlled)
			{
				if (displayMessage)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=b1DHZsaH}Cannot issue order right now.", null).ToString()));
				}
				return false;
			}
			if (!this.Team.HasBots || !this.PlayerHasAnyTroopUnderThem || (!this.Team.IsPlayerGeneral && !this.Team.IsPlayerSergeant))
			{
				if (displayMessage)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=DQvGNQ0g}There isn't any unit under command.", null).ToString()));
				}
				return false;
			}
			return !Mission.Current.IsMissionEnding || Mission.Current.CheckIfBattleInRetreat();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009DE0 File Offset: 0x00007FE0
		public bool TryCloseToggleOrder(bool applySelectedOrders = false)
		{
			if (this.IsToggleOrderShown)
			{
				Mission.Current.IsOrderMenuOpen = false;
				if (applySelectedOrders && this.SelectedOrderSet != null)
				{
					OrderItemVM orderItemVM = this.SelectedOrderSet.Orders.FirstOrDefault<OrderItemVM>((OrderItemVM o) => o.IsSelected);
					if (orderItemVM != null && this._callbacks.GetVisualOrderExecutionParameters != null)
					{
						VisualOrderExecutionParameters visualOrderExecutionParameters = this._callbacks.GetVisualOrderExecutionParameters();
						orderItemVM.ExecuteAction(visualOrderExecutionParameters);
					}
				}
				OrderSetVM selectedOrderSet = this.SelectedOrderSet;
				if (selectedOrderSet != null)
				{
					selectedOrderSet.ExecuteDeSelect();
				}
				this.IsToggleOrderShown = false;
				this.OnOrderShownToggle();
				if (!this.IsDeployment)
				{
					this.InputRestrictions.ResetInputRestrictions();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009E9A File Offset: 0x0000809A
		public void SetActiveOrders()
		{
			this.TroopController.SetCurrentActiveOrders();
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM os)
			{
				os.RefreshOrderStates();
			});
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009ED1 File Offset: 0x000080D1
		public void AfterInitialize()
		{
			this.TroopController.UpdateTroops();
			if (!this.IsDeployment)
			{
				this.TroopController.SelectAllFormations(false);
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009EF4 File Offset: 0x000080F4
		public void Update()
		{
			if (this.IsToggleOrderShown)
			{
				if (!this.CheckCanBeOpened(false))
				{
					this.TryCloseToggleOrder(false);
				}
				else if (this._updateTroopsTimer.Check(MBCommon.GetApplicationTime()))
				{
					this.TroopController.IntervalUpdate();
				}
				this.TroopController.Update();
				this.TroopController.RefreshTroopFormationTargetVisuals();
				this.UseAlternativeFormationLayout = Input.IsGamepadActive;
			}
			if (this.IsToggleOrderShown)
			{
				if (BannerlordConfig.SlowDownOnOrder && !this._isDeployment && !this._isMultiplayer && this._slowMotionSoundEvent == null)
				{
					this._slowMotionSoundEvent = SoundEvent.CreateEvent(this._slowMotionSoundEventGlobalIndex, Mission.Current.Scene);
					this._slowMotionSoundEvent.Play();
				}
			}
			else if (this._slowMotionSoundEvent != null)
			{
				this._slowMotionSoundEvent.Release();
				this._slowMotionSoundEvent = null;
			}
			this.DisplayFormationAIFeedback();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009FCA File Offset: 0x000081CA
		public void OnEscape()
		{
			if (this.IsToggleOrderShown)
			{
				if (this.SelectedOrderSet != null)
				{
					this.SelectedOrderSet.ExecuteDeSelect();
					return;
				}
				this.TryCloseToggleOrder(false);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009FF0 File Offset: 0x000081F0
		public void ViewOrders()
		{
			if (!this.IsToggleOrderShown)
			{
				this.TroopController.UpdateTroops();
				this.OpenToggleOrder(false, true);
				return;
			}
			this.TryCloseToggleOrder(false);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000A016 File Offset: 0x00008216
		public OrderSetVM GetOrderSetAtIndex(int orderSetIndex)
		{
			if (orderSetIndex < 0 || orderSetIndex >= this.OrderSets.Count)
			{
				return null;
			}
			return this.OrderSets[orderSetIndex];
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000A038 File Offset: 0x00008238
		public bool TrySelectOrderSet(OrderSetVM orderSet)
		{
			if (!this.CheckCanBeOpened(true))
			{
				return false;
			}
			VisualOrderExecutionParameters visualOrderExecutionParameters = this._callbacks.GetVisualOrderExecutionParameters();
			orderSet.ExecuteAction(visualOrderExecutionParameters);
			if (!this.IsToggleOrderShown && !orderSet.OrderSet.IsSoloOrder)
			{
				this.OpenToggleOrder(false, true);
			}
			else if (this.IsToggleOrderShown && orderSet.OrderSet.IsSoloOrder && !this.IsDeployment)
			{
				this.TryCloseToggleOrder(false);
			}
			return true;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000A0AD File Offset: 0x000082AD
		public void OnTroopFormationSelected(int formationTroopIndex)
		{
			if (!this.CheckCanBeOpened(true))
			{
				return;
			}
			this.TroopController.OnSelectFormationWithIndex(formationTroopIndex);
			this.TryCloseToggleOrder(false);
			this.OpenToggleOrder(false, true);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000A0D5 File Offset: 0x000082D5
		private void MissionOnMainAgentChanged(Agent oldAgent)
		{
			if (this.Mission.MainAgent == null)
			{
				this.TryCloseToggleOrder(false);
				this.Mission.IsOrderMenuOpen = false;
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000A0F8 File Offset: 0x000082F8
		internal void OnDeployAll()
		{
			this.TroopController.UpdateTroops();
			foreach (OrderTroopItemVM orderTroopItemVM in this.TroopController.TroopList)
			{
				this.TroopController.SetTroopActiveOrders(orderTroopItemVM);
			}
			if (!this.IsDeployment)
			{
				this.TroopController.SelectAllFormations(false);
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000A174 File Offset: 0x00008374
		private void OnOrderShownToggle()
		{
			this.IsTroopListShown = this.IsToggleOrderShown && !this.IsDeployment;
			if (!this._isDeployment)
			{
				if (this.IsToggleOrderShown)
				{
					this._callbacks.OnActivateToggleOrder();
				}
				else
				{
					this._callbacks.OnDeactivateToggleOrder();
				}
			}
			this._updateTroopsTimer = (this.IsToggleOrderShown ? new Timer(MBCommon.GetApplicationTime() - 2f, 2f, true) : null);
			this.IsTroopPlacingActive = this.IsToggleOrderShown;
			if (!this.IsDeployment && this.TroopController.TroopList.Count > 0 && Input.IsGamepadActive && this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.FormationIndex == this._lastHighlightedFormationIndex) == null)
			{
				this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
				{
					t.IsSelectionHighlightActive = false;
				});
				this.TroopController.TroopList[0].IsSelectionHighlightActive = true;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000A288 File Offset: 0x00008488
		public void OnTroopHighlightSelection(bool isDirectionLeft)
		{
			if (!this.CheckCanBeOpened(true))
			{
				return;
			}
			if (this.TroopController.TroopList.Count > 0)
			{
				OrderTroopItemVM highlightedFormation = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectionHighlightActive);
				if (highlightedFormation != null)
				{
					OrderTroopItemVM targetFormation = (isDirectionLeft ? this.TroopController.TroopList.LastOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.FormationIndex < highlightedFormation.FormationIndex) : this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.FormationIndex > highlightedFormation.FormationIndex));
					if (targetFormation == null)
					{
						targetFormation = (isDirectionLeft ? this.TroopController.TroopList.LastOrDefault<OrderTroopItemVM>() : this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>());
					}
					if (targetFormation != null)
					{
						this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
						{
							t.IsSelectionHighlightActive = t == targetFormation;
						});
						this._lastHighlightedFormationIndex = targetFormation.FormationIndex;
						return;
					}
				}
				else
				{
					this.TroopController.TroopList[0].IsSelectionHighlightActive = true;
				}
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000A3C4 File Offset: 0x000085C4
		public void ExecuteSelectHighlightedFormation()
		{
			OrderTroopItemVM orderTroopItemVM = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable && t.IsSelectionHighlightActive);
			if (orderTroopItemVM == null)
			{
				return;
			}
			if (orderTroopItemVM.IsSelected)
			{
				if (this.TroopController.TroopList.Count<OrderTroopItemVM>((OrderTroopItemVM x) => x.IsSelected) == 1)
				{
					this.TroopController.SelectAllFormations(true);
					return;
				}
			}
			this.OnTroopFormationSelected(orderTroopItemVM.FormationIndex);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A458 File Offset: 0x00008658
		public void ExecuteToggleHighlightedFormation()
		{
			OrderTroopItemVM orderTroopItemVM = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable && t.IsSelectionHighlightActive);
			if (orderTroopItemVM == null)
			{
				return;
			}
			if (orderTroopItemVM.IsSelected)
			{
				if (this.TroopController.TroopList.Count<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected) == 1)
				{
					this.TroopController.SelectAllFormations(true);
				}
				this.TroopController.OnDeselectFormation(orderTroopItemVM.FormationIndex);
			}
			else
			{
				this.TroopController.AddSelectedFormation(orderTroopItemVM);
			}
			this.TryCloseToggleOrder(false);
			this.OpenToggleOrder(false, true);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A510 File Offset: 0x00008710
		private void OnTransferStarted()
		{
			if (this.IsDeployment)
			{
				return;
			}
			foreach (OrderTroopItemVM orderTroopItemVM in this.TroopController.TransferTargetList)
			{
				orderTroopItemVM.IsSelected = false;
				orderTroopItemVM.IsSelectable = !this.OrderController.IsFormationListening(orderTroopItemVM.Formation);
			}
			OrderTroopItemVM orderTroopItemVM2 = this.TroopController.TransferTargetList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable);
			if (orderTroopItemVM2 != null)
			{
				this.TroopController.IsTransferActive = true;
				this.TroopController.ExecuteSelectTransferTroop(orderTroopItemVM2);
				this.TroopController.TransferMaxValue = this.TroopController.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected).Sum<OrderTroopItemVM>((OrderTroopItemVM t) => t.CurrentMemberCount);
				this.TroopController.TransferValue = this.TroopController.TransferMaxValue;
				this.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				return;
			}
			MBInformationManager.AddQuickInformation(new TextObject("{=SLY8z9fP}All formations are selected!", null), 0, null, null, "");
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A670 File Offset: 0x00008870
		protected void OnTransferFinished()
		{
			this._callbacks.OnTransferTroopsFinished();
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A684 File Offset: 0x00008884
		[Conditional("DEBUG")]
		private void DebugTick()
		{
			if (this.IsToggleOrderShown)
			{
				string text = "SelectedFormations (" + this.OrderController.SelectedFormations.Count + ") :";
				foreach (Formation formation in this.OrderController.SelectedFormations)
				{
					text = text + " " + formation.FormationIndex.GetName();
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A71C File Offset: 0x0000891C
		public void OnDeploymentFinished()
		{
			this.TroopController.OnDeploymentFinished();
			this.IsDeployment = false;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A730 File Offset: 0x00008930
		public void OnAfterDeploymentFinished()
		{
			this.TroopController.OnAfterDeploymentFinished();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A73D File Offset: 0x0000893D
		public void OnFiltersSet(List<MissionOrderVM.FormationConfiguration> filterData)
		{
			this._filterData = filterData;
			this.TroopController.OnFiltersSet(filterData);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A754 File Offset: 0x00008954
		public void UpdateCanUseShortcuts(bool value)
		{
			this.CanUseShortcuts = value;
			for (int i = 0; i < this.OrderSets.Count; i++)
			{
				this.OrderSets[i].UpdateCanUseShortcuts(value);
			}
			if (!value)
			{
				this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
				{
					t.ShowSelectionInputs = false;
				});
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A7C4 File Offset: 0x000089C4
		public void SetOrderIndexKey(int orderIndex, GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM;
			if (this._orderKeys.TryGetValue(orderIndex, out inputKeyItemVM) && inputKeyItemVM != null)
			{
				inputKeyItemVM.OnFinalize();
			}
			InputKeyItemVM inputKeyItemVM2 = InputKeyItemVM.CreateFromGameKey(gameKey, false);
			this._orderKeys[orderIndex] = inputKeyItemVM2;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A7FF File Offset: 0x000089FF
		public void SetReturnKey(GameKey gameKey)
		{
			InputKeyItemVM returnKey = this._returnKey;
			if (returnKey != null)
			{
				returnKey.OnFinalize();
			}
			this._returnKey = InputKeyItemVM.CreateFromGameKey(gameKey, false);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A81F File Offset: 0x00008A1F
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000A82E File Offset: 0x00008A2E
		// (set) Token: 0x06000282 RID: 642 RVA: 0x0000A836 File Offset: 0x00008A36
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000A854 File Offset: 0x00008A54
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000A85C File Offset: 0x00008A5C
		[DataSourceProperty]
		public MBBindingList<OrderSetVM> OrderSets
		{
			get
			{
				return this._orderSets;
			}
			set
			{
				if (value == this._orderSets)
				{
					return;
				}
				this._orderSets = value;
				base.OnPropertyChangedWithValue<MBBindingList<OrderSetVM>>(value, "OrderSets");
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000A87B File Offset: 0x00008A7B
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000A883 File Offset: 0x00008A83
		[DataSourceProperty]
		public MissionOrderTroopControllerVM TroopController
		{
			get
			{
				return this._troopController;
			}
			set
			{
				if (value == this._troopController)
				{
					return;
				}
				this._troopController = value;
				base.OnPropertyChangedWithValue<MissionOrderTroopControllerVM>(value, "TroopController");
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000A8A2 File Offset: 0x00008AA2
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000A8AA File Offset: 0x00008AAA
		[DataSourceProperty]
		public MissionOrderDeploymentControllerVM DeploymentController
		{
			get
			{
				return this._deploymentController;
			}
			set
			{
				if (value == this._deploymentController)
				{
					return;
				}
				this._deploymentController = value;
				base.OnPropertyChangedWithValue<MissionOrderDeploymentControllerVM>(value, "DeploymentController");
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000A8C9 File Offset: 0x00008AC9
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000A8D1 File Offset: 0x00008AD1
		[DataSourceProperty]
		public bool IsDeployment
		{
			get
			{
				return this._isDeployment;
			}
			set
			{
				this._isDeployment = value;
				base.OnPropertyChangedWithValue(value, "IsDeployment");
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000A8E6 File Offset: 0x00008AE6
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000A8EE File Offset: 0x00008AEE
		[DataSourceProperty]
		public bool HasAnyCascadingOrders
		{
			get
			{
				return this._hasAnyCascadingOrders;
			}
			set
			{
				if (value != this._hasAnyCascadingOrders)
				{
					this._hasAnyCascadingOrders = value;
					base.OnPropertyChangedWithValue(value, "HasAnyCascadingOrders");
				}
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000A90C File Offset: 0x00008B0C
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000A914 File Offset: 0x00008B14
		[DataSourceProperty]
		public bool IsToggleOrderShown
		{
			get
			{
				return this._isToggleOrderShown;
			}
			set
			{
				if (value == this._isToggleOrderShown)
				{
					return;
				}
				this._isToggleOrderShown = value;
				base.OnPropertyChangedWithValue(value, "IsToggleOrderShown");
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000A933 File Offset: 0x00008B33
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000A93B File Offset: 0x00008B3B
		[DataSourceProperty]
		public bool IsTroopListShown
		{
			get
			{
				return this._isTroopListShown;
			}
			set
			{
				if (value == this._isTroopListShown)
				{
					return;
				}
				this._isTroopListShown = value;
				base.OnPropertyChangedWithValue(value, "IsTroopListShown");
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000A95A File Offset: 0x00008B5A
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000A962 File Offset: 0x00008B62
		[DataSourceProperty]
		public bool CanUseShortcuts
		{
			get
			{
				return this._canUseShortcuts;
			}
			set
			{
				if (value != this._canUseShortcuts)
				{
					this._canUseShortcuts = value;
					base.OnPropertyChangedWithValue(value, "CanUseShortcuts");
				}
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000A980 File Offset: 0x00008B80
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000A988 File Offset: 0x00008B88
		[DataSourceProperty]
		public bool IsHolding
		{
			get
			{
				return this._isHolding;
			}
			set
			{
				if (value != this._isHolding)
				{
					this._isHolding = value;
					base.OnPropertyChangedWithValue(value, "IsHolding");
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000A9A6 File Offset: 0x00008BA6
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000A9AE File Offset: 0x00008BAE
		[DataSourceProperty]
		public bool IsAnyOrderSetActive
		{
			get
			{
				return this._isAnyOrderSetActive;
			}
			set
			{
				if (value != this._isAnyOrderSetActive)
				{
					this._isAnyOrderSetActive = value;
					base.OnPropertyChangedWithValue(value, "IsAnyOrderSetActive");
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000A9CC File Offset: 0x00008BCC
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000A9D4 File Offset: 0x00008BD4
		[DataSourceProperty]
		public string ReturnText
		{
			get
			{
				return this._returnText;
			}
			set
			{
				if (value != this._returnText)
				{
					this._returnText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReturnText");
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A9F7 File Offset: 0x00008BF7
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000A9FF File Offset: 0x00008BFF
		[DataSourceProperty]
		public bool UseAlternativeFormationLayout
		{
			get
			{
				return this._useAlternativeFormationLayout;
			}
			set
			{
				if (value != this._useAlternativeFormationLayout)
				{
					this._useAlternativeFormationLayout = value;
					base.OnPropertyChangedWithValue(value, "UseAlternativeFormationLayout");
				}
			}
		}

		// Token: 0x04000111 RID: 273
		public InputRestrictions InputRestrictions;

		// Token: 0x04000112 RID: 274
		private Timer _updateTroopsTimer;

		// Token: 0x04000113 RID: 275
		private MissionOrderCallbacks _callbacks;

		// Token: 0x04000114 RID: 276
		private bool _isTroopPlacingActive;

		// Token: 0x04000115 RID: 277
		private bool _isMultiplayer;

		// Token: 0x04000116 RID: 278
		private int _delayValueForAIFormationModifications;

		// Token: 0x04000117 RID: 279
		private readonly List<Formation> _modifiedAIFormations = new List<Formation>();

		// Token: 0x04000118 RID: 280
		private SoundEvent _slowMotionSoundEvent;

		// Token: 0x04000119 RID: 281
		private int _slowMotionSoundEventGlobalIndex;

		// Token: 0x0400011A RID: 282
		private List<MissionOrderVM.FormationConfiguration> _filterData;

		// Token: 0x0400011B RID: 283
		private Dictionary<int, InputKeyItemVM> _orderKeys;

		// Token: 0x0400011C RID: 284
		private InputKeyItemVM _returnKey;

		// Token: 0x0400011D RID: 285
		private int _lastHighlightedFormationIndex;

		// Token: 0x04000120 RID: 288
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000121 RID: 289
		private MBBindingList<OrderSetVM> _orderSets;

		// Token: 0x04000122 RID: 290
		private MissionOrderTroopControllerVM _troopController;

		// Token: 0x04000123 RID: 291
		private MissionOrderDeploymentControllerVM _deploymentController;

		// Token: 0x04000124 RID: 292
		private bool _isDeployment;

		// Token: 0x04000125 RID: 293
		private bool _hasAnyCascadingOrders;

		// Token: 0x04000126 RID: 294
		private bool _isToggleOrderShown;

		// Token: 0x04000127 RID: 295
		private bool _isTroopListShown;

		// Token: 0x04000128 RID: 296
		private bool _canUseShortcuts;

		// Token: 0x04000129 RID: 297
		private bool _isHolding;

		// Token: 0x0400012A RID: 298
		private bool _isAnyOrderSetActive;

		// Token: 0x0400012B RID: 299
		private string _returnText;

		// Token: 0x0400012C RID: 300
		private bool _useAlternativeFormationLayout;

		// Token: 0x020000BA RID: 186
		public enum CursorStates
		{
			// Token: 0x040005A9 RID: 1449
			Move,
			// Token: 0x040005AA RID: 1450
			Face,
			// Token: 0x040005AB RID: 1451
			Form
		}

		// Token: 0x020000BB RID: 187
		public struct ClassConfiguration
		{
			// Token: 0x06000C1E RID: 3102 RVA: 0x000291E9 File Offset: 0x000273E9
			public ClassConfiguration(int formationIndex, DeploymentFormationClass formationClass)
			{
				this.FormationIndex = formationIndex;
				this.FormationClass = formationClass;
			}

			// Token: 0x040005AC RID: 1452
			public int FormationIndex;

			// Token: 0x040005AD RID: 1453
			public DeploymentFormationClass FormationClass;
		}

		// Token: 0x020000BC RID: 188
		public struct FormationConfiguration
		{
			// Token: 0x06000C1F RID: 3103 RVA: 0x000291F9 File Offset: 0x000273F9
			public FormationConfiguration(int formationIndex, List<FormationFilterType> filters)
			{
				this.FormationIndex = formationIndex;
				this.Filters = filters;
			}

			// Token: 0x040005AE RID: 1454
			public int FormationIndex;

			// Token: 0x040005AF RID: 1455
			public List<FormationFilterType> Filters;
		}
	}
}
