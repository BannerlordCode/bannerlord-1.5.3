using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A6 RID: 678
	public class VoiceGroupPersonaIronicLowerTag : ConversationTag
	{
		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x0009F45A File Offset: 0x0009D65A
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicLowerTag";
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x0009F461 File Offset: 0x0009D661
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AF5 RID: 2805
		public const string Id = "VoiceGroupPersonaIronicLowerTag";
	}
}
