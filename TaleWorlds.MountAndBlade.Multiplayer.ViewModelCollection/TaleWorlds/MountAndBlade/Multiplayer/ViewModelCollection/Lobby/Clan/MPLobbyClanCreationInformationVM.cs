using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Clan
{
	// Token: 0x02000069 RID: 105
	public class MPLobbyClanCreationInformationVM : ViewModel
	{
		// Token: 0x06000A1E RID: 2590 RVA: 0x0001F5E1 File Offset: 0x0001D7E1
		public MPLobbyClanCreationInformationVM(Action openClanCreationPopup)
		{
			this._openClanCreationPopup = openClanCreationPopup;
			this.PartyMembers = new MBBindingList<MPLobbyClanMemberItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001F604 File Offset: 0x0001D804
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CloseText = GameTexts.FindText("str_close", null).ToString();
			this.CreateClanText = new TextObject("{=ECb8IPbA}Create Clan", null).ToString();
			this.CreateClanDescriptionText = new TextObject("{=aWzdkfvn}Currently you are not a member of a clan or you don't own a clan. You need to create a party from non-clan member players to form your own clan.", null).ToString();
			this.CreateYourClanText = new TextObject("{=kF3b8cH1}Create Your Clan", null).ToString();
			this.DontHaveEnoughPlayersInPartyText = new TextObject("{=bynNUfSr}Your party does not have enough members to create a clan.", null).ToString();
			this.PlayerText = new TextObject("{=RN6zHak0}Player", null).ToString();
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001F69C File Offset: 0x0001D89C
		public void RefreshWith(ClanHomeInfo info)
		{
			if (info == null)
			{
				this.CanCreateClan = false;
				this.CantCreateHint = new HintViewModel(new TextObject("{=EQAjujjO}Clan creation information can't be retrieved", null), null);
				this.DoesHaveEnoughPlayersToCreateClan = false;
				this.PartyMemberCountText = new TextObject("{=y1AGNqyV}Clan creation is not available", null).ToString();
				this.PartyMembers.Clear();
				this.PartyMembers.Add(new MPLobbyClanMemberItemVM(NetworkMain.GameClient.PlayerID));
				return;
			}
			this.CanCreateClan = info.CanCreateClan && NetworkMain.GameClient.IsPartyLeader;
			this.CantCreateHint = new HintViewModel();
			if (!NetworkMain.GameClient.IsPartyLeader)
			{
				this.CantCreateHint = new HintViewModel(new TextObject("{=OiWquyWY}You have to be the leader of the party to create a clan", null), null);
			}
			if (info.NotEnoughPlayersInfo == null)
			{
				this.DoesHaveEnoughPlayersToCreateClan = true;
			}
			else
			{
				this.CurrentPlayerCount = info.NotEnoughPlayersInfo.CurrentPlayerCount;
				this.RequiredPlayerCount = info.NotEnoughPlayersInfo.RequiredPlayerCount;
				this.DoesHaveEnoughPlayersToCreateClan = this.CurrentPlayerCount == this.RequiredPlayerCount;
				GameTexts.SetVariable("LEFT", this.CurrentPlayerCount);
				GameTexts.SetVariable("RIGHT", this.RequiredPlayerCount);
				GameTexts.SetVariable("STR1", GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString());
				GameTexts.SetVariable("STR2", this.PlayerText);
				this.PartyMemberCountText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			}
			this.PartyMembers.Clear();
			if (NetworkMain.GameClient.IsInParty)
			{
				using (List<PartyPlayerInLobbyClient>.Enumerator enumerator = NetworkMain.GameClient.PlayersInParty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PartyPlayerInLobbyClient partyPlayerInLobbyClient = enumerator.Current;
						MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = new MPLobbyClanMemberItemVM(partyPlayerInLobbyClient.PlayerId);
						if (info.PlayerNotEligibleInfos != null)
						{
							foreach (PlayerNotEligibleInfo playerNotEligibleInfo in info.PlayerNotEligibleInfos)
							{
								if (playerNotEligibleInfo.PlayerId == partyPlayerInLobbyClient.PlayerId)
								{
									foreach (PlayerNotEligibleError playerNotEligibleError in playerNotEligibleInfo.Errors)
									{
										mplobbyClanMemberItemVM.SetNotEligibleInfo(playerNotEligibleError);
									}
								}
							}
						}
						this.PartyMembers.Add(mplobbyClanMemberItemVM);
					}
					return;
				}
			}
			this.PartyMembers.Add(new MPLobbyClanMemberItemVM(NetworkMain.GameClient.PlayerID));
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001F8FC File Offset: 0x0001DAFC
		public void OnFriendListUpdated(bool forceUpdate = false)
		{
			foreach (MPLobbyClanMemberItemVM mplobbyClanMemberItemVM in this.PartyMembers)
			{
				mplobbyClanMemberItemVM.UpdateNameAndAvatar(forceUpdate);
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001F948 File Offset: 0x0001DB48
		public void OnPlayerNameUpdated()
		{
			for (int i = 0; i < this.PartyMembers.Count; i++)
			{
				MPLobbyClanMemberItemVM mplobbyClanMemberItemVM = this.PartyMembers[i];
				if (mplobbyClanMemberItemVM.Id == NetworkMain.GameClient.PlayerID)
				{
					mplobbyClanMemberItemVM.UpdateNameAndAvatar(true);
				}
			}
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001F996 File Offset: 0x0001DB96
		public void ExecuteOpenPopup()
		{
			this.IsEnabled = true;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001F99F File Offset: 0x0001DB9F
		public void ExecuteClosePopup()
		{
			this.IsEnabled = false;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0001F9A8 File Offset: 0x0001DBA8
		private void ExecuteOpenClanCreationPopup()
		{
			this.ExecuteClosePopup();
			this._openClanCreationPopup();
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0001F9BB File Offset: 0x0001DBBB
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x0001F9C3 File Offset: 0x0001DBC3
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

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0001F9E0 File Offset: 0x0001DBE0
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x0001F9E8 File Offset: 0x0001DBE8
		[DataSourceProperty]
		public bool CanCreateClan
		{
			get
			{
				return this._canCreateClan;
			}
			set
			{
				if (value != this._canCreateClan)
				{
					this._canCreateClan = value;
					base.OnPropertyChanged("CanCreateClan");
				}
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0001FA05 File Offset: 0x0001DC05
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x0001FA0D File Offset: 0x0001DC0D
		[DataSourceProperty]
		public bool DoesHaveEnoughPlayersToCreateClan
		{
			get
			{
				return this._doesHaveEnoughPlayersToCreateClan;
			}
			set
			{
				if (value != this._doesHaveEnoughPlayersToCreateClan)
				{
					this._doesHaveEnoughPlayersToCreateClan = value;
					base.OnPropertyChanged("DoesHaveEnoughPlayersToCreateClan");
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0001FA2A File Offset: 0x0001DC2A
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x0001FA32 File Offset: 0x0001DC32
		[DataSourceProperty]
		public int CurrentPlayerCount
		{
			get
			{
				return this._currentPlayerCount;
			}
			set
			{
				if (value != this._currentPlayerCount)
				{
					this._currentPlayerCount = value;
					base.OnPropertyChanged("CurrentPlayerCount");
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0001FA4F File Offset: 0x0001DC4F
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x0001FA57 File Offset: 0x0001DC57
		[DataSourceProperty]
		public int RequiredPlayerCount
		{
			get
			{
				return this._requiredPlayerCount;
			}
			set
			{
				if (value != this._requiredPlayerCount)
				{
					this._requiredPlayerCount = value;
					base.OnPropertyChanged("RequiredPlayerCount");
				}
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0001FA74 File Offset: 0x0001DC74
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x0001FA7C File Offset: 0x0001DC7C
		[DataSourceProperty]
		public string CreateClanText
		{
			get
			{
				return this._createClanText;
			}
			set
			{
				if (value != this._createClanText)
				{
					this._createClanText = value;
					base.OnPropertyChanged("CreateClanText");
				}
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x0001FA9E File Offset: 0x0001DC9E
		// (set) Token: 0x06000A33 RID: 2611 RVA: 0x0001FAA6 File Offset: 0x0001DCA6
		[DataSourceProperty]
		public string CreateClanDescriptionText
		{
			get
			{
				return this._createClanDescriptionText;
			}
			set
			{
				if (value != this._createClanDescriptionText)
				{
					this._createClanDescriptionText = value;
					base.OnPropertyChanged("CreateClanDescriptionText");
				}
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x0001FAD0 File Offset: 0x0001DCD0
		[DataSourceProperty]
		public string DontHaveEnoughPlayersInPartyText
		{
			get
			{
				return this._dontHaveEnoughPlayersInPartyText;
			}
			set
			{
				if (value != this._dontHaveEnoughPlayersInPartyText)
				{
					this._dontHaveEnoughPlayersInPartyText = value;
					base.OnPropertyChanged("DontHaveEnoughPlayersInPartyText");
				}
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0001FAF2 File Offset: 0x0001DCF2
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x0001FAFA File Offset: 0x0001DCFA
		[DataSourceProperty]
		public string PartyMemberCountText
		{
			get
			{
				return this._partyMemberCountText;
			}
			set
			{
				if (value != this._partyMemberCountText)
				{
					this._partyMemberCountText = value;
					base.OnPropertyChanged("PartyMemberCountText");
				}
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0001FB1C File Offset: 0x0001DD1C
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x0001FB24 File Offset: 0x0001DD24
		[DataSourceProperty]
		public string PlayerText
		{
			get
			{
				return this._playerText;
			}
			set
			{
				if (value != this._playerText)
				{
					this._playerText = value;
					base.OnPropertyChanged("PlayerText");
				}
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0001FB46 File Offset: 0x0001DD46
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x0001FB4E File Offset: 0x0001DD4E
		[DataSourceProperty]
		public string CreateYourClanText
		{
			get
			{
				return this._createYourClanText;
			}
			set
			{
				if (value != this._createYourClanText)
				{
					this._createYourClanText = value;
					base.OnPropertyChanged("CreateYourClanText");
				}
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0001FB70 File Offset: 0x0001DD70
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x0001FB78 File Offset: 0x0001DD78
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

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0001FB9B File Offset: 0x0001DD9B
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x0001FBA3 File Offset: 0x0001DDA3
		[DataSourceProperty]
		public MBBindingList<MPLobbyClanMemberItemVM> PartyMembers
		{
			get
			{
				return this._partyMembers;
			}
			set
			{
				if (value != this._partyMembers)
				{
					this._partyMembers = value;
					base.OnPropertyChanged("PartyMembers");
				}
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0001FBC0 File Offset: 0x0001DDC0
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0001FBC8 File Offset: 0x0001DDC8
		[DataSourceProperty]
		public HintViewModel CantCreateHint
		{
			get
			{
				return this._cantCreateHint;
			}
			set
			{
				if (value != this._cantCreateHint)
				{
					this._cantCreateHint = value;
					base.OnPropertyChanged("CantCreateHint");
				}
			}
		}

		// Token: 0x040004A2 RID: 1186
		private Action _openClanCreationPopup;

		// Token: 0x040004A3 RID: 1187
		private bool _isEnabled;

		// Token: 0x040004A4 RID: 1188
		private bool _canCreateClan;

		// Token: 0x040004A5 RID: 1189
		private bool _doesHaveEnoughPlayersToCreateClan;

		// Token: 0x040004A6 RID: 1190
		private int _currentPlayerCount;

		// Token: 0x040004A7 RID: 1191
		private int _requiredPlayerCount;

		// Token: 0x040004A8 RID: 1192
		private string _createClanText;

		// Token: 0x040004A9 RID: 1193
		private string _createClanDescriptionText;

		// Token: 0x040004AA RID: 1194
		private string _dontHaveEnoughPlayersInPartyText;

		// Token: 0x040004AB RID: 1195
		private string _partyMemberCountText;

		// Token: 0x040004AC RID: 1196
		private string _playerText;

		// Token: 0x040004AD RID: 1197
		private string _createYourClanText;

		// Token: 0x040004AE RID: 1198
		private string _closeText;

		// Token: 0x040004AF RID: 1199
		private MBBindingList<MPLobbyClanMemberItemVM> _partyMembers;

		// Token: 0x040004B0 RID: 1200
		private HintViewModel _cantCreateHint;
	}
}
