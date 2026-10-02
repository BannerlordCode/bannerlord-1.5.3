using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001D2 RID: 466
	public abstract class DisguiseDetectionModel : MBGameModel<DisguiseDetectionModel>
	{
		// Token: 0x06001EBE RID: 7870
		public abstract float CalculateDisguiseDetectionProbability(Settlement settlement);
	}
}
