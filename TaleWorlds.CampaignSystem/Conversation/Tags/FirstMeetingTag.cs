using System;

namespace TaleWorlds.CampaignSystem.Conversation.Tags
{
	// Token: 0x0200027C RID: 636
	public class FirstMeetingTag : ConversationTag
	{
		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x0009ECE2 File Offset: 0x0009CEE2
		public override string StringId
		{
			get
			{
				return "FirstMeetingTag";
			}
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0009ECE9 File Offset: 0x0009CEE9
		public override bool IsApplicableTo(CharacterObject character)
		{
			return Campaign.Current.ConversationManager.CurrentConversationIsFirst;
		}

		// Token: 0x04000ACB RID: 2763
		public const string Id = "FirstMeetingTag";
	}
}
