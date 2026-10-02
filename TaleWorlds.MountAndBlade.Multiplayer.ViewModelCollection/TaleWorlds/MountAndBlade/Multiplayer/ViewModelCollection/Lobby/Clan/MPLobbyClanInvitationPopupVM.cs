using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x0200006B RID: 107
	public class MPLobbyClanInvitationPopupVM : ViewModel
	{
		// Token: 0x06000A83 RID: 2691 RVA: 0x000206D8 File Offset: 0x0001E8D8
		public MPLobbyClanInvitationPopupVM()
		{
			this.PartyMembersList = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000206F4 File Offset: 0x0001E8F4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = new TextObject("{=D9zIAw9y}Clan Invite", null).ToString();
			this.InviteReceivedText = new TextObject("{=wNAl9o4A}You received an invite from", null).ToString();
			this.WantToJoinText = new TextObject("{=qa9aOxLm}Do you want to join this clan?", null).ToString();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002074C File Offset: 0x0001E94C
		public void Open(string clanName, string clanTag, bool isCreation)
		{
			GameTexts.SetVariable("STR", clanTag);
			string text = new TextObject("{=uTXYEAOg}[{STR}]", null).ToString();
			GameTexts.SetVariable("STR1", clanName);
			GameTexts.SetVariable("STR2", text);
			this.ClanNameAndTag = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.PartyMembersList.Clear();
			this.IsCreation = isCreation;
			if (isCreation)
			{
				this._invitationMode = MPLobbyClanInvitationPopupVM.InvitationMode.Creation;
				using (List<PartyPlayerInLobbyClient>.Enumerator enumerator = NetworkMain.GameClient.PlayersInParty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PartyPlayerInLobbyClient partyPlayerInLobbyClient = enumerator.Current;
						if (partyPlayerInLobbyClient.PlayerId != NetworkMain.GameClient.PlayerID)
						{
							MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = new MPLobbyClanMemberItemVM(partyPlayerInLobbyClient.PlayerId);
							mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=c0ZdKSkn}Waiting", null).ToString();
							this.PartyMembersList.Add(mplobbyClanMemberItemVM);
						}
					}
					goto IL_00E3;
				}
			}
			this._invitationMode = MPLobbyClanInvitationPopupVM.InvitationMode.Invitation;
			IL_00E3:
			this.WithPlayersText = ((this.PartyMembersList.Count > 1) ? new TextObject("{=iCaRFZpG}along with these players", null).ToString() : string.Empty);
			this.IsEnabled = true;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00020880 File Offset: 0x0001EA80
		public void Close()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0002088C File Offset: 0x0001EA8C
		public void UpdateConfirmation(PlayerId playerId, ClanCreationAnswer answer)
		{
			foreach (MPLobbyClanMemberItemVM mplobbyClanMemberItemVM in this.PartyMembersList)
			{
				if (mplobbyClanMemberItemVM.ProvidedID == playerId)
				{
					if (answer == ClanCreationAnswer.Accepted)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=JTMegIk4}Accepted", null).ToString();
					}
					else if (answer == ClanCreationAnswer.Declined)
					{
						mplobbyClanMemberItemVM.InviteAcceptInfo = new TextObject("{=FgaORzy5}Declined", null).ToString();
					}
				}
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00020918 File Offset: 0x0001EB18
		private async void ExecuteAcceptInvitation()
		{
			if (this._invitationMode == MPLobbyClanInvitationPopupVM.InvitationMode.Creation)
			{
				NetworkMain.GameClient.AcceptClanCreationRequest();
				this.IsEnabled = false;
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = NetworkMain.GameClient.AcceptClanInvitation().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					this.IsEnabled = false;
				}
			}
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00020951 File Offset: 0x0001EB51
		private void ExecuteDeclineInvitation()
		{
			this.IsEnabled = false;
			if (this._invitationMode == MPLobbyClanInvitationPopupVM.InvitationMode.Creation)
			{
				NetworkMain.GameClient.DeclineClanCreationRequest();
				return;
			}
			NetworkMain.GameClient.DeclineClanInvitation();
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00020977 File Offset: 0x0001EB77
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0002097F File Offset: 0x0001EB7F
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

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0002099C File Offset: 0x0001EB9C
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x000209A4 File Offset: 0x0001EBA4
		[DataSourceProperty]
		public bool IsCreation
		{
			get
			{
				return this._isCreation;
			}
			set
			{
				if (value != this._isCreation)
				{
					this._isCreation = value;
					base.OnPropertyChanged("IsCreation");
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000209C1 File Offset: 0x0001EBC1
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x000209C9 File Offset: 0x0001EBC9
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

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x000209EB File Offset: 0x0001EBEB
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x000209F3 File Offset: 0x0001EBF3
		[DataSourceProperty]
		public string ClanNameAndTag
		{
			get
			{
				return this._clanNameAndTag;
			}
			set
			{
				if (value != this._clanNameAndTag)
				{
					this._clanNameAndTag = value;
					base.OnPropertyChanged("ClanNameAndTag");
				}
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00020A15 File Offset: 0x0001EC15
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x00020A1D File Offset: 0x0001EC1D
		[DataSourceProperty]
		public string InviteReceivedText
		{
			get
			{
				return this._inviteReceivedText;
			}
			set
			{
				if (value != this._inviteReceivedText)
				{
					this._inviteReceivedText = value;
					base.OnPropertyChanged("InviteReceivedText");
				}
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x00020A3F File Offset: 0x0001EC3F
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x00020A47 File Offset: 0x0001EC47
		[DataSourceProperty]
		public string WithPlayersText
		{
			get
			{
				return this._withPlayersText;
			}
			set
			{
				if (value != this._withPlayersText)
				{
					this._withPlayersText = value;
					base.OnPropertyChanged("WithPlayersText");
				}
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x00020A69 File Offset: 0x0001EC69
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x00020A71 File Offset: 0x0001EC71
		[DataSourceProperty]
		public string WantToJoinText
		{
			get
			{
				return this._wantToJoinText;
			}
			set
			{
				if (value != this._wantToJoinText)
				{
					this._wantToJoinText = value;
					base.OnPropertyChanged("WantToJoinText");
				}
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x00020A93 File Offset: 0x0001EC93
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x00020A9B File Offset: 0x0001EC9B
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> PartyMembersList
		{
			get
			{
				return this._partyMembersList;
			}
			set
			{
				if (value != this._partyMembersList)
				{
					this._partyMembersList = value;
					base.OnPropertyChanged("PartyMembersList");
				}
			}
		}

		// Token: 0x040004C8 RID: 1224
		private MPLobbyClanInvitationPopupVM.InvitationMode _invitationMode;

		// Token: 0x040004C9 RID: 1225
		private bool _isEnabled;

		// Token: 0x040004CA RID: 1226
		private bool _isCreation;

		// Token: 0x040004CB RID: 1227
		private string _titleText;

		// Token: 0x040004CC RID: 1228
		private string _clanNameAndTag;

		// Token: 0x040004CD RID: 1229
		private string _inviteReceivedText;

		// Token: 0x040004CE RID: 1230
		private string _withPlayersText;

		// Token: 0x040004CF RID: 1231
		private string _wantToJoinText;

		// Token: 0x040004D0 RID: 1232
		private MBBindingList<MPLobbyClanMemberItemVM> _partyMembersList;

		// Token: 0x02000151 RID: 337
		public enum InvitationMode
		{
			// Token: 0x04000A12 RID: 2578
			Creation,
			// Token: 0x04000A13 RID: 2579
			Invitation
		}
	}
}
