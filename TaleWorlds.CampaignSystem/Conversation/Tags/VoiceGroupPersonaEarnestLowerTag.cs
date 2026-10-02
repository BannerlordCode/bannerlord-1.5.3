using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A0 RID: 672
	public class VoiceGroupPersonaEarnestLowerTag : ConversationTag
	{
		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x060024B1 RID: 9393 RVA: 0x0009F376 File Offset: 0x0009D576
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaEarnestLowerTag";
			}
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0009F37D File Offset: 0x0009D57D
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaEarnest && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AEF RID: 2799
		public const string Id = "VoiceGroupPersonaEarnestLowerTag";
	}
}
