using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000291 RID: 657
	public class DeviousTag : ConversationTag
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x0009F075 File Offset: 0x0009D275
		public override string StringId
		{
			get
			{
				return "DeviousTag";
			}
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x0009F07C File Offset: 0x0009D27C
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.GetTraitLevel(DefaultTraits.Honor) < 0;
		}

		// Token: 0x04000AE0 RID: 2784
		public const string Id = "DeviousTag";
	}
}
