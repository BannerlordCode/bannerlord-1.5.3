using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x0200001A RID: 26
	public static class DialogHelper
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x0000D16C File Offset: 0x0000B36C
		public static void SetDialogString(string stringVariable, string gameTextId)
		{
			MBTextManager.SetTextVariable(stringVariable, Campaign.Current.ConversationManager.FindMatchingTextOrNull(gameTextId, CharacterObject.OneToOneConversationCharacter), false);
		}
	}
}
