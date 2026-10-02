using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.View.Screens
{
	// Token: 0x02000004 RID: 4
	[GameStateScreen(typeof(LobbyPracticeState))]
	public class LobbyPracticeStateScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002060 File Offset: 0x00000260
		public LobbyPracticeStateScreen(LobbyPracticeState lobbyPracticeState)
		{
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002068 File Offset: 0x00000268
		void IGameStateListener.OnActivate()
		{
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000206A File Offset: 0x0000026A
		void IGameStateListener.OnDeactivate()
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000206C File Offset: 0x0000026C
		void IGameStateListener.OnInitialize()
		{
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000206E File Offset: 0x0000026E
		void IGameStateListener.OnFinalize()
		{
		}
	}
}
