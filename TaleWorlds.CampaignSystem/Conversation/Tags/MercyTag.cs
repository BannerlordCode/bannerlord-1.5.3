using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028C RID: 652
	public class MercyTag : ConversationTag
	{
		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002475 RID: 9333 RVA: 0x0009EF8F File Offset: 0x0009D18F
		public override string StringId
		{
			get
			{
				return "MercyTag";
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0009EF96 File Offset: 0x0009D196
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Mercy) > 0;
		}

		// Token: 0x04000ADB RID: 2779
		public const string Id = "MercyTag";
	}
}
