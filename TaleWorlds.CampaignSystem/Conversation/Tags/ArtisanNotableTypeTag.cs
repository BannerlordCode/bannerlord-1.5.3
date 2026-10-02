using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000281 RID: 641
	public class ArtisanNotableTypeTag : ConversationTag
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06002454 RID: 9300 RVA: 0x0009ED96 File Offset: 0x0009CF96
		public override string StringId
		{
			get
			{
				return "ArtisanNotableTypeTag";
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0009ED9D File Offset: 0x0009CF9D
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Artisan;
		}

		// Token: 0x04000AD0 RID: 2768
		public const string Id = "ArtisanNotableTypeTag";
	}
}
