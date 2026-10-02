using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001F6 RID: 502
	public abstract class TavernMercenaryTroopsModel : MBGameModel<TavernMercenaryTroopsModel>
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06001FC9 RID: 8137
		public abstract float RegularMercenariesSpawnChance { get; }
	}
}
