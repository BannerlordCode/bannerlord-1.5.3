using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029C RID: 668
	public class PersonaIronicTag : ConversationTag
	{
		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x0009F2DA File Offset: 0x0009D4DA
		public override string StringId
		{
			get
			{
				return "PersonaIronicTag";
			}
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x0009F2E1 File Offset: 0x0009D4E1
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaIronic;
		}

		// Token: 0x04000AEB RID: 2795
		public const string Id = "PersonaIronicTag";
	}
}
