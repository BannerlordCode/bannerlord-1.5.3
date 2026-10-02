using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000294 RID: 660
	public class ValorTag : ConversationTag
	{
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x0009F0FF File Offset: 0x0009D2FF
		public override string StringId
		{
			get
			{
				return "ValorTag";
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x0009F106 File Offset: 0x0009D306
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Valor) > 0;
		}

		// Token: 0x04000AE3 RID: 2787
		public const string Id = "ValorTag";
	}
}
