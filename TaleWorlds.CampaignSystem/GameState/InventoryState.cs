using System;
using Helpers;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003AE RID: 942
	public class InventoryState : PlayerGameState
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060036E4 RID: 14052 RVA: 0x000DF3A9 File Offset: 0x000DD5A9
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060036E5 RID: 14053 RVA: 0x000DF3AC File Offset: 0x000DD5AC
		// (set) Token: 0x060036E6 RID: 14054 RVA: 0x000DF3B4 File Offset: 0x000DD5B4
		public InventoryLogic InventoryLogic { get; set; }

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060036E7 RID: 14055 RVA: 0x000DF3BD File Offset: 0x000DD5BD
		// (set) Token: 0x060036E8 RID: 14056 RVA: 0x000DF3C5 File Offset: 0x000DD5C5
		public InventoryScreenHelper.InventoryMode InventoryMode { get; set; }

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060036E9 RID: 14057 RVA: 0x000DF3CE File Offset: 0x000DD5CE
		// (set) Token: 0x060036EA RID: 14058 RVA: 0x000DF3D6 File Offset: 0x000DD5D6
		public Action DoneLogicExtrasDelegate { get; set; }

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060036EB RID: 14059 RVA: 0x000DF3DF File Offset: 0x000DD5DF
		// (set) Token: 0x060036EC RID: 14060 RVA: 0x000DF3E7 File Offset: 0x000DD5E7
		public IInventoryStateHandler Handler { get; set; }
	}
}
