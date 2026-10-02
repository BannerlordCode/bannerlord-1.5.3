using System;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000021 RID: 33
	public static class PersuasionHelper
	{
		// Token: 0x06000119 RID: 281 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		public static TextObject ShowSuccess(PersuasionOptionArgs optionArgs, bool showToPlayer = true)
		{
			return TextObject.GetEmpty();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		public static TextObject GetDefaultPersuasionOptionReaction(PersuasionOptionResult optionResult)
		{
			TextObject textObject;
			if (optionResult == PersuasionOptionResult.CriticalSuccess)
			{
				textObject = new TextObject("{=yNSqDwse}Well... I can't argue with that.", null);
			}
			else if (optionResult == PersuasionOptionResult.Failure || optionResult == PersuasionOptionResult.Miss)
			{
				textObject = new TextObject("{=mZmCmC6q}I don't think so.", null);
			}
			else if (optionResult == PersuasionOptionResult.CriticalFailure)
			{
				textObject = new TextObject("{=zqapPfSK}No.. No.", null);
			}
			else
			{
				textObject = ((MBRandom.RandomFloat > 0.5f) ? new TextObject("{=AmBEgOyq}I see...", null) : new TextObject("{=hq13B7Ok}Yes.. You might be correct.", null));
			}
			return textObject;
		}
	}
}
