using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003F0 RID: 1008
	public class BattleWreckageCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x000FC24D File Offset: 0x000FA44D
		private bool HasOverriddenConsequence
		{
			get
			{
				return !string.IsNullOrEmpty(this._currentOverriddenConsequenceId);
			}
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x000FC260 File Offset: 0x000FA460
		public BattleWreckageCampaignBehavior()
		{
			this.AddWreckageConsequences();
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x000FC2B0 File Offset: 0x000FA4B0
		public override void RegisterEvents()
		{
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnMapEventEnded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.AiHourlyTickEvent.AddNonSerializedListener(this, new Action<MobileParty, PartyThinkParams>(this.OnAiHourlyTickEvent));
			CampaignEvents.OnPartyEncounterEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnPartyEncounter));
			CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, new Action<PartyBase, Hero>(this.OnHeroPrisonerTakenEvent));
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x000FC348 File Offset: 0x000FA548
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_playerInvestigationStartTime", ref this._playerInvestigationStartTime);
			dataStore.SyncData<CampaignTime>("_requiredInvestigationDuration", ref this._requiredInvestigationDuration);
			dataStore.SyncData<List<TextObject>>("_consequenceExplanations", ref this._consequenceExplanations);
			dataStore.SyncData<int>("_lootedGoldAmount", ref this._lootedGoldAmount);
			dataStore.SyncData<BattleWreckage>("_encounteredBattleWreckage", ref this._encounteredBattleWreckage);
			dataStore.SyncData<ItemRoster>("_lootedItems", ref this._lootedItems);
			dataStore.SyncData<TroopRoster>("_lootedTroops", ref this._lootedTroops);
			dataStore.SyncData<string>("_currentOverriddenConsequenceId", ref this._currentOverriddenConsequenceId);
			dataStore.SyncData<bool>("_isOverriddenConsequenceAccepted", ref this._isOverriddenConsequenceAccepted);
			dataStore.SyncData<int>("_recoverTroopCountsToAdd", ref this._recoverTroopCountsToAdd);
			dataStore.SyncData<CampaignTime>("_lastPlayerWreckageInteractionTime", ref this._lastPlayerWreckageInteractionTime);
			dataStore.SyncData<bool>("_isEncounteredWreckageInterrupted", ref this._isEncounteredWreckageInterrupted);
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x000FC42D File Offset: 0x000FA62D
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x000FC438 File Offset: 0x000FA638
		public void SetCurrentEncounteredBattleWreckage(BattleWreckage battleWreckage)
		{
			MobileParty.MainParty.SetMoveModeHold();
			this._isEncounteredWreckageInterrupted = this._encounteredBattleWreckage == battleWreckage;
			this._encounteredBattleWreckage = battleWreckage;
			this._playerInvestigationStartTime = CampaignTime.Now;
			if (!this._isEncounteredWreckageInterrupted)
			{
				this._requiredInvestigationDuration = CampaignTime.Hours(this.GetWreckageInvestigationDuration(this._isEncounteredWreckageInterrupted).ResultNumber);
			}
			TextObject textObject;
			if (!Campaign.Current.Models.BattleWreckageModel.CanPlayerInteractWithWreckage(out textObject))
			{
				GameMenu.ActivateGameMenu("battle_wreckage_encounter_blocking");
				return;
			}
			if (battleWreckage.IsInvestigated && battleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic)
			{
				GameMenu.ActivateGameMenu("battle_wreckage_remains");
				return;
			}
			GameMenu.ActivateGameMenu("battle_wreckage");
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x000FC4E0 File Offset: 0x000FA6E0
		private void OnHeroPrisonerTakenEvent(PartyBase capturerParty, Hero hero)
		{
			if (hero == Hero.MainHero && this._encounteredBattleWreckage != null)
			{
				this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.WreckageExpired);
			}
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x000FC4FC File Offset: 0x000FA6FC
		private void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("battle_wreckage", "{=!}{WRECKAGE_INIT_TEXT}", new OnInitDelegate(this.game_menu_battle_wreckage_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage", "investigate", "{=!}{WRECKAGE_INVESTIGATE_OPTION_TEXT}", new GameMenuOption.OnConditionDelegate(this.game_menu_investigate_wreckage_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_investigate_wreckage_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.game_menu_wreckage_init_leave_option_condition), new GameMenuOption.OnConsequenceDelegate(this.battle_wreckage_leave_without_start_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("battle_wreckage_results_menu", "{=!}{WRECKAGE_RESULTS_TEXT}", new OnInitDelegate(this.wreckage_results_menu_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage_results_menu", "leave", "{=DM6luo3c}Continue", new GameMenuOption.OnConditionDelegate(this.wreckage_results_menu_continue_option_condition), new GameMenuOption.OnConsequenceDelegate(this.battle_wreckage_results_leave_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("battle_wreckage_results_connection_menu", "{=!}Player should not see this text", new OnInitDelegate(this.wreckage_results_connection_menu), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddWaitGameMenu("battle_wreckage_investigate_wait_menu", "{=!}{INVESTIGATION_WAIT_MENU_TEXT}", new OnInitDelegate(this.game_menu_battle_wreckage_investigate_init), null, new OnConsequenceDelegate(this.game_menu_battle_wreckage_investigate_consequence), new OnTickDelegate(this.game_menu_battle_wreckage_investigate_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameMenu.MenuOverlayType.None, 0f, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage_investigate_wait_menu", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.investigation_menu_leave_option_condition), new GameMenuOption.OnConsequenceDelegate(this.battle_wreckage_investigate_leave_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("battle_wreckage_recover_troops_decision", "{=!}{RECOVER_TROOPS_TEXT}", new OnInitDelegate(this.game_menu_recover_troops_decision_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage_recover_troops_decision", "recover", "{=skLedR3E}Help the wounded", new GameMenuOption.OnConditionDelegate(this.recover_troops_accept_option_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_recover_troops_accept_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage_recover_troops_decision", "leave", "{=rClFoou7}Leave them", new GameMenuOption.OnConditionDelegate(this.recover_troops_leave_option_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_recover_troops_decline_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenu("battle_wreckage_remains", "{=!}{BATTLE_REMAINS_TEXT}", new OnInitDelegate(this.game_menu_battle_remains_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage_remains", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.battle_remains_leave_condition), new GameMenuOption.OnConsequenceDelegate(this.battle_remains_leave_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("battle_wreckage_encounter_blocking", "{=!}{BATTLE_WRECKAGE_BLOCKED_TEXT}", new OnInitDelegate(this.wreckage_encounter_blocked_menu_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("battle_wreckage_encounter_blocking", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.wreckage_encounter_blocked_leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.wreckage_encounter_blocked_consequence), true, -1, false, null);
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x000FC78C File Offset: 0x000FA98C
		private void AddWreckageConsequences()
		{
			this._wreckageConsequences.Add(new BattleWreckageCampaignBehavior.BattleWreckageConsequence("RecoverTroopsConsequence", new Func<BattleWreckage, bool>(this.RecoverTroopsCondition), new Action(this.RecoverTroopsConsequence), 0.6f, true));
			this._wreckageConsequences.Add(new BattleWreckageCampaignBehavior.BattleWreckageConsequence("GainTradeGoodsConsequence", null, new Action(this.GainTradeGoodsConsequence), 0.6f, false));
			this._wreckageConsequences.Add(new BattleWreckageCampaignBehavior.BattleWreckageConsequence("GainGoldConsequence", null, new Action(this.GainGoldConsequence), 0.5f, false));
			this._wreckageConsequences.Add(new BattleWreckageCampaignBehavior.BattleWreckageConsequence("NothingFoundConsequence", null, new Action(this.NothingFoundConsequence), -1f, false));
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x000FC844 File Offset: 0x000FAA44
		private void wreckage_results_menu_init(MenuCallbackArgs args)
		{
			TextObject textObject;
			if (this._encounteredBattleWreckage.Position.IsOnLand)
			{
				if (MobileParty.MainParty.MemberRoster.TotalManCount > 1)
				{
					textObject = new TextObject("{=qYbSovv1}Your men finished searching the battleground. They found {LISTED_ELEMENTS} among the fallen.", null);
				}
				else
				{
					textObject = new TextObject("{=mDhGN2y1}You finished searching the battleground. You found {LISTED_ELEMENTS} among the fallen.", null);
				}
			}
			else if (MobileParty.MainParty.MemberRoster.TotalManCount > 1)
			{
				textObject = new TextObject("{=aUGSXFHX}Your men finished searching the wreckage. They found {LISTED_ELEMENTS} among the debris.", null);
			}
			else
			{
				textObject = new TextObject("{=uBM2UY8b}You finished searching the wreckage. They found {LISTED_ELEMENTS} among the debris.", null);
			}
			TextObject textObject2 = GameTexts.GameTextHelper.MergeTextObjectsWithComma(this._consequenceExplanations, this._consequenceExplanations.Count > 1);
			textObject.SetTextVariable("LISTED_ELEMENTS", textObject2);
			GameTexts.SetVariable("WRECKAGE_RESULTS_TEXT", textObject);
			if (this._selectedConsequences.AnyQ<BattleWreckageCampaignBehavior.BattleWreckageConsequence>((BattleWreckageCampaignBehavior.BattleWreckageConsequence x) => x.StringId != "NothingFoundConsequence"))
			{
				args.MenuContext.SetPanelSound("event:/ui/wreckage/generic_recover");
			}
			this.SetWreckageMenuBackgrounds(args);
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x000FC932 File Offset: 0x000FAB32
		private bool wreckage_results_menu_continue_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x000FC93D File Offset: 0x000FAB3D
		private void battle_wreckage_results_leave_consequence(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.InvestigationCompleted);
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x000FC950 File Offset: 0x000FAB50
		private void game_menu_battle_wreckage_init(MenuCallbackArgs args)
		{
			if (this._encounteredBattleWreckage.IsInvestigated)
			{
				Debug.FailedAssert("Investigated wreckages should be routed to battle remains menu, not here. Check SetCurrentEncounteredBattleWreckage routing.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\BattleWreckageCampaignBehavior.cs", "game_menu_battle_wreckage_init", 319);
				return;
			}
			bool isEncounteredWreckageInterrupted = this._isEncounteredWreckageInterrupted;
			bool flag = !string.IsNullOrEmpty(this._currentOverriddenConsequenceId);
			ExplainedNumber wreckageInvestigationDuration = this.GetWreckageInvestigationDuration(isEncounteredWreckageInterrupted);
			TextObject textObject;
			TextObject textObject2;
			TextObject textObject3;
			this.GetWreckageMenuText(isEncounteredWreckageInterrupted, flag, wreckageInvestigationDuration, out textObject, out textObject2, out textObject3);
			GameTexts.SetVariable("WRECKAGE_INIT_TEXT", textObject);
			GameTexts.SetVariable("WRECKAGE_INVESTIGATE_OPTION_TEXT", textObject2);
			GameTexts.SetVariable("WRECKAGE_INVESTIGATE_OPTION_TOOLTIP_TEXT", textObject3);
			args.MenuContext.SetPanelSound("event:/ui/wreckage/wreckage_panel");
			this.SetWreckageMenuBackgrounds(args);
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x000FC9EC File Offset: 0x000FABEC
		private void GetWreckageMenuText(bool isInterruptedWreckage, bool isInterruptedDuringRecovery, ExplainedNumber investigationDurationWithExplanations, out TextObject menuText, out TextObject investigateOptionText, out TextObject investigationDurationTooltip)
		{
			float resultNumber = investigationDurationWithExplanations.ResultNumber;
			TextObject textObject = TextObject.GetEmpty();
			TextObject textObject2;
			if (isInterruptedWreckage)
			{
				if (isInterruptedDuringRecovery)
				{
					investigateOptionText = new TextObject("{=npdmTVcg}Continue the rescue.", null);
					if (this._encounteredBattleWreckage.Position.IsOnLand)
					{
						menuText = new TextObject("{=6fgFwhVd}You return to the battlefield that you had been searching. You can continue to tend the wounded you had been treating.", null);
					}
					else
					{
						menuText = new TextObject("{=Mbbm6eGd}You return to the wreckage that you had been searching. You can continue to tend the wounded you had been treating.", null);
					}
				}
				else if (this._encounteredBattleWreckage.Position.IsOnLand)
				{
					menuText = new TextObject("{=4LiDro4x}You return to the battlefield that you had been searching.", null);
					investigateOptionText = new TextObject("{=XOgUKPHA}Continue investigating the battlefield.", null);
				}
				else
				{
					menuText = new TextObject("{=RBjAz2Gf}You return to the wreckage that you had been searching.", null);
					investigateOptionText = new TextObject("{=Aau831CL}Continue investigating the wreckage.", null);
				}
				textObject2 = TextObject.GetEmpty();
			}
			else
			{
				if (this._encounteredBattleWreckage.Position.IsOnLand)
				{
					investigateOptionText = new TextObject("{=qNdiPWQO}Investigate the battlefield.", null);
					if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic)
					{
						menuText = new TextObject("{=xGf9VgY9}Carrion-birds wheel overhead, and the smell of death is in the air. You have come across the remnants of a major battle.", null);
					}
					else if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Normal)
					{
						menuText = new TextObject("{=AInsSCZ7}Carrion-birds wheel overhead, and the smell of death is in the air. You have come across the remnants of a battle.", null);
					}
					else
					{
						menuText = new TextObject("{=wALUbsrH}Carrion-birds wheel overhead, and the smell of death is in the air. You have come across the remnants of a skirmish.", null);
					}
				}
				else
				{
					investigateOptionText = new TextObject("{=sSCh8BIb}Investigate the wreckage.", null);
					if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic)
					{
						menuText = new TextObject("{=WRMZFxxK}Your look-outs spot gulls wheeling overhead, and broken timbers in the water. You have come across the remnants of a major battle at sea.", null);
					}
					else if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Normal)
					{
						menuText = new TextObject("{=bR39Nv4Y}Your look-outs spot gulls wheeling overhead, and broken timbers in the water. You have come across the remnants of a naval battle.", null);
					}
					else
					{
						menuText = new TextObject("{=E90zM22N}Your look-outs spot gulls wheeling overhead, and broken timbers in the water. You have come across the remnants of a duel between ships.", null);
					}
				}
				textObject2 = new TextObject("{=!}{newline}{LOCAL_REMAINING_HOURS_EXPLANATIONS}", null);
				textObject2.SetTextVariable("LOCAL_REMAINING_HOURS_EXPLANATIONS", investigationDurationWithExplanations.GetExplanations());
				textObject2.SetTextVariable("newline", "\n");
				int requiredPartySizeToInvestigateWreckageEfficiently = this.GetRequiredPartySizeToInvestigateWreckageEfficiently();
				if (MobileParty.MainParty.MemberRoster.TotalManCount < requiredPartySizeToInvestigateWreckageEfficiently && !isInterruptedWreckage)
				{
					textObject = new TextObject("{=MhEbgb8c}{newline}You need at least {REQUIRED_TROOP_COUNT} troops to fully investigate this wreckage. With fewer troops, you will only find minor spoils.", null);
					textObject.SetTextVariable("REQUIRED_TROOP_COUNT", requiredPartySizeToInvestigateWreckageEfficiently);
					textObject.SetTextVariable("newline", "\n");
				}
			}
			investigationDurationTooltip = new TextObject("{=J83ajkAA}Remaining Hours: {REMAINING_HOURS}{REMAINING_HOURS_EXPLANATIONS}{EFFECTIVE_INVESTIGATION_REASON}", null);
			investigationDurationTooltip.SetTextVariable("REMAINING_HOURS", resultNumber, 2);
			investigationDurationTooltip.SetTextVariable("REMAINING_HOURS_EXPLANATIONS", textObject2);
			investigationDurationTooltip.SetTextVariable("EFFECTIVE_INVESTIGATION_REASON", textObject);
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x000FCC20 File Offset: 0x000FAE20
		private void SetWreckageMenuBackgrounds(MenuCallbackArgs args)
		{
			string text = "wreckage_land";
			if (MobileParty.MainParty.IsCurrentlyAtSea)
			{
				text = "wreckage_sea";
			}
			args.MenuContext.SetBackgroundMeshName(text);
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x000FCC51 File Offset: 0x000FAE51
		private bool game_menu_investigate_wreckage_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			args.Tooltip = new TextObject("{=!}{WRECKAGE_INVESTIGATE_OPTION_TOOLTIP_TEXT}", null);
			return true;
		}

		// Token: 0x06003D1B RID: 15643 RVA: 0x000FCC6D File Offset: 0x000FAE6D
		private bool game_menu_wreckage_init_leave_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x000FCC78 File Offset: 0x000FAE78
		private void battle_wreckage_leave_without_start_consequence(MenuCallbackArgs args)
		{
			this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.LeaveWithoutInvestigation);
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x000FCC81 File Offset: 0x000FAE81
		private bool investigation_menu_leave_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003D1E RID: 15646 RVA: 0x000FCC8C File Offset: 0x000FAE8C
		private void battle_wreckage_investigate_leave_consequence(MenuCallbackArgs args)
		{
			this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.AbandonInvestigation);
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x000FCC98 File Offset: 0x000FAE98
		private void LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason reason)
		{
			if (reason == BattleWreckageCampaignBehavior.WreckageLeaveReason.InvestigationCompleted)
			{
				this._encounteredBattleWreckage.OnWreckageInvestigated();
				if (this._encounteredBattleWreckage.WreckageTypeCategory != BattleWreckage.WreckageType.Epic)
				{
					this.RemoveWreckageFromMap(this._encounteredBattleWreckage);
				}
			}
			else if (reason == BattleWreckageCampaignBehavior.WreckageLeaveReason.AbandonInvestigation)
			{
				if (!MobileParty.MainParty.IsDisorganized)
				{
					MobileParty.MainParty.SetDisorganized(true);
				}
			}
			else if (reason == BattleWreckageCampaignBehavior.WreckageLeaveReason.PlayerInterrupted)
			{
				CampaignTime campaignTime = CampaignTime.Now - this._playerInvestigationStartTime;
				float num = MathF.Max(1f, (float)(this._requiredInvestigationDuration.ToHours - campaignTime.ToHours));
				this._requiredInvestigationDuration = CampaignTime.Hours(num);
				this._lastPlayerWreckageInteractionTime = CampaignTime.Now;
			}
			if (PlayerEncounter.Current == null && Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.ExitToLast();
			}
			this._playerInvestigationStartTime = CampaignTime.Zero;
			this._selectedConsequences.Clear();
			this._lootedItems.Clear();
			this._lootedTroops.Clear();
			this._lootedGoldAmount = 0;
			this._consequenceExplanations.Clear();
			if (reason != BattleWreckageCampaignBehavior.WreckageLeaveReason.PlayerInterrupted)
			{
				this._encounteredBattleWreckage = null;
				this._recoverTroopCountsToAdd = 0;
				this._requiredInvestigationDuration = CampaignTime.Zero;
				this._currentOverriddenConsequenceId = string.Empty;
				this._isOverriddenConsequenceAccepted = false;
				this._lastPlayerWreckageInteractionTime = CampaignTime.Zero;
			}
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x000FCDC8 File Offset: 0x000FAFC8
		private void wreckage_results_connection_menu(MenuCallbackArgs args)
		{
			this.ApplyWreckageInvestigationResults();
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x000FCDD0 File Offset: 0x000FAFD0
		private void game_menu_investigate_wreckage_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("battle_wreckage_investigate_wait_menu");
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x000FCDDC File Offset: 0x000FAFDC
		private void game_menu_battle_wreckage_investigate_init(MenuCallbackArgs args)
		{
			TextObject textObject;
			if (!this._isOverriddenConsequenceAccepted)
			{
				float num = (float)((CampaignTime.Now - this._playerInvestigationStartTime).ToHours / this._requiredInvestigationDuration.ToHours);
				args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(MBMath.ClampFloat(num, 0f, 1f));
				if (this._encounteredBattleWreckage.Position.IsOnLand)
				{
					if (MobileParty.MainParty.MemberRoster.TotalManCount > 1)
					{
						textObject = new TextObject("{=V5pVt2tn}Your troops are searching amid the fallen.", null);
					}
					else
					{
						textObject = new TextObject("{=8cTc5fLY}You are searching amid the fallen.", null);
					}
				}
				else if (MobileParty.MainParty.Ships.Count == 1)
				{
					textObject = new TextObject("{=sB9m2SKE}Your ship are searching amid the debris.", null);
				}
				else
				{
					textObject = new TextObject("{=zPf03RS6}Your ships are searching amid the debris.", null);
				}
			}
			else
			{
				float num2 = ((this._playerInvestigationStartTime != CampaignTime.Zero) ? ((float)((CampaignTime.Now - this._playerInvestigationStartTime).ToHours / this._requiredInvestigationDuration.ToHours)) : 0f);
				args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(MBMath.ClampFloat(num2, 0f, 1f));
				if (this._encounteredBattleWreckage.Position.IsOnLand)
				{
					textObject = new TextObject("{=hk5aNGtg}You are treating the wounded troops.", null);
				}
				else
				{
					textObject = new TextObject("{=0MUhpB1E}You are rescuing survivors from the water.", null);
				}
			}
			GameTexts.SetVariable("INVESTIGATION_WAIT_MENU_TEXT", textObject);
			args.MenuContext.SetPanelSound("event:/ui/wreckage/investigate_the_battlefield");
			this.SetWreckageMenuBackgrounds(args);
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x000FCF64 File Offset: 0x000FB164
		private void game_menu_battle_wreckage_investigate_tick(MenuCallbackArgs args, CampaignTime dt)
		{
			float num = MathF.Min((float)((CampaignTime.Now - this._playerInvestigationStartTime).ToHours / this._requiredInvestigationDuration.ToHours), 1f);
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(num);
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x000FCFB2 File Offset: 0x000FB1B2
		private void game_menu_battle_wreckage_investigate_consequence(MenuCallbackArgs args)
		{
			this.SelectWreckageConsequences();
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x000FCFBC File Offset: 0x000FB1BC
		private void wreckage_encounter_blocked_menu_init(MenuCallbackArgs args)
		{
			TextObject textObject;
			Campaign.Current.Models.BattleWreckageModel.CanPlayerInteractWithWreckage(out textObject);
			GameTexts.SetVariable("BATTLE_WRECKAGE_BLOCKED_TEXT", textObject);
			args.MenuContext.SetPanelSound("event:/ui/wreckage/wreckage_panel");
			this.SetWreckageMenuBackgrounds(args);
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x000FD002 File Offset: 0x000FB202
		private bool wreckage_encounter_blocked_leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x000FD00D File Offset: 0x000FB20D
		private void wreckage_encounter_blocked_consequence(MenuCallbackArgs args)
		{
			this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.LeaveWithoutInvestigation);
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x000FD018 File Offset: 0x000FB218
		private void OnMapEventEnded(MapEvent mapEvent)
		{
			BattleWreckage.WreckageType wreckageType;
			MBReadOnlyList<BattleWreckage> mbreadOnlyList;
			if (this.CanMapEventCreateWreckage(mapEvent, out wreckageType, out mbreadOnlyList))
			{
				if (mbreadOnlyList.Count > 0)
				{
					foreach (BattleWreckage battleWreckage in mbreadOnlyList)
					{
						this.RemoveWreckageFromMap(battleWreckage);
					}
				}
				this.SpawnWreckage(mapEvent, wreckageType);
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x000FD084 File Offset: 0x000FB284
		private void SpawnWreckage(MapEvent mapEvent, BattleWreckage.WreckageType newWreckageType)
		{
			CampaignTime wreckageDestroyTimeOnCreation = this.GetWreckageDestroyTimeOnCreation(newWreckageType);
			BattleWreckage.CreateWreckage(mapEvent, newWreckageType, wreckageDestroyTimeOnCreation);
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x000FD0A4 File Offset: 0x000FB2A4
		private bool CanMapEventCreateWreckage(MapEvent mapEvent, out BattleWreckage.WreckageType newWreckageType, out MBReadOnlyList<BattleWreckage> wreckagesToDestroy)
		{
			wreckagesToDestroy = new MBReadOnlyList<BattleWreckage>();
			newWreckageType = BattleWreckage.WreckageType.Invalid;
			if (mapEvent.IsFieldBattle && mapEvent.HasWinner && !mapEvent.IsPlayerMapEvent)
			{
				if (!mapEvent.InvolvedParties.AnyQ<PartyBase>(delegate(PartyBase x)
				{
					MobileParty mobileParty = x.MobileParty;
					return mobileParty != null && mobileParty.IsCurrentlyUsedByAQuest;
				}))
				{
					if (Campaign.Current.Wreckages.CountQ<BattleWreckage>(delegate(BattleWreckage x)
					{
						if (!mapEvent.IsNavalMapEvent)
						{
							return x.Position.IsOnLand;
						}
						return !x.Position.IsOnLand;
					}) >= Campaign.Current.Models.BattleWreckageModel.GetMaxWreckageCountForMapEventType(mapEvent))
					{
						return false;
					}
					int num = mapEvent.AttackerSide.Parties.SumQ<MapEventParty>((MapEventParty x) => x.WoundedInBattle.TotalRegulars + x.DiedInBattle.TotalRegulars) + mapEvent.DefenderSide.Parties.SumQ<MapEventParty>((MapEventParty x) => x.WoundedInBattle.TotalRegulars + x.DiedInBattle.TotalRegulars);
					int wreckageCreationBattleSizeThreshold = Campaign.Current.Models.BattleWreckageModel.GetWreckageCreationBattleSizeThreshold(mapEvent);
					if (num < wreckageCreationBattleSizeThreshold)
					{
						return false;
					}
					newWreckageType = Campaign.Current.Models.BattleWreckageModel.GetWreckageTypeForMapEvent(mapEvent);
					return this.IsMapEventPartiesSuitableToCreateBattleWreckage(mapEvent, newWreckageType) && this.CanBattleWreckageCreateBasedOnPosition(mapEvent, newWreckageType, out wreckagesToDestroy) && (newWreckageType != BattleWreckage.WreckageType.Small || MBRandom.RandomFloat <= 0.85f);
				}
			}
			return false;
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x000FD23C File Offset: 0x000FB43C
		private bool CanBattleWreckageCreateBasedOnPosition(MapEvent mapEvent, BattleWreckage.WreckageType wreckageTypeToCreate, out MBReadOnlyList<BattleWreckage> wreckagesToDestroy)
		{
			wreckagesToDestroy = new MBReadOnlyList<BattleWreckage>();
			CampaignVec2 position = mapEvent.Position;
			int num = (position.IsOnLand ? 20 : 20);
			bool flag = wreckageTypeToCreate == BattleWreckage.WreckageType.Epic;
			MBList<BattleWreckage> mblist = null;
			CampaignVec2 campaignVec = new CampaignVec2(position.ToVec2(), false);
			if (campaignVec.Face.IsValid())
			{
				TerrainType terrainTypeAtPosition = Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(in campaignVec);
				if (terrainTypeAtPosition == TerrainType.River || terrainTypeAtPosition == TerrainType.UnderBridge || terrainTypeAtPosition == TerrainType.NonNavigableRiver)
				{
					return false;
				}
			}
			foreach (BattleWreckage battleWreckage in Campaign.Current.Wreckages)
			{
				if (position.Distance(battleWreckage.Position) < (float)num)
				{
					if (!flag)
					{
						return false;
					}
					if (!this.CanEpicWreckageReplaceNearbyWreckage(battleWreckage, ref mblist))
					{
						return false;
					}
				}
			}
			if (this.IsNearSettlement(position))
			{
				return false;
			}
			if (mblist != null)
			{
				wreckagesToDestroy = mblist;
			}
			return true;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x000FD33C File Offset: 0x000FB53C
		private bool CanEpicWreckageReplaceNearbyWreckage(BattleWreckage nearbyWreckage, ref MBList<BattleWreckage> wreckagesToDestroy)
		{
			BattleWreckage.WreckageType wreckageTypeCategory = nearbyWreckage.WreckageTypeCategory;
			if (wreckageTypeCategory == BattleWreckage.WreckageType.Epic)
			{
				return false;
			}
			if ((wreckageTypeCategory == BattleWreckage.WreckageType.Small || wreckageTypeCategory == BattleWreckage.WreckageType.Normal) && this.IsWreckageDestroyable(nearbyWreckage, true))
			{
				if (wreckagesToDestroy == null)
				{
					wreckagesToDestroy = new MBList<BattleWreckage>();
				}
				wreckagesToDestroy.Add(nearbyWreckage);
				return true;
			}
			return false;
		}

		// Token: 0x06003D2D RID: 15661 RVA: 0x000FD380 File Offset: 0x000FB580
		private bool IsNearSettlement(CampaignVec2 position)
		{
			int num = (position.IsOnLand ? 10 : 20);
			LocatableSearchData<Settlement> locatableSearchData = Settlement.StartFindingLocatablesAroundPosition(position.ToVec2(), (float)num);
			return Settlement.FindNextLocatable(ref locatableSearchData) != null;
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x000FD3B8 File Offset: 0x000FB5B8
		private bool IsMapEventPartiesSuitableToCreateBattleWreckage(MapEvent mapEvent, BattleWreckage.WreckageType wreckageType)
		{
			IEnumerable<PartyBase> involvedParties = mapEvent.InvolvedParties;
			if (wreckageType == BattleWreckage.WreckageType.Small)
			{
				bool flag = false;
				foreach (PartyBase partyBase in involvedParties)
				{
					MobileParty mobileParty = partyBase.MobileParty;
					if (mobileParty.IsVillager || mobileParty.IsCaravan || mobileParty.IsPatrolParty)
					{
						flag = true;
					}
					else if (mobileParty.IsLordParty)
					{
						return false;
					}
				}
				return flag;
			}
			if (wreckageType != BattleWreckage.WreckageType.Normal && wreckageType != BattleWreckage.WreckageType.Epic)
			{
				Debug.FailedAssert("Wreckage type should be defined, check this case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\BattleWreckageCampaignBehavior.cs", "IsMapEventPartiesSuitableToCreateBattleWreckage", 875);
				return false;
			}
			if (mapEvent.IsNavalMapEvent)
			{
				return true;
			}
			using (IEnumerator<PartyBase> enumerator = involvedParties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.MobileParty.IsLordParty)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x000FD4A8 File Offset: 0x000FB6A8
		private CampaignTime GetWreckageDestroyTimeOnCreation(BattleWreckage.WreckageType wreckageType)
		{
			float num = 0f;
			if (wreckageType == BattleWreckage.WreckageType.Small)
			{
				num = MBRandom.RandomFloatRanged(6f, 9f);
			}
			else if (wreckageType == BattleWreckage.WreckageType.Normal)
			{
				num = MBRandom.RandomFloatRanged(12f, 15f);
			}
			else if (wreckageType == BattleWreckage.WreckageType.Epic)
			{
				num = MBRandom.RandomFloatRanged(24f, 30f);
			}
			else
			{
				Debug.FailedAssert("This case should not be possible for the wreckage, check this", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\BattleWreckageCampaignBehavior.cs", "GetWreckageDestroyTimeOnCreation", 904);
			}
			return CampaignTime.DaysFromNow(num);
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x000FD51C File Offset: 0x000FB71C
		private ExplainedNumber GetWreckageInvestigationDuration(bool isInterruptedWreckage)
		{
			ExplainedNumber explainedNumber;
			if (isInterruptedWreckage)
			{
				explainedNumber = new ExplainedNumber((float)((int)this._requiredInvestigationDuration.ToHours), true, new TextObject("{=RJYaI2XT}Remaining Investigation Duration", null));
			}
			else
			{
				BattleWreckage.WreckageType wreckageTypeCategory = this._encounteredBattleWreckage.WreckageTypeCategory;
				int num;
				int num2;
				if (wreckageTypeCategory == BattleWreckage.WreckageType.Epic)
				{
					num = 30;
					num2 = 40;
				}
				else if (wreckageTypeCategory == BattleWreckage.WreckageType.Normal)
				{
					num = 20;
					num2 = 30;
				}
				else
				{
					num = 10;
					num2 = 20;
				}
				int num3 = Hero.MainHero.RandomIntWithSeed((uint)CampaignTime.Now.ToDays, num, num2);
				explainedNumber = new ExplainedNumber((float)num3, true, new TextObject("{=8tQRrAgZ}Base Investigation Duration", null));
				int num4 = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
				if (num4 > 0)
				{
					if (num4 > 150)
					{
						num4 = 150;
					}
					int num5 = MathF.Floor((float)num3 * 0.5f);
					int num6 = MathF.Min(MathF.Ceiling(MBMath.Map((float)num4, 0f, 150f, 0f, (float)num5)), num5);
					if (num6 > 0)
					{
						explainedNumber.Add((float)(-(float)num6), new TextObject("{=7KbpRJ5x}Player Roguery Skill Effect", null), null);
					}
				}
				int requiredPartySizeToInvestigateWreckageEfficiently = this.GetRequiredPartySizeToInvestigateWreckageEfficiently();
				int totalManCount = MobileParty.MainParty.MemberRoster.TotalManCount;
				if (totalManCount >= requiredPartySizeToInvestigateWreckageEfficiently)
				{
					int num7;
					if (wreckageTypeCategory == BattleWreckage.WreckageType.Epic)
					{
						num7 = 180;
					}
					else if (wreckageTypeCategory == BattleWreckage.WreckageType.Normal)
					{
						num7 = 90;
					}
					else
					{
						num7 = 50;
					}
					int num8 = num7 - requiredPartySizeToInvestigateWreckageEfficiently;
					if (num8 > 0)
					{
						int num9 = MathF.Floor((float)num3 * 0.3f);
						int num10 = MathF.Min((totalManCount - requiredPartySizeToInvestigateWreckageEfficiently) * num9 / num8, num9);
						if (num10 > 0)
						{
							explainedNumber.Add((float)(-(float)num10), new TextObject("{=VFWSFv3S}Player Party Size Effect", null), null);
						}
					}
				}
			}
			return explainedNumber;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x000FD6B6 File Offset: 0x000FB8B6
		private int GetRequiredPartySizeToInvestigateWreckageEfficiently()
		{
			if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic)
			{
				return 90;
			}
			if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Normal)
			{
				return 50;
			}
			return 1;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x000FD6DB File Offset: 0x000FB8DB
		private void RemoveWreckageFromMap(BattleWreckage wreckageToRemove)
		{
			wreckageToRemove.DestroyWreckage();
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x000FD6E4 File Offset: 0x000FB8E4
		private void OnHourlyTick()
		{
			for (int i = Campaign.Current.Wreckages.Count - 1; i >= 0; i--)
			{
				BattleWreckage battleWreckage = Campaign.Current.Wreckages[i];
				if (this.IsWreckageDestroyable(battleWreckage, false))
				{
					this.RemoveWreckageFromMap(battleWreckage);
				}
			}
			if (this._encounteredBattleWreckage != null && this._lastPlayerWreckageInteractionTime != CampaignTime.Zero && (CampaignTime.Now - this._lastPlayerWreckageInteractionTime).ToHours > 24.0)
			{
				this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.WreckageExpired);
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x000FD773 File Offset: 0x000FB973
		private bool IsWreckageDestroyable(BattleWreckage battleWreckage, bool isForCreationNewWreckage = false)
		{
			return this._encounteredBattleWreckage != battleWreckage && !battleWreckage.IsVisible && (isForCreationNewWreckage || battleWreckage.IsWreckageDestroyable);
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x000FD793 File Offset: 0x000FB993
		private void OnPartyEncounter(PartyBase attackerParty, PartyBase defenderParty)
		{
			if (defenderParty == PartyBase.MainParty && this._encounteredBattleWreckage != null)
			{
				this.LeaveWreckageEncounter(BattleWreckageCampaignBehavior.WreckageLeaveReason.PlayerInterrupted);
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x000FD7AC File Offset: 0x000FB9AC
		private void OnAiHourlyTickEvent(MobileParty mobileParty, PartyThinkParams partyThinkParams)
		{
			BattleWreckage battleWreckage;
			if (Campaign.Current.Wreckages.Count == 0 || !this.TryGetTargetableWreckageForParty(mobileParty, out battleWreckage))
			{
				return;
			}
			float num = (mobileParty.IsCurrentlyAtSea ? 6f : 3f);
			MobileParty.NavigationType navigationType = (mobileParty.IsCurrentlyAtSea ? MobileParty.NavigationType.Naval : MobileParty.NavigationType.Default);
			CampaignVec2 campaignVec = NavigationHelper.FindPointAroundPosition(battleWreckage.Position, battleWreckage.Position.IsOnLand ? MobileParty.NavigationType.Default : MobileParty.NavigationType.Naval, this.GetWreckageInvestigationRadiusForAiParties(battleWreckage), 0f, true, false);
			AIBehaviorData aibehaviorData = new AIBehaviorData(campaignVec, AiBehavior.GoToPoint, navigationType, false, false, false);
			ValueTuple<AIBehaviorData, float> valueTuple = new ValueTuple<AIBehaviorData, float>(aibehaviorData, num);
			partyThinkParams.AddBehaviorScore(in valueTuple);
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x000FD844 File Offset: 0x000FBA44
		private bool CanAiPartyTargetWreckage(MobileParty mobileParty)
		{
			if (mobileParty.IsBandit && !mobileParty.IsBanditBossParty && mobileParty.CurrentSettlement == null && !mobileParty.IsCurrentlyUsedByAQuest && mobileParty.MapEvent == null)
			{
				Clan actualClan = mobileParty.ActualClan;
				if (actualClan == null || actualClan.IsBanditFaction)
				{
					return mobileParty.IsCurrentlyAtSea || FactionHelper.IsLooterFaction(mobileParty.ActualClan);
				}
			}
			return false;
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x000FD8AC File Offset: 0x000FBAAC
		private float GetWreckageInvestigationRadiusForAiParties(BattleWreckage battleWreckage)
		{
			float getEncounterJoiningRadius = Campaign.Current.Models.EncounterModel.GetEncounterJoiningRadius;
			if (!battleWreckage.Position.IsOnLand)
			{
				return getEncounterJoiningRadius * 0.5f;
			}
			return getEncounterJoiningRadius;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x000FD8E4 File Offset: 0x000FBAE4
		private bool TryGetTargetableWreckageForParty(MobileParty mobileParty, out BattleWreckage targetWreckage)
		{
			targetWreckage = null;
			if (!this.CanAiPartyTargetWreckage(mobileParty))
			{
				return false;
			}
			BattleWreckage battleWreckage = null;
			float num = 20f;
			foreach (BattleWreckage battleWreckage2 in Campaign.Current.Wreckages)
			{
				if (!battleWreckage2.IsInvestigated && battleWreckage2.Position.IsOnLand == mobileParty.Position.IsOnLand)
				{
					float num2 = battleWreckage2.Position.Distance(mobileParty.Position);
					if (num2 < num)
					{
						battleWreckage = battleWreckage2;
						num = num2;
					}
				}
			}
			if (battleWreckage != null)
			{
				int wreckageTargetingAiPartiesLimit = this.GetWreckageTargetingAiPartiesLimit(battleWreckage);
				float wreckageInvestigationRadiusForAiParties = this.GetWreckageInvestigationRadiusForAiParties(battleWreckage);
				LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition(battleWreckage.Position.ToVec2(), wreckageInvestigationRadiusForAiParties);
				MobileParty mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
				int num3 = 0;
				while (mobileParty2 != null)
				{
					if (mobileParty2 != mobileParty && this.CanAiPartyTargetWreckage(mobileParty2))
					{
						num3++;
						if (num3 == wreckageTargetingAiPartiesLimit)
						{
							return false;
						}
					}
					mobileParty2 = MobileParty.FindNextLocatable(ref locatableSearchData);
				}
				targetWreckage = battleWreckage;
				return true;
			}
			return false;
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x000FD9F8 File Offset: 0x000FBBF8
		private int GetWreckageTargetingAiPartiesLimit(BattleWreckage wreckage)
		{
			if (wreckage.Position.IsOnLand)
			{
				if (wreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic)
				{
					return 6;
				}
				if (wreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Normal)
				{
					return 4;
				}
				return 2;
			}
			else
			{
				if (wreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic)
				{
					return 4;
				}
				if (wreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Normal)
				{
					return 3;
				}
				return 2;
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x000FDA38 File Offset: 0x000FBC38
		private void SelectWreckageConsequences()
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(this._currentOverriddenConsequenceId))
			{
				flag = true;
				if (this._isOverriddenConsequenceAccepted)
				{
					this._selectedConsequences.Add(this.FindConsequenceWithId(this._currentOverriddenConsequenceId));
				}
			}
			this._wreckageConsequences.Shuffle<BattleWreckageCampaignBehavior.BattleWreckageConsequence>();
			foreach (BattleWreckageCampaignBehavior.BattleWreckageConsequence battleWreckageConsequence in this._wreckageConsequences)
			{
				if (battleWreckageConsequence.Chance > 0f && (!battleWreckageConsequence.CanOverride || !flag) && (battleWreckageConsequence.Condition == null || battleWreckageConsequence.Condition(this._encounteredBattleWreckage)) && MBRandom.RandomFloat < battleWreckageConsequence.Chance)
				{
					if (battleWreckageConsequence.CanOverride)
					{
						this._selectedConsequences.Clear();
						this._currentOverriddenConsequenceId = battleWreckageConsequence.StringId;
						battleWreckageConsequence.Consequence();
						return;
					}
					this._selectedConsequences.Add(battleWreckageConsequence);
				}
			}
			if (this._selectedConsequences.Count == 0)
			{
				this._selectedConsequences.Add(this.FindConsequenceWithId("NothingFoundConsequence"));
			}
			this.InvokeSelectedConsequences();
			GameMenu.SwitchToMenu("battle_wreckage_results_connection_menu");
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x000FDB6C File Offset: 0x000FBD6C
		private void InvokeSelectedConsequences()
		{
			for (int i = 0; i < this._selectedConsequences.Count; i++)
			{
				this._selectedConsequences[i].Consequence();
			}
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x000FDBA8 File Offset: 0x000FBDA8
		private void ApplyWreckageInvestigationResults()
		{
			if (this._lootedTroops.Count > 0)
			{
				PartyScreenHelper.OpenScreenAsReceiveTroops(this._lootedTroops, new TextObject("{=Ee0WbUjr}Recovered Troops", null), delegate(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool isDoneClicked)
				{
					this._lootedTroops.Clear();
				});
				return;
			}
			if (this._lootedItems.Count > 0)
			{
				InventoryScreenHelper.OpenScreenAsReceiveItems(this._lootedItems, new TextObject("{=ObwRgNuJ}Salvaged Goods", null), delegate
				{
					this._lootedItems.Clear();
				});
				return;
			}
			if (this._consequenceExplanations.Count > 0)
			{
				if (this._lootedGoldAmount > 0)
				{
					GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this._lootedGoldAmount, true);
					TextObject textObject = GameTexts.FindText("str_you_received_gold_with_icon", null);
					textObject.SetTextVariable("GOLD_AMOUNT", MathF.Abs(this._lootedGoldAmount));
					InformationManager.DisplayMessage(new InformationMessage(textObject.ToString(), string.Empty));
					this._lootedGoldAmount = 0;
				}
				GameMenu.SwitchToMenu("battle_wreckage_results_menu");
				return;
			}
			Debug.FailedAssert("Explanation.Count == 0 case should not be possible, check this case", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\BattleWreckageCampaignBehavior.cs", "ApplyWreckageInvestigationResults", 1377);
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06003D3E RID: 15678 RVA: 0x000FDCA1 File Offset: 0x000FBEA1
		private int EpicWreckageRecoverTroopsTierLimit
		{
			get
			{
				return Campaign.Current.Models.CharacterStatsModel.MaxCharacterTier;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x000FDCB8 File Offset: 0x000FBEB8
		private bool RecoverTroopsCondition(BattleWreckage wreckage)
		{
			int recoverTroopsTierLimit = this.GetRecoverTroopsTierLimit();
			foreach (TroopRosterElement troopRosterElement in wreckage.GetTotalWoundedInBattle())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.Tier <= recoverTroopsTierLimit)
				{
					return true;
				}
			}
			foreach (TroopRosterElement troopRosterElement2 in wreckage.GetTotalDiedInBattle())
			{
				if (!troopRosterElement2.Character.IsHero && troopRosterElement2.Character.Tier <= recoverTroopsTierLimit)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x000FDD8C File Offset: 0x000FBF8C
		private void RecoverTroopsConsequence()
		{
			if (this._currentOverriddenConsequenceId == "RecoverTroopsConsequence")
			{
				if (!this._isOverriddenConsequenceAccepted)
				{
					this.CalculateAndShowRecoverTroopsDecision();
					return;
				}
				this.ApplyRecoverTroopsReward();
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x000FDDB8 File Offset: 0x000FBFB8
		private void ApplyRecoverTroopsReward()
		{
			List<CharacterObject> list = new List<CharacterObject>();
			int recoverTroopsTierLimit = this.GetRecoverTroopsTierLimit();
			foreach (TroopRosterElement troopRosterElement in this._encounteredBattleWreckage.GetTotalWoundedInBattle())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.Tier <= recoverTroopsTierLimit)
				{
					list.Add(troopRosterElement.Character);
				}
			}
			foreach (TroopRosterElement troopRosterElement2 in this._encounteredBattleWreckage.GetTotalDiedInBattle())
			{
				if (!troopRosterElement2.Character.IsHero && !list.Contains(troopRosterElement2.Character) && troopRosterElement2.Character.Tier <= recoverTroopsTierLimit)
				{
					list.Add(troopRosterElement2.Character);
				}
			}
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			int effectiveTroopCountToRecover = this.GetEffectiveTroopCountToRecover(this._recoverTroopCountsToAdd);
			for (int i = 0; i < effectiveTroopCountToRecover; i++)
			{
				CharacterObject randomElement = list.GetRandomElement<CharacterObject>();
				troopRoster.AddToCounts(randomElement, 1, false, 1, 0, true, -1);
				MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Medicine).AddSkillXp(DefaultSkills.Medicine, (float)(randomElement.Tier * 10));
			}
			this.AddLootedTroop(troopRoster);
			this.AddConsequenceExplanation(new TextObject("{=c7tVGjOl}some wounded troops", null));
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x000FDF34 File Offset: 0x000FC134
		private int GetRecoverTroopsTierLimit()
		{
			BattleWreckage.WreckageType effectiveWreckageTypeForConsequence = this.GetEffectiveWreckageTypeForConsequence();
			if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Epic)
			{
				return this.EpicWreckageRecoverTroopsTierLimit;
			}
			if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Normal)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x000FDF5C File Offset: 0x000FC15C
		private void CalculateAndShowRecoverTroopsDecision()
		{
			BattleWreckage.WreckageType effectiveWreckageTypeForConsequence = this.GetEffectiveWreckageTypeForConsequence();
			int num;
			int num2;
			if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Epic)
			{
				num = 10;
				num2 = 15;
			}
			else if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Normal)
			{
				num = 5;
				num2 = 10;
			}
			else
			{
				num = 2;
				num2 = 5;
			}
			int num3 = MBRandom.RandomInt(num, num2);
			int effectiveTroopCountToRecover = this.GetEffectiveTroopCountToRecover(num3);
			this._recoverTroopCountsToAdd = num3;
			this._requiredInvestigationDuration = CampaignTime.Hours((float)Math.Max(2, MathF.Round((float)effectiveTroopCountToRecover * 1f)));
			GameMenu.SwitchToMenu("battle_wreckage_recover_troops_decision");
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x000FDFD0 File Offset: 0x000FC1D0
		private int GetEffectiveTroopCountToRecover(int totalRecoverableTroopCount)
		{
			int skillValue = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Medicine).GetSkillValue(DefaultSkills.Medicine);
			if (skillValue >= 150)
			{
				return totalRecoverableTroopCount;
			}
			int num = MathF.Round((float)totalRecoverableTroopCount * 0.4f);
			int num2 = MathF.Round(MBMath.Map((float)(skillValue + 1), 0f, 150f, 0f, (float)(totalRecoverableTroopCount - num)));
			return num + num2;
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x000FE034 File Offset: 0x000FC234
		private void game_menu_recover_troops_decision_init(MenuCallbackArgs args)
		{
			int effectiveTroopCountToRecover = this.GetEffectiveTroopCountToRecover(this._recoverTroopCountsToAdd);
			Hero heroWithHighestSkill = MobilePartyHelper.GetHeroWithHighestSkill(MobileParty.MainParty, DefaultSkills.Medicine);
			int skillValue = heroWithHighestSkill.GetSkillValue(DefaultSkills.Medicine);
			TextObject textObject;
			if (effectiveTroopCountToRecover == this._recoverTroopCountsToAdd)
			{
				if (heroWithHighestSkill == Hero.MainHero)
				{
					textObject = new TextObject("{=7vU9GKFS}You found {TOTAL_WOUNDED_COUNT} wounded survivors of the battle. As the party member with the highest skill in medicine, {HIGHEST_MEDICINE_SKILLED_MEMBER} believe that all of them can be saved. Treating them will take {RECOVER_DURATION_HOURS} more hours. Or, you can focus on gathering loot.", null);
				}
				else
				{
					textObject = new TextObject("{=P8FS1iNL}You found {TOTAL_WOUNDED_COUNT} wounded survivors of the battle. As the party member with the highest skill in medicine, {HIGHEST_MEDICINE_SKILLED_MEMBER} believes that all of them can be saved. Treating them will take {RECOVER_DURATION_HOURS} more hours. Or, you can focus on gathering loot.", null);
				}
			}
			else
			{
				if (heroWithHighestSkill == Hero.MainHero)
				{
					textObject = new TextObject("{=CK7Li6bL}You found {TOTAL_WOUNDED_COUNT} wounded survivors of the battle. As the party member with the highest skill in medicine, {HIGHEST_MEDICINE_SKILLED_MEMBER} believe that only {EFFECTIVE_COUNT} can be saved. Treating them will take {RECOVER_DURATION_HOURS} more hours. Or, you can focus on gathering loot.", null);
				}
				else
				{
					textObject = new TextObject("{=dfr903U9}You found {TOTAL_WOUNDED_COUNT} wounded survivors of the battle. As the party member with the highest skill in medicine, {HIGHEST_MEDICINE_SKILLED_MEMBER} believes that only {EFFECTIVE_COUNT} can be saved. Treating them will take {RECOVER_DURATION_HOURS} more hours. Or, you can focus on gathering loot.", null);
				}
				textObject.SetTextVariable("EFFECTIVE_COUNT", effectiveTroopCountToRecover);
			}
			textObject.SetTextVariable("TOTAL_WOUNDED_COUNT", this._recoverTroopCountsToAdd);
			textObject.SetTextVariable("HIGHEST_MEDICINE_SKILLED_MEMBER", (heroWithHighestSkill != null && heroWithHighestSkill != Hero.MainHero) ? heroWithHighestSkill.Name : GameTexts.FindText("str_you", null));
			textObject.SetTextVariable("HIGHEST_MEDICINE_SKILL", skillValue);
			textObject.SetTextVariable("RECOVER_DURATION_HOURS", (int)this._requiredInvestigationDuration.ToHours);
			GameTexts.SetVariable("RECOVER_TROOPS_TEXT", textObject);
			this.SetWreckageMenuBackgrounds(args);
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x000FE139 File Offset: 0x000FC339
		private void game_menu_recover_troops_accept_consequence(MenuCallbackArgs args)
		{
			this._playerInvestigationStartTime = CampaignTime.Now;
			this._isOverriddenConsequenceAccepted = true;
			GameMenu.SwitchToMenu("battle_wreckage_investigate_wait_menu");
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x000FE157 File Offset: 0x000FC357
		private void game_menu_recover_troops_decline_consequence(MenuCallbackArgs args)
		{
			this._isOverriddenConsequenceAccepted = false;
			this._recoverTroopCountsToAdd = 0;
			this.SelectWreckageConsequences();
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x000FE16D File Offset: 0x000FC36D
		private bool recover_troops_leave_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x000FE178 File Offset: 0x000FC378
		private bool recover_troops_accept_option_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Continue;
			return true;
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x000FE184 File Offset: 0x000FC384
		private void GainGoldConsequence()
		{
			BattleWreckage.WreckageType effectiveWreckageTypeForConsequence = this.GetEffectiveWreckageTypeForConsequence();
			int num;
			if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Epic)
			{
				if (this._encounteredBattleWreckage.Position.IsOnLand)
				{
					num = MBRandom.RandomInt(5000, 8000);
				}
				else
				{
					num = MBRandom.RandomInt(7000, 10000);
				}
			}
			else if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Normal)
			{
				if (this._encounteredBattleWreckage.Position.IsOnLand)
				{
					num = MBRandom.RandomInt(1500, 3000);
				}
				else
				{
					num = MBRandom.RandomInt(2000, 4500);
				}
			}
			else if (this._encounteredBattleWreckage.Position.IsOnLand)
			{
				num = MBRandom.RandomInt(500, 1000);
			}
			else
			{
				num = MBRandom.RandomInt(700, 1500);
			}
			num++;
			TextObject textObject = new TextObject("{=!}{GOLD_GAIN}{GOLD_ICON}", null);
			textObject.SetTextVariable("GOLD_GAIN", num);
			textObject.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.AddLootedGold(num);
			this.AddConsequenceExplanation(textObject);
			if (MBRandom.RandomFloat <= 0.5f)
			{
				Hideout hideout = null;
				float num2 = float.MaxValue;
				foreach (Hideout hideout2 in Hideout.All)
				{
					if (this.IsHideoutSuitableToDiscoverFromWreckage(hideout2))
					{
						float num3;
						float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(MobileParty.MainParty, hideout2.Settlement, false, MobileParty.NavigationType.All, out num3);
						if (distance < num2)
						{
							num2 = distance;
							hideout = hideout2;
						}
					}
				}
				if (hideout != null)
				{
					hideout.Settlement.IsVisible = true;
					CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, hideout.Settlement.Party);
					Campaign.Current.VisualTrackerManager.RegisterObject(hideout.Settlement);
					this.AddConsequenceExplanation(new TextObject("{=JOquYB4T}a crude map to a nearby hideout", null));
				}
			}
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x000FE36C File Offset: 0x000FC56C
		private bool IsHideoutSuitableToDiscoverFromWreckage(Hideout hideoutToConsider)
		{
			return hideoutToConsider.IsInfested && !hideoutToConsider.Settlement.IsVisible && hideoutToConsider.Settlement.Parties.Count > 0;
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x000FE398 File Offset: 0x000FC598
		private void GainTradeGoodsConsequence()
		{
			int num;
			int num2;
			this.GetTradeGoodTargetValueAndRogueryXp(out num, out num2);
			int num3 = MathF.Ceiling((float)num2 * 1.2f);
			int num4 = (int)((float)num3 * 0.5f);
			int num5 = num3 - num4;
			num5 = MathF.Round(MBRandom.RandomFloatRanged((float)num5 * 0.5f, (float)num5));
			ItemRoster itemRoster = new ItemRoster();
			Dictionary<ItemObject, int> dictionary = new Dictionary<ItemObject, int>(64);
			Town town = Town.AllTowns.MinBy<Town, float>((Town x) => x.Settlement.Position.Distance(MobileParty.MainParty.Position));
			int num6 = 0;
			this.ProcessCasualtyLootSource(this._encounteredBattleWreckage.GetTotalDiedInBattle(), town, dictionary, itemRoster, num4, ref num6);
			this.ProcessCasualtyLootSource(this._encounteredBattleWreckage.GetTotalWoundedInBattle(), town, dictionary, itemRoster, num4, ref num6);
			List<ItemObject> list = new List<ItemObject>();
			HashSet<ItemObject> hashSet = new HashSet<ItemObject>();
			SmithingModel smithingModel = Campaign.Current.Models.SmithingModel;
			for (CraftingMaterials craftingMaterials = CraftingMaterials.IronOre; craftingMaterials < CraftingMaterials.NumCraftingMats; craftingMaterials++)
			{
				ItemObject craftingMaterialItem = smithingModel.GetCraftingMaterialItem(craftingMaterials);
				if (craftingMaterialItem != null)
				{
					hashSet.Add(craftingMaterialItem);
				}
			}
			foreach (ItemObject itemObject in Items.AllTradeGoods)
			{
				if (!itemObject.NotMerchandise && !itemObject.IsBannerItem && itemObject.IsTradeGood && itemObject.ItemType != ItemObject.ItemTypeEnum.Invalid && !itemObject.IsAnimal && !itemObject.IsMountable && itemObject != DefaultItems.Trash && itemObject.Culture == null && !hashSet.Contains(itemObject))
				{
					list.Add(itemObject);
					this.GetOrCalculateAverageItemValue(itemObject, town, dictionary);
				}
			}
			if (list.Count > 0)
			{
				int num7 = 0;
				int num8 = 0;
				do
				{
					ItemObject randomElement = list.GetRandomElement<ItemObject>();
					int num9 = dictionary[randomElement];
					if (randomElement.Value > 0 && num9 + num7 <= num5)
					{
						itemRoster.AddToCounts(randomElement, 1);
						num7 += num9;
					}
					else
					{
						num8++;
					}
				}
				while (num8 < 5);
			}
			this.AddLootedItem(itemRoster);
			this.AddConsequenceExplanation(new TextObject("{=PsIAb1eF}some valuables", null));
			Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, (float)num);
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x000FE5C0 File Offset: 0x000FC7C0
		private void GetTradeGoodTargetValueAndRogueryXp(out int rogueryXp, out int targetTradeGoodValue)
		{
			BattleWreckage.WreckageType effectiveWreckageTypeForConsequence = this.GetEffectiveWreckageTypeForConsequence();
			bool isOnLand = this._encounteredBattleWreckage.Position.IsOnLand;
			int num;
			int num2;
			if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Epic)
			{
				num = 3;
				num2 = (isOnLand ? 7 : 8);
			}
			else if (effectiveWreckageTypeForConsequence == BattleWreckage.WreckageType.Normal)
			{
				num = 2;
				num2 = (isOnLand ? 3 : 5);
			}
			else
			{
				num = 1;
				num2 = (isOnLand ? 1 : 2);
			}
			rogueryXp = 75 * num * (isOnLand ? 1 : 2);
			targetTradeGoodValue = 1000 * num2;
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x000FE628 File Offset: 0x000FC828
		private void ProcessCasualtyLootSource(MBList<TroopRosterElement> casualties, Town closestTown, Dictionary<ItemObject, int> itemValueCache, ItemRoster lootedItems, int targetTotalCasualtyValue, ref int totalLootedValueFromCasualties)
		{
			BattleRewardModel battleRewardModel = Campaign.Current.Models.BattleRewardModel;
			int num = casualties.Count - 1;
			while (num >= 0 && totalLootedValueFromCasualties < targetTotalCasualtyValue)
			{
				if (MBRandom.RandomFloat <= 0.5f)
				{
					CharacterObject character = casualties[num].Character;
					float expectedLootedItemValueFromCasualty = battleRewardModel.GetExpectedLootedItemValueFromCasualty(Hero.MainHero, character);
					EquipmentElement lootedItemFromTroop = battleRewardModel.GetLootedItemFromTroop(character, expectedLootedItemValueFromCasualty);
					if (lootedItemFromTroop.Item != null && !lootedItemFromTroop.Item.IsAnimal && !lootedItemFromTroop.Item.IsMountable)
					{
						int orCalculateAverageItemValue = this.GetOrCalculateAverageItemValue(lootedItemFromTroop.Item, closestTown, itemValueCache);
						if (totalLootedValueFromCasualties + orCalculateAverageItemValue <= targetTotalCasualtyValue)
						{
							lootedItems.AddToCounts(lootedItemFromTroop.Item, 1);
							totalLootedValueFromCasualties += orCalculateAverageItemValue;
						}
					}
				}
				num--;
			}
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x000FE6F0 File Offset: 0x000FC8F0
		private int GetOrCalculateAverageItemValue(ItemObject item, Town closestTown, Dictionary<ItemObject, int> itemValues)
		{
			int itemPrice;
			if (!itemValues.TryGetValue(item, out itemPrice))
			{
				itemPrice = closestTown.GetItemPrice(item, MobileParty.MainParty, true);
				itemValues.Add(item, itemPrice);
			}
			return itemPrice;
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x000FE71F File Offset: 0x000FC91F
		private void NothingFoundConsequence()
		{
			this.AddConsequenceExplanation(new TextObject("{=raa8Qxeh}nothing of value", null));
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x000FE734 File Offset: 0x000FC934
		private BattleWreckage.WreckageType GetEffectiveWreckageTypeForConsequence()
		{
			int requiredPartySizeToInvestigateWreckageEfficiently = this.GetRequiredPartySizeToInvestigateWreckageEfficiently();
			if (MobileParty.MainParty.MemberRoster.TotalManCount >= requiredPartySizeToInvestigateWreckageEfficiently)
			{
				return this._encounteredBattleWreckage.WreckageTypeCategory;
			}
			if (this._encounteredBattleWreckage.WreckageTypeCategory == BattleWreckage.WreckageType.Epic && MobileParty.MainParty.MemberRoster.TotalManCount >= 50)
			{
				return BattleWreckage.WreckageType.Normal;
			}
			return BattleWreckage.WreckageType.Small;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x000FE78C File Offset: 0x000FC98C
		private BattleWreckageCampaignBehavior.BattleWreckageConsequence FindConsequenceWithId(string id)
		{
			return this._wreckageConsequences.FirstOrDefaultQ<BattleWreckageCampaignBehavior.BattleWreckageConsequence>((BattleWreckageCampaignBehavior.BattleWreckageConsequence x) => x.StringId == id);
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x000FE7BD File Offset: 0x000FC9BD
		private void AddLootedItem(ItemRoster lootedItems)
		{
			this._lootedItems.Add(lootedItems);
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x000FE7CB File Offset: 0x000FC9CB
		private void AddLootedTroop(TroopRoster troops)
		{
			this._lootedTroops.Add(troops);
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x000FE7D9 File Offset: 0x000FC9D9
		private void AddLootedGold(int goldAmount)
		{
			this._lootedGoldAmount += goldAmount;
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x000FE7E9 File Offset: 0x000FC9E9
		private void AddConsequenceExplanation(TextObject explanation)
		{
			this._consequenceExplanations.Add(explanation);
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x000FE7F8 File Offset: 0x000FC9F8
		private void game_menu_battle_remains_init(MenuCallbackArgs args)
		{
			BattleWreckage encounteredBattleWreckage = this._encounteredBattleWreckage;
			List<TextObject> list = new List<TextObject>();
			TextObject textObject = new TextObject("{=18KcBMFI}Date: {DATE}", null);
			textObject.SetTextVariable("DATE", encounteredBattleWreckage.BattleStartTime.ToString());
			list.Add(textObject);
			TextObject textObject2 = new TextObject("{=UQGaJM5S}Victor: {PARTY} ({FACTION})", null);
			textObject2.SetTextVariable("PARTY", encounteredBattleWreckage.GetWinnerPartyName());
			textObject2.SetTextVariable("FACTION", encounteredBattleWreckage.GetWinnerFaction().Name);
			list.Add(textObject2);
			TextObject textObject3 = new TextObject("{=pfVrdaAf}Defeated: {PARTY} ({FACTION})", null);
			textObject3.SetTextVariable("PARTY", encounteredBattleWreckage.GetDefeatedPartyName());
			textObject3.SetTextVariable("FACTION", encounteredBattleWreckage.GetDefeatedFaction().Name);
			list.Add(textObject3);
			TextObject textObject4 = new TextObject("{=TZTEWlgx}Battle Size: {COUNT} Troops", null);
			textObject4.SetTextVariable("COUNT", this.GetBattleRemainsWinnerTroopCount(encounteredBattleWreckage) + this.GetBattleRemainsDefeatedTroopCount(encounteredBattleWreckage));
			list.Add(textObject4);
			TextObject textObject5 = new TextObject("{=WSs33gt0}{FACTION} deployed {COUNT} troops", null);
			textObject5.SetTextVariable("FACTION", encounteredBattleWreckage.GetWinnerFaction().Name);
			textObject5.SetTextVariable("COUNT", this.GetBattleRemainsWinnerTroopCount(encounteredBattleWreckage));
			list.Add(textObject5);
			TextObject textObject6 = new TextObject("{=WSs33gt0}{FACTION} deployed {COUNT} troops", null);
			textObject6.SetTextVariable("FACTION", encounteredBattleWreckage.GetDefeatedFaction().Name);
			textObject6.SetTextVariable("COUNT", this.GetBattleRemainsDefeatedTroopCount(encounteredBattleWreckage));
			list.Add(textObject6);
			TextObject textObject7 = new TextObject("{=rjrVuhbW}{newline}Casualties:", null);
			textObject7.SetTextVariable("newline", "\n");
			list.Add(textObject7);
			TextObject textObject8 = new TextObject("{=!}{FACTION}: {COUNT}", null);
			textObject8.SetTextVariable("FACTION", encounteredBattleWreckage.GetWinnerFaction().Name);
			textObject8.SetTextVariable("COUNT", this.GetBattleRemainsWinnerCasualties(encounteredBattleWreckage));
			list.Add(textObject8);
			TextObject textObject9 = new TextObject("{=!}{FACTION}: {COUNT}", null);
			textObject9.SetTextVariable("FACTION", encounteredBattleWreckage.GetDefeatedFaction().Name);
			textObject9.SetTextVariable("COUNT", this.GetBattleRemainsDefeatedCasualties(encounteredBattleWreckage));
			list.Add(textObject9);
			List<TextObject> battleRemainsFallenHeroLines = this.GetBattleRemainsFallenHeroLines(encounteredBattleWreckage);
			if (battleRemainsFallenHeroLines.Count > 0)
			{
				list.Add(new TextObject("{=h70VQ44K}Fallen Heroes:", null));
				list.AddRange(battleRemainsFallenHeroLines);
			}
			TextObject textObject10 = GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(list, new TextObject("{=!}{newline}", null), null);
			GameTexts.SetVariable("BATTLE_REMAINS_TEXT", textObject10);
			args.MenuContext.SetPanelSound("event:/ui/wreckage/wreckage_panel");
			this.SetWreckageMenuBackgrounds(args);
		}

		// Token: 0x06003D58 RID: 15704 RVA: 0x000FEA81 File Offset: 0x000FCC81
		private bool battle_remains_leave_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x000FEA8C File Offset: 0x000FCC8C
		private void battle_remains_leave_consequence(MenuCallbackArgs args)
		{
			this._encounteredBattleWreckage = null;
			if (PlayerEncounter.Current == null && Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.ExitToLast();
			}
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x000FEAAD File Offset: 0x000FCCAD
		private int GetBattleRemainsWinnerTroopCount(BattleWreckage wreckage)
		{
			if (wreckage.WinnerSide != BattleSideEnum.Attacker)
			{
				return wreckage.DefenderHealthyTroopCountAtStart;
			}
			return wreckage.AttackerHealthyTroopCountAtStart;
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x000FEAC5 File Offset: 0x000FCCC5
		private int GetBattleRemainsDefeatedTroopCount(BattleWreckage wreckage)
		{
			if (wreckage.WinnerSide != BattleSideEnum.Attacker)
			{
				return wreckage.AttackerHealthyTroopCountAtStart;
			}
			return wreckage.DefenderHealthyTroopCountAtStart;
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x000FEADD File Offset: 0x000FCCDD
		private int GetBattleRemainsWinnerCasualties(BattleWreckage wreckage)
		{
			if (wreckage.WinnerSide == BattleSideEnum.Attacker)
			{
				return wreckage.AttackerWoundedInBattle.TotalRegulars + wreckage.AttackerDiedInBattle.TotalRegulars;
			}
			return wreckage.DefenderWoundedInBattle.TotalRegulars + wreckage.DefenderDiedInBattle.TotalRegulars;
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x000FEB17 File Offset: 0x000FCD17
		private int GetBattleRemainsDefeatedCasualties(BattleWreckage wreckage)
		{
			if (wreckage.WinnerSide == BattleSideEnum.Attacker)
			{
				return wreckage.DefenderWoundedInBattle.TotalRegulars + wreckage.DefenderDiedInBattle.TotalRegulars;
			}
			return wreckage.AttackerWoundedInBattle.TotalRegulars + wreckage.AttackerDiedInBattle.TotalRegulars;
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x000FEB54 File Offset: 0x000FCD54
		private List<TextObject> GetBattleRemainsFallenHeroLines(BattleWreckage wreckage)
		{
			List<TextObject> list = new List<TextObject>();
			TroopRoster[] array = new TroopRoster[] { wreckage.AttackerDiedInBattle, wreckage.DefenderDiedInBattle };
			for (int i = 0; i < array.Length; i++)
			{
				foreach (TroopRosterElement troopRosterElement in array[i].GetTroopRoster())
				{
					if (troopRosterElement.Character.IsHero)
					{
						TextObject textObject = new TextObject("{=PSYezDpZ}- {HERO_NAME} (Killed)", null);
						textObject.SetTextVariable("HERO_NAME", troopRosterElement.Character.HeroObject.Name);
						list.Add(textObject);
					}
				}
			}
			return list;
		}

		// Token: 0x040012B3 RID: 4787
		private const float SmallWreckageCreationChance = 0.85f;

		// Token: 0x040012B4 RID: 4788
		private const string LandWreckageMenuBackgroundId = "wreckage_land";

		// Token: 0x040012B5 RID: 4789
		private const string NavalWreckageMenuBackgroundId = "wreckage_sea";

		// Token: 0x040012B6 RID: 4790
		private const string MenuIdBattleWreckage = "battle_wreckage";

		// Token: 0x040012B7 RID: 4791
		private const string MenuIdInvestigateWait = "battle_wreckage_investigate_wait_menu";

		// Token: 0x040012B8 RID: 4792
		private const string MenuIdResults = "battle_wreckage_results_menu";

		// Token: 0x040012B9 RID: 4793
		private const string MenuIdRecoverTroopsDecision = "battle_wreckage_recover_troops_decision";

		// Token: 0x040012BA RID: 4794
		private const string BattleWreckageResultsConnectionMenuId = "battle_wreckage_results_connection_menu";

		// Token: 0x040012BB RID: 4795
		private const string MenuIdBattleRemains = "battle_wreckage_remains";

		// Token: 0x040012BC RID: 4796
		private const string RecoverTroopsConsequenceId = "RecoverTroopsConsequence";

		// Token: 0x040012BD RID: 4797
		private const string GainTradeGoodsConsequenceId = "GainTradeGoodsConsequence";

		// Token: 0x040012BE RID: 4798
		private const string GainGoldConsequenceId = "GainGoldConsequence";

		// Token: 0x040012BF RID: 4799
		private const string UnlockHideoutConsequenceId = "UnlockHideoutConsequence";

		// Token: 0x040012C0 RID: 4800
		private const string NothingFoundConsequenceId = "NothingFoundConsequence";

		// Token: 0x040012C1 RID: 4801
		public const string MenuIdWreckageEncounterBlocking = "battle_wreckage_encounter_blocking";

		// Token: 0x040012C2 RID: 4802
		private const int MinDistanceFromSettlementToConsiderWreckageOnLand = 10;

		// Token: 0x040012C3 RID: 4803
		private const int MinDistanceFromSettlementToConsiderWreckageOnNaval = 20;

		// Token: 0x040012C4 RID: 4804
		private const int MinDistanceNeededFromWreckageOnLand = 20;

		// Token: 0x040012C5 RID: 4805
		private const int MinDistanceNeededFromWreckageOnNaval = 20;

		// Token: 0x040012C6 RID: 4806
		private const float UnlockHideoutMapsChance = 0.5f;

		// Token: 0x040012C7 RID: 4807
		private const int NormalWreckageInvestigationMinRequiredPartySize = 50;

		// Token: 0x040012C8 RID: 4808
		private const int EpicWreckageInvestigationMinRequiredPartySize = 90;

		// Token: 0x040012C9 RID: 4809
		private const string WreckageMenuSoundPath = "event:/ui/wreckage/wreckage_panel";

		// Token: 0x040012CA RID: 4810
		private const int InterruptedSnapshotExpiryAsHours = 24;

		// Token: 0x040012CB RID: 4811
		private const string RewardMenuSoundPath = "event:/ui/wreckage/generic_recover";

		// Token: 0x040012CC RID: 4812
		private const string InvestigationSoundPath = "event:/ui/wreckage/investigate_the_battlefield";

		// Token: 0x040012CD RID: 4813
		private BattleWreckage _encounteredBattleWreckage;

		// Token: 0x040012CE RID: 4814
		private CampaignTime _playerInvestigationStartTime;

		// Token: 0x040012CF RID: 4815
		private CampaignTime _requiredInvestigationDuration;

		// Token: 0x040012D0 RID: 4816
		private CampaignTime _lastPlayerWreckageInteractionTime;

		// Token: 0x040012D1 RID: 4817
		private bool _isEncounteredWreckageInterrupted;

		// Token: 0x040012D2 RID: 4818
		private readonly List<BattleWreckageCampaignBehavior.BattleWreckageConsequence> _wreckageConsequences = new List<BattleWreckageCampaignBehavior.BattleWreckageConsequence>();

		// Token: 0x040012D3 RID: 4819
		private string _currentOverriddenConsequenceId;

		// Token: 0x040012D4 RID: 4820
		private bool _isOverriddenConsequenceAccepted;

		// Token: 0x040012D5 RID: 4821
		private ItemRoster _lootedItems = new ItemRoster();

		// Token: 0x040012D6 RID: 4822
		private TroopRoster _lootedTroops = TroopRoster.CreateDummyTroopRoster();

		// Token: 0x040012D7 RID: 4823
		private int _lootedGoldAmount;

		// Token: 0x040012D8 RID: 4824
		private List<TextObject> _consequenceExplanations = new List<TextObject>();

		// Token: 0x040012D9 RID: 4825
		private readonly List<BattleWreckageCampaignBehavior.BattleWreckageConsequence> _selectedConsequences = new List<BattleWreckageCampaignBehavior.BattleWreckageConsequence>();

		// Token: 0x040012DA RID: 4826
		private int _recoverTroopCountsToAdd;

		// Token: 0x040012DB RID: 4827
		private const int SmallWreckageRecoverTroopCountsLimitMin = 2;

		// Token: 0x040012DC RID: 4828
		private const int SmallWreckageRecoverTroopCountsLimitMax = 5;

		// Token: 0x040012DD RID: 4829
		private const int NormalWreckageRecoverTroopCountsLimitMin = 5;

		// Token: 0x040012DE RID: 4830
		private const int NormalWreckageRecoverTroopCountsLimitMax = 10;

		// Token: 0x040012DF RID: 4831
		private const int EpicWreckageRecoverTroopCountsLimitMin = 10;

		// Token: 0x040012E0 RID: 4832
		private const int EpicWreckageRecoverTroopCountsLimitMax = 15;

		// Token: 0x040012E1 RID: 4833
		private const int SmallWreckageRecoverTroopsTierLimit = 2;

		// Token: 0x040012E2 RID: 4834
		private const int NormalWreckageRecoverTroopsTierLimit = 3;

		// Token: 0x040012E3 RID: 4835
		private const float CasualtyLootChance = 0.5f;

		// Token: 0x040012E4 RID: 4836
		private const float TradeGoodBonusLootMultiplier = 0.5f;

		// Token: 0x040012E5 RID: 4837
		private const int TradeGoodTargetValue = 1000;

		// Token: 0x040012E6 RID: 4838
		private const int RogueryXpBonusBase = 75;

		// Token: 0x040012E7 RID: 4839
		private const float TradeGoodBonusMultiplier = 1.2f;

		// Token: 0x020007FC RID: 2044
		private readonly struct BattleWreckageConsequence
		{
			// Token: 0x06006694 RID: 26260 RVA: 0x001D0B65 File Offset: 0x001CED65
			public BattleWreckageConsequence(string stringId, Func<BattleWreckage, bool> condition, Action consequence, float chance = 0.5f, bool canOverride = false)
			{
				this.StringId = stringId;
				this.Condition = condition;
				this.Consequence = consequence;
				this.Chance = chance;
				this.CanOverride = canOverride;
			}

			// Token: 0x040020A3 RID: 8355
			public readonly string StringId;

			// Token: 0x040020A4 RID: 8356
			public readonly Func<BattleWreckage, bool> Condition;

			// Token: 0x040020A5 RID: 8357
			public readonly Action Consequence;

			// Token: 0x040020A6 RID: 8358
			public readonly float Chance;

			// Token: 0x040020A7 RID: 8359
			public readonly bool CanOverride;
		}

		// Token: 0x020007FD RID: 2045
		private enum WreckageLeaveReason
		{
			// Token: 0x040020A9 RID: 8361
			LeaveWithoutInvestigation,
			// Token: 0x040020AA RID: 8362
			AbandonInvestigation,
			// Token: 0x040020AB RID: 8363
			InvestigationCompleted,
			// Token: 0x040020AC RID: 8364
			PlayerInterrupted,
			// Token: 0x040020AD RID: 8365
			WreckageExpired
		}
	}
}
