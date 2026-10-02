using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000087 RID: 135
	internal class DialogFlowLine
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x00053EDC File Offset: 0x000520DC
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x00053ED3 File Offset: 0x000520D3
		public List<KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>> Variations { get; private set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x00053EE4 File Offset: 0x000520E4
		public bool HasVariation
		{
			get
			{
				return this.Variations.Count > 0;
			}
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00053EF4 File Offset: 0x000520F4
		internal DialogFlowLine()
		{
			this.Variations = new List<KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>>();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00053F07 File Offset: 0x00052107
		public void AddVariation(TextObject text, List<GameTextManager.ChoiceTag> list)
		{
			this.Variations.Add(new KeyValuePair<TextObject, List<GameTextManager.ChoiceTag>>(text, list));
		}

		// Token: 0x04000559 RID: 1369
		internal TextObject Text;

		// Token: 0x0400055A RID: 1370
		internal string InputToken;

		// Token: 0x0400055B RID: 1371
		internal string OutputToken;

		// Token: 0x0400055C RID: 1372
		internal bool ByPlayer;

		// Token: 0x0400055D RID: 1373
		internal ConversationSentence.OnConditionDelegate ConditionDelegate;

		// Token: 0x0400055E RID: 1374
		internal ConversationSentence.OnClickableConditionDelegate ClickableConditionDelegate;

		// Token: 0x0400055F RID: 1375
		internal ConversationSentence.OnConsequenceDelegate ConsequenceDelegate;

		// Token: 0x04000560 RID: 1376
		internal ConversationSentence.OnMultipleConversationConsequenceDelegate SpeakerDelegate;

		// Token: 0x04000561 RID: 1377
		internal ConversationSentence.OnMultipleConversationConsequenceDelegate ListenerDelegate;

		// Token: 0x04000562 RID: 1378
		internal bool IsRepeatable;

		// Token: 0x04000563 RID: 1379
		internal bool IsSpecialOption;

		// Token: 0x04000564 RID: 1380
		internal bool IsUsedOnce;
	}
}
