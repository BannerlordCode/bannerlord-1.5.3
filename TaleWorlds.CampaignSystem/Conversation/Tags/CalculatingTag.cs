using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000292 RID: 658
	public class CalculatingTag : ConversationTag
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x0009F0A3 File Offset: 0x0009D2A3
		public override string StringId
		{
			get
			{
				return "CalculatingTag";
			}
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x0009F0AA File Offset: 0x0009D2AA
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Calculating) > 0;
		}

		// Token: 0x04000AE1 RID: 2785
		public const string Id = "CalculatingTag";
	}
}
