using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200006E RID: 110
	public interface IGameStarter
	{
		// Token: 0x060007D8 RID: 2008
		void AddModel(GameModel gameModel);

		// Token: 0x060007D9 RID: 2009
		void AddModel<T>(MBGameModel<T> gameModel) where T : GameModel;

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060007DA RID: 2010
		IEnumerable<GameModel> Models { get; }
	}
}
