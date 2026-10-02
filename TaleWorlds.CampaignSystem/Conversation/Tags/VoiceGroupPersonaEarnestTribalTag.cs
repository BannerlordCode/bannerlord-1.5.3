using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200029E RID: 670
	public class VoiceGroupPersonaEarnestTribalTag : ConversationTag
	{
		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x0009F32A File Offset: 0x0009D52A
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestTribalTag";
			}
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0009F331 File Offset: 0x0009D531
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x04000AED RID: 2797
		public const string Id = "VoiceGroupPersonaEarnestTribalTag";
	}
}
