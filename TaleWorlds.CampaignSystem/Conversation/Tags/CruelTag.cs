using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028D RID: 653
	public class CruelTag : ConversationTag
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06002478 RID: 9336 RVA: 0x0009EFBD File Offset: 0x0009D1BD
		public override string StringId
		{
			get
			{
				return "CruelTag";
			}
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0009EFC4 File Offset: 0x0009D1C4
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000ADC RID: 2780
		public const string Id = "CruelTag";
	}
}
