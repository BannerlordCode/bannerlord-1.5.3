using System;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000033 RID: 51
	[OverrideView(typeof(MissionMainAgentEquipDropView))]
	public class MissionGauntletMainAgentEquipDropView : MissionView
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000CCC7 File Offset: 0x0000AEC7
		private bool IsDisplayingADialog
		{
			get
			{
				IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
				return (missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog()) || base.MissionScreen.IsRadialMenuActive || base.Mission.IsOrderMenuOpen;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000CCF7 File Offset: 0x0000AEF7
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000CCFF File Offset: 0x0000AEFF
		private bool HoldHandled
		{
			get
			{
				return this._holdHandled;
			}
			set
			{
				this._holdHandled = value;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000CD08 File Offset: 0x0000AF08
		public MissionGauntletMainAgentEquipDropView()
		{
			this._missionScreenAsInterface = base.MissionScreen;
			this.HoldHandled = false;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000CD24 File Offset: 0x0000AF24
		public override void EarlyStart()
		{
			base.EarlyStart();
			this._gauntletLayer = new GauntletLayer("MissionEquipDrop", this.ViewOrderPriority, false);
			this._dataSource = new MissionMainAgentControllerEquipDropVM(new Action<EquipmentIndex>(this.OnToggleItem));
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._missionControllerLeaveLogic = base.Mission.GetMissionBehavior<EquipmentControllerLeaveLogic>();
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("CombatHotKeyCategory"));
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			this._gauntletLayer.LoadMovie("MainAgentControllerEquipDrop", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000CE12 File Offset: 0x0000B012
		public override void AfterStart()
		{
			base.AfterStart();
			this._dataSource.InitializeMainAgentPropterties();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000CE28 File Offset: 0x0000B028
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(TaleWorlds.InputSystem.Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
			base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._missionMainAgentController = null;
			this._missionControllerLeaveLogic = null;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CEAC File Offset: 0x0000B0AC
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this._dataSource.IsActive && !this.IsMainAgentAvailable())
			{
				this.HandleClosingHold();
			}
			if (this.IsMainAgentAvailable() && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsActive))
			{
				this.TickControls(dt);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CF04 File Offset: 0x0000B104
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				if (this.HoldHandled)
				{
					this.HoldHandled = false;
				}
				this._toggleHoldTime = 0f;
				this._dataSource.OnCancelHoldController();
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000CF38 File Offset: 0x0000B138
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent == Agent.Main)
			{
				this.HandleClosingHold();
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000CF48 File Offset: 0x0000B148
		private void TickControls(float dt)
		{
			if ((base.MissionScreen.SceneLayer.Input.IsGameKeyDown(34) || this._gauntletLayer.Input.IsGameKeyDown(34)) && !this.IsDisplayingADialog && !base.MissionScreen.IsPhotoModeEnabled && base.Mission.Mode != MissionMode.Deployment && base.Mission.Mode != MissionMode.CutScene && !base.MissionScreen.IsRadialMenuActive)
			{
				if (this._toggleHoldTime > 0.3f && !this.HoldHandled)
				{
					this.HandleOpeningHold();
					this.HoldHandled = true;
				}
				this._toggleHoldTime += dt;
				this._prevKeyDown = true;
			}
			else if (this._prevKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(34) && !this._gauntletLayer.Input.IsGameKeyDown(34))
			{
				if (this._toggleHoldTime < 0.3f)
				{
					this.HandleQuickRelease();
				}
				else
				{
					this.HandleClosingHold();
				}
				this.HoldHandled = false;
				this._toggleHoldTime = 0f;
				this._weaponDropHoldTime = 0f;
				this._prevKeyDown = false;
				this._weaponDropHandled = false;
			}
			if (!this.HoldHandled)
			{
				this._weaponDropHoldTime = 0f;
				this._weaponDropHandled = false;
				return;
			}
			int keyWeaponIndex = this.GetKeyWeaponIndex(false);
			int keyWeaponIndex2 = this.GetKeyWeaponIndex(true);
			this._dataSource.SetDropProgressForIndex(EquipmentIndex.None, this._weaponDropHoldTime / 0.5f);
			if (keyWeaponIndex != -1)
			{
				if (!this._weaponDropHandled)
				{
					int num = keyWeaponIndex;
					if (this._weaponDropHoldTime > 0.5f && !Agent.Main.Equipment[num].IsEmpty)
					{
						this.OnDropEquipment((EquipmentIndex)num);
						this._dataSource.OnWeaponDroppedAtIndex(keyWeaponIndex);
						this._weaponDropHandled = true;
					}
					this._dataSource.SetDropProgressForIndex((EquipmentIndex)num, this._weaponDropHoldTime / 0.5f);
				}
				this._weaponDropHoldTime += dt;
				return;
			}
			if (keyWeaponIndex2 != -1)
			{
				if (!this._weaponDropHandled)
				{
					int num2 = keyWeaponIndex2;
					if (!Agent.Main.Equipment[num2].IsEmpty && num2 != 4)
					{
						this.OnToggleItem((EquipmentIndex)num2);
						this._dataSource.OnWeaponEquippedAtIndex(keyWeaponIndex2);
						this._weaponDropHandled = true;
					}
				}
				this._weaponDropHoldTime = 0f;
				return;
			}
			this._weaponDropHoldTime = 0f;
			this._weaponDropHandled = false;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000D19C File Offset: 0x0000B39C
		private void HandleOpeningHold()
		{
			MissionMainAgentControllerEquipDropVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnToggle(true);
			}
			base.MissionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentEquipDropView>(this);
			EquipmentControllerLeaveLogic missionControllerLeaveLogic = this._missionControllerLeaveLogic;
			if (missionControllerLeaveLogic != null)
			{
				missionControllerLeaveLogic.SetIsEquipmentSelectionActive(true);
			}
			if (!GameNetwork.IsMultiplayer && !this._isSlowDownApplied)
			{
				base.Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.25f, 624));
				this._isSlowDownApplied = true;
			}
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000D220 File Offset: 0x0000B420
		private void HandleClosingHold()
		{
			MissionMainAgentControllerEquipDropVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnToggle(false);
			}
			base.MissionScreen.UnregisterRadialMenuObject(this);
			EquipmentControllerLeaveLogic missionControllerLeaveLogic = this._missionControllerLeaveLogic;
			if (missionControllerLeaveLogic != null)
			{
				missionControllerLeaveLogic.SetIsEquipmentSelectionActive(false);
			}
			if (!GameNetwork.IsMultiplayer && this._isSlowDownApplied)
			{
				base.Mission.RemoveTimeSpeedRequest(624);
				this._isSlowDownApplied = false;
			}
			this._gauntletLayer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(this._gauntletLayer);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000D29A File Offset: 0x0000B49A
		private void HandleQuickRelease()
		{
			this._missionMainAgentController.OnWeaponUsageToggleRequested();
			MissionMainAgentControllerEquipDropVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnToggle(false);
			}
			base.MissionScreen.UnregisterRadialMenuObject(this);
			EquipmentControllerLeaveLogic missionControllerLeaveLogic = this._missionControllerLeaveLogic;
			if (missionControllerLeaveLogic == null)
			{
				return;
			}
			missionControllerLeaveLogic.SetIsEquipmentSelectionActive(false);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
		private void OnToggleItem(EquipmentIndex indexToToggle)
		{
			bool flag = indexToToggle == Agent.Main.GetPrimaryWieldedItemIndex();
			bool flag2 = indexToToggle == Agent.Main.GetOffhandWieldedItemIndex();
			if (flag || flag2)
			{
				Agent.Main.TryToSheathWeaponInHand(flag ? Agent.HandIndex.MainHand : Agent.HandIndex.OffHand, Agent.WeaponWieldActionType.WithAnimation);
				return;
			}
			Agent.Main.TryToWieldWeaponInSlot(indexToToggle, Agent.WeaponWieldActionType.WithAnimation, false);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D328 File Offset: 0x0000B528
		private void OnDropEquipment(EquipmentIndex indexToDrop)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new DropWeapon(base.Input.IsGameKeyDown(10), indexToDrop));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Agent.Main.HandleDropWeapon(base.Input.IsGameKeyDown(10), indexToDrop);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000D378 File Offset: 0x0000B578
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			if (main != null && main.IsActive())
			{
				Agent main2 = Agent.Main;
				return (main2 != null && !main2.Mission.IsNavalBattle) || !Agent.Main.IsUsingGameObject;
			}
			return false;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000D3E9 File Offset: 0x0000B5E9
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D40E File Offset: 0x0000B60E
		private void OnGamepadActiveChanged()
		{
			this._dataSource.OnGamepadActiveChanged(TaleWorlds.InputSystem.Input.IsGamepadActive);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D420 File Offset: 0x0000B620
		private int GetKeyWeaponIndex(bool isReleased)
		{
			Func<string, bool> func;
			if (isReleased)
			{
				func = new Func<string, bool>(this._gauntletLayer.Input.IsHotKeyReleased);
			}
			else
			{
				func = new Func<string, bool>(this._gauntletLayer.Input.IsHotKeyDown);
			}
			string text = string.Empty;
			if (func("ControllerEquipDropWeapon1"))
			{
				text = "ControllerEquipDropWeapon1";
			}
			else if (func("ControllerEquipDropWeapon2"))
			{
				text = "ControllerEquipDropWeapon2";
			}
			else if (func("ControllerEquipDropWeapon3"))
			{
				text = "ControllerEquipDropWeapon3";
			}
			else if (func("ControllerEquipDropWeapon4"))
			{
				text = "ControllerEquipDropWeapon4";
			}
			else if (func("ControllerEquipDropExtraWeapon"))
			{
				text = "ControllerEquipDropExtraWeapon";
			}
			if (!string.IsNullOrEmpty(text))
			{
				for (int i = 0; i < this._dataSource.EquippedWeapons.Count; i++)
				{
					InputKeyItemVM shortcutKey = this._dataSource.EquippedWeapons[i].ShortcutKey;
					if (((shortcutKey != null) ? shortcutKey.HotKey.Id : null) == text)
					{
						return (int)this._dataSource.EquippedWeapons[i].Identifier;
					}
				}
				ControllerEquippedItemVM equippedExtraWeapon = this._dataSource.EquippedExtraWeapon;
				string text2;
				if (equippedExtraWeapon == null)
				{
					text2 = null;
				}
				else
				{
					InputKeyItemVM shortcutKey2 = equippedExtraWeapon.ShortcutKey;
					text2 = ((shortcutKey2 != null) ? shortcutKey2.HotKey.Id : null);
				}
				if (text2 == text)
				{
					return (int)this._dataSource.EquippedExtraWeapon.Identifier;
				}
			}
			return -1;
		}

		// Token: 0x0400011A RID: 282
		private const int _missionTimeSpeedRequestID = 624;

		// Token: 0x0400011B RID: 283
		private const float _slowDownAmountWhileRadialIsOpen = 0.25f;

		// Token: 0x0400011C RID: 284
		private bool _isSlowDownApplied;

		// Token: 0x0400011D RID: 285
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400011E RID: 286
		private MissionMainAgentControllerEquipDropVM _dataSource;

		// Token: 0x0400011F RID: 287
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x04000120 RID: 288
		private EquipmentControllerLeaveLogic _missionControllerLeaveLogic;

		// Token: 0x04000121 RID: 289
		private const float _minOpenHoldTime = 0.3f;

		// Token: 0x04000122 RID: 290
		private const float _minDropHoldTime = 0.5f;

		// Token: 0x04000123 RID: 291
		private readonly IMissionScreen _missionScreenAsInterface;

		// Token: 0x04000124 RID: 292
		private bool _holdHandled;

		// Token: 0x04000125 RID: 293
		private float _toggleHoldTime;

		// Token: 0x04000126 RID: 294
		private float _weaponDropHoldTime;

		// Token: 0x04000127 RID: 295
		private bool _prevKeyDown;

		// Token: 0x04000128 RID: 296
		private bool _weaponDropHandled;
	}
}
