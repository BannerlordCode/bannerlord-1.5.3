using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C5 RID: 453
	public abstract class ValuationModel : MBGameModel<ValuationModel>
	{
		// Token: 0x06001E71 RID: 7793
		public abstract float GetValueOfTroop(CharacterObject troop);

		// Token: 0x06001E72 RID: 7794
		public abstract float GetMilitaryValueOfParty(MobileParty party);

		// Token: 0x06001E73 RID: 7795
		public abstract float GetValueOfHero(Hero hero);
	}
}
