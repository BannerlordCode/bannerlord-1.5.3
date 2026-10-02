using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A8 RID: 680
	public class VoiceGroupPersonaSoftspokenUpperTag : ConversationTag
	{
		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060024C9 RID: 9417 RVA: 0x0009F4A6 File Offset: 0x0009D6A6
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenUpperTag";
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0009F4AD File Offset: 0x0009D6AD
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.UsesHighRegister(character);
		}

		// Token: 0x04000AF7 RID: 2807
		public const string Id = "VoiceGroupPersonaSoftspokenUpperTag";
	}
}
