using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000299 RID: 665
	public class RogueSkillsTag : ConversationTag
	{
		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x0009F25C File Offset: 0x0009D45C
		public override string StringId
		{
			get
			{
				return "RogueSkillsTag";
			}
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0009F263 File Offset: 0x0009D463
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.RogueSkills) > 0;
		}

		// Token: 0x04000AE8 RID: 2792
		public const string Id = "RogueSkillsTag";
	}
}
