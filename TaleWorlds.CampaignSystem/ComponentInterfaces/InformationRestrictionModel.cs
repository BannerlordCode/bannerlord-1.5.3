using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000196 RID: 406
	public abstract class InformationRestrictionModel : MBGameModel<InformationRestrictionModel>
	{
		// Token: 0x06001CD0 RID: 7376
		public abstract bool DoesPlayerKnowDetailsOf(Settlement settlement);

		// Token: 0x06001CD1 RID: 7377
		public abstract bool DoesPlayerKnowDetailsOf(Hero hero);
	}
}
