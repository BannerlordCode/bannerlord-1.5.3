using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000088 RID: 136
	public interface IGameStateManagerListener
	{
		// Token: 0x060008A5 RID: 2213
		void OnCreateState(GameState gameState);

		// Token: 0x060008A6 RID: 2214
		void OnPushState(GameState gameState, bool isTopGameState);

		// Token: 0x060008A7 RID: 2215
		void OnPopState(GameState gameState);

		// Token: 0x060008A8 RID: 2216
		void OnCleanStates();

		// Token: 0x060008A9 RID: 2217
		void OnSavedGameLoadFinished();
	}
}
