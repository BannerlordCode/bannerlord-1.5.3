using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A9 RID: 681
	public class VoiceGroupPersonaSoftspokenLowerTag : ConversationTag
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060024CC RID: 9420 RVA: 0x0009F4CC File Offset: 0x0009D6CC
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenLowerTag";
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x0009F4D3 File Offset: 0x0009D6D3
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.UsesLowRegister(character);
		}

		// Token: 0x04000AF8 RID: 2808
		public const string Id = "VoiceGroupPersonaSoftspokenLowerTag";
	}
}
