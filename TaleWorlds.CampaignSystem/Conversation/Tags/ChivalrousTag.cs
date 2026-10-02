using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200026E RID: 622
	public class ChivalrousTag : ConversationTag
	{
		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0009E8D2 File Offset: 0x0009CAD2
		public override string StringId
		{
			get
			{
				return "ChivalrousTag";
			}
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x0009E8D9 File Offset: 0x0009CAD9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetTraitLevel(DefaultTraits.Honor) + character.GetTraitLevel(DefaultTraits.Valor) > 0;
		}

		// Token: 0x04000ABC RID: 2748
		public const string Id = "ChivalrousTag";
	}
}
