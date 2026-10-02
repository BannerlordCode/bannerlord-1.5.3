using System;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000031 RID: 49
	[OverrideView(typeof(MissionMainAgentCheerBarkControllerView))]
	public class MissionGauntletMainAgentCheerControllerView : MissionView
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000BEC9 File Offset: 0x0000A0C9
		private bool IsDisplayingADialog
		{
			get
			{
				IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
				return (missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog()) || base.MissionScreen.IsRadialMenuActive || base.Mission.IsOrderMenuOpen;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000BEF9 File Offset: 0x0000A0F9
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000BF01 File Offset: 0x0000A101
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

		// Token: 0x06000206 RID: 518 RVA: 0x0000BF0A File Offset: 0x0000A10A
		public MissionGauntletMainAgentCheerControllerView()
		{
			this._missionScreenAsInterface = base.MissionScreen;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000BF30 File Offset: 0x0000A130
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._gauntletLayer = new GauntletLayer("MissionCheerController", this.ViewOrderPriority, false);
			this._missionMainAgentController = base.Mission.GetMissionBehavior<MissionMainAgentController>();
			this._dataSource = new MissionMainAgentCheerBarkControllerVM(new Action<int>(this.OnCheerSelect), new Action<int>(this.OnBarkSelect));
			this._gauntletLayer.LoadMovie("MainAgentCheerBarkController", this._dataSource);
			GameKeyContext category = HotKeyManager.GetCategory("CombatHotKeyCategory");
			if (this._missionMainAgentController != null)
			{
				InputContext inputContext = this._missionMainAgentController.Input as InputContext;
				if (inputContext != null && !inputContext.IsCategoryRegistered(category))
				{
					inputContext.RegisterHotKeyCategory(category);
				}
			}
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000C004 File Offset: 0x0000A204
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._missionMainAgentController = null;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C060 File Offset: 0x0000A260
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this.IsMainAgentAvailable() && base.Mission.Mode != MissionMode.Deployment && (!base.MissionScreen.IsRadialMenuActive || this._dataSource.IsActive))
			{
				this.TickControls(dt);
				return;
			}
			if (this._dataSource.IsActive)
			{
				this.HandleClosingHold(false);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000C0C0 File Offset: 0x0000A2C0
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				this.HandleClosingHold(false);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000C0D8 File Offset: 0x0000A2D8
		private void HandleNodeSelectionInput(CheerBarkNodeItemVM node, int nodeIndex, int parentNodeIndex = -1)
		{
			if (this._missionMainAgentController == null)
			{
				return;
			}
			IInputContext input = this._missionMainAgentController.Input;
			if (node.ShortcutKey != null)
			{
				if (input.IsHotKeyPressed(node.ShortcutKey.HotKey.Id))
				{
					if (parentNodeIndex != -1)
					{
						this._dataSource.SelectItem(parentNodeIndex, nodeIndex);
						return;
					}
					this._dataSource.SelectItem(nodeIndex, -1);
					this._isSelectingFromInput = node.HasSubNodes;
					return;
				}
				else if (input.IsHotKeyReleased(node.ShortcutKey.HotKey.Id))
				{
					if (!this._isSelectingFromInput)
					{
						this.HandleClosingHold(true);
						this._dataSource.Nodes.ApplyActionOnAllItems(delegate(CheerBarkNodeItemVM n)
						{
							n.ClearSelectionRecursive();
						});
					}
					this._isSelectingFromInput = false;
				}
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000C1A8 File Offset: 0x0000A3A8
		private void TickControls(float dt)
		{
			if (this._missionMainAgentController == null)
			{
				return;
			}
			IInputContext input = this._missionMainAgentController.Input;
			if (GameNetwork.IsMultiplayer && this._cooldownTimeRemaining > 0f)
			{
				this._cooldownTimeRemaining -= dt;
				if (input.IsGameKeyDown(31))
				{
					if (!this._prevCheerKeyDown && (double)this._cooldownTimeRemaining >= 0.1)
					{
						this._cooldownInfoText.SetTextVariable("SECONDS", this._cooldownTimeRemaining.ToString("0.0"));
						InformationManager.DisplayMessage(new InformationMessage(this._cooldownInfoText.ToString()));
					}
					this._prevCheerKeyDown = true;
					return;
				}
				this._prevCheerKeyDown = false;
				return;
			}
			else
			{
				if (this.HoldHandled && this._dataSource.IsActive)
				{
					int num = -1;
					for (int i = 0; i < this._dataSource.Nodes.Count; i++)
					{
						if (this._dataSource.Nodes[i].IsSelected)
						{
							num = i;
							break;
						}
					}
					if (this._dataSource.IsNodesCategories)
					{
						if (num != -1)
						{
							for (int j = 0; j < this._dataSource.Nodes[num].SubNodes.Count; j++)
							{
								this.HandleNodeSelectionInput(this._dataSource.Nodes[num].SubNodes[j], j, num);
							}
						}
						else if (input.IsHotKeyReleased("CheerBarkSelectFirstCategory"))
						{
							this._dataSource.SelectItem(0, -1);
						}
						else if (input.IsHotKeyReleased("CheerBarkSelectSecondCategory"))
						{
							this._dataSource.SelectItem(1, -1);
						}
					}
					else
					{
						for (int k = 0; k < this._dataSource.Nodes.Count; k++)
						{
							this.HandleNodeSelectionInput(this._dataSource.Nodes[k], k, -1);
						}
					}
				}
				if (input.IsGameKeyDown(31) && !this.IsDisplayingADialog && !base.MissionScreen.IsRadialMenuActive)
				{
					if (this._holdTime > 0f && !this.HoldHandled)
					{
						this.HandleOpenHold();
						this.HoldHandled = true;
					}
					this._holdTime += dt;
					this._prevCheerKeyDown = true;
					return;
				}
				if (this._prevCheerKeyDown && !input.IsGameKeyDown(31))
				{
					if (this._holdTime < 0f)
					{
						this.HandleQuickRelease();
					}
					else
					{
						this.HandleClosingHold(true);
					}
					this.HoldHandled = false;
					this._holdTime = 0f;
					this._prevCheerKeyDown = false;
				}
				return;
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000C41F File Offset: 0x0000A61F
		private void HandleOpenHold()
		{
			if (!this._dataSource.IsActive)
			{
				this._dataSource.ExecuteActivate();
				base.MissionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentCheerControllerView>(this);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000C445 File Offset: 0x0000A645
		private void HandleClosingHold(bool applySelection)
		{
			if (this._dataSource.IsActive)
			{
				this._dataSource.ExecuteDeactivate(applySelection);
				base.MissionScreen.UnregisterRadialMenuObject(this);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000C46C File Offset: 0x0000A66C
		private void HandleQuickRelease()
		{
			this.OnCheerSelect(-1);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000C478 File Offset: 0x0000A678
		private void OnCheerSelect(int tauntIndex)
		{
			if (tauntIndex < 0)
			{
				return;
			}
			if (GameNetwork.IsClient)
			{
				TauntUsageManager.TauntUsage.TauntUsageFlag actionNotUsableReason = CosmeticsManagerHelper.GetActionNotUsableReason(Agent.Main, tauntIndex);
				if (actionNotUsableReason != TauntUsageManager.TauntUsage.TauntUsageFlag.None)
				{
					InformationManager.DisplayMessage(new InformationMessage(TauntUsageManager.GetActionDisabledReasonText(actionNotUsableReason)));
					return;
				}
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new TauntSelected(tauntIndex));
				GameNetwork.EndModuleEventAsClient();
			}
			else
			{
				Agent main = Agent.Main;
				if (main != null)
				{
					main.HandleTaunt(tauntIndex, true);
				}
			}
			this._cooldownTimeRemaining = 4f;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000C4E5 File Offset: 0x0000A6E5
		private void OnBarkSelect(int indexOfBark)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new BarkSelected(indexOfBark));
				GameNetwork.EndModuleEventAsClient();
			}
			else
			{
				Agent main = Agent.Main;
				if (main != null)
				{
					main.HandleBark(indexOfBark);
				}
			}
			this._cooldownTimeRemaining = 2f;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000C521 File Offset: 0x0000A721
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive() && !Agent.Main.IsUsingGameObject && !Agent.Main.IsInWater();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000C551 File Offset: 0x0000A751
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000C576 File Offset: 0x0000A776
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000103 RID: 259
		private const float CooldownPeriodDurationAfterCheer = 4f;

		// Token: 0x04000104 RID: 260
		private const float CooldownPeriodDurationAfterBark = 2f;

		// Token: 0x04000105 RID: 261
		private const float _minHoldTime = 0f;

		// Token: 0x04000106 RID: 262
		private readonly IMissionScreen _missionScreenAsInterface;

		// Token: 0x04000107 RID: 263
		private MissionMainAgentController _missionMainAgentController;

		// Token: 0x04000108 RID: 264
		private readonly TextObject _cooldownInfoText = new TextObject("{=aogZyZlR}You need to wait {SECONDS} seconds until you can cheer/shout again.", null);

		// Token: 0x04000109 RID: 265
		private bool _holdHandled;

		// Token: 0x0400010A RID: 266
		private float _holdTime;

		// Token: 0x0400010B RID: 267
		private bool _prevCheerKeyDown;

		// Token: 0x0400010C RID: 268
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400010D RID: 269
		private MissionMainAgentCheerBarkControllerVM _dataSource;

		// Token: 0x0400010E RID: 270
		private float _cooldownTimeRemaining;

		// Token: 0x0400010F RID: 271
		private bool _isSelectingFromInput;
	}
}
