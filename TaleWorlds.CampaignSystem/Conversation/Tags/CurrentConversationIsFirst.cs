using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x02000272 RID: 626
	public class CurrentConversationIsFirst : ConversationTag
	{
		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x0009EA54 File Offset: 0x0009CC54
		public override string StringId
		{
			get
			{
				return "CurrentConversationIsFirst";
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0009EA5B File Offset: 0x0009CC5B
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000AC0 RID: 2752
		public const string Id = "CurrentConversationIsFirst";
	}
}
