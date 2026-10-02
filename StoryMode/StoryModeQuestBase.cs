using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace StoryMode
{
	// Token: 0x02000015 RID: 21
	public abstract class StoryModeQuestBase : QuestBase
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005191 File Offset: 0x00003391
		public override string SpecialQuestType
		{
			get
			{
				return "MainStoryline";
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00005198 File Offset: 0x00003398
		public override bool IsRemainingTimeHidden
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000519B File Offset: 0x0000339B
		protected StoryModeQuestBase(string questId, Hero questGiver, CampaignTime duration)
			: base(questId, questGiver, duration, 0)
		{
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000051A8 File Offset: 0x000033A8
		protected override void OnTimedOut()
		{
			base.OnTimedOut();
			TextObject textObject = new TextObject("{=JTPmw3cb}You couldn't complete the quest in time.", null);
			base.AddLog(textObject, false);
		}
	}
}
