using System;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AE RID: 686
	public class MissionCustomGameClientComponent : MissionLobbyComponent
	{
		// Token: 0x060025F6 RID: 9718 RVA: 0x000899CD File Offset: 0x00087BCD
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._lobbyClient = NetworkMain.GameClient;
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000899E0 File Offset: 0x00087BE0
		public void SetServerEndingBeforeClientLoaded(bool isServerEndingBeforeClientLoaded)
		{
			this._isServerEndedBeforeClientLoaded = isServerEndingBeforeClientLoaded;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000899EC File Offset: 0x00087BEC
		public override void QuitMission()
		{
			base.QuitMission();
			if (GameNetwork.IsServer)
			{
				if (base.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && this._lobbyClient.LoggedIn && this._lobbyClient.CurrentState == LobbyClient.State.HostingCustomGame)
				{
					this._lobbyClient.EndCustomGame();
					return;
				}
			}
			else if (!this._isServerEndedBeforeClientLoaded && base.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && this._lobbyClient.LoggedIn && this._lobbyClient.CurrentState == LobbyClient.State.InCustomGame)
			{
				this._lobbyClient.QuitFromCustomGame();
			}
		}

		// Token: 0x04000E9D RID: 3741
		private LobbyClient _lobbyClient;

		// Token: 0x04000E9E RID: 3742
		private bool _isServerEndedBeforeClientLoaded;
	}
}
