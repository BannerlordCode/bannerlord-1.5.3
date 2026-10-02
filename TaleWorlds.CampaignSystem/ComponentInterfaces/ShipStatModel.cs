using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200019A RID: 410
	public abstract class ShipStatModel : MBGameModel<ShipStatModel>
	{
		// Token: 0x06001CE2 RID: 7394
		public abstract float GetShipFlagshipScore(Ship ship);
	}
}
