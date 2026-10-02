using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A3 RID: 675
	public class VoiceGroupPersonaCurtLowerTag : ConversationTag
	{
		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x0009F3E8 File Offset: 0x0009D5E8
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaCurtLowerTag";
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x0009F3EF File Offset: 0x0009D5EF
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaCurt && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AF2 RID: 2802
		public const string Id = "VoiceGroupPersonaCurtLowerTag";
	}
}
