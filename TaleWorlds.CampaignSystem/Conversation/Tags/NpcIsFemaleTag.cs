using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000264 RID: 612
	public class NpcIsFemaleTag : ConversationTag
	{
		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x0009E65A File Offset: 0x0009C85A
		public override string StringId
		{
			get
			{
				return "NpcIsFemaleTag";
			}
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x0009E661 File Offset: 0x0009C861
		public override bool IsApplicableTo(CharacterObject character)
		{
			return character.IsFemale;
		}

		// Token: 0x04000AB2 RID: 2738
		public const string Id = "NpcIsFemaleTag";
	}
}
