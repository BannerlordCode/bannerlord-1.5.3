using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024D RID: 589
	public class NonCombatantTag : ConversationTag
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x0009E08C File Offset: 0x0009C28C
		public override string StringId
		{
			get
			{
				return "NonCombatantTag";
			}
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0009E093 File Offset: 0x0009C293
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.HeroObject.IsNoncombatant;
		}

		// Token: 0x04000A9B RID: 2715
		public const string Id = "NonCombatantTag";
	}
}
