using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x02000057 RID: 87
	public class MPLobbyPartyPlayerSuggestionPopupVM : ViewModel
	{
		// Token: 0x060007C7 RID: 1991 RVA: 0x000194EB File Offset: 0x000176EB
		public MPLobbyPartyPlayerSuggestionPopupVM()
		{
			this.RefreshValues();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000194F9 File Offset: 0x000176F9
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=q2Y7aHSF}Invite Suggestion", null).ToString();
			this.DoYouWantToInviteText = new TextObject("{=VFqoa6vD}Do you want to invite this player to your party?", null).ToString();
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00019530 File Offset: 0x00017730
		public void OpenWith(MPLobbyPartyPlayerSuggestionPopupVM.PlayerPartySuggestionData data)
		{
			this._suggestedPlayerId = data.PlayerId;
			this.SuggestedPlayer = new MPLobbyPlayerBaseVM(data.PlayerId, "", null, null);
			TextObject textObject = new TextObject("{=C7OHivNl}Your friend <a style=\"Strong\"><b>{PLAYER_NAME}</b></a> wants you to invite the player below to your party.", null);
			GameTexts.SetVariable("PLAYER_NAME", data.SuggestingPlayerName);
			this.PlayerSuggestedText = textObject.ToString();
			this.IsEnabled = true;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00019590 File Offset: 0x00017790
		public void Close()
		{
			this.IsEnabled = false;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00019599 File Offset: 0x00017799
		private void ExecuteAcceptSuggestion()
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					PlatformServices.Instance.CheckPermissionWithUser(Permission.PlayMultiplayer, this._suggestedPlayerId, async delegate(bool permissionResult)
					{
						if (permissionResult)
						{
							if (PlatformServices.Instance.UsePlatformInvitationService(this._suggestedPlayerId))
							{
								await NetworkMain.GameClient.InviteToPlatformSession(this._suggestedPlayerId);
							}
							else
							{
								bool flag = BannerlordConfig.EnableGenericNames && !NetworkMain.GameClient.IsKnownPlayer(this._suggestedPlayerId);
								NetworkMain.GameClient.InviteToParty(this._suggestedPlayerId, flag);
							}
						}
						this.Close();
					});
					return;
				}
				this.Close();
			});
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000195B3 File Offset: 0x000177B3
		private void ExecuteDeclineSuggestion()
		{
			this.Close();
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x000195BB File Offset: 0x000177BB
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x000195C3 File Offset: 0x000177C3
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
					base.OnPropertyChanged("IsEnabled");
				}
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x000195E0 File Offset: 0x000177E0
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x000195E8 File Offset: 0x000177E8
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChanged("TitleText");
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0001960A File Offset: 0x0001780A
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00019612 File Offset: 0x00017812
		[DataSourceProperty]
		public string DoYouWantToInviteText
		{
			get
			{
				return this._doYouWantToInviteText;
			}
			set
			{
				if (value != this._doYouWantToInviteText)
				{
					this._doYouWantToInviteText = value;
					base.OnPropertyChanged("DoYouWantToInviteText");
				}
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x00019634 File Offset: 0x00017834
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x0001963C File Offset: 0x0001783C
		[DataSourceProperty]
		public string PlayerSuggestedText
		{
			get
			{
				return this._playerSuggestedText;
			}
			set
			{
				if (value != this._playerSuggestedText)
				{
					this._playerSuggestedText = value;
					base.OnPropertyChanged("PlayerSuggestedText");
				}
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0001965E File Offset: 0x0001785E
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x00019666 File Offset: 0x00017866
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM SuggestedPlayer
		{
			get
			{
				return this._suggestedPlayer;
			}
			set
			{
				if (value != this._suggestedPlayer)
				{
					this._suggestedPlayer = value;
					base.OnPropertyChanged("SuggestedPlayer");
				}
			}
		}

		// Token: 0x04000391 RID: 913
		private PlayerId _suggestedPlayerId;

		// Token: 0x04000392 RID: 914
		private bool _isEnabled;

		// Token: 0x04000393 RID: 915
		private string _titleText;

		// Token: 0x04000394 RID: 916
		private string _doYouWantToInviteText;

		// Token: 0x04000395 RID: 917
		private string _playerSuggestedText;

		// Token: 0x04000396 RID: 918
		private MPLobbyPlayerBaseVM _suggestedPlayer;

		// Token: 0x02000122 RID: 290
		public class PlayerPartySuggestionData
		{
			// Token: 0x170005CD RID: 1485
			// (get) Token: 0x06001285 RID: 4741 RVA: 0x0003A6FF File Offset: 0x000388FF
			// (set) Token: 0x06001286 RID: 4742 RVA: 0x0003A707 File Offset: 0x00038907
			public PlayerId PlayerId { get; private set; }

			// Token: 0x170005CE RID: 1486
			// (get) Token: 0x06001287 RID: 4743 RVA: 0x0003A710 File Offset: 0x00038910
			// (set) Token: 0x06001288 RID: 4744 RVA: 0x0003A718 File Offset: 0x00038918
			public string PlayerName { get; private set; }

			// Token: 0x170005CF RID: 1487
			// (get) Token: 0x06001289 RID: 4745 RVA: 0x0003A721 File Offset: 0x00038921
			// (set) Token: 0x0600128A RID: 4746 RVA: 0x0003A729 File Offset: 0x00038929
			public PlayerId SuggestingPlayerId { get; private set; }

			// Token: 0x170005D0 RID: 1488
			// (get) Token: 0x0600128B RID: 4747 RVA: 0x0003A732 File Offset: 0x00038932
			// (set) Token: 0x0600128C RID: 4748 RVA: 0x0003A73A File Offset: 0x0003893A
			public string SuggestingPlayerName { get; private set; }

			// Token: 0x0600128D RID: 4749 RVA: 0x0003A743 File Offset: 0x00038943
			public PlayerPartySuggestionData(PlayerId playerId, string playerName, PlayerId suggestingPlayerId, string suggestingPlayerName)
			{
				this.PlayerId = playerId;
				this.PlayerName = playerName;
				this.SuggestingPlayerId = suggestingPlayerId;
				this.SuggestingPlayerName = suggestingPlayerName;
			}
		}
	}
}
