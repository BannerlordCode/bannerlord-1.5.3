using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001C0 RID: 448
	public abstract class RansomValueCalculationModel : MBGameModel<RansomValueCalculationModel>
	{
		// Token: 0x06001E37 RID: 7735
		public abstract int PrisonerRansomValue(CharacterObject prisoner, Hero sellerHero = null);
	}
}
