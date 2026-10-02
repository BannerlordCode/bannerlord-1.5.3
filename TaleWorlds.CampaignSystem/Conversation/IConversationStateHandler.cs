using System;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x02000246 RID: 582
	public interface IConversationStateHandler
	{
		// Token: 0x0600237E RID: 9086
		void OnConversationInstall();

		// Token: 0x0600237F RID: 9087
		void OnConversationUninstall();

		// Token: 0x06002380 RID: 9088
		void OnConversationActivate();

		// Token: 0x06002381 RID: 9089
		void OnConversationDeactivate();

		// Token: 0x06002382 RID: 9090
		void OnConversationContinue();

		// Token: 0x06002383 RID: 9091
		void ExecuteConversationContinue();
	}
}
