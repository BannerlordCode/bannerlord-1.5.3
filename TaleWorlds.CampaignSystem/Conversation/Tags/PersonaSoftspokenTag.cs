using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029D RID: 669
	public class PersonaSoftspokenTag : ConversationTag
	{
		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x060024A8 RID: 9384 RVA: 0x0009F302 File Offset: 0x0009D502
		public override string StringId
		{
			get
			{
				return "PersonaSoftspokenTag";
			}
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x0009F309 File Offset: 0x0009D509
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaSoftspoken;
		}

		// Token: 0x04000AEC RID: 2796
		public const string Id = "PersonaSoftspokenTag";
	}
}
