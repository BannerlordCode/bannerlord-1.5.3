using System;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200020C RID: 524
	public abstract class BattleWreckageModel : MBGameModel<BattleWreckageModel>
	{
		// Token: 0x06002063 RID: 8291
		public abstract bool CanPlayerInteractWithWreckage(out TextObject explanation);

		// Token: 0x06002064 RID: 8292
		public abstract int GetMaxWreckageCountForMapEventType(MapEvent mapEvent);

		// Token: 0x06002065 RID: 8293
		public abstract BattleWreckage.WreckageType GetWreckageTypeForMapEvent(MapEvent mapEvent);

		// Token: 0x06002066 RID: 8294
		public abstract int GetWreckageCreationBattleSizeThreshold(MapEvent mapEvent);
	}
}
