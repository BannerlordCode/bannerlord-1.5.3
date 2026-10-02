using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A2 RID: 674
	public class VoiceGroupPersonaCurtUpperTag : ConversationTag
	{
		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x060024B7 RID: 9399 RVA: 0x0009F3C2 File Offset: 0x0009D5C2
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaCurtUpperTag";
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0009F3C9 File Offset: 0x0009D5C9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaCurt && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AF1 RID: 2801
		public const string Id = "VoiceGroupPersonaCurtUpperTag";
	}
}
