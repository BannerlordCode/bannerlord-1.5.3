using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000104 RID: 260
	public class DefaultCampaignShipDamageModel : CampaignShipDamageModel
	{
		// Token: 0x06001738 RID: 5944 RVA: 0x0006C6B3 File Offset: 0x0006A8B3
		public override int GetHourlyShipDamage(MobileParty owner, Ship ship)
		{
			return 0;
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x0006C6B6 File Offset: 0x0006A8B6
		public override float GetEstimatedSafeSailDuration(MobileParty mobileParty)
		{
			return 0f;
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x0006C6BD File Offset: 0x0006A8BD
		public override float GetShipDamage(Ship ship, Ship rammingShip, float rawDamage)
		{
			return rawDamage;
		}
	}
}
