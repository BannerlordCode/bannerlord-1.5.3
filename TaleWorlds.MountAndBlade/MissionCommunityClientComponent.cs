using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002AD RID: 685
	public class MissionCommunityClientComponent : MissionLobbyComponent
	{
		// Token: 0x060025F2 RID: 9714 RVA: 0x00089978 File Offset: 0x00087B78
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._communityClient = NetworkMain.CommunityClient;
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0008998B File Offset: 0x00087B8B
		public void SetServerEndingBeforeClientLoaded(bool isServerEndingBeforeClientLoaded)
		{
			this._isServerEndedBeforeClientLoaded = isServerEndingBeforeClientLoaded;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x00089994 File Offset: 0x00087B94
		public override void QuitMission()
		{
			base.QuitMission();
			if (!this._isServerEndedBeforeClientLoaded && base.CurrentMultiplayerState != MissionLobbyComponent.MultiplayerGameState.Ending && this._communityClient.IsInGame)
			{
				this._communityClient.QuitFromGame();
			}
		}

		// Token: 0x04000E9B RID: 3739
		private CommunityClient _communityClient;

		// Token: 0x04000E9C RID: 3740
		private bool _isServerEndedBeforeClientLoaded;
	}
}
