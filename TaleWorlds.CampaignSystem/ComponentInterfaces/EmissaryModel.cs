using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B3 RID: 435
	public abstract class EmissaryModel : MBGameModel<EmissaryModel>
	{
		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001DCC RID: 7628
		public abstract int EmissaryRelationBonusForMainClan { get; }

		// Token: 0x06001DCD RID: 7629
		public abstract bool IsEmissary(Hero hero);
	}
}
