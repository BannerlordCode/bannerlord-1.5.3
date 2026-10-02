using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001E6 RID: 486
	public abstract class PlayerProgressionModel : MBGameModel<PlayerProgressionModel>
	{
		// Token: 0x06001F52 RID: 8018
		public abstract float GetPlayerProgress();
	}
}
