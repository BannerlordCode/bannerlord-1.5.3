using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029F RID: 671
	public class VoiceGroupPersonaEarnestUpperTag : ConversationTag
	{
		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x0009F350 File Offset: 0x0009D550
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestUpperTag";
			}
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x0009F357 File Offset: 0x0009D557
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AEE RID: 2798
		public const string Id = "VoiceGroupPersonaEarnestUpperTag";
	}
}
