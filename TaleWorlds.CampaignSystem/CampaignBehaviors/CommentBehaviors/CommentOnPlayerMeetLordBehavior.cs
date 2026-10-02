using System;
using TaleWorlds.CampaignSystem.LogEntries;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.CommentBehaviors
{
	// Token: 0x02000486 RID: 1158
	public class CommentOnPlayerMeetLordBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004AF2 RID: 19186 RVA: 0x00179E95 File Offset: 0x00178095
		public override void RegisterEvents()
		{
			CampaignEvents.OnPlayerMetHeroEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnPlayerMetCharacter));
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x00179EAE File Offset: 0x001780AE
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x00179EB0 File Offset: 0x001780B0
		private void OnPlayerMetCharacter(Hero hero)
		{
			if (hero.Mother != Hero.MainHero && hero.Father != Hero.MainHero)
			{
				LogEntry.AddLogEntry(new PlayerMeetLordLogEntry(hero));
			}
		}
	}
}
