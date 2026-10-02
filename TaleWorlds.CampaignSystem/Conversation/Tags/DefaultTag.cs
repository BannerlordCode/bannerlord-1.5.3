using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200024B RID: 587
	public class DefaultTag : ConversationTag
	{
		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x0009E054 File Offset: 0x0009C254
		public override string StringId
		{
			get
			{
				return "DefaultTag";
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0009E05B File Offset: 0x0009C25B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return true;
		}

		// Token: 0x04000A99 RID: 2713
		public const string Id = "DefaultTag";
	}
}
