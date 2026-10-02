using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.Screens
{
	// Token: 0x02000003 RID: 3
	[GameStateScreen(typeof(LobbyGameStateMatchmakerClient))]
	[GameStateScreen(typeof(LobbyGameStatePlayerBasedCustomServer))]
	public class LobbyGameStateScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002050 File Offset: 0x00000250
		public LobbyGameStateScreen(LobbyGameState lobbyGameState)
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000205A File Offset: 0x0000025A
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000205C File Offset: 0x0000025C
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000205E File Offset: 0x0000025E
		void IGameStateListener.OnFinalize()
		{
		}
	}
}
