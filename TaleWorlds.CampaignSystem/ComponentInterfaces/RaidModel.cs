using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001A3 RID: 419
	public abstract class RaidModel : MBGameModel<RaidModel>
	{
		// Token: 0x06001D1F RID: 7455
		public abstract MBReadOnlyList<ValueTuple<ItemObject, float>> GetCommonLootItemScores();

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001D20 RID: 7456
		public abstract int GoldRewardForEachLostHearth { get; }

		// Token: 0x06001D21 RID: 7457
		public abstract ExplainedNumber CalculateHitDamage(MapEventSide attackerSide, float settlementHitPoints);

		// Token: 0x06001D22 RID: 7458
		public abstract float GetRaidLootMultiplier(PartyBase receivingParty);
	}
}
