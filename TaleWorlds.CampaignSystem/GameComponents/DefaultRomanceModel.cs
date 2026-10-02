using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014E RID: 334
	public class DefaultRomanceModel : RomanceModel
	{
		// Token: 0x06001A55 RID: 6741 RVA: 0x00084EA8 File Offset: 0x000830A8
		public override int GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest)
		{
			return MathF.Abs((potentiallyInterestedCharacter.StaticBodyProperties.GetHashCode() + heroOfInterest.StaticBodyProperties.GetHashCode()) % 100);
		}
	}
}
