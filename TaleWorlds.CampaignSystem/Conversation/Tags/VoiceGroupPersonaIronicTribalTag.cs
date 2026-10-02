using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A4 RID: 676
	public class VoiceGroupPersonaIronicTribalTag : ConversationTag
	{
		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x060024BD RID: 9405 RVA: 0x0009F40E File Offset: 0x0009D60E
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaIronicTribalTag";
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0009F415 File Offset: 0x0009D615
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaIronic && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x04000AF3 RID: 2803
		public const string Id = "VoiceGroupPersonaIronicTribalTag";
	}
}
