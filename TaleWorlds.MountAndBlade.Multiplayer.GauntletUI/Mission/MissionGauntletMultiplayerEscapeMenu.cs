using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000013 RID: 19
	[OverrideView(typeof(MissionMultiplayerEscapeMenu))]
	public class MissionGauntletMultiplayerEscapeMenu : MissionGauntletEscapeMenuBase
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00005CB5 File Offset: 0x00003EB5
		public MissionGauntletMultiplayerEscapeMenu(string gameType)
			: base("MultiplayerEscapeMenu")
		{
			this._gameType = gameType;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005CCC File Offset: 0x00003ECC
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._missionOptionsComponent = base.Mission.GetMissionBehavior<MissionOptionsComponent>();
			this._missionLobbyComponent = base.Mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionAdminComponent = base.Mission.GetMissionBehavior<MultiplayerAdminComponent>();
			this._missionTeamSelectComponent = base.Mission.GetMissionBehavior<MultiplayerTeamSelectComponent>();
			this._gameModeClient = base.Mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			TextObject textObject = GameTexts.FindText("str_multiplayer_game_type", this._gameType);
			this.DataSource = new MPEscapeMenuVM(null, textObject);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005D52 File Offset: 0x00003F52
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this.DataSource.Tick(dt);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005D68 File Offset: 0x00003F68
		public override bool OnEscape()
		{
			bool flag = base.OnEscape();
			if (base.IsActive)
			{
				if (this._gameModeClient.IsGameModeUsingAllowTroopChange)
				{
					this._changeTroopItem.IsDisabled = !this._gameModeClient.CanRequestTroopChange();
				}
				if (this._gameModeClient.IsGameModeUsingAllowCultureChange)
				{
					this._changeCultureItem.IsDisabled = !this._gameModeClient.CanRequestCultureChange();
				}
			}
			return flag;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005DD0 File Offset: 0x00003FD0
		protected override List<EscapeMenuItemVM> GetEscapeMenuItems()
		{
			List<EscapeMenuItemVM> list = new List<EscapeMenuItemVM>();
			list.Add(new EscapeMenuItemVM(new TextObject("{=e139gKZc}Return to the Game", null), delegate(object o)
			{
				base.OnEscapeMenuToggled(false);
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=NqarFr4P}Options", null), delegate(object o)
			{
				base.OnEscapeMenuToggled(false);
				MissionOptionsComponent missionOptionsComponent = this._missionOptionsComponent;
				if (missionOptionsComponent == null)
				{
					return;
				}
				missionOptionsComponent.OnAddOptionsUIHandler();
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			MultiplayerTeamSelectComponent missionTeamSelectComponent = this._missionTeamSelectComponent;
			if (missionTeamSelectComponent != null && missionTeamSelectComponent.TeamSelectionEnabled && !GameNetwork.MyPeer.IsSpectator)
			{
				list.Add(new EscapeMenuItemVM(new TextObject("{=2SEofGth}Change Team", null), delegate(object o)
				{
					base.OnEscapeMenuToggled(false);
					if (this._missionTeamSelectComponent != null)
					{
						this._missionTeamSelectComponent.SelectTeam();
					}
				}, null, () => new Tuple<bool, TextObject>(false, null), false));
			}
			if (this._gameModeClient.IsGameModeUsingAllowCultureChange)
			{
				this._changeCultureItem = new EscapeMenuItemVM(new TextObject("{=aGGq9lJT}Change Culture", null), delegate(object o)
				{
					base.OnEscapeMenuToggled(false);
					this._missionLobbyComponent.RequestCultureSelection();
				}, null, () => new Tuple<bool, TextObject>(false, null), false);
				list.Add(this._changeCultureItem);
			}
			if (this._gameModeClient.IsGameModeUsingAllowTroopChange)
			{
				this._changeTroopItem = new EscapeMenuItemVM(new TextObject("{=Yza0JYJt}Change Troop", null), delegate(object o)
				{
					base.OnEscapeMenuToggled(false);
					this._missionLobbyComponent.RequestTroopSelection();
				}, null, () => new Tuple<bool, TextObject>(false, null), false);
				list.Add(this._changeTroopItem);
			}
			if (base.Mission.CurrentState == Mission.State.Continuing && base.Mission.GetMissionEndTimerValue() < 0f && ((GameNetwork.IsMyPeerReady && GameNetwork.MyPeer.IsAdmin) || GameNetwork.IsServer))
			{
				EscapeMenuItemVM escapeMenuItemVM = new EscapeMenuItemVM(new TextObject("{=xILeUbY3}Admin Panel", null), delegate(object o)
				{
					base.OnEscapeMenuToggled(false);
					if (this._missionAdminComponent != null)
					{
						this._missionAdminComponent.ChangeAdminMenuActiveState(true);
					}
				}, null, () => new Tuple<bool, TextObject>(false, null), false);
				list.Add(escapeMenuItemVM);
			}
			list.Add(new EscapeMenuItemVM(new TextObject("{=InGwtrWt}Quit", null), delegate(object o)
			{
				InformationManager.ShowInquiry(new InquiryData(new TextObject("{=InGwtrWt}Quit", null).ToString(), new TextObject("{=lxq6SaQn}Are you sure want to quit?", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					LobbyClient gameClient = NetworkMain.GameClient;
					CommunityClient communityClient = NetworkMain.CommunityClient;
					if (communityClient.IsInGame)
					{
						communityClient.QuitFromGame();
						return;
					}
					if (gameClient.CurrentState == LobbyClient.State.InCustomGame)
					{
						gameClient.QuitFromCustomGame();
						return;
					}
					if (gameClient.CurrentState == LobbyClient.State.HostingCustomGame)
					{
						gameClient.EndCustomGame();
						return;
					}
					gameClient.QuitFromMatchmakerGame();
				}, null, "", 0f, null, null, null), false, false);
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			return list;
		}

		// Token: 0x0400005A RID: 90
		private MissionOptionsComponent _missionOptionsComponent;

		// Token: 0x0400005B RID: 91
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x0400005C RID: 92
		private MultiplayerAdminComponent _missionAdminComponent;

		// Token: 0x0400005D RID: 93
		private MultiplayerTeamSelectComponent _missionTeamSelectComponent;

		// Token: 0x0400005E RID: 94
		private MissionMultiplayerGameModeBaseClient _gameModeClient;

		// Token: 0x0400005F RID: 95
		private readonly string _gameType;

		// Token: 0x04000060 RID: 96
		private EscapeMenuItemVM _changeTroopItem;

		// Token: 0x04000061 RID: 97
		private EscapeMenuItemVM _changeCultureItem;
	}
}
