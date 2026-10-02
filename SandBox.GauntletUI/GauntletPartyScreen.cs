using System;
using SandBox.View;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Party.PartyTroopManagerPopUp;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI
{
	// Token: 0x0200000E RID: 14
	[GameStateScreen(typeof(PartyState))]
	public class GauntletPartyScreen : ScreenBase, IGameStateListener, IChangeableScreen, IPartyScreenLogicHandler, IPartyScreenPrisonHandler, IPartyScreenTroopHandler
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000068C8 File Offset: 0x00004AC8
		public bool IsTroopUpgradesDisabled
		{
			get
			{
				PartyVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return false;
				}
				PartyScreenLogic partyScreenLogic = dataSource.PartyScreenLogic;
				bool? flag = ((partyScreenLogic != null) ? new bool?(partyScreenLogic.IsTroopUpgradesDisabled) : null);
				bool flag2 = true;
				return (flag.GetValueOrDefault() == flag2) & (flag != null);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00006914 File Offset: 0x00004B14
		public GauntletPartyScreen(PartyState partyState)
		{
			partyState.Handler = this;
			this._partyState = partyState;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000692A File Offset: 0x00004B2A
		protected override void OnInitialize()
		{
			base.OnInitialize();
			InformationManager.HideAllMessages();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00006938 File Offset: 0x00004B38
		protected override void OnReady()
		{
			base.OnReady();
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
				ScreenManager.TrySetFocus(this._gauntletLayer);
				PartyVM dataSource = this._dataSource;
				if (((dataSource != null) ? dataSource.SelectedCharacter : null) != null)
				{
					this._dataSource.SelectedCharacter.IsTableauEnabled = true;
				}
				return;
			}
			this._partyscreenCategory = UIResourceManager.LoadSpriteCategory("ui_partyscreen");
			this._gauntletLayer = new GauntletLayer("PartyScreen", 1, true);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("PartyHotKeyCategory"));
			this._dataSource = new PartyVM(this._partyState.PartyScreenLogic);
			this._dataSource.SetGetKeyTextFromKeyIDFunc(new Func<string, TextObject>(Game.Current.GameTextManager.GetHotKeyGameTextFromKeyID));
			this._dataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._dataSource.SetTakeAllTroopsInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("TakeAllTroops"));
			this._dataSource.SetDismissAllTroopsInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("GiveAllTroops"));
			this._dataSource.SetTakeAllPrisonersInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("TakeAllPrisoners"));
			this._dataSource.SetDismissAllPrisonersInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("GiveAllPrisoners"));
			this._dataSource.SetOpenUpgradePanelInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("OpenUpgradePopup"));
			this._dataSource.SetOpenRecruitPanelInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("OpenRecruitPopup"));
			this._dataSource.UpgradePopUp.SetPrimaryActionInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("PopupItemPrimaryAction"));
			this._dataSource.UpgradePopUp.SetSecondaryActionInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("PopupItemSecondaryAction"));
			this._dataSource.UpgradePopUp.SetTertiaryActionInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("GiveAllTroops"));
			this._dataSource.RecruitPopUp.SetPrimaryActionInputKey(HotKeyManager.GetCategory("PartyHotKeyCategory").GetHotKey("PopupItemSecondaryAction"));
			this._gauntletLayer.LoadMovie("PartyScreen", this._dataSource);
			base.AddLayer(this._gauntletLayer);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._gauntletLayer);
			ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			PartyVM dataSource2 = this._dataSource;
			if (((dataSource2 != null) ? dataSource2.SelectedCharacter : null) != null)
			{
				this._dataSource.SelectedCharacter.IsTableauEnabled = true;
			}
			this._gauntletLayer.GamepadNavigationContext.GainNavigationAfterFrames(2, null);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00006C6C File Offset: 0x00004E6C
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
			this._dataSource.IsFiveStackModifierActive = this._gauntletLayer.Input.IsHotKeyDown("FiveStackModifier");
			this._dataSource.IsEntireStackModifierActive = this._gauntletLayer.Input.IsHotKeyDown("EntireStackModifier");
			if (!this._partyState.IsActive || this._gauntletLayer.Input.IsHotKeyReleased("Exit") || (!this._gauntletLayer.Input.IsControlDown() && this._gauntletLayer.Input.IsGameKeyReleased(43)))
			{
				this.HandleCancelInput();
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
			{
				this.HandleDoneInput();
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Reset"))
			{
				this.HandleResetInput();
				return;
			}
			if (!this._dataSource.IsAnyPopUpOpen)
			{
				if (this._gauntletLayer.Input.IsHotKeyPressed("TakeAllTroops"))
				{
					if (this._dataSource.IsOtherTroopsHaveTransferableTroops)
					{
						UISoundsHelper.PlayUISound("event:/ui/inventory/take_all");
						this._dataSource.ExecuteTransferAllOtherTroops();
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("GiveAllTroops"))
				{
					if (this._dataSource.IsMainTroopsHaveTransferableTroops)
					{
						UISoundsHelper.PlayUISound("event:/ui/inventory/take_all");
						this._dataSource.ExecuteTransferAllMainTroops();
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("TakeAllPrisoners"))
				{
					if (this._dataSource.CurrentFocusedCharacter != null && Input.IsGamepadActive)
					{
						if (this._dataSource.CurrentFocusedCharacter.IsTroopTransferrable && this._dataSource.CurrentFocusedCharacter.Side == PartyScreenLogic.PartyRosterSide.Left)
						{
							this._dataSource.CurrentFocusedCharacter.ExecuteTransferSingle();
							UISoundsHelper.PlayUISound("event:/ui/transfer");
							return;
						}
					}
					else if (this._dataSource.IsOtherPrisonersHaveTransferableTroops)
					{
						UISoundsHelper.PlayUISound("event:/ui/inventory/take_all");
						this._dataSource.ExecuteTransferAllOtherPrisoners();
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("GiveAllPrisoners"))
				{
					if (this._dataSource.CurrentFocusedCharacter != null && Input.IsGamepadActive)
					{
						if (this._dataSource.CurrentFocusedCharacter.IsTroopTransferrable && this._dataSource.CurrentFocusedCharacter.Side == PartyScreenLogic.PartyRosterSide.Right)
						{
							this._dataSource.CurrentFocusedCharacter.ExecuteTransferSingle();
							UISoundsHelper.PlayUISound("event:/ui/transfer");
							return;
						}
					}
					else if (this._dataSource.IsMainPrisonersHaveTransferableTroops)
					{
						UISoundsHelper.PlayUISound("event:/ui/inventory/take_all");
						this._dataSource.ExecuteTransferAllMainPrisoners();
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("OpenUpgradePopup"))
				{
					if (!this._dataSource.IsUpgradePopUpDisabled)
					{
						this._dataSource.ExecuteOpenUpgradePopUp();
						UISoundsHelper.PlayUISound("event:/ui/default");
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyPressed("OpenRecruitPopup"))
				{
					if (!this._dataSource.IsRecruitPopUpDisabled)
					{
						this._dataSource.ExecuteOpenRecruitPopUp();
						UISoundsHelper.PlayUISound("event:/ui/default");
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsGameKeyReleased(39) && this._dataSource.CurrentFocusedCharacter != null && Input.IsGamepadActive)
				{
					this._dataSource.CurrentFocusedCharacter.ExecuteOpenTroopEncyclopedia();
					return;
				}
			}
			else if (Input.IsGamepadActive)
			{
				if (this._gauntletLayer.Input.IsHotKeyPressed("PopupItemPrimaryAction"))
				{
					if (this._dataSource.UpgradePopUp.IsOpen && this._dataSource.UpgradePopUp.IsPrimaryActionAvailable)
					{
						UISoundsHelper.PlayUISound("event:/ui/party/upgrade");
						this._dataSource.UpgradePopUp.ExecuteItemPrimaryAction();
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyReleased("PopupItemSecondaryAction"))
				{
					if (this._dataSource.UpgradePopUp.IsOpen)
					{
						if (this._dataSource.UpgradePopUp.IsSecondaryActionAvailable)
						{
							UISoundsHelper.PlayUISound("event:/ui/party/upgrade");
							this._dataSource.UpgradePopUp.ExecuteItemSecondaryAction();
							return;
						}
					}
					else if (this._dataSource.RecruitPopUp.IsOpen)
					{
						PartyTroopManagerItemVM focusedTroop = this._dataSource.RecruitPopUp.FocusedTroop;
						if (focusedTroop != null && focusedTroop.PartyCharacter.IsTroopRecruitable)
						{
							UISoundsHelper.PlayUISound("event:/ui/party/recruit_prisoner");
							this._dataSource.RecruitPopUp.ExecuteItemPrimaryAction();
							return;
						}
					}
				}
				else if (this._gauntletLayer.Input.IsHotKeyReleased("GiveAllTroops"))
				{
					if (this._dataSource.UpgradePopUp.IsOpen && this._dataSource.UpgradePopUp.IsTertiaryActionAvailable)
					{
						UISoundsHelper.PlayUISound("event:/ui/party/upgrade");
						this._dataSource.UpgradePopUp.ExecuteItemTertiaryAction();
						return;
					}
				}
				else if (this._gauntletLayer.Input.IsGameKeyReleased(39))
				{
					if (this._dataSource.RecruitPopUp.IsOpen && this._dataSource.RecruitPopUp.FocusedTroop != null)
					{
						this._dataSource.RecruitPopUp.FocusedTroop.PartyCharacter.ExecuteOpenTroopEncyclopedia();
						return;
					}
					if (this._dataSource.UpgradePopUp.IsOpen)
					{
						if (this._dataSource.UpgradePopUp.FocusedTroop != null)
						{
							this._dataSource.UpgradePopUp.FocusedTroop.ExecuteOpenTroopEncyclopedia();
							return;
						}
						if (this._dataSource.CurrentFocusedUpgrade != null)
						{
							this._dataSource.CurrentFocusedUpgrade.ExecuteUpgradeEncyclopediaLink();
						}
					}
				}
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000071E9 File Offset: 0x000053E9
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.PartyScreen));
			UISoundsHelper.PlayUISound("event:/ui/panels/panel_party_open");
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00007210 File Offset: 0x00005410
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
			PartyBase.MainParty.SetVisualAsDirty();
			PartyVM dataSource = this._dataSource;
			if (((dataSource != null) ? dataSource.SelectedCharacter : null) != null)
			{
				this._dataSource.SelectedCharacter.IsTableauEnabled = false;
			}
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
				this._gauntletLayer.IsFocusLayer = false;
				this._gauntletLayer.InputRestrictions.ResetInputRestrictions();
				base.RemoveLayer(this._gauntletLayer);
				ScreenManager.TryLoseFocus(this._gauntletLayer);
				this._gauntletLayer = null;
			}
			Game.Current.EventManager.TriggerEvent<TutorialContextChangedEvent>(new TutorialContextChangedEvent(TutorialContexts.None));
			if (Campaign.Current.ConversationManager.IsConversationInProgress && !Campaign.Current.ConversationManager.IsConversationFlowActive)
			{
				Campaign.Current.ConversationManager.OnConversationActivate();
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000072E6 File Offset: 0x000054E6
		void IGameStateListener.OnInitialize()
		{
			CampaignEvents.CompanionRemoved.AddNonSerializedListener(this, new Action<Hero, RemoveCompanionAction.RemoveCompanionDetail>(this.OnCompanionRemoved));
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007300 File Offset: 0x00005500
		void IGameStateListener.OnFinalize()
		{
			CampaignEvents.CompanionRemoved.ClearListeners(this);
			if (this._dataSource != null)
			{
				this._dataSource.OnFinalize();
				this._dataSource = null;
			}
			if (this._partyscreenCategory != null)
			{
				this._partyscreenCategory.Unload();
				this._partyscreenCategory = null;
			}
			this._gauntletLayer = null;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00007353 File Offset: 0x00005553
		void IPartyScreenPrisonHandler.ExecuteTakeAllPrisonersScript()
		{
			this._dataSource.ExecuteTransferAllOtherPrisoners();
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007360 File Offset: 0x00005560
		void IPartyScreenPrisonHandler.ExecuteDoneScript()
		{
			this._dataSource.ExecuteDone();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000736D File Offset: 0x0000556D
		void IPartyScreenPrisonHandler.ExecuteResetScript()
		{
			this._dataSource.ExecuteReset();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000737A File Offset: 0x0000557A
		void IPartyScreenPrisonHandler.ExecuteSellAllPrisoners()
		{
			this._dataSource.ExecuteTransferAllMainPrisoners();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00007387 File Offset: 0x00005587
		void IPartyScreenTroopHandler.PartyTroopTransfer()
		{
			this._dataSource.ExecuteTransferAllMainTroops();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00007394 File Offset: 0x00005594
		protected override void OnResume()
		{
			base.OnResume();
			PartyVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.IsInConversation)
			{
				this._dataSource.IsInConversation = false;
				if (this._dataSource.PartyScreenLogic.IsDoneActive())
				{
					this._dataSource.PartyScreenLogic.DoneLogic(false);
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000073EB File Offset: 0x000055EB
		public void RequestUserInput(string text, Action accept, Action cancel)
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000073ED File Offset: 0x000055ED
		private void HandleResetInput()
		{
			if (!this._dataSource.IsAnyPopUpOpen)
			{
				this._dataSource.ExecuteReset();
				UISoundsHelper.PlayUISound("event:/ui/default");
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00007414 File Offset: 0x00005614
		private void HandleCancelInput()
		{
			if (this._dataSource.UpgradePopUp.IsOpen)
			{
				this._dataSource.UpgradePopUp.ExecuteCancel();
			}
			else if (this._dataSource.RecruitPopUp.IsOpen)
			{
				this._dataSource.RecruitPopUp.ExecuteCancel();
			}
			else
			{
				this._dataSource.ExecuteCancel(true);
			}
			UISoundsHelper.PlayUISound("event:/ui/default");
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000747F File Offset: 0x0000567F
		void IPartyScreenTroopHandler.ExecuteDoneScript()
		{
			this._dataSource.ExecuteDone();
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000748C File Offset: 0x0000568C
		private void HandleDoneInput()
		{
			if (this._dataSource.UpgradePopUp.IsOpen)
			{
				this._dataSource.UpgradePopUp.ExecuteDone();
			}
			else if (this._dataSource.RecruitPopUp.IsOpen)
			{
				this._dataSource.RecruitPopUp.ExecuteDone();
			}
			else
			{
				this._dataSource.ExecuteDone();
			}
			UISoundsHelper.PlayUISound("event:/ui/default");
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000074F6 File Offset: 0x000056F6
		private void OnCompanionRemoved(Hero arg1, RemoveCompanionAction.RemoveCompanionDetail arg2)
		{
			((IChangeableScreen)this).ApplyChanges();
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000074FE File Offset: 0x000056FE
		bool IChangeableScreen.AnyUnsavedChanges()
		{
			return this._partyState.PartyScreenLogic.IsThereAnyChanges();
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00007510 File Offset: 0x00005710
		bool IChangeableScreen.CanChangesBeApplied()
		{
			return this._partyState.PartyScreenLogic.IsDoneActive();
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00007522 File Offset: 0x00005722
		void IChangeableScreen.ApplyChanges()
		{
			this._partyState.PartyScreenLogic.DoneLogic(true);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00007536 File Offset: 0x00005736
		void IChangeableScreen.ResetChanges()
		{
			this._partyState.PartyScreenLogic.Reset(true);
		}

		// Token: 0x0400004B RID: 75
		private PartyVM _dataSource;

		// Token: 0x0400004C RID: 76
		private GauntletLayer _gauntletLayer;

		// Token: 0x0400004D RID: 77
		private SpriteCategory _partyscreenCategory;

		// Token: 0x0400004E RID: 78
		private readonly PartyState _partyState;
	}
}
