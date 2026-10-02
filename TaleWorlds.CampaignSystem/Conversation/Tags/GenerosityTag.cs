using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028E RID: 654
	public class GenerosityTag : ConversationTag
	{
		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x0009EFEB File Offset: 0x0009D1EB
		public override string StringId
		{
			get
			{
				return "GenerosityTag";
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0009EFF2 File Offset: 0x0009D1F2
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) > 0;
		}

		// Token: 0x04000ADD RID: 2781
		public const string Id = "GenerosityTag";
	}
}
