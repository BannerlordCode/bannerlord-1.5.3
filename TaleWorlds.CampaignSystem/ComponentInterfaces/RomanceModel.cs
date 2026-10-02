using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001BA RID: 442
	public abstract class RomanceModel : MBGameModel<RomanceModel>
	{
		// Token: 0x06001DF8 RID: 7672
		public abstract int GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest);
	}
}
