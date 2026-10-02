using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000273 RID: 627
	public class MetBeforeTag : ConversationTag
	{
		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x0009EA74 File Offset: 0x0009CC74
		public override string StringId
		{
			get
			{
				return "MetBeforeTag";
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0009EA7B File Offset: 0x0009CC7B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return !Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000AC1 RID: 2753
		public const string Id = "MetBeforeTag";
	}
}
