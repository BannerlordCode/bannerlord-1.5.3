using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI
{
	// Token: 0x02000009 RID: 9
	public class MultiplayerSceneNotificationContextProvider : ISceneNotificationContextProvider
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00004552 File Offset: 0x00002752
		public bool IsContextAllowed(SceneNotificationData.RelevantContextType relevantType)
		{
			return relevantType != SceneNotificationData.RelevantContextType.MPLobby || GameStateManager.Current.ActiveState is LobbyState;
		}
	}
}
