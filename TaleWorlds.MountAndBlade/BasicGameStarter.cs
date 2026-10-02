using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001EA RID: 490
	public class BasicGameStarter : IGameStarter
	{
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001CDA RID: 7386 RVA: 0x0006243E File Offset: 0x0006063E
		IEnumerable<GameModel> IGameStarter.Models
		{
			get
			{
				return this._models;
			}
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x00062446 File Offset: 0x00060646
		public BasicGameStarter()
		{
			this._models = new List<GameModel>();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x0006245C File Offset: 0x0006065C
		public T GetModel<T>() where T : GameModel
		{
			for (int i = this._models.Count - 1; i >= 0; i--)
			{
				T t;
				if ((t = this._models[i] as T) != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000624AB File Offset: 0x000606AB
		public void AddModel(GameModel gameModel)
		{
			this._models.Add(gameModel);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000624BC File Offset: 0x000606BC
		public void AddModel<T>(MBGameModel<T> gameModel) where T : GameModel
		{
			T model = this.GetModel<T>();
			gameModel.Initialize(model);
			this._models.Add(gameModel);
		}

		// Token: 0x040009AA RID: 2474
		private List<GameModel> _models;
	}
}
