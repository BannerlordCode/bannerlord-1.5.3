using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029B RID: 667
	public class PersonaCurtTag : ConversationTag
	{
		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x0009F2B2 File Offset: 0x0009D4B2
		public override string StringId
		{
			get
			{
				return "PersonaCurtTag";
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0009F2B9 File Offset: 0x0009D4B9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.GetPersona() == DefaultTraits.PersonaCurt;
		}

		// Token: 0x04000AEA RID: 2794
		public const string Id = "PersonaCurtTag";
	}
}
