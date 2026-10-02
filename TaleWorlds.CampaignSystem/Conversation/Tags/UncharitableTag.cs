using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026C RID: 620
	public class UncharitableTag : ConversationTag
	{
		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x0009E87C File Offset: 0x0009CA7C
		public override string StringId
		{
			get
			{
				return "UncharitableTag";
			}
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0009E883 File Offset: 0x0009CA83
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Generosity) + character.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000ABA RID: 2746
		public const string Id = "UncharitableTag";
	}
}
