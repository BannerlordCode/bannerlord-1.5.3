using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends
{
	// Token: 0x0200005D RID: 93
	public class MPLobbyPartyJoinRequestPopupVM : ViewModel
	{
		// Token: 0x060008A9 RID: 2217 RVA: 0x0001BC4A File Offset: 0x00019E4A
		public MPLobbyPartyJoinRequestPopupVM()
		{
			this.RefreshValues();
			this.MaxAnswerDuration = 30f;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001BC63 File Offset: 0x00019E63
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=re37GzKI}Party Join Request", null).ToString();
			this.AcceptJoinRequestText = new TextObject("{=Ogr2N5bx}Accept request to join to your party?", null).ToString();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001BC98 File Offset: 0x00019E98
		public void OpenWith(PlayerId joiningPlayer, PlayerId viaPlayerId, string viaPlayerName)
		{
			this._viaPlayerId = viaPlayerId;
			this.JoiningPlayer = new MPLobbyPlayerBaseVM(joiningPlayer, "", null, null);
			this.RemainingAnswerDuration = this.MaxAnswerDuration;
			if (viaPlayerId == NetworkMain.GameClient.PlayerID)
			{
				TextObject textObject = new TextObject("{=BcEN71ts}Player wants to join your party.", null);
				this.JoiningPlayerText = textObject.ToString();
			}
			else
			{
				TextObject textObject = new TextObject("{=q3uBjUyB}Player wants to join your party through your party member <a style=\"Strong\"><b>{PLAYER_NAME}</b></a>.", null);
				GameTexts.SetVariable("PLAYER_NAME", viaPlayerName);
				this.JoiningPlayerText = textObject.ToString();
			}
			this.IsEnabled = true;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x0001BD21 File Offset: 0x00019F21
		public void OpenWithNewParty(PlayerId joiningPlayer)
		{
			this.JoiningPlayer = new MPLobbyPlayerBaseVM(joiningPlayer, "", null, null);
			this.JoiningPlayerText = "";
			this.RemainingAnswerDuration = this.MaxAnswerDuration;
			this.IsEnabled = true;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0001BD54 File Offset: 0x00019F54
		public void Close()
		{
			if (this.IsEnabled)
			{
				this.ExecuteDeclineJoinRequest();
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001BD64 File Offset: 0x00019F64
		public void OnTick(float dt)
		{
			if (this.IsEnabled)
			{
				this.RemainingAnswerDuration -= dt;
				if (this.RemainingAnswerDuration <= 0f)
				{
					this.ExecuteDeclineJoinRequest();
				}
			}
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001BD8F File Offset: 0x00019F8F
		private void ExecuteAcceptJoinRequest()
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Communication, true, delegate(bool result)
			{
				if (result)
				{
					PlatformServices.Instance.CheckPermissionWithUser(Permission.PlayMultiplayer, this.JoiningPlayer.ProvidedID, delegate(bool permissionResult)
					{
						if (permissionResult)
						{
							NetworkMain.GameClient.AcceptPartyJoinRequest(this.JoiningPlayer.ProvidedID);
						}
						else
						{
							NetworkMain.GameClient.DeclinePartyJoinRequest(this.JoiningPlayer.ProvidedID, PartyJoinDeclineReason.NoPlatformPermission);
						}
						this.IsEnabled = false;
					});
					return;
				}
				NetworkMain.GameClient.DeclinePartyJoinRequest(this.JoiningPlayer.ProvidedID, PartyJoinDeclineReason.NoPlatformPermission);
				this.IsEnabled = false;
			});
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001BDA9 File Offset: 0x00019FA9
		private void ExecuteDeclineJoinRequest()
		{
			NetworkMain.GameClient.DeclinePartyJoinRequest(this.JoiningPlayer.ProvidedID, PartyJoinDeclineReason.DeclinedByLeader);
			this.IsEnabled = false;
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0001BDC8 File Offset: 0x00019FC8
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x0001BDD0 File Offset: 0x00019FD0
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

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0001BDED File Offset: 0x00019FED
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0001BDF5 File Offset: 0x00019FF5
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

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0001BE17 File Offset: 0x0001A017
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0001BE1F File Offset: 0x0001A01F
		[DataSourceProperty]
		public string AcceptJoinRequestText
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
					base.OnPropertyChanged("AcceptJoinRequestText");
				}
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0001BE41 File Offset: 0x0001A041
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0001BE49 File Offset: 0x0001A049
		[DataSourceProperty]
		public string JoiningPlayerText
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
					base.OnPropertyChanged("JoiningPlayerText");
				}
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0001BE6B File Offset: 0x0001A06B
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x0001BE73 File Offset: 0x0001A073
		[DataSourceProperty]
		public MPLobbyPlayerBaseVM JoiningPlayer
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
					base.OnPropertyChanged("JoiningPlayer");
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0001BE90 File Offset: 0x0001A090
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0001BE98 File Offset: 0x0001A098
		[DataSourceProperty]
		public float RemainingAnswerDuration
		{
			get
			{
				return this._remainingAnswerDuration;
			}
			set
			{
				if (value != this._remainingAnswerDuration)
				{
					this._remainingAnswerDuration = value;
					base.OnPropertyChangedWithValue(value, "RemainingAnswerDuration");
				}
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x0001BEB6 File Offset: 0x0001A0B6
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0001BEBE File Offset: 0x0001A0BE
		[DataSourceProperty]
		public float MaxAnswerDuration
		{
			get
			{
				return this._maxAnswerDuration;
			}
			set
			{
				if (value != this._maxAnswerDuration)
				{
					this._maxAnswerDuration = value;
					base.OnPropertyChangedWithValue(value, "MaxAnswerDuration");
				}
			}
		}

		// Token: 0x04000404 RID: 1028
		private PlayerId _viaPlayerId;

		// Token: 0x04000405 RID: 1029
		private bool _isEnabled;

		// Token: 0x04000406 RID: 1030
		private float _remainingAnswerDuration;

		// Token: 0x04000407 RID: 1031
		private float _maxAnswerDuration;

		// Token: 0x04000408 RID: 1032
		private string _titleText;

		// Token: 0x04000409 RID: 1033
		private string _doYouWantToInviteText;

		// Token: 0x0400040A RID: 1034
		private string _playerSuggestedText;

		// Token: 0x0400040B RID: 1035
		private MPLobbyPlayerBaseVM _suggestedPlayer;
	}
}
