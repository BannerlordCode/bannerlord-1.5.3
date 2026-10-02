using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024E RID: 590
	public class CombatantTag : ConversationTag
	{
		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x0009E0B2 File Offset: 0x0009C2B2
		public override string StringId
		{
			get
			{
				return "CombatantTag";
			}
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0009E0B9 File Offset: 0x0009C2B9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !character.IsHero || !character.HeroObject.IsNoncombatant;
		}

		// Token: 0x04000A9C RID: 2716
		public const string Id = "CombatantTag";
	}
}
