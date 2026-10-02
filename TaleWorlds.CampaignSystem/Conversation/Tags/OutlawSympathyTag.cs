using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000251 RID: 593
	public class OutlawSympathyTag : ConversationTag
	{
		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x0009E1F7 File Offset: 0x0009C3F7
		public override string StringId
		{
			get
			{
				return "OutlawSympathyTag";
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x0009E1FE File Offset: 0x0009C3FE
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsWanderer && character.HeroObject.GetTraitLevel(DefaultTraits.RogueSkills) > 0;
		}

		// Token: 0x04000A9F RID: 2719
		public const string Id = "OutlawSympathyTag";
	}
}
