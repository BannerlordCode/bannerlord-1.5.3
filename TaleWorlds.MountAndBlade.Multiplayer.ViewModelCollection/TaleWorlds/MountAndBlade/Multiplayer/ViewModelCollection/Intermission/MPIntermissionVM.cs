using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Multiplayer.NetworkComponents;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Intermission
{
	// Token: 0x02000091 RID: 145
	public class MPIntermissionVM : ViewModel
	{
		// Token: 0x06000DDC RID: 3548 RVA: 0x0002A878 File Offset: 0x00028A78
		public MPIntermissionVM()
		{
			this.AvailableMaps = new MBBindingList<MPIntermissionMapItemVM>();
			this.AvailableCultures = new MBBindingList<MPIntermissionCultureItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0002A8FC File Offset: 0x00028AFC
		public override void RefreshValues()
		{
			this.QuitText = new TextObject("{=3sRdGQou}Leave", null).ToString();
			this.PlayersLabel = new TextObject("{=RfXJdNye}Players", null).ToString();
			this.MapVoteText = new TextObject("{=DraJ6bxq}Vote for the Next Map", null).ToString();
			this.CultureVoteText = new TextObject("{=oF27vprQ}Vote for the Next Culture", null).ToString();
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0002A964 File Offset: 0x00028B64
		public void Tick()
		{
			if (!this._hasBaseNetworkComponentSet)
			{
				this._baseNetworkComponent = GameNetwork.GetNetworkComponent<BaseNetworkComponent>();
				if (this._baseNetworkComponent != null)
				{
					this._hasBaseNetworkComponentSet = true;
					BaseNetworkComponent baseNetworkComponent = this._baseNetworkComponent;
					baseNetworkComponent.OnIntermissionStateUpdated = (Action)Delegate.Combine(baseNetworkComponent.OnIntermissionStateUpdated, new Action(this.OnIntermissionStateUpdated));
					return;
				}
			}
			else if (this._baseNetworkComponent.ClientIntermissionState == MultiplayerIntermissionState.Idle)
			{
				this.NextGameStateTimerLabel = this._serverIdleLabelText.ToString();
				this.NextGameStateTimerValue = string.Empty;
				this.IsMissionTimerEnabled = false;
				this.IsEndGameTimerEnabled = false;
				this.IsNextMapInfoEnabled = false;
				this.IsMapVoteEnabled = false;
				this.IsCultureVoteEnabled = false;
				this.IsPlayerCountEnabled = false;
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0002AA11 File Offset: 0x00028C11
		public override void OnFinalize()
		{
			if (this._baseNetworkComponent != null)
			{
				BaseNetworkComponent baseNetworkComponent = this._baseNetworkComponent;
				baseNetworkComponent.OnIntermissionStateUpdated = (Action)Delegate.Remove(baseNetworkComponent.OnIntermissionStateUpdated, new Action(this.OnIntermissionStateUpdated));
			}
			MultiplayerIntermissionVotingManager.Instance.ClearItems();
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0002AA4C File Offset: 0x00028C4C
		private void OnIntermissionStateUpdated()
		{
			this._currentIntermissionState = this._baseNetworkComponent.ClientIntermissionState;
			bool flag = true;
			if (this._currentIntermissionState == MultiplayerIntermissionState.CountingForMapVote)
			{
				int num = (int)this._baseNetworkComponent.CurrentIntermissionTimer;
				this.NextGameStateTimerLabel = this._voteLabelText.ToString();
				this.NextGameStateTimerValue = num.ToString();
				this.IsMissionTimerEnabled = true;
				this.IsEndGameTimerEnabled = false;
				this.IsNextMapInfoEnabled = false;
				this.IsCultureVoteEnabled = false;
				this.IsPlayerCountEnabled = true;
				flag = false;
				List<IntermissionVoteItem> mapVoteItems = MultiplayerIntermissionVotingManager.Instance.MapVoteItems;
				if (mapVoteItems.Count > 0)
				{
					this.IsMapVoteEnabled = true;
					using (List<IntermissionVoteItem>.Enumerator enumerator = mapVoteItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntermissionVoteItem mapItem = enumerator.Current;
							if (this.AvailableMaps.FirstOrDefault<MPIntermissionMapItemVM>((MPIntermissionMapItemVM m) => m.MapID == mapItem.Id) == null)
							{
								this.AvailableMaps.Add(new MPIntermissionMapItemVM(mapItem.Id, new Action<MPIntermissionMapItemVM>(this.OnPlayerVotedForMap)));
							}
							int voteCount = mapItem.VoteCount;
							this.AvailableMaps.First<MPIntermissionMapItemVM>((MPIntermissionMapItemVM m) => m.MapID == mapItem.Id).Votes = voteCount;
						}
					}
				}
			}
			if (this._baseNetworkComponent.ClientIntermissionState == MultiplayerIntermissionState.CountingForCultureVote)
			{
				int num2 = (int)this._baseNetworkComponent.CurrentIntermissionTimer;
				this.NextGameStateTimerLabel = this._voteLabelText.ToString();
				this.NextGameStateTimerValue = num2.ToString();
				this.IsMissionTimerEnabled = true;
				this.IsEndGameTimerEnabled = false;
				this.IsNextMapInfoEnabled = false;
				this.IsMapVoteEnabled = false;
				this.IsPlayerCountEnabled = true;
				flag = false;
				List<IntermissionVoteItem> cultureVoteItems = MultiplayerIntermissionVotingManager.Instance.CultureVoteItems;
				if (cultureVoteItems.Count > 0)
				{
					this.IsCultureVoteEnabled = true;
					using (List<IntermissionVoteItem>.Enumerator enumerator = cultureVoteItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntermissionVoteItem cultureItem = enumerator.Current;
							if (this.AvailableCultures.FirstOrDefault<MPIntermissionCultureItemVM>((MPIntermissionCultureItemVM c) => c.CultureCode == cultureItem.Id) == null)
							{
								this.AvailableCultures.Add(new MPIntermissionCultureItemVM(cultureItem.Id, new Action<MPIntermissionCultureItemVM>(this.OnPlayerVotedForCulture)));
							}
							int voteCount2 = cultureItem.VoteCount;
							this.AvailableCultures.FirstOrDefault<MPIntermissionCultureItemVM>((MPIntermissionCultureItemVM c) => c.CultureCode == cultureItem.Id).Votes = voteCount2;
						}
					}
				}
				string text;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.CultureTeam1, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text);
				string text2;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.CultureTeam2, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text2);
				this.NextFactionACultureID = text;
				this.NextFactionBCultureID = text2;
				BasicCultureObject basicCultureObject = (string.IsNullOrEmpty(this.NextFactionACultureID) ? null : MBObjectManager.Instance.GetObject<BasicCultureObject>(this.NextFactionACultureID));
				BasicCultureObject basicCultureObject2 = (string.IsNullOrEmpty(this.NextFactionACultureID) ? null : MBObjectManager.Instance.GetObject<BasicCultureObject>(this.NextFactionBCultureID));
				MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(basicCultureObject, basicCultureObject2);
				this.NextFactionACultureColor1 = multiplayerBattleColors.AttackerColors.Color1;
				this.NextFactionACultureColor2 = multiplayerBattleColors.AttackerColors.Color2;
				this.NextFactionBCultureColor1 = multiplayerBattleColors.DefenderColors.Color1;
				this.NextFactionBCultureColor2 = multiplayerBattleColors.DefenderColors.Color2;
			}
			if (this._currentIntermissionState == MultiplayerIntermissionState.CountingForMission)
			{
				int num3 = (int)this._baseNetworkComponent.CurrentIntermissionTimer;
				this.NextGameStateTimerLabel = this._nextGameLabelText.ToString();
				this.NextGameStateTimerValue = num3.ToString();
				this.IsMissionTimerEnabled = true;
				this.IsEndGameTimerEnabled = false;
				this.IsNextMapInfoEnabled = true;
				this.IsMapVoteEnabled = false;
				this.IsCultureVoteEnabled = false;
				this.IsPlayerCountEnabled = true;
				flag = true;
				this.AvailableMaps.Clear();
				this.AvailableCultures.Clear();
				MultiplayerIntermissionVotingManager.Instance.ClearVotes();
				this._votedMapItem = null;
				this._votedCultureItem = null;
			}
			if (this._currentIntermissionState == MultiplayerIntermissionState.CountingForEnd)
			{
				TextObject textObject = GameTexts.FindText("str_string_newline_string", null);
				textObject.SetTextVariable("STR1", this._matchFinishedText.ToString());
				textObject.SetTextVariable("STR2", this._returningToLobbyText.ToString());
				this.NextGameStateTimerLabel = textObject.ToString();
				this.NextGameStateTimerValue = string.Empty;
				this.IsMissionTimerEnabled = false;
				this.IsEndGameTimerEnabled = false;
				this.IsNextMapInfoEnabled = false;
				this.IsMapVoteEnabled = false;
				this.IsCultureVoteEnabled = false;
				this.IsPlayerCountEnabled = false;
				flag = false;
			}
			string text3;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.Map, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text3);
			this.NextMapID = (this.IsEndGameTimerEnabled ? string.Empty : text3);
			TextObject textObject2;
			string text4;
			if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject2, text3))
			{
				text4 = textObject2.ToString();
			}
			else
			{
				text4 = text3;
			}
			this.NextMapName = (this.IsEndGameTimerEnabled ? string.Empty : text4);
			if (flag)
			{
				string text5;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.CultureTeam1, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text5);
				this.IsFactionAValid = !this.IsEndGameTimerEnabled && !string.IsNullOrEmpty(text5) && this._currentIntermissionState != MultiplayerIntermissionState.CountingForMapVote;
				this.NextFactionACultureID = (this.IsEndGameTimerEnabled ? string.Empty : text5);
				BasicCultureObject basicCultureObject3 = (string.IsNullOrEmpty(this.NextFactionACultureID) ? null : MBObjectManager.Instance.GetObject<BasicCultureObject>(this.NextFactionACultureID));
				BasicCultureObject basicCultureObject4 = (string.IsNullOrEmpty(this.NextFactionACultureID) ? null : MBObjectManager.Instance.GetObject<BasicCultureObject>(this.NextFactionBCultureID));
				MultiplayerBattleColors multiplayerBattleColors2 = MultiplayerBattleColors.CreateWith(basicCultureObject3, basicCultureObject4);
				this.NextFactionACultureColor1 = multiplayerBattleColors2.AttackerColors.Color1;
				this.NextFactionACultureColor2 = multiplayerBattleColors2.AttackerColors.Color2;
				this.NextFactionBCultureColor1 = multiplayerBattleColors2.DefenderColors.Color1;
				this.NextFactionBCultureColor2 = multiplayerBattleColors2.DefenderColors.Color2;
				if (!string.IsNullOrEmpty(this.NextFactionACultureID))
				{
					this.NextFactionACultureColor1 = multiplayerBattleColors2.AttackerColors.Color1;
					this.NextFactionACultureColor2 = multiplayerBattleColors2.AttackerColors.Color2;
				}
				string text6;
				MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.CultureTeam2, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text6);
				this.IsFactionBValid = !this.IsEndGameTimerEnabled && !string.IsNullOrEmpty(text6) && this._currentIntermissionState != MultiplayerIntermissionState.CountingForMapVote;
				this.NextFactionBCultureID = (this.IsEndGameTimerEnabled ? string.Empty : text6);
				if (!string.IsNullOrEmpty(this.NextFactionBCultureID))
				{
					this.NextFactionBCultureColor1 = multiplayerBattleColors2.DefenderColors.Color1;
					this.NextFactionBCultureColor2 = multiplayerBattleColors2.DefenderColors.Color2;
				}
			}
			else
			{
				this.IsFactionAValid = false;
				this.IsFactionBValid = false;
			}
			string text7;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.ServerName, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text7);
			this.ServerName = text7;
			string text8;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.GameType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text8);
			this.NextGameType = (this.IsEndGameTimerEnabled ? string.Empty : GameTexts.FindText("str_multiplayer_game_type", text8).ToString());
			string text9;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.WelcomeMessage, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out text9);
			this.WelcomeMessage = (this.IsEndGameTimerEnabled ? string.Empty : text9);
			int num4;
			MultiplayerOptions.Instance.GetOptionFromOptionType(MultiplayerOptions.OptionType.MaxNumberOfPlayers, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions).GetValue(out num4);
			this.MaxNumPlayersValueText = num4.ToString();
			this.ConnectedPlayersCountValueText = GameNetwork.NetworkPeers.Count.ToString();
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0002B190 File Offset: 0x00029390
		public void ExecuteQuitServer()
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			if (gameClient.CurrentState == LobbyClient.State.InCustomGame)
			{
				gameClient.QuitFromCustomGame();
			}
			MultiplayerIntermissionVotingManager.Instance.ClearItems();
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0002B1C0 File Offset: 0x000293C0
		private void OnPlayerVotedForMap(MPIntermissionMapItemVM mapItem)
		{
			int num;
			if (this._votedMapItem != null)
			{
				this._baseNetworkComponent.IntermissionCastVote(this._votedMapItem.MapID, -1);
				this._votedMapItem.IsSelected = false;
				MPIntermissionMapItemVM votedMapItem = this._votedMapItem;
				num = votedMapItem.Votes;
				votedMapItem.Votes = num - 1;
			}
			this._baseNetworkComponent.IntermissionCastVote(mapItem.MapID, 1);
			this._votedMapItem = mapItem;
			this._votedMapItem.IsSelected = true;
			MPIntermissionMapItemVM votedMapItem2 = this._votedMapItem;
			num = votedMapItem2.Votes;
			votedMapItem2.Votes = num + 1;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0002B248 File Offset: 0x00029448
		private void OnPlayerVotedForCulture(MPIntermissionCultureItemVM cultureItem)
		{
			int num;
			if (this._votedCultureItem != null)
			{
				this._baseNetworkComponent.IntermissionCastVote(this._votedCultureItem.CultureCode, -1);
				this._votedCultureItem.IsSelected = false;
				MPIntermissionCultureItemVM votedCultureItem = this._votedCultureItem;
				num = votedCultureItem.Votes;
				votedCultureItem.Votes = num - 1;
			}
			this._baseNetworkComponent.IntermissionCastVote(cultureItem.CultureCode, 1);
			this._votedCultureItem = cultureItem;
			this._votedCultureItem.IsSelected = true;
			MPIntermissionCultureItemVM votedCultureItem2 = this._votedCultureItem;
			num = votedCultureItem2.Votes;
			votedCultureItem2.Votes = num + 1;
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000DE4 RID: 3556 RVA: 0x0002B2CF File Offset: 0x000294CF
		// (set) Token: 0x06000DE5 RID: 3557 RVA: 0x0002B2D7 File Offset: 0x000294D7
		[DataSourceProperty]
		public string ConnectedPlayersCountValueText
		{
			get
			{
				return this._connectedPlayersCountValueText;
			}
			set
			{
				if (value != this._connectedPlayersCountValueText)
				{
					this._connectedPlayersCountValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ConnectedPlayersCountValueText");
				}
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000DE6 RID: 3558 RVA: 0x0002B2FA File Offset: 0x000294FA
		// (set) Token: 0x06000DE7 RID: 3559 RVA: 0x0002B302 File Offset: 0x00029502
		[DataSourceProperty]
		public string MaxNumPlayersValueText
		{
			get
			{
				return this._maxNumPlayersValueText;
			}
			set
			{
				if (value != this._maxNumPlayersValueText)
				{
					this._maxNumPlayersValueText = value;
					base.OnPropertyChangedWithValue<string>(value, "MaxNumPlayersValueText");
				}
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x0002B325 File Offset: 0x00029525
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x0002B32D File Offset: 0x0002952D
		[DataSourceProperty]
		public bool IsFactionAValid
		{
			get
			{
				return this._isFactionAValid;
			}
			set
			{
				if (value != this._isFactionAValid)
				{
					this._isFactionAValid = value;
					base.OnPropertyChangedWithValue(value, "IsFactionAValid");
				}
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x0002B34B File Offset: 0x0002954B
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x0002B353 File Offset: 0x00029553
		[DataSourceProperty]
		public bool IsFactionBValid
		{
			get
			{
				return this._isFactionBValid;
			}
			set
			{
				if (value != this._isFactionBValid)
				{
					this._isFactionBValid = value;
					base.OnPropertyChangedWithValue(value, "IsFactionBValid");
				}
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x0002B371 File Offset: 0x00029571
		// (set) Token: 0x06000DED RID: 3565 RVA: 0x0002B379 File Offset: 0x00029579
		[DataSourceProperty]
		public bool IsMissionTimerEnabled
		{
			get
			{
				return this._isMissionTimerEnabled;
			}
			set
			{
				if (value != this._isMissionTimerEnabled)
				{
					this._isMissionTimerEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMissionTimerEnabled");
				}
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000DEE RID: 3566 RVA: 0x0002B397 File Offset: 0x00029597
		// (set) Token: 0x06000DEF RID: 3567 RVA: 0x0002B39F File Offset: 0x0002959F
		[DataSourceProperty]
		public bool IsEndGameTimerEnabled
		{
			get
			{
				return this._isEndGameTimerEnabled;
			}
			set
			{
				if (value != this._isEndGameTimerEnabled)
				{
					this._isEndGameTimerEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEndGameTimerEnabled");
				}
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0002B3BD File Offset: 0x000295BD
		// (set) Token: 0x06000DF1 RID: 3569 RVA: 0x0002B3C5 File Offset: 0x000295C5
		[DataSourceProperty]
		public bool IsNextMapInfoEnabled
		{
			get
			{
				return this._isNextMapInfoEnabled;
			}
			set
			{
				if (value != this._isNextMapInfoEnabled)
				{
					this._isNextMapInfoEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsNextMapInfoEnabled");
				}
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000DF2 RID: 3570 RVA: 0x0002B3E3 File Offset: 0x000295E3
		// (set) Token: 0x06000DF3 RID: 3571 RVA: 0x0002B3EB File Offset: 0x000295EB
		[DataSourceProperty]
		public bool IsMapVoteEnabled
		{
			get
			{
				return this._isMapVoteEnabled;
			}
			set
			{
				if (value != this._isMapVoteEnabled)
				{
					this._isMapVoteEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMapVoteEnabled");
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06000DF4 RID: 3572 RVA: 0x0002B409 File Offset: 0x00029609
		// (set) Token: 0x06000DF5 RID: 3573 RVA: 0x0002B411 File Offset: 0x00029611
		[DataSourceProperty]
		public bool IsCultureVoteEnabled
		{
			get
			{
				return this._isCultureVoteEnabled;
			}
			set
			{
				if (value != this._isCultureVoteEnabled)
				{
					this._isCultureVoteEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsCultureVoteEnabled");
				}
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0002B42F File Offset: 0x0002962F
		// (set) Token: 0x06000DF7 RID: 3575 RVA: 0x0002B437 File Offset: 0x00029637
		[DataSourceProperty]
		public bool IsPlayerCountEnabled
		{
			get
			{
				return this._isPlayerCountEnabled;
			}
			set
			{
				if (value != this._isPlayerCountEnabled)
				{
					this._isPlayerCountEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerCountEnabled");
				}
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0002B455 File Offset: 0x00029655
		// (set) Token: 0x06000DF9 RID: 3577 RVA: 0x0002B45D File Offset: 0x0002965D
		[DataSourceProperty]
		public string NextMapID
		{
			get
			{
				return this._nextMapId;
			}
			set
			{
				if (value != this._nextMapId)
				{
					this._nextMapId = value;
					base.OnPropertyChangedWithValue<string>(value, "NextMapID");
				}
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0002B480 File Offset: 0x00029680
		// (set) Token: 0x06000DFB RID: 3579 RVA: 0x0002B488 File Offset: 0x00029688
		[DataSourceProperty]
		public string NextFactionACultureID
		{
			get
			{
				return this._nextFactionACultureId;
			}
			set
			{
				if (value != this._nextFactionACultureId)
				{
					this._nextFactionACultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "NextFactionACultureID");
				}
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0002B4AB File Offset: 0x000296AB
		// (set) Token: 0x06000DFD RID: 3581 RVA: 0x0002B4B3 File Offset: 0x000296B3
		[DataSourceProperty]
		public Color NextFactionACultureColor1
		{
			get
			{
				return this._nextFactionACultureColor1;
			}
			set
			{
				if (value != this._nextFactionACultureColor1)
				{
					this._nextFactionACultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "NextFactionACultureColor1");
				}
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0002B4D6 File Offset: 0x000296D6
		// (set) Token: 0x06000DFF RID: 3583 RVA: 0x0002B4DE File Offset: 0x000296DE
		[DataSourceProperty]
		public Color NextFactionACultureColor2
		{
			get
			{
				return this._nextFactionACultureColor2;
			}
			set
			{
				if (value != this._nextFactionACultureColor2)
				{
					this._nextFactionACultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "NextFactionACultureColor2");
				}
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0002B501 File Offset: 0x00029701
		// (set) Token: 0x06000E01 RID: 3585 RVA: 0x0002B509 File Offset: 0x00029709
		[DataSourceProperty]
		public string NextFactionBCultureID
		{
			get
			{
				return this._nextFactionBCultureId;
			}
			set
			{
				if (value != this._nextFactionBCultureId)
				{
					this._nextFactionBCultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "NextFactionBCultureID");
				}
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0002B52C File Offset: 0x0002972C
		// (set) Token: 0x06000E03 RID: 3587 RVA: 0x0002B534 File Offset: 0x00029734
		[DataSourceProperty]
		public Color NextFactionBCultureColor1
		{
			get
			{
				return this._nextFactionBCultureColor1;
			}
			set
			{
				if (value != this._nextFactionBCultureColor1)
				{
					this._nextFactionBCultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "NextFactionBCultureColor1");
				}
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0002B557 File Offset: 0x00029757
		// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0002B55F File Offset: 0x0002975F
		[DataSourceProperty]
		public Color NextFactionBCultureColor2
		{
			get
			{
				return this._nextFactionBCultureColor2;
			}
			set
			{
				if (value != this._nextFactionBCultureColor2)
				{
					this._nextFactionBCultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "NextFactionBCultureColor2");
				}
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0002B582 File Offset: 0x00029782
		// (set) Token: 0x06000E07 RID: 3591 RVA: 0x0002B58A File Offset: 0x0002978A
		[DataSourceProperty]
		public string PlayersLabel
		{
			get
			{
				return this._playersLabel;
			}
			set
			{
				if (value != this._playersLabel)
				{
					this._playersLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "PlayersLabel");
				}
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0002B5AD File Offset: 0x000297AD
		// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0002B5B5 File Offset: 0x000297B5
		[DataSourceProperty]
		public string MapVoteText
		{
			get
			{
				return this._mapVoteText;
			}
			set
			{
				if (value != this._mapVoteText)
				{
					this._mapVoteText = value;
					base.OnPropertyChangedWithValue<string>(value, "MapVoteText");
				}
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0002B5D8 File Offset: 0x000297D8
		// (set) Token: 0x06000E0B RID: 3595 RVA: 0x0002B5E0 File Offset: 0x000297E0
		[DataSourceProperty]
		public string CultureVoteText
		{
			get
			{
				return this._cultureVoteText;
			}
			set
			{
				if (value != this._cultureVoteText)
				{
					this._cultureVoteText = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureVoteText");
				}
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0002B603 File Offset: 0x00029803
		// (set) Token: 0x06000E0D RID: 3597 RVA: 0x0002B60B File Offset: 0x0002980B
		[DataSourceProperty]
		public string NextGameStateTimerLabel
		{
			get
			{
				return this._nextGameStateTimerLabel;
			}
			set
			{
				if (value != this._nextGameStateTimerLabel)
				{
					this._nextGameStateTimerLabel = value;
					base.OnPropertyChangedWithValue<string>(value, "NextGameStateTimerLabel");
				}
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000E0E RID: 3598 RVA: 0x0002B62E File Offset: 0x0002982E
		// (set) Token: 0x06000E0F RID: 3599 RVA: 0x0002B636 File Offset: 0x00029836
		[DataSourceProperty]
		public string NextGameStateTimerValue
		{
			get
			{
				return this._nextGameStateTimerValue;
			}
			set
			{
				if (value != this._nextGameStateTimerValue)
				{
					this._nextGameStateTimerValue = value;
					base.OnPropertyChangedWithValue<string>(value, "NextGameStateTimerValue");
				}
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x0002B659 File Offset: 0x00029859
		// (set) Token: 0x06000E11 RID: 3601 RVA: 0x0002B661 File Offset: 0x00029861
		[DataSourceProperty]
		public string WelcomeMessage
		{
			get
			{
				return this._welcomeMessage;
			}
			set
			{
				if (value != this._welcomeMessage)
				{
					this._welcomeMessage = value;
					base.OnPropertyChangedWithValue<string>(value, "WelcomeMessage");
				}
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x0002B684 File Offset: 0x00029884
		// (set) Token: 0x06000E13 RID: 3603 RVA: 0x0002B68C File Offset: 0x0002988C
		[DataSourceProperty]
		public string ServerName
		{
			get
			{
				return this._serverName;
			}
			set
			{
				if (value != this._serverName)
				{
					this._serverName = value;
					base.OnPropertyChangedWithValue<string>(value, "ServerName");
				}
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x0002B6AF File Offset: 0x000298AF
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x0002B6B7 File Offset: 0x000298B7
		[DataSourceProperty]
		public string NextGameType
		{
			get
			{
				return this._nextGameType;
			}
			set
			{
				if (value != this._nextGameType)
				{
					this._nextGameType = value;
					base.OnPropertyChangedWithValue<string>(value, "NextGameType");
				}
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x0002B6DA File Offset: 0x000298DA
		// (set) Token: 0x06000E17 RID: 3607 RVA: 0x0002B6E2 File Offset: 0x000298E2
		[DataSourceProperty]
		public string NextMapName
		{
			get
			{
				return this._nextMapName;
			}
			set
			{
				if (value != this._nextMapName)
				{
					this._nextMapName = value;
					base.OnPropertyChangedWithValue<string>(value, "NextMapName");
				}
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0002B705 File Offset: 0x00029905
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x0002B70D File Offset: 0x0002990D
		[DataSourceProperty]
		public MBBindingList<MPIntermissionMapItemVM> AvailableMaps
		{
			get
			{
				return this._availableMaps;
			}
			set
			{
				if (value != this._availableMaps)
				{
					this._availableMaps = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPIntermissionMapItemVM>>(value, "AvailableMaps");
				}
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x06000E1A RID: 3610 RVA: 0x0002B72B File Offset: 0x0002992B
		// (set) Token: 0x06000E1B RID: 3611 RVA: 0x0002B733 File Offset: 0x00029933
		[DataSourceProperty]
		public MBBindingList<MPIntermissionCultureItemVM> AvailableCultures
		{
			get
			{
				return this._availableCultures;
			}
			set
			{
				if (value != this._availableCultures)
				{
					this._availableCultures = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPIntermissionCultureItemVM>>(value, "AvailableCultures");
				}
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x0002B751 File Offset: 0x00029951
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x0002B759 File Offset: 0x00029959
		[DataSourceProperty]
		public string QuitText
		{
			get
			{
				return this._quitText;
			}
			set
			{
				if (value != this._quitText)
				{
					this._quitText = value;
					base.OnPropertyChangedWithValue<string>(value, "QuitText");
				}
			}
		}

		// Token: 0x0400064C RID: 1612
		private bool _hasBaseNetworkComponentSet;

		// Token: 0x0400064D RID: 1613
		private BaseNetworkComponent _baseNetworkComponent;

		// Token: 0x0400064E RID: 1614
		private MultiplayerIntermissionState _currentIntermissionState;

		// Token: 0x0400064F RID: 1615
		private readonly TextObject _voteLabelText = new TextObject("{=KOVHgkVq}Voting Ends In:", null);

		// Token: 0x04000650 RID: 1616
		private readonly TextObject _nextGameLabelText = new TextObject("{=lX9Qx7Wo}Next Game Starts In:", null);

		// Token: 0x04000651 RID: 1617
		private readonly TextObject _serverIdleLabelText = new TextObject("{=Rhcberxf}Awaiting Server", null);

		// Token: 0x04000652 RID: 1618
		private readonly TextObject _matchFinishedText = new TextObject("{=RbazQjFt}Match is Finished", null);

		// Token: 0x04000653 RID: 1619
		private readonly TextObject _returningToLobbyText = new TextObject("{=1UaxKbn6}Returning to the Lobby...", null);

		// Token: 0x04000654 RID: 1620
		private MPIntermissionMapItemVM _votedMapItem;

		// Token: 0x04000655 RID: 1621
		private MPIntermissionCultureItemVM _votedCultureItem;

		// Token: 0x04000656 RID: 1622
		private string _connectedPlayersCountValueText;

		// Token: 0x04000657 RID: 1623
		private string _maxNumPlayersValueText;

		// Token: 0x04000658 RID: 1624
		private bool _isFactionAValid;

		// Token: 0x04000659 RID: 1625
		private bool _isFactionBValid;

		// Token: 0x0400065A RID: 1626
		private bool _isMissionTimerEnabled;

		// Token: 0x0400065B RID: 1627
		private bool _isEndGameTimerEnabled;

		// Token: 0x0400065C RID: 1628
		private bool _isNextMapInfoEnabled;

		// Token: 0x0400065D RID: 1629
		private bool _isMapVoteEnabled;

		// Token: 0x0400065E RID: 1630
		private bool _isCultureVoteEnabled;

		// Token: 0x0400065F RID: 1631
		private bool _isPlayerCountEnabled;

		// Token: 0x04000660 RID: 1632
		private string _nextMapId;

		// Token: 0x04000661 RID: 1633
		private string _nextFactionACultureId;

		// Token: 0x04000662 RID: 1634
		private string _nextFactionBCultureId;

		// Token: 0x04000663 RID: 1635
		private string _nextGameStateTimerLabel;

		// Token: 0x04000664 RID: 1636
		private string _nextGameStateTimerValue;

		// Token: 0x04000665 RID: 1637
		private string _playersLabel;

		// Token: 0x04000666 RID: 1638
		private string _mapVoteText;

		// Token: 0x04000667 RID: 1639
		private string _cultureVoteText;

		// Token: 0x04000668 RID: 1640
		private string _serverName;

		// Token: 0x04000669 RID: 1641
		private string _welcomeMessage;

		// Token: 0x0400066A RID: 1642
		private string _nextGameType;

		// Token: 0x0400066B RID: 1643
		private string _nextMapName;

		// Token: 0x0400066C RID: 1644
		private Color _nextFactionACultureColor1;

		// Token: 0x0400066D RID: 1645
		private Color _nextFactionACultureColor2;

		// Token: 0x0400066E RID: 1646
		private Color _nextFactionBCultureColor1;

		// Token: 0x0400066F RID: 1647
		private Color _nextFactionBCultureColor2;

		// Token: 0x04000670 RID: 1648
		private string _quitText;

		// Token: 0x04000671 RID: 1649
		private MBBindingList<MPIntermissionMapItemVM> _availableMaps;

		// Token: 0x04000672 RID: 1650
		private MBBindingList<MPIntermissionCultureItemVM> _availableCultures;
	}
}
