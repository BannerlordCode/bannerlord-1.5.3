using System;
using TaleWorlds.CampaignSystem.CharacterDevelopment;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x020002A1 RID: 673
	public class VoiceGroupPersonaCurtTribalTag : ConversationTag
	{
		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x0009F39C File Offset: 0x0009D59C
		public override string StringId
		{
			get
			{
				return "VoiceGroupPersonaCurtTribalTag";
			}
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0009F3A3 File Offset: 0x0009D5A3
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.GetPersona() == DefaultTraits.PersonaCurt && ConversationTagHelper.TribalVoiceGroup(character);
		}

		// Token: 0x04000AF0 RID: 2800
		public const string Id = "VoiceGroupPersonaCurtTribalTag";
	}
}
