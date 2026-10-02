using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000290 RID: 656
	public class HonorTag : ConversationTag
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002481 RID: 9345 RVA: 0x0009F047 File Offset: 0x0009D247
		public override string StringId
		{
			get
			{
				return "HonorTag";
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0009F04E File Offset: 0x0009D24E
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) > 0;
		}

		// Token: 0x04000ADF RID: 2783
		public const string Id = "HonorTag";
	}
}
