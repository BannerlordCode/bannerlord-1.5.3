using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Diamond.Lobby;
using TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.HostGame.HostGameOptions;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.CustomGame
{
	// Token: 0x02000062 RID: 98
	public class MPCustomGameVM : ViewModel
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600094A RID: 2378 RVA: 0x0001D454 File Offset: 0x0001B654
		// (remove) Token: 0x0600094B RID: 2379 RVA: 0x0001D488 File Offset: 0x0001B688
		public static event Action<bool> OnMapCheckingStateChanged;

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0001D4BB File Offset: 0x0001B6BB
		public static bool IsPingInfoAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0001D4C0 File Offset: 0x0001B6C0
		public MPCustomGameVM(LobbyState lobbyState, MPCustomGameVM.CustomGameMode customGameMode)
		{
			this._lobbyState = lobbyState;
			this._currentCustomGameList = new List<GameServerEntry>();
			this._customGameMode = customGameMode;
			this.HostGame = new MPHostGameVM(this._lobbyState, this._customGameMode);
			this.FiltersData = new MPCustomGameFiltersVM();
			this.GameList = new MBBindingList<MPCustomGameItemVM>();
			this.SortController = new MPCustomGameSortControllerVM(ref this._gameList, this._customGameMode);
			this.CustomServerActionsList = new MBBindingList<StringPairItemWithActionVM>();
			this._currentCustomGameList = new List<GameServerEntry>();
			if (customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this._lobbyState.RegisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.OnCustomServerActionRequested));
			}
			else
			{
				this._lobbyState.RegisterForPremadeServerAction(new Func<PremadeGameEntry, List<PremadeServerAction>>(this.OnPremadeServerActionRequested));
			}
			this.UpdateCanJoinOfficialServersAsAdmin();
			this.InitializeCallbacks();
			this.RefreshValues();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0001D58C File Offset: 0x0001B78C
		private async void UpdateCanJoinOfficialServersAsAdmin()
		{
			Badge[] array = await NetworkMain.GameClient.GetPlayerBadges();
			this._canJoinOfficialServersAsAdmin = array.Any<Badge>((Badge b) => b.StringId == "badge_official_server_admin");
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0001D5C5 File Offset: 0x0001B7C5
		private void InitializeCallbacks()
		{
			MPCustomGameFiltersVM filtersData = this.FiltersData;
			filtersData.OnFiltersApplied = (Action)Delegate.Combine(filtersData.OnFiltersApplied, new Action(this.RefreshFiltersAndSort));
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0001D5EE File Offset: 0x0001B7EE
		private void FinalizeCallbacks()
		{
			MPCustomGameFiltersVM filtersData = this.FiltersData;
			filtersData.OnFiltersApplied = (Action)Delegate.Remove(filtersData.OnFiltersApplied, new Action(this.RefreshFiltersAndSort));
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0001D618 File Offset: 0x0001B818
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.IsPasswordProtectedHint = new HintViewModel(new TextObject("{=dMdmyb3Y}Password Protected", null), null);
			this.CreateServerText = new TextObject("{=gzdNEM76}Create a Game", null).ToString();
			this.CloseText = new TextObject("{=6MQaCah5}Join a Game", null).ToString();
			this.RefreshText = new TextObject("{=qFPBhVh4}Refresh", null).ToString();
			this.JoinText = new TextObject("{=lWDq0Uss}JOIN", null).ToString();
			this.PasswordText = new TextObject("{=8nJFaJio}Password", null).ToString();
			this.ServerNameText = new TextObject("{=OVcoYxj1}Server Name", null).ToString();
			this.GameTypeText = new TextObject("{=JPimShCw}Game Type", null).ToString();
			this.MapText = new TextObject("{=w9m11T1y}Map", null).ToString();
			this.PlayerCountText = new TextObject("{=RfXJdNye}Players", null).ToString();
			this.PingText = new TextObject("{=7qySRF2T}Ping", null).ToString();
			this.FirstFactionText = new TextObject("{=FhnKJODX}Faction A", null).ToString();
			this.SecondFactionText = new TextObject("{=a9TcHtVw}Faction B", null).ToString();
			this.RegionText = new TextObject("{=uoVKchoC}Region", null).ToString();
			this.PremadeMatchTypeText = new TextObject("{=OzifZbSB}Match Type", null).ToString();
			this.HostText = new TextObject("{=2baWg4Gq}Host", null).ToString();
			this.GameList.ApplyActionOnAllItems(delegate(MPCustomGameItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
			this.FiltersData.RefreshValues();
			MPHostGameVM hostGame = this.HostGame;
			if (hostGame == null)
			{
				return;
			}
			hostGame.RefreshValues();
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0001D7DC File Offset: 0x0001B9DC
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._lobbyState != null)
			{
				this._lobbyState.UnregisterForCustomServerAction(new Func<GameServerEntry, List<CustomServerAction>>(this.OnCustomServerActionRequested));
				this._lobbyState.UnregisterForPremadeServerAction(new Func<PremadeGameEntry, List<PremadeServerAction>>(this.OnPremadeServerActionRequested));
			}
			InputKeyItemVM refreshInputKey = this.RefreshInputKey;
			if (refreshInputKey != null)
			{
				refreshInputKey.OnFinalize();
			}
			this.FinalizeCallbacks();
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0001D83C File Offset: 0x0001BA3C
		public void OnTick(float dt)
		{
			for (int i = 0; i < this.GameList.Count; i++)
			{
				this.GameList[i].UpdateIsFavorite();
			}
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0001D870 File Offset: 0x0001BA70
		public void SetPremadeGameList(PremadeGameEntry[] entries)
		{
			this.OnGameSelected(null);
			this.GameList.Clear();
			if (entries != null)
			{
				foreach (PremadeGameEntry premadeGameEntry in entries)
				{
					this.GameList.Add(new MPCustomGameItemVM(premadeGameEntry, new Action<MPCustomGameItemVM>(this.OnJoinGame)));
				}
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0001D8C1 File Offset: 0x0001BAC1
		public void SetCustomGameServerList(AvailableCustomGames availableCustomGames)
		{
			this.OnGameSelected(null);
			this._currentCustomGameList = availableCustomGames.CustomGameServerInfos;
			this.RefreshFiltersAndSort();
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001D8DC File Offset: 0x0001BADC
		private void RefreshFiltersAndSort()
		{
			this.OnGameSelected(null);
			this.GameList.Clear();
			List<GameServerEntry> list;
			if (this._isSpectateMode)
			{
				list = new List<GameServerEntry>(this._currentCustomGameList);
			}
			else
			{
				list = this.FiltersData.GetFilteredServerList(this._currentCustomGameList);
			}
			bool? hasCrossplayPrivilege = this._lobbyState.HasCrossplayPrivilege;
			bool flag = true;
			GameServerEntry.FilterGameServerEntriesBasedOnCrossplay(ref list, (hasCrossplayPrivilege.GetValueOrDefault() == flag) & (hasCrossplayPrivilege != null));
			foreach (GameServerEntry gameServerEntry in list)
			{
				this.GameList.Add(new MPCustomGameItemVM(gameServerEntry, new Action<MPCustomGameItemVM>(this.OnGameSelected), new Action<MPCustomGameItemVM>(this.OnJoinGame), new Action<MPCustomGameItemVM>(this.OnShowActionsForEntry), new Action<MPCustomGameItemVM>(this.OnToggleFavoriteServer)));
			}
			this.SortController.SortByCurrentState();
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0001D9D4 File Offset: 0x0001BBD4
		public async Task ExecuteAutoWatchGame()
		{
			this.IsSearchingGamesToSpectate = true;
			while (this.IsRefreshing)
			{
				await Task.Delay(10);
			}
			this._isSpectateMode = true;
			this.RefreshFiltersAndSort();
			this.IsRefreshing = true;
			try
			{
				await NetworkMain.GameClient.GetCustomGameServerList();
			}
			finally
			{
				this.IsRefreshing = false;
			}
			List<GameServerEntry> list = new List<GameServerEntry>(this._currentCustomGameList);
			bool? hasCrossplayPrivilege = this._lobbyState.HasCrossplayPrivilege;
			bool flag = true;
			GameServerEntry.FilterGameServerEntriesBasedOnCrossplay(ref list, (hasCrossplayPrivilege.GetValueOrDefault() == flag) & (hasCrossplayPrivilege != null));
			List<GameServerEntry> list2 = (from s in list
				where s.EnableSpectators && !s.PasswordProtected && (s.MaxSpectatorCount == 0 || s.SpectatorCount < s.MaxSpectatorCount)
				orderby s.PlayerCount descending
				select s).ToList<GameServerEntry>();
			if (list2.Count == 0)
			{
				this._isSpectateMode = false;
				this.RefreshFiltersAndSort();
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=V0zs1LfD}Watch Game", null).ToString(), new TextObject("{=IVrZ9Ua5}No spectatable games found. Browse the list to join one manually.", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				this.IsSearchingGamesToSpectate = false;
			}
			else
			{
				this.IsSearchingGamesToSpectate = false;
				GameServerEntry gameServerEntry = list2[0];
				this.PromptToJoinCustomGameAsSpectator(gameServerEntry);
			}
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0001DA1C File Offset: 0x0001BC1C
		private void PromptToJoinCustomGameAsSpectator(GameServerEntry selectedServer)
		{
			if (selectedServer.SpectatorPasswordProtected || selectedServer.PasswordProtected)
			{
				InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=V0zs1LfD}Watch Game", null).ToString(), new TextObject("{=qcttRnOI}Enter Spectator Password (leave blank if none)", null).ToString(), true, true, new TextObject("{=2SGyFsSa}Watch", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate(string passwordInput)
				{
					this.JoinCustomGame(selectedServer, CustomGameJoinType.Spectator, passwordInput);
				}, null, true, null, "", ""), false, false);
				return;
			}
			this.JoinCustomGame(selectedServer, CustomGameJoinType.Spectator, "");
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0001DAD4 File Offset: 0x0001BCD4
		public async void ExecuteRefresh()
		{
			if (this.IsEnabled)
			{
				if (this.IsRefreshing)
				{
					Debug.FailedAssert("Trying to refresh game list but list is already being refreshed", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameVM.cs", "ExecuteRefresh", 288);
				}
				else
				{
					this._isSpectateMode = false;
					this.IsRefreshing = true;
					this.OnGameSelected(null);
					this.GameList.Clear();
					Task task = null;
					if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
					{
						task = NetworkMain.GameClient.GetCustomGameServerList();
						MultiplayerOptions.Instance.CurrentOptionsCategory = MultiplayerOptions.OptionsCategory.Default;
					}
					else if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
					{
						task = NetworkMain.GameClient.GetPremadeGameList();
						MultiplayerOptions.Instance.CurrentOptionsCategory = MultiplayerOptions.OptionsCategory.PremadeMatch;
					}
					if (task != null)
					{
						DateTime refreshBeginTime = DateTime.Now;
						await Task.WhenAny(new Task[]
						{
							task,
							Task.Delay(10000)
						});
						TimeSpan timeSpan = DateTime.Now - refreshBeginTime;
						if (timeSpan.TotalSeconds < 3.0)
						{
							await Task.Delay((int)((3.0 - timeSpan.TotalSeconds) * 1000.0));
						}
					}
					MultiplayerOptions.Instance.OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					foreach (GenericHostGameOptionDataVM genericHostGameOptionDataVM in this.HostGame.HostGameOptions.GeneralOptions)
					{
						MultipleSelectionHostGameOptionDataVM multipleSelectionHostGameOptionDataVM = genericHostGameOptionDataVM as MultipleSelectionHostGameOptionDataVM;
						if (multipleSelectionHostGameOptionDataVM != null)
						{
							multipleSelectionHostGameOptionDataVM.RefreshList();
						}
					}
					this.IsRefreshing = false;
				}
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0001DB10 File Offset: 0x0001BD10
		private void OnShowActionsForEntry(MPCustomGameItemVM serverVM)
		{
			if (((serverVM != null) ? serverVM.GameServerInfo : null) != null)
			{
				this.CustomServerActionsList.Clear();
				List<CustomServerAction> actionsForCustomServer = this._lobbyState.GetActionsForCustomServer(serverVM.GameServerInfo);
				if (actionsForCustomServer.Count > 0)
				{
					for (int i = 0; i < actionsForCustomServer.Count; i++)
					{
						CustomServerAction customServerAction = actionsForCustomServer[i];
						this.CustomServerActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteSelectCustomServerAction), customServerAction.Name, customServerAction.Name, customServerAction));
					}
				}
				if (this.CustomServerActionsList.Count > 0)
				{
					this.IsCustomServerActionsActive = false;
					this.IsCustomServerActionsActive = true;
					return;
				}
			}
			else if (((serverVM != null) ? serverVM.PremadeGameInfo : null) != null)
			{
				this.CustomServerActionsList.Clear();
				List<PremadeServerAction> actionsForPremadeServer = this._lobbyState.GetActionsForPremadeServer(serverVM.PremadeGameInfo);
				if (actionsForPremadeServer.Count > 0)
				{
					for (int j = 0; j < actionsForPremadeServer.Count; j++)
					{
						PremadeServerAction premadeServerAction = actionsForPremadeServer[j];
						this.CustomServerActionsList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteSelectPremadeServerAction), premadeServerAction.Name, premadeServerAction.Name, premadeServerAction));
					}
				}
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0001DC35 File Offset: 0x0001BE35
		private void OnGameSelected(MPCustomGameItemVM gameItem)
		{
			if (this.SelectedGame != null)
			{
				this.SelectedGame.IsSelected = false;
			}
			this.SelectedGame = gameItem;
			if (this.SelectedGame != null)
			{
				this.SelectedGame.IsSelected = true;
			}
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0001DC66 File Offset: 0x0001BE66
		public void ExecuteJoinSelectedGame()
		{
			if (this.IsJoinEnabled)
			{
				this.OnJoinGame(this.SelectedGame);
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0001DC7C File Offset: 0x0001BE7C
		public void ExecuteWatchSelectedGame()
		{
			if (this.IsSelectedGameWatchable)
			{
				MPCustomGameItemVM selectedGame = this.SelectedGame;
				if (((selectedGame != null) ? selectedGame.GameServerInfo : null) != null)
				{
					this.PromptToJoinCustomGameAsSpectator(this.SelectedGame.GameServerInfo);
				}
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001DCAC File Offset: 0x0001BEAC
		public void OnJoinGame(MPCustomGameItemVM gameItem)
		{
			if (gameItem == null)
			{
				Debug.FailedAssert("Server to join is null.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameVM.cs", "OnJoinGame", 410);
				return;
			}
			if (gameItem.IsPasswordProtected)
			{
				string text = GameTexts.FindText("str_password_required", null).ToString();
				string text2 = GameTexts.FindText("str_enter_password", null).ToString();
				string text3 = GameTexts.FindText("str_ok", null).ToString();
				string text4 = GameTexts.FindText("str_cancel", null).ToString();
				InformationManager.ShowTextInquiry(new TextInquiryData(text, text2, true, true, text3, text4, this.GetOnTryPasswordForServerAction(gameItem), null, true, null, "", ""), false, false);
				return;
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				this.JoinCustomGame(gameItem.GameServerInfo, CustomGameJoinType.Player, "");
				return;
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				this.JoinPremadeGame(gameItem.PremadeGameInfo, "");
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001DD7C File Offset: 0x0001BF7C
		private void OnToggleFavoriteServer(MPCustomGameItemVM gameItem)
		{
			GameServerEntry gameServerInfo = gameItem.GameServerInfo;
			FavoriteServerData favoriteServerData;
			if (MultiplayerLocalDataManager.Instance.FavoriteServers.TryGetServerData(gameServerInfo, out favoriteServerData))
			{
				MultiplayerLocalDataManager.Instance.FavoriteServers.RemoveEntry(favoriteServerData);
				return;
			}
			FavoriteServerData favoriteServerData2 = FavoriteServerData.CreateFrom(gameServerInfo);
			MultiplayerLocalDataManager.Instance.FavoriteServers.AddEntry(favoriteServerData2);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001DDCC File Offset: 0x0001BFCC
		private Action<string> GetOnTryPasswordForServerAction(MPCustomGameItemVM serverItem)
		{
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
			{
				GameServerEntry serverInfo2 = serverItem.GameServerInfo;
				return delegate(string passwordInput)
				{
					this.JoinCustomGame(serverInfo2, CustomGameJoinType.Player, passwordInput);
				};
			}
			if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
			{
				PremadeGameEntry serverInfo = serverItem.PremadeGameInfo;
				return delegate(string passwordInput)
				{
					this.JoinPremadeGame(serverInfo, passwordInput);
				};
			}
			return delegate(string _)
			{
				Debug.FailedAssert("Fell through game modes, should never happen", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Lobby\\CustomGame\\MPCustomGameVM.cs", "GetOnTryPasswordForServerAction", 464);
			};
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001DE54 File Offset: 0x0001C054
		private List<CustomServerAction> OnCustomServerActionRequested(GameServerEntry serverEntry)
		{
			List<CustomServerAction> list = new List<CustomServerAction>();
			if (false || !serverEntry.IsOfficial)
			{
				Action<string> <>9__1;
				CustomServerAction customServerAction = new CustomServerAction(delegate
				{
					string text = new TextObject("{=FzG3CmEe}Join as Admin", null).ToString();
					string text2 = new TextObject("{=MNXyaVCT}Enter Admin Password", null).ToString();
					bool flag = true;
					bool flag2 = true;
					string text3 = new TextObject("{=es0Y3Bxc}Join", null).ToString();
					string text4 = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
					Action<string> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(string passwordInput)
						{
							this.JoinCustomGame(serverEntry, CustomGameJoinType.Admin, passwordInput);
						});
					}
					InformationManager.ShowTextInquiry(new TextInquiryData(text, text2, flag, flag2, text3, text4, action, null, true, null, "", ""), false, false);
				}, serverEntry, new TextObject("{=FzG3CmEe}Join as Admin", null).ToString());
				list.Add(customServerAction);
			}
			if (serverEntry.EnableSpectators)
			{
				CustomServerAction customServerAction2 = new CustomServerAction(delegate
				{
					this.PromptToJoinCustomGameAsSpectator(serverEntry);
				}, serverEntry, new TextObject("{=V0zs1LfD}Watch Game", null).ToString());
				list.Add(customServerAction2);
			}
			return list;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001DEF8 File Offset: 0x0001C0F8
		private List<PremadeServerAction> OnPremadeServerActionRequested(PremadeGameEntry serverEntry)
		{
			List<PremadeServerAction> list = new List<PremadeServerAction>();
			Action<string> <>9__1;
			PremadeServerAction premadeServerAction = new PremadeServerAction(delegate
			{
				if (serverEntry.IsSpectatorPasswordProtected || serverEntry.IsPasswordProtected)
				{
					string text = new TextObject("{=V0zs1LfD}Watch Game", null).ToString();
					string text2 = new TextObject("{=qcttRnOI}Enter Spectator Password (leave blank if none)", null).ToString();
					bool flag = true;
					bool flag2 = true;
					string text3 = new TextObject("{=2SGyFsSa}Watch", null).ToString();
					string text4 = new TextObject("{=3CpNUnVl}Cancel", null).ToString();
					Action<string> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(string passwordInput)
						{
							this.JoinPremadeGame(serverEntry, passwordInput);
						});
					}
					InformationManager.ShowTextInquiry(new TextInquiryData(text, text2, flag, flag2, text3, text4, action, null, true, null, "", ""), false, false);
					return;
				}
				this.JoinPremadeGame(serverEntry, "");
			}, serverEntry, new TextObject("{=V0zs1LfD}Watch Game", null).ToString());
			list.Add(premadeServerAction);
			return list;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0001DF50 File Offset: 0x0001C150
		private async void JoinCustomGame(GameServerEntry selectedServer, CustomGameJoinType joinType, string passwordInput = "")
		{
			Action<bool> onMapCheckingStateChanged = MPCustomGameVM.OnMapCheckingStateChanged;
			if (onMapCheckingStateChanged != null)
			{
				onMapCheckingStateChanged(true);
			}
			ValueTuple<bool, string> valueTuple = await MapCheckHelpers.CheckMaps(selectedServer);
			Action<bool> onMapCheckingStateChanged2 = MPCustomGameVM.OnMapCheckingStateChanged;
			if (onMapCheckingStateChanged2 != null)
			{
				onMapCheckingStateChanged2(false);
			}
			if (valueTuple.Item1)
			{
				this._lobbyState.OnClientRefusedToJoinCustomServer(selectedServer);
				string text = new TextObject("{=sVVaMyvb}You don't have at least one map ({MAP_NAME}) being played on the server or the local map is not identical. Download all missing maps from the server if you would like to join it.", null).SetTextVariable("MAP_NAME", valueTuple.Item2).ToString();
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_couldnt_join_server", null).ToString(), text, false, true, "", GameTexts.FindText("str_dismiss", null).ToString(), null, null, "", 0f, null, null, null), false, false);
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.RequestJoinCustomGame(selectedServer.Id, joinType, passwordInput).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					InformationManager.ShowInquiry(new InquiryData("", GameTexts.FindText("str_couldnt_join_server", null).ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", null, null, "", 0f, null, null, null), false, false);
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0001DFA1 File Offset: 0x0001C1A1
		private void JoinPremadeGame(PremadeGameEntry selectedGame, string passwordInput = "")
		{
			NetworkMain.GameClient.RequestToJoinPremadeGame(selectedGame.Id, passwordInput);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		private void ExecuteSelectCustomServerAction(object actionParam)
		{
			(actionParam as CustomServerAction).Execute();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001DFC6 File Offset: 0x0001C1C6
		public void ExecuteOpenCreateGamePanel()
		{
			if (this.CanPlayerCreateGame)
			{
				this.IsCreateGamePanelActive = true;
			}
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001DFD7 File Offset: 0x0001C1D7
		public void ExecuteCloseCreateGamePanel()
		{
			this.IsCreateGamePanelActive = false;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
		private void UpdateCanPlayerCreateGame()
		{
			this.CanPlayerCreateGame = (this.IsPlayerBasedCustomBattleEnabled || this.IsPremadeGameEnabled) && (this.IsPartyLeader || !this.IsInParty);
			if (!this.CanPlayerCreateGame)
			{
				this.IsCreateGamePanelActive = false;
			}
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0001E020 File Offset: 0x0001C220
		private void UpdateIsJoinEnabled()
		{
			this.IsJoinEnabled = this.IsAnyGameSelected && (this.IsPartyLeader || !this.IsInParty);
			this.IsSelectedGameWatchable = this.IsAnyGameSelected && this._selectedGame != null && this._selectedGame.EnableSpectators;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0001E076 File Offset: 0x0001C276
		private void ExecuteSelectPremadeServerAction(object actionParam)
		{
			(actionParam as PremadeServerAction).Execute();
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0001E088 File Offset: 0x0001C288
		public void SetRefreshInputKey(HotKey hotKey)
		{
			this.RefreshInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0001E097 File Offset: 0x0001C297
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x0001E09F File Offset: 0x0001C29F
		public InputKeyItemVM RefreshInputKey
		{
			get
			{
				return this._refreshInputKey;
			}
			set
			{
				if (value != this._refreshInputKey)
				{
					this._refreshInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RefreshInputKey");
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0001E0BD File Offset: 0x0001C2BD
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0001E0C5 File Offset: 0x0001C2C5
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					if (this.IsEnabled && !this.IsRefreshing)
					{
						this.ExecuteRefresh();
					}
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0001E0F9 File Offset: 0x0001C2F9
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x0001E101 File Offset: 0x0001C301
		[DataSourceProperty]
		public bool IsAnyGameSelected
		{
			get
			{
				return this._isAnyGameSelected;
			}
			set
			{
				if (value != this._isAnyGameSelected)
				{
					this._isAnyGameSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyGameSelected");
					this.UpdateIsJoinEnabled();
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0001E125 File Offset: 0x0001C325
		// (set) Token: 0x06000973 RID: 2419 RVA: 0x0001E12D File Offset: 0x0001C32D
		[DataSourceProperty]
		public bool IsCreateGamePanelActive
		{
			get
			{
				return this._isCreateGamePanelActive;
			}
			set
			{
				if (value != this._isCreateGamePanelActive)
				{
					this._isCreateGamePanelActive = value;
					base.OnPropertyChangedWithValue(value, "IsCreateGamePanelActive");
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0001E14B File Offset: 0x0001C34B
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0001E153 File Offset: 0x0001C353
		[DataSourceProperty]
		public bool CanPlayerCreateGame
		{
			get
			{
				return this._canPlayerCreateGame;
			}
			set
			{
				if (value != this._canPlayerCreateGame)
				{
					this._canPlayerCreateGame = value;
					base.OnPropertyChangedWithValue(value, "CanPlayerCreateGame");
				}
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0001E171 File Offset: 0x0001C371
		// (set) Token: 0x06000977 RID: 2423 RVA: 0x0001E179 File Offset: 0x0001C379
		[DataSourceProperty]
		public bool IsJoinEnabled
		{
			get
			{
				return this._isJoinEnabled;
			}
			set
			{
				if (value != this._isJoinEnabled)
				{
					this._isJoinEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsJoinEnabled");
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0001E197 File Offset: 0x0001C397
		// (set) Token: 0x06000979 RID: 2425 RVA: 0x0001E19F File Offset: 0x0001C39F
		[DataSourceProperty]
		public bool IsSelectedGameWatchable
		{
			get
			{
				return this._isSelectedGameWatchable;
			}
			set
			{
				if (value != this._isSelectedGameWatchable)
				{
					this._isSelectedGameWatchable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectedGameWatchable");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0001E1BD File Offset: 0x0001C3BD
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x0001E1C5 File Offset: 0x0001C3C5
		[DataSourceProperty]
		public MPCustomGameItemVM SelectedGame
		{
			get
			{
				return this._selectedGame;
			}
			set
			{
				if (value != this._selectedGame)
				{
					this._selectedGame = value;
					base.OnPropertyChangedWithValue<MPCustomGameItemVM>(value, "SelectedGame");
					this.IsAnyGameSelected = this._selectedGame != null;
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0001E1F2 File Offset: 0x0001C3F2
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0001E1FA File Offset: 0x0001C3FA
		[DataSourceProperty]
		public MPCustomGameFiltersVM FiltersData
		{
			get
			{
				return this._filtersData;
			}
			set
			{
				if (value != this._filtersData)
				{
					this._filtersData = value;
					base.OnPropertyChangedWithValue<MPCustomGameFiltersVM>(value, "FiltersData");
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0001E218 File Offset: 0x0001C418
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0001E220 File Offset: 0x0001C420
		[DataSourceProperty]
		public MPHostGameVM HostGame
		{
			get
			{
				return this._hostGame;
			}
			set
			{
				if (value != this._hostGame)
				{
					this._hostGame = value;
					base.OnPropertyChangedWithValue<MPHostGameVM>(value, "HostGame");
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0001E23E File Offset: 0x0001C43E
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0001E246 File Offset: 0x0001C446
		[DataSourceProperty]
		public MPCustomGameSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<MPCustomGameSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0001E264 File Offset: 0x0001C464
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0001E26C File Offset: 0x0001C46C
		[DataSourceProperty]
		public MBBindingList<MPCustomGameItemVM> GameList
		{
			get
			{
				return this._gameList;
			}
			set
			{
				if (value != this._gameList)
				{
					this._gameList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPCustomGameItemVM>>(value, "GameList");
				}
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0001E28A File Offset: 0x0001C48A
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x0001E292 File Offset: 0x0001C492
		[DataSourceProperty]
		public HintViewModel IsPasswordProtectedHint
		{
			get
			{
				return this._isPasswordProtectedHint;
			}
			set
			{
				if (value != this._isPasswordProtectedHint)
				{
					this._isPasswordProtectedHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "IsPasswordProtectedHint");
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0001E2B0 File Offset: 0x0001C4B0
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0001E2B8 File Offset: 0x0001C4B8
		[DataSourceProperty]
		public bool IsRefreshing
		{
			get
			{
				return this._isRefreshing;
			}
			set
			{
				if (value != this._isRefreshing)
				{
					this._isRefreshing = value;
					base.OnPropertyChangedWithValue(value, "IsRefreshing");
				}
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0001E2D6 File Offset: 0x0001C4D6
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x0001E2DE File Offset: 0x0001C4DE
		[DataSourceProperty]
		public bool IsSearchingGamesToSpectate
		{
			get
			{
				return this._isSearchingGamesToSpectate;
			}
			set
			{
				if (value != this._isSearchingGamesToSpectate)
				{
					this._isSearchingGamesToSpectate = value;
					base.OnPropertyChangedWithValue(value, "IsSearchingGamesToSpectate");
				}
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		// (set) Token: 0x0600098B RID: 2443 RVA: 0x0001E304 File Offset: 0x0001C504
		[DataSourceProperty]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (value != this._isPartyLeader)
				{
					this._isPartyLeader = value;
					base.OnPropertyChangedWithValue(value, "IsPartyLeader");
					this.UpdateCanPlayerCreateGame();
					this.UpdateIsJoinEnabled();
				}
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0001E32E File Offset: 0x0001C52E
		// (set) Token: 0x0600098D RID: 2445 RVA: 0x0001E336 File Offset: 0x0001C536
		[DataSourceProperty]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (value != this._isInParty)
				{
					this._isInParty = value;
					base.OnPropertyChangedWithValue(value, "IsInParty");
					this.UpdateCanPlayerCreateGame();
					this.UpdateIsJoinEnabled();
				}
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001E360 File Offset: 0x0001C560
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x0001E368 File Offset: 0x0001C568
		[DataSourceProperty]
		public string CreateServerText
		{
			get
			{
				return this._createServerText;
			}
			set
			{
				if (value != this._createServerText)
				{
					this._createServerText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateServerText");
				}
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001E38B File Offset: 0x0001C58B
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x0001E393 File Offset: 0x0001C593
		[DataSourceProperty]
		public bool IsCustomServerActionsActive
		{
			get
			{
				return this._isCustomServerActionsActive;
			}
			set
			{
				if (value != this._isCustomServerActionsActive)
				{
					this._isCustomServerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsCustomServerActionsActive");
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0001E3B1 File Offset: 0x0001C5B1
		// (set) Token: 0x06000993 RID: 2451 RVA: 0x0001E3B9 File Offset: 0x0001C5B9
		[DataSourceProperty]
		public string CloseText
		{
			get
			{
				return this._closeText;
			}
			set
			{
				if (value != this._closeText)
				{
					this._closeText = value;
					base.OnPropertyChangedWithValue<string>(value, "CloseText");
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001E3DC File Offset: 0x0001C5DC
		// (set) Token: 0x06000995 RID: 2453 RVA: 0x0001E3E4 File Offset: 0x0001C5E4
		[DataSourceProperty]
		public string RefreshText
		{
			get
			{
				return this._refreshText;
			}
			set
			{
				if (value != this._refreshText)
				{
					this._refreshText = value;
					base.OnPropertyChangedWithValue<string>(value, "RefreshText");
				}
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0001E407 File Offset: 0x0001C607
		// (set) Token: 0x06000997 RID: 2455 RVA: 0x0001E40F File Offset: 0x0001C60F
		[DataSourceProperty]
		public string JoinText
		{
			get
			{
				return this._joinText;
			}
			set
			{
				if (value != this._joinText)
				{
					this._joinText = value;
					base.OnPropertyChangedWithValue<string>(value, "JoinText");
				}
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000998 RID: 2456 RVA: 0x0001E432 File Offset: 0x0001C632
		// (set) Token: 0x06000999 RID: 2457 RVA: 0x0001E43A File Offset: 0x0001C63A
		[DataSourceProperty]
		public string ServerNameText
		{
			get
			{
				return this._serverNameText;
			}
			set
			{
				if (value != this._serverNameText)
				{
					this._serverNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "ServerNameText");
				}
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0001E45D File Offset: 0x0001C65D
		// (set) Token: 0x0600099B RID: 2459 RVA: 0x0001E465 File Offset: 0x0001C665
		[DataSourceProperty]
		public string GameTypeText
		{
			get
			{
				return this._gameTypeText;
			}
			set
			{
				if (value != this._gameTypeText)
				{
					this._gameTypeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameTypeText");
				}
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x0001E488 File Offset: 0x0001C688
		// (set) Token: 0x0600099D RID: 2461 RVA: 0x0001E490 File Offset: 0x0001C690
		[DataSourceProperty]
		public string MapText
		{
			get
			{
				return this._mapText;
			}
			set
			{
				if (value != this._mapText)
				{
					this._mapText = value;
					base.OnPropertyChangedWithValue<string>(value, "MapText");
				}
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x0001E4B3 File Offset: 0x0001C6B3
		// (set) Token: 0x0600099F RID: 2463 RVA: 0x0001E4BB File Offset: 0x0001C6BB
		[DataSourceProperty]
		public string PlayerCountText
		{
			get
			{
				return this._playerCountText;
			}
			set
			{
				if (value != this._playerCountText)
				{
					this._playerCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayerCountText");
				}
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0001E4DE File Offset: 0x0001C6DE
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x0001E4E6 File Offset: 0x0001C6E6
		[DataSourceProperty]
		public string PingText
		{
			get
			{
				return this._pingText;
			}
			set
			{
				if (value != this._pingText)
				{
					this._pingText = value;
					base.OnPropertyChangedWithValue<string>(value, "PingText");
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0001E509 File Offset: 0x0001C709
		// (set) Token: 0x060009A3 RID: 2467 RVA: 0x0001E511 File Offset: 0x0001C711
		[DataSourceProperty]
		public string PasswordText
		{
			get
			{
				return this._passwordText;
			}
			set
			{
				if (value != this._passwordText)
				{
					this._passwordText = value;
					base.OnPropertyChangedWithValue<string>(value, "PasswordText");
				}
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060009A4 RID: 2468 RVA: 0x0001E534 File Offset: 0x0001C734
		// (set) Token: 0x060009A5 RID: 2469 RVA: 0x0001E53C File Offset: 0x0001C73C
		[DataSourceProperty]
		public string FirstFactionText
		{
			get
			{
				return this._firstFactionText;
			}
			set
			{
				if (value != this._firstFactionText)
				{
					this._firstFactionText = value;
					base.OnPropertyChanged("FirstFactionText");
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0001E55E File Offset: 0x0001C75E
		// (set) Token: 0x060009A7 RID: 2471 RVA: 0x0001E566 File Offset: 0x0001C766
		[DataSourceProperty]
		public string SecondFactionText
		{
			get
			{
				return this._secondFactionText;
			}
			set
			{
				if (value != this._secondFactionText)
				{
					this._secondFactionText = value;
					base.OnPropertyChanged("SecondFactionText");
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060009A8 RID: 2472 RVA: 0x0001E588 File Offset: 0x0001C788
		// (set) Token: 0x060009A9 RID: 2473 RVA: 0x0001E590 File Offset: 0x0001C790
		[DataSourceProperty]
		public string RegionText
		{
			get
			{
				return this._regionText;
			}
			set
			{
				if (value != this._regionText)
				{
					this._regionText = value;
					base.OnPropertyChanged("RegionText");
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x0001E5B2 File Offset: 0x0001C7B2
		// (set) Token: 0x060009AB RID: 2475 RVA: 0x0001E5BA File Offset: 0x0001C7BA
		[DataSourceProperty]
		public string PremadeMatchTypeText
		{
			get
			{
				return this._premadeMatchTypeText;
			}
			set
			{
				if (value != this._premadeMatchTypeText)
				{
					this._premadeMatchTypeText = value;
					base.OnPropertyChanged("PremadeMatchTypeText");
				}
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0001E5DC File Offset: 0x0001C7DC
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x0001E5E4 File Offset: 0x0001C7E4
		[DataSourceProperty]
		public string HostText
		{
			get
			{
				return this._hostText;
			}
			set
			{
				if (value != this._hostText)
				{
					this._hostText = value;
					base.OnPropertyChanged("HostText");
				}
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x0001E606 File Offset: 0x0001C806
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x0001E610 File Offset: 0x0001C810
		[DataSourceProperty]
		public bool IsPlayerBasedCustomBattleEnabled
		{
			get
			{
				return this._isPlayerBasedCustomBattleEnabled;
			}
			set
			{
				if (this._customGameMode == MPCustomGameVM.CustomGameMode.CustomServer)
				{
					this.CreateServerText = (value ? new TextObject("{=gzdNEM76}Create a Game", null).ToString() : new TextObject("{=LrE2cUnG}Currently Disabled", null).ToString());
					if (value != this._isPlayerBasedCustomBattleEnabled)
					{
						this._isPlayerBasedCustomBattleEnabled = value;
						base.OnPropertyChangedWithValue(value, "IsPlayerBasedCustomBattleEnabled");
						this.UpdateCanPlayerCreateGame();
					}
				}
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0001E672 File Offset: 0x0001C872
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x0001E67C File Offset: 0x0001C87C
		public bool IsPremadeGameEnabled
		{
			get
			{
				return this._isPremadeGameEnabled;
			}
			set
			{
				if (this._customGameMode == MPCustomGameVM.CustomGameMode.PremadeGame)
				{
					this.CreateServerText = (value ? new TextObject("{=gzdNEM76}Create a Game", null).ToString() : new TextObject("{=LrE2cUnG}Currently Disabled", null).ToString());
					if (value != this._isPremadeGameEnabled)
					{
						this._isPremadeGameEnabled = value;
						base.OnPropertyChangedWithValue(value, "IsPremadeGameEnabled");
						this.UpdateCanPlayerCreateGame();
					}
				}
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x0001E6DF File Offset: 0x0001C8DF
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x0001E6E7 File Offset: 0x0001C8E7
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> CustomServerActionsList
		{
			get
			{
				return this._customServerActionsList;
			}
			set
			{
				if (value != this._customServerActionsList)
				{
					this._customServerActionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "CustomServerActionsList");
				}
			}
		}

		// Token: 0x0400044D RID: 1101
		private readonly LobbyState _lobbyState;

		// Token: 0x0400044E RID: 1102
		private List<GameServerEntry> _currentCustomGameList;

		// Token: 0x0400044F RID: 1103
		private MPCustomGameVM.CustomGameMode _customGameMode;

		// Token: 0x04000450 RID: 1104
		private bool _isSpectateMode;

		// Token: 0x04000451 RID: 1105
		private bool _canJoinOfficialServersAsAdmin;

		// Token: 0x04000452 RID: 1106
		private const string _officialServerAdminBadgeName = "badge_official_server_admin";

		// Token: 0x04000453 RID: 1107
		private InputKeyItemVM _refreshInputKey;

		// Token: 0x04000454 RID: 1108
		private bool _isEnabled;

		// Token: 0x04000455 RID: 1109
		private bool _isRefreshing;

		// Token: 0x04000456 RID: 1110
		private bool _isSearchingGamesToSpectate;

		// Token: 0x04000457 RID: 1111
		private bool _isPlayerBasedCustomBattleEnabled;

		// Token: 0x04000458 RID: 1112
		private bool _isPremadeGameEnabled;

		// Token: 0x04000459 RID: 1113
		private bool _isInParty;

		// Token: 0x0400045A RID: 1114
		private bool _isPartyLeader;

		// Token: 0x0400045B RID: 1115
		private bool _isCustomServerActionsActive;

		// Token: 0x0400045C RID: 1116
		private bool _isAnyGameSelected;

		// Token: 0x0400045D RID: 1117
		private bool _isCreateGamePanelActive;

		// Token: 0x0400045E RID: 1118
		private bool _canPlayerCreateGame;

		// Token: 0x0400045F RID: 1119
		private bool _isJoinEnabled;

		// Token: 0x04000460 RID: 1120
		private bool _isSelectedGameWatchable;

		// Token: 0x04000461 RID: 1121
		private MPCustomGameItemVM _selectedGame;

		// Token: 0x04000462 RID: 1122
		private MPCustomGameFiltersVM _filtersData;

		// Token: 0x04000463 RID: 1123
		private MPHostGameVM _hostGame;

		// Token: 0x04000464 RID: 1124
		private MPCustomGameSortControllerVM _sortController;

		// Token: 0x04000465 RID: 1125
		private MBBindingList<MPCustomGameItemVM> _gameList;

		// Token: 0x04000466 RID: 1126
		private MBBindingList<StringPairItemWithActionVM> _customServerActionsList;

		// Token: 0x04000467 RID: 1127
		private string _createServerText;

		// Token: 0x04000468 RID: 1128
		private string _closeText;

		// Token: 0x04000469 RID: 1129
		private string _refreshText;

		// Token: 0x0400046A RID: 1130
		private string _joinText;

		// Token: 0x0400046B RID: 1131
		private string _serverNameText;

		// Token: 0x0400046C RID: 1132
		private string _gameTypeText;

		// Token: 0x0400046D RID: 1133
		private string _mapText;

		// Token: 0x0400046E RID: 1134
		private string _playerCountText;

		// Token: 0x0400046F RID: 1135
		private string _pingText;

		// Token: 0x04000470 RID: 1136
		private string _passwordText;

		// Token: 0x04000471 RID: 1137
		private string _firstFactionText;

		// Token: 0x04000472 RID: 1138
		private string _secondFactionText;

		// Token: 0x04000473 RID: 1139
		private string _regionText;

		// Token: 0x04000474 RID: 1140
		private string _premadeMatchTypeText;

		// Token: 0x04000475 RID: 1141
		private string _hostText;

		// Token: 0x04000476 RID: 1142
		private HintViewModel _isPasswordProtectedHint;

		// Token: 0x02000141 RID: 321
		public enum CustomGameMode
		{
			// Token: 0x040009D3 RID: 2515
			CustomServer,
			// Token: 0x040009D4 RID: 2516
			PremadeGame
		}
	}
}
