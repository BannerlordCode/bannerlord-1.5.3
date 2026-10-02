using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200001C RID: 28
	public class BasicGameModels : GameModelsManager
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00006A10 File Offset: 0x00004C10
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00006A18 File Offset: 0x00004C18
		public RidingModel RidingModel { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00006A21 File Offset: 0x00004C21
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00006A29 File Offset: 0x00004C29
		public ItemCategorySelector ItemCategorySelector { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00006A32 File Offset: 0x00004C32
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00006A3A File Offset: 0x00004C3A
		public ItemValueModel ItemValueModel { get; private set; }

		// Token: 0x0600018C RID: 396 RVA: 0x00006A43 File Offset: 0x00004C43
		public BasicGameModels(IEnumerable<GameModel> inputComponents)
			: base(inputComponents)
		{
			this.RidingModel = base.GetGameModel<RidingModel>();
			this.ItemCategorySelector = base.GetGameModel<ItemCategorySelector>();
			this.ItemValueModel = base.GetGameModel<ItemValueModel>();
		}
	}
}
