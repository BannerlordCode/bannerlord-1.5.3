using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E3 RID: 483
	public abstract class WallHitPointCalculationModel : MBGameModel<WallHitPointCalculationModel>
	{
		// Token: 0x06001F42 RID: 8002
		public abstract float CalculateMaximumWallHitPoint(Town town);
	}
}
