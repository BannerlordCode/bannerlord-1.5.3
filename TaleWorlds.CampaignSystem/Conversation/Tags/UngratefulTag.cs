using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200028F RID: 655
	public class UngratefulTag : ConversationTag
	{
		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x0009F019 File Offset: 0x0009D219
		public override string StringId
		{
			get
			{
				return "UngratefulTag";
			}
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x0009F020 File Offset: 0x0009D220
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Generosity) < 0;
		}

		// Token: 0x04000ADE RID: 2782
		public const string Id = "UngratefulTag";
	}
}
