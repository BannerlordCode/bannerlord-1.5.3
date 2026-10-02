using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027D RID: 637
	public class PreacherNotableTypeTag : ConversationTag
	{
		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x0009ED02 File Offset: 0x0009CF02
		public override string StringId
		{
			get
			{
				return "PreacherNotableTypeTag";
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0009ED09 File Offset: 0x0009CF09
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsHero && character.Occupation == Occupation.Preacher;
		}

		// Token: 0x04000ACC RID: 2764
		public const string Id = "PreacherNotableTypeTag";
	}
}
