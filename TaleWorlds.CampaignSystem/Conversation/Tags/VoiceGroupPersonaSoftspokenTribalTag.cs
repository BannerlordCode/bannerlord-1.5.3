using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A7 RID: 679
	public class VoiceGroupPersonaSoftspokenTribalTag : ConversationTag
	{
		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x0009F480 File Offset: 0x0009D680
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaSoftspokenTribalTag";
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x0009F487 File Offset: 0x0009D687
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaSoftspoken && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x04000AF6 RID: 2806
		public const string Id = "VoiceGroupPersonaSoftspokenTribalTag";
	}
}
