using System;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001EE RID: 494
	public abstract class SiegeStrategyActionModel : MBGameModel<SiegeStrategyActionModel>
	{
		// Token: 0x06001F8C RID: 8076
		public abstract void GetLogicalActionForStrategy(ISiegeEventSide side, out SiegeStrategyActionModel.SiegeAction siegeAction, out SiegeEngineType siegeEngineType, out int deploymentIndex, out int reserveIndex);

		// Token: 0x02000632 RID: 1586
		public enum SiegeAction
		{
			// Token: 0x04001A4B RID: 6731
			ConstructNewSiegeEngine,
			// Token: 0x04001A4C RID: 6732
			DeploySiegeEngineFromReserve,
			// Token: 0x04001A4D RID: 6733
			MoveSiegeEngineToReserve,
			// Token: 0x04001A4E RID: 6734
			RemoveDeployedSiegeEngine,
			// Token: 0x04001A4F RID: 6735
			Hold
		}
	}
}
