using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A5 RID: 677
	public class VoiceGroupPersonaIronicUpperTag : ConversationTag
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x060024C0 RID: 9408 RVA: 0x0009F434 File Offset: 0x0009D634
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicUpperTag";
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0009F43B File Offset: 0x0009D63B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AF4 RID: 2804
		public const string Id = "VoiceGroupPersonaIronicUpperTag";
	}
}
