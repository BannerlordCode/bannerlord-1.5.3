using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000295 RID: 661
	public class CautiousTag : ConversationTag
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x0009F12D File Offset: 0x0009D32D
		public override string StringId
		{
			get
			{
				return "CautiousTag";
			}
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x0009F134 File Offset: 0x0009D334
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Valor) < 0;
		}

		// Token: 0x04000AE4 RID: 2788
		public const string Id = "CautiousTag";
	}
}
