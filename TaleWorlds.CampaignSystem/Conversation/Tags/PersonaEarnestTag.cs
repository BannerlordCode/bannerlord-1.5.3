using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029A RID: 666
	public class PersonaEarnestTag : ConversationTag
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x0009F28A File Offset: 0x0009D48A
		public override string StringId
		{
			get
			{
				return "PersonaEarnestTag";
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x0009F291 File Offset: 0x0009D491
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaEarnest;
		}

		// Token: 0x04000AE9 RID: 2793
		public const string Id = "PersonaEarnestTag";
	}
}
