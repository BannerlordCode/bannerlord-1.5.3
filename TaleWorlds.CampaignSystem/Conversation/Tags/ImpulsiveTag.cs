using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000293 RID: 659
	public class ImpulsiveTag : ConversationTag
	{
		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x0009F0D1 File Offset: 0x0009D2D1
		public override string StringId
		{
			get
			{
				return "ImpulsiveTag";
			}
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x0009F0D8 File Offset: 0x0009D2D8
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) < 0;
		}

		// Token: 0x04000AE2 RID: 2786
		public const string Id = "ImpulsiveTag";
	}
}
