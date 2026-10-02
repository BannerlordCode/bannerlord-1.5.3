using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000285 RID: 645
	public class NonviolentProfessionTag : ConversationTag
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x0009EE70 File Offset: 0x0009D070
		public override string StringId
		{
			get
			{
				return "NonviolentProfessionTag";
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0009EE77 File Offset: 0x0009D077
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && (character.Occupation == Occupation.Artisan || character.Occupation == Occupation.Merchant || character.Occupation == Occupation.Headman);
		}

		// Token: 0x04000AD4 RID: 2772
		public const string Id = "NonviolentProfessionTag";
	}
}
