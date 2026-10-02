using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026D RID: 621
	public class AmoralTag : ConversationTag
	{
		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x0009E8A7 File Offset: 0x0009CAA7
		public override string StringId
		{
			get
			{
				return "AmoralTag";
			}
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0009E8AE File Offset: 0x0009CAAE
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Honor) + character.GetTraitLevel(DefaultTraits.Mercy) < 0;
		}

		// Token: 0x04000ABB RID: 2747
		public const string Id = "AmoralTag";
	}
}
