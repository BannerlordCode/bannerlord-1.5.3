using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000418 RID: 1048
	public class HideoutCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x060042CE RID: 17102 RVA: 0x00131CB0 File Offset: 0x0012FEB0
		private static float IncreaseRelationWithVillageNotableMaximumDistanceAsDays
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000E91 RID: 3729
		// (get) Token: 0x060042CF RID: 17103 RVA: 0x00131CB7 File Offset: 0x0012FEB7
		private int CanAttackHideoutStart
		{
			get
			{
				return Campaign.Current.Models.HideoutModel.CanAttackHideoutStartTime;
			}
		}

		// Token: 0x17000E92 RID: 3730
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x00131CCD File Offset: 0x0012FECD
		private int CanAttackHideoutEnd
		{
			get
			{
				return Campaign.Current.Models.HideoutModel.CanAttackHideoutEndTime;
			}
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x00131CE4 File Offset: 0x0012FEE4
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnHideoutSpottedEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnHideoutSpotted));
			CampaignEvents.OnCollectLootsItemsEvent.AddNonSerializedListener(this, new Action<PartyBase, ItemRoster>(this.OnCollectLootItems));
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x00131D7B File Offset: 0x0012FF7B
		public void OnNewGameCreated(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x00131D84 File Offset: 0x0012FF84
		public void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x00131D90 File Offset: 0x0012FF90
		public void HourlyTickSettlement(Settlement settlement)
		{
			if (settlement.IsHideout && settlement.Hideout.IsInfested && !settlement.IsVisible)
			{
				float hideoutSpottingDistance = Campaign.Current.Models.MapVisibilityModel.GetHideoutSpottingDistance();
				float num = MobileParty.MainParty.Position.DistanceSquared(settlement.Position);
				float num2 = 1f - num / (hideoutSpottingDistance * hideoutSpottingDistance);
				if (num2 > 0f && settlement.Parties.Count > 0 && MBRandom.RandomFloat < num2)
				{
					settlement.IsVisible = true;
					CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, settlement.Party);
				}
			}
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x00131E39 File Offset: 0x00130039
		private void OnHideoutSpotted(PartyBase party, PartyBase hideout)
		{
			SkillLevelingManager.OnHideoutSpotted(party.MobileParty, hideout);
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x00131E47 File Offset: 0x00130047
		private int GetItemValueForHideoutLoot(ItemObject itemToLoot)
		{
			return Campaign.Current.Models.TradeItemPriceFactorModel.GetTheoreticalMaxItemMarketValue(itemToLoot) + 1;
		}

		// Token: 0x060042D7 RID: 17111 RVA: 0x00131E60 File Offset: 0x00130060
		private void OnCollectLootItems(PartyBase winnerParty, ItemRoster gainedLoots)
		{
			if (winnerParty == PartyBase.MainParty && !PlayerEncounter.Current.ForceHideoutSendTroops)
			{
				MapEvent mapEvent = MobileParty.MainParty.MapEvent;
				if (mapEvent.IsHideoutBattle && mapEvent.MapEventSettlement == Settlement.CurrentSettlement && this.IsItNighttimeNow())
				{
					int num = 0;
					foreach (MapEventParty mapEventParty in mapEvent.GetMapEventSide(mapEvent.PlayerSide).Parties)
					{
						if (mapEventParty.Party == PartyBase.MainParty)
						{
							num = mapEventParty.PlunderedGold;
							break;
						}
					}
					int totalLootedValue = 0;
					int initialHideoutPopulation = (PlayerEncounter.Battle.Component as HideoutEventComponent).InitialHideoutPopulation;
					float targetValue = (float)(num * (initialHideoutPopulation * 135));
					targetValue = MathF.Clamp(targetValue, (float)this._minimumHideoutLootTargetValue, 4750f);
					if ((float)totalLootedValue < targetValue)
					{
						int num2 = 0;
						ItemObject itemObject;
						while (num2 < this._potentialLootItems.Count && gainedLoots.Count < 5 && (float)totalLootedValue < targetValue)
						{
							itemObject = this._potentialLootItems[num2];
							int itemValueForHideoutLoot = this.GetItemValueForHideoutLoot(itemObject);
							if ((float)itemValueForHideoutLoot <= targetValue - (float)totalLootedValue)
							{
								gainedLoots.AddToCounts(itemObject, 1);
								totalLootedValue += itemValueForHideoutLoot;
							}
							num2++;
						}
						Func<ItemRosterElement, bool> <>9__0;
						do
						{
							Func<ItemRosterElement, bool> func;
							if ((func = <>9__0) == null)
							{
								func = (<>9__0 = (ItemRosterElement x) => !x.EquipmentElement.Item.NotMerchandise && !x.EquipmentElement.IsQuestItem && !x.EquipmentElement.Item.IsBannerItem && (float)this.GetItemValueForHideoutLoot(x.EquipmentElement.Item) <= targetValue - (float)totalLootedValue);
							}
							itemObject = gainedLoots.GetRandomElementWithPredicate<ItemRosterElement>(func).EquipmentElement.Item;
							if (itemObject != null)
							{
								gainedLoots.AddToCounts(itemObject, 1);
								totalLootedValue += this.GetItemValueForHideoutLoot(itemObject);
							}
						}
						while (itemObject != null);
					}
				}
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x00132068 File Offset: 0x00130268
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<float>("_hideoutWaitProgressHours", ref this._hideoutWaitProgressHours);
			dataStore.SyncData<float>("_hideoutWaitTargetHours", ref this._hideoutWaitTargetHours);
			dataStore.SyncData<float>("_hideoutSendTroopsWaitProgressHour", ref this._hideoutSendTroopsWaitProgressHour);
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x001320A0 File Offset: 0x001302A0
		private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			foreach (ItemObject itemObject in Campaign.Current.ObjectManager.GetObjectTypeList<ItemObject>())
			{
				if (itemObject.IsTradeGood)
				{
					int itemValueForHideoutLoot = this.GetItemValueForHideoutLoot(itemObject);
					if (itemValueForHideoutLoot >= this._minimumHideoutLootTargetValue && itemValueForHideoutLoot <= 4750)
					{
						this._potentialLootItems.Add(itemObject);
					}
				}
			}
			this._potentialLootItems = this._potentialLootItems.OrderByDescending<ItemObject, int>((ItemObject x) => x.Value).ToList<ItemObject>();
			if (this._potentialLootItems.Count > 0)
			{
				this._minimumHideoutLootTargetValue = this.GetItemValueForHideoutLoot(this._potentialLootItems[this._potentialLootItems.Count - 1]);
			}
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x0013218C File Offset: 0x0013038C
		protected void AddGameMenus(CampaignGameStarter campaignGameStarter)
		{
			campaignGameStarter.AddGameMenu("hideout_place", "{=!}{HIDEOUT_TEXT}", new OnInitDelegate(this.game_menu_hideout_place_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "attack", "{=p5GkeK8F}Sneak in now", new GameMenuOption.OnConditionDelegate(this.game_menu_hideout_sneak_in_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_sneak_in_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "assault", "{=b4YsZY5H}Assault hideout", new GameMenuOption.OnConditionDelegate(this.game_menu_assault_hideout_parties_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_assault_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "wait", "{=!}{WAIT_OPTION}", new GameMenuOption.OnConditionDelegate(this.game_menu_wait_until_nightfall_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_wait_until_nightfall_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "send_troops", "{=RepcYuoJ}Send troops to clear ({SUCCESS_CHANCE}% chance)", new GameMenuOption.OnConditionDelegate(this.game_menu_send_troops_hideout_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_send_troops_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_place", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddWaitGameMenu("hideout_wait", "{=!}{WAIT_TEXT}", new OnInitDelegate(this.hideout_wait_menu_on_init), new OnConditionDelegate(this.hideout_wait_menu_on_condition), new OnConsequenceDelegate(this.hideout_wait_menu_on_consequence), new OnTickDelegate(this.hideout_wait_menu_on_tick), GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, GameMenu.MenuOverlayType.None, this._hideoutWaitTargetHours, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_after_wait_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_wait", "{=!}{HIDEOUT_TEXT}", new OnInitDelegate(this.hideout_after_wait_menu_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "attack", "{=Abcgrf4j}Sneak in", new GameMenuOption.OnConditionDelegate(this.game_menu_hideout_sneak_in_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_sneak_in_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "assault", "{=b4YsZY5H}Assault hideout", new GameMenuOption.OnConditionDelegate(this.game_menu_assault_hideout_parties_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_assault_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "send_troops", "{=RepcYuoJ}Send troops to clear ({SUCCESS_CHANCE}% chance)", new GameMenuOption.OnConditionDelegate(this.game_menu_send_troops_hideout_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_encounter_send_troops_on_consequence), false, -1, false, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_after_wait_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_defeated_and_saved", "{=1zLZf5rw}The rest of your men rushed to your help, dragging you out to safety and driving the bandits back into hiding.", new OnInitDelegate(this.game_menu_hideout_after_defeated_and_saved_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_defeated_and_saved", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_after_found_by_sentries", "{=n0ynsBPx}Sentries detected you and alerted the rest of the bandits. Bandits moved back into hiding before you could round up your troops.", new OnInitDelegate(this.game_menu_hideout_after_defeated_and_saved_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_after_found_by_sentries", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_hideout_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddWaitGameMenu("hideout_send_troops_wait", "{=QOT7PSUp}Your troops are clearing the hideout.", new OnInitDelegate(this.hideout_send_troops_wait_menu_on_init), null, null, new OnTickDelegate(this.hideout_send_troops_wait_menu_tick), GameMenu.MenuAndOptionType.WaitMenuShowProgressAndHoursOption, GameMenu.MenuOverlayType.None, 6f, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_send_troops_wait", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.hideout_send_troops_wait_leave_on_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_send_troops_result_success", "{=nfgQU4Uk}Your men surprised the bandits, and cut down several before the rest fled in disarray. You don't think they'll be back.", new OnInitDelegate(this.hideout_send_troops_result_success_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_send_troops_result_success", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.hideout_send_troops_result_success_consequence), true, -1, false, null);
			campaignGameStarter.AddGameMenu("hideout_send_troops_result_failure", "{=b4HypxQ5}Your men failed their approach... The bandits' sentries spotted them before they attacked, and the bandits withdrew in good order, probably to some nearby hiding places. Until they are dealt with properly, their presence will continue to threaten the region. You expect it won't be long before they return to their lair.\r\n", new OnInitDelegate(this.hideout_send_troops_result_failure_on_init), GameMenu.MenuOverlayType.None, GameMenu.MenuFlags.None, null);
			campaignGameStarter.AddGameMenuOption("hideout_send_troops_result_failure", "leave", "{=3sRdGQou}Leave", new GameMenuOption.OnConditionDelegate(this.leave_on_condition), new GameMenuOption.OnConsequenceDelegate(this.hideout_send_troops_result_failure_consequence), true, -1, false, null);
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x001325B0 File Offset: 0x001307B0
		private void hideout_send_troops_result_success_consequence(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			PlayerEncounter.Finish(true);
			this.SetCleanHideoutRelations(currentSettlement);
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x001325D0 File Offset: 0x001307D0
		private void hideout_send_troops_result_success_on_init(MenuCallbackArgs args)
		{
			if (PlayerEncounter.Battle.WinningSide == BattleSideEnum.None)
			{
				PlayerEncounter.Battle.SetOverrideWinner(PlayerEncounter.Battle.PlayerSide);
				CampaignEventDispatcher.Instance.OnHideoutBattleCompleted(PlayerEncounter.Battle.PlayerSide, (HideoutEventComponent)PlayerEncounter.Battle.Component, HideoutEventComponent.HideoutBattleEndState.SendTroops);
				PlayerEncounter.Update();
				Settlement encounterSettlement = PlayerEncounter.EncounterSettlement;
				if (encounterSettlement == null)
				{
					return;
				}
				encounterSettlement.Party.SetVisualAsDirty();
			}
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x0013263B File Offset: 0x0013083B
		private void hideout_send_troops_result_failure_on_init(MenuCallbackArgs args)
		{
			Settlement.CurrentSettlement.Hideout.SetNextPossibleAttackTime(Campaign.Current.Models.HideoutModel.HideoutHiddenDuration);
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x00132660 File Offset: 0x00130860
		private void hideout_send_troops_result_failure_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x00132668 File Offset: 0x00130868
		private void hideout_wait_menu_on_init(MenuCallbackArgs args)
		{
			this.UpdateHideoutWaitProgress(args);
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x00132674 File Offset: 0x00130874
		private bool IsItNighttimeNow()
		{
			float currentHourInDay = CampaignTime.Now.CurrentHourInDay;
			return (this.CanAttackHideoutStart > this.CanAttackHideoutEnd && (currentHourInDay >= (float)this.CanAttackHideoutStart || currentHourInDay < (float)this.CanAttackHideoutEnd)) || (this.CanAttackHideoutStart < this.CanAttackHideoutEnd && currentHourInDay >= (float)this.CanAttackHideoutStart && currentHourInDay < (float)this.CanAttackHideoutEnd);
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x001326D9 File Offset: 0x001308D9
		public bool hideout_wait_menu_on_condition(MenuCallbackArgs args)
		{
			return true;
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x001326DC File Offset: 0x001308DC
		public void hideout_wait_menu_on_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._hideoutWaitProgressHours += (float)campaignTime.ToHours;
			this.UpdateHideoutWaitProgress(args);
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x001326FC File Offset: 0x001308FC
		private void UpdateHideoutWaitProgress(MenuCallbackArgs args)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (!this.IsItNighttimeNow())
			{
				textObject = new TextObject("{=VLLAOXve}Waiting until nightfall to sneak in.", null);
			}
			else
			{
				textObject = new TextObject("{=GrrWYNIZ}Waiting until morning to begin the assault.", null);
			}
			MBTextManager.SetTextVariable("WAIT_TEXT", textObject, false);
			if (this._hideoutWaitTargetHours.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this.CalculateHideoutAttackTime();
			}
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._hideoutWaitProgressHours / this._hideoutWaitTargetHours);
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x00132777 File Offset: 0x00130977
		public void hideout_wait_menu_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_after_wait");
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00132783 File Offset: 0x00130983
		private bool leave_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leave;
			return true;
		}

		// Token: 0x060042E6 RID: 17126 RVA: 0x00132790 File Offset: 0x00130990
		[GameMenuInitializationHandler("hideout_wait")]
		[GameMenuInitializationHandler("hideout_after_wait")]
		[GameMenuInitializationHandler("hideout_after_defeated_and_saved")]
		private static void game_menu_hideout_ui_place_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Hideout.WaitMeshName);
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x001327BC File Offset: 0x001309BC
		[GameMenuInitializationHandler("hideout_place")]
		private static void game_menu_hideout_sound_place_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetPanelSound("event:/ui/panels/settlement_hideout");
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Hideout.WaitMeshName);
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x001327F5 File Offset: 0x001309F5
		private void game_menu_hideout_after_defeated_and_saved_on_init(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.IsHideout)
			{
				return;
			}
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
		}

		// Token: 0x060042E9 RID: 17129 RVA: 0x00132818 File Offset: 0x00130A18
		private void game_menu_hideout_place_on_init(MenuCallbackArgs args)
		{
			if (!Settlement.CurrentSettlement.IsHideout)
			{
				return;
			}
			this._hideoutWaitProgressHours = 0f;
			this._hideoutSendTroopsWaitProgressHour = 0f;
			if (!this.IsItNighttimeNow())
			{
				this.CalculateHideoutAttackTime();
			}
			else
			{
				this._hideoutWaitTargetHours = 0f;
			}
			Settlement currentSettlement = Settlement.CurrentSettlement;
			int num = 0;
			foreach (MobileParty mobileParty in currentSettlement.Parties)
			{
				num += mobileParty.MemberRoster.TotalManCount - mobileParty.MemberRoster.TotalWounded;
			}
			GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=DOmb81Mu}(Undefined hideout type)");
			if (currentSettlement.Culture.StringId.Equals("forest_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=cu2cLT5r}You spy though the trees what seems to be a clearing in the forest with what appears to be the outlines of a camp.");
			}
			if (currentSettlement.Culture.StringId.Equals("sea_raiders"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=bJ6ygV3P}As you travel along the coast, you see a sheltered cove with what appears to the outlines of a camp.");
			}
			if (currentSettlement.Culture.StringId.Equals("mountain_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=iyWUDSm8}Passing by the slopes of the mountains, you see an outcrop crowned with the ruins of an ancient fortress.");
			}
			if (currentSettlement.Culture.StringId.Equals("desert_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=b3iBOVXN}Passing by a wadi, you see what looks like a camouflaged well to tap the groundwater left behind by rare rainfalls.");
			}
			if (currentSettlement.Culture.StringId.Equals("steppe_bandits"))
			{
				GameTexts.SetVariable("HIDEOUT_DESCRIPTION", "{=5JaGVr0U}While traveling by a low range of hills, you see what appears to be the remains of a campsite in a stream gully.");
			}
			bool flag = !currentSettlement.Hideout.NextPossibleAttackTime.IsPast;
			if (flag)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=KLWn6yZQ}{HIDEOUT_DESCRIPTION} The remains of a fire suggest that it's been recently occupied, but its residents - whoever they are - are well-hidden for now. Until they are dealt with properly, their presence will continue to threaten the region.");
			}
			else if (num > 0)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=prcBBqMR}{HIDEOUT_DESCRIPTION} You see armed men moving about. As you listen quietly, you hear scraps of conversation about raids, ransoms, and the best places to waylay travellers.");
			}
			else
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=gywyEgZa}{HIDEOUT_DESCRIPTION} There seems to be no one inside.");
			}
			if (!flag && num > 0 && Hero.MainHero.IsWounded)
			{
				GameTexts.SetVariable("HIDEOUT_TEXT", "{=fMekM2UH}{HIDEOUT_DESCRIPTION} You can not attack since your wounds do not allow you.");
			}
			if (MobileParty.MainParty.CurrentSettlement == null)
			{
				PlayerEncounter.EnterSettlement();
			}
			bool isInfested = Settlement.CurrentSettlement.Hideout.IsInfested;
			Settlement settlement = (Settlement.CurrentSettlement.IsHideout ? Settlement.CurrentSettlement : null);
			if (PlayerEncounter.Battle != null)
			{
				bool flag2 = PlayerEncounter.Battle.WinningSide == PlayerEncounter.Current.PlayerSide;
				PlayerEncounter.Update();
				if (flag2 && PlayerEncounter.Battle == null && settlement != null)
				{
					this.SetCleanHideoutRelations(settlement);
				}
			}
		}

		// Token: 0x060042EA RID: 17130 RVA: 0x00132A74 File Offset: 0x00130C74
		private void CalculateHideoutAttackTime()
		{
			float currentHourInDay = CampaignTime.Now.CurrentHourInDay;
			if (this.IsItNighttimeNow())
			{
				this._hideoutWaitTargetHours = (((float)this.CanAttackHideoutEnd > currentHourInDay) ? ((float)this.CanAttackHideoutEnd - currentHourInDay) : ((float)CampaignTime.HoursInDay - currentHourInDay + (float)this.CanAttackHideoutEnd));
				return;
			}
			this._hideoutWaitTargetHours = (((float)this.CanAttackHideoutStart > currentHourInDay) ? ((float)this.CanAttackHideoutStart - currentHourInDay) : ((float)CampaignTime.HoursInDay - currentHourInDay + (float)this.CanAttackHideoutStart));
		}

		// Token: 0x060042EB RID: 17131 RVA: 0x00132AF0 File Offset: 0x00130CF0
		private void SetCleanHideoutRelations(Settlement hideout)
		{
			List<Settlement> list = new List<Settlement>();
			float num = HideoutCampaignBehavior.IncreaseRelationWithVillageNotableMaximumDistanceAsDays * Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay;
			foreach (Village village in Campaign.Current.AllVillages)
			{
				if (village.Settlement.Position.DistanceSquared(hideout.Position) <= num * num)
				{
					list.Add(village.Settlement);
				}
			}
			foreach (Settlement settlement in list)
			{
				if (settlement.Notables.Count > 0)
				{
					ChangeRelationAction.ApplyPlayerRelation(settlement.Notables.GetRandomElement<Hero>(), 2, true, false);
				}
			}
			if (Hero.MainHero.GetPerkValue(DefaultPerks.Charm.EffortForThePeople))
			{
				Town town = SettlementHelper.FindNearestTownToSettlement(hideout, MobileParty.NavigationType.All, null);
				Hero leader = town.OwnerClan.Leader;
				if (leader == Hero.MainHero)
				{
					town.Loyalty += 1f;
				}
				else
				{
					ChangeRelationAction.ApplyPlayerRelation(leader, (int)DefaultPerks.Charm.EffortForThePeople.PrimaryBonus, true, true);
				}
			}
			MBTextManager.SetTextVariable("RELATION_VALUE", (int)DefaultPerks.Charm.EffortForThePeople.PrimaryBonus);
			MBInformationManager.AddQuickInformation(new TextObject("{=o0qwDa0q}Your relation increased by {RELATION_VALUE} with nearby notables.", null), 0, null, null, "");
		}

		// Token: 0x060042EC RID: 17132 RVA: 0x00132C6C File Offset: 0x00130E6C
		private void hideout_after_wait_menu_on_init(MenuCallbackArgs args)
		{
			TextObject textObject = TextObject.GetEmpty();
			if (this.IsItNighttimeNow())
			{
				textObject = new TextObject("{=VbU8Ue0O}After waiting for a while you find a good opportunity to close in undetected beneath the shroud of the night.", null);
			}
			else
			{
				textObject = new TextObject("{=7ovEa1gB}After waiting for a while you find a good opportunity to assault the camp.", null);
			}
			MBTextManager.SetTextVariable("HIDEOUT_TEXT", textObject, false);
		}

		// Token: 0x060042ED RID: 17133 RVA: 0x00132CB0 File Offset: 0x00130EB0
		private bool game_menu_hideout_sneak_in_on_condition(MenuCallbackArgs args)
		{
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			object obj;
			if (Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit))
				{
					obj = hideout.NextPossibleAttackTime.IsPast;
					goto IL_0062;
				}
			}
			obj = 0;
			IL_0062:
			object obj2 = obj;
			if (obj2 != null)
			{
				if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=pM9GOxrV}You are wounded, you can't sneak in!", null);
				}
				else
				{
					int minimumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, false);
					if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumTroopCountForHideoutMission)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=XasRXCod}You should have more than {AMOUNT} healthy troops in your party to attack!", null);
						args.Tooltip.SetTextVariable("AMOUNT", minimumTroopCountForHideoutMission);
					}
					else if (!this.IsItNighttimeNow())
					{
						args.Tooltip = new TextObject("{=vILXBINS}Trying to sneak past the sentries in daylight is not an option.", null);
						args.IsEnabled = false;
					}
				}
			}
			args.optionLeaveType = GameMenuOption.LeaveType.SneakIn;
			return obj2 != null;
		}

		// Token: 0x060042EE RID: 17134 RVA: 0x00132DCC File Offset: 0x00130FCC
		private bool game_menu_assault_hideout_parties_on_condition(MenuCallbackArgs args)
		{
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			object obj;
			if (Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit))
				{
					obj = hideout.NextPossibleAttackTime.IsPast;
					goto IL_0062;
				}
			}
			obj = 0;
			IL_0062:
			object obj2 = obj;
			if (obj2 != null)
			{
				if (Hero.MainHero.IsWounded)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=ZCKKVRjT}You are wounded, you can't assault the hideout!", null);
				}
				else
				{
					int minimumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, true);
					if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumTroopCountForHideoutMission)
					{
						args.IsEnabled = false;
						args.Tooltip = new TextObject("{=XasRXCod}You should have more than {AMOUNT} healthy troops in your party to attack!", null);
						args.Tooltip.SetTextVariable("AMOUNT", minimumTroopCountForHideoutMission);
					}
					else if (this.IsItNighttimeNow())
					{
						args.Tooltip = new TextObject("{=rgo5t91d}With bandit sentries on the lookout, a frontal assault against the hideout is too dangerous at night.", null);
						args.IsEnabled = false;
					}
				}
				args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			}
			return obj2 != null;
		}

		// Token: 0x060042EF RID: 17135 RVA: 0x00132EE8 File Offset: 0x001310E8
		private void game_menu_encounter_sneak_in_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args, delegate(TroopRoster x)
			{
				this.OnTroopRosterManageDone(x, false);
			}, false);
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x00132EFE File Offset: 0x001310FE
		private void game_menu_encounter_assault_on_consequence(MenuCallbackArgs args)
		{
			this.game_menu_encounter_attack_on_consequence(args, delegate(TroopRoster x)
			{
				this.OnTroopRosterManageDone(x, true);
			}, true);
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x00132F14 File Offset: 0x00131114
		private void game_menu_encounter_attack_on_consequence(MenuCallbackArgs args, Action<TroopRoster> onDone, bool isDirectAssault)
		{
			BanditDensityModel banditDensityModel = Campaign.Current.Models.BanditDensityModel;
			int maximumTroopCountForHideoutMission = banditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty.MainParty, isDirectAssault);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster strongestAndPriorTroops = MobilePartyHelper.GetStrongestAndPriorTroops(MobileParty.MainParty, maximumTroopCountForHideoutMission, true);
			troopRoster.Add(strongestAndPriorTroops);
			int maximumTroopCountForHideoutMission2 = banditDensityModel.GetMaximumTroopCountForHideoutMission(MobileParty.MainParty, isDirectAssault);
			args.MenuContext.OpenTroopSelection(MobileParty.MainParty.MemberRoster, troopRoster, new Func<CharacterObject, bool>(this.CanChangeStatusOfTroop), onDone, maximumTroopCountForHideoutMission2, banditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, isDirectAssault));
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x00132F98 File Offset: 0x00131198
		private bool game_menu_send_troops_hideout_on_condition(MenuCallbackArgs args)
		{
			Hideout hideout = Settlement.CurrentSettlement.Hideout;
			args.Tooltip = new TextObject("{=Xum0Iddf}You will gain no loot or experience.", null);
			object obj;
			if (Settlement.CurrentSettlement.MapFaction != PartyBase.MainParty.MapFaction)
			{
				if (Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit))
				{
					obj = hideout.NextPossibleAttackTime.IsPast;
					goto IL_0073;
				}
			}
			obj = 0;
			IL_0073:
			object obj2 = obj;
			if (obj2 != null)
			{
				int minimumTroopCountForHideoutMission = Campaign.Current.Models.BanditDensityModel.GetMinimumTroopCountForHideoutMission(MobileParty.MainParty, false);
				if (MobileParty.MainParty.MemberRoster.TotalHealthyCount < minimumTroopCountForHideoutMission)
				{
					args.IsEnabled = false;
					args.Tooltip = new TextObject("{=yUbdUFSC}You should have more than {AMOUNT} healthy troops in your party to send your troops!", null);
					args.Tooltip.SetTextVariable("AMOUNT", minimumTroopCountForHideoutMission);
				}
				args.optionLeaveType = GameMenuOption.LeaveType.OrderTroopsToAttack;
				float sendTroopsSuccessChance = Campaign.Current.Models.HideoutModel.GetSendTroopsSuccessChance(hideout);
				MBTextManager.SetTextVariable("SUCCESS_CHANCE", MathF.Round(sendTroopsSuccessChance * 100f));
			}
			return obj2 != null;
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x001330A9 File Offset: 0x001312A9
		private void game_menu_encounter_send_troops_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Current.ForceHideoutSendTroops = true;
			GameMenu.SwitchToMenu("encounter");
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x001330C0 File Offset: 0x001312C0
		private void ArrangeHideoutTroopCountsForMission()
		{
			int numberOfMinimumBanditTroopsInHideoutMission = Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditTroopsInHideoutMission;
			int num = Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForFirstFightInHideout + Campaign.Current.Models.BanditDensityModel.NumberOfMaximumTroopCountForBossFightInHideout;
			MBList<MobileParty> mblist = Settlement.CurrentSettlement.Parties.Where<MobileParty>((MobileParty x) => x.IsBandit || x.IsBanditBossParty).ToMBList<MobileParty>();
			int num2 = mblist.Sum<MobileParty>((MobileParty x) => x.MemberRoster.TotalHealthyCount);
			if (num2 > num)
			{
				int i = num2 - num;
				mblist.RemoveAll((MobileParty x) => x.IsBanditBossParty || x.MemberRoster.TotalHealthyCount == 1);
				while (i > 0)
				{
					if (mblist.Count <= 0)
					{
						return;
					}
					MobileParty randomElement = mblist.GetRandomElement<MobileParty>();
					List<TroopRosterElement> troopRoster = randomElement.MemberRoster.GetTroopRoster();
					List<ValueTuple<TroopRosterElement, float>> list = new List<ValueTuple<TroopRosterElement, float>>();
					foreach (TroopRosterElement troopRosterElement in troopRoster)
					{
						list.Add(new ValueTuple<TroopRosterElement, float>(troopRosterElement, (float)(troopRosterElement.Number - troopRosterElement.WoundedNumber)));
					}
					TroopRosterElement troopRosterElement2 = MBRandom.ChooseWeighted<TroopRosterElement>(list);
					randomElement.MemberRoster.AddToCounts(troopRosterElement2.Character, -1, false, 0, 0, true, -1);
					i--;
					if (randomElement.MemberRoster.TotalHealthyCount == 1)
					{
						mblist.Remove(randomElement);
					}
				}
			}
			else if (num2 < numberOfMinimumBanditTroopsInHideoutMission)
			{
				int num3 = numberOfMinimumBanditTroopsInHideoutMission - num2;
				mblist.RemoveAll((MobileParty x) => x.MemberRoster.GetTroopRoster().All<TroopRosterElement>((TroopRosterElement y) => y.Number == 0 || y.Character.Culture.BanditBoss == y.Character || y.Character.IsHero));
				while (num3 > 0 && mblist.Count > 0)
				{
					MobileParty randomElement2 = mblist.GetRandomElement<MobileParty>();
					List<TroopRosterElement> troopRoster2 = randomElement2.MemberRoster.GetTroopRoster();
					List<ValueTuple<TroopRosterElement, float>> list2 = new List<ValueTuple<TroopRosterElement, float>>();
					foreach (TroopRosterElement troopRosterElement3 in troopRoster2)
					{
						list2.Add(new ValueTuple<TroopRosterElement, float>(troopRosterElement3, (float)(troopRosterElement3.Number * ((troopRosterElement3.Character.Culture.BanditBoss == troopRosterElement3.Character || troopRosterElement3.Character.IsHero) ? 0 : 1))));
					}
					TroopRosterElement troopRosterElement4 = MBRandom.ChooseWeighted<TroopRosterElement>(list2);
					randomElement2.MemberRoster.AddToCounts(troopRosterElement4.Character, 1, false, 0, 0, true, -1);
					num3--;
				}
			}
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x00133370 File Offset: 0x00131570
		private void OnTroopRosterManageDone(TroopRoster hideoutTroops, bool isDirectAssault)
		{
			this.ArrangeHideoutTroopCountsForMission();
			GameMenu.SwitchToMenu("hideout_place");
			Settlement.CurrentSettlement.Hideout.SetNextPossibleAttackTime(Campaign.Current.Models.HideoutModel.HideoutHiddenDuration);
			if (PlayerEncounter.IsActive)
			{
				PlayerEncounter.LeaveEncounter = false;
			}
			else
			{
				PlayerEncounter.Start();
				PlayerEncounter.Current.SetupFields(PartyBase.MainParty, Settlement.CurrentSettlement.Party);
			}
			if (PlayerEncounter.Battle == null)
			{
				PlayerEncounter.StartBattle();
				PlayerEncounter.Update();
			}
			if (isDirectAssault)
			{
				this.AdjustTroopCountForHideoutAssault();
			}
			Location locationWithId = Settlement.CurrentSettlement.LocationComplex.GetLocationWithId("hideout_center");
			if (isDirectAssault)
			{
				CampaignMission.OpenHideoutBattleMission(locationWithId.GetSceneName(0), (hideoutTroops != null) ? hideoutTroops.ToFlattenedRoster() : null, false);
				return;
			}
			CampaignMission.OpenHideoutAmbushMission(locationWithId.GetSceneName(0), (hideoutTroops != null) ? hideoutTroops.ToFlattenedRoster() : null, locationWithId);
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00133448 File Offset: 0x00131648
		private void AdjustTroopCountForHideoutAssault()
		{
			int num = 0;
			MapEventParty mapEventParty = null;
			foreach (MapEventParty mapEventParty2 in MapEvent.PlayerMapEvent.PartiesOnSide(BattleSideEnum.Defender))
			{
				if (mapEventParty2.Party.IsMobile)
				{
					if (mapEventParty == null)
					{
						mapEventParty = mapEventParty2;
					}
					num += mapEventParty2.Party.MemberRoster.TotalHealthyCount;
				}
			}
			if (mapEventParty != null && num < 25)
			{
				int num2 = 25 - num;
				CharacterObject banditBandit = mapEventParty.Party.Culture.BanditBandit;
				mapEventParty.Party.MemberRoster.AddToCounts(banditBandit, num2, false, 0, 0, true, -1);
			}
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x001334FC File Offset: 0x001316FC
		private bool CanChangeStatusOfTroop(CharacterObject character)
		{
			return !character.IsPlayerCharacter && !character.IsNotTransferableInHideouts;
		}

		// Token: 0x060042F8 RID: 17144 RVA: 0x00133514 File Offset: 0x00131714
		private bool game_menu_talk_to_leader_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Conversation;
			PartyBase party = Settlement.CurrentSettlement.Parties[0].Party;
			return party != null && party.LeaderHero != null && party.LeaderHero != Hero.MainHero;
		}

		// Token: 0x060042F9 RID: 17145 RVA: 0x0013355C File Offset: 0x0013175C
		private void game_menu_talk_to_leader_on_consequence(MenuCallbackArgs args)
		{
			PartyBase party = Settlement.CurrentSettlement.Parties[0].Party;
			ConversationCharacterData conversationCharacterData = new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false);
			ConversationCharacterData conversationCharacterData2 = new ConversationCharacterData(ConversationHelper.GetConversationCharacterPartyLeader(party), party, false, false, false, false, false, false);
			CampaignMission.OpenConversationMission(conversationCharacterData, conversationCharacterData2, "", "", false);
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x001335BC File Offset: 0x001317BC
		private bool game_menu_wait_until_nightfall_on_condition(MenuCallbackArgs args)
		{
			TextObject textObject;
			if (this.IsItNighttimeNow())
			{
				textObject = new TextObject("{=qr1V1Drj}Wait until morning to begin the assault", null);
			}
			else
			{
				textObject = new TextObject("{=JYH6FF35}Wait until nightfall to sneak in", null);
			}
			MBTextManager.SetTextVariable("WAIT_OPTION", textObject, false);
			args.optionLeaveType = GameMenuOption.LeaveType.Wait;
			return Settlement.CurrentSettlement.Parties.Any<MobileParty>((MobileParty t) => t != MobileParty.MainParty) && Settlement.CurrentSettlement.Hideout.NextPossibleAttackTime.IsPast;
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x00133648 File Offset: 0x00131848
		private void game_menu_wait_until_nightfall_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_wait");
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x00133654 File Offset: 0x00131854
		private void game_menu_hideout_leave_on_consequence(MenuCallbackArgs args)
		{
			if (MobileParty.MainParty.CurrentSettlement != null)
			{
				PlayerEncounter.LeaveSettlement();
			}
			PlayerEncounter.Finish(true);
			MobileParty.MainParty.SetMoveModeHold();
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x00133677 File Offset: 0x00131877
		private void game_menu_hideout_after_wait_leave_on_consequence(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("hideout_place");
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x00133683 File Offset: 0x00131883
		private void hideout_send_troops_wait_menu_on_init(MenuCallbackArgs args)
		{
			args.MenuContext.SetBackgroundMeshName(Settlement.CurrentSettlement.Hideout.WaitMeshName);
			this.UpdateSendTroopsToClearProgress(args);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x001336A6 File Offset: 0x001318A6
		private void hideout_send_troops_wait_menu_tick(MenuCallbackArgs args, CampaignTime campaignTime)
		{
			this._hideoutSendTroopsWaitProgressHour += (float)campaignTime.ToHours;
			this.UpdateSendTroopsToClearProgress(args);
			if (args.MenuContext.GameMenu.Progress >= 1f)
			{
				this.ApplySendTroopsResults();
			}
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x001336E1 File Offset: 0x001318E1
		private void ApplySendTroopsResults()
		{
			if (this.GetHideoutCanBeSuccessfullyClearedWithSendTroops(Settlement.CurrentSettlement.Hideout))
			{
				GameMenu.SwitchToMenu("hideout_send_troops_result_success");
				return;
			}
			GameMenu.SwitchToMenu("hideout_send_troops_result_failure");
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x0013370C File Offset: 0x0013190C
		private bool GetHideoutCanBeSuccessfullyClearedWithSendTroops(Hideout hideout)
		{
			return hideout.Settlement.RandomFloatWithSeed((uint)CampaignTime.Now.ToHours) < Campaign.Current.Models.HideoutModel.GetSendTroopsSuccessChance(hideout);
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x00133749 File Offset: 0x00131949
		private void UpdateSendTroopsToClearProgress(MenuCallbackArgs args)
		{
			args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(this._hideoutSendTroopsWaitProgressHour / 6f);
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00133767 File Offset: 0x00131967
		private void hideout_send_troops_wait_leave_on_consequence(MenuCallbackArgs args)
		{
			PlayerEncounter.Finish(true);
		}

		// Token: 0x040013E1 RID: 5089
		private const int HideoutClearRelationEffect = 2;

		// Token: 0x040013E2 RID: 5090
		private const int HideoutLootTargetValueMultiplier = 135;

		// Token: 0x040013E3 RID: 5091
		private int _minimumHideoutLootTargetValue = 350;

		// Token: 0x040013E4 RID: 5092
		private const int MaximumHideoutLootTargetValue = 4750;

		// Token: 0x040013E5 RID: 5093
		private const int MaximumHideoutExtraLootTypeCount = 5;

		// Token: 0x040013E6 RID: 5094
		private const float HideoutSendTroopsWaitTargetHour = 6f;

		// Token: 0x040013E7 RID: 5095
		private float _hideoutWaitProgressHours;

		// Token: 0x040013E8 RID: 5096
		private float _hideoutWaitTargetHours;

		// Token: 0x040013E9 RID: 5097
		private float _hideoutSendTroopsWaitProgressHour;

		// Token: 0x040013EA RID: 5098
		private List<ItemObject> _potentialLootItems = new List<ItemObject>();
	}
}
