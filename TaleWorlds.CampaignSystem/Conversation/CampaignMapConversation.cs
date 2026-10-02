using System;

namespace TaleWorlds.CampaignSystem.Conversation
{
	// Token: 0x0200023E RID: 574
	public static class CampaignMapConversation
	{
		// Token: 0x060022F6 RID: 8950 RVA: 0x0009AB4E File Offset: 0x00098D4E
		public static void OpenConversation(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData)
		{
			Campaign.Current.ConversationManager.OpenMapConversation(playerCharacterData, conversationPartnerData);
		}
	}
}
