using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BE RID: 446
	public abstract class NotableSpawnModel : MBGameModel<NotableSpawnModel>
	{
		// Token: 0x06001E21 RID: 7713
		public abstract int GetTargetNotableCountForSettlement(Settlement settlement, Occupation occupation);
	}
}
