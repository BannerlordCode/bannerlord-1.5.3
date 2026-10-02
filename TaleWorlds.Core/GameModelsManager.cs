using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000071 RID: 113
	public abstract class GameModelsManager
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x0001A449 File Offset: 0x00018649
		protected GameModelsManager(IEnumerable<GameModel> inputComponents)
		{
			this._gameModels = inputComponents.ToMBList<GameModel>();
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001A460 File Offset: 0x00018660
		protected T GetGameModel<T>() where T : GameModel
		{
			for (int i = this._gameModels.Count - 1; i >= 0; i--)
			{
				T t;
				if ((t = this._gameModels[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001A4AF File Offset: 0x000186AF
		public MBReadOnlyList<GameModel> GetGameModels()
		{
			return this._gameModels;
		}

		// Token: 0x04000415 RID: 1045
		private readonly MBList<GameModel> _gameModels;
	}
}
