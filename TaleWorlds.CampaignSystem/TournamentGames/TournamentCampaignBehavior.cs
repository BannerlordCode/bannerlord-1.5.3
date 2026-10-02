using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.TournamentGames
{
	// Token: 0x020002E7 RID: 743
	public class TournamentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x0600285E RID: 10334 RVA: 0x000A8190 File Offset: 0x000A6390
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.DailyTickSettlement));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
			CampaignEvents.HeroKilledEvent.AddNonSerializedListener(this, new Action<Hero, Hero, KillCharacterAction.KillCharacterActionDetail, bool>(this.OnHeroKilled));
			CampaignEvents.TournamentFinished.AddNonSerializedListener(this, new Action<CharacterObject, MBReadOnlyList<CharacterObject>, Town, ItemObject>(this.OnTournamentFinished));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.TownRebelliosStateChanged.AddNonSerializedListener(this, new Action<Town, bool>(this.OnTownRebelliousStateChanged));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventStarted));
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000A826C File Offset: 0x000A646C
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			Campaign.Current.TournamentManager.InitializeLeaderboardEntry(Hero.MainHero, 0);
			this.InitializeTournamentLeaderboard();
			for (int i = 0; i < 3; i++)
			{
				foreach (Town town in Town.AllTowns)
				{
					if (town.IsTown)
					{
						this.ConsiderStartOrEndTournament(town);
					}
				}
			}
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000A82F0 File Offset: 0x000A64F0
		private void OnDailyTick()
		{
			Hero leaderBoardLeader = Campaign.Current.TournamentManager.GetLeaderBoardLeader();
			if (leaderBoardLeader != null && leaderBoardLeader.IsAlive && leaderBoardLeader.Clan != null)
			{
				leaderBoardLeader.Clan.AddRenown(1f, true);
			}
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000A8334 File Offset: 0x000A6534
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			foreach (Town town in Town.AllTowns)
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(town);
				if (tournamentGame != null && tournamentGame.Prize != null && (tournamentGame.Prize == DefaultItems.Trash || !tournamentGame.Prize.IsReady))
				{
					tournamentGame.UpdateTournamentPrize(false, true);
				}
			}
			foreach (KeyValuePair<Town, CampaignTime> keyValuePair in this._lastCreatedTournamentDatesInTowns.ToList<KeyValuePair<Town, CampaignTime>>())
			{
				if (keyValuePair.Value.ElapsedDaysUntilNow >= 15f)
				{
					this._lastCreatedTournamentDatesInTowns.Remove(keyValuePair.Key);
				}
			}
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000A8428 File Offset: 0x000A6628
		private void OnTownRebelliousStateChanged(Town town, bool rebelliousState)
		{
			if (town.InRebelliousState)
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(town);
				if (tournamentGame != null)
				{
					Campaign.Current.TournamentManager.ResolveTournament(tournamentGame, town);
				}
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000A8464 File Offset: 0x000A6664
		private void OnSiegeEventStarted(SiegeEvent siegeEvent)
		{
			Town town = siegeEvent.BesiegedSettlement.Town;
			if (town != null)
			{
				TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(town);
				if (tournamentGame != null)
				{
					Campaign.Current.TournamentManager.ResolveTournament(tournamentGame, town);
				}
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000A84A5 File Offset: 0x000A66A5
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Town, CampaignTime>>("_lastCreatedTournamentTimesInTowns", ref this._lastCreatedTournamentDatesInTowns);
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000A84B9 File Offset: 0x000A66B9
		private void OnHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
		{
			Campaign.Current.TournamentManager.DeleteLeaderboardEntry(victim);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000A84CB File Offset: 0x000A66CB
		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddDialogs(campaignGameStarter);
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x000A84DB File Offset: 0x000A66DB
		private void DailyTickSettlement(Settlement settlement)
		{
			if (settlement.IsTown)
			{
				this.ConsiderStartOrEndTournament(settlement.Town);
			}
		}

		// Token: 0x06002868 RID: 10344 RVA: 0x000A84F4 File Offset: 0x000A66F4
		private void ConsiderStartOrEndTournament(Town town)
		{
			CampaignTime campaignTime;
			if (!this._lastCreatedTournamentDatesInTowns.TryGetValue(town, out campaignTime) || campaignTime.ElapsedDaysUntilNow >= 15f)
			{
				ITournamentManager tournamentManager = Campaign.Current.TournamentManager;
				TournamentGame tournamentGame = tournamentManager.GetTournamentGame(town);
				if (tournamentGame != null && tournamentGame.CreationTime.ElapsedDaysUntilNow >= (float)tournamentGame.RemoveTournamentAfterDays)
				{
					tournamentManager.ResolveTournament(tournamentGame, town);
				}
				if (tournamentGame == null)
				{
					if (MBRandom.RandomFloat < Campaign.Current.Models.TournamentModel.GetTournamentStartChance(town))
					{
						tournamentManager.AddTournament(Campaign.Current.Models.TournamentModel.CreateTournament(town));
						if (!this._lastCreatedTournamentDatesInTowns.ContainsKey(town))
						{
							this._lastCreatedTournamentDatesInTowns.Add(town, CampaignTime.Now);
							return;
						}
						this._lastCreatedTournamentDatesInTowns[town] = CampaignTime.Now;
						return;
					}
				}
				else if (tournamentGame.CreationTime.ElapsedDaysUntilNow < (float)tournamentGame.RemoveTournamentAfterDays && MBRandom.RandomFloat < Campaign.Current.Models.TournamentModel.GetTournamentEndChance(tournamentGame))
				{
					tournamentManager.ResolveTournament(tournamentGame, town);
				}
			}
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000A8604 File Offset: 0x000A6804
		private void OnTournamentFinished(CharacterObject winner, MBReadOnlyList<CharacterObject> participants, Town town, ItemObject prize)
		{
			if (winner.IsHero && winner.HeroObject.Clan != null)
			{
				winner.HeroObject.Clan.AddRenown((float)Campaign.Current.Models.TournamentModel.GetRenownReward(winner.HeroObject, town), true);
				GainKingdomInfluenceAction.ApplyForDefault(winner.HeroObject, (float)Campaign.Current.Models.TournamentModel.GetInfluenceReward(winner.HeroObject, town));
			}
		}

		// Token: 0x0600286A RID: 10346 RVA: 0x000A867A File Offset: 0x000A687A
		private float GetTournamentSimulationScore(Hero hero)
		{
			return Campaign.Current.Models.TournamentModel.GetTournamentSimulationScore(hero.CharacterObject);
		}

		// Token: 0x0600286B RID: 10347 RVA: 0x000A8698 File Offset: 0x000A6898
		private void InitializeTournamentLeaderboard()
		{
			Hero[] array = Hero.AllAliveHeroes.Where<Hero>((Hero x) => x.IsLord && this.GetTournamentSimulationScore(x) > 1.5f).ToArray<Hero>();
			int numLeaderboardVictoriesAtGameStart = Campaign.Current.Models.TournamentModel.GetNumLeaderboardVictoriesAtGameStart();
			if (array.Length < 3)
			{
				return;
			}
			List<Hero> list = new List<Hero>();
			for (int i = 0; i < numLeaderboardVictoriesAtGameStart; i++)
			{
				list.Clear();
				for (int j = 0; j < 16; j++)
				{
					Hero hero = array[MBRandom.RandomInt(array.Length)];
					list.Add(hero);
				}
				Hero hero2 = null;
				float num = 0f;
				foreach (Hero hero3 in list)
				{
					float num2 = this.GetTournamentSimulationScore(hero3) * (0.8f + 0.2f * MBRandom.RandomFloat);
					if (num2 > num)
					{
						num = num2;
						hero2 = hero3;
					}
				}
				Campaign.Current.TournamentManager.AddLeaderboardEntry(hero2);
				hero2.Clan.AddRenown((float)Campaign.Current.Models.TournamentModel.GetRenownReward(hero2, null), true);
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000A87C8 File Offset: 0x000A69C8
		protected void AddDialogs(CampaignGameStarter campaignGameSystemStarter)
		{
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000A87CC File Offset: 0x000A69CC
		protected void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "join_tournament", "{=LN09ZLXZ}Join the tournament", new GameMenuOption.OnConditionDelegate(this.game_menu_join_tournament_on_condition), delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("menu_town_tournament_join");
			}, false, 1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "mno_tournament_event_watch", "{=6bQIRaIl}Watch the tournament", new GameMenuOption.OnConditionDelegate(this.game_menu_tournament_watch_on_condition), new GameMenuOption.OnConsequenceDelegate(this.game_menu_tournament_watch_current_game_on_consequence), false, 2, false, null);
			campaignGameSystemStarter.AddGameMenuOption("town_arena", "mno_see_tournament_leaderboard", "{=vGF5S2hE}Leaderboard", new GameMenuOption.OnConditionDelegate(TournamentCampaignBehavior.game_menu_town_arena_see_leaderboard_on_condition), null, false, 3, false, null);
			campaignGameSystemStarter.AddGameMenu("menu_town_tournament_join", "{=5Adr6toM}{MENU_TEXT}", new OnInitDelegate(this.game_menu_tournament_join_on_init), GameMenu.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.None, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_town_tournament_join", "mno_tournament_event_1", "{=es0Y3Bxc}Join", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Mission;
				return true;
			}, new GameMenuOption.OnConsequenceDelegate(this.game_menu_tournament_join_current_game_on_consequence), false, -1, false, null);
			campaignGameSystemStarter.AddGameMenuOption("menu_town_tournament_join", "mno_tournament_leave", "{=3sRdGQou}Leave", delegate(MenuCallbackArgs args)
			{
				args.optionLeaveType = GameMenuOption.LeaveType.Leave;
				return true;
			}, delegate(MenuCallbackArgs args)
			{
				GameMenu.SwitchToMenu("town_arena");
			}, true, -1, false, null);
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000A892E File Offset: 0x000A6B2E
		[GameMenuEventHandler("town_arena", "mno_see_tournament_leaderboard", GameMenuEventHandler.EventType.OnConsequence)]
		public static void game_menu_ui_town_arena_see_leaderboard_on_consequence(MenuCallbackArgs args)
		{
			args.MenuContext.OpenTournamentLeaderboards();
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000A893C File Offset: 0x000A6B3C
		private bool game_menu_join_tournament_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.JoinTournament, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.HostileAction;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000A8979 File Offset: 0x000A6B79
		private static bool game_menu_town_arena_see_leaderboard_on_condition(MenuCallbackArgs args)
		{
			args.optionLeaveType = GameMenuOption.LeaveType.Leaderboard;
			return Settlement.CurrentSettlement != null && Settlement.CurrentSettlement.IsTown;
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000A8998 File Offset: 0x000A6B98
		[GameMenuInitializationHandler("menu_town_tournament_join")]
		private static void game_menu_ui_town_ui_on_init(MenuCallbackArgs args)
		{
			Settlement currentSettlement = Settlement.CurrentSettlement;
			args.MenuContext.SetBackgroundMeshName(currentSettlement.Town.WaitMeshName);
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000A89C4 File Offset: 0x000A6BC4
		private void game_menu_tournament_join_on_init(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			tournamentGame.UpdateTournamentPrize(true, false);
			GameTexts.SetVariable("MENU_TEXT", tournamentGame.GetMenuText());
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000A8A04 File Offset: 0x000A6C04
		private void game_menu_tournament_join_current_game_on_consequence(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			GameMenu.SwitchToMenu("town");
			tournamentGame.PrepareForTournamentGame(true);
			Campaign.Current.TournamentManager.OnPlayerJoinTournament(tournamentGame.GetType(), Settlement.CurrentSettlement);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000A8A58 File Offset: 0x000A6C58
		private bool game_menu_tournament_watch_on_condition(MenuCallbackArgs args)
		{
			bool flag2;
			TextObject textObject;
			bool flag = Campaign.Current.Models.SettlementAccessModel.CanMainHeroDoSettlementAction(Settlement.CurrentSettlement, SettlementAccessModel.SettlementAction.WatchTournament, out flag2, out textObject);
			args.optionLeaveType = GameMenuOption.LeaveType.Mission;
			return MenuHelper.SetOptionProperties(args, flag, flag2, textObject);
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000A8A94 File Offset: 0x000A6C94
		private void game_menu_tournament_watch_current_game_on_consequence(MenuCallbackArgs args)
		{
			TournamentGame tournamentGame = Campaign.Current.TournamentManager.GetTournamentGame(Settlement.CurrentSettlement.Town);
			GameMenu.SwitchToMenu("town");
			tournamentGame.PrepareForTournamentGame(false);
			Campaign.Current.TournamentManager.OnPlayerWatchTournament(tournamentGame.GetType(), Settlement.CurrentSettlement);
		}

		// Token: 0x04000BC0 RID: 3008
		private const int TournamentCooldownDurationAsDays = 15;

		// Token: 0x04000BC1 RID: 3009
		private Dictionary<Town, CampaignTime> _lastCreatedTournamentDatesInTowns = new Dictionary<Town, CampaignTime>();
	}
}
