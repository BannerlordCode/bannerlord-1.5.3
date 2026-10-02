using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DF RID: 479
	public abstract class HeirSelectionCalculationModel : MBGameModel<HeirSelectionCalculationModel>
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06001F32 RID: 7986
		public abstract int HighestSkillPoint { get; }

		// Token: 0x06001F33 RID: 7987
		public abstract int CalculateHeirSelectionPoint(Hero candidateHeir, Hero deadHero, ref Hero maxSkillHero);
	}
}
