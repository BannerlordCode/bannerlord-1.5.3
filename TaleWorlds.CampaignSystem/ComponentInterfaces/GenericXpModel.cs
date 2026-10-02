using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001AA RID: 426
	public abstract class GenericXpModel : MBGameModel<GenericXpModel>
	{
		// Token: 0x06001D59 RID: 7513
		public abstract float GetXpMultiplier(Hero hero);
	}
}
