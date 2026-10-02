using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000198 RID: 408
	public abstract class CampaignShipDamageModel : MBGameModel<CampaignShipDamageModel>
	{
		// Token: 0x06001CD9 RID: 7385
		public abstract int GetHourlyShipDamage(MobileParty owner, Ship ship);

		// Token: 0x06001CDA RID: 7386
		public abstract float GetEstimatedSafeSailDuration(MobileParty mobileParty);

		// Token: 0x06001CDB RID: 7387
		public abstract float GetShipDamage(Ship ship, Ship rammingShip, float rawDamage);
	}
}
