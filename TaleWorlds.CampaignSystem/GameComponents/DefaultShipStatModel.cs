using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Naval;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200015D RID: 349
	public class DefaultShipStatModel : ShipStatModel
	{
		// Token: 0x06001B0C RID: 6924 RVA: 0x00089640 File Offset: 0x00087840
		public override float GetShipFlagshipScore(Ship ship)
		{
			return 0f;
		}
	}
}
