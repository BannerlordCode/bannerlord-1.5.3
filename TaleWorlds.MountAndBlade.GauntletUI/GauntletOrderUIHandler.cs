using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.MissionViews.Order;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000016 RID: 22
	public abstract class GauntletOrderUIHandler : MissionView
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B9 RID: 185
		public abstract bool IsDeployment { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BA RID: 186
		public abstract bool IsSiegeDeployment { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000BB RID: 187
		public abstract bool IsValidForTick { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000060D2 File Offset: 0x000042D2
		public MissionOrderVM.CursorStates CursorState
		{
			get
			{
				MissionOrderVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return MissionOrderVM.CursorStates.Move;
				}
				return dataSource.CursorState;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000060E5 File Offset: 0x000042E5
		protected float _minHoldTimeForActivation
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000060EC File Offset: 0x000042EC
		public bool IsOrderMenuActive
		{
			get
			{
				MissionOrderVM dataSource = this._dataSource;
				return dataSource != null && dataSource.IsToggleOrderShown;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000060FF File Offset: 0x000042FF
		public bool IsAnyOrderSetActive
		{
			get
			{
				MissionOrderVM dataSource = this._dataSource;
				return dataSource != null && dataSource.IsAnyOrderSetActive;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00006112 File Offset: 0x00004312
		public bool IsViewCreated
		{
			get
			{
				return this._gauntletLayer != null && this._dataSource != null;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006127 File Offset: 0x00004327
		public GauntletOrderUIHandler()
		{
			this.ViewOrderPriority = 14;
		}

		// Token: 0x060000C2 RID: 194
		protected abstract void OnTransferFinished();

		// Token: 0x060000C3 RID: 195
		protected abstract void SetLayerEnabled(bool isEnabled);

		// Token: 0x060000C4 RID: 196 RVA: 0x0000614D File Offset: 0x0000434D
		protected virtual void SetSuspendTroopPlacer(bool value)
		{
			this._orderTroopPlacer.SuspendTroopPlacer = value;
			base.MissionScreen.SetOrderFlagVisibility(!value);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000616A File Offset: 0x0000436A
		public virtual void SelectFormationAtIndex(int index)
		{
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.OnTroopFormationSelected(index);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000617D File Offset: 0x0000437D
		public virtual void DeselectFormationAtIndex(int index)
		{
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.TroopController.OnDeselectFormation(index);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00006195 File Offset: 0x00004395
		protected virtual IOrderable GetFocusedOrderableObject()
		{
			OrderFlag orderFlag = base.MissionScreen.OrderFlag;
			if (orderFlag == null)
			{
				return null;
			}
			return orderFlag.FocusedOrderableObject;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000061B0 File Offset: 0x000043B0
		protected VisualOrderExecutionParameters GetVisualOrderExecutionParameters()
		{
			WorldPosition? worldPosition = null;
			if (base.MissionScreen.Mission.Scene != null)
			{
				Vec3 orderFlagPosition = base.MissionScreen.GetOrderFlagPosition();
				worldPosition = new WorldPosition?(new WorldPosition(base.MissionScreen.Mission.Scene, orderFlagPosition));
			}
			return new VisualOrderExecutionParameters(Agent.Main, this._focusedFormation, worldPosition);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006218 File Offset: 0x00004418
		public override void OnMissionScreenActivate()
		{
			base.OnMissionScreenActivate();
			if (this._dataSource != null)
			{
				this._dataSource.AfterInitialize();
				this._isInitialized = true;
			}
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006265 File Offset: 0x00004465
		public override void OnMissionScreenDeactivate()
		{
			base.OnMissionScreenDeactivate();
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000628D File Offset: 0x0000448D
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveStateChanged));
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000062B5 File Offset: 0x000044B5
		private void OnGamepadActiveStateChanged()
		{
			if (this._dataSource != null)
			{
				this._dataSource.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
				{
					t.UpdateSelectionKeyInfo();
				});
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000062F4 File Offset: 0x000044F4
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._latestDt = dt;
			this._isReceivingInput = false;
			if (this.IsValidForTick && this._dataSource != null && this._gauntletLayer.IsActive)
			{
				this.TickInput(dt);
				this._dataSource.Update();
				if (this._dataSource.IsToggleOrderShown)
				{
					if (this._targetFormationOrderGivenWithActionButton)
					{
						this.SetSuspendTroopPlacer(false);
						this._targetFormationOrderGivenWithActionButton = false;
					}
					OrderTroopPlacer orderTroopPlacer = this._orderTroopPlacer;
					OrderSetVM selectedOrderSet = this._dataSource.SelectedOrderSet;
					orderTroopPlacer.IsDrawingForced = ((selectedOrderSet != null) ? selectedOrderSet.OrderSet.StringId : null) == "order_type_movement";
					OrderTroopPlacer orderTroopPlacer2 = this._orderTroopPlacer;
					OrderSetVM selectedOrderSet2 = this._dataSource.SelectedOrderSet;
					orderTroopPlacer2.IsDrawingFacing = ((selectedOrderSet2 != null) ? selectedOrderSet2.OrderSet.StringId : null) == "order_type_facing";
					this._orderTroopPlacer.IsDrawingForming = false;
					if (this.CursorState == MissionOrderVM.CursorStates.Face)
					{
						Vec2 orderLookAtDirection = OrderController.GetOrderLookAtDirection(base.Mission.MainAgent.Team.PlayerOrderController.SelectedFormations, base.MissionScreen.OrderFlag.Position.AsVec2);
						base.MissionScreen.OrderFlag.SetArrowVisibility(true, orderLookAtDirection);
					}
					else
					{
						base.MissionScreen.OrderFlag.SetArrowVisibility(false, Vec2.Invalid);
					}
					if (this.CursorState == MissionOrderVM.CursorStates.Form)
					{
						float orderFormCustomWidth = OrderController.GetOrderFormCustomWidth(base.Mission.MainAgent.Team.PlayerOrderController.SelectedFormations, base.MissionScreen.OrderFlag.Position);
						base.MissionScreen.OrderFlag.SetWidthVisibility(true, orderFormCustomWidth);
					}
					else
					{
						base.MissionScreen.OrderFlag.SetWidthVisibility(false, -1f);
					}
					if (TaleWorlds.InputSystem.Input.IsGamepadActive)
					{
						OrderSetVM selectedOrderSet3 = this._dataSource.SelectedOrderSet;
						if (selectedOrderSet3 == null || selectedOrderSet3.HasSingleOrder)
						{
							if (this._orderTroopPlacer.SuspendTroopPlacer)
							{
								this._orderTroopPlacer.SuspendTroopPlacer = false;
							}
						}
						else if (!this._orderTroopPlacer.SuspendTroopPlacer)
						{
							this._orderTroopPlacer.SuspendTroopPlacer = true;
						}
					}
				}
				else if (this._dataSource.TroopController.IsTransferActive || this.IsDeployment)
				{
					this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				}
				else
				{
					if (!this._dataSource.TroopController.IsTransferActive && !this._orderTroopPlacer.SuspendTroopPlacer)
					{
						this._orderTroopPlacer.SuspendTroopPlacer = true;
					}
					this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				}
				if (this.IsDeployment)
				{
					if (!base.MissionScreen.IsRadialMenuActive && (base.MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.RightMouseButton) || base.MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.ControllerLTrigger)))
					{
						this._gauntletLayer.InputRestrictions.SetMouseVisibility(false);
					}
					else
					{
						this._gauntletLayer.InputRestrictions.SetMouseVisibility(true);
					}
				}
				base.MissionScreen.OrderFlag.IsTroop = true;
				this.TickOrderFlag(this._latestDt, false);
			}
			bool flag = this.IsOrderRadialActive();
			if (this._isOrderRadialEnabled && !flag)
			{
				base.MissionScreen.UnregisterRadialMenuObject(this);
			}
			else if (!this._isOrderRadialEnabled && flag)
			{
				base.MissionScreen.RegisterRadialMenuObject<GauntletOrderUIHandler>(this);
			}
			this._isOrderRadialEnabled = flag;
			this._targetFormationOrderGivenWithActionButton = false;
			MissionOrderVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.UpdateCanUseShortcuts(this._isReceivingInput);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006664 File Offset: 0x00004864
		protected virtual void TickInput(float dt)
		{
			if (this._dataSource == null)
			{
				return;
			}
			bool displayDialog = ((IMissionScreen)base.MissionScreen).GetDisplayDialog();
			bool flag = base.MissionScreen.SceneLayer.IsHitThisFrame || this._gauntletLayer.IsHitThisFrame;
			if (displayDialog || (TaleWorlds.InputSystem.Input.IsGamepadActive && !flag))
			{
				this._isReceivingInput = false;
				this._dataSource.UpdateCanUseShortcuts(false);
				return;
			}
			if (TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				for (int i = 0; i < this._dataSource.TroopController.TroopList.Count; i++)
				{
					OrderTroopItemVM orderTroopItemVM = this._dataSource.TroopController.TroopList[i];
					orderTroopItemVM.ShowSelectionInputs = orderTroopItemVM.IsSelectionHighlightActive && orderTroopItemVM.IsSelectable;
				}
			}
			else
			{
				for (int j = 0; j < this._dataSource.TroopController.TroopList.Count; j++)
				{
					OrderTroopItemVM orderTroopItemVM2 = this._dataSource.TroopController.TroopList[j];
					orderTroopItemVM2.IsSelectionHighlightActive = false;
					orderTroopItemVM2.ShowSelectionInputs = orderTroopItemVM2.IsSelectable;
				}
			}
			this._isReceivingInput = true;
			if (!this.IsDeployment)
			{
				if (!this._holdHandled && base.Input.IsGameKeyDown(87) && !this._dataSource.IsToggleOrderShown)
				{
					this._holdTime += dt;
					if (this._holdTime >= this._minHoldTimeForActivation)
					{
						this._dataSource.OpenToggleOrder(true, !this._dataSource.IsHolding);
						this._dataSource.IsHolding = true;
						this._holdHandled = true;
					}
				}
				else if (this._holdHandled && !base.Input.IsGameKeyDown(87))
				{
					if (this._dataSource.IsHolding && this._dataSource.IsToggleOrderShown)
					{
						this._dataSource.TryCloseToggleOrder(true);
					}
					this._dataSource.IsHolding = false;
					this._holdTime = 0f;
					this._holdHandled = false;
				}
			}
			if (this._dataSource.IsToggleOrderShown)
			{
				if (base.Input.IsKeyReleased(InputKey.LeftMouseButton) || base.Input.IsKeyReleased(InputKey.ControllerRTrigger))
				{
					if (this._dataSource.SelectedOrderSet != null && TaleWorlds.InputSystem.Input.IsGamepadActive)
					{
						VisualOrderExecutionParameters visualOrderExecutionParameters = this.GetVisualOrderExecutionParameters();
						OrderItemVM orderItemVM = this._dataSource.SelectedOrderSet.Orders.FirstOrDefault<OrderItemVM>((OrderItemVM o) => o.IsSelected);
						if (orderItemVM != null)
						{
							orderItemVM.ExecuteAction(visualOrderExecutionParameters);
						}
						if (this.IsDeployment || this._dataSource.IsHolding)
						{
							OrderSetVM selectedOrderSet = this._dataSource.SelectedOrderSet;
							if (selectedOrderSet != null)
							{
								selectedOrderSet.ExecuteDeSelect();
							}
						}
						else
						{
							this._dataSource.TryCloseToggleOrder(false);
						}
					}
					else
					{
						switch (this.CursorState)
						{
						case MissionOrderVM.CursorStates.Move:
							if (this._focusedFormation != null)
							{
								OrderItemBaseVM chargeOrder = this.GetChargeOrder();
								VisualOrderExecutionParameters visualOrderExecutionParameters2 = this.GetVisualOrderExecutionParameters();
								chargeOrder.ExecuteAction(visualOrderExecutionParameters2);
								this.SetSuspendTroopPlacer(true);
								this._targetFormationOrderGivenWithActionButton = true;
								if (!this._dataSource.IsHolding)
								{
									this._dataSource.TryCloseToggleOrder(false);
								}
							}
							else
							{
								IOrderable focusedOrderableObject = this.GetFocusedOrderableObject();
								if (focusedOrderableObject != null)
								{
									if (this._dataSource.OrderController.SelectedFormations.Count > 0)
									{
										this._dataSource.OrderController.SetOrderWithOrderableObject(focusedOrderableObject);
									}
									else
									{
										Debug.FailedAssert("No selected formations when issuing order", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletOrderUIBase.cs", "TickInput", 377);
									}
								}
							}
							break;
						case MissionOrderVM.CursorStates.Face:
							this._dataSource.OrderController.SetOrderWithPosition(OrderType.LookAtDirection, new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, base.MissionScreen.GetOrderFlagPosition(), false));
							break;
						case MissionOrderVM.CursorStates.Form:
							this._dataSource.OrderController.SetOrderWithPosition(OrderType.FormCustom, new WorldPosition(Mission.Current.Scene, UIntPtr.Zero, base.MissionScreen.GetOrderFlagPosition(), false));
							break;
						default:
							Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletOrderUIBase.cs", "TickInput", 392);
							break;
						}
					}
				}
				if (base.Input.IsKeyReleased(InputKey.RightMouseButton) && !this.IsDeployment)
				{
					this._dataSource.OnEscape();
				}
			}
			else if (this._dataSource.TroopController.IsTransferActive != this._isTransferEnabled)
			{
				this._isTransferEnabled = this._dataSource.TroopController.IsTransferActive;
				if (!this._isTransferEnabled)
				{
					this._gauntletLayer.UIContext.ContextAlpha = (BannerlordConfig.HideBattleUI ? 0f : 1f);
					this._gauntletLayer.IsFocusLayer = false;
					ScreenManager.TryLoseFocus(this._gauntletLayer);
				}
				else
				{
					this._gauntletLayer.UIContext.ContextAlpha = 1f;
					this._gauntletLayer.IsFocusLayer = true;
					ScreenManager.TrySetFocus(this._gauntletLayer);
				}
			}
			else if (this._dataSource.TroopController.IsTransferActive)
			{
				if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.TroopController.ExecuteCancelTransfer();
				}
				else if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
				{
					if (this._dataSource.TroopController.IsTransferValid)
					{
						UISoundsHelper.PlayUISound("event:/ui/default");
						this._dataSource.TroopController.ExecuteConfirmTransfer();
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyReleased("Reset"))
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._dataSource.TroopController.ExecuteReset();
				}
			}
			int num = -1;
			if ((!TaleWorlds.InputSystem.Input.IsGamepadActive || this._dataSource.IsToggleOrderShown) && !base.DebugInput.IsControlDown())
			{
				if (base.Input.IsGameKeyPressed(69))
				{
					num = 0;
				}
				else if (base.Input.IsGameKeyPressed(70))
				{
					num = 1;
				}
				else if (base.Input.IsGameKeyPressed(71))
				{
					num = 2;
				}
				else if (base.Input.IsGameKeyPressed(72))
				{
					num = 3;
				}
				else if (base.Input.IsGameKeyPressed(73))
				{
					num = 4;
				}
				else if (base.Input.IsGameKeyPressed(74))
				{
					num = 5;
				}
				else if (base.Input.IsGameKeyPressed(75))
				{
					num = 6;
				}
				else if (base.Input.IsGameKeyPressed(76))
				{
					num = 7;
				}
				else if (base.Input.IsGameKeyPressed(77) && !TaleWorlds.InputSystem.Input.IsGamepadActive)
				{
					num = 8;
				}
			}
			if (num > -1)
			{
				if (this._dataSource.SelectedOrderSet != null)
				{
					int count = this._dataSource.SelectedOrderSet.Orders.Count;
					if (count > 0 && num >= 0)
					{
						if (num == 8)
						{
							if (this._dataSource.SelectedOrderSet.Orders.Any<OrderItemVM>((OrderItemVM x) => x.Order is ReturnVisualOrder))
							{
								this._dataSource.SelectedOrderSet.ExecuteDeSelect();
								goto IL_082E;
							}
						}
						if (num < count)
						{
							OrderItemVM orderItemVM2 = this._dataSource.SelectedOrderSet.Orders[num];
							if (!(orderItemVM2.Order is ReturnVisualOrder))
							{
								VisualOrderExecutionParameters visualOrderExecutionParameters3 = this.GetVisualOrderExecutionParameters();
								orderItemVM2.ExecuteAction(visualOrderExecutionParameters3);
								if (this.IsDeployment || this._dataSource.IsHolding)
								{
									OrderSetVM selectedOrderSet2 = this._dataSource.SelectedOrderSet;
									if (selectedOrderSet2 != null)
									{
										selectedOrderSet2.ExecuteDeSelect();
									}
								}
								else
								{
									this._dataSource.TryCloseToggleOrder(false);
								}
							}
						}
					}
				}
				else
				{
					this._dataSource.OpenToggleOrder(false, true);
					if (this._dataSource.IsToggleOrderShown)
					{
						if (num == 8)
						{
							if (this._dataSource.OrderSets.Any<OrderSetVM>((OrderSetVM x) => x.HasSingleOrder && x.Orders[0].Order is ReturnVisualOrder))
							{
								this._dataSource.TryCloseToggleOrder(false);
								goto IL_082E;
							}
						}
						OrderSetVM orderSetAtIndex = this._dataSource.GetOrderSetAtIndex(num);
						if (orderSetAtIndex != null && (!orderSetAtIndex.HasSingleOrder || !(orderSetAtIndex.Orders[0].Order is ReturnVisualOrder)))
						{
							this._dataSource.TrySelectOrderSet(orderSetAtIndex);
						}
					}
				}
			}
			IL_082E:
			int num2 = -1;
			if (base.Input.IsGameKeyPressed(78))
			{
				num2 = 100;
			}
			else if (base.Input.IsGameKeyPressed(79))
			{
				num2 = 0;
			}
			else if (base.Input.IsGameKeyPressed(80))
			{
				num2 = 1;
			}
			else if (base.Input.IsGameKeyPressed(81))
			{
				num2 = 2;
			}
			else if (base.Input.IsGameKeyPressed(82))
			{
				num2 = 3;
			}
			else if (base.Input.IsGameKeyPressed(83))
			{
				num2 = 4;
			}
			else if (base.Input.IsGameKeyPressed(84))
			{
				num2 = 5;
			}
			else if (base.Input.IsGameKeyPressed(85))
			{
				num2 = 6;
			}
			else if (base.Input.IsGameKeyPressed(86))
			{
				num2 = 7;
			}
			if (!this.IsDeployment && this._dataSource.IsToggleOrderShown && TaleWorlds.InputSystem.Input.IsGamepadActive)
			{
				if (base.Input.IsGameKeyPressed(88))
				{
					this._dataSource.OnTroopHighlightSelection(true);
				}
				else if (base.Input.IsGameKeyPressed(89))
				{
					this._dataSource.OnTroopHighlightSelection(false);
				}
				else if (base.Input.IsGameKeyPressed(90))
				{
					this._dataSource.ExecuteSelectHighlightedFormation();
				}
				else if (base.Input.IsGameKeyPressed(91))
				{
					this._dataSource.ExecuteToggleHighlightedFormation();
				}
			}
			if (num2 != -1)
			{
				this._dataSource.OnTroopFormationSelected(num2);
			}
			if (base.Input.IsGameKeyPressed(68))
			{
				this._dataSource.ViewOrders();
			}
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007018 File Offset: 0x00005218
		protected virtual OrderItemVM GetChargeOrder()
		{
			if (this._dataSource == null)
			{
				return null;
			}
			for (int i = 0; i < this._dataSource.OrderSets.Count; i++)
			{
				OrderSetVM orderSetVM = this._dataSource.OrderSets[i];
				for (int j = 0; j < orderSetVM.Orders.Count; j++)
				{
					OrderItemVM orderItemVM = orderSetVM.Orders[j];
					if (orderItemVM.Order.StringId == "order_movement_charge")
					{
						return orderItemVM;
					}
				}
			}
			return null;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007099 File Offset: 0x00005299
		public override void OnAgentBuild(Agent agent, Banner banner)
		{
			if (!this._isInitialized)
			{
				return;
			}
			if (agent.IsHuman && this._dataSource != null)
			{
				this._dataSource.TroopController.AddTroops(agent);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000070C5 File Offset: 0x000052C5
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
		{
			base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, killingBlow);
			if (affectedAgent.IsHuman && this._dataSource != null)
			{
				this._dataSource.TroopController.RemoveTroops(affectedAgent);
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000070F3 File Offset: 0x000052F3
		public override bool OnEscape()
		{
			if (this._dataSource != null)
			{
				bool isToggleOrderShown = this._dataSource.IsToggleOrderShown;
				this._dataSource.OnEscape();
				return isToggleOrderShown;
			}
			return false;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00007115 File Offset: 0x00005315
		public override bool IsReady()
		{
			return this._spriteCategory.IsCategoryFullyLoaded();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00007124 File Offset: 0x00005324
		private bool IsOrderRadialActive()
		{
			if (this._dataSource != null && this._dataSource.IsToggleOrderShown && (TaleWorlds.InputSystem.Input.IsGamepadActive || base.Mission.Mode == MissionMode.Deployment))
			{
				return this._dataSource.OrderSets.Any<OrderSetVM>((OrderSetVM x) => x.IsSelected);
			}
			return false;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000718C File Offset: 0x0000538C
		public void OnActivateToggleOrder()
		{
			this.SetLayerEnabled(true);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007195 File Offset: 0x00005395
		public void OnDeactivateToggleOrder()
		{
			if (this._dataSource != null && !this._dataSource.TroopController.IsTransferActive)
			{
				this.SetLayerEnabled(false);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000071B8 File Offset: 0x000053B8
		protected void OnBeforeOrder()
		{
			this.TickOrderFlag(this._latestDt, true);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x000071C8 File Offset: 0x000053C8
		protected void TickOrderFlag(float dt, bool forceUpdate)
		{
			if ((base.MissionScreen.OrderFlag.IsVisible || forceUpdate) && Utilities.EngineFrameNo != base.MissionScreen.OrderFlag.LatestUpdateFrameNo)
			{
				base.MissionScreen.OrderFlag.Tick(this._latestDt);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007216 File Offset: 0x00005416
		protected void ToggleScreenRotation(bool isLocked)
		{
			MissionScreen.SetFixedMissionCameraActive(isLocked);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000721E File Offset: 0x0000541E
		protected override void OnSuspendView()
		{
			base.OnSuspendView();
			this._dataSource.TryCloseToggleOrder(false);
			ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000723F File Offset: 0x0000543F
		protected override void OnResumeView()
		{
			base.OnResumeView();
			ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
		}

		// Token: 0x04000078 RID: 120
		protected Formation _focusedFormation;

		// Token: 0x04000079 RID: 121
		protected string _radialOrderMovieName = "OrderRadial";

		// Token: 0x0400007A RID: 122
		protected string _barOrderMovieName = "OrderBar";

		// Token: 0x0400007B RID: 123
		protected float _holdTime;

		// Token: 0x0400007C RID: 124
		protected bool _holdHandled;

		// Token: 0x0400007D RID: 125
		protected OrderTroopPlacer _orderTroopPlacer;

		// Token: 0x0400007E RID: 126
		protected GauntletLayer _gauntletLayer;

		// Token: 0x0400007F RID: 127
		protected GauntletMovieIdentifier _movie;

		// Token: 0x04000080 RID: 128
		protected SpriteCategory _spriteCategory;

		// Token: 0x04000081 RID: 129
		protected MissionOrderVM _dataSource;

		// Token: 0x04000082 RID: 130
		protected SiegeDeploymentHandler _siegeDeploymentHandler;

		// Token: 0x04000083 RID: 131
		protected MissionFormationTargetSelectionHandler _formationTargetHandler;

		// Token: 0x04000084 RID: 132
		protected bool _isOrderRadialEnabled;

		// Token: 0x04000085 RID: 133
		protected bool _isReceivingInput;

		// Token: 0x04000086 RID: 134
		protected bool _isInitialized;

		// Token: 0x04000087 RID: 135
		protected bool _slowedDownMission;

		// Token: 0x04000088 RID: 136
		protected float _latestDt;

		// Token: 0x04000089 RID: 137
		protected bool _targetFormationOrderGivenWithActionButton;

		// Token: 0x0400008A RID: 138
		protected bool _isTransferEnabled;
	}
}
